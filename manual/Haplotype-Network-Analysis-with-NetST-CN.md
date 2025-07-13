# Haplotype Network Analysis using NetST

## 1. 软件准备

### 1.1 下载和安装NetST

1. 访问[NetST GitHub发布页面](https://github.com/sculab/NetST/releases)下载最新版本
2. 双击`NetST.exe`即可运行（无需安装）
3. **注意**：请务必在NetST文件夹中运行程序，因为所需依赖已打包在该文件夹中

![The-Interface-of-NetST](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292123757.png)

### 1.2 输入数据格式

NetST接受带有样本元信息（trait ）的序列数据，支持fasta和csv格式。

本教程准备了两个示例数据集，可以在[Examples](../Examples)中访问：

1. SL-SARS-CoV-2
   * 包含130个主要单倍型
   * 基于121,618个病毒基因组的L和S亚系
   * 以序列的sublineage作为离散性状
2.  DP-SARS-CoV-2
   * 包含482个SARS-CoV-2基因组序列
   * 72个钻石公主号邮轮样本 + 410个全球各地样本
   * 以location作为离散性状，采集时间作为连续性状

## 2. 数据导入和预处理

### 2.1 载入数据

1. 打开菜单栏【文件 > 载入序列】
2. 选择数据文件（如：Examples/DP-SARS-CoV-2/DP-SARS-CoV-2.fasta）
3. 在弹出的标准化窗口中进行预处理

![image-20250629215748815](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292157869.png)

### 2.2 数据预处理设置

在预处理窗口中，需要配置以下参数：

- **分隔符**：用于拆分序列ID的字符（如`|`）
- 位置设置
  - 0号位置：新序列名
  - 1号位置：离散性状（分类参考）
  - 2号位置：连续性状
  - 3号位置：数量信息
  - 4号位置：物种名

### 2.3 序列管理

完成预处理后，系统会显示序列管理窗口，包含以下信息：

- ID、Name、Sequence
- Discrete Traits（离散性状）
- Continuous Trait（连续性状）
- Quantity（数量）
- Organism（物种）

**提示**：可通过复选框选择特定序列参与网络构建

![image-20250629220131412](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506292201462.png)

## 3. 构建单倍型网络

### 3.1 算法选择

NetST集成了三种常用的单倍型网络构建算法：

* **MSN**（Minimum Spanning Network）
* **MJN**（Median Joining Network）
* **TCS**（Templeton-Crandall-Sing）

在本教程中将以TCS单倍型网络构建为例，展示NetST简单易用的单倍型网络构建方式。

### 3.2 网络构建步骤

1. 选择菜单，点击【Analyze > TCS Haplotype Network 】
2. NetST将自动执行以下步骤
   * 多序列比对（multiple sequence alignment）
   * 单倍型序列计算（haplotype sequence calculation）
   * 单倍型网络构建（haplotype network construction）
   * 单倍型网络可视化（haplotype network visualizatio）

### 3.3 结果界面

![image-20250630002738023](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202506300027160.png)

主窗口包含三个部分：

1. 左侧面板：单倍型信息
   - 样本名称
   - 单倍型
   - 间断性状及其分类颜色
2. 中间面板：网络可视化主体
   - 采用双性状可视化策略
   - 节点以同心圆方式展示
   - 外环：离散性状（如位置信息）
   - 内环：连续性状（如采集时间）
3. 右侧面板：力导向布局设置
   - 一般情况下无需调整

## 4. 图形美化和导出

### 4.1 可视化调整

在可视化窗口上方包含多个功能按钮，可以用于网络图的可视化调整和美化：

![image-20250704221114652](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507042211822.png)

其中较为常用的功能：

- **图例**：显示/隐藏图例

- 风格选择

  - 连续性状模式

  - 间断性状模式

![未标题-1](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507042228483.jpg)

- **Save SVG**：保存为SVG矢量图格式

### 4.2 导出选项

- 支持SVG格式导出
- 保持图形的高质量和可编辑性

## 5. 高级分析功能

![image-20250702001748773](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507020017940.png)

NetST 在提供基础的单倍型网络构建和可视化功能的同时，具有众多高级分析功能，包括：

- **网络拓扑分析**（Network Topology Analysis）

- **社区分析**（Community Analysis）

- **模块度分析**（Modularity Analysis）
- **社区可视化**（Visual Community）
- **序列分析**（Sequence Analysis）
- **群体统计**（Population Statistics）
- **性状关联分析**（Dual-Trait Correlation Analysis）

**使用方法**：在完成网络构建后，通过**Analyze**菜单栏选择相应的分析功能

## 6. 结果管理

### 6.1 结果文件位置

- 结果文件保存在NetST文件夹的`history`目录中
- 用户可直接访问该文件夹查看分析结果

### 6.2 结果查看

- 在软件的**Report**页面中可查看整理后的可视化结果
- 提供便捷的结果浏览和管理功能

![image-20250702111009057](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202507021110180.png)

