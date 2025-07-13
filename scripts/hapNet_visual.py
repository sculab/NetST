# !/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time       : 2025/5/16 00:00
# @Author     : zzhen
# @File       : hapNet_visual.py
# @Software   : PyCharm
# @Description:
# """
# Haplotype Network Visualization Tool
#
# This script generates a visual representation of haplotype networks from sequence data.
# It creates a network graph where nodes represent haplotypes and edges represent
# the relationships between them, highlighting critical nodes and edges.
#
# Input:
#     - JSON file containing network structure (nodes and edges)
#     - CSV file mapping sequences to haplotypes
#
# Output:
#     - SVG visualization of the haplotype network
# """
# @Copyright  : Copyright (c) 2023 by zzhen, All Rights Reserved.
import argparse
import csv
import json
from typing import Dict

import matplotlib.pyplot as plt
import networkx as nx

# Set global visualization style
plt.style.use('ggplot')


class HaplotypeNetwork:
    """Class for handling haplotype network data and visualization."""

    def __init__(self, graph_json: str, seq2hap_csv: str):
        """
        Initialize the HaplotypeNetwork with input files.

        Args:
            graph_json: Path to JSON file containing network structure
            seq2hap_csv: Path to CSV file with sequence-to-haplotype mapping
        """
        self.graph_json = graph_json
        self.seq2hap_csv = seq2hap_csv
        self.graph = nx.Graph()
        self.positions = None
        self.node_sizes = []
        self.node_labels = {}
        self.critical_nodes = []
        self.critical_edges = []

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
        Construct the network graph from input files and calculate network metrics.
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
                length=edge.get('change', 1)  # Preserve original change value for display
            )

        # Calculate network layout and metrics
        self._calculate_network_metrics()

    def _calculate_network_metrics(self) -> None:
        """
        Calculate network metrics including layout, node sizes, and critical elements.
        """
        # Determine node sizes (scaled by frequency)
        self.node_sizes = [self.graph.nodes[n]['size'] * 15 for n in self.graph.nodes()]

        # Calculate average degree of the graph
        average_degree = sum(dict(self.graph.degree()).values()) / len(self.graph)

        # Generate positions for all nodes using spring layout for better visualization
        self.positions = nx.spring_layout(self.graph, seed=10, iterations=1000)

        # Identify critical nodes using closeness centrality
        # (nodes that have shortest paths to all other nodes)
        closeness_centrality = nx.closeness_centrality(self.graph, distance='length')
        sorted_centrality = sorted(closeness_centrality.items(), key=lambda item: item[1], reverse=True)

        # Select top 10% of nodes as critical (minimum 10)
        cri_node_num = min(10, int(len(self.graph.nodes()) * 0.1))
        self.critical_nodes = [n for n, c in sorted_centrality[:cri_node_num]]

        # Create node labels for critical nodes
        self.node_labels = {n: self.graph.nodes[n]['label'] for n in self.critical_nodes}

        # Identify critical edges using edge betweenness centrality
        # (edges that appear on many shortest paths between nodes)
        edge_betweenness = nx.edge_betweenness_centrality(self.graph, normalized=True)
        sorted_edge_betweenness = sorted(edge_betweenness.items(), key=lambda x: x[1], reverse=True)

        # Select top 10% of edges as critical (minimum 10)
        cri_edge_num = min(10, int(len(self.graph.edges()) * 0.1))
        self.critical_edges = [e for e, c in sorted_edge_betweenness[:cri_edge_num]]

    def visualize(self, image_suffix: str, output_dir: str) -> None:
        """
        Create and save the haplotype network visualization.

        Args:
            image_suffix: Suffix for the output image filename
        """
        plt.figure(figsize=(14, 14), dpi=300)

        # Remove grid and axis for cleaner visualization
        plt.grid(False)
        plt.axis('off')

        # Draw regular nodes
        nx.draw_networkx_nodes(
            self.graph,
            self.positions,
            node_size=self.node_sizes,
            alpha=0.9,
            node_color="#2F5C85",  # Standard node color (blue)
        )

        # Draw critical nodes with different color
        nx.draw_networkx_nodes(
            self.graph,
            self.positions,
            nodelist=self.critical_nodes,
            node_size=[self.graph.nodes[n]['size'] * 15 for n in self.critical_nodes],
            node_color="#D95F02",  # Critical node color (orange)
            alpha=0.9,
            label="Critical Nodes"
        )

        # Draw regular edges
        nx.draw_networkx_edges(
            self.graph,
            self.positions,
            width=0.5,
            edge_color="#666666",  # Standard edge color (gray)
            alpha=0.7
        )

        # Draw critical edges with different color and width
        nx.draw_networkx_edges(
            self.graph,
            self.positions,
            edgelist=self.critical_edges,
            width=2,
            edge_color="#17becf",  # Critical edge color (cyan)
            alpha=0.9,
            label="Critical Edges"
        )

        # Add haplotype labels to critical nodes
        nx.draw_networkx_labels(
            self.graph,
            self.positions,
            labels=self.node_labels,
            font_size=10,
            font_color="black",
            font_family="Times New Roman",
        )

        # Add legend
        plt.legend(
            loc="upper left",
            fontsize=10,
            frameon=True,
            framealpha=0.8,
            fancybox=True
        )

        # Add title
        plt.title("Haplotype Network Visualization", fontsize=16)

        # Save the visualization
        output_file = f"{image_suffix}_VisualHapNet.svg"

        if output_dir is not None:
            output_file = f"{output_dir}/{output_file}"

        plt.savefig(output_file, format="svg", bbox_inches="tight")
        plt.close()


def parse_arguments():
    """
    Parse command line arguments.

    Returns:
        Parsed command line arguments
    """
    parser = argparse.ArgumentParser(
        description="Haplotype Network Visualization Tool",
        formatter_class=argparse.ArgumentDefaultsHelpFormatter
    )

    parser.add_argument(
        "--graph",
        required=True,
        help="Path to the input graph JSON file"
    )

    parser.add_argument(
        "--seq2hap",
        required=True,
        help="Path to the sequence-to-haplotype CSV file"
    )

    parser.add_argument(
        '--out',
        type=str,
        default=None,
        help='Output directory for visualization files'
    )

    parser.add_argument(
        '--img_suffix',
        type=str,
        default='default',
        help='Suffix for output image files'
    )

    return parser.parse_args()


def main():
    """Main function to run the haplotype network visualization."""
    # Parse command line arguments
    args = parse_arguments()

    # Create haplotype network from input files
    network = HaplotypeNetwork(args.graph, args.seq2hap)
    network.build_network()

    # Generate and save visualization
    network.visualize(args.img_suffix, args.out)

    print(f"Haplotype network visualization saved as {args.img_suffix}_haplotype_network.svg")


if __name__ == '__main__':
    # --graph test/test.json --seq2hap test/test_seq2hap.csv --out test --img_suffix test
    main()
