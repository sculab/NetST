﻿Imports System.IO
Imports System.Text

Module Module_Function
    Public Function ReadSettings(filePath As String) As Dictionary(Of String, String)
        Dim settings As New Dictionary(Of String, String)()

        If File.Exists(filePath) Then
            Dim lines As String() = File.ReadAllLines(filePath)

            For Each line As String In lines
                Dim parts As String() = line.Split("="c)
                If parts.Length = 2 Then
                    Dim key As String = parts(0).Trim()
                    Dim value As String = parts(1).Trim()
                    settings(key) = value
                End If
            Next
        End If

        Return settings
    End Function

    ' 保存设置到文件
    Public Sub SaveSettings(filePath As String, settings As Dictionary(Of String, String))
        Dim lines As New List(Of String)()

        For Each kvp As KeyValuePair(Of String, String) In settings
            Dim line As String = $"{kvp.Key}={kvp.Value}"
            lines.Add(line)
        Next
        File.WriteAllLines(filePath, lines)
    End Sub

    Public Function new_line(is_LF As Boolean) As String
        If is_LF Then
            Return vbLf
        Else
            Return vbCrLf
        End If
    End Function


    ' 优化版本1：线程安全的信息显示函数
    Public Sub show_info(info As String)
        Try
            ' 检查是否需要线程调用
            If form_main.RichTextBox1.InvokeRequired Then
                ' 在UI线程上执行
                form_main.RichTextBox1.Invoke(Sub()
                                                  AppendTextSafely(info)
                                              End Sub)
            Else
                ' 已在UI线程上，直接执行
                AppendTextSafely(info)
            End If
        Catch ex As Exception
            ' 如果UI操作失败，至少在控制台显示信息
            Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd H:mm:ss}] {info}")
            Console.WriteLine($"Error updating UI: {ex.Message}")
        End Try
    End Sub

    ' 辅助函数：安全地添加文本
    Private Sub AppendTextSafely(info As String)
        Try
            ' 检查控件是否已创建且可用
            If form_main.RichTextBox1 IsNot Nothing AndAlso
           Not form_main.RichTextBox1.IsDisposed AndAlso
           form_main.RichTextBox1.IsHandleCreated Then

                ' 格式化时间和信息
                Dim formattedMessage As String = $"{DateTime.Now:yyyy/MM/dd H:mm:ss}{vbTab}{info}{vbCrLf}"

                ' 添加文本
                form_main.RichTextBox1.AppendText(formattedMessage)

                ' 强制刷新显示
                form_main.RichTextBox1.Refresh()

                ' 自动滚动到最新内容
                form_main.RichTextBox1.SelectionStart = form_main.RichTextBox1.Text.Length
                form_main.RichTextBox1.ScrollToCaret()

                ' 限制文本长度，防止内存过多占用
                LimitTextLength()

            Else
                ' 控件不可用时的备用方案
                Console.WriteLine($"[{DateTime.Now:yyyy/MM/dd H:mm:ss}] {info}")
            End If
        Catch ex As Exception
            Console.WriteLine($"Error in AppendTextSafely: {ex.Message}")
        End Try
    End Sub

    ' 限制文本长度的辅助函数
    Private Sub LimitTextLength()
        Try
            Const MAX_LENGTH As Integer = 100000 ' 最大字符数
            If form_main.RichTextBox1.Text.Length > MAX_LENGTH Then
                ' 保留最新的80%内容
                Dim keepLength As Integer = CInt(MAX_LENGTH * 0.8)
                Dim textToKeep As String = form_main.RichTextBox1.Text.Substring(form_main.RichTextBox1.Text.Length - keepLength)
                form_main.RichTextBox1.Text = "... (earlier content truncated)" & vbCrLf & textToKeep
                form_main.RichTextBox1.SelectionStart = form_main.RichTextBox1.Text.Length
                form_main.RichTextBox1.ScrollToCaret()
            End If
        Catch ex As Exception
            ' 如果截断失败，清空文本框
            Try
                form_main.RichTextBox1.Text = $"[{DateTime.Now:yyyy/MM/dd H:mm:ss}] Log cleared due to length limit" & vbCrLf
            Catch
                ' 忽略清空错误
            End Try
        End Try
    End Sub


    Public Function Check_Mixed_AA(seq As String) As Boolean
        Dim mixed_AA() As String = {"R", "Y", "M", "K", "S", "W", "H", "B", "V", "D", "N"}
        For Each i As String In mixed_AA
            If seq.ToUpper.Contains(i) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function RemoveNonAGCTChars(input As String) As String
        Dim result = ""
        For Each c As Char In input
            If "AGCT".Contains(c) Then
                result &= c
            End If
        Next
        Return result
    End Function

    Public Sub safe_copy(source, target)
        If File.Exists(source) Then
            File.Copy(source, target, True)
            File.Delete(source)
        End If
    End Sub

    Public Sub DeleteDir(aimPath As String)
        If (aimPath(aimPath.Length - 1) <> Path.DirectorySeparatorChar) Then
            aimPath += Path.DirectorySeparatorChar
        End If  '判断待删除的目录是否存在,不存在则退出.  
        If (Not Directory.Exists(aimPath)) Then Exit Sub ' 
        Dim fileList() As String = Directory.GetFileSystemEntries(aimPath)  ' 遍历所有的文件和目录  
        For Each FileName As String In fileList
            If (Directory.Exists(FileName)) Then ' 先当作目录处理如果存在这个目录就递归
                DeleteDir(aimPath + Path.GetFileName(FileName))
            Else ' 否则直接Delete文件  
                Try
                    File.Delete(aimPath + Path.GetFileName(FileName))
                Catch ex As Exception
                End Try
            End If
        Next  '删除文件夹  
    End Sub

    Public Sub format_path()
        Select Case TargetOS
            Case "linux"
                path_char = "/"
            Case "win32", "macos"
                path_char = "\"
            Case Else
                path_char = "\"
        End Select
        root_path = (Application.StartupPath + path_char).Replace(path_char + path_char, path_char)
        Dec_Sym = CInt("0").ToString("F1").Replace("0", "")
        If Dec_Sym <> "." Then
            MsgBox(
                "Notice: We will use dat (.) as decimal quotation instead of comma (,). We recommand to change your system's number format to English! ")
        End If
    End Sub

    Public Function Check_Prec_Ns(seq As String) As Single
        seq = seq.ToUpper.Replace("-", "")
        Dim total_length As Integer = seq.Length
        seq = seq.ToUpper.Replace("N", "")
        Return (total_length - seq.Length)/total_length
    End Function

    Public Function hap_fasta(input As String, output As String, new_line_char As String) As Boolean
        Try
            info_text = "Preloading Sequences ..."
            show_info("Calculating haplotypes")
            Dim total_number As Integer = get_total_number(input)
            Dim count = 0
            Dim hap_seq() As String
            Dim name() As String
            Dim org_seq() As String
            Dim org_seq_clean() As String
            Dim hap_name() As Integer
            Dim gap_list() As Integer
            ReDim hap_seq(0)
            ReDim hap_name(0)
            ReDim name(total_number)
            ReDim org_seq(total_number)
            ReDim org_seq_clean(total_number)
            ReDim gap_list(0)
            Dim AA() As Char = "ACGTU"

            Dim sr As New StreamReader(input)
            Dim line As String

            line = sr.ReadLine
            Do
                If line <> "" Then
                    If line(0) = ">" Then
                        count += 1
                        name(count) = line.Substring(1)
                        info_text = "Reading sequence " + count.ToString
                        'PB_value = 100 * count / total_number
                    Else
                        org_seq_clean(count) += line.ToUpper
                        org_seq(count) += line.ToUpper
                    End If
                End If
                line = sr.ReadLine
            Loop Until line Is Nothing
            sr.Close()

            gap_list(0) = - 1

            Dim is_var() As Boolean
            ReDim is_var(org_seq(1).Length - 1)
            For j = 0 To org_seq(1).Length - 1
                is_var(j) = False
            Next
            For i = 1 To UBound(org_seq)
                info_text = "Checking wobbles in seq " + i.ToString
                'PB_value = 100 * i / UBound(org_seq)
                For j = 0 To org_seq(1).Length - 1
                    If is_var(j) = False Then
                        If Array.IndexOf(AA, org_seq(i)(j)) < 0 Then
                            is_var(j) = True
                        End If
                    End If

                Next
            Next
            For j = 0 To UBound(is_var)
                If is_var(j) = True Then
                    ReDim Preserve gap_list(UBound(gap_list) + 1)
                    gap_list(UBound(gap_list)) = j
                End If
            Next
            Array.Sort(gap_list)

            For i = 1 To UBound(org_seq_clean)
                Dim temp_char() As Char = org_seq_clean(i)
                info_text = "Cleaning sequence " + i.ToString
                'PB_value = 100 * i / UBound(org_seq_clean)
                For j = 0 To UBound(is_var)
                    If is_var(j) = True Then
                        temp_char(j) = "#"
                    End If
                Next
                Dim temp_seq As String = temp_char
                org_seq_clean(i) = temp_seq.Replace("#", "")
            Next

            Dim is_dup() As Boolean
            ReDim is_dup(UBound(org_seq_clean))
            For j = 0 To UBound(org_seq_clean)
                is_dup(j) = False
            Next
            For i = 1 To UBound(org_seq_clean)
                info_text = "Calculating duplicate " + i.ToString
                'PB_value = 100 * i / UBound(org_seq_clean)
                If is_dup(i) = False Then
                    For j As Integer = i + 1 To UBound(org_seq_clean)
                        If org_seq_clean(i) = org_seq_clean(j) Then
                            is_dup(j) = True
                        End If
                    Next
                End If
            Next
            '写入不重复序列
            'Dim sw_seq_nodup As New StreamWriter(output + ".nodup.fasta", False)
            'For i As Integer = 1 To UBound(org_seq_clean)
            '	If is_dup(i) = False Then
            '	sw_seq_nodup.Write(">" + name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(0))
            '	sw_seq_nodup.Write(new_line_char)
            '		sw_seq_nodup.Write(org_seq_clean(i))
            '		sw_seq_nodup.Write(new_line_char)
            '	End If
            'Next
            'sw_seq_nodup.Close()

            For i = 1 To UBound(org_seq_clean)
                info_text = "Calculating haplotype " + i.ToString
                'PB_value = 100 * i / UBound(org_seq_clean)

                line = org_seq_clean(i)
                ReDim Preserve hap_name(UBound(hap_name) + 1)
                Dim hap_index As Integer = Array.IndexOf(hap_seq, line)
                If hap_index > 0 Then
                    hap_name(UBound(hap_name)) = hap_index
                Else
                    ReDim Preserve hap_seq(UBound(hap_seq) + 1)
                    hap_seq(UBound(hap_seq)) = line
                    hap_name(UBound(hap_name)) = UBound(hap_seq)
                    count += 1
                End If
            Next
            Dim sw_seq_hap As New StreamWriter(output + "_seq2hap.csv", False)
            sw_seq_hap.Write("id,hap,name,trait")
            sw_seq_hap.Write(new_line_char)
            For i = 1 To UBound(org_seq_clean)
                sw_seq_hap.Write(i.ToString + ",")
                sw_seq_hap.Write("Hap_" + hap_name(i).ToString + ",")
                Dim parts As String() = name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)
                sw_seq_hap.Write(parts(0).Split("="c)(0) + ",")
                sw_seq_hap.Write(parts(1))
                sw_seq_hap.Write(new_line_char)
            Next
            sw_seq_hap.Close()

            Dim trait_list() As String
            ReDim trait_list(0)
            For i = 1 To UBound(name)
                Dim s As String = name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(1)
                If Array.IndexOf(trait_list, s) < 0 Then
                    ReDim Preserve trait_list(UBound(trait_list) + 1)
                    trait_list(UBound(trait_list)) = s
                End If
            Next
            Array.Sort(trait_list)

            Dim hap_trait_table(,) As String
            ReDim hap_trait_table(UBound(hap_seq), UBound(trait_list))

            For i = 0 To UBound(trait_list)
                For j = 0 To UBound(hap_seq)
                    hap_trait_table(j, i) = 0
                Next
            Next
            For i = 1 To UBound(trait_list)
                hap_trait_table(0, i) = trait_list(i)
            Next
            For i = 1 To UBound(hap_seq)
                hap_trait_table(i, 0) = "Hap_" + i.ToString
            Next
            hap_trait_table(0, 0) = ""
            For i = 1 To UBound(name)
                Dim s As String = name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(1)
                hap_trait_table(hap_name(i), Array.IndexOf(trait_list, s)) =
                    CInt(hap_trait_table(hap_name(i), Array.IndexOf(trait_list, s))) + 1
            Next
            'Building trait table for haplotype
            Dim hap_trait_sw As New StreamWriter(output + "_hap_trait.csv", False)
            For j = 0 To UBound(hap_seq)
                info_text = "Building trait table for haplotype " + j.ToString
                'PB_value = 100 * j / UBound(hap_seq)
                For i = 0 To (UBound(trait_list) - 1)
                    hap_trait_sw.Write(hap_trait_table(j, i) + ",")
                Next
                hap_trait_sw.Write(hap_trait_table(j, UBound(trait_list)) + vbCrLf)
            Next
            hap_trait_sw.Close()
            'Building trait table for sequence
            Dim seq_trait_sw As New StreamWriter(output + "_seq_trait.csv", False)
            For i = 1 To UBound(trait_list)
                seq_trait_sw.Write("," + trait_list(i))
            Next
            seq_trait_sw.Write(vbCrLf)


            For j = 1 To UBound(name)
                info_text = "Building trait table for sequence " + j.ToString
                'PB_value = 100 * j / UBound(name)

                seq_trait_sw.Write(name(j).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(0))
                For i = 1 To (UBound(trait_list))
                    If name(j).ToUpper.Contains(trait_list(i)) Then
                        seq_trait_sw.Write(",1")
                    Else
                        seq_trait_sw.Write(",0")
                    End If
                Next
                seq_trait_sw.Write(vbCrLf)
            Next
            seq_trait_sw.Close()

            Dim reduced_seq() As String
            ReDim reduced_seq(UBound(hap_seq))
            Dim SNP_Pos(0) As Integer
            ReDim is_var(hap_seq(1).Length - 1)
            info_text = "Checking haplotype site ... "
            For j = 1 To hap_seq(1).Length
                'PB_value = 100 * j / hap_seq(1).Length

                is_var(j - 1) = False

                If Array.IndexOf(AA, hap_seq(1)(j - 1)) >= 0 Then
                    For i = 1 To UBound(hap_seq)
                        If Array.IndexOf(AA, hap_seq(i)(j - 1)) < 0 Then
                            GoTo go_to1
                        End If
                    Next


                    For k = 2 To UBound(hap_seq)
                        If hap_seq(1)(j - 1) <> hap_seq(k)(j - 1) Then
                            is_var(j - 1) = True
                            Exit For
                        End If
                    Next
                    If is_var(j - 1) Then
                        ReDim Preserve SNP_Pos(UBound(SNP_Pos) + 1)
                        SNP_Pos(UBound(SNP_Pos)) = j
                        go_to1:
                    End If
                End If

            Next
            For i = 1 To UBound(hap_seq)
                Dim temp_char() As Char = hap_seq(i)
                For j = 0 To hap_seq(1).Length - 1
                    If is_var(j) = False Then
                        temp_char(j) = "#"
                    End If
                Next
                Dim temp_seq As String = temp_char
                reduced_seq(i) = temp_seq.Replace("#", "")
            Next
            Dim reduced_seq1() As String
            ReDim reduced_seq1(UBound(org_seq_clean))
            For i = 1 To UBound(org_seq_clean)
                Dim temp_char() As Char = org_seq_clean(i)
                For j = 0 To org_seq_clean(1).Length - 1
                    If is_var(j) = False Then
                        temp_char(j) = "#"
                    End If
                Next
                Dim temp_seq As String = temp_char
                reduced_seq1(i) = temp_seq.Replace("#", "")
            Next


            For i = 1 To UBound(gap_list)
                For j = 1 To UBound(SNP_Pos)
                    If gap_list(i) <= SNP_Pos(j) - 1 Then
                        SNP_Pos(j) += 1
                    End If
                Next
            Next

            Dim sw As New StreamWriter(output + "_hap.fasta", False)
            For i = 1 To UBound(reduced_seq)
                sw.Write(">Hap_" + i.ToString)
                sw.Write(new_line_char)
                sw.Write(reduced_seq(i))
                sw.Write(new_line_char)
            Next
            sw.Close()
            Dim sw1 As New StreamWriter(output + "_hap.phy", False)
            sw1.Write(UBound(reduced_seq).ToString + " " + reduced_seq(1).Length.ToString + vbCrLf)
            For i = 1 To UBound(reduced_seq)
                sw1.Write("Hap_" + i.ToString + " " + reduced_seq(i))
                sw1.Write(new_line_char)
            Next
            sw1.Close()

            Dim sw2 As New StreamWriter(output + "_seq.phy", False)
            sw2.Write(UBound(reduced_seq1).ToString + " " + reduced_seq1(1).Length.ToString + vbCrLf)
            For i = 1 To UBound(reduced_seq1)
                sw2.Write(
                    name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(0) + " " + reduced_seq1(i))
                sw2.Write(new_line_char)
            Next
            sw2.Close()

            Dim sw3 As New StreamWriter(output + "_seq.fasta", False)
            For i = 1 To UBound(reduced_seq1)
                sw3.Write(
                    ">" + name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(0) + Chr(13) +
                    reduced_seq1(i))
                sw3.Write(new_line_char)
            Next
            sw3.Close()

            Dim sw_meta As New StreamWriter(output + ".meta", False)
            For i = 1 To UBound(reduced_seq1)
                If name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(1) <> "" Then
                    sw_meta.Write(
                        name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(0) + "	" +
                        name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(1))
                Else
                    sw_meta.Write(
                        name(i).ToUpper.Split(New String() {"$SPLIT$"}, StringSplitOptions.None)(0) + "	Default")
                End If
                sw_meta.Write(new_line_char)
            Next
            sw_meta.Close()

            show_info(
                (hap_seq.Length - 1).ToString + " haplotype and " + reduced_seq(1).Length.ToString +
                " variation sites in total, ")
            show_info("Processing completed.")
            info_text = ""
            Return True
        Catch ex As Exception
            show_info(ex.ToString)
            show_info("Process terminated.")
            info_text = ""
            PB_value = 0
            Return False
        End Try
    End Function

    Public Function format_fasta(input As String, output As String, new_line_char As String) As Boolean
        Try
            timer_id = 0
            info_text = "Preloading Sequences ..."
            Dim total_number As Integer = get_total_number(input)
            show_info("Start format")
            Dim line As String
            Dim current_name = ""
            Dim current_state = ""
            Dim current_seq = ""
            Dim current_count = "1"
            Dim current_time = "0"
            Dim current_org = ""
            dtView.AllowNew = True
            dtView.AllowEdit = True
            Dim newrow(6) As String
            Dim count = 0
            Dim name() As String
            Dim pre_fasta_seq = 0
            If add_data Then
                pre_fasta_seq = UBound(fasta_seq)
                ReDim Preserve fasta_seq(pre_fasta_seq + total_number)
            Else
                ReDim fasta_seq(total_number)
            End If
            ReDim name(total_number)
            Dim sr As New StreamReader(input)
            line = sr.ReadLine
            Do
                If line <> "" Then
                    If line(0) = ">" Then
                        count += 1
                        name(count) = line.Substring(1)
                        info_text = "Reading sequence " + count.ToString
                        PB_value = 100*count/total_number

                    Else
                        If form_config_stand.CheckBox5.Checked Then
                            Dim mixed_AA() As String = {"R", "Y", "M", "K", "S", "W", "H", "B", "V", "D", "N"}
                            For Each i As String In mixed_AA
                                line = line.Replace(i, "-")
                            Next
                        End If
                        fasta_seq(pre_fasta_seq + count) += line.ToUpper
                    End If
                End If
                line = sr.ReadLine
            Loop Until line Is Nothing
            sr.Close()
            For i = 1 To count
                info_text = "Formating sequence " + count.ToString
                PB_value = 100*i/total_number
                line = name(i)
                line = line.Replace(",", "_").Replace("""", "").Replace("'", "").Replace("=", "_")
                If form_config_stand.CheckBox3.Checked Then
                    line = line.Replace(form_config_stand.TextBox1.Text, form_config_stand.TextBox2.Text)
                End If
                current_name = line
                If form_config_stand.CheckBox2.Checked Then
                    If form_config_stand.TextBox3.Text <> "" Then
                        If form_config_stand.NumericUpDown1.Value <= UBound(line.Split(form_config_stand.TextBox3.Text)) _
                            Then
                            current_name =
                                line.Split(form_config_stand.TextBox3.Text)(form_config_stand.NumericUpDown1.Value)
                        Else
                            show_info("无法对" + line + "中的序列名进行拆分")
                        End If
                    End If
                End If
                If form_config_stand.CheckBox6.Checked Then
                    If form_config_stand.TextBox4.Text <> "" Then
                        If form_config_stand.NumericUpDown2.Value <= UBound(line.Split(form_config_stand.TextBox4.Text)) _
                            Then

                            current_state =
                                line.Split(form_config_stand.TextBox4.Text)(form_config_stand.NumericUpDown2.Value)
                        Else
                            show_info("Unable to split the " + line)
                        End If
                    End If
                End If
                If form_config_stand.CheckBox7.Checked Then
                    If form_config_stand.TextBox5.Text <> "" Then
                        If form_config_stand.NumericUpDown3.Value <= UBound(line.Split(form_config_stand.TextBox5.Text)) _
                            Then
                            current_count =
                                line.Split(form_config_stand.TextBox5.Text)(form_config_stand.NumericUpDown3.Value)
                        Else
                            current_count = "1"
                            show_info("Unable to split the " + line)
                        End If
                    End If
                End If
                If form_config_stand.CheckBox8.Checked Then
                    If form_config_stand.TextBox6.Text <> "" Then
                        If form_config_stand.NumericUpDown4.Value <= UBound(line.Split(form_config_stand.TextBox6.Text)) _
                            Then
                            current_time =
                                line.Split(form_config_stand.TextBox6.Text)(form_config_stand.NumericUpDown4.Value)
                        Else
                            current_time = "0"
                            show_info("Unable to split the " + line)
                        End If
                    End If
                End If
                If form_config_stand.CheckBox9.Checked Then
                    If form_config_stand.TextBox8.Text <> "" Then
                        If form_config_stand.NumericUpDown5.Value <= UBound(line.Split(form_config_stand.TextBox8.Text)) _
                            Then
                            current_org =
                                line.Split(form_config_stand.TextBox8.Text)(form_config_stand.NumericUpDown5.Value)
                        Else
                            current_org = "0"
                            show_info("Unable to split the " + line)
                        End If
                    End If
                End If

                If form_config_stand.CheckBox4.Checked Then
                    current_name = i.ToString
                End If

                dtView.AddNew()
                newrow(0) = pre_fasta_seq + i
                newrow(1) = current_name

                If Len(fasta_seq(pre_fasta_seq + i)) > 1000 Then
                    newrow(2) = fasta_seq(pre_fasta_seq + i).Substring(0, 1000) + "..."
                Else
                    newrow(2) = fasta_seq(pre_fasta_seq + i)
                End If

                newrow(3) = current_state
                newrow(4) = current_time
                newrow(5) = current_count
                newrow(6) = current_org
                dtView.Item(pre_fasta_seq + i - 1).Row.ItemArray = newrow
            Next

            dtView.AllowNew = False
            dtView.AllowEdit = True
            sr.Close()
            info_text = count.ToString + " sequences in total"
            show_info(count.ToString + " sequences in total")
            show_info("Processing completed.")
            PB_value = 0
            add_data = False
            Return True
        Catch ex As Exception
            add_data = False
            info_text = ""
            PB_value = 0
            show_info(ex.ToString)
            show_info("Formatting terminated.")
            Return False
        End Try
    End Function

    Public Function DetectFileEncoding(filePath As String) As Encoding
        Try
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                Using reader As New StreamReader(fs, Encoding.Default, True)
                    reader.Peek() ' 自动检测文件编码
                    Return reader.CurrentEncoding
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine($"无法检测文件编码：{ex.Message}")
            Return Nothing
        End Try
    End Function

    Public Function get_total_number(input As String) As Integer
        Dim pre_read As New StreamReader(input)
        Dim line As String = pre_read.ReadLine
        Dim count = 0
        Do
            If line(0) = ">" Then
                count += 1
            End If
            line = pre_read.ReadLine
        Loop Until line Is Nothing
        pre_read.Close()
        Return count
    End Function


    Public Function clean_fasta(input As String, output As String, new_line_char As String) As Boolean
        Try
            info_text = "Preloading Sequences ..."
            show_info("Starting the cleaning process")
            Dim total_number As Integer = get_total_number(input)
            Dim keeped = 0
            Dim dropped = 0
            Dim passed = True
            Dim first_seq = ""
            'If form_config_clean.CheckBox6.Checked Then
            '	If form_main.TextBox5.Lines.Length < total_number Then
            '		show_info("Error: the lines in list of main window is less than the number of sequences")
            '		show_info("end clean")
            '		Return False
            '	End If
            'End If
            Dim line = ""
            Dim fasta_seq() As String
            Dim name() As String
            Dim count = 0
            ReDim fasta_seq(total_number)
            ReDim name(total_number)

            Dim sr As New StreamReader(input)
            line = sr.ReadLine
            Do
                If line <> "" Then
                    If line(0) = ">" Then
                        count += 1
                        name(count) = line.Substring(1)
                        info_text = "Reading sequence " + count.ToString
                        PB_value = 100*count/total_number

                    Else
                        fasta_seq(count) += line.ToUpper
                    End If
                End If
                line = sr.ReadLine
            Loop Until line Is Nothing
            sr.Close()

            Dim sw As New StreamWriter(output, False)

            For j = 1 To UBound(name)
                info_text = "Analyzing sequence " + j.ToString
                PB_value = 100*j/UBound(fasta_seq)
                Dim pass_info = ""

                If form_config_clean.CheckBox5.Checked Then
                    Dim mixed() As String = {"R", "Y", "M", "K", "S", "W", "H", "B", "V", "D", "N"}
                    For Each i In mixed
                        fasta_seq(j) = fasta_seq(j).Replace(i, "-")
                    Next
                End If
                If form_config_clean.CheckBox2.Checked Then
                    If Check_Mixed_AA(fasta_seq(j)) Then
                        passed = True
                    Else
                        passed = False
                        pass_info = "have wobbles"
                    End If
                End If
                If form_config_clean.CheckBox6.Checked Then
                    name(j) = name(j).Replace(form_config_clean.TextBox6.Text, form_config_clean.TextBox5.Text)
                End If
                If form_config_clean.CheckBox7.Checked Then
                    If name(j).Contains(form_config_clean.TextBox2.Text) Then
                        passed = False
                    Else
                        passed = True
                    End If
                End If

                If form_config_clean.CheckBox8.Checked Then
                    If name(j).Contains(form_config_clean.TextBox3.Text) Then
                        passed = True
                    Else
                        passed = False
                    End If
                End If

                If form_config_clean.CheckBox4.Checked Then
                    Dim temp_pres As Single = Check_Prec_Ns(fasta_seq(j))*100

                    If temp_pres < CSng(form_config_clean.TextBox1.Text) Then
                        passed = True
                    Else
                        passed = False
                        pass_info = temp_pres.ToString("F4") + "% Ns"
                    End If
                End If
                If form_config_clean.CheckBox3.Checked And form_config_clean.TextBox9.Text <> "" Then
                    Dim temp_length As Integer = fasta_seq(j).Replace("-", "").Length
                    If temp_length >= CInt(form_config_clean.TextBox9.Text) Then
                        passed = True
                    Else
                        passed = False
                        pass_info = temp_length.ToString + " in length"
                    End If
                End If
                'If form_config_clean.CheckBox6.Checked Then
                '	If form_main.TextBox5.Lines(j - 1) = "0" Then
                '		passed = False
                '		pass_info = "removed in list"
                '	End If
                'End If
                If passed Then

                    keeped += 1
                    sw.Write(">" + name(j))
                    sw.Write(new_line_char)

                    sw.Write(fasta_seq(j))
                    sw.Write(new_line_char)
                Else
                    dropped += 1
                    form_main.RichTextBox1.AppendText(name(j) + vbTab + pass_info + Chr(13))
                End If
            Next

            sr.Close()
            sw.Close()
            show_info(count.ToString + " sequences in total")
            show_info("Keeped " + keeped.ToString + " sequences, dropped " + dropped.ToString + " sequences")
            show_info("Cleaning completed.")
            Return True
        Catch ex As Exception
            info_text = ""
            PB_value = 0
            show_info(ex.ToString)
            show_info("Terminating the cleaning process.")
            Return False
        End Try
    End Function

    Public Function get_info(input As String, output As String, new_line_char As String) As Boolean
        Try
            info_text = "Preloading Sequences ..."
            show_info("Starting to process the file.")
            Dim total_number As Integer = get_total_number(input)

            Dim sr As New StreamReader(input)
            Dim line = ""
            Dim fasta_seq() As String
            Dim name() As String
            Dim count = 0
            ReDim fasta_seq(total_number)
            ReDim name(total_number)

            line = sr.ReadLine
            Do
                If line <> "" Then
                    If line(0) = ">" Then
                        count += 1
                        name(count) = line.Substring(1)
                        info_text = "Reading sequence " + count.ToString
                        PB_value = 100*count/total_number

                    Else
                        fasta_seq(count) += line.ToUpper
                    End If
                End If
                line = sr.ReadLine
            Loop Until line Is Nothing
            sr.Close()

            form_main.TextBox5.Text = ""

            Dim sw As New StreamWriter(output, False)
            sw.Write("ID")
            If form_config_info.CheckBox2.Checked Then
                sw.Write(",name,state")
            End If
            If form_config_info.CheckBox3.Checked Then
                sw.Write(",len")
            End If
            If form_config_info.CheckBox4.Checked Then
                sw.Write(",len_wo_gap")
            End If
            If form_config_info.CheckBox5.Checked Then
                sw.Write(",seq")
            End If
            sw.Write(new_line_char)
            For i = 1 To UBound(name)
                info_text = "Analyzing sequence " + i.ToString
                PB_value = 100*i/UBound(fasta_seq)

                sw.Write(i.ToString)
                If form_config_info.CheckBox2.Checked Then
                    Dim current_state = ""
                    Dim current_name = ""
                    If form_config_info.CheckBox7.Checked Then
                        name(i) = name(i).Replace(form_config_info.TextBox3.Text, form_config_info.TextBox2.Text)
                    End If
                    If form_config_info.CheckBox8.Checked Then
                        If form_config_info.TextBox5.Text <> "" Then
                            If _
                                form_config_info.NumericUpDown1.Value <=
                                UBound(name(i).Split(form_config_info.TextBox5.Text)) Then
                                current_name =
                                    name(i).Split(form_config_info.TextBox5.Text)(form_config_info.NumericUpDown1.Value)
                            End If
                        End If
                    End If
                    If form_config_info.CheckBox6.Checked Then
                        If form_config_info.TextBox1.Text <> "" Then
                            If _
                                form_config_info.NumericUpDown2.Value <=
                                UBound(name(i).Split(form_config_info.TextBox1.Text)) Then
                                current_state =
                                    name(i).Split(form_config_info.TextBox1.Text)(form_config_info.NumericUpDown2.Value)
                            End If
                        End If
                    End If
                    sw.Write("," + current_name + "," + current_state)
                End If
                If form_config_info.CheckBox3.Checked Then
                    sw.Write("," + fasta_seq(i).Length.ToString)
                End If
                If form_config_info.CheckBox4.Checked Then
                    sw.Write("," + fasta_seq(i).Replace("-", "").Length.ToString)
                End If
                If form_config_info.CheckBox5.Checked Then
                    sw.Write("," + fasta_seq(i))
                End If
                sw.Write(new_line_char)
            Next
            sw.Close()
            'form_main.TextBox5.Text = tmp_text
            show_info(count.ToString + " sequences in total")
            show_info("Processing completed.")
            Return True
        Catch ex As Exception
            info_text = ""
            PB_value = 0
            show_info(ex.ToString)
            show_info("File processing terminated.")
            Return False
        End Try
    End Function
End Module
