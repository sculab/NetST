# 教程1：构建单倍型网络

**总结：介绍NetST核心功能之一——构建单倍型网络。本教程描述了如何使用NetST处理带有离散性状和连续性状的序列数据，并构建单倍型网络进行分析。通过本教程您将学会如何导入数据、选择算法和生成单倍型网络图的过程。**

本教程将指引您通过运行NetST来做一个简单的分析。如果您还没有下载并安装NetST，可以通过以下网址下载：

[sculab/NetST (github.com)](https://github.com/sculab/NetST)

## 首次运行NetST

NetST软件具有命令行版本和图形界面版本。对于Linux和MacOS用户，我们提供了命令行版本的NetST用于使用，而对于Windows用户，我们开发了更加友好易用的图形界面。本教程将只显示Windows版本的NetST的使用方式。

### 运行NetST

对于Windows用户，可以在[https://github.com/sculab/NetST/releases](https://github.com/sculab/NetST/releases)中下载NetST-GUI的软件压缩包，解压缩后在压缩文件夹中可以看到NetST软件相关文件。

通过双击NetST.exe的图标即可运行NetST。当你运行后，你会看到这样一个窗口：

![image-20230820150248051](https://github.com/sculab/NetST/blob/master/main/screen.png)

您需要做的第一件事是加载带有离散性状和连续性状的序列数据，可以是fasta格式或者csv格式。在本教程中，所有的文件都存储在DEMOS/DEMO_01文件夹下。DEMOS/DEMO_01文件夹下包含一个名为TEST_HA.fasta的fasta格式序列文件和一个名为HA_REFS.csv的csv格式的序列文件。两份文件的内容一致，仅格式不同。

NOTE：

打开菜单栏【文件 > 载入序列】并导航到DEMOS/DEMO_01/TEST_HA.fasta并选择它。

NOTE：

### 载入序列

当加载序列后，您会看到下面这个窗口：

![image-20230820165309281](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202308201653345.png)

该窗口可用于获取序列相关信息：间断性状、连续性状、数量信息以及样本信息。您可以通过使用指定的分隔符对序列ID（即fasta格式序列的第一行的信息）进行拆分，并通过指定序列ID拆分后的位置来指定不同的信息。对于提供的序列数据，通过将`|`作为分隔符，0号位置设置为新序列名、1号位置设置为间断性状、2号位置设置为连续性状、3号位置设置为数量信息、4号位置设置为物种名。点击窗口的确定后，将会得到下面的窗口：

![image-20230820212458490](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202308211739408.png)

注意：该窗口展示的序列的详细信息，同csv格式的文件中的信息一致。csv 中的列同样是 ID、Name、Sequence、Discrete Traits、Continuous Trait、Quantity、Organism。

注意：通过序列管理窗口，用户可以通过点击复选框，根据自己的需求，选择或取消序列。

### 构建单倍型网络

NetST提供了三种常用的构建单倍型网络的算法：MSN（minimum spanning network）、MJN（Median Joining Network）、TCS（Templeton-Crandall-Sing）。这些算法已经在多个程序中实现，例如 NETWORK、PopART、SPLITSTREE和 TCS 软件；且被广泛应用。在本教程中将以TCS单倍型网络构建为例，展示NetST简单易用的单倍型网络构建方式。

对于上述导入的序列数据和信息，通过点击【分析 > TCS单倍型网络】，NetST将在后台对数据进行【多序列比对 > 单倍型网络构建】，而后将结果展示在下面的窗口中：

![image-20230822173301524](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202308221733624.png)

上述网络图可视化窗口包括三个部分：最左边的窗口主要展示序列相关信息，包括样本的名称信息、样本的间隔性状信息及其在单倍型网络中呈现的颜色，是NetST独有的序列管理窗口；中间的窗口是NetST可视化窗口的主体部分，用于展示构建的网络图；最右边的设定窗口，同可视化网络展示过程中的动画相关，一般情况下无需调整。

窗口主体部分展示的就是一个网络图。在该单倍型网络中，每个彩色圆代表一种单倍型，圆的大小表示该单倍型包含的样本数，彩色圆中嵌套的两个小圆，分别代表间断性状（该例子中使用H1、H2、H3表示）和连续性状（在该例子中是指样本的时间），两个圆之间的连线表示两个单倍型彼此相关。另外，每个白色的圆点表示算法推断的可能存在的单倍型，但没有样品。

在可视化窗口的主体部分的上方有部分功能按钮，可以用于调整构建的单倍型网络，用于图形美化。其中最为主要的就是图例和风格。通过点击图例，将在网络图中展示不同颜色所表示的信息。

![image-20230911222547245](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202309112225290.png)

在风格中通过选择【风格 -> 连续性状】或【风格 ->间断性状】，可视化网络将修改彩色圆，只显示间断形状或者连续性状所代表的颜色。其他相关的修改功能主要用于网络的颜色或者线条调整，用户可自行探索。

![image-20230911222635989](https://cdn.jsdelivr.net/gh/plant720/TyporaPic/img/202309112226029.png)

打开【保存SVG】后，可以将构建及美化后的单倍型网络保存为SVG文件。

## Reference

Chi L, Zhang X, Xue Y, Chen H. 2023. fastHaN: a fast and scalable program for constructing haplotype network for large‐sample sequences. *Molecular Ecology Resources*:1755-0998.13829.

Múrias Dos Santos A, Cabezas MP, Tavares AI, Xavier R, Branco M. 2016. tcsBU: a tool to extend TCS network layout and visualization. *Bioinformatics* 32:627–628.

