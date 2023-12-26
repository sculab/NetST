Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Public Class Config_Split
    Private Sub Config_Split_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim opendialog As New FolderBrowserDialog
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox4.Text = opendialog.SelectedPath
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox4.Text = "" Then
            MsgBox("The output folder cannot be empty.")
        Else
            waiting = True
            timer_id = 1
            Dim th1 As New Threading.Thread(AddressOf But1)
            th1.Start()
            Me.Hide()
        End If



    End Sub
    Public Sub But1()
        Dim currentTime As DateTime = DateTime.Now
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim target_file As String = ""
        Dim dis_id As Integer = 0
        If RadioButton2.Checked Then
            dis_id = 3
        End If


        If data_type = "fas" Then
            target_file = root_path + "history\" + currentTimeStamp.ToString + ".fasta"
            Dim sw As New StreamWriter(target_file)
            For i As Integer = 1 To dtView.Count
                If form_main.DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    sw.WriteLine(">" + dtView.Item(i - 1).Item(6) + "#" + dtView.Item(i - 1).Item(dis_id).ToString)
                    sw.WriteLine(fasta_seq(i))
                End If
            Next
            sw.Close()
        End If

        If data_type = "gb" Then
            target_file = root_path + "history\" + currentTimeStamp.ToString
            If Directory.Exists(target_file) = False Then
                My.Computer.FileSystem.CreateDirectory(target_file)
            End If
            For i As Integer = 1 To dtView.Count
                If form_main.DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    File.Copy(root_path + "history\out_gb\" + dtView.Item(i - 1).Item(0).ToString + ".gb", target_file + "\" + dtView.Item(i - 1).Item(6) + "#" + dtView.Item(i - 1).Item(dis_id).ToString + ".gb")
                End If
            Next
        End If

        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = currentDirectory + "analysis\build_primer.exe" ' 替换为实际的命令行程序路径
        startInfo.WorkingDirectory = currentDirectory + "analysis\" ' 替换为实际的运行文件夹路径
        'startInfo.CreateNoWindow = True
        startInfo.Arguments = "-input " + """" + target_file + """"
        startInfo.Arguments += " -min_seq_length " + NumericUpDown2.Value.ToString + " -max_seq_length " + NumericUpDown3.Value.ToString
        startInfo.Arguments += " -soft_boundary " + NumericUpDown4.Value.ToString
        startInfo.Arguments += " -split_only True"
        startInfo.Arguments += " -do_aln " + CheckBox1.Checked.ToString
        startInfo.Arguments += " -out_dir " + """" + TextBox4.Text + """"
        Dim process As Process = Process.Start(startInfo)
        process.WaitForExit()
        process.Close()

        Dim sw4 As New StreamWriter(currentDirectory + "history/history.csv", True)
        sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + ",Primer," + TextBox4.Text)
        sw4.Close()

        timer_id = 6
    End Sub

    Private Sub Config_Split_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If data_type = "gb" Then
            Label4.Visible = True
            NumericUpDown4.Visible = True
            If language = "CH" Then
                Label2.Text = "基因最小长度"
                Label3.Text = "基因最大长度"
            Else
                Label2.Text = "Minimum Gene Length"
                Label3.Text = "Maximum Gene Length"
            End If
        End If
        If data_type = "fas" Then
            Label4.Visible = False
            NumericUpDown4.Visible = False
            CheckBox1.Visible = False
            If language = "CH" Then
                Label2.Text = "片段重叠长度"
                Label3.Text = "片段分隔长度"
                CheckBox1.Text = "比对获取的序列"

            Else
                Label2.Text = "Fragment Overlap Length"
                Label3.Text = "Fragment Separation Length"
                CheckBox1.Text = "Align sequences"
            End If
        End If
    End Sub
End Class