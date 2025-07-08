Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class Mainform
    Public Taxon_Dataset As New DataSet

    Delegate Sub SetUrl(url As String)

    Dim set_web_main_url As SetUrl
    Private set_web_analysis_url As SetUrl

    Delegate Sub AppendText(text As String)

    Dim Append_Text As AppendText

    Public Sub Append_info(info As String)
        Invoke(Append_Text, New Object() {Format(Now(), "yyyy/MM/dd H:mm:ss") + vbTab + info + Chr(13)})
    End Sub

    Public Sub initialize_data()
        Dim Column_Select As New DataGridViewCheckBoxColumn
        Column_Select.HeaderText = "Select"
        DataGridView1.Columns.Insert(0, Column_Select)
        DataGridView1.AllowUserToAddRows = False

        Dim taxon_table As New DataTable
        taxon_table.TableName = "Taxon Table"
        Dim Column_ID As New DataColumn("ID", Type.GetType("System.Int32"))
        Dim Column_Taxon As New DataColumn("Name")
        Dim Column_Seq As New DataColumn("Sequence")
        Dim Column_Time As New DataColumn("Continuous Traits")
        Dim Column_State As New DataColumn("Discrete Traits")
        Dim Column_Count As New DataColumn("Quantity")
        Dim Column_Organism As New DataColumn("Organism")
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

    Private Sub RT_Append_Text(text As String)
        RichTextBox1.AppendText(text)
        RichTextBox1.Refresh()
    End Sub

    Private Sub web_main_seturl(url As String)
        WebView_main.Source = New Uri(url)
    End Sub

    Private Sub web_analysis_seturl(url As String)
        WebView21.Source = New Uri(url)
    End Sub

    Private Sub Mainform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Application.Exit()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WebView_main.CreationProperties = New CoreWebView2CreationProperties With {
            .BrowserExecutableFolder = Path.Combine(root_path, "webview")
            }
        WebView21.CreationProperties = New CoreWebView2CreationProperties With {
            .BrowserExecutableFolder = Path.Combine(root_path, "webview")
            }
        CheckForIllegalCrossThreadCalls = False
        currentDirectory = Application.StartupPath
        set_web_main_url = New SetUrl(AddressOf web_main_seturl)
        set_web_analysis_url = New SetUrl(AddressOf web_analysis_seturl)

        Append_Text = New AppendText(AddressOf RT_Append_Text)
        WebView_main.Source = New Uri("file:///" + currentDirectory + "/main/" + language + "/main.html")
        WebView21.Source = New Uri("file:///" + currentDirectory + "/main/" + "report_init.html")
        initialize_data()
    End Sub

    Private Sub WebView_main_CoreWebView2InitializationCompleted(sender As Object,
                                                                 e As CoreWebView2InitializationCompletedEventArgs) _
        Handles WebView_main.CoreWebView2InitializationCompleted
        WebView_main.CoreWebView2.Settings.IsStatusBarEnabled = False
        WebView_main.CoreWebView2.Settings.AreDefaultContextMenusEnabled = False
    End Sub

    Private Sub WebView21_CoreWebView2InitializationCompleted(sender As Object,
                                                              e As CoreWebView2InitializationCompletedEventArgs)
        ' 启用WebMessage接收
        WebView21.CoreWebView2.Settings.IsWebMessageEnabled = True

        ' 添加Web消息接收事件处理程序
        AddHandler WebView21.CoreWebView2.WebMessageReceived, AddressOf WebView21_WebMessageReceived

        ' 添加导航事件处理程序
        AddHandler WebView21.CoreWebView2.NavigationStarting, AddressOf WebView21_NavigationStarting
    End Sub

    ' 处理导航开始事件
    Private Sub WebView21_NavigationStarting(sender As Object, e As CoreWebView2NavigationStartingEventArgs)
        ' 检查URL是否是文件链接
        Dim url As String = e.Uri

        ' 如果是文件URL，检查是否应该取消导航并在资源管理器中显示文件
        If url.StartsWith("file:///") AndAlso Not url.EndsWith(".html") Then
            ' 获取文件路径（去掉file:///前缀并处理URL编码）
            Dim filePath As String = Uri.UnescapeDataString(url.Substring(8)).Replace("/", "\")

            ' 检查文件是否存在
            If File.Exists(filePath) Then
                ' 在文件资源管理器中选择文件
                ShowFileInExplorer(filePath)
                e.Cancel = True ' 取消在WebView中的导航
            End If
        End If
    End Sub

    Private Sub WebView21_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs)
        Try
            ' 解析JSON消息
            Dim msgObj As JObject = JsonConvert.DeserializeObject(e.WebMessageAsJson)

            ' 检查消息类型
            If msgObj("type").ToString() = "fileClick" Then
                Dim filePath As String = msgObj("path").ToString()

                ' 检查文件是否存在
                If File.Exists(filePath) Then
                    ' 在资源管理器中显示文件
                    ShowFileInExplorer(filePath)
                Else
                    MsgBox("The file doesn't exist: " & filePath)
                End If
            End If
        Catch ex As Exception
            Console.WriteLine("Error processing web message: " + ex.Message)
        End Try
    End Sub

    ' 在资源管理器中显示文件
    Private Sub ShowFileInExplorer(filePath As String)
        Try
            Process.Start("explorer.exe", "/select,""" & filePath & """")
        Catch ex As Exception
            Try
                ' 备选方法：打开包含文件的文件夹
                Process.Start("explorer.exe", """" & Path.GetDirectoryName(filePath) & """")
            Catch ex2 As Exception
                MsgBox("无法在资源管理器中显示文件: " & ex2.Message)
            End Try
        End Try
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
                    Me.Invoke(set_web_main_url,
                              New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
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
                Me.Invoke(set_web_main_url,
                          New Object() {"file:///" + currentDirectory + "history/" + current_file + ".html"})
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
            Dim th1 As New Thread(AddressOf load_csv_data)
            th1.Start(opendialog.FileName)
        End If
    End Sub

    Public Sub load_csv_data(file_path As String)
        Dim dr As StreamReader = Nothing
        Try
            dr = New StreamReader(file_path)
            Dim line As String = dr.ReadLine
            Dim count = 0
            Dim pre_fasta_seq = 0
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
            show_info("CSV Data Loading Completed Successfully!")
            show_info($"• File: {System.IO.Path.GetFileName(file_path)}")
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
                Dim state_line = "ID,Name"
                For j = 3 To DataGridView1.ColumnCount - 1
                    state_line += "," + DataGridView1.Columns(j).HeaderText
                Next
                dw.WriteLine(state_line)
                For i = 1 To dtView.Count
                    state_line = i.ToString
                    For j = 2 To DataGridView1.ColumnCount - 1
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
            show_info("Data Export Completed Successfully!")
            show_info($"• Output file: {System.IO.Path.GetFileName(opendialog.FileName)}")
        End If
    End Sub


    Private Sub 全选ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 全选ToolStripMenuItem.Click
        For i = 1 To dtView.Count
            DataGridView1.Rows(i - 1).Cells(0).Value = True
        Next
        DataGridView1.RefreshEdit()
    End Sub

    Private Sub 清除ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清除ToolStripMenuItem.Click
        For i = 1 To dtView.Count
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
            Dim selected_count = 0
            Dim checked = True
            For i = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If

                If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Continuous Traits:" +
                        DataGridView1.Rows(i - 1).Cells(6).Value + " is not a numerical value.")
                    Exit For
                End If
                If IsNumeric(DataGridView1.Rows(i - 1).Cells(6).Value) = False Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" +
                        DataGridView1.Rows(i - 1).Cells(5).Value + " is not a numerical value.")
                    Exit For
                ElseIf CInt(DataGridView1.Rows(i - 1).Cells(6).Value) <= 0 Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" +
                        DataGridView1.Rows(i - 1).Cells(5).Value + " cannot be less than zero.")
                    Exit For
                End If
            Next
            If checked Then
                If selected_count >= 1 Then
                    TabControl1.SelectedIndex = 0
                    Invoke(set_web_main_url,
                           New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                    Dim th1 As New Thread(AddressOf analysis_network)
                    th1.Start("modified_tcs")
                Else
                    MsgBox("Please select at least one sequence!")
                End If
            End If
        Else
            MsgBox("Please select at least one sequence!")
        End If
    End Sub

    Public Sub analysis_network(network_type As String)
        Dim currentTime As DateTime = DateTime.Now
        Dim success = True
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")

        ' 添加一个文件 存储过程中的分析文件
        Dim currentRetFilePath As String = root_path + "history\" + currentTimeStamp.ToString + "_rst.csv"
        Dim currentRetSw As New StreamWriter(currentRetFilePath)

        ' 写入更详细的CSV标题，包含文件状态和描述
        currentRetSw.WriteLine("FileType,FilePath,Status,Description")

        ' 添加网络类型和分析时间信息到CSV文件
        currentRetSw.WriteLine("# Network Type: " + network_type)
        currentRetSw.WriteLine("# Analysis Time: " + formattedTime)
        currentRetSw.WriteLine("# Timestamp: " + currentTimeStamp.ToString)

        Dim inPath As String = root_path + "history\" + currentTimeStamp.ToString + ".fasta"
        Dim sw As New StreamWriter(inPath)
        Dim aligned = True
        Dim epsilonStr = ""

        For i = 1 To dtView.Count
            If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                sw.WriteLine(
                    ">" + dtView.Item(i - 1).Item(1) + "=" + dtView.Item(i - 1).Item(5) + "=" +
                    dtView.Item(i - 1).Item(4) + "$SPLIT$" + dtView.Item(i - 1).Item(3))
                sw.WriteLine(fasta_seq(i))
                If Len(fasta_seq(i)) <> Len(fasta_seq(1)) Then
                    aligned = False
                End If
            End If
        Next
        sw.Close()

        ' 记录原始序列文件
        currentRetSw.WriteLine("Ori_seqs," + inPath + ",Success,Original sequences")

        PB_value = 10
        Dim outPath As String = root_path + "history\" + currentTimeStamp.ToString + "_aln.fasta"
        If aligned Then
            File.Copy(inPath, outPath)
            currentRetSw.WriteLine("Aligned_seqs," + outPath + ",Success,Sequences already aligned")
        Else
            Dim startInfo As New ProcessStartInfo()
            startInfo.FileName = currentDirectory + "analysis\mafft-win\mafft.bat" ' 替换为实际的命令行程序路径
            startInfo.WorkingDirectory = currentDirectory + "analysis\mafft-win\" ' 替换为实际的运行文件夹路径
            startInfo.Arguments = "--retree 2 --inputorder " + """" + inPath + """" + ">" + """" + outPath + """"
            Dim process As Process = Process.Start(startInfo)
            process.WaitForExit()
            process.Close()

            If File.Exists(outPath) Then
                currentRetSw.WriteLine("Aligned_seqs," + outPath + ",Success,Sequences aligned by MAFFT")
            Else
                currentRetSw.WriteLine("Aligned_seqs," + outPath + ",Failed,MAFFT alignment failed")
            End If
        End If

        PB_value = 30
        If File.Exists(outPath) Then
            Dim encoding As Encoding = DetectFileEncoding(outPath)
            If encoding IsNot Nothing AndAlso Not encoding.Equals(Encoding.UTF8) Then
                ' 读取文件内容
                Dim fileContent As String = File.ReadAllText(outPath, encoding)
                ' 将文件内容以UTF-8编码保存
                File.WriteAllText(outPath, fileContent, Encoding.UTF8)
                currentRetSw.WriteLine("Encoding_conversion," + outPath + ",Success,Converted to UTF-8")
            End If

            Dim networkApp = "fastHaN_win_intel.exe"
            If cpu_info.ToUpper.StartsWith("ARM") Then
                networkApp = "fastHaN_win_arm.exe"
            End If

            Dim fileSuffix As String = root_path + "history\" + currentTimeStamp.ToString
            If hap_fasta(outPath, fileSuffix, new_line(False)) Then
                ' 存储hap_fasta函数处理得到的结果文件
                currentRetSw.WriteLine(
                    "Hap_seq_phylip," + fileSuffix + "_hap.phy" + ",Success,Haplotype sequences in PHYLIP format")
                currentRetSw.WriteLine(
                    "Hap_seq_fasta," + fileSuffix + "_hap.fasta" + ",Success,Haplotype sequences in FASTA format")
                currentRetSw.WriteLine(
                    "Seq_with_traits_phylip," + fileSuffix + "_seq.phy" +
                    ",Success,Aligned sequences with traits in PHYLIP format")
                currentRetSw.WriteLine(
                    "Seq_with_traits_fasta," + fileSuffix + "_seq.fasta" +
                    ",Success,Aligned sequences with traits in FASTA format")
                currentRetSw.WriteLine("Seq_metadata," + fileSuffix + ".meta" + ",Success,Sequence metadata")
                currentRetSw.WriteLine(
                    "Hap_trait," + fileSuffix + "_hap_trait.csv" + ",Success,Haplotype trait information")
                currentRetSw.WriteLine(
                    "Seq_trait," + fileSuffix + "_seq_trait.csv" + ",Success,Sequence trait information")
                currentRetSw.WriteLine(
                    "Seq2Hap," + fileSuffix + "_seq2hap.csv" + ",Success,Sequence to haplotype mapping")

                Dim startInfo_hap As New ProcessStartInfo()
                startInfo_hap.FileName = currentDirectory + "analysis\" + networkApp ' 替换为实际的命令行程序路径
                startInfo_hap.WorkingDirectory = currentDirectory + "history\" ' 替换为实际的运行文件夹路径
                'startInfo_hap.CreateNoWindow = True
                startInfo_hap.Arguments = network_type + " -i " + """" + root_path + "history\" +
                                          currentTimeStamp.ToString + "_seq.phy" + """" + " -o " + """" + root_path +
                                          "history\" + currentTimeStamp.ToString + """" + epsilonStr
                Dim process_hap As Process = Process.Start(startInfo_hap)
                process_hap.WaitForExit()
                process_hap.Close()
                PB_value = 50

                Dim gmlFile As String = fileSuffix + ".gml"
                Dim jsonFile As String = fileSuffix + ".json"

                If File.Exists(gmlFile) Then
                    currentRetSw.WriteLine("HapNet_gml," + gmlFile + ",Success,Haplotype network in GML format")

                    If File.Exists(jsonFile) Then
                        currentRetSw.WriteLine("HapNet_json," + jsonFile + ",Success,Haplotype network in JSON format")
                    Else
                        currentRetSw.WriteLine(
                            "HapNet_json," + jsonFile + ",Failed,JSON format network file not created")
                    End If

                    Dim startInfo_network As New ProcessStartInfo()
                    startInfo_network.FileName = currentDirectory + "analysis\GenNetworkConfig2.exe" ' 替换为实际的命令行程序路径
                    startInfo_network.WorkingDirectory = currentDirectory + "history\" ' 替换为实际的运行文件夹路径
                    'startInfo_network.CreateNoWindow = True
                    startInfo_network.Arguments = currentTimeStamp.ToString + ".gml " + currentTimeStamp.ToString +
                                                  "_seq2hap.csv " + currentTimeStamp.ToString
                    Dim process_network As Process = Process.Start(startInfo_network)
                    process_network.WaitForExit()
                    process_network.Close()
                    PB_value = 80

                    Dim jsFile As String = root_path + "history\" + currentTimeStamp.ToString + ".js"
                    If File.Exists(jsFile) Then
                        Dim htmlFile As String = currentDirectory + "history/" + currentTimeStamp.ToString + ".html"
                        Dim sw0 As New StreamWriter(htmlFile)
                        Dim sr1 As New StreamReader(currentDirectory + "main/" + language + "/tcsBU.txt")
                        sw0.Write(sr1.ReadToEnd.Replace("$data$", currentTimeStamp.ToString + ".js"))
                        sr1.Close()
                        sw0.Close()

                        Dim hapconfFile As String = fileSuffix + "_hapconf.csv"
                        Dim groupconfFile As String = fileSuffix + "_groupconf.csv"

                        If File.Exists(hapconfFile) Then
                            currentRetSw.WriteLine("HapNet_hapconf," + hapconfFile + ",Success,Haplotype configuration")
                        Else
                            currentRetSw.WriteLine(
                                "HapNet_hapconf," + hapconfFile + ",Failed,Haplotype configuration file not created")
                        End If

                        If File.Exists(groupconfFile) Then
                            currentRetSw.WriteLine("HapNet_groupconf," + groupconfFile + ",Success,Group configuration")
                        Else
                            currentRetSw.WriteLine(
                                "HapNet_groupconf," + groupconfFile + ",Failed,Group configuration file not created")
                        End If

                        currentRetSw.WriteLine("HapNet_js," + jsFile + ",Success,Visualization JavaScript")
                        currentRetSw.WriteLine("HapNet_html," + htmlFile + ",Success,Visualization HTML")

                        Me.Invoke(set_web_main_url, New Object() {"file:///" + htmlFile})
                    Else
                        currentRetSw.WriteLine("HapNet_js," + jsFile + ",Failed,Visualization JavaScript not created")
                        MsgBox("Haplotype network construction failed. Please check the data.")
                        Me.Invoke(set_web_main_url,
                                  New Object() {"file:///" + currentDirectory + "main/" + language + "/main.html"})
                        success = False
                    End If
                Else
                    currentRetSw.WriteLine("HapNet_gml," + gmlFile + ",Failed,GML network file not created")
                    success = False
                End If
            Else
                currentRetSw.WriteLine(
                    "Sequence_processing,multiple files,Failed,Failed to process sequences with hap_fasta")
                success = False
            End If
        Else
            currentRetSw.WriteLine("Aligned_seqs," + outPath + ",Failed,Sequence alignment failed")
            MsgBox("Sequence alignment failed. Please check the sequences.")
            Me.Invoke(set_web_main_url, New Object() {"file:///" + currentDirectory + "main/" + language + "/main.html"})
            success = False
        End If

        PB_value = 100
        If success Then
            Dim historyFile As String = currentDirectory + "history/history.csv"
            Dim sw4 As New StreamWriter(historyFile, True)
            sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + "," + network_type + " Haplotype network")
            sw4.Close()
        End If

        PB_value = 0
        currentRetSw.Close()

        ' 生成HTML报告
        Dim htmlReportPath As String = root_path + "history\" + currentTimeStamp.ToString + "_report.html"
        GenerateHtmlReport(currentRetFilePath, htmlReportPath, currentTimeStamp.ToString, formattedTime,
                           success)

        Me.Invoke(set_web_analysis_url, New Object() {"file:///" + htmlReportPath})
    End Sub


    Private Sub DataGridView1_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) _
        Handles DataGridView1.DataBindingComplete
        If data_loaded = False And dtView.Count > 0 Then
            For i = 1 To dtView.Count
                DataGridView1.Rows(i - 1).Cells(0).Value = True
            Next
        End If
    End Sub

    Private Sub MJN单倍型网络ToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles MJN单倍型网络ToolStripMenuItem.Click
        If dtView.Count >= 1 Then
            Dim selected_count = 0
            Dim checked = True
            For i = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If

                If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Date:" +
                        DataGridView1.Rows(i - 1).Cells(6).Value + " is not a numerical value.")
                    Exit For
                End If
                If IsNumeric(DataGridView1.Rows(i - 1).Cells(6).Value) = False Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" +
                        DataGridView1.Rows(i - 1).Cells(5).Value + " is not a numerical value.")
                    Exit For
                ElseIf CInt(DataGridView1.Rows(i - 1).Cells(6).Value) <= 0 Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" +
                        DataGridView1.Rows(i - 1).Cells(5).Value + " cannot be less than zero.")
                    Exit For
                End If
            Next
            If checked Then
                If selected_count >= 1 Then
                    TabControl1.SelectedIndex = 0
                    Me.Invoke(set_web_main_url,
                              New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                    Dim th1 As New Thread(AddressOf analysis_network)
                    th1.Start("mjn")
                Else
                    MsgBox("Please select at least one sequence!")
                End If
            End If
        Else
            MsgBox("Please select at least one sequence!")
        End If
    End Sub

    Private Sub MSN单倍型网络ToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles MSN单倍型网络ToolStripMenuItem.Click
        If dtView.Count >= 1 Then
            Dim selected_count = 0
            Dim checked = True
            For i = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If

                If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Date:" +
                        DataGridView1.Rows(i - 1).Cells(6).Value + " is not a numerical value.")
                    Exit For
                End If
                If IsNumeric(DataGridView1.Rows(i - 1).Cells(6).Value) = False Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" +
                        DataGridView1.Rows(i - 1).Cells(5).Value + " is not a numerical value.")
                    Exit For
                ElseIf CInt(DataGridView1.Rows(i - 1).Cells(6).Value) <= 0 Then
                    checked = False
                    MsgBox(
                        "ID:" + DataGridView1.Rows(i - 1).Cells(1).Value.ToString + ", Quantity:" +
                        DataGridView1.Rows(i - 1).Cells(5).Value + " cannot be less than zero.")
                    Exit For
                End If
            Next
            If checked Then
                If selected_count >= 1 Then
                    TabControl1.SelectedIndex = 0
                    Me.Invoke(set_web_main_url,
                              New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})

                    Dim th1 As New Thread(AddressOf analysis_network)
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
                    sw.WriteLine(
                        "<tr><td>" + line_list(0) + "</td><td>" + line_list(1) + "</td><td>" + line_list(2) + "</td> ")
                    If line_list(2).Contains("Haplotype") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + ".html'>View Results</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + "_seq.phy' target='_new'>Original Sequence</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_seq_trait.csv'  target='_new'>Trait Matrix</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + "_hap.phy' target='_new'>Haplotype Sequence</a> ")
                        sw.WriteLine(
                            "<a href='./" + line_list(1) + "_hap_trait.csv'  target='_new'>Haplotype Trait Matrix</a>")
                        sw.WriteLine(
                            "<a href='./" + line_list(1) + "_seq2hap.csv'  target='_new'>Haplotype Source</a></td></tr>")
                        ' 处理新的分析类型
                    ElseIf line_list(2).Contains("Topology Analysis") Then
                        sw.WriteLine(
                            "<td><a href='./" + line_list(1) + "_VisualHapNet.svg' target='_new'>Network Topology</a>")
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
                        For i = 0 To kValues.Count - 1
                            Dim k As String = kValues(i)

                            ' 添加社区可视化链接
                            sw.WriteLine(
                                "<a href='./" + line_list(1) + "_community_k" + k + ".svg' target='_new'>Community (k=" +
                                k + ")</a>")

                            ' 检查是否有相应的文本文件，如果有则添加链接
                            If File.Exists(currentDirectory + "history/" + line_list(1) + "_community_k" + k + ".txt") _
                                Then
                                sw.WriteLine(
                                    "<a href='./" + line_list(1) + "_community_k" + k +
                                    ".txt' target='_new'>[Details]</a>")
                            End If

                            ' 如果不是最后一个k值，添加分隔符
                            If i < kValues.Count - 1 Then
                                sw.WriteLine(" ")
                            End If
                        Next

                        sw.WriteLine("</td></tr>")
                    ElseIf line_list(2).Contains("Modularity Analysis") Then
                        sw.WriteLine(
                            "<td><a href='./" + line_list(1) +
                            "_modularity_vs_communities.svg' target='_new'>Modularity Curve</a> ")
                        sw.WriteLine(
                            "<a href='./" + line_list(1) +
                            "_highest_modularity_community.svg' target='_new'>Best Community Structure</a>")
                        sw.WriteLine("</td></tr>")
                    ElseIf line_list(2).Contains("Hap Sequence Analysis") Then
                        sw.WriteLine(
                            "<td><a href='./" + line_list(1) +
                            "_sequence_conservation.svg' target='_new'>conservation site</a> ")
                        sw.WriteLine(
                            "<a href='./" + line_list(1) +
                            "_p-distance_heatmap.svg' target='_new'>p-distance heatmap</a> ")
                        sw.WriteLine(
                            "<a href='./" + line_list(1) + "_p-distance_pca.svg' target='_new'>p-distance pca</a>")
                        sw.WriteLine("</td></tr>")
                    ElseIf line_list(2).Contains("Population Analysis") Then
                        sw.WriteLine(
                            "<td><a href='./" + line_list(1) +
                            "_population_stats.txt' target='_new'>Population Statistics</a></td></tr>")
                    ElseIf line_list(2).Contains("Sequence Alignment") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + ".html'>View Results</a>")
                        sw.WriteLine(
                            "<a href='./" + line_list(1) + "_aln.fasta'  target='_new'>Sequence Alignment</a></td></tr>")
                    ElseIf line_list(2).Contains("Genotyping") Then
                        sw.WriteLine("<td><a href='./" + line_list(1) + ".html'>View Results</a>")
                        sw.WriteLine("<a href='./" + line_list(1) + ".csv'  target='_new'>Genotyping Results</a> ")
                        sw.WriteLine("<a href='./" + line_list(1) + "_null.csv'  target='_new'>Failed Sequences</a> ")
                        If File.Exists(currentDirectory + "history/" + line_list(1) + ".gz") Then
                            sw.WriteLine(
                                "<a href='./" + line_list(1) + ".gz'  target='_new'>Genotyping Database</a></td></tr>")
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

    Private Sub 获取序列信息ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 获取序列信息ToolStripMenuItem.Click
        TabControl1.SelectedIndex = 2
        form_config_info.Show()
    End Sub

    Private Sub 清理序列ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 清理序列ToolStripMenuItem.Click
        TabControl1.SelectedIndex = 2
        form_config_clean.Show()
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
                Dim state_line = ""
                For i = 1 To dtView.Count
                    state_line = ">" + dtView.Item(i - 1).Item(1).ToString.Replace(split_sig, "_")
                    For j = 4 To DataGridView1.ColumnCount - 1
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

        For i = 1 To dtView.Count
            If IsNumeric(DataGridView1.Rows(i - 1).Cells(5).Value) = False Then
                Dim tmp_date() As String = DataGridView1.Rows(i - 1).Cells(5).Value.ToString.Split(splitstr)
                If UBound(tmp_date) = 1 Then
                    DataGridView1.Rows(i - 1).Cells(5).Value = (CInt(tmp_date(0)) + CInt(tmp_date(1)) / 12).ToString("F4")
                End If
                If UBound(tmp_date) = 2 Then
                    DataGridView1.Rows(i - 1).Cells(5).Value =
                        (CInt(tmp_date(0)) + CInt(tmp_date(1)) / 12 + CInt(tmp_date(2)) / 30 / 12).ToString("F4")
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

    Public Sub mafft_align(method As String)
        DataGridView1.EndEdit()
        DataGridView1.RefreshEdit()
        If dtView.Count > 1 Then
            Dim selected_count = 0
            For i = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If
            Next
            If selected_count > 1 Then
                TabControl1.SelectedIndex = 0
                Me.Invoke(set_web_main_url,
                          New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                Dim th1 As New Thread(AddressOf do_mafft_align)
                th1.Start(method)
            Else
                MsgBox("Please select at least two sequences!")
            End If
        End If
    End Sub

    Public Sub do_mafft_align(method As String)
        Dim currentTime As DateTime = DateTime.Now
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim in_path As String = root_path + "history\" + currentTimeStamp.ToString + ".fasta"
        Dim sw As New StreamWriter(in_path)
        For i = 1 To dtView.Count
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
            Dim tmp_id = ""
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
            For i = 1 To dtView.Count
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
            Me.Invoke(set_web_main_url,
                      New Object() {"file:///" + currentDirectory + "history/" + currentTimeStamp.ToString + ".html"})
        End If
        PB_value = 0
        Dim sw4 As New StreamWriter(currentDirectory + "history/history.csv", True)
        sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + ",Sequence Alignment")
        sw4.Close()
        PB_value = 0
    End Sub

    Private Sub FFTNS1VeryFastButVeryRoughToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles FFTNS1VeryFastButVeryRoughToolStripMenuItem.Click
        mafft_align("--retree 1")
    End Sub

    Private Sub FFTNS2FastButRoughToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles FFTNS2FastButRoughToolStripMenuItem.Click
        mafft_align("--retree 2")
    End Sub

    Private Sub GINSiVerySlowToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles GINSiVerySlowToolStripMenuItem.Click
        mafft_align("--globalpair --maxiterate 16")
    End Sub

    Private Sub LINSiMostAccurateVerySlowToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles LINSiMostAccurateVerySlowToolStripMenuItem.CheckedChanged
    End Sub

    Private Sub EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles EINSiForLongUnalignableRegionsVerySlowToolStripMenuItem.Click
        mafft_align("--genafpair  --maxiterate 16")
    End Sub

    Private Sub PPPAlgorithmToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles PPPAlgorithmToolStripMenuItem.Click
        muscle_align("-align")
    End Sub

    Public Sub muscle_align(method As String)
        DataGridView1.EndEdit()
        DataGridView1.RefreshEdit()
        If dtView.Count > 1 Then
            Dim selected_count = 0
            For i = 1 To dtView.Count
                If DataGridView1.Rows(i - 1).Cells(0).FormattedValue.ToString = "True" Then
                    selected_count += 1
                End If
            Next
            If selected_count > 1 Then
                TabControl1.SelectedIndex = 0
                Me.Invoke(set_web_main_url,
                          New Object() {"file:///" + currentDirectory + "main/" + language + "/waiting.html"})
                Dim th1 As New Thread(AddressOf do_muscle_align)
                th1.Start(method)
            Else
                MsgBox("Please select at least two sequences!")
            End If
        End If
    End Sub

    Public Sub do_muscle_align(method As String)
        Dim currentTime As DateTime = DateTime.Now
        Dim currentTimeStamp As Long = (currentTime - New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
        Dim formattedTime As String = currentTime.ToString("yyyy-MM-dd HH:mm")
        Dim in_path As String = root_path + "history\" + currentTimeStamp.ToString + ".fasta"
        Dim sw As New StreamWriter(in_path)
        For i = 1 To dtView.Count
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
            Dim tmp_id = ""
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
            For i = 1 To dtView.Count
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
            Me.Invoke(set_web_main_url,
                      New Object() {"file:///" + currentDirectory + "history/" + currentTimeStamp.ToString + ".html"})
        End If
        PB_value = 0
        Dim sw4 As New StreamWriter(currentDirectory + "history/history.csv", True)
        sw4.WriteLine(formattedTime + "," + currentTimeStamp.ToString + ",Sequence Alignment")
        sw4.Close()
        PB_value = 0
    End Sub

    Private Sub Super5AlgorithmToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles Super5AlgorithmToolStripMenuItem.Click
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
    Private Function GetHaplotypeFilePaths() _
        As (TimeStamp As String, Seq2HapFile As String, GraphJsonFile As String, AllFilesExist As Boolean)
        ' Get current timestamp from WebView URI
        Dim currentUri As Uri = WebView_main.Source
        Dim timeStamp As String = Path.GetFileNameWithoutExtension(currentUri.ToString)

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
    Private Function RunExecutable(exePath As String, arguments As String, ByRef outputFiles As List(Of String)) _
        As Boolean
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
            Dim allFilesExist = True
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


    ' 通用函数：将分析结果添加到CSV文件
    Private Sub AddAnalysisResultsToCSV(csvFilePath As String, analysisType As String,
                                        resultFiles As List(Of (FilePath As String, Description As String)),
                                        success As Boolean)
        Try
            ' 检查CSV文件是否存在
            If Not File.Exists(csvFilePath) Then
                Return
            End If

            ' 打开CSV文件以追加内容
            Using sw As New StreamWriter(csvFilePath, True)
                ' 写入分析信息
                Dim status As String = If(success, "Success", "Failed")

                ' 写入分析类型记录
                sw.WriteLine(
                    analysisType + ",timestamp=" +
                    Path.GetFileNameWithoutExtension(csvFilePath).Replace("_rst", "") + "," + status + "," +
                    analysisType)

                ' 写入结果文件
                If success Then
                    For Each resultFile In resultFiles
                        ' 检查文件是否存在
                        Dim fileStatus As String = If(File.Exists(resultFile.FilePath), "Success", "Failed")
                        sw.WriteLine(
                            analysisType + "," + resultFile.FilePath + "," +
                            fileStatus + "," + resultFile.Description)
                    Next
                End If
            End Using

        Catch ex As Exception
            ' 处理错误但不影响主流程
            Console.WriteLine("Error adding analysis results to CSV: " + ex.Message)
        End Try
    End Sub

    Private Sub 拓扑结构分析ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 拓扑结构分析ToolStripMenuItem.Click
        ' Get file paths and validate
        Dim filePaths = GetHaplotypeFilePaths()

        If Not filePaths.AllFilesExist Then
            MsgBox("Topology Analysis - File Validation Failed" & vbCrLf & vbCrLf &
                   "Required analysis files not found. Please ensure the following steps are completed:" & vbCrLf &
                   "1. Load sequence data" & vbCrLf &
                   "2. Build haplotype network" & vbCrLf & vbCrLf &
                   "Note: Please complete the basic analysis steps before performing topology analysis.",
                   MsgBoxStyle.Exclamation, "Topology Analysis")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\hapNet_visual.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Topology Analysis - Executable Missing" & vbCrLf & vbCrLf &
                   "Analysis executable not found at:" & vbCrLf &
                   exePath & vbCrLf & vbCrLf &
                   "Please check if the program installation is complete or contact technical support.",
                   MsgBoxStyle.Critical, "Topology Analysis")
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "--seq2hap """ & filePaths.Seq2HapFile & """ --graph """ & filePaths.GraphJsonFile &
                                  """ --img_suffix " & filePaths.TimeStamp

        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        Dim outputSvgFile As String = currentDirectory + "history\" + filePaths.TimeStamp + "_VisualHapNet.svg"
        expectedOutputFiles.Add(outputSvgFile)

        ' Show processing message
        MsgBox("Topology Analysis - Processing Started" & vbCrLf & vbCrLf &
               "Topology analysis is in progress, please wait..." & vbCrLf &
               "Results will be displayed automatically upon completion.",
               MsgBoxStyle.Information, "Topology Analysis")

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        ' 准备结果文件列表
        Dim resultFiles As New List(Of (FilePath As String, Description As String))
        resultFiles.Add((outputSvgFile, "Topology visualization of haplotype network"))

        ' 将结果添加到CSV文件
        Dim csvFilePath As String = root_path + "history\" + filePaths.TimeStamp + "_rst.csv"
        AddAnalysisResultsToCSV(csvFilePath, "TopologyAnalysis", resultFiles, success)

        If success Then
            ' Log to history file
            LogAnalysisHistory(filePaths.TimeStamp, "Topology Analysis")

            ' Show success message with detailed instructions
            MsgBox("Topology Analysis - Analysis Completed" & vbCrLf & vbCrLf &
                   "Topology analysis completed successfully!" & vbCrLf & vbCrLf &
                   "Result files saved to:" & vbCrLf &
                   "• Visualization: " & outputSvgFile & vbCrLf & vbCrLf &
                   "View Analysis Results:" & vbCrLf &
                   "• All analysis results have been integrated into the comprehensive report" & vbCrLf &
                   "• The result report page will refresh automatically",
                   MsgBoxStyle.Information, "Topology Analysis")

            ' 生成更新的HTML报告
            Dim htmlReportPath As String = root_path + "history\" + filePaths.TimeStamp + "_report.html"
            If File.Exists(csvFilePath) Then
                GenerateHtmlReport(csvFilePath, htmlReportPath, filePaths.TimeStamp,
                                   DateTime.Now.ToString("yyyy-MM-dd HH:mm"), True)
                ' 重新加载WebView21中的HTML报告
                Me.Invoke(Sub()
                              ' 检查当前显示的是否为报告页面
                              If WebView21.Source.ToString().Contains("_report.html") Then
                                  ' 如果当前已经显示报告页面，则刷新
                                  WebView21.Reload()
                              Else
                                  ' 如果当前不是显示报告页面，则导航到报告页面
                                  Me.Invoke(set_web_analysis_url, New Object() {"file:///" + htmlReportPath})
                              End If
                          End Sub)
            End If
        Else
            ' Show failure message with troubleshooting guidance
            MsgBox("Topology Analysis - Analysis Failed" & vbCrLf & vbCrLf &
                   "An error occurred during topology analysis" & vbCrLf & vbCrLf &
                   "Possible causes:" & vbCrLf &
                   "• Incorrect input file format" & vbCrLf &
                   "• Data quality issues" & vbCrLf &
                   "• Insufficient system resources" & vbCrLf & vbCrLf &
                   "Suggested solutions:" & vbCrLf &
                   "1. Check input data integrity" & vbCrLf &
                   "2. Rebuild haplotype network" & vbCrLf &
                   "3. Check system logs for detailed error information" & vbCrLf &
                   "4. Contact technical support if the problem persists",
                   MsgBoxStyle.Critical, "Topology Analysis")
        End If
    End Sub

    ' 修改模块度分析函数
    Private Sub 模块度分析ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 模块度分析ToolStripMenuItem.Click
        ' Get file paths and validate
        Dim filePaths = GetHaplotypeFilePaths()

        If Not filePaths.AllFilesExist Then
            MsgBox("Modularity Analysis - File Validation Failed" & vbCrLf & vbCrLf &
                   "Required analysis files not found. Please ensure the following steps are completed:" & vbCrLf &
                   "1. Load sequence data" & vbCrLf &
                   "2. Build haplotype network" & vbCrLf & vbCrLf &
                   "Note: Please complete the basic analysis steps before performing modularity analysis.",
                   MsgBoxStyle.Exclamation, "Modularity Analysis")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\hapNet_community_modularity.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Modularity Analysis - Executable Missing" & vbCrLf & vbCrLf &
                   "Analysis executable not found at:" & vbCrLf &
                   exePath & vbCrLf & vbCrLf &
                   "Please check if the program installation is complete or contact technical support.",
                   MsgBoxStyle.Critical, "Modularity Analysis")
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "--seq2hap """ & filePaths.Seq2HapFile & """ --graph """ & filePaths.GraphJsonFile &
                                  """ --img_suffix " & filePaths.TimeStamp

        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        Dim modularityPlot As String = currentDirectory + "history\" + filePaths.TimeStamp +
                                       "_modularity_vs_communities.svg"
        Dim bestCommunityPlot As String = currentDirectory + "history\" + filePaths.TimeStamp +
                                          "_highest_modularity_community.svg"
        expectedOutputFiles.Add(modularityPlot)
        expectedOutputFiles.Add(bestCommunityPlot)

        ' Show processing message
        MsgBox("Modularity Analysis - Processing Started" & vbCrLf & vbCrLf &
               "Modularity analysis is in progress, please wait..." & vbCrLf &
               "This analysis will generate community structure visualizations." & vbCrLf &
               "Results will be displayed automatically upon completion.",
               MsgBoxStyle.Information, "Modularity Analysis")

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        ' 准备结果文件列表
        Dim resultFiles As New List(Of (FilePath As String, Description As String))
        resultFiles.Add((modularityPlot, "Modularity score vs. number of communities"))
        resultFiles.Add((bestCommunityPlot, "Best community structure with highest modularity"))

        ' 将结果添加到CSV文件
        Dim csvFilePath As String = root_path + "history\" + filePaths.TimeStamp + "_rst.csv"
        AddAnalysisResultsToCSV(csvFilePath, "ModularityAnalysis", resultFiles, success)

        If success Then
            ' Log to history file
            LogAnalysisHistory(filePaths.TimeStamp, "Modularity Analysis")

            ' Show success message with detailed instructions
            MsgBox("Modularity Analysis - Analysis Completed" & vbCrLf & vbCrLf &
                   "Modularity analysis completed successfully!" & vbCrLf & vbCrLf &
                   "Two result files have been generated:" & vbCrLf &
                   "• Modularity score vs. number of communities" & vbCrLf &
                   "• Best community structure visualization" & vbCrLf & vbCrLf &
                   "View Analysis Results:" & vbCrLf &
                   "• All analysis results have been integrated into the comprehensive report" & vbCrLf &
                   "• The result report page will refresh automatically",
                   MsgBoxStyle.Information, "Modularity Analysis")

            ' 生成更新的HTML报告
            Dim htmlReportPath As String = root_path + "history\" + filePaths.TimeStamp + "_report.html"
            If File.Exists(csvFilePath) Then
                GenerateHtmlReport(csvFilePath, htmlReportPath, filePaths.TimeStamp,
                                   DateTime.Now.ToString("yyyy-MM-dd HH:mm"), True)
                ' 重新加载WebView21中的HTML报告
                Me.Invoke(Sub()
                              ' 检查当前显示的是否为报告页面
                              If WebView21.Source.ToString().Contains("_report.html") Then
                                  ' 如果当前已经显示报告页面，则刷新
                                  WebView21.Reload()
                              Else
                                  ' 如果当前不是显示报告页面，则导航到报告页面
                                  Me.Invoke(set_web_analysis_url, New Object() {"file:///" + htmlReportPath})
                              End If
                          End Sub)
            End If
        Else
            ' Show failure message with troubleshooting guidance
            MsgBox("Modularity Analysis - Analysis Failed" & vbCrLf & vbCrLf &
                   "An error occurred during modularity analysis" & vbCrLf & vbCrLf &
                   "Possible causes:" & vbCrLf &
                   "• Insufficient data for community detection" & vbCrLf &
                   "• Network structure too simple for modularity analysis" & vbCrLf &
                   "• System resource limitations" & vbCrLf & vbCrLf &
                   "Suggested solutions:" & vbCrLf &
                   "1. Ensure your network has sufficient complexity" & vbCrLf &
                   "2. Check input data quality" & vbCrLf &
                   "3. Verify system logs for detailed error information" & vbCrLf &
                   "4. Contact technical support if the problem persists",
                   MsgBoxStyle.Critical, "Modularity Analysis")
        End If
    End Sub

    ' 修改社区绘制函数
    Private Sub 社区绘制ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 社区绘制ToolStripMenuItem.Click
        ' Get file paths and validate
        Dim filePaths = GetHaplotypeFilePaths()

        If Not filePaths.AllFilesExist Then
            MsgBox("Community Analysis - File Validation Failed" & vbCrLf & vbCrLf &
                   "Required analysis files not found. Please ensure the following steps are completed:" & vbCrLf &
                   "1. Load sequence data" & vbCrLf &
                   "2. Build haplotype network" & vbCrLf & vbCrLf &
                   "Note: Please complete the basic analysis steps before performing community analysis.",
                   MsgBoxStyle.Exclamation, "Community Analysis")
            Return
        End If

        ' Ask user for k values (up to 4)
        Dim kValuesInput As String = InputBox("Community Analysis - K Values Selection" & vbCrLf & vbCrLf &
                                              "Enter up to 4 k values separated by spaces (e.g., '3 5 7 9'):" & vbCrLf &
                                              "Each k value represents a specific number of communities to visualize." &
                                              vbCrLf & vbCrLf &
                                              "Note: K values must be positive integers greater than 1",
                                              "K-specific Community Analysis")

        If String.IsNullOrEmpty(kValuesInput) Then
            MsgBox("Community Analysis - Operation Cancelled" & vbCrLf & vbCrLf &
                   "No k values provided. Analysis cancelled by user.",
                   MsgBoxStyle.Information, "Community Analysis")
            Return
        End If

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
            MsgBox("Community Analysis - Invalid Input" & vbCrLf & vbCrLf &
                   "No valid k values provided." & vbCrLf &
                   "Please enter at least one positive integer greater than 1." & vbCrLf & vbCrLf &
                   "Example: 3 5 7 9",
                   MsgBoxStyle.Exclamation, "Community Analysis")
            Return
        End If

        ' Limit to 4 k values
        If kValues.Count > 4 Then
            kValues = kValues.Take(4).ToList()
            MsgBox("Community Analysis - Input Adjusted" & vbCrLf & vbCrLf &
                   "Only the first 4 k values will be used: " & String.Join(", ", kValues) & vbCrLf & vbCrLf &
                   "Processing will continue with these values.",
                   MsgBoxStyle.Information, "Community Analysis")
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\hapNet_plot_k_community.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Community Analysis - Executable Missing" & vbCrLf & vbCrLf &
                   "Analysis executable not found at:" & vbCrLf &
                   exePath & vbCrLf & vbCrLf &
                   "Please check if the program installation is complete or contact technical support.",
                   MsgBoxStyle.Critical, "Community Analysis")
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "--seq2hap """ & filePaths.Seq2HapFile & """ --graph """ & filePaths.GraphJsonFile &
                                  """ --img_suffix " & filePaths.TimeStamp &
                                  " --k " & String.Join(" ", kValues)

        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        For Each k As Integer In kValues
            Dim communityFile As String = currentDirectory + "history\" + filePaths.TimeStamp + "_community_k" +
                                          k.ToString() + ".svg"
            expectedOutputFiles.Add(communityFile)
        Next

        ' Show processing message
        MsgBox("Community Analysis - Processing Started" & vbCrLf & vbCrLf &
               "Community visualization is in progress for k values: " & String.Join(", ", kValues) & vbCrLf &
               "Generating " & kValues.Count & " community structure visualization(s)..." & vbCrLf &
               "Results will be displayed automatically upon completion.",
               MsgBoxStyle.Information, "Community Analysis")

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        ' 准备结果文件列表
        Dim resultFiles As New List(Of (FilePath As String, Description As String))
        For Each k As Integer In kValues
            Dim communityFile As String = currentDirectory + "history\" + filePaths.TimeStamp + "_community_k" +
                                          k.ToString() + ".svg"
            resultFiles.Add((communityFile, "Community structure visualization (k=" & k.ToString() & ")"))
        Next

        ' 将结果添加到CSV文件
        Dim csvFilePath As String = root_path + "history\" + filePaths.TimeStamp + "_rst.csv"
        AddAnalysisResultsToCSV(csvFilePath, "CommunityAnalysis", resultFiles, success)

        If success Then
            ' Log to history file
            LogAnalysisHistory(filePaths.TimeStamp, "Community Structure Analysis (k=" & String.Join(" ", kValues) & ")")

            ' Show success message with detailed instructions
            MsgBox("Community Analysis - Analysis Completed" & vbCrLf & vbCrLf &
                   "Community visualization completed successfully!" & vbCrLf & vbCrLf &
                   "Generated " & kValues.Count & " community structure visualization(s) for:" & vbCrLf &
                   "• K values: " & String.Join(", ", kValues) & vbCrLf & vbCrLf &
                   "View Analysis Results:" & vbCrLf &
                   "• All analysis results have been integrated into the comprehensive report" & vbCrLf &
                   "• The result report page will refresh automatically",
                   MsgBoxStyle.Information, "Community Analysis")

            ' 生成更新的HTML报告
            Dim htmlReportPath As String = root_path + "history\" + filePaths.TimeStamp + "_report.html"
            If File.Exists(csvFilePath) Then
                GenerateHtmlReport(csvFilePath, htmlReportPath, filePaths.TimeStamp,
                                   DateTime.Now.ToString("yyyy-MM-dd HH:mm"), True)
                ' 重新加载WebView21中的HTML报告
                Me.Invoke(Sub()
                              ' 检查当前显示的是否为报告页面
                              If WebView21.Source.ToString().Contains("_report.html") Then
                                  ' 如果当前已经显示报告页面，则刷新
                                  WebView21.Reload()
                              Else
                                  ' 如果当前不是显示报告页面，则导航到报告页面
                                  Me.Invoke(set_web_analysis_url, New Object() {"file:///" + htmlReportPath})
                              End If
                          End Sub)
            End If
        Else
            ' Show failure message with troubleshooting guidance
            MsgBox("Community Analysis - Analysis Failed" & vbCrLf & vbCrLf &
                   "An error occurred during community analysis" & vbCrLf & vbCrLf &
                   "Possible causes:" & vbCrLf &
                   "• Invalid k values for the network structure" & vbCrLf &
                   "• Insufficient data for community detection" & vbCrLf &
                   "• System resource limitations" & vbCrLf & vbCrLf &
                   "Suggested solutions:" & vbCrLf &
                   "1. Try different k values (2-10 recommended)" & vbCrLf &
                   "2. Ensure network has sufficient complexity" & vbCrLf &
                   "3. Check system logs for detailed error information" & vbCrLf &
                   "4. Contact technical support if the problem persists",
                   MsgBoxStyle.Critical, "Community Analysis")
        End If
    End Sub

    ' 修改序列分析函数
    Private Sub 序列分析ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 序列分析ToolStripMenuItem.Click
        Dim currentUri As Uri = WebView_main.Source
        Dim timeStamp As String = Path.GetFileNameWithoutExtension(currentUri.ToString)

        ' Define file paths
        Dim hap_fasta As String = currentDirectory + "history\" + timeStamp + "_hap.fasta"

        ' Check if all required files exist
        Dim hapFastaExists As Boolean = File.Exists(hap_fasta)

        If Not hapFastaExists Then
            MsgBox("Sequence Analysis - File Validation Failed" & vbCrLf & vbCrLf &
                   "Required haplotype sequence file not found." & vbCrLf &
                   "Expected file: " & hap_fasta & vbCrLf & vbCrLf &
                   "Please ensure the following steps are completed:" & vbCrLf &
                   "1. Load sequence data" & vbCrLf &
                   "2. Build haplotype network" & vbCrLf & vbCrLf &
                   "Note: Haplotype sequences are generated during network construction.",
                   MsgBoxStyle.Exclamation, "Sequence Analysis")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\seq_analysis.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Sequence Analysis - Executable Missing" & vbCrLf & vbCrLf &
                   "Analysis executable not found at:" & vbCrLf &
                   exePath & vbCrLf & vbCrLf &
                   "Please check if the program installation is complete or contact technical support.",
                   MsgBoxStyle.Critical, "Sequence Analysis")
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

        ' Show processing message
        MsgBox("Sequence Analysis - Processing Started" & vbCrLf & vbCrLf &
               "Haplotype sequence analysis is in progress..." & vbCrLf &
               "This analysis will generate:" & vbCrLf &
               "• P-distance heatmap" & vbCrLf &
               "• Principal Component Analysis" & vbCrLf &
               "• Sequence conservation plot" & vbCrLf & vbCrLf &
               "Results will be displayed automatically upon completion.",
               MsgBoxStyle.Information, "Sequence Analysis")

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        ' 准备结果文件列表
        Dim resultFiles As New List(Of (FilePath As String, Description As String))
        resultFiles.Add((heatmapPlot, "P-distance heatmap between haplotypes"))
        resultFiles.Add((pcaPlot, "Principal Component Analysis of p-distances"))
        resultFiles.Add((seqConservationPlot, "Sequence conservation across haplotypes"))

        ' 将结果添加到CSV文件
        Dim csvFilePath As String = root_path + "history\" + timeStamp + "_rst.csv"
        AddAnalysisResultsToCSV(csvFilePath, "SequenceAnalysis", resultFiles, success)

        If success Then
            ' Log to history file
            LogAnalysisHistory(timeStamp, "Hap Sequence Analysis")

            ' Show success message with detailed instructions
            MsgBox("Sequence Analysis - Analysis Completed" & vbCrLf & vbCrLf &
                   "Haplotype sequence analysis completed successfully!" & vbCrLf & vbCrLf &
                   "Three visualization files have been generated:" & vbCrLf &
                   "• P-distance heatmap between haplotypes" & vbCrLf &
                   "• Principal Component Analysis of p-distances" & vbCrLf &
                   "• Sequence conservation across haplotypes" & vbCrLf & vbCrLf &
                   "View Analysis Results:" & vbCrLf &
                   "• All analysis results have been integrated into the comprehensive report" & vbCrLf &
                   "• The result report page will refresh automatically",
                   MsgBoxStyle.Information, "Sequence Analysis")

            ' 生成更新的HTML报告
            Dim htmlReportPath As String = root_path + "history\" + timeStamp + "_report.html"
            If File.Exists(csvFilePath) Then
                GenerateHtmlReport(csvFilePath, htmlReportPath, timeStamp,
                                   DateTime.Now.ToString("yyyy-MM-dd HH:mm"), True)
                ' 重新加载WebView21中的HTML报告
                Me.Invoke(Sub()
                              ' 检查当前显示的是否为报告页面
                              If WebView21.Source.ToString().Contains("_report.html") Then
                                  ' 如果当前已经显示报告页面，则刷新
                                  WebView21.Reload()
                              Else
                                  ' 如果当前不是显示报告页面，则导航到报告页面
                                  Me.Invoke(set_web_analysis_url, New Object() {"file:///" + htmlReportPath})
                              End If
                          End Sub)
            End If
        Else
            ' Show failure message with troubleshooting guidance
            MsgBox("Sequence Analysis - Analysis Failed" & vbCrLf & vbCrLf &
                   "An error occurred during sequence analysis" & vbCrLf & vbCrLf &
                   "Possible causes:" & vbCrLf &
                   "• Invalid sequence format in haplotype file" & vbCrLf &
                   "• Insufficient sequence diversity" & vbCrLf &
                   "• Memory limitations for large datasets" & vbCrLf & vbCrLf &
                   "Suggested solutions:" & vbCrLf &
                   "1. Verify haplotype sequence file integrity" & vbCrLf &
                   "2. Check sequence quality and format" & vbCrLf &
                   "3. Review system logs for detailed error information" & vbCrLf &
                   "4. Contact technical support if the problem persists",
                   MsgBoxStyle.Critical, "Sequence Analysis")
        End If
    End Sub

    ' 修改群体信息统计函数
    Private Sub 群体信息统计ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 群体信息统计ToolStripMenuItem.Click
        Dim currentUri As Uri = WebView_main.Source
        Dim timeStamp As String = Path.GetFileNameWithoutExtension(currentUri.ToString)

        ' Define file paths
        Dim hap_fasta As String = currentDirectory + "history\" + timeStamp + "_hap.fasta"

        ' Check if all required files exist
        Dim hapFastaExists As Boolean = File.Exists(hap_fasta)

        If Not hapFastaExists Then
            MsgBox("Population Analysis - File Validation Failed" & vbCrLf & vbCrLf &
                   "Required haplotype sequence file not found." & vbCrLf &
                   "Expected file: " & hap_fasta & vbCrLf & vbCrLf &
                   "Please ensure the following steps are completed:" & vbCrLf &
                   "1. Load sequence data" & vbCrLf &
                   "2. Build haplotype network" & vbCrLf & vbCrLf &
                   "Note: Haplotype sequences are generated during network construction.",
                   MsgBoxStyle.Exclamation, "Population Analysis")
            Return
        End If

        ' Define executable path
        Dim exePath As String = currentDirectory + "analysis\population_analysis.exe"

        ' Verify executable exists
        If Not File.Exists(exePath) Then
            MsgBox("Population Analysis - Executable Missing" & vbCrLf & vbCrLf &
                   "Analysis executable not found at:" & vbCrLf &
                   exePath & vbCrLf & vbCrLf &
                   "Please check if the program installation is complete or contact technical support.",
                   MsgBoxStyle.Critical, "Population Analysis")
            Return
        End If

        ' Define command line arguments
        Dim arguments As String = "-i """ & hap_fasta & """ -o " & timeStamp & """_population_stats.txt"

        ' Define expected output files
        Dim expectedOutputFiles As New List(Of String)
        Dim populationStat As String = currentDirectory + "history\" + timeStamp + "_population_stats.txt"
        expectedOutputFiles.Add(populationStat)

        ' Show processing message
        MsgBox("Population Analysis - Processing Started" & vbCrLf & vbCrLf &
               "Population statistics analysis is in progress..." & vbCrLf &
               "This analysis will calculate genetic diversity metrics and population statistics." & vbCrLf &
               "Results will be displayed automatically upon completion.",
               MsgBoxStyle.Information, "Population Analysis")

        ' Run the analysis
        Dim success As Boolean = RunExecutable(exePath, arguments, expectedOutputFiles)

        ' 准备结果文件列表
        Dim resultFiles As New List(Of (FilePath As String, Description As String))
        resultFiles.Add((populationStat, "Population statistics summary"))

        ' 将结果添加到CSV文件
        Dim csvFilePath As String = root_path + "history\" + timeStamp + "_rst.csv"
        AddAnalysisResultsToCSV(csvFilePath, "PopulationStatistics", resultFiles, success)

        If success Then
            ' Log to history file
            LogAnalysisHistory(timeStamp, "Population Analysis")

            ' Show success message with detailed instructions
            MsgBox("Population Analysis - Analysis Completed" & vbCrLf & vbCrLf &
                   "Population analysis completed successfully!" & vbCrLf & vbCrLf &
                   "Population statistics summary has been generated:" & vbCrLf &
                   "• File location: " & populationStat & vbCrLf &
                   "• Contains genetic diversity metrics and population parameters" & vbCrLf & vbCrLf &
                   "View Analysis Results:" & vbCrLf &
                   "• All analysis results have been integrated into the comprehensive report" & vbCrLf &
                   "• The result report page will refresh automatically",
                   MsgBoxStyle.Information, "Population Analysis")

            ' 生成更新的HTML报告
            Dim htmlReportPath As String = root_path + "history\" + timeStamp + "_report.html"
            If File.Exists(csvFilePath) Then
                GenerateHtmlReport(csvFilePath, htmlReportPath, timeStamp,
                                   DateTime.Now.ToString("yyyy-MM-dd HH:mm"), True)
                ' 重新加载WebView21中的HTML报告
                Me.Invoke(Sub()
                              ' 检查当前显示的是否为报告页面
                              If WebView21.Source.ToString().Contains("_report.html") Then
                                  ' 如果当前已经显示报告页面，则刷新
                                  WebView21.Reload()
                              Else
                                  ' 如果当前不是显示报告页面，则导航到报告页面
                                  Me.Invoke(set_web_analysis_url, New Object() {"file:///" + htmlReportPath})
                              End If
                          End Sub)
            End If
        Else
            ' Show failure message with troubleshooting guidance
            MsgBox("Population Analysis - Analysis Failed" & vbCrLf & vbCrLf &
                   "An error occurred during population analysis" & vbCrLf & vbCrLf &
                   "Possible causes:" & vbCrLf &
                   "• Invalid sequence format in haplotype file" & vbCrLf &
                   "• Insufficient sample size for statistical analysis" & vbCrLf &
                   "• Missing population grouping information" & vbCrLf & vbCrLf &
                   "Suggested solutions:" & vbCrLf &
                   "1. Verify haplotype sequence file format and quality" & vbCrLf &
                   "2. Ensure adequate sample size (minimum 10 sequences recommended)" & vbCrLf &
                   "3. Check system logs for detailed error information" & vbCrLf &
                   "4. Contact technical support if the problem persists",
                   MsgBoxStyle.Critical, "Population Analysis")
        End If
    End Sub

    Private Sub 性状关联分析ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 性状关联分析ToolStripMenuItem.Click

    End Sub
End Class
