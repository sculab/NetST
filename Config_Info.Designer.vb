<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Info
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
        CheckBox4 = New CheckBox()
        Button2 = New Button()
        Button1 = New Button()
        CheckBox5 = New CheckBox()
        Label1 = New Label()
        Button3 = New Button()
        TextBox4 = New TextBox()
        TextBox1 = New TextBox()
        CheckBox6 = New CheckBox()
        NumericUpDown2 = New NumericUpDown()
        Label3 = New Label()
        TextBox2 = New TextBox()
        TextBox3 = New TextBox()
        Label2 = New Label()
        CheckBox7 = New CheckBox()
        NumericUpDown1 = New NumericUpDown()
        Label4 = New Label()
        TextBox5 = New TextBox()
        CheckBox8 = New CheckBox()
        TextPreview = New TextBox()
        Label5 = New Label()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(12, 149)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(112, 21)
        CheckBox1.TabIndex = 1
        CheckBox1.Text = "使用Unix换行符"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' CheckBox2
        ' 
        CheckBox2.AutoSize = True
        CheckBox2.Checked = True
        CheckBox2.CheckState = CheckState.Checked
        CheckBox2.Location = New Point(12, 122)
        CheckBox2.Name = "CheckBox2"
        CheckBox2.Size = New Size(87, 21)
        CheckBox2.TabIndex = 2
        CheckBox2.Text = "获取序列名"
        CheckBox2.UseVisualStyleBackColor = True
        ' 
        ' CheckBox3
        ' 
        CheckBox3.AutoSize = True
        CheckBox3.Checked = True
        CheckBox3.CheckState = CheckState.Checked
        CheckBox3.Location = New Point(110, 122)
        CheckBox3.Name = "CheckBox3"
        CheckBox3.Size = New Size(99, 21)
        CheckBox3.TabIndex = 3
        CheckBox3.Text = "获取序列长度"
        CheckBox3.UseVisualStyleBackColor = True
        ' 
        ' CheckBox4
        ' 
        CheckBox4.AutoSize = True
        CheckBox4.Location = New Point(168, 149)
        CheckBox4.Name = "CheckBox4"
        CheckBox4.Size = New Size(170, 21)
        CheckBox4.TabIndex = 4
        CheckBox4.Text = "获取不包含gap的序列长度"
        CheckBox4.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(188, 449)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 11
        Button2.Text = "取消"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(70, 449)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 10
        Button1.Text = "确定"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' CheckBox5
        ' 
        CheckBox5.AutoSize = True
        CheckBox5.Checked = True
        CheckBox5.CheckState = CheckState.Checked
        CheckBox5.Location = New Point(215, 122)
        CheckBox5.Name = "CheckBox5"
        CheckBox5.Size = New Size(123, 21)
        CheckBox5.TabIndex = 12
        CheckBox5.Text = "在表格中保存序列"
        CheckBox5.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 5)
        Label1.Name = "Label1"
        Label1.Size = New Size(71, 17)
        Label1.TabIndex = 19
        Label1.Text = "待处理序列:"
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(263, 26)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 18
        Button3.Text = "浏览"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(12, 30)
        TextBox4.Name = "TextBox4"
        TextBox4.ReadOnly = True
        TextBox4.Size = New Size(245, 23)
        TextBox4.TabIndex = 17
        ' 
        ' TextBox1
        ' 
        TextBox1.Enabled = False
        TextBox1.Location = New Point(27, 366)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(120, 23)
        TextBox1.TabIndex = 31
        TextBox1.Text = "|"
        ' 
        ' CheckBox6
        ' 
        CheckBox6.AutoSize = True
        CheckBox6.Location = New Point(12, 339)
        CheckBox6.Name = "CheckBox6"
        CheckBox6.Size = New Size(183, 21)
        CheckBox6.TabIndex = 30
        CheckBox6.Text = "使用下面的分隔符拆分序列名"
        CheckBox6.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown2
        ' 
        NumericUpDown2.Enabled = False
        NumericUpDown2.Location = New Point(27, 412)
        NumericUpDown2.Name = "NumericUpDown2"
        NumericUpDown2.Size = New Size(120, 23)
        NumericUpDown2.TabIndex = 29
        NumericUpDown2.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(27, 392)
        Label3.Name = "Label3"
        Label3.Size = New Size(176, 17)
        Label3.TabIndex = 28
        Label3.Text = "将如下拆分出的位置作为性状名"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(188, 207)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(105, 23)
        TextBox2.TabIndex = 27
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(27, 207)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(105, 23)
        TextBox3.TabIndex = 26
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(138, 210)
        Label2.Name = "Label2"
        Label2.Size = New Size(44, 17)
        Label2.TabIndex = 25
        Label2.Text = "替换为"
        ' 
        ' CheckBox7
        ' 
        CheckBox7.AutoSize = True
        CheckBox7.Location = New Point(12, 180)
        CheckBox7.Name = "CheckBox7"
        CheckBox7.Size = New Size(135, 21)
        CheckBox7.TabIndex = 24
        CheckBox7.Text = "替换序列名中的字符"
        CheckBox7.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Enabled = False
        NumericUpDown1.Location = New Point(27, 310)
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(120, 23)
        NumericUpDown1.TabIndex = 23
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(27, 290)
        Label4.Name = "Label4"
        Label4.Size = New Size(188, 17)
        Label4.TabIndex = 22
        Label4.Text = "将如下拆分出的位置作为新序列名"
        ' 
        ' TextBox5
        ' 
        TextBox5.Enabled = False
        TextBox5.Location = New Point(27, 264)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(105, 23)
        TextBox5.TabIndex = 21
        TextBox5.Text = "|"
        ' 
        ' CheckBox8
        ' 
        CheckBox8.AutoSize = True
        CheckBox8.Location = New Point(12, 237)
        CheckBox8.Name = "CheckBox8"
        CheckBox8.Size = New Size(183, 21)
        CheckBox8.TabIndex = 20
        CheckBox8.Text = "使用下面的分隔符拆分序列名"
        CheckBox8.UseVisualStyleBackColor = True
        ' 
        ' TextPreview
        ' 
        TextPreview.BackColor = SystemColors.Info
        TextPreview.Location = New Point(12, 82)
        TextPreview.Name = "TextPreview"
        TextPreview.ReadOnly = True
        TextPreview.Size = New Size(326, 23)
        TextPreview.TabIndex = 32
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(12, 62)
        Label5.Name = "Label5"
        Label5.Size = New Size(107, 17)
        Label5.TabIndex = 33
        Label5.Text = "第一条序列名预览:"
        ' 
        ' Config_Info
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(348, 489)
        ControlBox = False
        Controls.Add(Label5)
        Controls.Add(TextPreview)
        Controls.Add(TextBox1)
        Controls.Add(CheckBox6)
        Controls.Add(NumericUpDown2)
        Controls.Add(Label3)
        Controls.Add(TextBox2)
        Controls.Add(TextBox3)
        Controls.Add(Label2)
        Controls.Add(CheckBox7)
        Controls.Add(NumericUpDown1)
        Controls.Add(Label4)
        Controls.Add(TextBox5)
        Controls.Add(CheckBox8)
        Controls.Add(Label1)
        Controls.Add(Button3)
        Controls.Add(TextBox4)
        Controls.Add(CheckBox5)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(CheckBox4)
        Controls.Add(CheckBox3)
        Controls.Add(CheckBox2)
        Controls.Add(CheckBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Info"
        StartPosition = FormStartPosition.CenterScreen
        Text = "序列转表格"
        TopMost = True
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents CheckBox3 As CheckBox
    Friend WithEvents CheckBox4 As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents CheckBox5 As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button3 As Button
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents CheckBox6 As CheckBox
    Friend WithEvents NumericUpDown2 As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents CheckBox7 As CheckBox
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents CheckBox8 As CheckBox
    Friend WithEvents TextPreview As TextBox
    Friend WithEvents Label5 As Label
End Class
