Public Class Config_Combine
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim opendialog As New FolderBrowserDialog
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox1.Text = opendialog.SelectedPath
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim savedialog As New SaveFileDialog
        savedialog.Filter = "Fasta file(*.fasta)|*.fas;*.fasta"
        savedialog.FileName = ""
        savedialog.DefaultExt = ".fasta"
        Dim resultdialog As DialogResult = savedialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox3.Text = savedialog.FileName
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If RadioButton1.Checked Then
            combine_file_vertical(TextBox1.Text, TextBox2.Text, TextBox3.Text)
        End If
        If RadioButton2.Checked Then
            combine_file_horizontal(TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox4.Text)
        End If
        Me.Hide()
    End Sub

    Private Sub Config_Combine_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class