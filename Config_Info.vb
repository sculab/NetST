Imports System.ComponentModel
Imports System.Threading
Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Config_Info

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim th1 As New Thread(AddressOf butt1)
        th1.Start()
        Me.Hide()
    End Sub
    Public Sub butt1()
        If get_info(TextBox4.Text, root_path + "temp\temp_file.tmp", new_line(CheckBox1.Checked)) Then
            timer_id = 4
        Else
            timer_id = 0
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub Config_Info_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Config_Info_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "Fasta(*.*)|*.fas;*.FAS;*.fasta;*.FASTA"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".fasta"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox4.Text = opendialog.FileName
            Dim sr As New StreamReader(TextBox4.Text)
            TextPreview.Text = sr.ReadLine()
            sr.Close()
        End If
    End Sub

    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        TextBox5.Enabled = CheckBox8.Checked Xor False
        NumericUpDown1.Enabled = CheckBox8.Checked Xor False
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        TextBox1.Enabled = CheckBox6.Checked Xor False
        NumericUpDown2.Enabled = CheckBox6.Checked Xor False
    End Sub
End Class