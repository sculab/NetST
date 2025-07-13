# Haplotype Network Analysis using NetST

## 1. Software Preparation

### 1.1 Download and Installation of NetST

1. Visit the NetST [GitHub release page](https://github.com/sculab/NetST/releases) to download the latest version
2. Double-click `NetST.exe` to run the program (no installation required)
3. **Note**: Please make sure to run the program within the NetST folder, as all required dependencies are packaged within this directory

![The-Interface-of-NetST](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292123757.png)

### 1.2 Input Data Format

NetST accepts sequence data with sample metadata (traits), supporting both FASTA and CSV formats.

This tutorial provides two example datasets, accessible in the Examples folder:

1. **SL-SARS-CoV-2**
   * Contains 130 major haplotypes
   * Based on L and S sublineages from 121,618 viral genomes
   * Uses sequence sublineage as discrete traits

2. **DP-SARS-CoV-2**
   * Contains 482 SARS-CoV-2 genome sequences
   * 72 isolates from Diamond Princess cruise ship + 410 samples from various countries and regions worldwide
   * Uses location as discrete traits and collection time as continuous traits

## 2. Data Import and Preprocessing

### 2.1 Data Loading

1. Open the menu bar [File > Load Sequences]
2. Select the data file (e.g., Examples/DP-SARS-CoV-2/DP-SARS-CoV-2.fasta)
3. Configure preprocessing settings in the standardization window that appears

![image-20250629215748815](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292157869.png)

### 2.2 Data Preprocessing Settings

In the preprocessing window, configure the following parameters:

* **Delimiter**: Character used to split sequence IDs (e.g., `|`)
* Position settings:
   * Position 0: New sequence name
   * Position 1: Discrete traits (classification reference)
   * Position 2: Continuous traits
   * Position 3: Quantity information
   * Position 4: Organism name

### 2.3 Sequence Management

After preprocessing, the system displays a sequence management window containing:

* ID, Name, Sequence
* Discrete Traits
* Continuous Trait
* Quantity
* Organism

**Tip**: Use checkboxes to select specific sequences for network construction

![image-20250629220131412](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292201462.png)

## 3. Haplotype Network Construction

### 3.1 Algorithm Selection

NetST integrates three commonly used haplotype network construction algorithms:

* **MSN** (Minimum Spanning Network)
* **MJN** (Median Joining Network)
* **TCS** (Templeton-Crandall-Sing)

This tutorial uses TCS haplotype network construction as an example to demonstrate NetST's simple and user-friendly network construction approach.

### 3.2 Network Construction Steps

1. Select the menu and click [Analyze > TCS Haplotype Network]
2. NetST will automatically execute the following steps:
   * Multiple sequence alignment
   * Haplotype sequence calculation
   * Haplotype network construction
   * Haplotype network visualization

### 3.3 Results Interface

![image-20250630002738023](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506300027160.png)

The main window contains three sections:

1. **Left Panel**: Haplotype information
   * Sample names
   * Haplotypes
   * Discrete traits and their classification colors

2. **Middle Panel**: Network visualization main body
   * Employs dual-trait visualization strategy
   * Nodes displayed in concentric circles
   * Outer ring: Discrete traits (e.g., location information)
   * Inner ring: Continuous traits (e.g., collection time)

3. **Right Panel**: Force-directed layout settings
   * Generally no adjustment needed

## 4. Graph Enhancement and Export

### 4.1 Visualization Adjustments

The visualization window contains multiple functional buttons for network graph visualization adjustment and enhancement:

![image-20250704221114652](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507042211822.png)

Commonly used functions include:

* **Legend**: Show/hide legend
* **Style selection**:
   * Continuous traits mode
   * Discrete traits mode

![未标题-1](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507042228483.jpg)

* **Save SVG**: Export as SVG vector format

### 4.2 Export Options

* Supports SVG format export
* Maintains high-quality and editable graphics

## 5. Advanced Analysis Functions

![image-20250702001748773](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507020017940-20250704235203081.png)

NetST provides numerous advanced analysis functions alongside basic haplotype network construction and visualization capabilities, including:

* **Network Topology Analysis**
* **Community Analysis**
* **Modularity Analysis**
* **Visual Community**
* **Sequence Analysis**
* **Population Statistics**
* **Dual-Trait Correlation Analysis**

**Usage**: After completing network construction, select the corresponding analysis function through the **Analyze** menu bar

## 6. Results Management

### 6.1 Result File Location

* Result files are saved in the `history` directory within the NetST folder
* Users can directly access this folder to view analysis results

### 6.2 Result Viewing

* View organized and visualized results in the software's **Report** page
* Provides convenient result browsing and management functions

![image-20250702111009057](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507021110180.png)

## Quick Start Guide

1. **Prepare Data**: Ensure sequence data contains necessary metadata
2. **Load Data**: Import FASTA or CSV files through the menu
3. **Preprocess**: Set correct delimiters and position parameters
4. **Build Network**: Select TCS algorithm to construct haplotype network
5. **Enhance Graphics**: Adjust legend, style, and other visualization options
6. **Export Results**: Save as SVG format for future use
7. **Advanced Analysis**: Perform topology analysis, community detection, etc., as needed

**Tip**: We recommend starting with the example datasets to familiarize yourself with the software workflow before processing your own data.
