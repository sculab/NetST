<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Clean
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
        CheckBox1 = New CheckBox()
        CheckBox2 = New CheckBox()
        CheckBox3 = New CheckBox()
        TextBox1 = New TextBox()
        CheckBox4 = New CheckBox()
        TextBox9 = New TextBox()
        CheckBox5 = New CheckBox()
        Button1 = New Button()
        Button2 = New Button()
        CheckBox7 = New CheckBox()
        TextBox2 = New TextBox()
        TextBox3 = New TextBox()
        CheckBox8 = New CheckBox()
        TextBox4 = New TextBox()
        Button3 = New Button()
        Label1 = New Label()
        Label5 = New Label()
        TextPreview = New TextBox()
        TextBox5 = New TextBox()
        TextBox6 = New TextBox()
        Label2 = New Label()
        CheckBox6 = New CheckBox()
        SuspendLayout()
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(12, 412)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(112, 21)
        CheckBox1.TabIndex = 0
        CheckBox1.Text = "使用Unix换行符"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' CheckBox2
        ' 
        CheckBox2.AutoSize = True
        CheckBox2.Location = New Point(12, 65)
        CheckBox2.Name = "CheckBox2"
        CheckBox2.Size = New Size(183, 21)
        CheckBox2.TabIndex = 1
        CheckBox2.Text = "移除所有包含简并碱基的序列"
        CheckBox2.UseVisualStyleBackColor = True
        ' 
        ' CheckBox3
        ' 
        CheckBox3.AutoSize = True
        CheckBox3.Location = New Point(12, 92)
        CheckBox3.Name = "CheckBox3"
        CheckBox3.Size = New Size(159, 21)
        CheckBox3.TabIndex = 2
        CheckBox3.Text = "移除短于以下长度的序列"
        CheckBox3.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(28, 175)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(105, 23)
        TextBox1.TabIndex = 3
        ' 
        ' CheckBox4
        ' 
        CheckBox4.AutoSize = True
        CheckBox4.Location = New Point(12, 148)
        CheckBox4.Name = "CheckBox4"
        CheckBox4.Size = New Size(193, 21)
        CheckBox4.TabIndex = 4
        CheckBox4.Text = "移除含N高于以下百分比的序列"
        CheckBox4.UseVisualStyleBackColor = True
        ' 
        ' TextBox9
        ' 
        TextBox9.Location = New Point(28, 119)
        TextBox9.Name = "TextBox9"
        TextBox9.Size = New Size(105, 23)
        TextBox9.TabIndex = 5
        ' 
        ' CheckBox5
        ' 
        CheckBox5.AutoSize = True
        CheckBox5.Location = New Point(12, 385)
        CheckBox5.Name = "CheckBox5"
        CheckBox5.Size = New Size(159, 21)
        CheckBox5.TabIndex = 6
        CheckBox5.Text = "将简并碱基替换为gap(-)"
        CheckBox5.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(439, 439)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 8
        Button1.Text = "确定"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(520, 439)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 9
        Button2.Text = "取消"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' CheckBox7
        ' 
        CheckBox7.AutoSize = True
        CheckBox7.Location = New Point(12, 273)
        CheckBox7.Name = "CheckBox7"
        CheckBox7.Size = New Size(195, 21)
        CheckBox7.TabIndex = 10
        CheckBox7.Text = "移除序列名包含以下字符的序列"
        CheckBox7.UseVisualStyleBackColor = True
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(28, 300)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(105, 23)
        TextBox2.TabIndex = 11
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(28, 356)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(105, 23)
        TextBox3.TabIndex = 13
        ' 
        ' CheckBox8
        ' 
        CheckBox8.AutoSize = True
        CheckBox8.Location = New Point(12, 329)
        CheckBox8.Name = "CheckBox8"
        CheckBox8.Size = New Size(207, 21)
        CheckBox8.TabIndex = 12
        CheckBox8.Text = "只保留序列名包含以下字符的序列"
        CheckBox8.UseVisualStyleBackColor = True
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(12, 34)
        TextBox4.Name = "TextBox4"
        TextBox4.ReadOnly = True
        TextBox4.Size = New Size(201, 23)
        TextBox4.TabIndex = 14
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(219, 30)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 15
        Button3.Text = "浏览"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(71, 17)
        Label1.TabIndex = 16
        Label1.Text = "待处理序列:"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(313, 9)
        Label5.Name = "Label5"
        Label5.Size = New Size(71, 17)
        Label5.TabIndex = 35
        Label5.Text = "序列名预览:"
        ' 
        ' TextPreview
        ' 
        TextPreview.BackColor = SystemColors.Info
        TextPreview.Location = New Point(313, 34)
        TextPreview.Multiline = True
        TextPreview.Name = "TextPreview"
        TextPreview.ReadOnly = True
        TextPreview.Size = New Size(282, 399)
        TextPreview.TabIndex = 34
        ' 
        ' TextBox5
        ' 
        TextBox5.Location = New Point(189, 235)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(105, 23)
        TextBox5.TabIndex = 39
        ' 
        ' TextBox6
        ' 
        TextBox6.Location = New Point(28, 235)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(105, 23)
        TextBox6.TabIndex = 38
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(139, 238)
        Label2.Name = "Label2"
        Label2.Size = New Size(44, 17)
        Label2.TabIndex = 37
        Label2.Text = "替换为"
        ' 
        ' CheckBox6
        ' 
        CheckBox6.AutoSize = True
        CheckBox6.Location = New Point(12, 208)
        CheckBox6.Name = "CheckBox6"
        CheckBox6.Size = New Size(135, 21)
        CheckBox6.TabIndex = 36
        CheckBox6.Text = "替换序列名中的字符"
        CheckBox6.UseVisualStyleBackColor = True
        ' 
        ' Config_Clean
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(603, 478)
        ControlBox = False
        Controls.Add(TextBox5)
        Controls.Add(TextBox6)
        Controls.Add(Label2)
        Controls.Add(CheckBox6)
        Controls.Add(Label5)
        Controls.Add(TextPreview)
        Controls.Add(Label1)
        Controls.Add(Button3)
        Controls.Add(TextBox4)
        Controls.Add(TextBox3)
        Controls.Add(CheckBox8)
        Controls.Add(TextBox2)
        Controls.Add(CheckBox7)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(CheckBox5)
        Controls.Add(TextBox9)
        Controls.Add(CheckBox4)
        Controls.Add(TextBox1)
        Controls.Add(CheckBox3)
        Controls.Add(CheckBox2)
        Controls.Add(CheckBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Clean"
        StartPosition = FormStartPosition.CenterScreen
        Text = "序列清理"
        TopMost = True
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents CheckBox3 As CheckBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents CheckBox4 As CheckBox
    Friend WithEvents TextBox9 As TextBox
    Friend WithEvents CheckBox5 As CheckBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents CheckBox7 As CheckBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents CheckBox8 As CheckBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents TextPreview As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents CheckBox6 As CheckBox
End Class
