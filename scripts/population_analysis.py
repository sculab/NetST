# !/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time       : 2025/5/16 06:19
# @Author     : zzhen
# @File       : population_analysis.py
# @Software   : PyCharm
# @Description:
"""
DNA Sequence Population Genetics Analysis Script
------------------------------------------------
This script calculates various population genetics statistics from aligned DNA sequences.
It takes a FASTA file as input and produces a summary of genetic diversity measures.
"""
# @Copyright  : Copyright (c) 2023 by zzhen, All Rights Reserved.


import argparse

import numpy as np
from Bio import SeqIO


def segregating_sites(sequences):
    """
    Calculate the number of segregating (polymorphic) sites.

    A segregating site is a position in the sequence alignment where
    at least two different nucleotides are observed across the sequences.

    Args:
        sequences (list): List of aligned DNA sequences

    Returns:
        int: Number of segregating sites
    """
    # Transpose the sequences to iterate through columns (sites)
    # For each site, count if there's more than one type of nucleotide
    return sum(len(set(column)) > 1 for column in zip(*sequences))


def parsimony_informative_sites(sequences):
    """
    Calculate the number of parsimony-informative sites.

    A site is parsimony-informative if at least two different nucleotides
    occur at the site, each in at least two sequences. These sites
    are useful for constructing phylogenetic trees.

    Args:
        sequences (list): List of aligned DNA sequences

    Returns:
        int: Number of parsimony-informative sites
    """
    count = 0
    # Iterate through each column (site) in the alignment
    for column in zip(*sequences):
        # Get the unique bases at this site
        unique_bases = set(column)
        if len(unique_bases) > 1:
            # Count occurrences of each unique base
            base_counts = [column.count(base) for base in unique_bases]
            # Site is parsimony-informative if at least two different bases
            # each appear in at least two sequences
            if sum(c >= 2 for c in base_counts) >= 2:
                count += 1
    return count


def pairwise_differences(sequences):
    """
    Calculate the sum of pairwise differences between all sequences.

    This function counts the total number of nucleotide differences
    for all possible pairs of sequences in the sample.

    Args:
        sequences (list): List of aligned DNA sequences

    Returns:
        int: Sum of all pairwise differences
    """
    n = len(sequences)
    # Double nested loop to compare each sequence pair exactly once
    return sum(
        # Count differences between sequences i and j
        sum(c1 != c2 for c1, c2 in zip(sequences[i], sequences[j]))
        for i in range(n)
        for j in range(i + 1, n)
    )


def nucleotide_diversity(sequences):
    """
    Calculate nucleotide diversity (π).

    Nucleotide diversity is the average number of nucleotide differences
    per site between any two sequences chosen from the sample population.
    It's a measure of genetic variation in the population.

    π = (sum of pairwise differences) / (n choose 2) / L

    Args:
        sequences (list): List of aligned DNA sequences

    Returns:
        float: Nucleotide diversity value
    """
    n = len(sequences)
    if n < 2:
        return 0

    # Calculate total pairwise differences
    pairwise_diffs = pairwise_differences(sequences)

    # Divide by number of pairs and sequence length
    # (n choose 2) = n(n-1)/2 = number of possible sequence pairs
    return pairwise_diffs / (n * (n - 1) / 2) / len(sequences[0])


def wattersons_theta(sequences):
    """
    Calculate Watterson's theta (θW).

    Watterson's estimator is another measure of genetic diversity,
    based on the number of segregating sites. It's an estimate of
    the population mutation rate (4Nμ for diploid organisms).

    θW = S / a1 / L
    where S is the number of segregating sites,
    a1 is the (n-1)th harmonic number,
    L is the sequence length.

    Args:
        sequences (list): List of aligned DNA sequences

    Returns:
        float: Watterson's theta value
    """
    n = len(sequences)
    if n < 2:
        return 0

    # Count segregating sites
    S = segregating_sites(sequences)

    # Calculate the (n-1)th harmonic number
    a1 = sum(1 / i for i in range(1, n))

    # Calculate theta and normalize by sequence length
    return S / a1 / len(sequences[0])


def tajimas_d(sequences):
    """
    Calculate Tajima's D statistic.

    Tajima's D is a test statistic used to test the neutral theory
    of molecular evolution. It compares two estimates of genetic diversity:
    π (based on pairwise differences) and θW (based on segregating sites).

    A negative D suggests recent selective sweep, population expansion or
    purifying selection, while positive D suggests balancing selection or
    population structure.

    Args:
        sequences (list): List of aligned DNA sequences

    Returns:
        float or None: Tajima's D value, or None if variance is zero
    """
    n = len(sequences)
    if n < 2:
        raise ValueError("At least two sequences are required for Tajima's D calculation.")

    L = len(sequences[0])  # Sequence length

    # Step 1: Nucleotide diversity (π) × sequence length (not normalized by L)
    pi = nucleotide_diversity(sequences) * L

    # Step 2: Calculate Watterson's θ (not normalized by L)
    S = segregating_sites(sequences)
    a1 = sum(1 / i for i in range(1, n))
    theta_w = S / a1

    # Step 3: Calculate variance and Tajima's D
    # These coefficients come from the theoretical derivation of the variance
    a2 = sum(1 / (i ** 2) for i in range(1, n))
    b1 = (n + 1) / (3 * (n - 1))
    b2 = 2 * (n ** 2 + n + 3) / (9 * n * (n - 1))
    c1 = b1 - (1 / a1)
    c2 = b2 - (n + 2) / (a1 * n) + a2 / (a1 ** 2)
    e1 = c1 / a1
    e2 = c2 / (a1 ** 2 + a2)

    # Calculate the variance of the difference between π and θW
    variance = e1 * S + e2 * S * (S - 1)

    if variance == 0:
        return None  # Avoid division by zero

    # Calculate Tajima's D
    tajima_d = (pi - theta_w) / np.sqrt(variance)
    return tajima_d


def haplotype_diversity(sequences):
    """
    Calculate haplotype diversity (Hd).

    Haplotype diversity is a measure of the uniqueness of haplotypes
    in a given population. It represents the probability that two
    randomly sampled haplotypes are different.

    Hd = (n/(n-1)) * (1 - Σ(fi²))
    where n is sample size and fi is the frequency of the ith haplotype.

    Args:
        sequences (list): List of aligned DNA sequences

    Returns:
        float: Haplotype diversity value (0-1)
    """
    n = len(sequences)
    if n < 2:
        return 0

    # Count the frequency of each haplotype
    haplotype_counts = {}
    for seq in sequences:
        if seq in haplotype_counts:
            haplotype_counts[seq] += 1
        else:
            haplotype_counts[seq] = 1

    # Calculate haplotype diversity
    # Sum of squared frequencies
    sum_squared_freqs = sum((count / n) ** 2 for count in haplotype_counts.values())
    # Final formula with correction for sample size
    hd = (n / (n - 1)) * (1 - sum_squared_freqs)

    return hd


def analyze_sequences(input_file, output_file=None):
    """
    Analyze sequences and output statistics.

    This function loads sequences from a FASTA file, calculates various
    population genetics statistics, and outputs the results to a file
    or prints them to the console.

    Args:
        input_file (str): Path to input FASTA file
        output_file (str, optional): Path to output statistics file

    Returns:
        bool: True if analysis was successful, False otherwise
    """
    try:
        # Load sequences from FASTA file using BioPython
        print(f"Loading sequences from {input_file}...")
        sequences = [str(record.seq) for record in SeqIO.parse(input_file, "fasta")]

        if not sequences:
            raise ValueError(f"No sequences found in {input_file}")

        # Verify all sequences have the same length (aligned)
        if len(set(len(seq) for seq in sequences)) > 1:
            raise ValueError("All sequences must have the same length for analysis.")

        print(f"Found {len(sequences)} sequences, calculating statistics...")

        # Calculate basic sequence information
        seq_count = len(sequences)
        seq_length = len(sequences[0])
        haplotype_count = len(set(sequences))

        # Calculate population genetics statistics
        S = segregating_sites(sequences)
        pis = parsimony_informative_sites(sequences)
        pi = nucleotide_diversity(sequences)
        theta = wattersons_theta(sequences)
        hd = haplotype_diversity(sequences)
        D = tajimas_d(sequences)

        # Prepare results in a formatted text block
        results = [
            f"Sequence Analysis Results",
            f"========================",
            f"Input file: {input_file}",
            f"Number of sequences: {seq_count}",
            f"Sequence length: {seq_length} bp",
            f"Number of haplotypes: {haplotype_count}",
            f"Haplotype diversity (Hd): {hd:.4f}",
            f"Segregating Sites (S): {S}",
            f"Parsimony-Informative Sites: {pis}",
            f"Nucleotide Diversity (π): {pi:.6f}",
            f"Watterson's Theta (θW): {theta:.6f}"
        ]

        # Add Tajima's D if it could be calculated
        if D is not None:
            results.append(f"Tajima's D: {D:.4f}")
        else:
            results.append("Variance is zero; Tajima's D cannot be calculated.")

        # Join all results into a single string
        results_text = "\n".join(results)

        # Output results to file or console
        if output_file:
            with open(output_file, 'w') as f:
                f.write(results_text + "\n")
            print(f"Results written to {output_file}")
        else:
            print("\n" + results_text)

        return True

    except FileNotFoundError:
        print(f"Error: File {input_file} not found.")
        return False
    except ValueError as e:
        print(f"Error: {e}")
        return False
    except Exception as e:
        print(f"An unexpected error occurred: {e}")
        import traceback
        traceback.print_exc()
        return False


def main():
    """
    Main function to parse arguments and run the analysis.
    """
    # Set up command line argument parser
    parser = argparse.ArgumentParser(
        description="Calculate population genetics statistics from DNA sequence alignments.",
        epilog="Example: python population_analysis.py input.fasta -o results.txt"
    )

    # Add required and optional arguments
    parser.add_argument(
        "-i", "--input_file",
        help="Path to input aligned DNA sequences in FASTA format"
    )
    parser.add_argument(
        "-o", "--output",
        help="Path to output statistics file (optional)"
    )

    # Parse arguments from command line
    args = parser.parse_args()

    # Run the sequence analysis
    analyze_sequences(args.input_file, args.output)


if __name__ == "__main__":
    main()
