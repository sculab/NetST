Imports System.IO
Imports System.Threading

Public Class Welcome
    Dim total_file As Integer = 2
    Dim current_file As Single = 0

    Private Sub Welcome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Thread.CurrentThread.CurrentCulture = ci
        Timer1.Enabled = True
        cpu_info = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")
        format_path()
        load_main()

        'MsgBox(cpu_info)
        form_main.Show()
    End Sub

    Public Sub load_main()
        My.Computer.FileSystem.CreateDirectory(root_path + "history")
        My.Computer.FileSystem.CreateDirectory(root_path + "temp")
        current_file = total_file

        If language = "CH" Then
            to_ch()
        Else
            to_en()
        End If

        form_main.获取序列信息ToolStripMenuItem.Visible = True
        form_main.ToolStripSeparator11.Visible = True
        form_main.网络图可视化ToolStripMenuItem.Visible = True
    End Sub

    Private Sub Welcome_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If current_file < total_file Then
            ProgressBar1.Value = Math.Min(current_file/total_file*100, 100)
        Else
            Timer1.Enabled = False
            Me.Hide()
        End If
    End Sub

    Public Sub DeleteTemp(aimPath As String)
        If (aimPath(aimPath.Length - 1) <> Path.DirectorySeparatorChar) Then
            aimPath += Path.DirectorySeparatorChar
        End If  '判断待删除的目录是否存在,不存在则退出.  
        If (Not Directory.Exists(aimPath)) Then Exit Sub ' 
        Dim fileList() As String = Directory.GetFileSystemEntries(aimPath)  ' 遍历所有的文件和目录  
        total_file = Math.Max(fileList.Length, total_file)
        For Each FileName As String In fileList
            If (Directory.Exists(FileName)) Then ' 先当作目录处理如果存在这个目录就递归
                DeleteDir(aimPath + Path.GetFileName(FileName))
            Else ' 否则直接Delete文件  
                Try
                    File.Delete(aimPath + Path.GetFileName(FileName))
                    current_file += 1
                Catch ex As Exception
                End Try
            End If
        Next  '删除文件夹  
    End Sub

    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs) Handles ProgressBar1.Click
    End Sub
End Class