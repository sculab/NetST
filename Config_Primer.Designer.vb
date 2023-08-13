<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Primer
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
        Label1 = New Label()
        NumericUpDown1 = New NumericUpDown()
        NumericUpDown2 = New NumericUpDown()
        Label2 = New Label()
        NumericUpDown3 = New NumericUpDown()
        Label3 = New Label()
        NumericUpDown4 = New NumericUpDown()
        Label4 = New Label()
        NumericUpDown5 = New NumericUpDown()
        Label5 = New Label()
        NumericUpDown6 = New NumericUpDown()
        Label6 = New Label()
        NumericUpDown7 = New NumericUpDown()
        Label7 = New Label()
        NumericUpDown8 = New NumericUpDown()
        Label8 = New Label()
        CheckBox1 = New CheckBox()
        Button3 = New Button()
        TextBox4 = New TextBox()
        Button2 = New Button()
        Button1 = New Button()
        RadioButton1 = New RadioButton()
        RadioButton2 = New RadioButton()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown5, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown6, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown7, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown8, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(140, 17)
        Label1.TabIndex = 0
        Label1.Text = "每片段最大候选引物数量"
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Location = New Point(203, 7)
        NumericUpDown1.Maximum = New Decimal(New Integer() {512, 0, 0, 0})
        NumericUpDown1.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(87, 23)
        NumericUpDown1.TabIndex = 2
        NumericUpDown1.Value = New Decimal(New Integer() {16, 0, 0, 0})
        ' 
        ' NumericUpDown2
        ' 
        NumericUpDown2.Location = New Point(203, 36)
        NumericUpDown2.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        NumericUpDown2.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        NumericUpDown2.Name = "NumericUpDown2"
        NumericUpDown2.Size = New Size(87, 23)
        NumericUpDown2.TabIndex = 4
        NumericUpDown2.Value = New Decimal(New Integer() {200, 0, 0, 0})
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(12, 38)
        Label2.Name = "Label2"
        Label2.Size = New Size(80, 17)
        Label2.TabIndex = 3
        Label2.Text = "基因最小长度"
        ' 
        ' NumericUpDown3
        ' 
        NumericUpDown3.Location = New Point(203, 65)
        NumericUpDown3.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        NumericUpDown3.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        NumericUpDown3.Name = "NumericUpDown3"
        NumericUpDown3.Size = New Size(87, 23)
        NumericUpDown3.TabIndex = 6
        NumericUpDown3.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(12, 67)
        Label3.Name = "Label3"
        Label3.Size = New Size(80, 17)
        Label3.TabIndex = 5
        Label3.Text = "基因最大长度"
        ' 
        ' NumericUpDown4
        ' 
        NumericUpDown4.Location = New Point(203, 94)
        NumericUpDown4.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        NumericUpDown4.Name = "NumericUpDown4"
        NumericUpDown4.Size = New Size(87, 23)
        NumericUpDown4.TabIndex = 8
        NumericUpDown4.Value = New Decimal(New Integer() {200, 0, 0, 0})
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(12, 96)
        Label4.Name = "Label4"
        Label4.Size = New Size(80, 17)
        Label4.TabIndex = 7
        Label4.Text = "扩展边界长度"
        ' 
        ' NumericUpDown5
        ' 
        NumericUpDown5.Location = New Point(203, 152)
        NumericUpDown5.Maximum = New Decimal(New Integer() {50000, 0, 0, 0})
        NumericUpDown5.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        NumericUpDown5.Name = "NumericUpDown5"
        NumericUpDown5.Size = New Size(87, 23)
        NumericUpDown5.TabIndex = 12
        NumericUpDown5.Value = New Decimal(New Integer() {2000, 0, 0, 0})
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(12, 154)
        Label5.Name = "Label5"
        Label5.Size = New Size(99, 17)
        Label5.TabIndex = 11
        Label5.Text = "Marker最大长度"
        ' 
        ' NumericUpDown6
        ' 
        NumericUpDown6.Location = New Point(203, 123)
        NumericUpDown6.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        NumericUpDown6.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        NumericUpDown6.Name = "NumericUpDown6"
        NumericUpDown6.Size = New Size(87, 23)
        NumericUpDown6.TabIndex = 10
        NumericUpDown6.Value = New Decimal(New Integer() {200, 0, 0, 0})
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(12, 125)
        Label6.Name = "Label6"
        Label6.Size = New Size(99, 17)
        Label6.TabIndex = 9
        Label6.Text = "Marker最小长度"
        ' 
        ' NumericUpDown7
        ' 
        NumericUpDown7.Location = New Point(203, 181)
        NumericUpDown7.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        NumericUpDown7.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        NumericUpDown7.Name = "NumericUpDown7"
        NumericUpDown7.Size = New Size(87, 23)
        NumericUpDown7.TabIndex = 14
        NumericUpDown7.Value = New Decimal(New Integer() {100, 0, 0, 0})
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(12, 183)
        Label7.Name = "Label7"
        Label7.Size = New Size(82, 17)
        Label7.TabIndex = 13
        Label7.Text = "ladder分辨率"
        ' 
        ' NumericUpDown8
        ' 
        NumericUpDown8.Location = New Point(203, 210)
        NumericUpDown8.Maximum = New Decimal(New Integer() {512, 0, 0, 0})
        NumericUpDown8.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        NumericUpDown8.Name = "NumericUpDown8"
        NumericUpDown8.Size = New Size(87, 23)
        NumericUpDown8.TabIndex = 16
        NumericUpDown8.Value = New Decimal(New Integer() {8, 0, 0, 0})
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(12, 212)
        Label8.Name = "Label8"
        Label8.Size = New Size(44, 17)
        Label8.TabIndex = 15
        Label8.Text = "线程数"
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Checked = True
        CheckBox1.CheckState = CheckState.Checked
        CheckBox1.Location = New Point(12, 245)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(171, 21)
        CheckBox1.TabIndex = 17
        CheckBox1.Text = "优先使用保守区域设计引物"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(215, 298)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 19
        Button3.Text = "保存位置"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(12, 302)
        TextBox4.Name = "TextBox4"
        TextBox4.ReadOnly = True
        TextBox4.Size = New Size(197, 23)
        TextBox4.TabIndex = 18
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(215, 334)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 21
        Button2.Text = "取消"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(134, 334)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 20
        Button1.Text = "确定"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(12, 272)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(111, 21)
        RadioButton1.TabIndex = 22
        RadioButton1.TabStop = True
        RadioButton1.Text = "使用ID区分样本"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.Location = New Point(151, 272)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(127, 21)
        RadioButton2.TabIndex = 23
        RadioButton2.Text = "使用State区分样本"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' Config_Primer
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(302, 374)
        ControlBox = False
        Controls.Add(RadioButton2)
        Controls.Add(RadioButton1)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(Button3)
        Controls.Add(TextBox4)
        Controls.Add(CheckBox1)
        Controls.Add(NumericUpDown8)
        Controls.Add(Label8)
        Controls.Add(NumericUpDown7)
        Controls.Add(Label7)
        Controls.Add(NumericUpDown5)
        Controls.Add(Label5)
        Controls.Add(NumericUpDown6)
        Controls.Add(Label6)
        Controls.Add(NumericUpDown4)
        Controls.Add(Label4)
        Controls.Add(NumericUpDown3)
        Controls.Add(Label3)
        Controls.Add(NumericUpDown2)
        Controls.Add(Label2)
        Controls.Add(NumericUpDown1)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Primer"
        StartPosition = FormStartPosition.CenterScreen
        Text = "引物设计"
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown5, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown6, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown7, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown8, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents NumericUpDown2 As NumericUpDown
    Friend WithEvents Label2 As Label
    Friend WithEvents NumericUpDown3 As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents NumericUpDown4 As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents NumericUpDown5 As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents NumericUpDown6 As NumericUpDown
    Friend WithEvents Label6 As Label
    Friend WithEvents NumericUpDown7 As NumericUpDown
    Friend WithEvents Label7 As Label
    Friend WithEvents NumericUpDown8 As NumericUpDown
    Friend WithEvents Label8 As Label
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Button3 As Button
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
End Class
