<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Split
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
        RadioButton2 = New RadioButton()
        RadioButton1 = New RadioButton()
        Button2 = New Button()
        Button1 = New Button()
        Button3 = New Button()
        TextBox4 = New TextBox()
        NumericUpDown4 = New NumericUpDown()
        Label4 = New Label()
        NumericUpDown3 = New NumericUpDown()
        Label3 = New Label()
        NumericUpDown2 = New NumericUpDown()
        Label2 = New Label()
        CheckBox1 = New CheckBox()
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.Location = New Point(13, 125)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(193, 21)
        RadioButton2.TabIndex = 46
        RadioButton2.Text = "使用Organism+State区分样本"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(14, 98)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(177, 21)
        RadioButton1.TabIndex = 45
        RadioButton1.TabStop = True
        RadioButton1.Text = "使用Organism+ID区分样本"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(216, 211)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 44
        Button2.Text = "取消"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(135, 211)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 43
        Button1.Text = "确定"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(216, 175)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 42
        Button3.Text = "保存位置"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(13, 179)
        TextBox4.Name = "TextBox4"
        TextBox4.ReadOnly = True
        TextBox4.Size = New Size(197, 23)
        TextBox4.TabIndex = 41
        ' 
        ' NumericUpDown4
        ' 
        NumericUpDown4.Location = New Point(204, 69)
        NumericUpDown4.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        NumericUpDown4.Name = "NumericUpDown4"
        NumericUpDown4.Size = New Size(87, 23)
        NumericUpDown4.TabIndex = 31
        NumericUpDown4.Value = New Decimal(New Integer() {200, 0, 0, 0})
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(13, 71)
        Label4.Name = "Label4"
        Label4.Size = New Size(80, 17)
        Label4.TabIndex = 30
        Label4.Text = "扩展边界长度"
        ' 
        ' NumericUpDown3
        ' 
        NumericUpDown3.Location = New Point(204, 40)
        NumericUpDown3.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        NumericUpDown3.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        NumericUpDown3.Name = "NumericUpDown3"
        NumericUpDown3.Size = New Size(87, 23)
        NumericUpDown3.TabIndex = 29
        NumericUpDown3.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(13, 42)
        Label3.Name = "Label3"
        Label3.Size = New Size(80, 17)
        Label3.TabIndex = 28
        Label3.Text = "基因最大长度"
        ' 
        ' NumericUpDown2
        ' 
        NumericUpDown2.Location = New Point(204, 11)
        NumericUpDown2.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        NumericUpDown2.Minimum = New Decimal(New Integer() {50, 0, 0, 0})
        NumericUpDown2.Name = "NumericUpDown2"
        NumericUpDown2.Size = New Size(87, 23)
        NumericUpDown2.TabIndex = 27
        NumericUpDown2.Value = New Decimal(New Integer() {200, 0, 0, 0})
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(13, 13)
        Label2.Name = "Label2"
        Label2.Size = New Size(80, 17)
        Label2.TabIndex = 26
        Label2.Text = "基因最小长度"
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(14, 154)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(111, 21)
        CheckBox1.TabIndex = 47
        CheckBox1.Text = "比对获取的序列"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' Config_Split
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(303, 250)
        ControlBox = False
        Controls.Add(CheckBox1)
        Controls.Add(RadioButton2)
        Controls.Add(RadioButton1)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(Button3)
        Controls.Add(TextBox4)
        Controls.Add(NumericUpDown4)
        Controls.Add(Label4)
        Controls.Add(NumericUpDown3)
        Controls.Add(Label3)
        Controls.Add(NumericUpDown2)
        Controls.Add(Label2)
        Name = "Config_Split"
        Text = "分割序列"
        TopMost = True
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents NumericUpDown4 As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents NumericUpDown3 As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents NumericUpDown2 As NumericUpDown
    Friend WithEvents Label2 As Label
    Friend WithEvents CheckBox1 As CheckBox
End Class
