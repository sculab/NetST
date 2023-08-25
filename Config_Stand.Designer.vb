<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Stand
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
        TextBox3 = New TextBox()
        Label1 = New Label()
        NumericUpDown1 = New NumericUpDown()
        CheckBox5 = New CheckBox()
        CheckBox3 = New CheckBox()
        Label2 = New Label()
        TextBox1 = New TextBox()
        TextBox2 = New TextBox()
        CheckBox4 = New CheckBox()
        Button1 = New Button()
        Button2 = New Button()
        NumericUpDown2 = New NumericUpDown()
        Label3 = New Label()
        TextBox4 = New TextBox()
        CheckBox6 = New CheckBox()
        Label5 = New Label()
        TextPreview = New TextBox()
        TextBox5 = New TextBox()
        CheckBox7 = New CheckBox()
        NumericUpDown3 = New NumericUpDown()
        Label4 = New Label()
        TextBox6 = New TextBox()
        CheckBox8 = New CheckBox()
        NumericUpDown4 = New NumericUpDown()
        Label6 = New Label()
        Label7 = New Label()
        TextBox7 = New TextBox()
        TextBox8 = New TextBox()
        CheckBox9 = New CheckBox()
        NumericUpDown5 = New NumericUpDown()
        Label8 = New Label()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown5, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Location = New Point(187, 362)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(112, 21)
        CheckBox1.TabIndex = 1
        CheckBox1.Text = "使用Unix换行符"
        CheckBox1.UseVisualStyleBackColor = True
        CheckBox1.Visible = False
        ' 
        ' CheckBox2
        ' 
        CheckBox2.AutoSize = True
        CheckBox2.Location = New Point(12, 67)
        CheckBox2.Name = "CheckBox2"
        CheckBox2.Size = New Size(159, 21)
        CheckBox2.TabIndex = 2
        CheckBox2.Text = "使用该分隔符拆分序列名"
        CheckBox2.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Enabled = False
        TextBox3.Location = New Point(188, 65)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(57, 23)
        TextBox3.TabIndex = 3
        TextBox3.Text = "|"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(27, 97)
        Label1.Name = "Label1"
        Label1.Size = New Size(128, 17)
        Label1.TabIndex = 4
        Label1.Text = "将该位置作为新序列名"
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Enabled = False
        NumericUpDown1.Location = New Point(188, 95)
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(58, 23)
        NumericUpDown1.TabIndex = 5
        ' 
        ' CheckBox5
        ' 
        CheckBox5.AutoSize = True
        CheckBox5.Location = New Point(12, 12)
        CheckBox5.Name = "CheckBox5"
        CheckBox5.Size = New Size(183, 21)
        CheckBox5.TabIndex = 6
        CheckBox5.Text = "移除所有包含简并碱基的序列"
        CheckBox5.UseVisualStyleBackColor = True
        ' 
        ' CheckBox3
        ' 
        CheckBox3.AutoSize = True
        CheckBox3.Location = New Point(12, 39)
        CheckBox3.Name = "CheckBox3"
        CheckBox3.Size = New Size(99, 21)
        CheckBox3.TabIndex = 7
        CheckBox3.Text = "替换序列名中"
        CheckBox3.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(167, 40)
        Label2.Name = "Label2"
        Label2.Size = New Size(20, 17)
        Label2.TabIndex = 8
        Label2.Text = "为"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(108, 37)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(57, 23)
        TextBox1.TabIndex = 9
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(187, 37)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(58, 23)
        TextBox2.TabIndex = 10
        ' 
        ' CheckBox4
        ' 
        CheckBox4.AutoSize = True
        CheckBox4.Location = New Point(11, 362)
        CheckBox4.Name = "CheckBox4"
        CheckBox4.Size = New Size(159, 21)
        CheckBox4.TabIndex = 11
        CheckBox4.Text = "使用编号作为序列的名称"
        CheckBox4.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(420, 358)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 12
        Button1.Text = "确定"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(501, 358)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 30)
        Button2.TabIndex = 13
        Button2.Text = "取消"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown2
        ' 
        NumericUpDown2.Enabled = False
        NumericUpDown2.Location = New Point(188, 153)
        NumericUpDown2.Name = "NumericUpDown2"
        NumericUpDown2.Size = New Size(57, 23)
        NumericUpDown2.TabIndex = 15
        NumericUpDown2.Value = New Decimal(New Integer() {1, 0, 0, 0})
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(27, 155)
        Label3.Name = "Label3"
        Label3.Size = New Size(128, 17)
        Label3.TabIndex = 14
        Label3.Text = "将该位置作为间断性状"
        ' 
        ' TextBox4
        ' 
        TextBox4.Enabled = False
        TextBox4.Location = New Point(188, 124)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(57, 23)
        TextBox4.TabIndex = 17
        TextBox4.Text = "|"
        ' 
        ' CheckBox6
        ' 
        CheckBox6.AutoSize = True
        CheckBox6.Location = New Point(12, 126)
        CheckBox6.Name = "CheckBox6"
        CheckBox6.Size = New Size(159, 21)
        CheckBox6.TabIndex = 16
        CheckBox6.Text = "使用该分隔符拆分序列名"
        CheckBox6.UseVisualStyleBackColor = True
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(252, 13)
        Label5.Name = "Label5"
        Label5.Size = New Size(71, 17)
        Label5.TabIndex = 35
        Label5.Text = "序列名预览:"
        ' 
        ' TextPreview
        ' 
        TextPreview.BackColor = SystemColors.Info
        TextPreview.Location = New Point(252, 37)
        TextPreview.Multiline = True
        TextPreview.Name = "TextPreview"
        TextPreview.ReadOnly = True
        TextPreview.ScrollBars = ScrollBars.Vertical
        TextPreview.Size = New Size(214, 315)
        TextPreview.TabIndex = 34
        TextPreview.WordWrap = False
        ' 
        ' TextBox5
        ' 
        TextBox5.Enabled = False
        TextBox5.Location = New Point(189, 240)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(57, 23)
        TextBox5.TabIndex = 39
        TextBox5.Text = "|"
        ' 
        ' CheckBox7
        ' 
        CheckBox7.AutoSize = True
        CheckBox7.Location = New Point(13, 242)
        CheckBox7.Name = "CheckBox7"
        CheckBox7.Size = New Size(159, 21)
        CheckBox7.TabIndex = 38
        CheckBox7.Text = "使用该分隔符拆分序列名"
        CheckBox7.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown3
        ' 
        NumericUpDown3.Enabled = False
        NumericUpDown3.Location = New Point(189, 269)
        NumericUpDown3.Name = "NumericUpDown3"
        NumericUpDown3.Size = New Size(57, 23)
        NumericUpDown3.TabIndex = 37
        NumericUpDown3.Value = New Decimal(New Integer() {3, 0, 0, 0})
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(28, 271)
        Label4.Name = "Label4"
        Label4.Size = New Size(104, 17)
        Label4.TabIndex = 36
        Label4.Text = "将该位置作为数量"
        ' 
        ' TextBox6
        ' 
        TextBox6.Enabled = False
        TextBox6.Location = New Point(188, 182)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(57, 23)
        TextBox6.TabIndex = 43
        TextBox6.Text = "|"
        ' 
        ' CheckBox8
        ' 
        CheckBox8.AutoSize = True
        CheckBox8.Location = New Point(12, 184)
        CheckBox8.Name = "CheckBox8"
        CheckBox8.Size = New Size(159, 21)
        CheckBox8.TabIndex = 42
        CheckBox8.Text = "使用该分隔符拆分序列名"
        CheckBox8.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown4
        ' 
        NumericUpDown4.Enabled = False
        NumericUpDown4.Location = New Point(188, 211)
        NumericUpDown4.Name = "NumericUpDown4"
        NumericUpDown4.Size = New Size(57, 23)
        NumericUpDown4.TabIndex = 41
        NumericUpDown4.Value = New Decimal(New Integer() {2, 0, 0, 0})
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(27, 213)
        Label6.Name = "Label6"
        Label6.Size = New Size(140, 17)
        Label6.TabIndex = 40
        Label6.Text = "将如该位置作为连续性状"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(472, 13)
        Label7.Name = "Label7"
        Label7.Size = New Size(83, 17)
        Label7.TabIndex = 45
        Label7.Text = "拆分结果预览:"
        ' 
        ' TextBox7
        ' 
        TextBox7.BackColor = SystemColors.Info
        TextBox7.Location = New Point(472, 37)
        TextBox7.Multiline = True
        TextBox7.Name = "TextBox7"
        TextBox7.ReadOnly = True
        TextBox7.ScrollBars = ScrollBars.Vertical
        TextBox7.Size = New Size(104, 315)
        TextBox7.TabIndex = 44
        TextBox7.WordWrap = False
        ' 
        ' TextBox8
        ' 
        TextBox8.Enabled = False
        TextBox8.Location = New Point(188, 299)
        TextBox8.Name = "TextBox8"
        TextBox8.Size = New Size(57, 23)
        TextBox8.TabIndex = 49
        TextBox8.Text = "|"
        ' 
        ' CheckBox9
        ' 
        CheckBox9.AutoSize = True
        CheckBox9.Location = New Point(12, 301)
        CheckBox9.Name = "CheckBox9"
        CheckBox9.Size = New Size(159, 21)
        CheckBox9.TabIndex = 48
        CheckBox9.Text = "使用该分隔符拆分序列名"
        CheckBox9.UseVisualStyleBackColor = True
        ' 
        ' NumericUpDown5
        ' 
        NumericUpDown5.Enabled = False
        NumericUpDown5.Location = New Point(188, 328)
        NumericUpDown5.Name = "NumericUpDown5"
        NumericUpDown5.Size = New Size(57, 23)
        NumericUpDown5.TabIndex = 47
        NumericUpDown5.Value = New Decimal(New Integer() {4, 0, 0, 0})
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(27, 330)
        Label8.Name = "Label8"
        Label8.Size = New Size(116, 17)
        Label8.TabIndex = 46
        Label8.Text = "将该位置作为物种名"
        ' 
        ' Config_Stand
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(583, 399)
        ControlBox = False
        Controls.Add(TextBox8)
        Controls.Add(CheckBox9)
        Controls.Add(NumericUpDown5)
        Controls.Add(Label8)
        Controls.Add(Label7)
        Controls.Add(TextBox7)
        Controls.Add(TextBox6)
        Controls.Add(CheckBox8)
        Controls.Add(NumericUpDown4)
        Controls.Add(Label6)
        Controls.Add(TextBox5)
        Controls.Add(CheckBox7)
        Controls.Add(NumericUpDown3)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(TextPreview)
        Controls.Add(TextBox4)
        Controls.Add(CheckBox6)
        Controls.Add(NumericUpDown2)
        Controls.Add(Label3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(CheckBox4)
        Controls.Add(TextBox2)
        Controls.Add(TextBox1)
        Controls.Add(Label2)
        Controls.Add(CheckBox3)
        Controls.Add(CheckBox5)
        Controls.Add(NumericUpDown1)
        Controls.Add(Label1)
        Controls.Add(TextBox3)
        Controls.Add(CheckBox2)
        Controls.Add(CheckBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Stand"
        StartPosition = FormStartPosition.CenterScreen
        Text = "序列标准化"
        TopMost = True
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown2, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown3, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown4, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown5, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents CheckBox2 As CheckBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents CheckBox5 As CheckBox
    Friend WithEvents CheckBox3 As CheckBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents CheckBox4 As CheckBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents NumericUpDown2 As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents CheckBox6 As CheckBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextPreview As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents CheckBox7 As CheckBox
    Friend WithEvents NumericUpDown3 As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents CheckBox8 As CheckBox
    Friend WithEvents NumericUpDown4 As NumericUpDown
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents TextBox8 As TextBox
    Friend WithEvents CheckBox9 As CheckBox
    Friend WithEvents NumericUpDown5 As NumericUpDown
    Friend WithEvents Label8 As Label
End Class
