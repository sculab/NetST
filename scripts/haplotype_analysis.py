# !/usr/bin/env python
# -*- coding: utf-8 -*-
# @Time       : 2024/4/3 17:22
# @Author     : zzhen
# @File       : haplotype_analysis.py
# @Software   : PyCharm
# @Description: 
# @Copyright  : Copyright (c) 2023 by zzhen, All Rights Reserved.
import csv
import json
import argparse
import numpy as np
import pandas as pd
from Bio import SeqIO
import networkx as nx
import matplotlib.pyplot as plt
from networkx.algorithms import community
from sklearn.decomposition import PCA


def format_hap(hap_str):
    # 提取数字部分，假设格式总是 "Hap_xxx"
    num_part = hap_str.split('_')[1]
    # 构造新的字符串
    return f"H{num_part}"


def hamming_distance(seq1, seq2):
    """Calculate the Hamming distance between two sequences."""
    return sum(c1 != c2 for c1, c2 in zip(seq1, seq2))


def plot_genetic_distance_heatmap(ax, norm_distance_matrix):
    # Correcting the heatmap plot by ensuring the mappable object is correctly referenced for the colorbar
    # Plot the heatmap with a single colorbar and labeled axes representing haplotypes
    ax.imshow(norm_distance_matrix, cmap='viridis')
    ax.set_title('Genetic distance heatmap among different haplotype sequences', fontsize=20, pad=20)
    ax.set_xlabel('Haplotypes', fontsize=15, labelpad=10)
    ax.set_ylabel('Haplotypes', fontsize=15, labelpad=10)


def plot_pca(ax, norm_distance_matrix, sequences):
    # Creating a PCA object, keeping two principal components
    pca = PCA(n_components=2)

    # Performing PCA on the genetic distance matrix
    pca_result = pca.fit_transform(norm_distance_matrix)
    # Making the scatter plot with semi-transparent points
    ax.scatter(pca_result[:, 0], pca_result[:, 1], alpha=0.7, s=80)
    # Generating haplotype labels
    haplotype_labels = [format_hap(seq.id) for seq in sequences]

    # Adding annotations with a random offset to decrease overlap
    for i, label in enumerate(haplotype_labels):
        offset = np.random.normal(0, 0.01, size=2)  # Small random offset
        ax.annotate(label, (pca_result[i, 0] + offset[0], pca_result[i, 1] + offset[1]),
                    fontsize=10)

    # Setting title and labels for axes
    ax.set_title('PCA Plot of Haplotypes', fontsize=20, pad=20)
    ax.set_xlabel('Principal Component 1', fontsize=15, labelpad=10)
    ax.set_ylabel('Principal Component 2', fontsize=15, labelpad=10)


# Plot 1: Haplotype Network with key nodes labeled
def plot_haplotype_network(G, pos, node_sizes, node_labels, ax):
    nx.draw(G, pos=pos, node_size=node_sizes, labels=node_labels, font_size=10, edge_color='gray', alpha=0.7, width=1,
            font_color='black', font_family='Arial', node_color='skyblue', ax=ax)
    ax.set_title("Haplotype Network with key nodes labeled", fontsize=20)


# Plot 2: Community Detection Visualization
# Recalculate communities if needed or use previously calculated ones
def plot_community_visualization(G, pos, node_sizes, ax, best_n):
    communities = list(community.greedy_modularity_communities(G, weight='change', best_n=best_n))
    community_colors = {node: i for i, com in enumerate(communities) for node in com}
    colors = [community_colors[node] for node in G.nodes()]

    nx.draw_networkx_nodes(G, pos, node_color=colors, cmap=plt.cm.tab10, node_size=node_sizes, ax=ax)
    nx.draw_networkx_edges(G, pos, alpha=0.7, ax=ax)
    ax.set_title(f'Community Detection Visualization (k={best_n})', fontsize=20)
    ax.axis('off')


# 绘图数据1
# 统计节点和离散性状的关系 显示由单倍型构成最多的前10个节点 以及其对应的离散性状构成 使用条形堆积图
# 该图用于显示 #哪些单倍型是最常见的？#哪些地理区域（离散性状）有显著的单倍型差异？
# count the nodes with more than 1 samples
def plot_node_trait(ax, nodes_dict, seq_dict):
    haplo_nodes = {}
    for k, v in nodes_dict.items():
        haplo_nodes[k] = {}
        for seq_id in v:
            # 获取某个节点包含的多个样本在不同分布区的数量
            haplo_nodes[k][seq_dict[seq_id][2]] = haplo_nodes[k].get(seq_dict[seq_id][2], 0) + 1

    # get the nodes with the most samples
    haplo_count = {k: len(v) for k, v in nodes_dict.items()}
    if len(haplo_count) > 10:
        haplo_count = sorted(haplo_count.items(), key=lambda x: x[1], reverse=True)[:10]
    else:
        haplo_count = sorted(haplo_count.items(), key=lambda x: x[1], reverse=True)
    # get the haplotypes of the nodes with the most samples
    # translate the node id to a haplotype
    haplo_nodes = {k: v for k, v in haplo_nodes.items() if k in [i[0] for i in haplo_count]}
    # 'Hap_5': {'DP': 29, 'OTHER ASIA': 2, 'CHINA': 1, 'CRUISEA': 1}
    haplo_nodes = {seq_dict[k][3]: v for k, v in haplo_nodes.items()}

    chars = list(set([i for v in haplo_nodes.values() for i in v.keys()]))

    for haplo_node in haplo_nodes.keys():
        for char in chars:
            haplo_nodes[haplo_node][char] = haplo_nodes[haplo_node].get(char, 0)

    sorted_haplo_nodes = [(k, v, sum(v.values())) for k, v in
                          sorted(haplo_nodes.items(), key=lambda item: sum(item[1].values()))]

    # # Prepare data for plotting
    labels = [i[0] for i in sorted_haplo_nodes]
    char_data = {char: [i[1][char] for i in sorted_haplo_nodes] for char in chars}

    bottom = np.zeros(len(labels))
    for char, values in char_data.items():
        ax.bar(labels, values, label=char, bottom=bottom)
        bottom += values

    for i, _, j in sorted_haplo_nodes:
        ax.text(i, j + 0.05, '%.0f' % j, ha='center', va='bottom')
    ax.set_xlabel('Haplotype', fontsize=16, labelpad=15)
    ax.set_yticks([])
    ax.set_title('The most common haplotype with their trait distribution', fontsize=20, pad=20)
    ax.legend(loc='upper left')
    [ax.spines[loc_axis].set_visible(False) for loc_axis in ['top', 'right', 'bottom', 'left']]


# 绘图数据2
# 统计节点和连续性状的关系 随着连续性状变化的趋势产生的单倍型的数量
def plot_continue_trait(ax, seq_dict):
    continue_trait_haplo = {}
    # '54': {'DP': 20, 'CHINA': 1}
    # 获取和时间相关的信息
    for value in seq_dict.values():
        if value[1] in continue_trait_haplo.keys():
            continue_trait_haplo[value[1]][value[2]] = continue_trait_haplo[value[1]].get(value[2], 0) + 1
        else:
            continue_trait_haplo[value[1]] = {value[2]: 1}

    # 简化信息 54: 21 连续性状取值:总数
    continue_trait_haplo = {int(k): sum(v.values()) for k, v in continue_trait_haplo.items()}

    # Sorting the data by day
    sorted_days = sorted(continue_trait_haplo.keys())
    sorted_counts = [continue_trait_haplo[day] for day in sorted_days]

    # Plotting the data
    ax.plot(sorted_days, sorted_counts, marker='o', linestyle='-', color='black')
    ax.set_xlabel('Time', fontsize=16, labelpad=15)
    ax.set_ylabel('Number of haplotypes', fontsize=16, labelpad=15)
    ax.set_title('The number of haplotypes with the change of continuous trait', fontsize=20, pad=20)
    [ax.spines[loc_axis].set_visible(False) for loc_axis in ['top', 'right']]


# 展示连接的节点对
def plot_connected_pair(ax, G, seq_dict):
    # Initialize a list to hold the pairs
    connected_pairs_with_titles = []

    # Iterate over the edges
    for edge in G.edges(data=True):
        # Retrieve the nodes connected by the edge
        node1, node2 = edge[0], edge[1]
        # Retrieve the 'title1' attribute for each node
        title1 = G.nodes[node1]['name']
        title2 = G.nodes[node2]['name']
        # Record the pair with their titles
        connected_pairs_with_titles.append((title1, title2))

    # 统计离散性状之间的关联性
    connected_pairs_with_trait = []
    for p1, p2 in connected_pairs_with_titles:
        if p1 in seq_dict and p2 in seq_dict:
            connected_pairs_with_trait.append((seq_dict[p1][2], seq_dict[p2][2]))

    # 统计两辆性状之间的关联
    from collections import defaultdict

    # Count the frequency of each pair
    # Filter out generic pairs that are not informative (like ('DP', 'DP'))
    # 处理pair信息时 一般可以去除性状信息一致的的pair
    # filtered_node_pairs = [pair for pair in connected_pairs_with_locations]
    filtered_node_pairs = [pair for pair in connected_pairs_with_trait if pair[0] != pair[1]]

    pair_counts = defaultdict(int)

    unique_locations = set(x for pair in filtered_node_pairs for x in pair)
    # Increment count for each pair in the original list (including 'DP' pairs)
    for pair in filtered_node_pairs:
        pair_counts[pair] += 1

    # Convert to a list of tuples and sort by frequency
    sorted_pair_counts = sorted(pair_counts.items(), key=lambda item: item[1], reverse=True)
    # 创建无向图
    G_undirected = nx.Graph()

    # 添加节点和边到无向图
    for (source, target), weight in sorted_pair_counts:
        G_undirected.add_edge(source, target, weight=weight)

    pos = nx.spring_layout(G_undirected, seed=42)

    # 绘制节点
    nx.draw_networkx_nodes(G_undirected, pos, node_size=800, node_color="lightblue", ax=ax)
    # 绘制边，边的粗细根据权重调整
    for (u, v, weight) in G_undirected.edges(data='weight'):
        nx.draw_networkx_edges(G_undirected, pos, edgelist=[(u, v)], width=weight / 2, ax=ax, edge_color='green')
    # 绘制节点的标签
    nx.draw_networkx_labels(G_undirected, pos, ax=ax, font_size=10, font_color='black',
                            font_family="Arial", font_weight="bold")

    ax.set_title('The relationship between discrete traits from the connected haplotypes', fontsize=20, pad=20)
    ax.axis('off')


def main(json_file: str, seq2hap: str, hap_file: str, img_suffix: str):
    # 读取json文件 将单倍型图的节点 边信息存储在字典中
    with open(json_file, "r") as f:
        json_data = json.load(f)

    # 获取haplotype文件中的name和hap的对应关系
    name2hap_dict = {}
    with open(seq2hap, 'r') as file:
        reader = csv.DictReader(file)
        for row in reader:
            name2hap_dict[row['name']] = format_hap(row['hap'])
    """
        # 从单倍型网络的结构出发 画 单倍型网络图 和 社区图
    """
    # Create a graph from the nodes and edges
    G = nx.Graph()
    # Add nodes with their ID as node attribute (for potential future use)
    for node in json_data['nodes']:
        G.add_node(node['id'], label=name2hap_dict.get(node["title1"], ""), size=node["frequency"], name=node["title1"])
    # Add edges
    for edge in json_data['edges']:
        G.add_edge(edge['source'], edge['target'], change=edge['change'])

    # Find nodes with degree 1 and store them with their neighbor
    node_degree_one = []
    for n in G.nodes():
        if G.degree(n) == 1:
            # Mark this node for merging
            node_degree_one.append(n)

    # network中node的size和label
    node_sizes = [G.nodes[n]['size'] * 15 for n in G.nodes()]
    node_labels = {n: G.nodes[n]['label'] for n in G.nodes() if n not in node_degree_one}
    # positions for all nodes
    pos = nx.spring_layout(G, seed=100)

    # Create a figure with 2 subplots
    fig, axes = plt.subplots(nrows=2, ncols=2, figsize=(27, 18), dpi=300)
    plt.subplots_adjust(wspace=0.1)  # Adjust the space between the two plots
    plot_haplotype_network(G, pos, node_sizes, node_labels, axes[0, 0])
    plot_community_visualization(G, pos, node_sizes, axes[0, 1], 3)
    plot_community_visualization(G, pos, node_sizes, axes[1, 0], 4)
    plot_community_visualization(G, pos, node_sizes, axes[1, 1], 5)
    # 调整子图之间的间距
    plt.tight_layout()
    # Save the entire figure
    plt.savefig("{}_community_visualization.jpg".format(img_suffix), bbox_inches='tight')

    """
        # 从单倍型网络中节点之间的关系出发 画节点之间的关系图
        # 从seq2hap中读取序列相关信息 ID 离散性状 连续性状 单倍型
    """
    ori_seq_dict = pd.read_csv(seq2hap).set_index("name").to_dict("index")
    # 重新处理序列信息
    seq_dict = {}
    for k, v in ori_seq_dict.items():
        # {DP0005=1=54: [DP0005,54,DP,Hap_1]}
        seq_dict[k] = [k.split("=")[0], k.split("=")[-1], v["trait"], v["hap"]]

    ori_nodes = json_data["nodes"]
    nodes_dict = {}
    for i in ori_nodes:
        nodes_dict[i["title1"]] = i["title2"].split(";")
    # 'DP0005=1=54':['DP0005=1=54','DP0462=1=55','DP0464=1=55','DP0697=1=56','DP0703=1=56','DP0764=1=56']
    # 重新处理节点信息 移除无用节点 即不在序列中的节点（如MJN中的中值节点）
    nodes_dict = {k: v for k, v in nodes_dict.items() if k in seq_dict.keys()}
    # 创建一个图形和3个子图
    fig, axs = plt.subplots(2, 2, figsize=(18, 15))

    plot_node_trait(axs[0, 0], nodes_dict, seq_dict)
    plot_continue_trait(axs[0, 1], seq_dict)
    plot_connected_pair(axs[1, 0], G, seq_dict)

    axs[1, 1].axis('off')

    # 调整子图之间的间距
    plt.tight_layout()
    # 保存图片
    plt.savefig("{}_haplotype_info.jpg".format(img_suffix), dpi=300, bbox_inches='tight')

    """
        从单倍型网络中节点之间的关系出发 画节点之间的关系图
    """
    sequences = list(SeqIO.parse(hap_file, "fasta"))
    sequences = sorted(sequences, key=lambda x: x.id)

    # Calculate the pairwise Hamming distances
    num_sequences = len(sequences)
    distance_matrix = np.zeros((num_sequences, num_sequences), dtype=int)

    for i in range(num_sequences):
        for j in range(i + 1, num_sequences):
            distance = hamming_distance(sequences[i].seq, sequences[j].seq)
            distance_matrix[i, j] = distance_matrix[j, i] = distance

    # Normalize the distance matrix
    if np.max(distance_matrix) > 0:
        norm_distance_matrix = distance_matrix / np.max(distance_matrix)
    else:
        norm_distance_matrix = distance_matrix
    fig, ax = plt.subplots(1, 2, figsize=(24, 10))
    plot_genetic_distance_heatmap(ax[0], norm_distance_matrix)
    plot_pca(ax[1], norm_distance_matrix, sequences)
    # 调整子图之间的间距
    plt.tight_layout()
    # 保存图片
    plt.savefig("{}_haplotype_seq_info.jpg".format(img_suffix), dpi=300, bbox_inches='tight')


if __name__ == "__main__":
    pars = argparse.ArgumentParser(formatter_class=argparse.RawDescriptionHelpFormatter,
                                   description='''Analyse the haplotype graph--zzhen''')
    pars.add_argument('-graph', metavar='<str>', type=str, help='''the json file of the haplotype graph''',
                      required=True)
    pars.add_argument('-seq2hap', metavar='<str>', type=str, help='''the seq2hap csv file''', required=True)
    pars.add_argument('-hap_fa', metavar='<str>', type=str, help='''the haplotype fasta file''', required=True)
    pars.add_argument('-img_suffix', metavar='<str>', type=str, help='''the image suffix''', required=False, default="")
    args = pars.parse_args()
    main(args.graph, args.seq2hap, args.hap_fa, args.out)
