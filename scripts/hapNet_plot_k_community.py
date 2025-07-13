# !/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time       : 2025/5/16 01:16
# @Author     : zzhen
# @File       : hapNet_plot_k_community.py
# @Software   : PyCharm
# @Description: 
# @Copyright  : Copyright (c) 2023 by zzhen, All Rights Reserved.
# !/usr/bin/env python3

"""
This script visualizes community structures in haplotype networks based on user-specified
k values (number of communities). It uses the Girvan-Newman algorithm to find community
divisions and generates visualizations for each specified k value.

Input:
    - JSON file containing network structure (nodes and edges)
    - CSV file mapping sequences to haplotypes
    - List of k values (up to 4) for community structure visualization

Output:
    - SVG visualization for each k value community structure
    - Text file with community membership details for each k value
"""

import argparse
import csv
import json
from typing import Dict, Tuple, List, Optional, Set

import matplotlib.pyplot as plt
import networkx as nx
from networkx.algorithms import community

# Set global visualization style
plt.style.use('ggplot')


class HaplotypeKCommunityAnalyzer:
    """Class for analyzing k-specific community structures in haplotype networks."""

    def __init__(self, graph_json: str, seq2hap_csv: str):
        """
        Initialize the k-community analyzer with input files.

        Args:
            graph_json: Path to JSON file containing network structure
            seq2hap_csv: Path to CSV file with sequence-to-haplotype mapping
        """
        self.graph_json = graph_json
        self.seq2hap_csv = seq2hap_csv
        self.graph = nx.Graph()
        self.positions = None
        self.node_sizes = []
        self.communities_list = []
        self.color_map = plt.get_cmap('tab20')

    @staticmethod
    def format_haplotype(hap_str: str) -> str:
        """
        Format haplotype string from "Hap_xxx" to "Hxxx".

        Args:
            hap_str: Original haplotype string in "Hap_xxx" format

        Returns:
            Formatted haplotype string "Hxxx"
        """
        num_part = hap_str.split('_')[1]
        return f"H{num_part}"

    def load_sequence_haplotype_mapping(self) -> Dict[str, str]:
        """
        Load mapping between sequence names and haplotypes from CSV.

        Returns:
            Dictionary mapping sequence names to formatted haplotype labels
        """
        name2hap_dict = {}
        with open(self.seq2hap_csv, 'r') as file:
            reader = csv.DictReader(file)
            for row in reader:
                name2hap_dict[row['name']] = self.format_haplotype(row['hap'])
        return name2hap_dict

    def build_network(self) -> None:
        """
        Construct the network graph from input files.
        """
        # Load sequence to haplotype mapping
        name2hap_dict = self.load_sequence_haplotype_mapping()

        # Load graph JSON data
        with open(self.graph_json, "r") as f:
            json_data = json.load(f)

        # Add nodes with attributes
        for node in json_data['nodes']:
            sequence_name = node["title1"].split("=")[0]
            hap_label = name2hap_dict.get(sequence_name, "")

            self.graph.add_node(
                node['id'],
                label=hap_label,
                size=node.get("frequency", 1),
                name=sequence_name
            )

        # Add edges with 'change' attribute as genetic distance
        for edge in json_data['edges']:
            self.graph.add_edge(
                edge['source'],
                edge['target'],
                length=edge.get('change', 1)  # Preserve original change value for distance
            )

        # Calculate node sizes based on frequency
        self.node_sizes = [self.graph.nodes[n]['size'] * 15 for n in self.graph.nodes()]

        # Generate positions for visualization using spring layout
        # Higher iterations for better visualization quality
        self.positions = nx.spring_layout(self.graph, iterations=1000, seed=10)

        # Generate all possible community structures using Girvan-Newman algorithm
        self.communities_list = list(community.girvan_newman(self.graph))

    def find_community_by_k(self, k: int) -> Optional[Tuple[Set]]:
        """
        Find community structure with exactly k communities.

        Args:
            k: Number of communities to find

        Returns:
            Tuple of sets containing node communities, or None if not found
        """
        # Validate k value
        if k < 2 or k > len(self.graph.nodes()):
            print(f"Warning: k={k} is out of valid range (2 to {len(self.graph.nodes())})")
            return None

        # Search for community structure with k communities
        for communities in self.communities_list:
            if len(communities) == k:
                return communities

        print(f"Warning: No community structure with exactly {k} communities found")
        return None

    def visualize_community(self, communities: Tuple[Set], k: int, output_prefix: str, out_dir: str) -> None:
        """
        Visualize a specific community structure.

        Args:
            communities: Tuple of sets containing node communities
            k: Number of communities in the structure
            output_prefix: Prefix for output filenames
            out_dir: Output directory for image files
        """
        if communities is None:
            return

        # Calculate modularity for this community structure
        modularity = community.modularity(self.graph, communities)

        # Map nodes to community indices for coloring
        community_colors = {}
        for i, community_nodes in enumerate(communities):
            for node in community_nodes:
                community_colors[node] = i

        colors = [community_colors.get(node, -1) for node in self.graph.nodes()]

        # Create the visualization
        plt.figure(figsize=(12, 12), dpi=300)
        plt.grid(False)
        plt.axis('off')

        # Draw nodes with community-based coloring
        nx.draw_networkx_nodes(
            self.graph,
            self.positions,
            node_size=self.node_sizes,
            node_color=colors,
            cmap=self.color_map
        )

        # Draw edges
        nx.draw_networkx_edges(
            self.graph,
            self.positions,
            alpha=0.6,
            edge_color='gray'
        )

        # Add title with community count and modularity score
        plt.title(f"Community Structure (k={k}, Modularity={modularity:.3f})")

        # Save the visualization as SVG
        svg_file = f"{output_prefix}_community_k{k}.svg"
        if out_dir:
            svg_file = f"{out_dir}/{svg_file}"
        plt.savefig(svg_file, format='svg', bbox_inches='tight')
        plt.close()

        # Create text file with community membership details
        text_file = f"{output_prefix}_community_k{k}.txt"
        if out_dir:
            text_file = f"{out_dir}/{text_file}"
        with open(text_file, 'w') as f:
            f.write(f"Number of communities (k): {k}\n")
            f.write(f"Modularity score: {modularity:.3f}\n\n")

            # List members of each community
            for i, nodes in enumerate(communities):
                f.write(f"Community {i + 1} ({len(nodes)} members):\n")
                # Sort nodes by haplotype label for better readability
                sorted_nodes = sorted(nodes, key=lambda n: self.graph.nodes[n]['label'])

                # Output haplotype labels in rows
                labels = [self.graph.nodes[node]['label'] for node in sorted_nodes]
                # Format as columns with 5 labels per line
                for j in range(0, len(labels), 5):
                    f.write("  " + "  ".join(labels[j:j + 5]) + "\n")
                f.write("\n")

    def run_analysis(self, k_values: List[int], img_suffix: str, out_dir: str) -> None:
        """
        Execute community analysis for multiple k values.

        Args:
            k_values: List of k values (number of communities) to analyze
            img_suffix: Suffix for output image filenames
            out_dir: Output directory for image files
        """
        # Build the network from input files and compute all community divisions
        self.build_network()

        # Count of valid k values processed
        valid_k_count = 0

        # Process each k value
        for k in k_values:
            # Find community structure with k communities
            communities = self.find_community_by_k(k)

            if communities:
                # Visualize this community structure
                self.visualize_community(communities, k, img_suffix, out_dir)
                valid_k_count += 1
                print(
                    f"Created visualization for k={k} (Modularity: {community.modularity(self.graph, communities):.3f})")

        # Report summary
        print(f"Analysis complete. Created visualizations for {valid_k_count} k values.")


def parse_arguments() -> argparse.Namespace:
    """
    Parse command line arguments.

    Returns:
        Parsed command line arguments
    """
    parser = argparse.ArgumentParser(
        description="Visualize community structures with specific k values in haplotype networks.",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter
    )

    parser.add_argument(
        '--graph',
        type=str,
        required=True,
        help='Path to the JSON file containing the haplotype network data'
    )

    parser.add_argument(
        '--seq2hap',
        type=str,
        required=True,
        help='Path to the CSV file mapping sequences to haplotypes'
    )

    parser.add_argument(
        '--k',
        type=int,
        nargs='+',
        required=True,
        help='Up to 4 k values for community visualization (e.g., --k 3 5 7)'
    )

    parser.add_argument(
        '--out',
        type=str,
        default='./',
        help='Output directory for image files'
    )

    parser.add_argument(
        '--img_suffix',
        type=str,
        default='default',
        help='Suffix for output image files'
    )

    return parser.parse_args()


def validate_k_values(k_values: List[int]) -> List[int]:
    """
    Validate and limit k values to a maximum of 4.

    Args:
        k_values: List of k values provided as arguments

    Returns:
        Validated list of k values (maximum 4)
    """
    if len(k_values) > 4:
        print(f"Warning: Only the first 4 k values will be used ({k_values[:4]})")
        return k_values[:4]
    return k_values


def main() -> None:
    """
    Main function to run the k-specific community analysis.
    """
    # Parse command line arguments
    args = parse_arguments()

    # Validate k values (maximum 4)
    k_values = validate_k_values(args.k)

    # Create and run the k-community analyzer
    analyzer = HaplotypeKCommunityAnalyzer(args.graph, args.seq2hap)
    analyzer.run_analysis(k_values, args.img_suffix, args.out)


if __name__ == "__main__":
    main()
