# !/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time       : 2025/5/16 00:47
# @Author     : zzhen
# @File       : hapNet_community_modularity.py
# @Software   : PyCharm
# @Description:
"""
Haplotype Network Community Structure Analysis

This script analyzes community structures within haplotype networks using the Girvan-Newman algorithm.
It identifies optimal community partitions based on modularity scores and visualizes the results.

Input:
    - JSON file containing network structure (nodes and edges)
    - CSV file mapping sequences to haplotypes

Output:
    - SVG visualization of the community structure with the highest modularity score
    - SVG plot of modularity scores vs. number of communities
"""
# @Copyright  : Copyright (c) 2023 by zzhen, All Rights Reserved.
# !/usr/bin/env python3
import argparse
import csv
import json
from typing import Dict

import matplotlib.pyplot as plt
import networkx as nx
from networkx.algorithms import community

# Set global visualization style
plt.style.use('ggplot')


class HaplotypeCommunityAnalyzer:
    """Class for analyzing community structures in haplotype networks."""

    def __init__(self, graph_json: str, seq2hap_csv: str):
        """
        Initialize the community analyzer with input files.

        Args:
            graph_json: Path to JSON file containing network structure
            seq2hap_csv: Path to CSV file with sequence-to-haplotype mapping
        """
        self.graph_json = graph_json
        self.seq2hap_csv = seq2hap_csv
        self.graph = nx.Graph()
        self.positions = None
        self.node_sizes = []
        self.best_communities = None
        self.best_modularity = 0.0
        self.community_counts = []
        self.modularity_scores = []
        self.color_map = plt.get_cmap('tab20')  # Colormap for community visualization

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
        # Lower iterations for community analysis as position exactness is less critical
        self.positions = nx.spring_layout(self.graph, seed=10, iterations=200)

    def analyze_communities(self) -> None:
        """
        Analyze community structures using the Girvan-Newman algorithm.

        This method finds the optimal community division by calculating modularity
        scores for each possible community structure.
        """
        # Apply Girvan-Newman algorithm to find communities
        # This algorithm progressively removes edges with highest betweenness centrality
        communities_generator = community.girvan_newman(self.graph)

        # Track community structures and their modularity scores
        self.best_modularity = -1
        self.best_communities = None
        self.community_counts = []
        self.modularity_scores = []

        # Evaluate each community structure
        for communities in communities_generator:
            # Convert generator to list of communities
            comm_list = [list(c) for c in communities]

            # Calculate modularity score
            # Modularity measures the density of links inside communities compared to links between communities
            modularity = nx.community.modularity(self.graph, comm_list)

            # Track metrics
            self.modularity_scores.append(modularity)
            self.community_counts.append(len(comm_list))

            # Update the best communities if current modularity is higher
            if modularity > self.best_modularity:
                self.best_modularity = modularity
                self.best_communities = communities

    def plot_modularity_curve(self, output_file: str) -> None:
        """
        Create a plot showing the relationship between number of communities and modularity scores.

        Args:
            output_file: Path to save the output SVG visualization
        """
        plt.figure(figsize=(10, 6), dpi=300)

        # Plot modularity vs. number of communities
        plt.plot(self.community_counts, self.modularity_scores, marker='o', color='tab:blue')

        # Add reference line for good modularity threshold (0.3 is commonly used)
        plt.axhline(y=0.3, color='tab:red', linestyle='--', label='Good Modularity Threshold (0.3)')

        # Add labels and title
        plt.xlabel('Number of Communities')
        plt.ylabel('Modularity Score')
        plt.title('Modularity vs. Number of Communities')
        plt.grid(True)
        plt.legend()
        plt.tight_layout()

        # Save the plot
        plt.savefig(output_file, format='svg', bbox_inches='tight')
        plt.close()

    def visualize_best_community(self, output_file: str) -> None:
        """
        Visualize the community structure with the highest modularity score.

        Args:
            output_file: Path to save the output SVG visualization
        """
        # Map nodes to their community index for coloring
        community_colors = {}
        for i, community_nodes in enumerate(self.best_communities):
            for node in community_nodes:
                community_colors[node] = i

        # Get color index for each node
        colors = [community_colors.get(node, -1) for node in self.graph.nodes()]

        # Create the visualization
        plt.figure(figsize=(12, 12), dpi=300)
        plt.grid(False)
        plt.axis('off')

        # Draw nodes with community-based coloring
        nx.draw_networkx_nodes(
            self.graph,
            self.positions,
            node_color=colors,
            cmap=self.color_map,
            node_size=self.node_sizes
        )

        # Draw edges
        nx.draw_networkx_edges(
            self.graph,
            self.positions,
            width=1,
            edge_color='gray',
            alpha=0.5
        )

        # Add title with modularity score
        plt.title(f"Community Structure with Highest Modularity (Modularity = {self.best_modularity:.3f})")

        # Save the visualization
        plt.savefig(output_file, format='svg', bbox_inches='tight')
        plt.close()

    def run_analysis(self, img_suffix: str, out_dir: str) -> None:
        """
        Execute the complete community analysis workflow.

        Args:
            img_suffix: Suffix for output image filenames
        """
        # Build the network from input files
        self.build_network()

        # Find the optimal community structure
        self.analyze_communities()

        # Create and save visualizations
        modularity_plot_file = f"{img_suffix}_modularity_vs_communities.svg"
        if out_dir:
            modularity_plot_file = f"{out_dir}/{modularity_plot_file}"
        self.plot_modularity_curve(modularity_plot_file)

        best_community_plot_file = f"{img_suffix}_highest_modularity_community.svg"
        if out_dir:
            best_community_plot_file = f"{out_dir}/{img_suffix}_highest_modularity_community.svg"
        self.visualize_best_community(best_community_plot_file)

        # Report results
        print(
            f"Analysis complete. Found {len(list(self.best_communities))} communities with modularity {self.best_modularity:.3f}")
        print(f"Visualizations saved as {modularity_plot_file} and {best_community_plot_file}")


def parse_arguments() -> argparse.Namespace:
    """
    Parse command line arguments.

    Returns:
        Parsed command line arguments
    """
    parser = argparse.ArgumentParser(
        description="Analyze and visualize community structures in haplotype networks.",
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
        '--out',
        type=str,
        default=None,
        help='Path to the output directory for image files'
    )

    parser.add_argument(
        '--img_suffix',
        type=str,
        default='default',
        help='Suffix for output image files'
    )

    return parser.parse_args()


def main() -> None:
    """
    Main function to run the haplotype network community analysis.
    """
    # Parse command line arguments
    args = parse_arguments()

    # Create and run the community analyzer
    analyzer = HaplotypeCommunityAnalyzer(args.graph, args.seq2hap)
    analyzer.run_analysis(args.img_suffix, args.out)


if __name__ == "__main__":
    main()
