# NetST User Manual

## 1. Overview

**NetST** is an integrated software platform designed for large-scale, multi-trait haplotype datasets. It supports haplotype sequence identification, network construction, visualization, and automated analytics, and is applicable to various research fields including population genetics, phylogeography, and molecular epidemiology.

NetST provides core functionalities for haplotype network construction and visualization, with a complete set of upstream and downstream analysis pipelines that greatly improve research efficiency. It features an innovative **quantitative topological analysis** module for analyzing network structures, enabling users to identify key haplotypes, core groups, and important connections within the network.

Additionally, NetST offers **dual-trait mapping and correlation analysis**, allowing users to visualize multiple associated traits on a single haplotype network and statistically assess their relationships—offering a more comprehensive perspective on phylogenetic and phenotypic associations.

**Key Features**:

- Support for three mainstream haplotype network algorithms (TCS / MJN / MSN)
- Network topology and community structure analysis (centrality, modularity)
- Innovative dual-trait visualization and correlation analysis system
- Unified graphical interface with automated workflow

## 2. Software Workflow

![workflow2](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507192107232.png)

NetST provides an end-to-end analysis pipeline covering data import, sequence alignment, haplotype identification, network construction, visualization, and downstream analysis. The workflow is composed of the following core modules:

**1. Data Import and Preprocessing**

- Accepts both FASTA and CSV file formats with embedded metadata (e.g., trait information)
- Utilizes MAFFT for high-quality multiple sequence alignment
- Offers an interactive interface for sequence inspection and metadata management

**2. Haplotype Identification**

- Applies a DnaSP-inspired algorithm to process aligned sequences
- Automatically collapses identical haplotypes to streamline the network

**3. Network Construction**

- Based on high-performance fastHaN software, implements three mainstream network construction algorithms:
  - **TCS (Statistical Parsimony Network)**
  - **MJN (Median-Joining Network)**
  - **MSN (Minimum Spanning Network)**
- Outputs standardized JSON/GML network formats

**4. Network Visualization**

- Uses enhanced tcsBU software for network rendering
- Uses concentric circle strategy to visualize discrete and continuous traits
- Supports flexible node annotation
- Supports SVG vector export

**5. Automated Analytics**

- **Quantitative Topological Analysis**: Uses optimized GN algorithm for community detection, and evaluates based on modularity
- **Sequence Statistical Analysis**: Integrates Principal Component Analysis (PCA), Tajima's D test, and other classical methods
- **Population Genetics Analysis**: Calculates classical population genetics indicators based on input sequences
- **Dual-trait Association Analysis**: Uses ANOVA/Kruskal-Wallis for trait association analysis

## 3. Installing and Running NetST

- Download the latest release from the [NetST GitHub page](https://github.com/sculab/NetST/releases)
- Extract the package and double-click `NetST.exe` to launch the software (no installation required)
- **Important**: Ensure the program is run from within the extracted NetST directory. All required dependencies are included and linked internall

![The-Interface-of-NetST](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292123757.png)

## 4. Quick Start

### **Supported Data Formats**

NetST accepts sequence datasets in either FASTA or CSV formats. Each sequence should include sample metadata.

Two sample datasets are provided in the [Examples](https://claude.ai/Examples) directory:

- SL-SARS-CoV-2: Contains 130 representative haplotypes distinguishing L/S lineages (discrete trait)
- DP-SARS-CoV-2:
- Contains 482 genome sequences with annotated geographic location (discrete) and collection date (continuous)

### **Loading and Preprocessing Data**

#### Loading Data

1. Click 【File > Load Sequences】 in the menu bar
2. Select your data file (e.g., `Examples/DP-SARS-CoV-2/DP-SARS-CoV-2.fasta`)
3. Configure preprocessing options in the standardization dialog

![image-20250629215748815](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292157869.png)

#### Preprocessing Settings

Specify the following:

- **Delimiter**: Character to split sequence IDs (e.g., `|`)
- **Field positions**:
  - 0: Sequence name
  - 1: Discrete trait (e.g., lineage)
  - 2: Continuous trait (e.g., collection date)
  - 3: Quantity (e.g., sample count)
  - 4: Organism or species name

#### Sequence Management

After preprocessing, a management interface displays key metadata:

- Sequence ID, Name, and Sequence
- Discrete and continuous traits
- Quantity
- Organism

**Tip**: Use checkboxes to include or exclude specific sequences from downstream analysis.

![image-20250629220131412](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292201462.png)

### Constructing a Haplotype Network

#### Algorithm Selection

Three supported algorithms:

- **TCS** (Statistical Parsimony)
- **MJN** (Median Joining)
- **MSN** (Minimum Spanning)

This tutorial uses TCS as an example.

#### Steps

1. Click 【Analyze > TCS Haplotype Network】 in the menu bar
2. NetST will automatically perform:
   - Sequence alignment
   - Haplotype calling
   - Network construction
   - Visualization

#### Interface Overview

![image-20250630002738023](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506300027160.png)

The results interface window contains three parts:

* **Left panel**: Lists samples and haplotypes, including trait categories
* **Center panel**: Network graph with dual-trait visualization
  * Outer ring: discrete traits
  * Inner ring: continuous traits
* **Right panel**: Layout controls (typically no adjustment needed)

### Visualization and Export

A toolbar above the network graph allows for interactive adjustments:

![image-20250704221114652](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507042211822.png)

Options include:

- **Toggle Legend**
- Style selection: Switch between discrete or continuous trait views

![未标题-1](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507042228483.jpg)

- **Save SVG**: Save high-resolution vector image

### Downstream Analysis

![image-20250702001748773](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507020017940.png)

In addition to basic network construction, NetST offers advanced analytics:

- **Network Topology Analysis**: Automatically calculates centrality, connectivity, and other indicators, identifying potentially important nodes and edges
- **Modularity Analysis**: Uses modularity to evaluate community detection
- **Visual Community**: Performs community detection based on user-defined K values
- **Sequence Analysis**: Evaluates sequence conservation and genetic distance distribution
- **Population Statistics**: Calculates population genetics indicators
- **Dual-Trait Correlation Analysis**: Dual-trait statistical testing and potential functional haplotype identification

**Usage**: Run these via the 【Analyze】 menu. Results will be saved and displayed in the **Report** interface.

### Results Management

#### File Output

- All result files are saved in the `history` subdirectory within the NetST folder

#### View Results

- Go to the **Report** tab to access all analysis outputs, summaries, and visualization exports

![image-20250702111009057](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507021110180.png)

## 5. Interface Overview

![The-Interface-of-NetST](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292123757.png)

NetST’s GUI includes:

- **Top menus**: File, Edit, Analyze, Browse, Tools
- **Tabs**:
  - **Main**: Visualization canvas
  - **Sequences**: Sequence metadata
  - **Report**: Summary of analyses
  - **Information**: Logs and process messages

### Sub-interface Function

#### **Main**

The central workspace of NetST, primarily used to display the haplotype network visualization and analysis history.

![image-20250722205601073](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507222056982.png)

![image-20250722205738068](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507222057580.png)

#### **Sequences**

Provides comprehensive sequence data management, including importing, editing, viewing, and organizing sequences. Users can manage multiple datasets within this interface and access trait metadata for each sequence.

![image-20250722205843765](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507222058376.png)

#### **Report**

Presents the detailed results of haplotype network analysis, including network topology metrics, community detection outcomes, and trait association analysis. 

![image-20250722210109831](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507222101085.png)

#### **Information**

Displays real-time logs and status updates during network construction, visualization, and analysis, allowing users to track processes and troubleshoot issues.

![image-20250722210251095](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507222102330.png)

### Menu Bar Description

#### 【File】 Menu Bar

![image-20250722184619472](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507221846541.png)

The **File** menu provides essential functions for data import and export, including:

- **Load Sequences**: Import FASTA-formatted sequence files
- **Load Table**: Load structured CSV data tables
- **Add Sequences**: Append additional sequences to the current project
- **Save Table**: Export the current data table as a CSV file
- **Save Sequences**: Export sequence data in FASTA format

**FASTA Format**: Each sequence entry consists of a header line and nucleotide string. The header uses a pipe-delimited (`|`) format to encode metadata, such as ID, traits, and sample source:

```txt
>DP0005|DP|54|1|Human
TTTCTGGCCCCCCCCTCGGCCGATTCCGCCGCGTAGCAACCGCCGCTGCACCCAGCGCTCGCTCGATTCCGAGCCACAAGCGCGGGCGTCCGAAGCCGGGACTCCCGCCGCGTCTG
```

**CSV Format**: A structured table where rows represent samples and columns represent sequence data and trait values:

| ID   | Name   | Sequence     | Discrete Traits | Continuous Traits | Quantity | Organism |
| ---- | ------ | ------------ | --------------- | ----------------- | -------- | -------- |
| 1    | DP0005 | TTTCTGGCCCC… | DP              | 54                | 1        | Human    |
| 2    | DP0027 | TTTCTGGCCCC  | DP              | 54                | 1        | Human    |

#### 【Edit】 Menu

![image-20250722204051739](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507222040282.png)

Provides batch editing and data management features within the **Sequences** tab, supporting unified metadata modification and table operations.

#### 【Analyze】 Menu

![image-20250722204332136](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507222043342.png)

NetST’s main analysis module, combining three core functionalities:

##### Haplotype Network Construction

Offers three widely used algorithms for constructing haplotype networks, suitable for different biological and evolutionary scenarios:

**MSN (Minimum Spanning Network)**

- **Ideal for**: Closely related sequences and populations with low genetic divergence
- **Advantages**: Builds the simplest connectivity structure, suitable for intra-species or population-level phylogenetic analysis
- **Use cases**: Population structure inference, recent evolutionary history

**MJN (Median-Joining Network)**

- **Ideal for**: Datasets with recombination events or missing data
- **Advantages**: Accommodates reticulate evolutionary patterns and infers ancestral haplotypes
- **Use cases**: Complex evolutionary reconstructions, ancient DNA, viral dynamics

**TCS (Statistical Parsimony)**

- **Ideal for**: Intraspecific phylogeographic analyses
- **Advantages**: Uses a 95% connection threshold to ensure statistically reliable links
- **Use cases**: Geographic structure exploration, shallow-level divergence analysis

##### Multiple Sequence Alignment

NetST supports both **MAFFT** and **MUSCLE**, two leading MSA algorithms. Alignment can be previewed, edited, or batch-processed.

**Note**: This function is optional for most users. NetST automatically performs internal alignment during haplotype construction, but this module is useful for visual inspection or fine-tuning alignments where necessary.

##### Haplotype Network Analysis

Provides comprehensive network analysis and statistical testing modules, including:

**Community Detection**

- **Modularity Analysis**: Quantifies the strength of community structure using Newman’s Q value
- **GN Algorithm**: Detects communities using the enhanced Girvan–Newman method
- **Topological Metrics**: Calculates centrality, betweenness, and node connectivity

**Sequence Analysis**

- **Conservation**: Identifies highly conserved or variable nucleotide sites
- **Distance Metrics**: Supports P-distance, Jukes-Cantor, and K2P models
- **PCA**: Visualizes haplotype relationships via principal component analysis

**Population Genetics**

- **Hd**: Haplotype diversity
- **S**: Number of segregating sites
- **π**: Nucleotide diversity
- **θw**: Watterson’s estimator
- **Tajima's D**: Neutrality test for demographic history or selection

**Trait Association Analysis**

- **Dual-trait Visualization**: Simultaneously maps continuous and categorical traits
- **Statistical Tests**: Performs ANOVA or Kruskal-Wallis depending on data normality
- **Functional Haplotype Detection**: Ranks haplotypes based on a composite scoring system to highlight candidates with potential phenotypic relevance

## 6. Methods and Algorithms

### Improved Girvan-Newman Community Detection Algorithm

The Girvan–Newman (GN) algorithm is a hierarchical clustering method based on graph edge removal, widely applied in the detection of community structures within complex networks. It operates by calculating the **edge betweenness centrality**—a measure of how often an edge appears on shortest paths between nodes—and iteratively removing the most central edges to reveal the network’s modular structure.

In haplotype networks, each edge typically represents a mutational step between haplotypes, acting as a bridge between evolutionary units. While the GN algorithm is structurally suitable for analyzing such networks, it does not account for the biological distances (e.g., nucleotide divergence) inherent in genetic data.

To address this limitation, NetST incorporates an **improved GN algorithm** that integrates **genetic distance weighting**, ensuring that biologically significant edges—such as those connecting highly divergent haplotypes—are prioritized for removal. This dual-criterion approach enhances the biological interpretability of detected communities.

#### Core Concept

The modified GN algorithm combines:

- Topological importance (via classic edge betweenness), and
- Biological relevance (via nucleotide divergence-based weighting)

This hybrid measure more accurately identifies meaningful partitions in haplotype networks, especially when ancient divergence or recombination events are present.

#### **Algorithm Steps**

**Step 1: Calculate Standard Edge Betweenness**

Treat the network as an unweighted graph and compute edge betweenness for each edge:
$$
B(i, j) = \sum_{s \ne t} \frac{\sigma_{st}(i, j)}{\sigma_{st}}
$$
Where:

- $\sigma_{st}$ is the total number of shortest paths from node $s$ to node $t$
- $\sigma_{st}(i, j)$ is the number of those paths that pass through edge $(i, j)$

**Step 2: Apply Genetic Distance Weighting**

ntroduce a genetic distance-based weight:
$$
B_w(i, j) = \frac{B(i, j)}{w_{ij} + \varepsilon}
$$
Where:

- $w_{ij} = e^{-d_{ij}/\sigma}$ is the weight of edge $(i, j)$
- $d_{ij}$ is the number of nucleotide differences between nodes $i$ and $j$
- $\sigma$ is a scaling factor
- $\varepsilon$ is a small constant to avoid division by zero

This weighting ensures that edges between highly divergent haplotypes (low similarity) receive higher $B_w$ values and are thus more likely to be removed.

**Step 3: Remove Edge with Highest Weighted Betweenness**

Remove the edge with the highest $B_w(i, j)$ value.

**Step 4: Recompute Betweenness**

Update betweenness scores for the modified network.

**Step 5: Iterative**

Repeat steps 2–4 until the desired number of communities is reached or a stopping criterion is met.

### Modularity-Based Community Evaluation

To assess the statistical significance and biological relevance of community structures, NetST computes **Newman’s modularity index (Q)**. This metric quantifies the strength of community divisions by comparing the observed edge density within communities to that expected in a random network.

The modularity score is calculated as:
$$
Q = \frac{1}{2m} \sum_{ij} \left[ A_{ij} - \frac{k_i k_j}{2m} \right] \delta(c_i, c_j)
$$
Where:

- $A_{ij}$ is the adjacency matrix (1 if nodes $i$ and $j$ are connected, 0 otherwise)
- $k_i$ and $k_j$ are the degrees of nodes $i$ and $j$
- $m$ is the total number of edges in the network
- $c_i$ and $c_j$ denote the community assignments of nodes $i$ and $j$
- $\delta(c_i, c_j)$ is the Kronecker delta (1 if $c_i = c_j$, otherwise 0)

#### Significance Threshold

A modularity value $Q > 0.3$ is generally considered indicative of a statistically significant community structure, supporting the existence of non-random groupings.

#### Practical Considerations

While maximizing $Q$ can help uncover structure, overly high values may result from **over-partitioning**, which can yield biologically meaningless micro-communities. Therefore, NetST recommends a **threshold-based validation** strategy:

- **Do not chase the absolute highest Q value**
- **Focus on whether the modularity of a user-defined partition exceeds 0.3**

This approach ensures statistical robustness while preserving biological interpretability.

#### Best Practices

To optimize community detection:

1. **Predefine community numbers** based on biological context or research goals
2. **Verify modularity** of the resulting partition ($Q > 0.3$)
3. **Interpret communities in context** (e.g., geography, phenotype)
4. **Iterate as needed**, adjusting parameters to balance statistical and biological considerations

### Dual-trait Association Analysis

NetST includes a built-in module for testing the relationship between **discrete** (e.g., lineage, region) and **continuous** traits (e.g., collection date, phenotype) associated with each haplotype.

#### Analysis Strategy

1. **Group samples** by discrete traits
2. **Compare continuous trait values** between groups
3. **Perform statistical testing**
4. **Evaluate effect size** to determine biological relevance

#### Statistical Tests

NetST automatically selects the appropriate test based on trait distribution:

```
If trait data is normally distributed:
    → Perform one-way ANOVA
Else:
    → Use Kruskal–Wallis non-parametric test
```

#### Effect Size

To quantify biological relevance, NetST calculates **eta squared** ($\eta^2$):
$$
\eta^2 = \frac{\text{Between-group variance}}{\text{Total variance}}
$$
Interpretation:

- $\eta^2$ = 0.01–0.06 → small effect
- $\eta^2$ = 0.06–0.14 → medium effect
- $\eta^2$ ≥ 0.14 → large effect

Only results with both $p < 0.05$ and $\eta^2 > 0.06$ are considered **significantly associated**.

#### Functional Haplotype Scoring

To identify biologically important haplotypes, NetST uses a **composite scoring system** to evaluate enrichment patterns within trait-defined groups.

##### Scoring Criteria (12-point total)

| Dimension            | Score Range | Description                                         |
| -------------------- | ----------- | --------------------------------------------------- |
| Sample size (power)  | 0–3         | Larger haplotype groups score higher                |
| Trait specificity    | 0–4         | Degree of association with a particular trait group |
| Phenotypic deviation | 0–3         | Divergence of continuous traits from the group mean |
| Enrichment fold      | 0–2         | Observed vs. expected frequency                     |

* **Score ≥ 6**: Indicates likely functional importance

#### Output Files

The dual-trait analysis module generates the following files:

- **Descriptive Summary**: Overall data statistics and distribution plots
- **Group Comparison Report**: Trait means, test results, p-values
- **Haplotype Score Table**: Scores for all haplotypes
- **Functional Candidate List**: Haplotypes with score ≥ 6
- **Final Report**: Integrated summary of methods, findings, and interpretations

## 7. Citation

If using NetST in your research, please cite:

> Zhang Z, Yu Y. NetST: An integrated software for large-scale haplotype network construction, visualization, and automated analytics.

Also cite the following dependencies:

- Chi L, Zhang X, Xue Y, Chen H. 2023. *fastHaN: a fast and scalable program for constructing haplotype network for large‐sample sequences*. Mol Ecol Resour. https://doi.org/10.1111/1755-0998.13829
- Múrias Dos Santos A et al. 2016. *tcsBU: a tool to extend TCS network layout and visualization*. Bioinformatics 32:627–628.
- Nakamura T et al. 2018. *Parallelization of MAFFT for large-scale multiple sequence alignments*. Bioinformatics 34:2490–2492.

## 8. Contact Us

For any questions, suggestions, or comments about NetST, please contact: [yyu@scu.edu.cn](mailto:yyu@scu.edu.cn), [zzhen0302@163.com](mailto:zzhen0302@163.com).