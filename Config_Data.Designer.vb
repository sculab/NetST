<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Data
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
        ComboBox1 = New ComboBox()
        TextBox1 = New TextBox()
        Button1 = New Button()
        Label1 = New Label()
        Label2 = New Label()
        ComboBox2 = New ComboBox()
        Label3 = New Label()
        ComboBox3 = New ComboBox()
        TextBox2 = New TextBox()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        RadioButton1 = New RadioButton()
        RadioButton2 = New RadioButton()
        Button5 = New Button()
        TextBox3 = New TextBox()
        CheckBox1 = New CheckBox()
        CheckBox2 = New CheckBox()
        SuspendLayout()
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(124, 71)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(121, 25)
        ComboBox1.TabIndex = 0
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(13, 42)
        TextBox1.Name = "TextBox1"
        TextBox1.ReadOnly = True
        TextBox1.Size = New Size(233, 23)
        TextBox1.TabIndex = 1
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(251, 38)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 2
        Button1.Text = "浏览"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(13, 74)
        Label1.Name = "Label1"
        Label1.Size = New Size(59, 17)
        Label1.TabIndex = 3
        Label1.Text = "序列名称:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(13, 105)
        Label2.Name = "Label2"
        Label2.Size = New Size(64, 17)
        Label2.TabIndex = 4
        Label2.Text = "性状/分型:"
        ' 
        ' ComboBox2
        ' 
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(124, 102)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(121, 25)
        ComboBox2.TabIndex = 5
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(13, 136)
        Label3.Name = "Label3"
        Label3.Size = New Size(35, 17)
        Label3.TabIndex = 6
        Label3.Text = "序列:"
        ' 
        ' ComboBox3
        ' 
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.FormattingEnabled = True
        ComboBox3.Location = New Point(124, 133)
        ComboBox3.Name = "ComboBox3"
        ComboBox3.Size = New Size(121, 25)
        ComboBox3.TabIndex = 7
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(12, 285)
        TextBox2.Name = "TextBox2"
        TextBox2.ReadOnly = True
        TextBox2.Size = New Size(233, 23)
        TextBox2.TabIndex = 8
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(251, 281)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 9
        Button2.Text = "保存位置"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(196, 317)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 15
        Button3.Text = "取消"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(67, 318)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 30)
        Button4.TabIndex = 14
        Button4.Text = "确定"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(12, 12)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(104, 21)
        RadioButton1.TabIndex = 16
        RadioButton1.TabStop = True
        RadioButton1.Text = "从csv文件构建"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.Location = New Point(12, 172)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(114, 21)
        RadioButton2.TabIndex = 17
        RadioButton2.TabStop = True
        RadioButton2.Text = "从fasta文件构建"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(250, 195)
        Button5.Name = "Button5"
        Button5.Size = New Size(75, 30)
        Button5.TabIndex = 20
        Button5.Text = "浏览"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(12, 199)
        TextBox3.Name = "TextBox3"
        TextBox3.ReadOnly = True
        TextBox3.Size = New Size(233, 23)
        TextBox3.TabIndex = 19
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(12, 228)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(99, 21)
        CheckBox1.TabIndex = 21
        CheckBox1.Text = "去除重复序列"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' CheckBox2
        ' 
        CheckBox2.AutoSize = True
        CheckBox2.Location = New Point(12, 255)
        CheckBox2.Name = "CheckBox2"
        CheckBox2.Size = New Size(159, 21)
        CheckBox2.TabIndex = 22
        CheckBox2.Text = "去除包含简并碱基的序列"
        CheckBox2.UseVisualStyleBackColor = True
        ' 
        ' Config_Data
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 17.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(337, 356)
        ControlBox = False
        Controls.Add(CheckBox2)
        Controls.Add(CheckBox1)
        Controls.Add(Button5)
        Controls.Add(TextBox3)
        Controls.Add(RadioButton2)
        Controls.Add(RadioButton1)
        Controls.Add(Button3)
        Controls.Add(Button4)
        Controls.Add(Button2)
        Controls.Add(TextBox2)
        Controls.Add(ComboBox3)
        Controls.Add(Label3)
        Controls.Add(ComboBox2)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Button1)
        Controls.Add(TextBox1)
        Controls.Add(ComboBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Data"
        StartPosition = FormStartPosition.CenterScreen
        Text = "数据集构建"
        TopMost = True
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents ComboBox3 As ComboBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents Button5 As Button
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents CheckBox2 As CheckBox
End Class
