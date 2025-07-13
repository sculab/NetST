# !/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time       : 2025/5/16 05:04
# @Author     : zzhen
# @File       : seq_analysis.py
# @Software   : PyCharm
# @Description: 
# @Copyright  : Copyright (c) 2023 by zzhen, All Rights Reserved.
# !/usr/bin/env python3
# !/usr/bin/env python3
import argparse

import matplotlib.pyplot as plt
import numpy as np
import seaborn as sns
from Bio import SeqIO
from Bio.Align import MultipleSeqAlignment
from matplotlib.colors import LinearSegmentedColormap
from sklearn.decomposition import PCA

# Set matplotlib style
plt.style.use('ggplot')


def format_hap(hap_str):
    """Format haplotype labels for better readability"""
    try:
        num_part = hap_str.split('_')[1]  # Extract numeric part (e.g., Hap_xxx -> xxx)
        return f"H{num_part}"  # Convert to shorter label format (e.g., Hxxx)
    except IndexError:
        # If not in expected format, return original label
        return hap_str


def calculate_conservation(alignment):
    """
    Calculate conservation score for each position in the alignment
    Returns: conservation scores list and indices of variable sites
    """
    alignment_length = alignment.get_alignment_length()
    conservation_scores = []

    for i in range(alignment_length):
        # Get all bases at current position
        column = alignment[:, i]

        # Calculate base frequencies, ignoring gaps
        bases = [base for base in column if base != '-']
        if not bases:  # If all gaps, conservation is 0
            conservation_scores.append(0)
            continue

        # Calculate base frequencies
        base_freq = {}
        for base in bases:
            if base in base_freq:
                base_freq[base] += 1
            else:
                base_freq[base] = 1

        # Find most common base and its frequency
        most_common_base = max(base_freq, key=base_freq.get)
        conservation = base_freq[most_common_base] / len(bases)
        conservation_scores.append(conservation)

    # Find low conservation sites (below average)
    avg_conservation = np.mean(conservation_scores)
    variable_sites = [i for i, score in enumerate(conservation_scores) if score < avg_conservation]

    return conservation_scores, variable_sites


def plot_conservation(conservation_scores, output_file):
    """Plot sequence conservation and save as SVG file"""
    fig, ax = plt.subplots(figsize=(12, 5))
    ax.plot(range(1, len(conservation_scores) + 1), conservation_scores, 'b-')
    ax.fill_between(range(1, len(conservation_scores) + 1), conservation_scores, alpha=0.3)

    ax.set_xlabel('Sequence Position', fontsize=14)
    ax.set_ylabel('Conservation Score', fontsize=14)
    ax.set_title('Sequence Conservation Analysis', fontsize=16)
    ax.set_ylim(0, 1.05)
    ax.grid(True, linestyle='--', alpha=0.7)

    # Add threshold line and annotation
    mean_conservation = np.mean(conservation_scores)
    ax.axhline(y=mean_conservation, color='r', linestyle='--', alpha=0.8)
    ax.annotate(f'Mean Conservation: {mean_conservation:.3f}',
                xy=(len(conservation_scores) * 0.75, mean_conservation + 0.05),
                fontsize=10, color='red')

    plt.tight_layout()
    plt.savefig(output_file, format='svg')
    plt.close(fig)


def calculate_p_distance(seq1, seq2):
    """Calculate p-distance between two sequences"""
    diff_count = 0
    valid_count = 0

    for i in range(len(seq1)):
        # Skip positions with gaps
        if seq1[i] == '-' or seq2[i] == '-':
            continue

        valid_count += 1
        if seq1[i] != seq2[i]:
            diff_count += 1

    if valid_count == 0:
        return 0.0

    return diff_count / valid_count


def calculate_jukes_cantor(seq1, seq2):
    """Calculate Jukes-Cantor distance, accounting for multiple substitutions"""
    # First calculate p-distance
    p_dist = calculate_p_distance(seq1, seq2)

    # Apply Jukes-Cantor formula: -3/4 * ln(1 - 4/3 * p)
    if p_dist >= 0.75:  # Theoretical limit
        return 5.0  # Return a large value to indicate saturation

    return -0.75 * np.log(1 - (4.0 / 3.0) * p_dist)


def calculate_kimura(seq1, seq2):
    """Calculate Kimura 2-parameter distance, distinguishing transitions and transversions"""
    # Define purines and pyrimidines
    purines = set(['A', 'G'])
    pyrimidines = set(['C', 'T', 'U'])

    transitions = 0  # Purine-purine or pyrimidine-pyrimidine substitutions
    transversions = 0  # Purine-pyrimidine substitutions
    valid_sites = 0

    for i in range(len(seq1)):
        # Skip positions with gaps
        if seq1[i] == '-' or seq2[i] == '-':
            continue

        valid_sites += 1

        if seq1[i] != seq2[i]:
            # Check if transition or transversion
            if (seq1[i] in purines and seq2[i] in purines) or \
                    (seq1[i] in pyrimidines and seq2[i] in pyrimidines):
                transitions += 1
            else:
                transversions += 1

    if valid_sites == 0:
        return 0.0

    P = transitions / valid_sites  # Transition proportion
    Q = transversions / valid_sites  # Transversion proportion

    # K2P formula: -0.5 * ln[(1 - 2P - Q) * sqrt(1 - 2Q)]
    if 2 * P + Q >= 1 or 2 * Q >= 1:  # Check valid range
        return 5.0  # Return a large value to indicate saturation

    return -0.5 * np.log((1 - 2 * P - Q) * np.sqrt(1 - 2 * Q))


def calculate_genetic_distance(sequences, model):
    """Calculate genetic distance matrix using the specified model"""
    num_sequences = len(sequences)
    distance_matrix = np.zeros((num_sequences, num_sequences))

    print(f"Calculating genetic distances using {model} model...")

    # Select distance calculation function based on model
    if model == "p-distance":
        distance_func = calculate_p_distance
    elif model == "jukes-cantor":
        distance_func = calculate_jukes_cantor
    elif model == "kimura":
        distance_func = calculate_kimura
    else:
        raise ValueError(f"Unknown distance model: {model}")

    # Calculate distances between all sequence pairs
    for i in range(num_sequences):
        for j in range(i + 1, num_sequences):
            seq1 = str(sequences[i].seq)
            seq2 = str(sequences[j].seq)

            distance = distance_func(seq1, seq2)
            distance_matrix[i, j] = distance_matrix[j, i] = distance

    return distance_matrix


def plot_genetic_distance_heatmap(distance_matrix, labels, model_name, output_file):
    """Plot genetic distance heatmap and save as SVG file"""
    # Create custom gradient colormap
    colors = ["#313695", "#4575b4", "#74add1", "#abd9e9", "#e0f3f8",
              "#ffffbf", "#fee090", "#fdae61", "#f46d43", "#d73027", "#a50026"]
    cmap = LinearSegmentedColormap.from_list("custom_cmap", colors)

    # Create heatmap
    fig, ax = plt.subplots(figsize=(10, 8))

    # Use seaborn for better heatmap visualization
    sns.heatmap(distance_matrix, cmap=cmap, annot=False, ax=ax,
                cbar_kws={'label': 'Genetic Distance'})

    # Set chart title and labels
    ax.set_title(f'Haplotype Genetic Distance Heatmap ({model_name} model)', fontsize=16)
    ax.set_xlabel('Haplotype', fontsize=14)
    ax.set_ylabel('Haplotype', fontsize=14)

    # Adjust tick labels
    plt.xticks(rotation=45, ha='right', fontsize=9)
    plt.yticks(fontsize=9)

    plt.tight_layout()
    plt.savefig(output_file, format='svg')
    plt.close(fig)


def plot_pca(distance_matrix, labels, variable_sites_count, model_name, output_file):
    """Perform PCA analysis, plot results, and save as SVG file"""
    # Perform PCA
    pca = PCA(n_components=2)
    pca_result = pca.fit_transform(distance_matrix)

    # Calculate explained variance ratio
    explained_variance = pca.explained_variance_ratio_ * 100

    # Plot PCA scatter plot
    fig, ax = plt.subplots(figsize=(10, 8))

    # Use different colors and markers
    scatter = ax.scatter(pca_result[:, 0], pca_result[:, 1],
                         c=range(len(labels)), cmap='viridis',
                         alpha=0.8, s=100, edgecolors='w')

    # Add labels
    for i, label in enumerate(labels):
        ax.annotate(label, (pca_result[i, 0], pca_result[i, 1]),
                    fontsize=8, ha='left', va='bottom',
                    bbox=dict(boxstyle="round,pad=0.1", fc="white", ec="gray", alpha=0.8))

    # Add more information
    ax.set_title(f'PCA of Haplotypes ({model_name} model)', fontsize=16)
    ax.set_xlabel(f'PC1 ({explained_variance[0]:.2f}%)', fontsize=14)
    ax.set_ylabel(f'PC2 ({explained_variance[1]:.2f}%)', fontsize=14)

    # Add information text box
    info_text = f"Based on {variable_sites_count} polymorphic sites\n"
    info_text += f"First two PCs explain {sum(explained_variance):.2f}% of total variance"

    ax.text(0.02, 0.02, info_text, transform=ax.transAxes, fontsize=10,
            bbox=dict(boxstyle="round,pad=0.5", fc="white", ec="gray", alpha=0.8))

    # Add colorbar representing point order
    plt.colorbar(scatter, label='Haplotype Index', ax=ax, fraction=0.046, pad=0.04)

    # Add grid lines
    ax.grid(True, linestyle='--', alpha=0.7)

    plt.tight_layout()
    plt.savefig(output_file, format='svg')
    plt.close(fig)


def process_haplotypes(file_path, output_prefix="", models=None):
    """
    Process haplotype sequences, calculate genetic distances using selected models, and plot results.

    Parameters:
        file_path: Path to input haplotype FASTA file
        output_prefix: Output file prefix (optional)
        models: List of distance models to use (optional)
    """
    if models is None:
        models = ["p-distance", "jukes-cantor", "kimura"]

    print("Loading and processing sequences...")

    # Load and sort DNA sequences
    sequences = list(SeqIO.parse(file_path, "fasta"))
    sequences.sort(key=lambda x: x.id)

    # Create alignment object - use compatible method
    if hasattr(MultipleSeqAlignment, "from_list"):
        alignment = MultipleSeqAlignment.from_list(sequences)
    else:
        # Adapt for older Biopython
        alignment = MultipleSeqAlignment(sequences)

    # Calculate sequence conservation
    print("Analyzing sequence conservation...")
    conservation_scores, variable_sites = calculate_conservation(alignment)

    # Output basic information
    print(f"Loaded {len(sequences)} haplotype sequences")
    print(f"Sequence length: {alignment.get_alignment_length()} bp")
    print(f"Found {len(variable_sites)} variable sites")

    # Generate haplotype labels
    haplotype_labels = [format_hap(seq.id) for seq in sequences]

    # Plot and save conservation chart
    plot_conservation(conservation_scores, f"{output_prefix}_sequence_conservation.svg")
    print(f"  - {output_prefix}_sequence_conservation.svg")

    # Calculate distances and create visualizations for each selected model
    for model in models:
        print(f"\nProcessing {model} model...")

        model_name = model
        if model == "kimura":
            model_name = "Kimura 2-parameter"
        elif model == "jukes-cantor":
            model_name = "Jukes-Cantor"

        try:
            # Calculate distance matrix
            distance_matrix = calculate_genetic_distance(sequences, model)

            # Create visualizations
            # 1. Heatmap
            heatmap_file = f"{output_prefix}_{model}_heatmap.svg"
            plot_genetic_distance_heatmap(distance_matrix, haplotype_labels, model_name, heatmap_file)
            print(f"  - {heatmap_file}")

            # 2. PCA
            pca_file = f"{output_prefix}_{model}_pca.svg"
            plot_pca(distance_matrix, haplotype_labels, len(variable_sites), model_name, pca_file)
            print(f"  - {pca_file}")

        except Exception as e:
            print(f"Warning: Error processing {model} model: {e}")

    print("\nProcessing complete! All results saved as SVG files.")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Analyze haplotype sequences and create visualizations")
    parser.add_argument("-i", "--hap_fasta", required=True,
                        help="Path to input haplotype FASTA file")
    parser.add_argument("-o", "--output_prefix", default="default", help="Prefix for output files")
    parser.add_argument("-m", "--models", nargs="+", choices=["p-distance", "jukes-cantor", "kimura"],
                        default=["p-distance"],
                        help="Genetic distance models to use (default: p-distance)")

    args = parser.parse_args()

    # Ensure prefix ends with underscore or slash if provided
    if args.output_prefix and not (args.output_prefix.endswith('_') or args.output_prefix.endswith('/')):
        args.output_prefix += '_'

    try:
        process_haplotypes(args.hap_fasta, args.output_prefix, args.models)
    except FileNotFoundError:
        print(f"Error: File '{args.input_file}' not found.")
    except Exception as e:
        print(f"An error occurred: {e}")
        # Print more detailed error information
        import traceback

        traceback.print_exc()
