Imports System.IO
Public Class Config_Data
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RadioButton1.Checked = True
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "CSV文件(*.*)|*.csv;*.CSV"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".csv"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox1.Text = opendialog.FileName
            Dim sr As New StreamReader(TextBox1.Text)
            Dim title() As String = sr.ReadLine.Split(",")
            sr.Close()
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()
            ComboBox3.Items.Clear()
            For Each i As String In title
                ComboBox1.Items.Add(i)
                ComboBox2.Items.Add(i)
                ComboBox3.Items.Add(i)
            Next

            ComboBox1.SelectedIndex = 0
            ComboBox2.SelectedIndex = 0
            ComboBox3.SelectedIndex = 0
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

        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If TextBox2.Text <> "" Then
            Dim th1 As New Threading.Thread(AddressOf But5)
            th1.Start()
            Me.Hide()
        Else
            MsgBox("Please specify the output folder.")
        End If
    End Sub

    Public Sub But5()
        Dim file_type As String = "csv"
        Dim input As String = TextBox1.Text
        If RadioButton2.Checked Then
            file_type = "fas"
            input = TextBox3.Text
        End If
        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = currentDirectory + "analysis\MakeData.exe" ' 替换为实际的命令行程序路径
        startInfo.WorkingDirectory = currentDirectory + "results\" ' 替换为实际的运行文件夹路径
        'startInfo.CreateNoWindow = True
        If RadioButton1.Checked Then
            startInfo.Arguments = "-input " + """" + input + """" + " -file_type " + file_type + " -index_name " + ComboBox1.SelectedIndex.ToString + " -index_type " + ComboBox2.SelectedIndex.ToString + " -index_seq " + ComboBox3.SelectedIndex.ToString + " -out_dir " + """" + TextBox2.Text + """" + " -clean " + CInt(CheckBox1.Checked).ToString + " -valid " + CInt(CheckBox2.Checked).ToString
        Else
            startInfo.Arguments = "-input " + """" + input + """" + " -file_type " + file_type + " -out_dir " + """" + TextBox2.Text + """" + " -clean " + CInt(CheckBox1.Checked).ToString + " -valid " + CInt(CheckBox2.Checked).ToString
        End If
        Dim process As Process = Process.Start(startInfo)
        process.WaitForExit()
        process.Close()
        MsgBox("Processing completed.")
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        RadioButton2.Checked = True
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "Fasta file(*.fasta)|*.fas;*.FAS;*.fasta;*.FASTA"
        opendialog.FileName = ""
        opendialog.Multiselect = False
        opendialog.DefaultExt = ".fasta"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            TextBox3.Text = opendialog.FileName

        End If
    End Sub

    Private Sub Config_Data_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class