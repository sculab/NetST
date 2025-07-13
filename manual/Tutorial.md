# NetST User Manual

## 1. Introduction

NetST is an integrated software platform for haplotype network construction, visualization, and automated analytics. It addresses critical limitations in current tools by providing:

- **Automated quantitative analytics** replacing subjective visual interpretation
- **Dual-trait visualization** enabling simultaneous display of discrete and continuous traits
- **Comprehensive workflow** from raw sequences to publication-ready outputs

### Key Features

- Three network construction algorithms: MSN, Median-joining, and TCS
- Automated community detection using modified Girvan-Newman algorithm
- Quantitative network topology metrics
- Novel dual-trait visualization system
- Support for large-scale datasets (>10,000 sequences)

## 2. Workflow

NetST integrates five interconnected modules:

1. **Data Input**: Accept FASTA/CSV formats with metadata
2. **Sequence Alignment**: Integrated MAFFT alignment with interactive curation
3. **Haplotype Identification**: Collapse identical sequences while maintaining metadata
4. **Network Construction**: Generate networks using fastHaN engine
5. **Visualization & Analysis**: Dual-trait mapping and automated topological analytics

## 3. Installation

### System Requirements

- Operating System: [To be specified]
- Memory: [Recommended RAM]
- Dependencies: [List of required software]

### Installation Steps

[To be completed by user]

## 4. Quick Start

### Basic Pipeline

1. Prepare your sequence data in FASTA format
2. Launch NetST and load your data
3. Select alignment options and review results
4. Choose network construction algorithm (MSN/MJ/TCS)
5. Visualize network with trait mapping
6. Perform automated community detection
7. Export results and metrics

### Example Dataset

A sample dataset is provided at: https://github.com/sculab/NetST/examples

## 5. Data Preparation

### Input Requirements

- **Sequence Format**: FASTA with unique headers
- **Metadata Format**: CSV file with sample IDs matching FASTA headers
- **Supported Data**: Single or multi-locus sequences

### Metadata Structure

- Column 1: Sample ID (must match FASTA headers)
- Column 2+: Trait data (categorical or continuous)
- Missing data: Use "NA" or leave blank

### Data Tips

- Ensure consistent naming between sequences and metadata
- Check for duplicate sequence names
- Validate trait data ranges for continuous variables

## 6. Function

### 6.1 Sequence Alignment

- **Algorithm**: MAFFT with optimized parameters
- **Interactive Curation**: Manual adjustment of alignments
- **Export Options**: Aligned sequences in multiple formats

### 6.2 Network Construction

- **Minimum Spanning Network (MSN)**: Best for closely related sequences
- **Median-Joining Network**: Handles missing data and recombination
- **TCS (Statistical Parsimony)**: 95% parsimony connection limit

### 6.3 Visualization Features

- **Single Trait Mode**: Traditional pie-chart representation

- **Continuous Trait Mode**: Gradient visualization

- Dual-Trait Mode

  : Concurrent display of two traits

  - Outer ring: Discrete categorical traits
  - Inner circle: Continuous variables (0-100% saturation)

### 6.4 Automated Analytics

- Community Detection

  : Modified Girvan-Newman algorithm

  - Automated identification of population structure
  - Modularity (Q) scores for validation

- Network Metrics

  :

  - Node-level: Degree, betweenness, closeness centrality
  - Edge-level: Betweenness centrality
  - Global: Network diameter, clustering coefficient

## 7. Output & Interpretation

### 7.1 Visualization Outputs

- **File Formats**: SVG (vector graphics), PNG, PDF
- **Customization**: Node size, colors, labels, edge thickness

### 7.2 Analytics Results

- Community Files

  :

  - Community assignments for each haplotype
  - Modularity scores for different k values

- Metrics Export

  :

  - Node metrics table (CSV)
  - Edge metrics table (CSV)
  - Global network statistics

### 7.3 Interpretation Guide

- **Community Detection**: Q > 0.3 indicates significant structure
- **Hub Nodes**: High betweenness centrality suggests ancestral haplotypes
- **Edge Betweenness**: High values indicate critical evolutionary pathways

## 8. Q&A

### Common Issues

**Q: How do I handle large datasets?** A: NetST efficiently processes >10,000 sequences. For optimal performance:

- Use multi-threading options
- Consider pre-filtering identical sequences
- Adjust visualization density settings

**Q: What if community detection gives unclear results?** A: Try different k values and compare modularity scores. Values of Q > 0.3 indicate meaningful partitions.

**Q: Can I analyze multiple genes simultaneously?** A: Yes, NetST supports multi-locus analysis. Concatenate sequences with appropriate separators.

**Q: How do I choose between network algorithms?** A:

- MSN: Best for population-level studies with low divergence
- MJ: Suitable for complex evolutionary histories
- TCS: Traditional choice for intraspecific phylogeography

## 9. Citation & Acknowledgments

### Citation

If you use NetST in your research, please cite:

Zhang Z, Yu Y. NetST: An integrated graphic tool for haplotype network construction, visualization, and automated analytics. [Journal] [Year].

### Acknowledgments

Development supported by the National Natural Science Foundation of China.

### License

NetST is distributed under the MIT License.

### Contact

- GitHub: https://github.com/sculab/NetST
- Email: yyu@scu.edu.cn
