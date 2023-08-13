<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Mix
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        RadioButton1 = New RadioButton()
        RadioButton2 = New RadioButton()
        TextBox1 = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        NumericUpDown1 = New NumericUpDown()
        Label3 = New Label()
        TextBox2 = New TextBox()
        Button1 = New Button()
        Button2 = New Button()
        Button3 = New Button()
        TextBox3 = New TextBox()
        Label4 = New Label()
        Label5 = New Label()
        NumericUpDown2 = New NumericUpDown()
        GroupBox1 = New GroupBox()
        Button4 = New Button()
        Button5 = New Button()
        Label6 = New Label()
        NumericUpDown3 = New NumericUpDown()
        Label7 = New Label()
        NumericUpDown4 = New NumericUpDown()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Location = New Point(14, 22)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(98, 21)
        RadioButton1.TabIndex = 0
        RadioButton1.Text = "基于已有结果"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.Checked = True
        RadioButton2.Location = New Point(14, 78)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(98, 21)
        RadioButton2.TabIndex = 1
        RadioButton2.TabStop = True
        RadioButton2.Text = "从头开始分析"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(96, 49)
        TextBox1.Name = "TextBox1"
        TextBox1.ReadOnly = True
        TextBox1.Size = New Size(208, 23)
        TextBox1.TabIndex = 2
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(31, 52)
        Label1.Name = "Label1"
        Label1.Size = New Size(59, 17)
        Label1.TabIndex = 3
        Label1.Text = "结果文件:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(12, 205)
        Label2.Name = "Label2"
        Label2.Size = New Size(71, 17)
        Label2.TabIndex = 4
        Label2.Text = "参考数据集:"
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Location = New Point(323, 138)
        NumericUpDown1.Maximum = New Decimal(New Integer() {512, 0, 0, 0})
        NumericUpDown1.Minimum = New Decimal(New Integer() {11, 0, 0, 0})
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(74, 23)
        NumericUpDown1.TabIndex = 22
        NumericUpDown1.Value = New Decimal(New Integer() {21, 0, 0, 0})
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(269, 140)
        Label3.Name = "Label3"
        Label3.Size = New Size(31, 17)
        Label3.TabIndex = 21
        Label3.Text = "K值:"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(12, 225)
        TextBox2.Name = "TextBox2"
        TextBox2.ReadOnly = True
        TextBox2.Size = New Size(305, 23)
        TextBox2.TabIndex = 23
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(310, 45)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 24
        Button1.Text = "浏览"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(322, 221)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 25
        Button2.Text = "浏览"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(323, 273)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 28
        Button3.Text = "浏览"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(12, 277)
        TextBox3.Name = "TextBox3"
        TextBox3.ReadOnly = True
        TextBox3.Size = New Size(305, 23)
        TextBox3.TabIndex = 27
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(12, 257)
        Label4.Name = "Label4"
        Label4.Size = New Size(71, 17)
        Label4.TabIndex = 26
        Label4.Text = "结果文件夹:"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(12, 140)
        Label5.Name = "Label5"
        Label5.Size = New Size(107, 17)
        Label5.TabIndex = 29
        Label5.Text = "假定混合分型阈值:"
        ' 
        ' NumericUpDown2
        ' 
        NumericUpDown2.Location = New Point(149, 138)
        NumericUpDown2.Name = "NumericUpDown2"
        NumericUpDown2.Size = New Size(74, 23)
        NumericUpDown2.TabIndex = 30
        NumericUpDown2.Value = New Decimal(New Integer() {85, 0, 0, 0})
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(RadioButton1)
        GroupBox1.Controls.Add(RadioButton2)
        GroupBox1.Controls.Add(TextBox1)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(393, 113)
        GroupBox1.TabIndex = 31
        GroupBox1.TabStop = False
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(234, 314)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 30)
        Button4.TabIndex = 33
        Button4.Text = "取消"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(115, 314)
        Button5.Name = "Button5"
        Button5.Size = New Size(75, 30)
        Button5.TabIndex = 32
        Button5.Text = "确定"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(12, 173)
        Label6.Name = "Label6"
        Label6.Size = New Size(131, 17)
        Label6.TabIndex = 34
        Label6.Text = "最大单个分型序列数量:"
        ' 
        ' NumericUpDown3
        ' 
        NumericUpDown3.Location = New Point(149, 171)
        NumericUpDown3.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        NumericUpDown3.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        NumericUpDown3.Name = "NumericUpDown3"
        NumericUpDown3.Size = New Size(74, 23)
        NumericUpDown3.TabIndex = 35
        NumericUpDown3.Value = New Decimal(New Integer() {100, 0, 0, 0})
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(269, 173)
        Label7.Name = "Label7"
        Label7.Size = New Size(47, 17)
        Label7.TabIndex = 36
        Label7.Text = "线程数:"
        ' 
        ' NumericUpDown4
        ' 
        NumericUpDown4.Location = New Point(323, 171)
        NumericUpDown4.Maximum = New Decimal(New Integer() {512, 0, 0, 0})
        NumericUpDown4.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        NumericUpDown4.Name = "NumericUpDown4"
        NumericUpDown4.Size = New Size(74, 23)
        NumericUpDown4.TabIndex = 37
        NumericUpDown4.Value = New Decimal(New Integer() {8, 0, 0, 0})
        ' 
        ' Config_Mix
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(417, 353)
        ControlBox = False
        Controls.Add(Label7)
        Controls.Add(NumericUpDown4)
        Controls.Add(Label6)
        Controls.Add(NumericUpDown3)
        Controls.Add(Button4)
        Controls.Add(Button5)
        Controls.Add(GroupBox1)
        Controls.Add(NumericUpDown2)
        Controls.Add(Label3)
        Controls.Add(Label5)
        Controls.Add(NumericUpDown1)
        Controls.Add(Button3)
        Controls.Add(TextBox3)
        Controls.Add(Label4)
        Controls.Add(Button2)
        Controls.Add(TextBox2)
        Controls.Add(Label2)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Mix"
        StartPosition = FormStartPosition.CenterScreen
        Text = "混合分型检测"
        TopMost = True
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents NumericUpDown2 As NumericUpDown
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents NumericUpDown3 As NumericUpDown
    Friend WithEvents Label7 As Label
    Friend WithEvents NumericUpDown4 As NumericUpDown
End Class
