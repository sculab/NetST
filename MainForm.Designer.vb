<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Mainform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        WebView_main = New Microsoft.Web.WebView2.WinForms.WebView2()
        MenuStrip1 = New MenuStrip()
        文件FToolStripMenuItem = New ToolStripMenuItem()
        载入序列ToolStripMenuItem = New ToolStripMenuItem()
        载入数据ToolStripMenuItem = New ToolStripMenuItem()
        增加数据ToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator4 = New ToolStripSeparator()
        保存数据ToolStripMenuItem = New ToolStripMenuItem()
        导出序列ToolStripMenuItem = New ToolStripMenuItem()
        编辑ToolStripMenuItem = New ToolStripMenuItem()
        全选ToolStripMenuItem = New ToolStripMenuItem()
        清除ToolStripMenuItem = New ToolStripMenuItem()
        分析ToolStripMenuItem = New ToolStripMenuItem()
        MSN单倍型网络ToolStripMenuItem = New ToolStripMenuItem()
        MJN单倍型网络ToolStripMenuItem = New ToolStripMenuItem()
        单倍型网络ToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator3 = New ToolStripSeparator()
        序列比对高速ToolStripMenuItem = New ToolStripMenuItem()
        AutoToolStripMenuItem = New ToolStripMenuItem()
        FFTNS1VeryFastButVeryRoughToolStripMenuItem = New ToolStripMenuItem()
        FFTNS2FastButRoughToolStripMenuItem = New ToolStripMenuItem()
        GINSiVerySlowToolStripMenuItem = New ToolStripMenuItem()
        LINSiMostAccurateVerySlowToolStripMenuItem = New ToolStripMenuItem()
        EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem = New ToolStripMenuItem()
        序列比对ToolStripMenuItem = New ToolStripMenuItem()
        PPPAlgorithmToolStripMenuItem = New ToolStripMenuItem()
        Super5AlgorithmToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator5 = New ToolStripSeparator()
        网络图可视化ToolStripMenuItem = New ToolStripMenuItem()
        浏览ToolStripMenuItem = New ToolStripMenuItem()
        前进ToolStripMenuItem = New ToolStripMenuItem()
        后退ToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator1 = New ToolStripSeparator()
        分析记录ToolStripMenuItem = New ToolStripMenuItem()
        工具ToolStripMenuItem = New ToolStripMenuItem()
        清理序列ToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator6 = New ToolStripSeparator()
        获取序列信息ToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator7 = New ToolStripSeparator()
        日期转换数字ToolStripMenuItem = New ToolStripMenuItem()
        ToolStripSeparator11 = New ToolStripSeparator()
        EnglishToolStripMenuItem = New ToolStripMenuItem()
        TabControl1 = New TabControl()
        TabPage1 = New TabPage()
        TabPage2 = New TabPage()
        DataGridView1 = New DataGridView()
        TabPage3 = New TabPage()
        TextBox5 = New TextBox()
        RichTextBox1 = New RichTextBox()
        TextBox1 = New TextBox()
        ProgressBar1 = New ProgressBar()
        Timer1 = New Timer(components)
        CType(WebView_main, ComponentModel.ISupportInitialize).BeginInit()
        MenuStrip1.SuspendLayout()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        TabPage2.SuspendLayout()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        TabPage3.SuspendLayout()
        SuspendLayout()
        ' 
        ' WebView_main
        ' 
        WebView_main.AllowExternalDrop = True
        WebView_main.CreationProperties = Nothing
        WebView_main.DefaultBackgroundColor = Color.White
        WebView_main.Dock = DockStyle.Fill
        WebView_main.Location = New Point(3, 3)
        WebView_main.Name = "WebView_main"
        WebView_main.Size = New Size(786, 432)
        WebView_main.TabIndex = 0
        WebView_main.ZoomFactor = 1R
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Items.AddRange(New ToolStripItem() {文件FToolStripMenuItem, 编辑ToolStripMenuItem, 分析ToolStripMenuItem, 浏览ToolStripMenuItem, 工具ToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(800, 25)
        MenuStrip1.TabIndex = 1
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' 文件FToolStripMenuItem
        ' 
        文件FToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {载入序列ToolStripMenuItem, 载入数据ToolStripMenuItem, 增加数据ToolStripMenuItem, ToolStripSeparator4, 保存数据ToolStripMenuItem, 导出序列ToolStripMenuItem})
        文件FToolStripMenuItem.Name = "文件FToolStripMenuItem"
        文件FToolStripMenuItem.Size = New Size(58, 21)
        文件FToolStripMenuItem.Text = "文件(&F)"
        ' 
        ' 载入序列ToolStripMenuItem
        ' 
        载入序列ToolStripMenuItem.Name = "载入序列ToolStripMenuItem"
        载入序列ToolStripMenuItem.Size = New Size(124, 22)
        载入序列ToolStripMenuItem.Text = "载入序列"
        ' 
        ' 载入数据ToolStripMenuItem
        ' 
        载入数据ToolStripMenuItem.Name = "载入数据ToolStripMenuItem"
        载入数据ToolStripMenuItem.Size = New Size(124, 22)
        载入数据ToolStripMenuItem.Text = "载入表格"
        ' 
        ' 增加数据ToolStripMenuItem
        ' 
        增加数据ToolStripMenuItem.Name = "增加数据ToolStripMenuItem"
        增加数据ToolStripMenuItem.Size = New Size(124, 22)
        增加数据ToolStripMenuItem.Text = "增加序列"
        ' 
        ' ToolStripSeparator4
        ' 
        ToolStripSeparator4.Name = "ToolStripSeparator4"
        ToolStripSeparator4.Size = New Size(121, 6)
        ' 
        ' 保存数据ToolStripMenuItem
        ' 
        保存数据ToolStripMenuItem.Name = "保存数据ToolStripMenuItem"
        保存数据ToolStripMenuItem.Size = New Size(124, 22)
        保存数据ToolStripMenuItem.Text = "导出表格"
        ' 
        ' 导出序列ToolStripMenuItem
        ' 
        导出序列ToolStripMenuItem.Name = "导出序列ToolStripMenuItem"
        导出序列ToolStripMenuItem.Size = New Size(124, 22)
        导出序列ToolStripMenuItem.Text = "导出序列"
        ' 
        ' 编辑ToolStripMenuItem
        ' 
        编辑ToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {全选ToolStripMenuItem, 清除ToolStripMenuItem})
        编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem"
        编辑ToolStripMenuItem.Size = New Size(44, 21)
        编辑ToolStripMenuItem.Text = "编辑"
        ' 
        ' 全选ToolStripMenuItem
        ' 
        全选ToolStripMenuItem.Name = "全选ToolStripMenuItem"
        全选ToolStripMenuItem.Size = New Size(100, 22)
        全选ToolStripMenuItem.Text = "全选"
        ' 
        ' 清除ToolStripMenuItem
        ' 
        清除ToolStripMenuItem.Name = "清除ToolStripMenuItem"
        清除ToolStripMenuItem.Size = New Size(100, 22)
        清除ToolStripMenuItem.Text = "清除"
        ' 
        ' 分析ToolStripMenuItem
        ' 
        分析ToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {MSN单倍型网络ToolStripMenuItem, MJN单倍型网络ToolStripMenuItem, 单倍型网络ToolStripMenuItem, ToolStripSeparator3, 序列比对高速ToolStripMenuItem, 序列比对ToolStripMenuItem, ToolStripSeparator5, 网络图可视化ToolStripMenuItem})
        分析ToolStripMenuItem.Name = "分析ToolStripMenuItem"
        分析ToolStripMenuItem.Size = New Size(44, 21)
        分析ToolStripMenuItem.Text = "分析"
        ' 
        ' MSN单倍型网络ToolStripMenuItem
        ' 
        MSN单倍型网络ToolStripMenuItem.Name = "MSN单倍型网络ToolStripMenuItem"
        MSN单倍型网络ToolStripMenuItem.Size = New Size(173, 22)
        MSN单倍型网络ToolStripMenuItem.Text = "MSN单倍型网络"
        ' 
        ' MJN单倍型网络ToolStripMenuItem
        ' 
        MJN单倍型网络ToolStripMenuItem.Name = "MJN单倍型网络ToolStripMenuItem"
        MJN单倍型网络ToolStripMenuItem.Size = New Size(173, 22)
        MJN单倍型网络ToolStripMenuItem.Text = "MJN单倍型网络"
        ' 
        ' 单倍型网络ToolStripMenuItem
        ' 
        单倍型网络ToolStripMenuItem.Name = "单倍型网络ToolStripMenuItem"
        单倍型网络ToolStripMenuItem.Size = New Size(173, 22)
        单倍型网络ToolStripMenuItem.Text = "TCS单倍型网络"
        ' 
        ' ToolStripSeparator3
        ' 
        ToolStripSeparator3.Name = "ToolStripSeparator3"
        ToolStripSeparator3.Size = New Size(170, 6)
        ' 
        ' 序列比对高速ToolStripMenuItem
        ' 
        序列比对高速ToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {AutoToolStripMenuItem, FFTNS1VeryFastButVeryRoughToolStripMenuItem, FFTNS2FastButRoughToolStripMenuItem, GINSiVerySlowToolStripMenuItem, LINSiMostAccurateVerySlowToolStripMenuItem, EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem})
        序列比对高速ToolStripMenuItem.Name = "序列比对高速ToolStripMenuItem"
        序列比对高速ToolStripMenuItem.Size = New Size(173, 22)
        序列比对高速ToolStripMenuItem.Text = "序列比对(Mafft)"
        ' 
        ' AutoToolStripMenuItem
        ' 
        AutoToolStripMenuItem.Name = "AutoToolStripMenuItem"
        AutoToolStripMenuItem.Size = New Size(358, 22)
        AutoToolStripMenuItem.Text = "Auto"
        ' 
        ' FFTNS1VeryFastButVeryRoughToolStripMenuItem
        ' 
        FFTNS1VeryFastButVeryRoughToolStripMenuItem.Name = "FFTNS1VeryFastButVeryRoughToolStripMenuItem"
        FFTNS1VeryFastButVeryRoughToolStripMenuItem.Size = New Size(358, 22)
        FFTNS1VeryFastButVeryRoughToolStripMenuItem.Text = "FFT-NS-1 (Very fast but very rough)"
        ' 
        ' FFTNS2FastButRoughToolStripMenuItem
        ' 
        FFTNS2FastButRoughToolStripMenuItem.Name = "FFTNS2FastButRoughToolStripMenuItem"
        FFTNS2FastButRoughToolStripMenuItem.Size = New Size(358, 22)
        FFTNS2FastButRoughToolStripMenuItem.Text = "FFT-NS-2 (Fast but rough)"
        ' 
        ' GINSiVerySlowToolStripMenuItem
        ' 
        GINSiVerySlowToolStripMenuItem.Name = "GINSiVerySlowToolStripMenuItem"
        GINSiVerySlowToolStripMenuItem.Size = New Size(358, 22)
        GINSiVerySlowToolStripMenuItem.Text = "G-INS-i (For similar lengths, very slow)"
        ' 
        ' LINSiMostAccurateVerySlowToolStripMenuItem
        ' 
        LINSiMostAccurateVerySlowToolStripMenuItem.Name = "LINSiMostAccurateVerySlowToolStripMenuItem"
        LINSiMostAccurateVerySlowToolStripMenuItem.Size = New Size(358, 22)
        LINSiMostAccurateVerySlowToolStripMenuItem.Text = "L-INS-i (Most accurate, very slow)"
        ' 
        ' EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem
        ' 
        EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem.Name = "EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem"
        EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem.Size = New Size(358, 22)
        EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem.Text = "E-INS-i (For long unalignable regions, very slow)"
        ' 
        ' 序列比对ToolStripMenuItem
        ' 
        序列比对ToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {PPPAlgorithmToolStripMenuItem, Super5AlgorithmToolStripMenuItem})
        序列比对ToolStripMenuItem.Name = "序列比对ToolStripMenuItem"
        序列比对ToolStripMenuItem.Size = New Size(173, 22)
        序列比对ToolStripMenuItem.Text = "序列比对(Muscle)"
        ' 
        ' PPPAlgorithmToolStripMenuItem
        ' 
        PPPAlgorithmToolStripMenuItem.Name = "PPPAlgorithmToolStripMenuItem"
        PPPAlgorithmToolStripMenuItem.Size = New Size(177, 22)
        PPPAlgorithmToolStripMenuItem.Text = "PPP algorithm"
        ' 
        ' Super5AlgorithmToolStripMenuItem
        ' 
        Super5AlgorithmToolStripMenuItem.Name = "Super5AlgorithmToolStripMenuItem"
        Super5AlgorithmToolStripMenuItem.Size = New Size(177, 22)
        Super5AlgorithmToolStripMenuItem.Text = "Super5 algorithm"
        ' 
        ' ToolStripSeparator5
        ' 
        ToolStripSeparator5.Name = "ToolStripSeparator5"
        ToolStripSeparator5.Size = New Size(170, 6)
        ' 
        ' 网络图可视化ToolStripMenuItem
        ' 
        网络图可视化ToolStripMenuItem.Name = "网络图可视化ToolStripMenuItem"
        网络图可视化ToolStripMenuItem.Size = New Size(173, 22)
        网络图可视化ToolStripMenuItem.Text = "网络图可视化"
        ' 
        ' 浏览ToolStripMenuItem
        ' 
        浏览ToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {前进ToolStripMenuItem, 后退ToolStripMenuItem, ToolStripSeparator1, 分析记录ToolStripMenuItem})
        浏览ToolStripMenuItem.Name = "浏览ToolStripMenuItem"
        浏览ToolStripMenuItem.Size = New Size(44, 21)
        浏览ToolStripMenuItem.Text = "浏览"
        ' 
        ' 前进ToolStripMenuItem
        ' 
        前进ToolStripMenuItem.Name = "前进ToolStripMenuItem"
        前进ToolStripMenuItem.Size = New Size(124, 22)
        前进ToolStripMenuItem.Text = "前进"
        ' 
        ' 后退ToolStripMenuItem
        ' 
        后退ToolStripMenuItem.Name = "后退ToolStripMenuItem"
        后退ToolStripMenuItem.Size = New Size(124, 22)
        后退ToolStripMenuItem.Text = "后退"
        ' 
        ' ToolStripSeparator1
        ' 
        ToolStripSeparator1.Name = "ToolStripSeparator1"
        ToolStripSeparator1.Size = New Size(121, 6)
        ' 
        ' 分析记录ToolStripMenuItem
        ' 
        分析记录ToolStripMenuItem.Name = "分析记录ToolStripMenuItem"
        分析记录ToolStripMenuItem.Size = New Size(124, 22)
        分析记录ToolStripMenuItem.Text = "查看记录"
        ' 
        ' 工具ToolStripMenuItem
        ' 
        工具ToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {清理序列ToolStripMenuItem, ToolStripSeparator6, 获取序列信息ToolStripMenuItem, ToolStripSeparator7, 日期转换数字ToolStripMenuItem, ToolStripSeparator11, EnglishToolStripMenuItem})
        工具ToolStripMenuItem.Name = "工具ToolStripMenuItem"
        工具ToolStripMenuItem.Size = New Size(44, 21)
        工具ToolStripMenuItem.Text = "工具"
        ' 
        ' 清理序列ToolStripMenuItem
        ' 
        清理序列ToolStripMenuItem.Name = "清理序列ToolStripMenuItem"
        清理序列ToolStripMenuItem.Size = New Size(180, 22)
        清理序列ToolStripMenuItem.Text = "清理序列数据"
        ' 
        ' ToolStripSeparator6
        ' 
        ToolStripSeparator6.Name = "ToolStripSeparator6"
        ToolStripSeparator6.Size = New Size(177, 6)
        ' 
        ' 获取序列信息ToolStripMenuItem
        ' 
        获取序列信息ToolStripMenuItem.Name = "获取序列信息ToolStripMenuItem"
        获取序列信息ToolStripMenuItem.Size = New Size(180, 22)
        获取序列信息ToolStripMenuItem.Text = "序列生成表格"
        获取序列信息ToolStripMenuItem.Visible = False
        ' 
        ' ToolStripSeparator7
        ' 
        ToolStripSeparator7.Name = "ToolStripSeparator7"
        ToolStripSeparator7.Size = New Size(177, 6)
        ToolStripSeparator7.Visible = False
        ' 
        ' 日期转换数字ToolStripMenuItem
        ' 
        日期转换数字ToolStripMenuItem.Name = "日期转换数字ToolStripMenuItem"
        日期转换数字ToolStripMenuItem.Size = New Size(180, 22)
        日期转换数字ToolStripMenuItem.Text = "日期转换数字"
        ' 
        ' ToolStripSeparator11
        ' 
        ToolStripSeparator11.Name = "ToolStripSeparator11"
        ToolStripSeparator11.Size = New Size(177, 6)
        ' 
        ' EnglishToolStripMenuItem
        ' 
        EnglishToolStripMenuItem.Name = "EnglishToolStripMenuItem"
        EnglishToolStripMenuItem.Size = New Size(180, 22)
        EnglishToolStripMenuItem.Text = "English"
        ' 
        ' TabControl1
        ' 
        TabControl1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TabControl1.Controls.Add(TabPage1)
        TabControl1.Controls.Add(TabPage2)
        TabControl1.Controls.Add(TabPage3)
        TabControl1.Location = New Point(0, 25)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(800, 468)
        TabControl1.TabIndex = 2
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(WebView_main)
        TabPage1.Location = New Point(4, 26)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(792, 438)
        TabPage1.TabIndex = 0
        TabPage1.Text = "结果"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(DataGridView1)
        TabPage2.Location = New Point(4, 26)
        TabPage2.Name = "TabPage2"
        TabPage2.Padding = New Padding(3)
        TabPage2.Size = New Size(792, 438)
        TabPage2.TabIndex = 1
        TabPage2.Text = "序列"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        DataGridView1.Dock = DockStyle.Fill
        DataGridView1.Location = New Point(3, 3)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowTemplate.Height = 25
        DataGridView1.Size = New Size(786, 432)
        DataGridView1.TabIndex = 0
        ' 
        ' TabPage3
        ' 
        TabPage3.Controls.Add(TextBox5)
        TabPage3.Controls.Add(RichTextBox1)
        TabPage3.Location = New Point(4, 26)
        TabPage3.Name = "TabPage3"
        TabPage3.Size = New Size(792, 438)
        TabPage3.TabIndex = 2
        TabPage3.Text = "工具信息"
        TabPage3.UseVisualStyleBackColor = True
        ' 
        ' TextBox5
        ' 
        TextBox5.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right
        TextBox5.BackColor = SystemColors.Info
        TextBox5.Location = New Point(619, 5)
        TextBox5.Multiline = True
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(170, 430)
        TextBox5.TabIndex = 2
        ' 
        ' RichTextBox1
        ' 
        RichTextBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        RichTextBox1.Location = New Point(3, 5)
        RichTextBox1.Name = "RichTextBox1"
        RichTextBox1.ReadOnly = True
        RichTextBox1.Size = New Size(609, 430)
        RichTextBox1.TabIndex = 0
        RichTextBox1.Text = ""
        ' 
        ' TextBox1
        ' 
        TextBox1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        TextBox1.BorderStyle = BorderStyle.FixedSingle
        TextBox1.Location = New Point(4, 499)
        TextBox1.Name = "TextBox1"
        TextBox1.ReadOnly = True
        TextBox1.Size = New Size(613, 23)
        TextBox1.TabIndex = 3
        ' 
        ' ProgressBar1
        ' 
        ProgressBar1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        ProgressBar1.Location = New Point(623, 499)
        ProgressBar1.Name = "ProgressBar1"
        ProgressBar1.Size = New Size(173, 23)
        ProgressBar1.TabIndex = 4
        ' 
        ' Timer1
        ' 
        Timer1.Enabled = True
        Timer1.Interval = 1000
        ' 
        ' Mainform
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 526)
        Controls.Add(ProgressBar1)
        Controls.Add(TextBox1)
        Controls.Add(TabControl1)
        Controls.Add(MenuStrip1)
        MainMenuStrip = MenuStrip1
        Name = "Mainform"
        StartPosition = FormStartPosition.CenterScreen
        Text = "性状网络分析"
        CType(WebView_main, ComponentModel.ISupportInitialize).EndInit()
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        TabControl1.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        TabPage2.ResumeLayout(False)
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        TabPage3.ResumeLayout(False)
        TabPage3.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents WebView_main As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents 文件FToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 载入序列ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Timer1 As Timer
    Friend WithEvents 载入数据ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 保存数据ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 分析ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 编辑ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 全选ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 清除ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 浏览ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 前进ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 后退ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 单倍型网络ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MSN单倍型网络ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MJN单倍型网络ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 分析记录ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents 序列比对ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents 工具ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 获取序列信息ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 清理序列ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents 序列比对高速ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 导出序列ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 日期转换数字ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents 增加数据ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents AutoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FFTNS1VeryFastButVeryRoughToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FFTNS2FastButRoughToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GINSiVerySlowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LINSiMostAccurateVerySlowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnglishToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents PPPAlgorithmToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Super5AlgorithmToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnglishToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 网络图可视化ToolStripMenuItem As ToolStripMenuItem
End Class
