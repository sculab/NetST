<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config_Combine
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
        Button5 = New Button()
        TextBox3 = New TextBox()
        Button3 = New Button()
        Button4 = New Button()
        Button1 = New Button()
        TextBox1 = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        TextBox2 = New TextBox()
        Label3 = New Label()
        RadioButton1 = New RadioButton()
        RadioButton2 = New RadioButton()
        TextBox4 = New TextBox()
        Label4 = New Label()
        SuspendLayout()
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(252, 166)
        Button5.Name = "Button5"
        Button5.Size = New Size(75, 30)
        Button5.TabIndex = 26
        Button5.Text = "浏览"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(14, 170)
        TextBox3.Name = "TextBox3"
        TextBox3.ReadOnly = True
        TextBox3.Size = New Size(233, 23)
        TextBox3.TabIndex = 25
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(199, 202)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 30)
        Button3.TabIndex = 24
        Button3.Text = "取消"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(70, 203)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 30)
        Button4.TabIndex = 23
        Button4.Text = "确定"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(252, 54)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 30)
        Button1.TabIndex = 22
        Button1.Text = "浏览"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(14, 58)
        TextBox1.Name = "TextBox1"
        TextBox1.ReadOnly = True
        TextBox1.Size = New Size(233, 23)
        TextBox1.TabIndex = 21
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(14, 38)
        Label1.Name = "Label1"
        Label1.Size = New Size(131, 17)
        Label1.TabIndex = 27
        Label1.Text = "存放待合并文件的位置:"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(14, 93)
        Label2.Name = "Label2"
        Label2.Size = New Size(119, 17)
        Label2.TabIndex = 28
        Label2.Text = "待合并文件的扩展名:"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(172, 90)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(75, 23)
        TextBox2.TabIndex = 29
        TextBox2.Text = ".fasta,.fas"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(14, 150)
        Label3.Name = "Label3"
        Label3.Size = New Size(107, 17)
        Label3.TabIndex = 30
        Label3.Text = "合并后文件保存在:"
        ' 
        ' RadioButton1
        ' 
        RadioButton1.AutoSize = True
        RadioButton1.Checked = True
        RadioButton1.Location = New Point(14, 12)
        RadioButton1.Name = "RadioButton1"
        RadioButton1.Size = New Size(86, 21)
        RadioButton1.TabIndex = 31
        RadioButton1.TabStop = True
        RadioButton1.Text = "按文件合并"
        RadioButton1.UseVisualStyleBackColor = True
        ' 
        ' RadioButton2
        ' 
        RadioButton2.AutoSize = True
        RadioButton2.Location = New Point(172, 12)
        RadioButton2.Name = "RadioButton2"
        RadioButton2.Size = New Size(86, 21)
        RadioButton2.TabIndex = 32
        RadioButton2.Text = "按物种合并"
        RadioButton2.UseVisualStyleBackColor = True
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(172, 119)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(75, 23)
        TextBox4.TabIndex = 34
        TextBox4.Text = "N"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(14, 122)
        Label4.Name = "Label4"
        Label4.Size = New Size(107, 17)
        Label4.TabIndex = 33
        Label4.Text = "缺失序列填充字符:"
        ' 
        ' Config_Combine
        ' 
        AutoScaleDimensions = New SizeF(7F, 17F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(339, 241)
        ControlBox = False
        Controls.Add(TextBox4)
        Controls.Add(Label4)
        Controls.Add(RadioButton2)
        Controls.Add(RadioButton1)
        Controls.Add(Label3)
        Controls.Add(TextBox2)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(Button5)
        Controls.Add(TextBox3)
        Controls.Add(Button3)
        Controls.Add(Button4)
        Controls.Add(Button1)
        Controls.Add(TextBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Name = "Config_Combine"
        StartPosition = FormStartPosition.CenterScreen
        Text = "合并序列文件"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button5 As Button
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label4 As Label
End Class
