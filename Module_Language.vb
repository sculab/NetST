﻿Imports System.Reflection.Emit
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Module Module_Language
    Public Sub to_en()
        form_main.Text = "NetST (Network for Sequences and Traits)" + " ver. " + version
        form_main.文件FToolStripMenuItem.Text = "File"
        form_main.载入序列ToolStripMenuItem.Text = "Load Sequences"
        form_main.载入数据ToolStripMenuItem.Text = "Load Table"
        form_main.增加数据ToolStripMenuItem.Text = "Add Sequences"
        form_main.保存数据ToolStripMenuItem.Text = "Save Table"
        form_main.导出序列ToolStripMenuItem.Text = "Save Sequences"
        form_main.导出分型数据集ToolStripMenuItem.Text = "Export Genotyping Dataset"
        form_main.编辑ToolStripMenuItem.Text = "Edit"
        form_main.全选ToolStripMenuItem.Text = "Select All"
        form_main.清除ToolStripMenuItem.Text = "Clear"
        form_main.分析ToolStripMenuItem.Text = "Analyze"
        form_main.分型ToolStripMenuItem.Text = "HIV Drug Resistance Analysis (Remote)"
        form_main.本地分析ToolStripMenuItem.Text = "HIV Drug Resistance Analysis (Local)"
        form_main.MSN单倍型网络ToolStripMenuItem.Text = "MSN Haplotype Network"
        form_main.MJN单倍型网络ToolStripMenuItem.Text = "MJN Haplotype Network"
        form_main.单倍型网络ToolStripMenuItem.Text = "TCS Haplotype Network"
        form_main.序列比对高速ToolStripMenuItem.Text = "Sequence Alignment (Fast)"
        form_main.序列比对ToolStripMenuItem.Text = "Sequence Alignment (Accurate)"
        form_main.基于参考序列分型ToolStripMenuItem.Text = "Quick Genotyping/Identification"
        form_main.混合分型分析ToolStripMenuItem.Text = "Mixed Typing Analysis"
        form_main.工具ToolStripMenuItem.Text = "Tools"
        form_main.分割序列文件ToolStripMenuItem.Text = "Split Sequence File"
        form_main.合并序列文件ToolStripMenuItem.Text = "Merge Sequence Files"
        form_main.清理序列ToolStripMenuItem.Text = "Clean Sequence Data"
        form_main.获取序列信息ToolStripMenuItem.Text = "Generate Sequence Table"
        form_main.CSV生成序列ToolStripMenuItem.Text = "Build Genotyping Dataset"
        form_main.日期转换数字ToolStripMenuItem.Text = "Date to Number Conversion"
        form_main.浏览ToolStripMenuItem.Text = "Browse"
        form_main.前进ToolStripMenuItem.Text = "Forward"
        form_main.后退ToolStripMenuItem.Text = "Backward"
        form_main.分析记录ToolStripMenuItem.Text = "View Records"
        form_main.EnglishToolStripMenuItem.Text = "中文"
        form_main.TabControl1.TabPages(0).Text = "Results"
        form_main.TabControl1.TabPages(1).Text = "Sequences"
        form_main.TabControl1.TabPages(2).Text = "Information"
        form_main.分型引物构建ToolStripMenuItem.Text = "Design Genotyping Primer"

        form_config_stand.Text = "Standardization"
        form_config_stand.CheckBox1.Text = "Use Unix line endings"
        form_config_stand.CheckBox2.Text = "Split names using:"
        form_config_stand.CheckBox6.Text = "Split names using:"
        form_config_stand.CheckBox7.Text = "Split names using:"
        form_config_stand.CheckBox8.Text = "Split names using:"
        form_config_stand.CheckBox9.Text = "Split names using:"
        form_config_stand.Label1.Text = "Use as the new name"
        form_config_stand.CheckBox5.Text = "Remove seq with ambiguous bases"
        form_config_stand.CheckBox3.Text = "Replace"
        form_config_stand.Label2.Text = "->"
        form_config_stand.CheckBox4.Text = "Use numbering as seq names"
        form_config_stand.Button1.Text = "OK"
        form_config_stand.Button2.Text = "Cancel"
        form_config_stand.Label3.Text = "Use as discrete trait"
        form_config_stand.Label5.Text = "Preview Names:"
        form_config_stand.Label4.Text = "Use as counts"
        form_config_stand.Label6.Text = "Use as continuous trait"
        form_config_stand.Label7.Text = "Split Results:"
        form_config_stand.Label8.Text = "Use as organism"

        form_config_primer.Label1.Text = "Maximum Primer per Segment"
        form_config_primer.Label2.Text = "Minimum Gene Length"
        form_config_primer.Label3.Text = "Maximum Gene Length"
        form_config_primer.Label4.Text = "Extension Boundary Length"
        form_config_primer.Label5.Text = "Maximum Marker Length"
        form_config_primer.Label6.Text = "Minimum Marker Length"
        form_config_primer.Label7.Text = "Ladder Resolution"
        form_config_primer.Label8.Text = "Number of Threads"
        form_config_primer.CheckBox1.Text = "Prioritize Primer in Conserved Regions"
        form_config_primer.RadioButton1.Text = "Distinguish by ID"
        form_config_primer.RadioButton2.Text = "Distinguish by State"
        form_config_primer.Button1.Text = "OK"
        form_config_primer.Button2.Text = "Cancel"
        form_config_primer.Button3.Text = "Save to"
        form_config_primer.Text = "Primer Design"

        form_config_split.RadioButton2.Text = "Distinguish by Organism+State"
        form_config_split.RadioButton1.Text = "Distinguish by Organism+ID"
        form_config_split.Button2.Text = "Cancel"
        form_config_split.Button1.Text = "OK"
        form_config_split.Button3.Text = "Save to"
        form_config_split.Label4.Text = "Extension Boundary Length"
        form_config_split.Label3.Text = "Maximum Gene Length"
        form_config_split.Label2.Text = "Minimum Gene Length"
        form_config_split.Text = "Split Sequences"


        form_config_clean.Text = "Clean"
        form_config_clean.CheckBox1.Text = "Use Unix line endings"
        form_config_clean.CheckBox2.Text = "Remove seq with ambiguous bases"
        form_config_clean.CheckBox3.Text = "Remove sew shorter than:"
        form_config_clean.CheckBox4.Text = "Remove seq with N percentage above:"
        form_config_clean.CheckBox5.Text = "Replace ambiguous bases with gap(-)"
        form_config_clean.CheckBox6.Text = "Replace in seq name:"
        form_config_clean.Button1.Text = "OK"
        form_config_clean.Button2.Text = "Cancel"
        form_config_clean.CheckBox7.Text = "Remove in sequence name:"
        form_config_clean.CheckBox8.Text = "Keep only seqs with the following in name:"
        form_config_clean.Label1.Text = "Input sequences:"
        form_config_clean.Label5.Text = "Preview Names:"
        form_config_clean.Label2.Text = "with"
        form_config_clean.Button3.Text = "Browse"

        form_config_data.Text = "Build Genotyping Dataset"
        form_config_data.Button1.Text = "Browse"
        form_config_data.Label1.Text = "Sequence Name:"
        form_config_data.Label2.Text = "Trait/Genotype:"
        form_config_data.Label3.Text = "Sequence:"
        form_config_data.Button2.Text = "Save"
        form_config_data.Button3.Text = "Cancel"
        form_config_data.Button4.Text = "OK"
        form_config_data.Button5.Text = "Browse"
        form_config_data.CheckBox1.Text = "Remove Duplicate Sequences"
        form_config_data.CheckBox2.Text = "Remove Sequences with Ambiguous Bases"
        form_config_data.RadioButton1.Text = "From csv File:"
        form_config_data.RadioButton2.Text = "From fasta File:"

        form_config_mix.Text = "Mixture Genotyping Detection"
        form_config_mix.RadioButton1.Text = "Based on Existing Results"
        form_config_mix.RadioButton2.Text = "Analyze use References"
        form_config_mix.Label1.Text = "File:"
        form_config_mix.Label2.Text = "Reference Dataset:"
        form_config_mix.Label3.Text = "K:"
        form_config_mix.Button1.Text = "Browse"
        form_config_mix.Button2.Text = "Browse"
        form_config_mix.Button3.Text = "Browse"
        form_config_mix.Label4.Text = "Result Folder:"
        form_config_mix.Label5.Text = "Genotype Threshold:"
        form_config_mix.Button4.Text = "Cancel"
        form_config_mix.Button5.Text = "OK"
        form_config_mix.Label6.Text = "Maximum Sequences:"
        form_config_mix.Label7.Text = "Threads:"

        form_config_type.Text = "Quick Genotyping"
        form_config_type.RadioButton1.Text = "Use Custom Reference Sequence"
        form_config_type.RadioButton2.Text = "Use Custom K-mer dictionary"
        form_config_type.Button2.Text = "Cancel"
        form_config_type.Button1.Text = "OK"
        form_config_type.RadioButton3.Text = "Use Built-in K-mer dictionary"
        form_config_type.Label1.Text = "K:"
        form_config_type.Button3.Text = "Browse"
        form_config_type.Button4.Text = "Browse"

        form_config_combine.Text = "Merge Sequence Files"
        form_config_combine.Button5.Text = "Browse"
        form_config_combine.Button3.Text = "Cancel"
        form_config_combine.Button4.Text = "OK"
        form_config_combine.Button1.Text = "Browse"
        form_config_combine.Label1.Text = "Location of Files to be Merged:"
        form_config_combine.Label2.Text = "Extension to be Merged:"
        form_config_combine.Label3.Text = "Save Merged File to:"
        form_config_combine.Label4.Text = "Fill missing sequences:"
        form_config_combine.RadioButton1.Text = "Merge by file"
        form_config_combine.RadioButton2.Text = "Merge by species"


        form_config_clean.CheckBox1.Text = "Use Unix Line Endings"
        form_config_clean.CheckBox2.Text = "Remove Seqs Containing Ambiguous Bases"
        form_config_clean.CheckBox3.Text = "Remove Seqs Shorter than:"
        form_config_clean.CheckBox4.Text = "Remove Seqs with N Content Higher than:"
        form_config_clean.CheckBox5.Text = "Replace Ambiguous Bases with Gap(-)"
        form_config_clean.Button1.Text = "OK"
        form_config_clean.Button2.Text = "Cancel"
        form_config_clean.CheckBox7.Text = "Remove Seqs with Names Containing:"
        form_config_clean.CheckBox8.Text = "Keep Only Seqs with Names Containing:"
        form_config_clean.Label1.Text = "Input Sequences:"
        form_config_clean.Label2.Text = "with"
        form_config_clean.Label5.Text = "Name Preview:"
        form_config_clean.CheckBox6.Text = "Replace Characters in Seq Names"
        form_config_clean.Text = "Sequence Cleanup"


        If data_type = "gb" Then
            form_config_primer.Label2.Text = "Minimum Gene Length"
            form_config_primer.Label3.Text = "Maximum Gene Length"
            form_config_split.Label2.Text = "Minimum Gene Length"
            form_config_split.Label3.Text = "Maximum Gene Length"
        End If
        If data_type = "fas" Then
            form_config_primer.Label2.Text = "Fragment Overlap Length"
            form_config_primer.Label3.Text = "Length"
            form_config_split.Label2.Text = "Fragment Overlap Length"
            form_config_split.Label3.Text = "Fragment Separation Length"
            form_config_split.CheckBox1.Text = "Align sequences"
        End If

        language = "EN"

    End Sub

    Public Sub to_ch()
        form_main.Text = "NetST 序列和性状网络分析" + " ver." + version
        form_main.文件FToolStripMenuItem.Text = "文件"
        form_main.载入序列ToolStripMenuItem.Text = "载入序列"
        form_main.载入数据ToolStripMenuItem.Text = "载入表格"
        form_main.增加数据ToolStripMenuItem.Text = "增加序列"
        form_main.保存数据ToolStripMenuItem.Text = "保存表格"
        form_main.导出序列ToolStripMenuItem.Text = "保存序列"
        form_main.导出分型数据集ToolStripMenuItem.Text = "导出分型"
        form_main.编辑ToolStripMenuItem.Text = "编辑"
        form_main.全选ToolStripMenuItem.Text = "全选"
        form_main.清除ToolStripMenuItem.Text = "清除"
        form_main.分析ToolStripMenuItem.Text = "分析"
        form_main.分型ToolStripMenuItem.Text = "HIV耐药性分析(远程)"
        form_main.本地分析ToolStripMenuItem.Text = "HIV耐药性分析(本地)"
        form_main.MSN单倍型网络ToolStripMenuItem.Text = "MSN单倍型网络"
        form_main.MJN单倍型网络ToolStripMenuItem.Text = "MJN单倍型网络"
        form_main.单倍型网络ToolStripMenuItem.Text = "TCS单倍型网络"
        form_main.序列比对高速ToolStripMenuItem.Text = "序列比对(高速)"
        form_main.序列比对ToolStripMenuItem.Text = "序列比对高速(精确)"
        form_main.基于参考序列分型ToolStripMenuItem.Text = "快速分型/鉴定"
        form_main.混合分型分析ToolStripMenuItem.Text = "混合分型分析"
        form_main.工具ToolStripMenuItem.Text = "工具"
        form_main.分割序列文件ToolStripMenuItem.Text = "分割序列文件"
        form_main.合并序列文件ToolStripMenuItem.Text = "合并序列文件"
        form_main.清理序列ToolStripMenuItem.Text = "清理序列数据"
        form_main.获取序列信息ToolStripMenuItem.Text = "序列生成表格"
        form_main.CSV生成序列ToolStripMenuItem.Text = "构建分型数据集"
        form_main.日期转换数字ToolStripMenuItem.Text = "日期转换数字"
        form_main.浏览ToolStripMenuItem.Text = "浏览"
        form_main.前进ToolStripMenuItem.Text = "前进"
        form_main.后退ToolStripMenuItem.Text = "后退"
        form_main.分析记录ToolStripMenuItem.Text = "查看记录"
        form_main.EnglishToolStripMenuItem.Text = "English"
        form_main.TabControl1.TabPages(0).Text = "结果"
        form_main.TabControl1.TabPages(1).Text = "序列"
        form_main.分型引物构建ToolStripMenuItem.Text = "分型引物构建"
        form_main.TabControl1.TabPages(2).Text = "信息"

        form_config_primer.Label1.Text = "每片段最大候选引物数量"
        form_config_primer.Label2.Text = "基因最小长度"
        form_config_primer.Label3.Text = "基因最大长度"
        form_config_primer.Label4.Text = "扩展边界长度"
        form_config_primer.Label5.Text = "Marker最大长度"
        form_config_primer.Label6.Text = "Marker最小长度"
        form_config_primer.Label7.Text = "ladder分辨率"
        form_config_primer.Label8.Text = "线程数"
        form_config_primer.CheckBox1.Text = "优先使用保守区域设计引物"
        form_config_primer.RadioButton1.Text = "使用ID区分样本"
        form_config_primer.RadioButton2.Text = "使用State区分样本"
        form_config_primer.Button1.Text = "确定"
        form_config_primer.Button2.Text = "取消"
        form_config_primer.Button3.Text = "保存位置"
        form_config_primer.Text = "引物设计"

        form_config_split.Text = "分割序列"
        form_config_split.RadioButton2.Text = "使用Organism+State区分样本"
        form_config_split.RadioButton1.Text = "使用Organism+ID区分样本"
        form_config_split.Button2.Text = "取消"
        form_config_split.Button1.Text = "确定"
        form_config_split.Button3.Text = "保存位置"
        form_config_split.Label4.Text = "扩展边界长度"
        form_config_split.Label3.Text = "基因最大长度"
        form_config_split.Label2.Text = "基因最小长度"

        form_config_stand.Text = "序列标准化"
        form_config_stand.CheckBox1.Text = "使用Unix换行符"
        form_config_stand.CheckBox2.Text = "使用该分隔符拆分序列名"
        form_config_stand.CheckBox6.Text = "使用该分隔符拆分序列名"
        form_config_stand.CheckBox7.Text = "使用该分隔符拆分序列名"
        form_config_stand.CheckBox8.Text = "使用该分隔符拆分序列名"
        form_config_stand.CheckBox9.Text = "使用该分隔符拆分序列名"
        form_config_stand.Label1.Text = "将该位置作为新序列名"
        form_config_stand.CheckBox5.Text = "移除所有包含简并碱基的序列"
        form_config_stand.CheckBox3.Text = "替换序列名中"
        form_config_stand.Label2.Text = "为"
        form_config_stand.CheckBox4.Text = "使用编号作为序列的名称"
        form_config_stand.Button1.Text = "确定"
        form_config_stand.Button2.Text = "取消"
        form_config_stand.Label3.Text = "将该位置作为间断性状"
        form_config_stand.Label5.Text = "序列名预览:"
        form_config_stand.Label4.Text = "将该位置作为数量"
        form_config_stand.Label6.Text = "将该位置作为连续性状"
        form_config_stand.Label7.Text = "拆分结果预览:"
        form_config_stand.Label8.Text = "将该位置作为物种名"

        form_config_clean.Text = "清理序列"
        form_config_clean.CheckBox1.Text = "使用Unix换行符"
        form_config_clean.CheckBox2.Text = "移除所有包含简并碱基的序列"
        form_config_clean.CheckBox3.Text = "移除短于以下长度的序列"
        form_config_clean.CheckBox4.Text = "移除含N高于以下百分比的序列"
        form_config_clean.CheckBox5.Text = "将简并碱基替换为gap(-)"
        form_config_clean.CheckBox6.Text = "替换序列名中的字符"
        form_config_clean.CheckBox7.Text = "移除序列名包含以下字符的序列"
        form_config_clean.CheckBox8.Text = "只保留序列名包含以下字符的序列"
        form_config_clean.Label1.Text = "待处理序列:"
        form_config_clean.Label5.Text = "第一条序列名预览:"
        form_config_clean.Button1.Text = "确定"
        form_config_clean.Button2.Text = "取消"
        form_config_clean.Button3.Text = "浏览"

        form_config_data.Text = "构建分型数据集"
        form_config_data.Button1.Text = "浏览"
        form_config_data.Label1.Text = "序列名称:"
        form_config_data.Label2.Text = "性状/分型:"
        form_config_data.Label3.Text = "序列:"
        form_config_data.Button2.Text = "保存位置"
        form_config_data.Button3.Text = "取消"
        form_config_data.Button4.Text = "确定"
        form_config_data.Button5.Text = "浏览"
        form_config_data.CheckBox1.Text = "去除重复序列"
        form_config_data.CheckBox2.Text = "去除包含简并碱基的序列"
        form_config_data.RadioButton1.Text = "从csv文件构建:"
        form_config_data.RadioButton2.Text = "从fasta文件构建:"

        form_config_mix.Text = "混合分型检测"
        form_config_mix.RadioButton1.Text = "基于已有结果"
        form_config_mix.RadioButton2.Text = "从头开始分析"
        form_config_mix.Label1.Text = "结果文件:"
        form_config_mix.Label2.Text = "参考数据集:"
        form_config_mix.Label3.Text = "K值:"
        form_config_mix.Button1.Text = "浏览"
        form_config_mix.Button2.Text = "浏览"
        form_config_mix.Button3.Text = "浏览"
        form_config_mix.Label4.Text = "结果文件夹:"
        form_config_mix.Label5.Text = "假定混合分型阈值:"
        form_config_mix.Button4.Text = "取消"
        form_config_mix.Button5.Text = "确定"
        form_config_mix.Label6.Text = "最大单个分型序列数量:"
        form_config_mix.Label7.Text = "线程数:"

        form_config_type.Text = "快速分型"
        form_config_type.RadioButton1.Text = "使用自定义参考序列"
        form_config_type.RadioButton2.Text = "使用自定义K-mer字典"
        form_config_type.Button2.Text = "取消"
        form_config_type.Button1.Text = "确定"
        form_config_type.RadioButton3.Text = "使用内置K-mer字典"
        form_config_type.Label1.Text = "K:"
        form_config_type.Button3.Text = "浏览"
        form_config_type.Button4.Text = "浏览"

        form_config_combine.Text = "合并序列文件"
        form_config_combine.Button5.Text = "浏览"
        form_config_combine.Button3.Text = "取消"
        form_config_combine.Button4.Text = "确定"
        form_config_combine.Button1.Text = "浏览"
        form_config_combine.Label1.Text = "存放待合并文件的位置:"
        form_config_combine.Label2.Text = "待合并文件的扩展名:"
        form_config_combine.Label3.Text = "合并后文件保存在:"
        form_config_combine.Label4.Text = "缺失序列填充字符:"
        form_config_combine.RadioButton1.Text = "按文件合并"
        form_config_combine.RadioButton2.Text = "按物种合并"

        form_config_clean.CheckBox1.Text = "使用Unix换行符"
        form_config_clean.CheckBox2.Text = "移除所有包含简并碱基的序列"
        form_config_clean.CheckBox3.Text = "移除短于以下长度的序列"
        form_config_clean.CheckBox4.Text = "移除含N高于以下百分比的序列"
        form_config_clean.CheckBox5.Text = "将简并碱基替换为gap(-)"
        form_config_clean.Button1.Text = "确定"
        form_config_clean.Button2.Text = "取消"
        form_config_clean.CheckBox7.Text = "移除序列名包含以下字符的序列"
        form_config_clean.CheckBox8.Text = "只保留序列名包含以下字符的序列"
        form_config_clean.Label1.Text = "待处理序列:"
        form_config_clean.Label2.Text = "替换为"
        form_config_clean.Label5.Text = "序列名预览:"
        form_config_clean.CheckBox6.Text = "替换序列名中的字符"
        form_config_clean.Text = "序列清理"

        If data_type = "gb" Then
            form_config_primer.Label2.Text = "基因最小长度"
            form_config_primer.Label3.Text = "基因最大长度"
            form_config_split.Label2.Text = "基因最小长度"
            form_config_split.Label3.Text = "基因最大长度"
        End If
        If data_type = "fas" Then
            form_config_primer.Label2.Text = "片段重叠长度"
            form_config_primer.Label3.Text = "片段分隔长度"
            form_config_split.Label2.Text = "片段重叠长度"
            form_config_split.Label3.Text = "片段分隔长度"

        End If

        language = "CH"


    End Sub
End Module
