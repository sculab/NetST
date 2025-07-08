Imports System.IO
Imports System.Text
Imports System.Web

Module Report
    Public Sub GenerateHtmlReport(csvFilePath As String, htmlFilePath As String, timestamp As String, formattedTime As String, success As Boolean)
        Try
            ' 检查CSV文件是否存在
            If Not File.Exists(csvFilePath) Then
                Return
            End If

            ' 读取CSV文件
            Dim csvLines As String() = File.ReadAllLines(csvFilePath)
            If csvLines.Length <= 1 Then ' 只有标题行或空文件
                Return
            End If

            ' 从CSV文件头中提取网络类型
            Dim analysisType As String = ExtractNetworkTypeFromCsv(csvLines)

            ' 读取HTML模板
            Dim templatePath As String = Path.Combine(Application.StartupPath, "history\ReportTemplate.html")
            If Not File.Exists(templatePath) Then
                ' 如果模板不存在，创建默认模板
                CreateDefaultTemplate(templatePath)
            End If

            Dim htmlTemplate As String = File.ReadAllText(templatePath, Encoding.UTF8)

            ' 解析CSV并分类文件
            Dim fileCategories = ParseCsvAndCategorizeFiles(csvLines)

            ' 替换基本信息占位符
            htmlTemplate = ReplaceBasicPlaceholders(htmlTemplate, analysisType, formattedTime, timestamp, success)

            ' 生成动态内容
            htmlTemplate = htmlTemplate.Replace("{{TAB_BUTTONS}}", GenerateTabButtons(fileCategories))
            htmlTemplate = htmlTemplate.Replace("{{BASIC_FILES_TABLE}}", GenerateBasicFilesTable(fileCategories))
            htmlTemplate = htmlTemplate.Replace("{{ANALYSIS_TABS}}", GenerateAnalysisTabs(fileCategories))
            htmlTemplate = htmlTemplate.Replace("{{SUMMARY_CONTENT}}", GenerateSummaryContent(success, fileCategories))
            htmlTemplate = htmlTemplate.Replace("{{CSV_PATH}}", csvFilePath)

            ' 写入最终HTML文件
            File.WriteAllText(htmlFilePath, htmlTemplate, Encoding.UTF8)

        Catch ex As Exception
            Console.WriteLine("Error generating HTML report: " + ex.Message)
        End Try
    End Sub

    ' 新增函数：从CSV文件头中提取网络类型
    Private Function ExtractNetworkTypeFromCsv(csvLines As String()) As String
        ' 从CSV文件头中提取网络类型
        For Each line In csvLines
            If line.StartsWith("# Network Type:") Then
                Dim parts As String() = line.Split(":"c)
                If parts.Length >= 2 Then
                    Dim networkType As String = parts(1).Trim()
                    Return networkType.ToUpper() ' 转换为大写以保持一致性
                End If
            End If
        Next

        ' 如果没有找到网络类型，返回默认值
        Return "UNKNOWN"
    End Function

    Private Sub CreateDefaultTemplate(templatePath As String)
        ' 创建默认HTML模板文件
        Dim defaultTemplate =
                "<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>NetST Analysis Report</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; color: #333; line-height: 1.6; }
        h1 { color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px; }
        h2 { color: #2980b9; margin-top: 30px; }
        .info-box { background-color: #f8f9fa; border-left: 4px solid #3498db; padding: 15px; margin: 20px 0; }
        table { border-collapse: collapse; width: 100%; margin: 20px 0; }
        th, td { text-align: left; padding: 12px; }
        th { background-color: #3498db; color: white; }
        tr:nth-child(even) { background-color: #f2f2f2; }
        tr:hover { background-color: #e9f7fe; }
        .success { color: green; }
        .failed { color: red; }
        .section { margin-top: 20px; border-top: 1px solid #eee; padding-top: 10px; }
        .file-link { color: #3498db; text-decoration: none; }
        .file-link:hover { text-decoration: underline; }
        .summary { margin-top: 30px; padding: 15px; background-color: #e8f6fd; border-radius: 5px; }
        .tab-container { margin-top: 20px; }
        .tab-buttons { display: flex; border-bottom: 1px solid #ddd; }
        .tab-button { padding: 10px 15px; cursor: pointer; background-color: #f1f1f1; margin-right: 2px; border: 1px solid #ddd; border-bottom: none; border-radius: 5px 5px 0 0; }
        .tab-button:hover { background-color: #ddd; }
        .tab-button.active { background-color: #3498db; color: white; }
        .tab-content { display: none; padding: 15px; border: 1px solid #ddd; border-top: none; }
        .tab-content.active { display: block; }
    </style>
</head>
<body>
    <h1>Haplotype Network Analysis Report</h1>
    <div class='info-box'>
        <p><strong>Analysis Type:</strong> {{ANALYSIS_TYPE}} Haplotype Network</p>
        <p><strong>Date and Time:</strong> {{FORMATTED_TIME}}</p>
        <p><strong>Timestamp:</strong> {{TIMESTAMP}}</p>
        <p><strong>Status:</strong> <span class='{{STATUS_CLASS}}'>{{STATUS_TEXT}}</span></p>
    </div>
    <div class='tab-container'>
        <div class='tab-buttons'>
            <div class='tab-button active' onclick='openTab(event, ""basicFiles"")'>Basic Files</div>
            {{TAB_BUTTONS}}
        </div>
        <div id='basicFiles' class='tab-content active'>
            <h2>Network Construction Files</h2>
            <table>
                <tr>
                    <th>File Type</th>
                    <th>File Path</th>
                    <th>Status</th>
                    <th>Description</th>
                </tr>
                {{BASIC_FILES_TABLE}}
            </table>
        </div>
        {{ANALYSIS_TABS}}
    </div>
    <div class='summary'>
        <h2>Analysis Summary</h2>
        {{SUMMARY_CONTENT}}
    </div>
    <p>A CSV file with all generated files is available <a href='file:///{{CSV_PATH}}' class='file-link'>here</a>.</p>
    <script>
        function openTab(evt, tabName) {
            var tabContents = document.getElementsByClassName('tab-content');
            for (var i = 0; i < tabContents.length; i++) {
                tabContents[i].className = tabContents[i].className.replace(' active', '');
            }
            var tabButtons = document.getElementsByClassName('tab-button');
            for (var i = 0; i < tabButtons.length; i++) {
                tabButtons[i].className = tabButtons[i].className.replace(' active', '');
            }
            document.getElementById(tabName).className += ' active';
            evt.currentTarget.className += ' active';
        }
        document.addEventListener('DOMContentLoaded', function() {
            var fileLinks = document.querySelectorAll('a.file-link');
            fileLinks.forEach(function(link) {
                link.addEventListener('click', function(e) {
                    if (this.classList.contains('network-link')) {
                        return;
                    }
                    var filePath = this.getAttribute('href').replace('file:///', '');
                    filePath = decodeURIComponent(filePath);
                    if (window.chrome && window.chrome.webview) {
                        window.chrome.webview.postMessage({
                            type: 'fileClick',
                            path: filePath
                        });
                        e.preventDefault();
                    }
                });
            });
        });
    </script>
</body>
</html>"

        File.WriteAllText(templatePath, defaultTemplate, Encoding.UTF8)
    End Sub

    Private Function ParseCsvAndCategorizeFiles(csvLines As String()) As Dictionary(Of String, List(Of String()))
        Dim categories As New Dictionary(Of String, List(Of String()))

        ' 初始化各类别
        categories("original") = New List(Of String())
        categories("alignment") = New List(Of String())
        categories("haplotype") = New List(Of String())
        categories("network") = New List(Of String())
        categories("visualization") = New List(Of String())
        categories("topology") = New List(Of String())
        categories("modularity") = New List(Of String())
        categories("community") = New List(Of String())
        categories("sequence") = New List(Of String())
        categories("population") = New List(Of String())
        categories("other") = New List(Of String())

        ' 从CSV解析文件信息并分类
        For i = 1 To csvLines.Length - 1
            ' 跳过注释行
            If csvLines(i).StartsWith("#") Then
                Continue For
            End If
            Dim parts As String() = csvLines(i).Split(","c)
            If parts.Length >= 4 Then
                Dim fileType As String = parts(0)
                Dim filePath As String = parts(1)
                Dim status As String = parts(2)
                Dim description As String = If(parts.Length > 3, parts(3), "")

                Dim fileInfo As String() = {fileType, filePath, status, description}

                ' 根据文件类型进行分类
                If fileType.StartsWith("Ori_seqs") Then
                    categories("original").Add(fileInfo)
                ElseIf fileType.StartsWith("Aligned_seqs") Or fileType.StartsWith("Encoding_conversion") Then
                    categories("alignment").Add(fileInfo)
                ElseIf fileType.StartsWith("Hap_seq") Or fileType.StartsWith("Seq_with_traits") Or
                       fileType.StartsWith("Seq_metadata") Or fileType.StartsWith("Hap_trait") Or
                       fileType.StartsWith("Seq_trait") Or fileType.StartsWith("Seq2Hap") Then
                    categories("haplotype").Add(fileInfo)
                ElseIf fileType.StartsWith("HapNet_gml") Or fileType.StartsWith("HapNet_json") Then
                    categories("network").Add(fileInfo)
                ElseIf fileType.StartsWith("HapNet_hapconf") Or fileType.StartsWith("HapNet_groupconf") Or
                       fileType.StartsWith("HapNet_js") Or fileType.StartsWith("HapNet_html") Then
                    categories("visualization").Add(fileInfo)
                ElseIf fileType.StartsWith("TopologyAnalysis") Then
                    categories("topology").Add(fileInfo)
                ElseIf fileType.StartsWith("ModularityAnalysis") Then
                    categories("modularity").Add(fileInfo)
                ElseIf fileType.StartsWith("CommunityAnalysis") Then
                    categories("community").Add(fileInfo)
                ElseIf fileType.StartsWith("SequenceAnalysis") Then
                    categories("sequence").Add(fileInfo)
                ElseIf fileType.StartsWith("PopulationStatistics") Then
                    categories("population").Add(fileInfo)
                Else
                    categories("other").Add(fileInfo)
                End If
            End If
        Next

        Return categories
    End Function

    Private Function ReplaceBasicPlaceholders(template As String, analysisType As String, formattedTime As String,
                                              timestamp As String, success As Boolean) As String
        template = template.Replace("{{ANALYSIS_TYPE}}", analysisType)
        template = template.Replace("{{FORMATTED_TIME}}", formattedTime)
        template = template.Replace("{{TIMESTAMP}}", timestamp)
        template = template.Replace("{{STATUS_CLASS}}", If(success, "success", "failed"))
        template = template.Replace("{{STATUS_TEXT}}", If(success, "Success", "Failed"))
        Return template
    End Function

    Private Function GenerateTabButtons(categories As Dictionary(Of String, List(Of String()))) As String
        Dim buttons As New StringBuilder()

        If categories("topology").Count > 0 Then
            buttons.AppendLine(
                "            <div class='tab-button' onclick='openTab(event, ""topologyAnalysis"")'>Topology Analysis</div>")
        End If
        If categories("modularity").Count > 0 Then
            buttons.AppendLine(
                "            <div class='tab-button' onclick='openTab(event, ""modularityAnalysis"")'>Modularity Analysis</div>")
        End If
        If categories("community").Count > 0 Then
            buttons.AppendLine(
                "            <div class='tab-button' onclick='openTab(event, ""communityAnalysis"")'>Community Analysis</div>")
        End If
        If categories("sequence").Count > 0 Then
            buttons.AppendLine(
                "            <div class='tab-button' onclick='openTab(event, ""sequenceAnalysis"")'>Sequence Analysis</div>")
        End If
        If categories("population").Count > 0 Then
            buttons.AppendLine(
                "            <div class='tab-button' onclick='openTab(event, ""populationStats"")'>Population Statistics</div>")
        End If

        Return buttons.ToString()
    End Function

    Private Function GenerateBasicFilesTable(categories As Dictionary(Of String, List(Of String()))) As String
        Dim table As New StringBuilder()

        If categories("original").Count > 0 Then
            table.AppendLine(
                "                <tr><td colspan='4' style='background-color: #d6eaf8;'><strong>Original Sequence Files</strong></td></tr>")
            table.Append(RenderFileTableRows(categories("original")))
        End If

        If categories("alignment").Count > 0 Then
            table.AppendLine(
                "                <tr><td colspan='4' style='background-color: #d6eaf8;'><strong>Alignment Files</strong></td></tr>")
            table.Append(RenderFileTableRows(categories("alignment")))
        End If

        If categories("haplotype").Count > 0 Then
            table.AppendLine(
                "                <tr><td colspan='4' style='background-color: #d6eaf8;'><strong>Haplotype Data Files</strong></td></tr>")
            table.Append(RenderFileTableRows(categories("haplotype")))
        End If

        If categories("network").Count > 0 Then
            table.AppendLine(
                "                <tr><td colspan='4' style='background-color: #d6eaf8;'><strong>Network Files</strong></td></tr>")
            table.Append(RenderFileTableRows(categories("network")))
        End If

        If categories("visualization").Count > 0 Then
            table.AppendLine(
                "                <tr><td colspan='4' style='background-color: #d6eaf8;'><strong>Visualization Files</strong></td></tr>")
            table.Append(RenderFileTableRows(categories("visualization")))
        End If

        If categories("other").Count > 0 Then
            table.AppendLine(
                "                <tr><td colspan='4' style='background-color: #d6eaf8;'><strong>Other Files</strong></td></tr>")
            table.Append(RenderFileTableRows(categories("other")))
        End If

        Return table.ToString()
    End Function

    Private Function GenerateAnalysisTabs(categories As Dictionary(Of String, List(Of String()))) As String
        Dim tabs As New StringBuilder()

        ' 拓扑分析选项卡
        If categories("topology").Count > 0 Then
            tabs.AppendLine("        <div id='topologyAnalysis' class='tab-content'>")
            tabs.AppendLine("            <h2>Topology Analysis Results</h2>")
            tabs.AppendLine("            <div class='info-box'>")
            tabs.AppendLine(
                "                <p>Topology analysis visualizes the structure of the haplotype network, showing how haplotypes are connected and their relationships.</p>")
            tabs.AppendLine("            </div>")
            tabs.AppendLine("            <table>")
            tabs.AppendLine(
                "                <tr><th>File Type</th><th>File Path</th><th>Status</th><th>Description</th></tr>")
            tabs.Append(RenderFileTableRows(categories("topology")))
            tabs.AppendLine("            </table>")
            tabs.Append(GenerateVisualizationPreview(categories("topology")))
            tabs.AppendLine("        </div>")
        End If

        ' 模块度分析选项卡
        If categories("modularity").Count > 0 Then
            tabs.AppendLine("        <div id='modularityAnalysis' class='tab-content'>")
            tabs.AppendLine("            <h2>Modularity Analysis Results</h2>")
            tabs.AppendLine("            <div class='info-box'>")
            tabs.AppendLine(
                "                <p>Modularity analysis examines how the network can be divided into communities, showing the optimal grouping of haplotypes based on their connections.</p>")
            tabs.AppendLine("            </div>")
            tabs.AppendLine("            <table>")
            tabs.AppendLine(
                "                <tr><th>File Type</th><th>File Path</th><th>Status</th><th>Description</th></tr>")
            tabs.Append(RenderFileTableRows(categories("modularity")))
            tabs.AppendLine("            </table>")
            tabs.Append(GenerateMultiVisualizationPreview(categories("modularity")))
            tabs.AppendLine("        </div>")
        End If

        ' 社区分析选项卡
        If categories("community").Count > 0 Then
            tabs.AppendLine("        <div id='communityAnalysis' class='tab-content'>")
            tabs.AppendLine("            <h2>Community Analysis Results</h2>")
            tabs.AppendLine("            <div class='info-box'>")
            tabs.AppendLine(
                "                <p>Community analysis divides the network into specific numbers of communities (k-values), showing how haplotypes can be grouped together.</p>")
            tabs.AppendLine("            </div>")
            tabs.AppendLine("            <table>")
            tabs.AppendLine(
                "                <tr><th>File Type</th><th>File Path</th><th>Status</th><th>Description</th></tr>")
            tabs.Append(RenderFileTableRows(categories("community")))
            tabs.AppendLine("            </table>")
            tabs.Append(GenerateMultiVisualizationPreview(categories("community")))
            tabs.AppendLine("        </div>")
        End If

        ' 序列分析选项卡
        If categories("sequence").Count > 0 Then
            tabs.AppendLine("        <div id='sequenceAnalysis' class='tab-content'>")
            tabs.AppendLine("            <h2>Sequence Analysis Results</h2>")
            tabs.AppendLine("            <div class='info-box'>")
            tabs.AppendLine(
                "                <p>Sequence analysis examines the genetic distance between haplotypes and conservation patterns in the sequence alignment.</p>")
            tabs.AppendLine("            </div>")
            tabs.AppendLine("            <table>")
            tabs.AppendLine(
                "                <tr><th>File Type</th><th>File Path</th><th>Status</th><th>Description</th></tr>")
            tabs.Append(RenderFileTableRows(categories("sequence")))
            tabs.AppendLine("            </table>")
            tabs.Append(GenerateMultiVisualizationPreview(categories("sequence")))
            tabs.AppendLine("        </div>")
        End If

        ' 群体统计选项卡
        If categories("population").Count > 0 Then
            tabs.AppendLine("        <div id='populationStats' class='tab-content'>")
            tabs.AppendLine("            <h2>Population Statistics Results</h2>")
            tabs.AppendLine("            <div class='info-box'>")
            tabs.AppendLine(
                "                <p>Population statistics provide information about genetic diversity, nucleotide diversity, and other population genetic parameters.</p>")
            tabs.AppendLine("            </div>")
            tabs.AppendLine("            <table>")
            tabs.AppendLine(
                "                <tr><th>File Type</th><th>File Path</th><th>Status</th><th>Description</th></tr>")
            tabs.Append(RenderFileTableRows(categories("population")))
            tabs.AppendLine("            </table>")
            tabs.Append(GenerateStatsPreview(categories("population")))
            tabs.AppendLine("        </div>")
        End If

        Return tabs.ToString()
    End Function

    Private Function RenderFileTableRows(files As List(Of String())) As String
        Dim rows As New StringBuilder()

        For Each fileInfo In files
            Dim fileType = fileInfo(0)
            Dim filePath = fileInfo(1)
            Dim status = fileInfo(2)
            Dim description = If(fileInfo.Length > 3, fileInfo(3), "")

            rows.AppendLine("                <tr>")
            rows.AppendLine("                    <td>" + fileType + "</td>")
            rows.AppendLine(
                "                    <td><a href='file:///" + filePath + "' class='file-link'>" + filePath + "</a></td>")
            rows.AppendLine("                    <td>" + status + "</td>")
            rows.AppendLine("                    <td>" + description + "</td>")
            rows.AppendLine("                </tr>")
        Next

        Return rows.ToString()
    End Function

    Private Function GenerateVisualizationPreview(files As List(Of String())) As String
        Dim preview As New StringBuilder()

        ' 寻找主要可视化文件
        For Each fileInfo In files
            If fileInfo(2) = "Success" AndAlso fileInfo(1).EndsWith(".svg") Then
                preview.AppendLine("            <h3>Visualization Preview</h3>")
                preview.AppendLine("            <div style='text-align: center; margin: 20px 0;'>")
                preview.AppendLine(
                    "                <object data='file:///" + fileInfo(1) +
                    "' type='image/svg+xml' style='width: 80%; max-width: 800px;'></object>")
                preview.AppendLine("            </div>")
                Exit For
            End If
        Next

        Return preview.ToString()
    End Function

    Private Function GenerateMultiVisualizationPreview(files As List(Of String())) As String
        Dim preview As New StringBuilder()
        Dim hasVisualizations = False

        For Each fileInfo In files
            If fileInfo(2) = "Success" AndAlso fileInfo(1).EndsWith(".svg") Then
                If Not hasVisualizations Then
                    preview.AppendLine("            <h3>Visualization Previews</h3>")
                    preview.AppendLine(
                        "            <div style='display: flex; flex-wrap: wrap; justify-content: center; gap: 20px; margin: 20px 0;'>")
                    hasVisualizations = True
                End If

                preview.AppendLine("                <div style='flex: 1; min-width: 300px; max-width: 500px;'>")
                preview.AppendLine("                    <h4>" + fileInfo(3) + "</h4>")
                preview.AppendLine(
                    "                    <object data='file:///" + fileInfo(1) +
                    "' type='image/svg+xml' style='width: 100%;'></object>")
                preview.AppendLine("                </div>")
            End If
        Next

        If hasVisualizations Then
            preview.AppendLine("            </div>")
        End If

        Return preview.ToString()
    End Function

    Private Function GenerateStatsPreview(files As List(Of String())) As String
        Dim preview As New StringBuilder()

        ' 寻找统计文件并嵌入内容
        For Each fileInfo In files
            If fileInfo(2) = "Success" Then
                Try
                    If File.Exists(fileInfo(1)) Then
                        Dim statsContent As String = File.ReadAllText(fileInfo(1))
                        preview.AppendLine("            <h3>Statistics Summary</h3>")
                        preview.AppendLine(
                            "            <pre style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; overflow-x: auto;'>")
                        preview.AppendLine(HttpUtility.HtmlEncode(statsContent))
                        preview.AppendLine("            </pre>")
                        Exit For
                    End If
                Catch ex As Exception
                    ' 如果读取失败，忽略嵌入
                End Try
            End If
        Next

        Return preview.ToString()
    End Function

    Private Function GenerateSummaryContent(success As Boolean, categories As Dictionary(Of String, List(Of String()))) _
        As String
        Dim summary As New StringBuilder()

        If success Then
            summary.AppendLine("        <p class='success'><strong>Status:</strong> Analysis completed successfully</p>")

            ' 找到可视化HTML文件
            Dim visualizationHtml = ""
            For Each fileInfo In categories("visualization")
                If fileInfo(0) = "HapNet_html" And fileInfo(2) = "Success" Then
                    visualizationHtml = fileInfo(1)
                    Exit For
                End If
            Next

            If visualizationHtml <> "" Then
                summary.AppendLine(
                    "        <p>The haplotype network analysis has been completed. You can <a href='file:///" +
                    visualizationHtml +
                    "' class='file-link network-link'>click here</a> to view the network visualization.</p>")
            Else
                summary.AppendLine("        <p>The haplotype network analysis has been completed.</p>")
            End If

            ' 添加其他分析摘要
            Dim analysisCompleted As New List(Of String)
            If categories("topology").Count > 0 Then analysisCompleted.Add("Topology Analysis")
            If categories("modularity").Count > 0 Then analysisCompleted.Add("Modularity Analysis")
            If categories("community").Count > 0 Then analysisCompleted.Add("Community Analysis")
            If categories("sequence").Count > 0 Then analysisCompleted.Add("Sequence Analysis")
            If categories("population").Count > 0 Then analysisCompleted.Add("Population Statistics")

            If analysisCompleted.Count > 0 Then
                summary.AppendLine(
                    "        <p><strong>Additional analyses completed:</strong> " + String.Join(", ", analysisCompleted) +
                    "</p>")
            End If
        Else
            summary.AppendLine("        <p class='failed'><strong>Status:</strong> Analysis failed</p>")
            summary.AppendLine(
                "        <p>There was an error during the haplotype network analysis. Please check the file status in the table above for details.</p>")
        End If

        Return summary.ToString()
    End Function
End Module
