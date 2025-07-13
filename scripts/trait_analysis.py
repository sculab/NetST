# !/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time       : 2025/6/15 23:38
# @Author     : zzhen
# @File       : trait_analysis_old.py
# @Software   : PyCharm
# @Copyright  : Copyright (c) 2023 by zzhen, All Rights Reserved.

"""
NetST Haplotype-Trait Association Analysis Script with Integrated Data Preprocessing
functionality:
1. Data preprocessing from meta and seq2hap files
2. Descriptive statistical analysis
3. Trait association analysis (ANOVA/Kruskal-Wallis)
4. Effect size quantification (eta-squared)
5. Haplotype enrichment analysis
6. Statistical power assessment
7. Comprehensive reporting

Usage:
python trait_analysis.py --meta meta_file.meta --seq2hap seq2hap_file.csv -o output_prefix
OR
python trait_analysis.py -i input_file.csv -o output_prefix

Input formats:
- Meta file: tab-separated with format "key=value\ttrait_description"
- Seq2hap file: CSV with columns including 'id', 'name', 'hap', 'trait'
- Direct CSV: columns 'id', 'name', 'hap', 'trait', 'trait2'
"""

import argparse
import csv
import os
import sys
import warnings
from datetime import datetime
from tempfile import NamedTemporaryFile

import numpy as np
import pandas as pd
from scipy import stats
from scipy.stats import kruskal, shapiro

warnings.filterwarnings('ignore')


class NetSTDataPreprocessor:
    """Data preprocessing component for NetST analysis"""

    def __init__(self, meta_file, seq2hap_file):
        self.meta_file = meta_file
        self.seq2hap_file = seq2hap_file
        self.processed_data = []

    def load_meta_data(self):
        """Load and parse meta data file"""
        try:
            meta_data = {}
            with open(self.meta_file, 'r', encoding='utf-8') as f:
                meta_lines = f.readlines()

            for line in meta_lines:
                line = line.strip()
                if not line or not '\t' in line:
                    continue

                parts = line.split('\t', 1)
                if len(parts) < 2:
                    continue

                key_value = parts[0].strip()
                trait_description = parts[1].strip()

                if '=' in key_value:
                    key = key_value.split('=')[0].strip()
                    value = key_value.split('=')[-1].strip()
                    meta_data[key] = (value, trait_description)

            print(f"Loaded meta data: {len(meta_data)} entries")
            return meta_data

        except Exception as e:
            print(f"Error loading meta file: {e}")
            sys.exit(1)

    def load_seq2hap_data(self):
        """Load seq2hap CSV data"""
        try:
            seq2hap_data = []
            with open(self.seq2hap_file, 'r', encoding='utf-8') as f:
                reader = csv.DictReader(f)
                for row in reader:
                    seq2hap_data.append(row)

            print(f"Loaded seq2hap data: {len(seq2hap_data)} entries")
            return seq2hap_data

        except Exception as e:
            print(f"Error loading seq2hap file: {e}")
            sys.exit(1)

    def merge_data(self):
        """Merge meta and seq2hap data"""
        print("Merging meta and seq2hap data...")

        meta_data = self.load_meta_data()
        seq2hap_data = self.load_seq2hap_data()

        merged_data = []
        matched_count = 0

        for row in seq2hap_data:
            sample_name = row.get("name", "").strip()

            if sample_name in meta_data:
                # Extract trait2 value from meta data
                trait2_value = meta_data[sample_name][0]

                # Try to convert to numeric
                try:
                    trait2_numeric = float(trait2_value)
                except ValueError:
                    # If conversion fails, skip this sample
                    print(
                        f"Warning: Could not convert trait2 value '{trait2_value}' for sample '{sample_name}' to numeric")
                    continue

                # Create merged row
                merged_row = {
                    "id": row.get("id", ""),
                    "name": sample_name,
                    "hap": row.get("hap", ""),
                    "trait": row.get("trait", ""),
                    "trait2": trait2_numeric
                }

                merged_data.append(merged_row)
                matched_count += 1

        print(f"Successfully merged {matched_count} samples")
        print(f"Samples with missing meta data: {len(seq2hap_data) - matched_count}")

        return merged_data

    def save_processed_data(self, output_file):
        """Save processed data to CSV file"""
        merged_data = self.merge_data()

        try:
            with open(output_file, 'w', newline='', encoding='utf-8') as f:
                if merged_data:
                    fieldnames = ["id", "name", "hap", "trait", "trait2"]
                    writer = csv.DictWriter(f, fieldnames=fieldnames)
                    writer.writeheader()
                    writer.writerows(merged_data)

            print(f"Processed data saved to: {output_file}")
            return output_file

        except Exception as e:
            print(f"Error saving processed data: {e}")
            sys.exit(1)


class NetSTAnalyzer:
    """NetST Haplotype-Trait Association Analyzer"""

    def __init__(self, data_file):
        """Initialize NetST analyzer"""
        self.data_file = data_file
        self.data = None
        self.results = {}
        self.significance_threshold = 0.05
        self.effect_size_threshold = 0.06  # NetST threshold for biological relevance
        self.load_data()

    def load_data(self):
        """Load and validate input data"""
        try:
            self.data = pd.read_csv(self.data_file)
            print(f"Data loaded successfully: {len(self.data)} samples")

            # Validate required columns
            required_cols = ['id', 'name', 'hap', 'trait', 'trait2']
            missing_cols = [col for col in required_cols if col not in self.data.columns]
            if missing_cols:
                raise ValueError(f"Missing required columns: {missing_cols}")

            # Data cleaning
            initial_count = len(self.data)
            self.data = self.data.dropna()
            self.data['trait2'] = pd.to_numeric(self.data['trait2'], errors='coerce')
            self.data = self.data.dropna()

            print(f"Data cleaning completed: {len(self.data)} valid samples")
            if len(self.data) < initial_count:
                print(f"  - Removed {initial_count - len(self.data)} samples with missing values")

        except Exception as e:
            print(f"✗ Data loading failed: {e}")
            sys.exit(1)

    def descriptive_statistics(self):
        """Compute descriptive statistics"""
        print("\n" + "=" * 60)
        print("Step 1: Descriptive Statistical Analysis")
        print("=" * 60)

        # Basic statistics
        basic_stats = {
            'total_samples': len(self.data),
            'unique_haplotypes': self.data['hap'].nunique(),
            'unique_discrete_traits': self.data['trait'].nunique(),
            'continuous_trait_mean': self.data['trait2'].mean(),
            'continuous_trait_std': self.data['trait2'].std(),
            'continuous_trait_min': self.data['trait2'].min(),
            'continuous_trait_max': self.data['trait2'].max(),
            'continuous_trait_median': self.data['trait2'].median()
        }

        # Discrete trait distribution
        discrete_dist = self.data['trait'].value_counts().to_dict()
        discrete_prop = (self.data['trait'].value_counts() / len(self.data) * 100).to_dict()

        # Haplotype frequency distribution
        hap_freq = self.data['hap'].value_counts()

        basic_stats.update({
            'discrete_trait_distribution': discrete_dist,
            'discrete_trait_proportions': discrete_prop,
            'haplotype_frequencies': hap_freq.to_dict(),
            'common_haplotypes': hap_freq[hap_freq >= 5].to_dict()  # Minimum 5 samples
        })

        self.results['descriptive_statistics'] = basic_stats

        # Display results
        print(f"Total samples: {basic_stats['total_samples']}")
        print(f"Unique haplotypes: {basic_stats['unique_haplotypes']}")
        print(f"Discrete trait categories: {basic_stats['unique_discrete_traits']}")
        print(f"Continuous trait statistics:")
        print(f"  Mean: {basic_stats['continuous_trait_mean']:.3f}")
        print(f"  Std: {basic_stats['continuous_trait_std']:.3f}")
        print(f"  Range: [{basic_stats['continuous_trait_min']:.3f}, {basic_stats['continuous_trait_max']:.3f}]")
        print(f"  Median: {basic_stats['continuous_trait_median']:.3f}")
        print(f"Common haplotypes (≥5 samples): {len(basic_stats['common_haplotypes'])}")

        return basic_stats

    def test_normality(self, data):
        """Test data normality using Shapiro-Wilk test"""
        if len(data) < 3:
            return False, np.nan

        if len(data) > 5000:  # Shapiro-Wilk limitation
            # Use a random sample for large datasets
            sample_data = np.random.choice(data, 5000, replace=False)
        else:
            sample_data = data

        try:
            stat, p_value = shapiro(sample_data)
            return p_value > 0.05, p_value
        except:
            return False, np.nan

    def trait_association_analysis(self):
        """NetST trait association analysis with ANOVA/Kruskal-Wallis"""
        print("\n" + "=" * 60)
        print("Step 2: Trait Association Analysis")
        print("=" * 60)

        # Group statistics
        group_stats = {}
        overall_mean = self.data['trait2'].mean()
        overall_std = self.data['trait2'].std()

        groups_data = []
        group_names = []

        for trait in self.data['trait'].unique():
            group_data = self.data[self.data['trait'] == trait]
            trait2_values = group_data['trait2'].values

            if len(trait2_values) < 3:  # Skip groups with insufficient samples
                continue

            groups_data.append(trait2_values)
            group_names.append(trait)

            # Calculate group statistics
            group_mean = trait2_values.mean()
            group_std = trait2_values.std()
            deviation = group_mean - overall_mean
            effect_size = abs(deviation) / overall_std if overall_std > 0 else 0

            group_stats[trait] = {
                'count': len(trait2_values),
                'mean': group_mean,
                'std': group_std,
                'deviation': deviation,
                'effect_size': effect_size,
                'min': trait2_values.min(),
                'max': trait2_values.max(),
                'median': np.median(trait2_values)
            }

        # Test normality for each group
        normality_results = {}
        overall_normal = True

        for i, (group_name, group_data) in enumerate(zip(group_names, groups_data)):
            is_normal, p_val = self.test_normality(group_data)
            normality_results[group_name] = {'normal': is_normal, 'p_value': p_val}
            if not is_normal:
                overall_normal = False

        # Choose appropriate statistical test
        if overall_normal and len(groups_data) > 1:
            # Use ANOVA for normal data
            try:
                f_statistic, p_value = stats.f_oneway(*groups_data)
                test_used = "ANOVA"
                test_statistic = f_statistic

                # Calculate eta-squared (effect size)
                ss_between = sum(len(group) * (np.mean(group) - overall_mean) ** 2 for group in groups_data)
                ss_total = sum((self.data['trait2'] - overall_mean) ** 2)
                eta_squared = ss_between / ss_total if ss_total > 0 else 0

            except Exception as e:
                print(f"ANOVA calculation warning: {e}")
                f_statistic, p_value, eta_squared = np.nan, np.nan, np.nan
                test_used = "ANOVA (failed)"
                test_statistic = f_statistic
        else:
            # Use Kruskal-Wallis for non-normal data
            try:
                h_statistic, p_value = kruskal(*groups_data)
                test_used = "Kruskal-Wallis"
                test_statistic = h_statistic

                # Calculate eta-squared equivalent for Kruskal-Wallis
                eta_squared = (h_statistic - len(groups_data) + 1) / (len(self.data) - len(groups_data))
                eta_squared = max(0, eta_squared)  # Ensure non-negative

            except Exception as e:
                print(f"Kruskal-Wallis calculation warning: {e}")
                h_statistic, p_value, eta_squared = np.nan, np.nan, np.nan
                test_used = "Kruskal-Wallis (failed)"
                test_statistic = h_statistic

        # Identify significant associations (NetST criteria)
        is_significant = (not np.isnan(p_value) and p_value < self.significance_threshold and
                          eta_squared > self.effect_size_threshold)

        # Identify key groups for enrichment analysis
        key_groups = []
        if is_significant:
            for trait, stats_dict in group_stats.items():
                if stats_dict['effect_size'] > 0.5:  # Large effect size
                    group_type = 'High-value group' if stats_dict['deviation'] > 0 else 'Low-value group'
                    key_groups.append({
                        'group': trait,
                        'effect_size': stats_dict['effect_size'],
                        'deviation': stats_dict['deviation'],
                        'count': stats_dict['count'],
                        'type': group_type
                    })

        key_groups.sort(key=lambda x: x['effect_size'], reverse=True)

        association_results = {
            'overall_mean': overall_mean,
            'overall_std': overall_std,
            'group_statistics': group_stats,
            'normality_results': normality_results,
            'test_used': test_used,
            'test_statistic': test_statistic,
            'p_value': p_value,
            'eta_squared': eta_squared,
            'is_significant': is_significant,
            'key_groups': key_groups
        }

        self.results['trait_association'] = association_results

        # Display results
        print(f"Statistical test: {test_used}")
        print(f"Test statistic: {test_statistic:.3f}" if not np.isnan(
            test_statistic) else "Test statistic: Unable to calculate")
        print(f"P-value: {p_value:.2e}" if not np.isnan(p_value) else "P-value: Unable to calculate")
        print(f"Effect size (eta-squared): {eta_squared:.4f} ({eta_squared * 100:.2f}%)")
        print(
            f"Significant association: {'Yes' if is_significant else 'No'} (p < {self.significance_threshold}, eta-squared > {self.effect_size_threshold})")

        if key_groups:
            print(f"\nKey groups identified for enrichment analysis:")
            for kg in key_groups:
                print(f"  {kg['group']}: Effect size = {kg['effect_size']:.3f}, "
                      f"Deviation = {kg['deviation']:+.3f}, {kg['type']}")
        else:
            print("\nNo key groups identified for enrichment analysis")

        return association_results

    def haplotype_enrichment_analysis(self):
        """NetST haplotype enrichment analysis"""
        print("\n" + "=" * 60)
        print("Step 3: Haplotype Enrichment Analysis")
        print("=" * 60)

        if not self.results['trait_association']['is_significant']:
            print("No significant trait associations found. Skipping enrichment analysis.")
            self.results['haplotype_enrichment'] = {
                'enriched_haplotypes': [],
                'significant_haplotypes': [],
                'summary': 'No significant associations detected'
            }
            return

        key_groups = self.results['trait_association']['key_groups']
        if not key_groups:
            print("No key groups identified. Performing general haplotype analysis.")
            key_group_names = list(self.data['trait'].unique())
        else:
            key_group_names = [kg['group'] for kg in key_groups]

        overall_mean = self.results['trait_association']['overall_mean']
        overall_std = self.results['trait_association']['overall_std']
        group_stats = self.results['trait_association']['group_statistics']

        enriched_haplotypes = []

        # Minimum sample size for statistical power
        min_samples = 3

        for hap in self.data['hap'].unique():
            hap_data = self.data[self.data['hap'] == hap]

            if len(hap_data) < min_samples:
                continue

            # Basic haplotype statistics
            sample_count = len(hap_data)
            trait2_values = hap_data['trait2'].values
            hap_mean = trait2_values.mean()
            hap_std = trait2_values.std()
            hap_deviation = hap_mean - overall_mean

            # Trait specificity analysis
            trait_dist = hap_data['trait'].value_counts().to_dict()
            total_hap_samples = len(hap_data)

            # Calculate concentration (trait specificity)
            max_trait_count = max(trait_dist.values()) if trait_dist else 0
            concentration = max_trait_count / total_hap_samples
            dominant_trait = max(trait_dist.items(), key=lambda x: x[1])[0] if trait_dist else None

            # Enrichment analysis for key groups
            key_group_samples = hap_data[hap_data['trait'].isin(key_group_names)]
            key_group_ratio = len(key_group_samples) / total_hap_samples

            # Calculate enrichment fold change
            max_enrichment = 0
            enriched_group = ""

            for group in key_group_names:
                if group in group_stats:
                    hap_group_count = len(hap_data[hap_data['trait'] == group])
                    hap_proportion = hap_group_count / total_hap_samples
                    overall_proportion = group_stats[group]['count'] / len(self.data)

                    if overall_proportion > 0:
                        enrichment = hap_proportion / overall_proportion
                        if enrichment > max_enrichment:
                            max_enrichment = enrichment
                            enriched_group = group

            # Statistical power assessment
            statistical_power = min(sample_count / 10, 1.0)  # Normalized to 1.0

            # Phenotypic deviation assessment
            phenotypic_deviation = abs(hap_deviation) / overall_std if overall_std > 0 else 0

            # NetST integrated scoring
            # Combines statistical power, trait specificity, and phenotypic deviation
            score = 0

            # Statistical power component (0-3 points)
            if statistical_power >= 0.8:
                power_score = 3
            elif statistical_power >= 0.6:
                power_score = 2
            elif statistical_power >= 0.4:
                power_score = 1
            else:
                power_score = 0
            score += power_score

            # Trait specificity component (0-4 points)
            if concentration >= 0.9:
                specificity_score = 4
            elif concentration >= 0.8:
                specificity_score = 3
            elif concentration >= 0.6:
                specificity_score = 2
            elif concentration >= 0.4:
                specificity_score = 1
            else:
                specificity_score = 0
            score += specificity_score

            # Phenotypic deviation component (0-3 points)
            if phenotypic_deviation >= 2.0:
                deviation_score = 3
            elif phenotypic_deviation >= 1.5:
                deviation_score = 2
            elif phenotypic_deviation >= 1.0:
                deviation_score = 1
            else:
                deviation_score = 0
            score += deviation_score

            # Enrichment bonus (0-2 points)
            if max_enrichment >= 5.0:
                enrichment_score = 2
            elif max_enrichment >= 3.0:
                enrichment_score = 1
            else:
                enrichment_score = 0
            score += enrichment_score

            enriched_haplotypes.append({
                'haplotype': hap,
                'sample_count': sample_count,
                'trait2_mean': hap_mean,
                'trait2_std': hap_std,
                'trait2_deviation': hap_deviation,
                'trait_distribution': trait_dist,
                'concentration': concentration,
                'dominant_trait': dominant_trait,
                'key_group_ratio': key_group_ratio,
                'max_enrichment': max_enrichment,
                'enriched_group': enriched_group,
                'statistical_power': statistical_power,
                'phenotypic_deviation': phenotypic_deviation,
                'total_score': score,
                'power_score': power_score,
                'specificity_score': specificity_score,
                'deviation_score': deviation_score,
                'enrichment_score': enrichment_score
            })

        # Sort by total score
        enriched_haplotypes.sort(key=lambda x: x['total_score'], reverse=True)

        # Identify potentially functional haplotypes (NetST criteria)
        functional_threshold = 6  # Minimum score for functional consideration
        significant_haplotypes = [h for h in enriched_haplotypes if h['total_score'] >= functional_threshold]

        self.results['haplotype_enrichment'] = {
            'enriched_haplotypes': enriched_haplotypes,
            'significant_haplotypes': significant_haplotypes,
            'functional_threshold': functional_threshold,
            'key_groups_analyzed': key_group_names
        }

        # Display results
        print(f"Analyzed {len(enriched_haplotypes)} haplotypes (≥{min_samples} samples)")
        print(f"Potentially functional haplotypes (≥{functional_threshold} points): {len(significant_haplotypes)}")
        print(f"Key groups analyzed: {', '.join(key_group_names)}")

        if enriched_haplotypes:
            print("\nTop 10 enriched haplotypes:")
            for i, hap in enumerate(enriched_haplotypes[:10]):
                print(f"  {i + 1:2d}. {hap['haplotype']:15s}: {hap['total_score']:4.1f} points "
                      f"(n={hap['sample_count']:3d}, specificity={hap['concentration']:.2f}, "
                      f"dominant={hap['dominant_trait']})")

        return self.results['haplotype_enrichment']

    def run_netst_analysis(self):
        """Run complete NetST analysis pipeline"""
        print("Starting NetST Haplotype-Trait Association Analysis...")
        print("=" * 60)
        print(f"Input file: {self.data_file}")
        print(f"Analysis timestamp: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")
        print(f"NetST parameters: p < {self.significance_threshold}, eta-squared > {self.effect_size_threshold}")

        # Execute analysis pipeline
        self.descriptive_statistics()
        self.trait_association_analysis()
        self.haplotype_enrichment_analysis()

        print("\n" + "=" * 60)
        print("NetST Analysis Completed Successfully!")
        print("=" * 60)

        return self.results

    def generate_reports(self, output_prefix="netst_analysis"):
        """Generate comprehensive NetST analysis reports"""
        print(f"\nGenerating NetST analysis reports...")

        # 1. Descriptive statistics report
        desc_stats = self.results['descriptive_statistics']
        desc_df = pd.DataFrame([desc_stats])
        desc_df.to_csv(f"{output_prefix}_descriptive_statistics.csv", index=False, encoding='utf-8-sig')

        # 2. Group statistics report
        group_stats = self.results['trait_association']['group_statistics']
        if group_stats:
            group_df = pd.DataFrame.from_dict(group_stats, orient='index')
            group_df.index.name = 'discrete_trait'
            group_df.to_csv(f"{output_prefix}_group_statistics.csv", encoding='utf-8-sig')

        # 3. Haplotype enrichment report
        enrichment_results = self.results['haplotype_enrichment']['enriched_haplotypes']
        if enrichment_results:
            enrichment_df = pd.DataFrame(enrichment_results)
            enrichment_df.to_csv(f"{output_prefix}_haplotype_enrichment.csv", index=False, encoding='utf-8-sig')

        # 4. Significant haplotypes report
        significant_haps = self.results['haplotype_enrichment']['significant_haplotypes']
        if significant_haps:
            significant_df = pd.DataFrame(significant_haps)
            significant_df.to_csv(f"{output_prefix}_significant_haplotypes.csv", index=False, encoding='utf-8-sig')

        # 5. Comprehensive summary report
        self.generate_summary_report(output_prefix)

        # Display generated files
        print(f"NetST analysis reports generated:")
        print(f"  - {output_prefix}_descriptive_statistics.csv")
        if group_stats:
            print(f"  - {output_prefix}_group_statistics.csv")
        if enrichment_results:
            print(f"  - {output_prefix}_haplotype_enrichment.csv")
        if significant_haps:
            print(f"  - {output_prefix}_significant_haplotypes.csv")
        print(f"  - {output_prefix}_summary_report.txt")

    def generate_summary_report(self, output_prefix):
        """Generate comprehensive text summary report"""
        with open(f"{output_prefix}_summary_report.txt", 'w', encoding='utf-8') as f:
            f.write("NetST Haplotype-Trait Association Analysis Report\n")
            f.write("=" * 60 + "\n\n")

            f.write(f"Analysis timestamp: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}\n")
            f.write(f"Input file: {self.data_file}\n")
            f.write(
                f"NetST parameters: p < {self.significance_threshold}, eta-squared > {self.effect_size_threshold}\n\n")

            # Descriptive statistics
            desc = self.results['descriptive_statistics']
            f.write("1. DESCRIPTIVE STATISTICS\n")
            f.write("-" * 40 + "\n")
            f.write(f"Total samples: {desc['total_samples']}\n")
            f.write(f"Unique haplotypes: {desc['unique_haplotypes']}\n")
            f.write(f"Discrete trait categories: {desc['unique_discrete_traits']}\n")
            f.write(f"Continuous trait statistics:\n")
            f.write(f"  Mean: {desc['continuous_trait_mean']:.3f}\n")
            f.write(f"  Std: {desc['continuous_trait_std']:.3f}\n")
            f.write(f"  Range: [{desc['continuous_trait_min']:.3f}, {desc['continuous_trait_max']:.3f}]\n")
            f.write(f"  Median: {desc['continuous_trait_median']:.3f}\n\n")

            f.write("Discrete trait distribution:\n")
            for trait, count in desc['discrete_trait_distribution'].items():
                prop = desc['discrete_trait_proportions'][trait]
                f.write(f"  {trait}: {count} samples ({prop:.1f}%)\n")
            f.write("\n")

            # Trait association analysis
            assoc = self.results['trait_association']
            f.write("2. TRAIT ASSOCIATION ANALYSIS\n")
            f.write("-" * 40 + "\n")
            f.write(f"Statistical test: {assoc['test_used']}\n")
            f.write(f"Test statistic: {assoc['test_statistic']:.3f}\n" if not np.isnan(
                assoc['test_statistic']) else "Test statistic: Unable to calculate\n")
            f.write(f"P-value: {assoc['p_value']:.2e}\n" if not np.isnan(
                assoc['p_value']) else "P-value: Unable to calculate\n")
            f.write(f"Effect size (eta-squared): {assoc['eta_squared']:.4f} ({assoc['eta_squared'] * 100:.2f}%)\n")
            f.write(f"Significant association: {'Yes' if assoc['is_significant'] else 'No'}\n\n")

            if assoc['key_groups']:
                f.write("Key groups identified:\n")
                for kg in assoc['key_groups']:
                    f.write(f"  {kg['group']}: Effect size = {kg['effect_size']:.3f}, "
                            f"Deviation = {kg['deviation']:+.3f}, {kg['type']}\n")
            else:
                f.write("No key groups identified\n")
            f.write("\n")

            # Haplotype enrichment analysis
            enrich = self.results['haplotype_enrichment']
            f.write("3. HAPLOTYPE ENRICHMENT ANALYSIS\n")
            f.write("-" * 40 + "\n")

            if enrich['enriched_haplotypes']:
                f.write(f"Analyzed haplotypes: {len(enrich['enriched_haplotypes'])}\n")
                f.write(
                    f"Potentially functional haplotypes (≥{enrich['functional_threshold']} points): {len(enrich['significant_haplotypes'])}\n")
                f.write(f"Key groups analyzed: {', '.join(enrich['key_groups_analyzed'])}\n\n")

                if enrich['significant_haplotypes']:
                    f.write("Potentially functional haplotypes:\n")
                    for i, hap in enumerate(enrich['significant_haplotypes']):
                        f.write(f"  {i + 1:2d}. {hap['haplotype']:15s}: {hap['total_score']:4.1f} points\n")
                        f.write(f"      Samples: {hap['sample_count']:3d}, Specificity: {hap['concentration']:.2f}\n")
                        f.write(f"      Dominant trait: {hap['dominant_trait']}\n")
                        f.write(f"      Phenotypic deviation: {hap['phenotypic_deviation']:.2f}\n")
                        if hap['max_enrichment'] > 1:
                            f.write(f"      Enrichment: {hap['max_enrichment']:.2f}x in {hap['enriched_group']}\n")
                        f.write("\n")
                else:
                    f.write("No potentially functional haplotypes identified\n\n")

                f.write("Top 10 enriched haplotypes:\n")
                for i, hap in enumerate(enrich['enriched_haplotypes'][:10]):
                    f.write(f"  {i + 1:2d}. {hap['haplotype']:15s}: {hap['total_score']:4.1f} points "
                            f"(n={hap['sample_count']:3d}, specificity={hap['concentration']:.2f})\n")
            else:
                f.write("No enrichment analysis performed (no significant associations)\n")

            f.write("\n\nAnalysis completed using NetST methodology.\n")
            f.write("For questions about methodology, please refer to NetST documentation.\n")


def main():
    """Main function with integrated data preprocessing"""
    parser = argparse.ArgumentParser(description='NetST Haplotype-Trait Association Analysis with Data Preprocessing')

    # Input options
    input_group = parser.add_mutually_exclusive_group(required=True)
    input_group.add_argument('-i', '--input', help='Direct input CSV file path (columns: id, name, hap, trait, trait2)')
    input_group.add_argument('--meta', help='Meta data file path (use with --seq2hap)')

    parser.add_argument('--seq2hap', help='Seq2hap CSV file path (use with --meta)')
    parser.add_argument('-o', '--output', default='netst_analysis',
                        help='Output file prefix (default: netst_analysis)')
    parser.add_argument('--significance', type=float, default=0.05,
                        help='Significance threshold (default: 0.05)')
    parser.add_argument('--effect-size', type=float, default=0.06,
                        help='Effect size threshold (default: 0.06)')
    parser.add_argument('--keep-processed', default=True, action='store_true',
                        help='Keep the processed CSV file after analysis')

    args = parser.parse_args()

    # Validate input arguments
    if args.meta and not args.seq2hap:
        print("Error: --seq2hap is required when using --meta")
        sys.exit(1)

    if args.seq2hap and not args.meta:
        print("Error: --meta is required when using --seq2hap")
        sys.exit(1)

    # Determine input file
    if args.input:
        # Direct CSV input
        if not os.path.exists(args.input):
            print(f"Error: Input file '{args.input}' does not exist")
            sys.exit(1)
        input_file = args.input
        temp_file_created = False

    else:
        # Meta + seq2hap input - requires preprocessing
        if not os.path.exists(args.meta):
            print(f"Error: Meta file '{args.meta}' does not exist")
            sys.exit(1)

        if not os.path.exists(args.seq2hap):
            print(f"Error: Seq2hap file '{args.seq2hap}' does not exist")
            sys.exit(1)

        print("=" * 60)
        print("Data Preprocessing Stage")
        print("=" * 60)

        # Create preprocessor and process data
        preprocessor = NetSTDataPreprocessor(args.meta, args.seq2hap)

        # Create temporary processed file
        if args.keep_processed:
            processed_file = f"{args.output}_merged_data.csv"
        else:
            # Use temporary file
            temp_file = NamedTemporaryFile(mode='w', suffix='.csv', delete=False)
            processed_file = temp_file.name
            temp_file.close()

        input_file = preprocessor.save_processed_data(processed_file)
        temp_file_created = not args.keep_processed

    try:
        print("\n" + "=" * 60)
        print("NetST Analysis Stage")
        print("=" * 60)

        # Create NetST analyzer and run analysis
        analyzer = NetSTAnalyzer(input_file)

        # Set custom thresholds if provided
        analyzer.significance_threshold = args.significance
        analyzer.effect_size_threshold = args.effect_size

        results = analyzer.run_netst_analysis()

        # Generate reports
        analyzer.generate_reports(args.output)

        print(f"\nNetST analysis completed successfully!")
        print(f"Results saved with prefix: {args.output}")

        if temp_file_created:
            print(f"Temporary processed file removed: {input_file}")
        elif args.meta and args.seq2hap and args.keep_processed:
            print(f"Processed data saved: {input_file}")

    except Exception as e:
        print(f"✗ Error during analysis: {e}")
        import traceback
        traceback.print_exc()
        sys.exit(1)

    finally:
        # Clean up temporary file if created
        if temp_file_created and os.path.exists(input_file):
            try:
                os.unlink(input_file)
            except:
                pass


if __name__ == "__main__":
    main()
