Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Config_Type
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "fasta序列文件(*.*)|*.fas;*.fasta"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".fasta"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox1.Text = opendialog.FileName
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "压缩的数据库文件(*.*)|*.gz;*.GZ"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".gz"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox2.Text = opendialog.FileName
        End If
    End Sub

    Private Sub Config_Type_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            RadioButton1.Checked = False
            RadioButton3.Checked = False
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            RadioButton1.Checked = False
            RadioButton2.Checked = False
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            RadioButton2.Checked = False
            RadioButton3.Checked = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        waiting = True
        timer_id = 1
        Dim th1 As New Threading.Thread(AddressOf But1)
        th1.Start()
        Me.Hide()
    End Sub
    Public Sub But1()
        Dim currentTime As DateTime = DateTime.Now
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim in_path As String = root_path + "results\" + currentTimeStamp.ToString + ".fasta"

        Dim sw As New StreamWriter(in_path)
        For i As Integer = 1 To dtView.Count
            If form_main.DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                sw.WriteLine(">" + dtView.Item(i - 1).Item(1))
                sw.WriteLine(fasta_seq(2))
            End If
        Next
        sw.Close()
        Dim k_value As String = NumericUpDown1.Value.ToString
        Dim dict_file, ref_file, target_file, result_file As String
        If RadioButton1.Checked Then
            ref_file = TextBox1.Text
            dict_file = root_path + "results\" + currentTimeStamp.ToString + ".gz"

        End If
        If RadioButton2.Checked Then
            ref_file = "nofile"
            dict_file = TextBox2.Text
        End If
        If RadioButton3.Checked Then
            Select Case ComboBox1.SelectedIndex
                Case 0
                    ref_file = "nofile"
                    dict_file = root_path + "kdicts\influenza_ha.gz"
                Case Else
            End Select
        End If
        target_file = in_path
        result_file = root_path + "results\" + currentTimeStamp.ToString

        'PB_value = 10
        Dim out_path As String = root_path + "results\" + currentTimeStamp.ToString + "_aln.fasta"

        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = currentDirectory + "analysis\GetType.exe" ' 替换为实际的命令行程序路径
        startInfo.WorkingDirectory = currentDirectory + "results\" ' 替换为实际的运行文件夹路径
        'startInfo.CreateNoWindow = True
        startInfo.Arguments = "-k " + k_value + " -db " + """" + dict_file + """" + " -ref " + """" + ref_file + """" + " -input " + """" + target_file + """" + " -output " + """" + result_file + """" + " -cut 0"
        Dim process As Process = Process.Start(startInfo)
        process.WaitForExit()
        process.Close()

        Dim sw0 As New StreamWriter(currentDirectory + "results\" + currentTimeStamp.ToString + ".html")
        Dim sr1 As New StreamReader(currentDirectory + "main\" + language + "\header_type.txt")
        sw0.Write(sr1.ReadToEnd)
        sr1.Close()
        Dim sr2 As New StreamReader(currentDirectory + "results\" + currentTimeStamp.ToString + ".csv")
        Dim line As String = sr2.ReadLine
        line = sr2.ReadLine
        If line <> "" Then
            Do
                Dim line_list As String() = line.Split(",")
                If CInt(line_list(4).Replace("%", "")) > 10 Then
                    sw0.WriteLine("<tr><td>" + line_list(0) + "</td><td>" + line_list(1) + "</td><td>" + line_list(2) + "</td><td>" + line_list(3) + "</td><td>" + line_list(4) + "</td><td>" + line_list(5) + "</td><td>" + line_list(6) + "</td></tr>")
                End If
                line = sr2.ReadLine
            Loop Until line Is Nothing
        End If
        sr2.Close()
        Dim sr3 As New StreamReader(currentDirectory + "main/" + language + "/footer_type.txt")
        sw0.Write(sr3.ReadToEnd)
        sw0.Close()

        Dim sw4 As New StreamWriter(currentDirectory + "results/history.csv", True)
        sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + ",Genotyping")
        sw4.Close()

        current_file = currentTimeStamp
        timer_id = 5
    End Sub
End Class