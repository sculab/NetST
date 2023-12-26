Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Public Class Config_Mix
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RadioButton1.Checked = True
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "Result File (*_mix.csv)|*_mix.csv;*.CSV"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = "_mix.csv"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox1.Text = opendialog.FileName
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim opendialog As New FolderBrowserDialog
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox2.Text = opendialog.SelectedPath
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim opendialog As New FolderBrowserDialog
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox3.Text = opendialog.SelectedPath
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Hide()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If TextBox2.Text <> "" And TextBox3.Text <> "" And ((RadioButton1.Checked And TextBox1.Text <> "") Or RadioButton1.Checked = False) Then
            waiting = True
            timer_id = 1
            Dim th1 As New Threading.Thread(AddressOf But5)
            th1.Start()
            Me.Hide()
        End If

    End Sub
    Public Sub But5()
        Dim currentTime As DateTime = DateTime.Now
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim target_file As String = root_path + "history\" + currentTimeStamp.ToString + ".fasta"

        Dim sw As New StreamWriter(target_file)
        For i As Integer = 1 To dtView.Count
            If form_main.DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                sw.WriteLine(">" + dtView.Item(i - 1).Item(1))
                sw.WriteLine(fasta_seq(i))
            End If
        Next
        sw.Close()
        Dim k_value As String = NumericUpDown1.Value.ToString
        Dim cut_value As String = NumericUpDown2.Value.ToString
        Dim dict_file, ref_file, result_file As String
        Dim mix_file As String = ""
        result_file = root_path + "history\" + currentTimeStamp.ToString
        If RadioButton1.Checked Then
            mix_file = TextBox1.Text
        End If
        If RadioButton2.Checked Then
            ref_file = (TextBox2.Text + "\ref_combine.fasta").Replace("\\", "\")
            dict_file = root_path + "history\" + currentTimeStamp.ToString + ".gz"
            Dim startInfo As New ProcessStartInfo()
            startInfo.FileName = currentDirectory + "analysis\GetType.exe" ' 替换为实际的命令行程序路径
            startInfo.WorkingDirectory = currentDirectory + "history\" ' 替换为实际的运行文件夹路径
            'startInfo.CreateNoWindow = True
            startInfo.Arguments = "-k " + k_value + " -db " + """" + dict_file + """" + " -ref " + """" + ref_file + """" + " -input " + """" + target_file + """" + " -output " + """" + result_file + """" + " -cut " + cut_value
            Dim process As Process = Process.Start(startInfo)
            process.WaitForExit()
            process.Close()
            mix_file = result_file + "_mix.csv"
            File.Copy(mix_file, TextBox3.Text + "\" + currentTimeStamp.ToString + "_mix.csv")
        End If

        Dim startInfo_mix As New ProcessStartInfo()
        startInfo_mix.FileName = currentDirectory + "analysis\GetMixSeq.exe" ' 替换为实际的命令行程序路径
        startInfo_mix.WorkingDirectory = currentDirectory + "history\" ' 替换为实际的运行文件夹路径
        'startInfo_mix.CreateNoWindow = True
        startInfo_mix.Arguments = "-k " + k_value + " -ref_dir " + """" + TextBox2.Text + """" + " -input " + """" + mix_file + """" + " -out_dir " + """" + TextBox3.Text + """" + " -max " + NumericUpDown3.Value.ToString + " -t " + NumericUpDown4.Value.ToString
        Dim process_mix As Process = Process.Start(startInfo_mix)
        process_mix.WaitForExit()
        process_mix.Close()

        Dim sw0 As New StreamWriter(currentDirectory + "history\" + currentTimeStamp.ToString + ".html")
        Dim sr1 As New StreamReader(currentDirectory + "main/" + language + "/header_mix.txt")
        sw0.Write(sr1.ReadToEnd)
        sr1.Close()
        Dim sr2 As New StreamReader(mix_file)
        Dim line As String = sr2.ReadLine
        line = sr2.ReadLine
        If line <> "" Then
            Do
                Dim line_list As String() = line.Split(",")
                sw0.WriteLine("<tr><td>" + line_list(0) + "</td><td>" + line_list(1) + "</td><td>" + line_list(2) + "</td><td>" + line_list(3) + "</td><td>" + line_list(4) + "</td></tr>")
                line = sr2.ReadLine
            Loop Until line Is Nothing
        End If
        sr2.Close()
        Dim sr3 As New StreamReader(currentDirectory + "main/" + language + "/footer_type.txt")
        sw0.Write(sr3.ReadToEnd)
        sw0.Close()

        Dim sw4 As New StreamWriter(currentDirectory + "history/history.csv", True)
        sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + ",Genotyping")
        sw4.Close()

        current_file = currentTimeStamp
        timer_id = 5
    End Sub

    Private Sub Config_Mix_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class