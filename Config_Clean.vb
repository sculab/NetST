Imports System.IO
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Public Class Config_Clean
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim th1 As New Thread(AddressOf butt1)
        th1.Start()
        Me.Hide()
    End Sub
    Public Sub butt1()

        If clean_fasta(TextBox4.Text, root_path + "temp\temp_file.tmp", new_line(CheckBox1.Checked)) Then
            timer_id = 3
        Else
            timer_id = 0
        End If

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub


    Private Sub Config_Clean_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        Me.Hide()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        CheckBox4.Checked = True
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        CheckBox3.Checked = True
    End Sub

    Private Sub Config_Clean_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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
            Dim line As String = sr.ReadLine()
            Dim limit As Integer = 100
            TextPreview.Text = ""
            Do

                If line.StartsWith(">") Then
                    line = line.Replace(",", "_").Replace("""", "").Replace("'", "").Replace("=", "_")
                    TextPreview.Text += line.Substring(1) + vbCrLf
                    limit -= 1
                End If
                line = sr.ReadLine()
            Loop Until line Is Nothing Or limit = 0
            sr.Close()
        End If
    End Sub


End Class