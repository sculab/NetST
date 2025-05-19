Imports System.IO
Imports System.Text
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Public Class Mainform
    Public Taxon_Dataset As New DataSet
    Delegate Sub SetUrl(ByVal url As String)
    Dim set_web_main_url As SetUrl

    Delegate Sub AppendText(ByVal text As String)
    Dim Append_Text As AppendText

    Public Sub Append_info(ByVal info As String)
        Me.Invoke(Append_Text, New Object() {Format(Now(), "yyyy/MM/dd H:mm:ss") + vbTab + info + Chr(13)})
    End Sub

    Public Sub initialize_data()
        Dim Column_Select As New DataGridViewCheckBoxColumn
        Column_Select.HeaderText = "Select"
        DataGridView1.Columns.Insert(0, Column_Select)
        DataGridView1.AllowUserToAddRows = False

        Dim taxon_table As New System.Data.DataTable
        taxon_table.TableName = "Taxon Table"
        Dim Column_ID As New System.Data.DataColumn("ID", System.Type.GetType("System.Int32"))
        Dim Column_Taxon As New System.Data.DataColumn("Name")
        Dim Column_Seq As New System.Data.DataColumn("Sequence")
        Dim Column_Time As New System.Data.DataColumn("Continuous Traits")
        Dim Column_State As New System.Data.DataColumn("Discrete Traits")
        Dim Column_Count As New System.Data.DataColumn("Quantity")
        Dim Column_Organism As New System.Data.DataColumn("Organism")
        taxon_table.Columns.Add(Column_ID)
        taxon_table.Columns.Add(Column_Taxon)
        taxon_table.Columns.Add(Column_Seq)
        taxon_table.Columns.Add(Column_State)
        taxon_table.Columns.Add(Column_Time)
        taxon_table.Columns.Add(Column_Count)
        taxon_table.Columns.Add(Column_Organism)
        Taxon_Dataset.Tables.Add(taxon_table)

        dtView = Taxon_Dataset.Tables("Taxon Table").DefaultView
        dtView.AllowNew = False
        dtView.AllowDelete = False
        dtView.AllowEdit = False
    End Sub
    Private Sub RT_Append_Text(ByVal text As String)
        RichTextBox1.AppendText(text)
        RichTextBox1.Refresh()
    End Sub
    Private Sub web_main_seturl(ByVal url As String)
        WebView_main.Source = New Uri(url)
    End Sub

    Private Sub Mainform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WebView_main.CreationProperties = New CoreWebView2CreationProperties With {
            .BrowserExecutableFolder = Path.Combine(root_path, "webview")
        }
        CheckForIllegalCrossThreadCalls = False
        currentDirectory = Application.StartupPath
        set_web_main_url = New SetUrl(AddressOf web_main_seturl)
        Append_Text = New AppendText(AddressOf RT_Append_Text)
        WebView_main.Source = New Uri("file:///" + currentDirectory + "/main/" + language + "/main.html")
        initialize_data()
    End Sub
    Private Sub WebView_main_CoreWebView2InitializationCompleted(sender As Object, e As CoreWebView2InitializationCompletedEventArgs) Handles WebView_main.CoreWebView2InitializationCompleted
        WebView_main.CoreWebView2.Settings.IsStatusBarEnabled = False
        WebView_main.CoreWebView2.Settings.AreDefaultContextMenusEnabled = False
    End Sub

    Private Sub 载入序列ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 载入序列ToolStripMenuItem.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "Fasta File(*.fasta)|*.fas;*.fasta;*.fa"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".fas"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            current_file = opendialog.FileName
            Taxon_Dataset.Tables("Taxon Table").Clear()
            DataGridView1.DataSource = Nothing
            TabControl1.SelectedIndex = 1
            data_loaded = False
            form_config_stand.Show()
        End If
    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Select Case timer_id
            Case 0
                ProgressBar1.Value = PB_value
                TextBox1.Text = info_text
            Case 1
                If waiting Then
                    TabControl1.SelectedIndex = 0
                    Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                    waiting = False
                End If

            Case 2
                Timer1.Enabled = False
                DataGridView1.DataSource = dtView
                dtView.AllowNew = False
                dtView.AllowEdit = True

                DataGridView1.Columns(1).ReadOnly = True
                DataGridView1.Columns(2).ReadOnly = True
                DataGridView1.Columns(3).ReadOnly = True
                DataGridView1.Columns(4).ReadOnly = False

                DataGridView1.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
                DataGridView1.Columns(0).Width = 50
                DataGridView1.Columns(1).Width = 50
                DataGridView1.Columns(2).Width = 120
                DataGridView1.Columns(3).Width = 400
                DataGridView1.Columns(4).Width = 120
                DataGridView1.Columns(5).Width = 120
                DataGridView1.Columns(6).Width = 60
                DataGridView1.Columns(7).Width = 120

                PB_value = 0
                info_text = ""
                Timer1.Enabled = True
                DataGridView1.RefreshEdit()
                GC.Collect()
                timer_id = 0
                data_loaded = True
            Case 3
                Timer1.Enabled = False
                Dim savedialog As New SaveFileDialog
                savedialog.Filter = "fasta File(*.fasta)|*.fas;*.fasta"
                savedialog.FileName = ""
                savedialog.DefaultExt = ".fasta"
                Dim resultdialog As DialogResult = savedialog.ShowDialog()
                If resultdialog = DialogResult.OK Then
                    safe_copy(root_path + "temp\temp_file.tmp", savedialog.FileName)
                    show_info("save to " + savedialog.FileName)
                End If
                timer_id = 0
                PB_value = 0
                info_text = ""
                Timer1.Enabled = True
                GC.Collect()
            Case 4
                Timer1.Enabled = False
                Dim savedialog As New SaveFileDialog
                savedialog.Filter = "csv file(*.csv)|*.csv;*.CSV"
                savedialog.FileName = ""
                savedialog.DefaultExt = ".csv"
                Dim resultdialog As DialogResult = savedialog.ShowDialog()
                If resultdialog = DialogResult.OK Then
                    safe_copy(root_path + "temp\temp_file.tmp", savedialog.FileName)
                    show_info("save to " + savedialog.FileName)
                End If
                timer_id = 0
                PB_value = 0
                info_text = ""
                Timer1.Enabled = True
                GC.Collect()
            Case 5
                Timer1.Enabled = False
                Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "history/" + current_file + ".html"})
                timer_id = 0
                PB_value = 0
                info_text = ""
                Timer1.Enabled = True
            Case 6
                Timer1.Enabled = False
                分析记录ToolStripMenuItem_Click(sender, e)
                timer_id = 0
                PB_value = 0
                info_text = ""
                Timer1.Enabled = True
            Case Else

        End Select
    End Sub

    Private Sub 载入数据ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 载入数据ToolStripMenuItem.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "CSV File (*.csv)|*.csv;*.CSV|ALL Files(*.*)|*.*"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".csv"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TabControl1.SelectedIndex = 1
            DataGridView1.EndEdit()
            Taxon_Dataset.Tables("Taxon Table").Clear()
            DataGridView1.DataSource = Nothing
            data_loaded = False
            Dim th1 As New Threading.Thread(AddressOf load_csv_data)
            th1.Start(opendialog.FileName)
        End If
    End Sub
    Public Sub load_csv_data(ByVal file_path As String)
        Dim dr As StreamReader = Nothing
        Try
            dr = New StreamReader(file_path)
            Dim line As String = dr.ReadLine
            Dim count As Integer = 0
            Dim pre_fasta_seq As Integer = 0
            If add_data Then
                pre_fasta_seq = UBound(fasta_seq)
            End If
            line = dr.ReadLine
            dtView.AllowNew = True
            Do
                If line <> "" Then
                    count += 1
                    dtView.AddNew()
                    Dim newrow() As String = line.Split(",")
                    ReDim Preserve newrow(6)
                    newrow(0) = pre_fasta_seq + count
                    ReDim Preserve fasta_seq(count)
                    fasta_seq(count) = newrow(2)
                    If fasta_seq(count).Length > 1000 Then
                        newrow(2) = newrow(2).Substring(0, 1000)
                    End If
                    dtView.Item(pre_fasta_seq + count - 1).Row.ItemArray = newrow
                End If
                line = dr.ReadLine
            Loop Until line Is Nothing

            dr.Close()
            add_data = False
            Append_info("Data loaded successfully!")
            timer_id = 2

        Catch ex As Exception
            MsgBox(ex.Message)
            If dr IsNot Nothing Then
                dr.Close()
            End If
            add_data = False
            timer_id = 0
        End Try

    End Sub

    Private Sub 保存数据ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 保存数据ToolStripMenuItem.Click
        Dim opendialog As New SaveFileDialog
        opendialog.Filter = "CSV File (*.csv)|*.csv;*.CSV"
        opendialog.FileName = ""
        opendialog.DefaultExt = ".csv"
        opendialog.CheckFileExists = False
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            If opendialog.FileName.ToLower.EndsWith(".csv") Then
                Dim dw As New StreamWriter(opendialog.FileName, False)
                Dim state_line As String = "ID,Name"
                For j As Integer = 3 To DataGridView1.ColumnCount - 1
                    state_line += "," + DataGridView1.Columns(j).HeaderText
                Next
                dw.WriteLine(state_line)
                For i As Integer = 1 To dtView.Count
                    state_line = i.ToString
                    For j As Integer = 2 To DataGridView1.ColumnCount - 1
                        If j = 3 Then
                            state_line += "," + fasta_seq(i)
                        Else
                            state_line += "," + dtView.Item(i - 1).Item(j - 1)
                        End If
                    Next
                    dw.WriteLine(state_line)
                Next
                dw.Close()
            End If
            Append_info("Save Successfully!")
        End If
    End Sub



    Private Sub 全选ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 全选ToolStripMenuItem.Click
        For i As Integer = 1 To dtView.Count
            DataGridView1.Rows(i - 1).Cells(0).Value = True
        Next
        DataGridView1.RefreshEdit()
    End Sub

    Private Sub 清除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清除ToolStripMenuItem.Click
        For i As Integer = 1 To dtView.Count
            DataGridView1.Rows(i - 1).Cells(0).Value = False
        Next
        DataGridView1.RefreshEdit()
    End Sub

    Private Sub 前进ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 前进ToolStripMenuItem.Click
        WebView_main.GoForward()

    End Sub

    Private Sub 后退ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 后退ToolStripMenuItem.Click
        WebView_main.GoBack()
    End Sub

    Private Sub 单倍型网络ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 单倍型网络ToolStripMenuItem.Click
        If dtView.Count >= 1 Then
            Dim selected_count As Integer = 0
            Dim checked As Boolean = True
            For i As Integer = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If

                If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Continuous Traits:" + DataGridView1.Rows(i - 1).Cells(6).Value + " is not a numerical value.")
                    Exit For
                End If
                If IsNumeric(DataGridView1.Rows(i - 1).Cells(6).Value) = False Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" + DataGridView1.Rows(i - 1).Cells(5).Value + " is not a numerical value.")
                    Exit For
                ElseIf CInt(DataGridView1.Rows(i - 1).Cells(6).Value) <= 0 Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" + DataGridView1.Rows(i - 1).Cells(5).Value + " cannot be less than zero.")
                    Exit For
                End If
            Next
            If checked Then
                If selected_count >= 1 Then
                    TabControl1.SelectedIndex = 0
                    Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                    Dim th1 As New Threading.Thread(AddressOf analysis_network)
                    th1.Start("modified_tcs")
                Else
                    MsgBox("Please select at least one sequence!")
                End If
            End If
        Else
            MsgBox("Please select at least one sequence!")
        End If
    End Sub
    Public Sub analysis_network(ByVal network_type As String)
        Dim currentTime As DateTime = DateTime.Now
        Dim success As Boolean = True
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim in_path As String = root_path + "history\" + currentTimeStamp.ToString + ".fasta"
        Dim sw As New StreamWriter(in_path)
        Dim isaligened As Boolean = True
        Dim epsilon_str As String = ""

        For i As Integer = 1 To dtView.Count
            If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                sw.WriteLine(">" + dtView.Item(i - 1).Item(1) + "=" + dtView.Item(i - 1).Item(5) + "=" + dtView.Item(i - 1).Item(4) + "$SPLIT$" + dtView.Item(i - 1).Item(3))
                sw.WriteLine(fasta_seq(i))
                If Len(fasta_seq(i)) <> Len(fasta_seq(1)) Then
                    isaligened = False
                End If
            End If
        Next
        sw.Close()
        PB_value = 10
        Dim out_path As String = root_path + "history\" + currentTimeStamp.ToString + "_aln.fasta"
        If isaligened Then
            File.Copy(in_path, out_path)
        Else
            Dim startInfo As New ProcessStartInfo()
            startInfo.FileName = currentDirectory + "analysis\mafft-win\mafft.bat" ' 替换为实际的命令行程序路径
            startInfo.WorkingDirectory = currentDirectory + "analysis\mafft-win\" ' 替换为实际的运行文件夹路径
            'startInfo.CreateNoWindow = True
            startInfo.Arguments = "--retree 2 --inputorder " + """" + in_path + """" + ">" + """" + out_path + """"
            Dim process As Process = Process.Start(startInfo)
            process.WaitForExit()
            process.Close()
        End If
        PB_value = 30
        If File.Exists(out_path) Then
            Dim encoding As Encoding = DetectFileEncoding(out_path)

            If encoding IsNot Nothing AndAlso Not encoding.Equals(Encoding.UTF8) Then
                ' 读取文件内容
                Dim fileContent As String = File.ReadAllText(out_path, encoding)

                ' 将文件内容以UTF-8编码保存
                File.WriteAllText(out_path, fileContent, Encoding.UTF8)
            End If
            Dim network_app As String = "fastHaN_win_intel.exe"
            If cpu_info.ToUpper.StartsWith("ARM") Then
                network_app = "fastHaN_win_arm.exe"
            End If
            If hap_fasta(out_path, root_path + "history\" + currentTimeStamp.ToString, new_line(False)) Then
                Dim startInfo_hap As New ProcessStartInfo()
                startInfo_hap.FileName = currentDirectory + "analysis\" + network_app ' 替换为实际的命令行程序路径
                startInfo_hap.WorkingDirectory = currentDirectory + "history\" ' 替换为实际的运行文件夹路径
                'startInfo_hap.CreateNoWindow = True
                startInfo_hap.Arguments = network_type + " -i " + """" + root_path + "history\" + currentTimeStamp.ToString + "_seq.phy" + """" + " -o " + """" + root_path + "history\" + currentTimeStamp.ToString + """" + epsilon_str
                Dim process_hap As Process = Process.Start(startInfo_hap)
                process_hap.WaitForExit()
                process_hap.Close()
                PB_value = 50
                If File.Exists(root_path + "history\" + currentTimeStamp.ToString + ".gml") Then
                    Dim startInfo_network As New ProcessStartInfo()
                    startInfo_network.FileName = currentDirectory + "analysis\GenNetworkConfig2.exe" ' 替换为实际的命令行程序路径
                    startInfo_network.WorkingDirectory = currentDirectory + "history\" ' 替换为实际的运行文件夹路径
                    'startInfo_network.CreateNoWindow = True
                    startInfo_network.Arguments = currentTimeStamp.ToString + ".gml " + currentTimeStamp.ToString + "_seq2hap.csv " + currentTimeStamp.ToString
                    Dim process_network As Process = Process.Start(startInfo_network)
                    process_network.WaitForExit()
                    process_network.Close()
                    PB_value = 80
                    If File.Exists(root_path + "history\" + currentTimeStamp.ToString + ".js") Then
                        Dim sw0 As New StreamWriter(currentDirectory + "history/" + currentTimeStamp.ToString + ".html")
                        Dim sr1 As New StreamReader(currentDirectory + "main/" + language + "/tcsBU.txt")
                        sw0.Write(sr1.ReadToEnd.Replace("$data$", currentTimeStamp.ToString + ".js"))
                        sr1.Close()
                        sw0.Close()
                        Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "history/" + currentTimeStamp.ToString + ".html"})
                    Else
                        MsgBox("Haplotype network construction failed. Please check the data.")
                        Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/main.html"})
                        success = False
                    End If
                End If
            End If
        Else
            MsgBox("Sequence alignment failed. Please check the sequences.")
            Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/main.html"})
            success = False
        End If
        PB_value = 100
        If success Then
            Dim sw4 As New StreamWriter(currentDirectory + "history/history.csv", True)
            sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + "," + network_type + "Haplotype network")
            sw4.Close()
        End If
        PB_value = 0


    End Sub

    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridView1.DataBindingComplete
        If data_loaded = False And dtView.Count > 0 Then
            For i As Integer = 1 To dtView.Count
                DataGridView1.Rows(i - 1).Cells(0).Value = True
            Next
        End If

    End Sub

    Private Sub WebView_main_Click(sender As Object, e As EventArgs) Handles WebView_main.Click

    End Sub

    Private Sub MJN单倍型网络ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MJN单倍型网络ToolStripMenuItem.Click
        If dtView.Count >= 1 Then
            Dim selected_count As Integer = 0
            Dim checked As Boolean = True
            For i As Integer = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If

                If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Date:" + DataGridView1.Rows(i - 1).Cells(6).Value + " is not a numerical value.")
                    Exit For
                End If
                If IsNumeric(DataGridView1.Rows(i - 1).Cells(6).Value) = False Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" + DataGridView1.Rows(i - 1).Cells(5).Value + " is not a numerical value.")
                    Exit For
                ElseIf CInt(DataGridView1.Rows(i - 1).Cells(6).Value) <= 0 Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" + DataGridView1.Rows(i - 1).Cells(5).Value + " cannot be less than zero.")
                    Exit For
                End If
            Next
            If checked Then
                If selected_count >= 1 Then
                    TabControl1.SelectedIndex = 0
                    Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                    Dim th1 As New Threading.Thread(AddressOf analysis_network)
                    th1.Start("mjn")
                Else
                    MsgBox("Please select at least one sequence!")
                End If
            End If
        Else
            MsgBox("Please select at least one sequence!")
        End If
    End Sub

    Private Sub MSN单倍型网络ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MSN单倍型网络ToolStripMenuItem.Click
        If dtView.Count >= 1 Then
            Dim selected_count As Integer = 0
            Dim checked As Boolean = True
            For i As Integer = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If

                If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Date:" + DataGridView1.Rows(i - 1).Cells(6).Value + " is not a numerical value.")
                    Exit For
                End If
                If IsNumeric(DataGridView1.Rows(i - 1).Cells(6).Value) = False Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" + DataGridView1.Rows(i - 1).Cells(5).Value + " is not a numerical value.")
                    Exit For
                ElseIf CInt(DataGridView1.Rows(i - 1).Cells(6).Value) <= 0 Then
                    checked = False
                    MsgBox("ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" + DataGridView1.Rows(i - 1).Cells(5).Value + " cannot be less than zero.")
                    Exit For
                End If
            Next
            If checked Then
                If selected_count >= 1 Then
                    TabControl1.SelectedIndex = 0
                    Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})

                    Dim th1 As New Threading.Thread(AddressOf analysis_network)
                    th1.Start("msn")
                Else
                    MsgBox("Please select at least one sequence!")
                End If
            End If
        Else
            MsgBox("Please select at least one sequence!")
        End If
    End Sub

    Private Sub 分析记录ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 分析记录ToolStripMenuItem.Click
        If File.Exists(currentDirectory + "history/history.csv") = False Then
            File.Create(currentDirectory + "history/history.csv").Close()
        End If
        Dim sw As New StreamWriter(currentDirectory + "history/history.html")
        Dim sr1 As New StreamReader(currentDirectory + "main/" + language + "/header_history.txt")
        sw.Write(sr1.ReadToEnd)
        sr1.Close()
        Dim sr2 As New StreamReader(currentDirectory + "history/history.csv")
        Dim lines() As String = sr2.ReadToEnd.Split(vbCrLf)

        For Each line In lines.Reverse
            If line <> "" Then
                Do
                    Dim line_list As String() = line.Split(",")
                    sw.WriteLine("<tr><td>" + line_list(0) + "</td><td>" + line_list(1) + "</td><td>" + line_list(2) + "</td> ")
                    If line_list(2).Contains("Haplotype") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + ".html'>View Results</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + "_seq.phy' target='_new'>Original Sequence</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_seq_trait.csv'  target='_new'>Trait Matrix</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + "_hap.phy' target='_new'>Haplotype Sequence</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_hap_trait.csv'  target='_new'>Haplotype Trait Matrix</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + "_seq2hap.csv'  target='_new'>Haplotype Source</a></td></tr>")
                        ' 处理新的分析类型
                    ElseIf line_list(2).Contains("Topology Analysis") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + "_VisualHapNet.svg' target='_new'>Network Topology</a>")
                        sw.WriteLine("</td></tr>")
                    ElseIf line_list(2).Contains("Community Structure Analysis") Then
                        ' 从记录中提取k值
                        Dim kValues As New List(Of String)()

                        If line_list(2).Contains("k=") Then
                            ' 获取 k= 后面的部分
                            Dim startIndex As Integer = line_list(2).IndexOf("k=") + 2
                            Dim endIndex As Integer = line_list(2).Length

                            ' 如果有右括号，调整结束位置
                            If line_list(2).IndexOf(")", startIndex) > -1 Then
                                endIndex = line_list(2).IndexOf(")", startIndex)
                            End If

                            ' 提取 k 值部分
                            Dim kPart As String = line_list(2).Substring(startIndex, endIndex - startIndex)

                            ' 分割并添加所有 k 值
                            For Each k As String In kPart.Split(" "c)
                                If Not String.IsNullOrWhiteSpace(k.Trim()) Then
                                    kValues.Add(k.Trim())
                                End If
                            Next
                        End If

                        ' 如果没有找到有效的 k 值，设置默认值
                        If kValues.Count = 0 Then
                            kValues.Add("3")
                        End If

                        sw.WriteLine("<td>")

                        ' 为每个 k 值生成链接，所有链接放在同一行
                        For i As Integer = 0 To kValues.Count - 1
                            Dim k As String = kValues(i)

                            ' 添加社区可视化链接
                            sw.WriteLine("<a href='./" + line_list(1) + "_community_k" + k + ".svg' target='_new'>Community (k=" + k + ")</a>")

                            ' 检查是否有相应的文本文件，如果有则添加链接
                            If File.Exists(currentDirectory + "history/" + line_list(1) + "_community_k" + k + ".txt") Then
                                sw.WriteLine("<a href='./" + line_list(1) + "_community_k" + k + ".txt' target='_new'>[Details]</a>")
                            End If

                            ' 如果不是最后一个k值，添加分隔符
                            If i < kValues.Count - 1 Then
                                sw.WriteLine(" ")
                            End If
                        Next

                        sw.WriteLine("</td></tr>")
                    ElseIf line_list(2).Contains("Modularity Analysis") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + "_modularity_vs_communities.svg' target='_new'>Modularity Curve</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_highest_modularity_community.svg' target='_new'>Best Community Structure</a>")
                        sw.WriteLine("</td></tr>")
                    ElseIf line_list(2).Contains("Hap Sequence Analysis") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + "_sequence_conservation.svg' target='_new'>conservation site</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_p-distance_heatmap.svg' target='_new'>p-distance heatmap</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_p-distance_pca.svg' target='_new'>p-distance pca</a>")
                        sw.WriteLine("</td></tr>")
                    ElseIf line_list(2).Contains("Population Analysis") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + "_population_stats.txt' target='_new'>Population Statistics</a></td></tr>")
                    ElseIf line_list(2).Contains("Sequence Alignment") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + ".html'>View Results</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + "_aln.fasta'  target='_new'>Sequence Alignment</a></td></tr>")
                    ElseIf line_list(2).Contains("Genotyping") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + ".html'>View Results</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + ".csv'  target='_new'>Genotyping Results</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_null.csv'  target='_new'>Failed Sequences</a> ")
                        If File.Exists(currentDirectory + "history/" + line_list(1) + ".gz") Then
                            sw.WriteLine("<a href='./" + line_list(1) + ".gz'  target='_new'>Genotyping Database</a></td></tr>")
                        Else
                            sw.WriteLine("</tr>")
                        End If
                    ElseIf line_list(2).Contains("Primer") Then
                        sw.WriteLine("<td><a href='" + line_list(3) + "'  target='_new'>Primer Output</a></td></tr>")
                    Else
                        sw.WriteLine("<td><a href='./" + line_list(1) + ".html'>View Results</a>")
                        sw.WriteLine("<a Then href='./" + line_list(1) + ".0.json' target='_new'>json</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + ".fasta'  target='_new'>Sequence</a></td></tr>")
                    End If
                    line = sr2.ReadLine

                Loop Until line Is Nothing
            End If
        Next

        sr2.Close()
        Dim sr3 As New StreamReader(currentDirectory + "main/" + language + "/footer_history.txt")
        sw.Write(sr3.ReadToEnd)
        sw.Close()

        WebView_main.Source = New Uri("file:///" + currentDirectory + "/history/history.html")
    End Sub

    Private Sub 序列比对ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 序列比对ToolStripMenuItem.Click
        'DataGridView1.EndEdit()
        'DataGridView1.RefreshEdit()
        'If dtView.Count > 1 Then
        '    Dim selected_count As Integer = 0
        '    For i As Integer = 1 To dtView.Count
        '        If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
        '            selected_count += 1
        '        End If
        '    Next
        '    If selected_count > 1 Then
        '        TabControl1.SelectedIndex = 0
        '        Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
        '        Dim th1 As New Threading.Thread(AddressOf analysis_align)
        '        th1.Start("2")
        '    Else
        '        MsgBox("Please select at least two sequences!")
        '    End If
        'End If

    End Sub


    'Private Sub 载入待处理序列ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 载入待处理序列ToolStripMenuItem.Click
    '    Dim opendialog As New OpenFileDialog
    '    opendialog.Filter = "Fasta文件(*.*)|*.fas;*.fasta"
    '    opendialog.FileName = ""
    '    opendialog.Multiselect = False
    '    opendialog.DefaultExt = ".fas"
    '    opendialog.CheckFileExists = True
    '    opendialog.CheckPathExists = True
    '    Dim resultdialog As DialogResult = opendialog.ShowDialog()
    '    If resultdialog = DialogResult.OK Then
    '        TabControl1.SelectedIndex = 2
    '        TextBox6.Text = opendialog.FileName
    '    End If
    'End Sub

    Private Sub 获取序列信息ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 获取序列信息ToolStripMenuItem.Click
        TabControl1.SelectedIndex = 2
        form_config_info.Show()
    End Sub

    Private Sub 清理序列ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清理序列ToolStripMenuItem.Click
        TabControl1.SelectedIndex = 2
        form_config_clean.Show()

    End Sub


    Private Sub 序列比对高速ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 序列比对高速ToolStripMenuItem.Click

    End Sub


    Private Sub 导出序列ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 导出序列ToolStripMenuItem.Click
        Dim opendialog As New SaveFileDialog
        opendialog.Filter = "FASTA File (*.fasta)|*.fasta;*.FASTA"
        opendialog.FileName = ""
        opendialog.DefaultExt = ".csv"
        opendialog.CheckFileExists = False
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            If opendialog.FileName.ToLower.EndsWith(".fasta") Then
                Dim split_sig As String = InputBox("", "Enter the delimiter", "|")
                Dim dw As New StreamWriter(opendialog.FileName, False)
                Dim state_line As String = ""
                For i As Integer = 1 To dtView.Count
                    state_line = ">" + dtView.Item(i - 1).Item(1).ToString.Replace(split_sig, "_")
                    For j As Integer = 4 To DataGridView1.ColumnCount - 1
                        state_line += split_sig + dtView.Item(i - 1).Item(j - 1).ToString.Replace(split_sig, "_")
                    Next
                    dw.WriteLine(state_line)
                    dw.WriteLine(fasta_seq(i))
                Next
                dw.Close()
            End If
            Append_info("Save Successfully!")
        End If
    End Sub


    Private Sub 日期转换数字ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 日期转换数字ToolStripMenuItem.Click
        Dim splitstr As String = InputBox("", "Enter the delimiter:", "-")

        For i As Integer = 1 To dtView.Count
            If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                Dim tmp_date() As String = DataGridView1.Rows(i - 1).Cells(5).Value.ToString.Split(splitstr)
                If UBound(tmp_date) = 1 Then
                    DataGridView1.Rows(i - 1).Cells(5).Value = (CInt(tmp_date(0)) + CInt(tmp_date(1)) / 12).ToString("F4")
                End If
                If UBound(tmp_date) = 2 Then
                    DataGridView1.Rows(i - 1).Cells(5).Value = (CInt(tmp_date(0)) + CInt(tmp_date(1)) / 12 + CInt(tmp_date(2)) / 30 / 12).ToString("F4")
                End If
            End If
        Next
    End Sub



    Private Sub 增加数据ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 增加数据ToolStripMenuItem.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "fasta File(*.fasta)|*.fas;*.fasta"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".fas"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            current_file = opendialog.FileName
            add_data = True
            TabControl1.SelectedIndex = 1
            data_loaded = False
            form_config_stand.Show()
        End If
    End Sub




    Private Sub AutoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoToolStripMenuItem.Click
        mafft_align("--auto")
    End Sub
    Public Sub mafft_align(ByVal method As String)
        DataGridView1.EndEdit()
        DataGridView1.RefreshEdit()
        If dtView.Count > 1 Then
            Dim selected_count As Integer = 0
            For i As Integer = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If
            Next
            If selected_count > 1 Then
                TabControl1.SelectedIndex = 0
                Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                Dim th1 As New Threading.Thread(AddressOf do_mafft_align)
                th1.Start(method)
            Else
                MsgBox("Please select at least two sequences!")
            End If
        End If
    End Sub
    Public Sub do_mafft_align(ByVal method As String)
        Dim currentTime As DateTime = DateTime.Now
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim in_path As String = root_path + "history\" + currentTimeStamp.ToString + ".fasta"
        Dim sw As New StreamWriter(in_path)
        For i As Integer = 1 To dtView.Count
            If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                sw.WriteLine(">T" + dtView.Item(i - 1).Item(0).ToString)
                sw.WriteLine(fasta_seq(i))
            End If
        Next
        sw.Close()
        PB_value = 10
        Dim out_path As String = root_path + "history\" + currentTimeStamp.ToString + "_tmp.fasta"

        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = currentDirectory + "analysis\mafft-win\mafft.bat" ' 替换为实际的命令行程序路径
        startInfo.WorkingDirectory = currentDirectory + "analysis\mafft-win\" ' 替换为实际的运行文件夹路径
        startInfo.Arguments = method + " --inputorder " + """" + in_path + """" + ">" + """" + out_path + """"
        Dim process As Process = Process.Start(startInfo)
        process.WaitForExit()
        process.Close()

        If File.Exists(out_path) Then
            Dim encoding As Encoding = DetectFileEncoding(out_path)
            If encoding IsNot Nothing AndAlso Not encoding.Equals(Encoding.UTF8) Then
                ' 读取文件内容
                Dim fileContent As String = File.ReadAllText(out_path, encoding)
                ' 将文件内容以UTF-8编码保存
                File.WriteAllText(out_path, fileContent, Encoding.UTF8)
            End If

            Dim sr As New StreamReader(out_path)
            Dim line As String = sr.ReadLine
            Dim tmp_id As String = ""
            Do
                If line <> "" Then
                    If line(0) = ">" Then
                        tmp_id = line.Substring(2)
                        fasta_seq(Int(tmp_id)) = ""
                    Else
                        fasta_seq(Int(tmp_id)) += line.ToUpper
                    End If
                End If
                line = sr.ReadLine
            Loop Until line Is Nothing
            sr.Close()
            out_path = root_path + "history\" + currentTimeStamp.ToString + "_aln.fasta"
            Dim sw1 As New StreamWriter(out_path)
            For i As Integer = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    dtView.Item(i - 1).Item(2) = fasta_seq(i)
                    sw1.WriteLine(">" + dtView.Item(i - 1).Item(1))
                    sw1.WriteLine(fasta_seq(i))
                End If
            Next
            sw1.Close()
        Else
            Exit Sub
            MsgBox("Meet errors in align!")
        End If


        PB_value = 100
        Dim sw_align As New StreamWriter(root_path + "history\" + currentTimeStamp.ToString + ".js")
        Dim sr_align As New StreamReader(out_path)
        sw_align.Write("$(document).ready(function () { let data = " + """")
        sw_align.Write(sr_align.ReadToEnd.Replace(vbCrLf, "\n").Replace(vbCr, "\n").Replace(vbLf, "\n"))
        sr_align.Close()
        sw_align.Write("""" + ";" + vbCrLf)
        sw_align.Write("loadNewMSA(data);" + vbCrLf)
        sw_align.Write("});")
        sw_align.Close()
        If File.Exists(root_path + "history\" + currentTimeStamp.ToString + ".js") Then
            Dim sw0 As New StreamWriter(currentDirectory + "history/" + currentTimeStamp.ToString + ".html")
            Dim sr1 As New StreamReader(currentDirectory + "main/" + language + "/alignmentviewer.txt")
            sw0.Write(sr1.ReadToEnd.Replace("$data$", currentTimeStamp.ToString + ".js"))
            sr1.Close()
            sw0.Close()
            Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "history/" + currentTimeStamp.ToString + ".html"})
        End If
        PB_value = 0
        Dim sw4 As New StreamWriter(currentDirectory + "history/history.csv", True)
        sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + ",Sequence Alignment")
        sw4.Close()
        PB_value = 0
    End Sub

    Private Sub FFTNS1VeryFastButVeryRoughToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FFTNS1VeryFastButVeryRoughToolStripMenuItem.Click
        mafft_align("--retree 1")

    End Sub

    Private Sub FFTNS2FastButRoughToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FFTNS2FastButRoughToolStripMenuItem.Click
        mafft_align("--retree 2")

    End Sub

    Private Sub GINSiVerySlowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GINSiVerySlowToolStripMenuItem.Click
        mafft_align("--globalpair --maxiterate 16")
    End Sub

    Private Sub LINSiMostAccurateVerySlowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LINSiMostAccurateVerySlowToolStripMenuItem.CheckedChanged

    End Sub

    Private Sub EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem.Click
        mafft_align("--genafpair  --maxiterate 16")

    End Sub
    Private Sub PPPAlgorithmToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PPPAlgorithmToolStripMenuItem.Click
        muscle_align("-align")
    End Sub
    Public Sub muscle_align(ByVal method As String)
        DataGridView1.EndEdit()
        DataGridView1.RefreshEdit()
        If dtView.Count > 1 Then
            Dim selected_count As Integer = 0
            For i As Integer = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If
            Next
            If selected_count > 1 Then
                TabControl1.SelectedIndex = 0
                Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                Dim th1 As New Threading.Thread(AddressOf do_muscle_align)
                th1.Start(method)
            Else
                MsgBox("Please select at least two sequences!")
            End If
        End If
    End Sub
    Public Sub do_muscle_align(ByVal method As String)
        Dim currentTime As DateTime = DateTime.Now
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim in_path As String = root_path + "history\" + currentTimeStamp.ToString + ".fasta"
        Dim sw As New StreamWriter(in_path)
        For i As Integer = 1 To dtView.Count
            If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                sw.WriteLine(">T" + dtView.Item(i - 1).Item(0).ToString)
                sw.WriteLine(fasta_seq(i))
            End If
        Next
        sw.Close()
        PB_value = 10
        Dim out_path As String = root_path + "history\" + currentTimeStamp.ToString + "_tmp.fasta"

        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = currentDirectory + "analysis\muscle5.1.win64.exe" ' 替换为实际的命令行程序路径
        startInfo.WorkingDirectory = currentDirectory + "analysis\" ' 替换为实际的运行文件夹路径
        'startInfo.CreateNoWindow = True
        startInfo.Arguments = method + " " + """" + in_path + """" + " -output " + """" + out_path + """"
        Dim process As Process = Process.Start(startInfo)
        process.WaitForExit()
        process.Close()

        If File.Exists(out_path) Then
            Dim encoding As Encoding = DetectFileEncoding(out_path)
            If encoding IsNot Nothing AndAlso Not encoding.Equals(Encoding.UTF8) Then
                ' 读取文件内容
                Dim fileContent As String = File.ReadAllText(out_path, encoding)
                ' 将文件内容以UTF-8编码保存
                File.WriteAllText(out_path, fileContent, Encoding.UTF8)
            End If

            Dim sr As New StreamReader(out_path)
            Dim line As String = sr.ReadLine
            Dim tmp_id As String = ""
            Do
                If line <> "" Then
                    If line(0) = ">" Then
                        tmp_id = line.Substring(2)
                        fasta_seq(Int(tmp_id)) = ""
                    Else
                        fasta_seq(Int(tmp_id)) += line.ToUpper
                    End If
                End If
                line = sr.ReadLine
            Loop Until line Is Nothing
            sr.Close()
            out_path = root_path + "history\" + currentTimeStamp.ToString + "_aln.fasta"
            Dim sw1 As New StreamWriter(out_path)
            For i As Integer = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    dtView.Item(i - 1).Item(2) = fasta_seq(i)
                    sw1.WriteLine(">" + dtView.Item(i - 1).Item(1))
                    sw1.WriteLine(fasta_seq(i))
                End If
            Next
            sw1.Close()
        Else
            Exit Sub
            MsgBox("Met errors in align!")
        End If


        PB_value = 100
        Dim sw_align As New StreamWriter(root_path + "history\" + currentTimeStamp.ToString + ".js")
        Dim sr_align As New StreamReader(out_path)
        sw_align.Write("$(document).ready(function () { let data = " + """")
        sw_align.Write(sr_align.ReadToEnd.Replace(vbCrLf, "\n").Replace(vbCr, "\n").Replace(vbLf, "\n"))
        sr_align.Close()
        sw_align.Write("""" + ";" + vbCrLf)
        sw_align.Write("loadNewMSA(data);" + vbCrLf)
        sw_align.Write("});")
        sw_align.Close()
        If File.Exists(root_path + "history\" + currentTimeStamp.ToString + ".js") Then
            Dim sw0 As New StreamWriter(currentDirectory + "history/" + currentTimeStamp.ToString + ".html")
            Dim sr1 As New StreamReader(currentDirectory + "main/" + language + "/alignmentviewer.txt")
            sw0.Write(sr1.ReadToEnd.Replace("$data$", currentTimeStamp.ToString + ".js"))
            sr1.Close()
            sw0.Close()
            Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "history/" + currentTimeStamp.ToString + ".html"})
        End If
        PB_value = 0
        Dim sw4 As New StreamWriter(currentDirectory + "history/history.csv", True)
        sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + ",Sequence Alignment")
        sw4.Close()
        PB_value = 0
    End Sub

    Private Sub Super5AlgorithmToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Super5AlgorithmToolStripMenuItem.Click
        muscle_align("-super5")
    End Sub


    Private Sub EnglishToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnglishToolStripMenuItem.Click
        If language = "EN" Then
            to_ch()
        Else
            to_en()
        End If
    End Sub



    ' Common helper function to get required file paths and check if they exist
    Private Function GetHaplotypeFilePaths() As (TimeStamp As String, Seq2HapFile As String, GraphJsonFile As String, AllFilesExist As Boolean)
        ' Get current timestamp from WebView URI
        Dim currentUri As Uri = WebView_main.Source
        Dim timeStamp As String = System.IO.Path.GetFileNameWithoutExtension(currentUri.ToString)

        ' Define file paths
        Dim seq2hap_file As String = currentDirectory + "history\" + timeStamp + "_seq2hap.csv"
        Dim graph_json As String = currentDirectory + "history\" + timeStamp + ".json"

        ' Check if all required files exist
        Dim seq2hapExists As Boolean = File.Exists(seq2hap_file)
        Dim graphJsonExists As Boolean = File.Exists(graph_json)

        Dim allFilesExist As Boolean = seq2hapExists AndAlso graphJsonExists

        Return (timeStamp, seq2hap_file, graph_json, allFilesExist)
    End Function

    ' Helper function to log analysis history
    Private Sub LogAnalysisHistory(timeStamp As String, analysisType As String)
        Dim currentTime As DateTime = DateTime.Now
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")

        Dim sw As New StreamWriter(currentDirectory + "history/history.csv", True)
        sw.WriteLine(formattedTime + "," + timeStamp + "," + analysisType)
        sw.Close()
    End Sub

    ' Helper function to run executable with parameters
    Private Function RunExecutable(exePath As String, arguments As String, ByRef outputFiles As List(Of String)) As Boolean
        Try
            ' Prepare process startup info
            Dim startInfo As New ProcessStartInfo()
            startInfo.FileName = exePath
            startInfo.WorkingDirectory = currentDirectory + "history\"
            startInfo.Arguments = arguments
            startInfo.CreateNoWindow = True
            startInfo.UseShellExecute = False
            startInfo.RedirectStandardOutput = True
            startInfo.RedirectStandardError = True

            ' Start and wait for process
            Dim process As Process = Process.Start(startInfo)

            ' Read output for diagnostic purposes
            Dim output As String = process.StandardOutput.ReadToEnd()
            Dim stderror As String = process.StandardError.ReadToEnd()

            process.WaitForExit()

            ' Check for errors
            If Not String.IsNullOrEmpty(stderror) Then
                MsgBox("Error executing analysis: " & stderror)
                Return False
            End If

            ' Check exit code
            If process.ExitCode <> 0 Then
                MsgBox("Analysis failed with exit code: " & process.ExitCode.ToString())
                Return False
            End If

            ' Check if expected output files exist
            Dim allFilesExist As Boolean = True
            For Each filePath In outputFiles
                If Not File.Exists(filePath) Then
                    allFilesExist = False
                    MsgBox("Expected output file not found: " & filePath)
                End If
            Next

            Return allFilesExist
        Catch ex As Exception
            MsgBox("Exception during analysis: " & ex.Message)
            Return False
        End Try
    End Function


    ' Topology Analysis implementation
    Private Sub 拓扑结构分析ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 拓扑结构分析ToolStripMenuItem.Click
        ' Get file paths and validate
        Dim filePaths = GetHaplotypeFilePaths()

        If Not filePaths.AllFilesExist Then
            MsgBox("Not all required files for topology analysis exist. Please load your sequence data and build haplotype network first.")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\hapNet_visual.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Topology analysis executable not found at: " & exePath)
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "--seq2hap """ & filePaths.Seq2HapFile & """ --graph """ & filePaths.GraphJsonFile &
                           """ --img_suffix " & filePaths.TimeStamp

        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        Dim outputSvgFile As String = currentDirectory + "history\" + filePaths.TimeStamp + "_VisualHapNet.svg"
        expectedOutputFiles.Add(outputSvgFile)

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        If success Then
            ' Log to history file
            LogAnalysisHistory(filePaths.TimeStamp, "Topology Analysis")

            ' Show success message and open the visualization
            MsgBox("Topology analysis completed successfully. The visualization has been saved.")
        End If
    End Sub

    ' Modularity Analysis implementation
    Private Sub 模块度分析ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 模块度分析ToolStripMenuItem.Click
        ' Get file paths and validate
        Dim filePaths = GetHaplotypeFilePaths()

        If Not filePaths.AllFilesExist Then
            MsgBox("Not all required files for modularity analysis exist. Please load your sequence data and build haplotype network first.")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\hapNet_community_modularity.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Modularity analysis executable not found at: " & exePath)
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "--seq2hap """ & filePaths.Seq2HapFile & """ --graph """ & filePaths.GraphJsonFile &
                            """ --img_suffix " & filePaths.TimeStamp

        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        Dim modularityPlot As String = currentDirectory + "history\" + filePaths.TimeStamp + "_modularity_vs_communities.svg"
        Dim bestCommunityPlot As String = currentDirectory + "history\" + filePaths.TimeStamp + "_highest_modularity_community.svg"
        expectedOutputFiles.Add(modularityPlot)
        expectedOutputFiles.Add(bestCommunityPlot)

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        If success Then
            ' Log to history file
            LogAnalysisHistory(filePaths.TimeStamp, "Modularity Analysis")

            ' Show success message and open the visualizations
            MsgBox("Modularity analysis completed successfully. Two visualizations have been saved:" & vbCrLf &
              "1. Modularity score vs. number of communities" & vbCrLf &
              "2. Best community structure")

        End If
    End Sub

    ' Community Visualization implementation
    Private Sub 社区绘制ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 社区绘制ToolStripMenuItem.Click
        ' Get file paths and validate
        Dim filePaths = GetHaplotypeFilePaths()

        If Not filePaths.AllFilesExist Then
            MsgBox("Not all required files for community visualization exist. Please load your sequence data and build haplotype network first.")
            Return
        End If

        ' Ask user for k values (up to 4)
        Dim kValuesInput As String = InputBox("Enter up to 4 k values separated by spaces (e.g., '3 5 7 9'):" & vbCrLf &
                                     "Each k value represents a specific number of communities to visualize.",
                                     "K-specific Community Analysis")

        If String.IsNullOrEmpty(kValuesInput) Then Return

        ' Parse and validate k values
        Dim kValues As New List(Of Integer)
        For Each k As String In kValuesInput.Split(" "c)
            Dim kValue As Integer
            If Integer.TryParse(k.Trim(), kValue) AndAlso kValue > 1 Then
                kValues.Add(kValue)
            End If
        Next

        ' Check if we have valid k values
        If kValues.Count = 0 Then
            MsgBox("No valid k values provided. Please enter at least one positive integer greater than 1.")
            Return
        End If

        ' Limit to 4 k values
        If kValues.Count > 4 Then
            kValues = kValues.Take(4).ToList()
            MsgBox("Only the first 4 k values will be used: " & String.Join(", ", kValues))
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\hapNet_plot_k_community.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Community analysis executable not found at: " & exePath)
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "--seq2hap """ & filePaths.Seq2HapFile & """ --graph """ & filePaths.GraphJsonFile &
                           """ --img_suffix " & filePaths.TimeStamp &
                           " --k " & String.Join(" ", kValues)

        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        For Each k As Integer In kValues
            Dim communityFile As String = currentDirectory + "history\" + filePaths.TimeStamp + "_community_k" + k.ToString() + ".svg"
            expectedOutputFiles.Add(communityFile)
        Next

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        If success Then
            ' Log to history file
            LogAnalysisHistory(filePaths.TimeStamp, "Community Structure Analysis (k=" & String.Join(" ", kValues) & ")")

            ' Show success message
            MsgBox("Community visualization completed successfully. " & kValues.Count & " community structure(s) visualized.")
        End If
    End Sub

    Private Sub 序列分析ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 序列分析ToolStripMenuItem.Click
        Dim currentUri As Uri = WebView_main.Source
        Dim timeStamp As String = System.IO.Path.GetFileNameWithoutExtension(currentUri.ToString)

        ' Define file paths
        Dim hap_fasta As String = currentDirectory + "history\" + timeStamp + "_hap.fasta"

        ' Check if all required files exist
        Dim hapFastaExists As Boolean = File.Exists(hap_fasta)

        If Not hapFastaExists Then
            MsgBox("Not all required files for haplotype sequence analysis exist. Please load your sequence data and build haplotype network first.")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\seq_analysis.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Haplotype sequence analysis executable not found at: " & exePath)
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "-i """ & hap_fasta & """ -o " & timeStamp


        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        Dim heatmapPlot As String = currentDirectory + "history\" + timeStamp + "_p-distance_heatmap.svg"
        Dim pcaPlot As String = currentDirectory + "history\" + timeStamp + "_p-distance_pca.svg"
        Dim seqConservationPlot As String = currentDirectory + "history\" + timeStamp + "_sequence_conservation.svg"
        expectedOutputFiles.Add(heatmapPlot)
        expectedOutputFiles.Add(pcaPlot)
        expectedOutputFiles.Add(seqConservationPlot)

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        If success Then
            ' Log to history file
            LogAnalysisHistory(timeStamp, "Hap Sequence Analysis")

            ' Show success message and open the visualization
            MsgBox("Haplotype Sequence analysis completed successfully. The visualization has been saved.")
        End If
    End Sub

    Private Sub 群体信息统计ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 群体信息统计ToolStripMenuItem.Click
        Dim currentUri As Uri = WebView_main.Source
        Dim timeStamp As String = System.IO.Path.GetFileNameWithoutExtension(currentUri.ToString)

        ' Define file paths
        Dim hap_fasta As String = currentDirectory + "history\" + timeStamp + "_hap.fasta"

        ' Check if all required files exist
        Dim hapFastaExists As Boolean = File.Exists(hap_fasta)

        If Not hapFastaExists Then
            MsgBox("Not all required files for population analysis exist. Please load your sequence data and build haplotype network first.")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\population_analysis.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Population analysis executable not found at: " & exePath)
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "-i """ & hap_fasta & """ -o " & timeStamp & """_population_stats.txt"


        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        Dim populationStat As String = currentDirectory + "history\" + timeStamp + "_population_stats.txt"
        expectedOutputFiles.Add(populationStat)

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        If success Then
            ' Log to history file
            LogAnalysisHistory(timeStamp, "Population Analysis")

            ' Show success message and open the visualization
            MsgBox("Population analysis completed successfully. The statistics results have been saved.")
        End If

    End Sub
End Class
