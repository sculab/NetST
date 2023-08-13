<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Type
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
        Button2 = New Button()
        Button1 = New Button()
        ComboBox1 = New ComboBox()
        Button3 = New Button()
        Button4 = New Button()
        TextBox2 = New TextBox()
        RadioButton3 = New RadioButton()
        Label1 = New Label()
        NumericUpDown1 = New NumericUpDown()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(12, 12)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(134, 21)
        RadioButton1.TabIndex = 0
        RadioButton1.TabStop = True
        RadioButton1.Text = "使用自定义参考序列"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.Location = New Point(12, 68)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(122, 21)
        RadioButton2.TabIndex = 1
        RadioButton2.Text = "使用自定义数据库"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(17, 39)
        TextBox1.Name = "TextBox1"
        TextBox1.ReadOnly = True
        TextBox1.Size = New Size(263, 23)
        TextBox1.TabIndex = 2
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(286, 190)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 13
        Button2.Text = "取消"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(205, 190)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 12
        Button1.Text = "确定"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.Enabled = False
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(17, 151)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(344, 25)
        ComboBox1.TabIndex = 14
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(286, 35)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 15
        Button3.Text = "浏览"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(286, 91)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 30)
        Button4.TabIndex = 18
        Button4.Text = "浏览"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(17, 95)
        TextBox2.Name = "TextBox2"
        TextBox2.ReadOnly = True
        TextBox2.Size = New Size(263, 23)
        TextBox2.TabIndex = 17
        ' 
        ' RadioButton3
        ' 
        RadioButton3.AutoSize = True
        RadioButton3.Enabled = False
        RadioButton3.Location = New Point(12, 124)
        RadioButton3.Name = "RadioButton3"
        RadioButton3.Size = New Size(110, 21)
        RadioButton3.TabIndex = 16
        RadioButton3.Text = "使用内置数据库"
        RadioButton3.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(17, 197)
        Label1.Name = "Label1"
        Label1.Size = New Size(19, 17)
        Label1.TabIndex = 19
        Label1.Text = "K:"
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Location = New Point(42, 195)
        NumericUpDown1.Maximum = New Decimal(New Integer() {512, 0, 0, 0})
        NumericUpDown1.Minimum = New Decimal(New Integer() {11, 0, 0, 0})
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(53, 23)
        NumericUpDown1.TabIndex = 20
        NumericUpDown1.Value = New Decimal(New Integer() {31, 0, 0, 0})
        ' 
        ' Config_Type
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(374, 229)
        ControlBox = False
        Controls.Add(NumericUpDown1)
        Controls.Add(Label1)
        Controls.Add(Button4)
        Controls.Add(TextBox2)
        Controls.Add(RadioButton3)
        Controls.Add(Button3)
        Controls.Add(ComboBox1)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(TextBox1)
        Controls.Add(RadioButton2)
        Controls.Add(RadioButton1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Type"
        StartPosition = FormStartPosition.CenterScreen
        Text = "快速分型"
        TopMost = True
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents NumericUpDown1 As NumericUpDown
End Class
