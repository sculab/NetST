Imports System.Globalization

Module Module_Var
#Const TargetOS = "win32"
#If TargetOS = "linux" Then
    Public TargetOS As String = "linux"
#ElseIf TargetOS = "macos" Then
    Public TargetOS As String = "macos"
#ElseIf TargetOS = "win32" Then
    Public TargetOS As String = "win32"
#End If
    Public version As String = "20250713"
    Public settings As Dictionary(Of String, String)
    Public currentDirectory As String
    Public dtView As New DataView
    Public ci As CultureInfo = New CultureInfo("en-us")
    Public path_char As String
    Public root_path As String
    Public lib_path As String
    Public Dec_Sym As String
    Public timer_id As Integer = 0
    Public waiting As Boolean = False
    Public current_file As String
    Public form_config_stand As New Config_Stand
    Public form_config_clean As New Config_Clean
    Public form_config_info As New Config_Info
    Public form_main As New Mainform
    Public PB_value As Integer = 0
    Public info_text As String = ""
    Public language As String = "EN"
    Public fasta_seq() As String
    Public add_data As Boolean = False
    Public data_loaded As Boolean = False
    Public cpu_info As String
End Module
