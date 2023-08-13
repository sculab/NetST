Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Public Class Config_Stand

    Private Sub Config_Stand_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Me.Hide()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim th1 As New Thread(AddressOf butt1)
        th1.Start()
        Me.Hide()
    End Sub
    Public Sub butt1()
        format_fasta(current_file, root_path + "temp\temp_file.tmp", new_line(CheckBox1.Checked))
        timer_id = 2
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub



    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        TextBox3.ReadOnly = CheckBox2.Checked Xor True
        NumericUpDown1.ReadOnly = CheckBox2.Checked Xor True
        If TextBox3.Text <> "" And CheckBox2.Checked Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox3.Text)(NumericUpDown1.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        TextBox4.ReadOnly = CheckBox6.Checked Xor True
        NumericUpDown2.ReadOnly = CheckBox6.Checked Xor True
        If TextBox4.Text <> "" And CheckBox6.Checked Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox4.Text)(NumericUpDown2.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub Config_Stand_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim sr As New StreamReader(current_file)
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
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        TextBox5.ReadOnly = CheckBox7.Checked Xor True
        NumericUpDown3.ReadOnly = CheckBox7.Checked Xor True
        If TextBox5.Text <> "" And CheckBox7.Checked Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox5.Text)(NumericUpDown3.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        If TextBox3.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox3.Text)(NumericUpDown1.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        If TextBox3.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox3.Text)(NumericUpDown1.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        If TextBox4.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox4.Text)(NumericUpDown2.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        If TextBox4.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox4.Text)(NumericUpDown2.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        If TextBox5.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox5.Text)(NumericUpDown3.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        If TextBox5.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox5.Text)(NumericUpDown3.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        If TextBox6.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox6.Text)(NumericUpDown4.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub NumericUpDown4_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown4.ValueChanged
        If TextBox6.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox6.Text)(NumericUpDown4.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        TextBox6.ReadOnly = CheckBox8.Checked Xor True
        NumericUpDown4.ReadOnly = CheckBox8.Checked Xor True
        If TextBox6.Text <> "" And CheckBox8.Checked Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox6.Text)(NumericUpDown4.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub Config_Stand_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible Then
            Dim sr As New StreamReader(current_file)
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
            TextBox7.Text = ""
        End If
    End Sub

    Private Sub CheckBox9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        TextBox8.ReadOnly = CheckBox9.Checked Xor True
        NumericUpDown5.ReadOnly = CheckBox9.Checked Xor True
        If TextBox8.Text <> "" And CheckBox9.Checked Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox8.Text)(NumericUpDown5.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        If TextBox8.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox8.Text)(NumericUpDown5.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub

    Private Sub NumericUpDown5_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown5.ValueChanged
        If TextBox8.Text <> "" Then
            TextBox7.Text = ""
            For Each i As String In TextPreview.Lines
                Try
                    TextBox7.Text += i.Split(TextBox8.Text)(NumericUpDown5.Value) + vbCrLf
                Catch ex As Exception
                    TextBox7.Text += "Error:" + i + vbCrLf
                End Try
            Next
        End If
    End Sub
End Class