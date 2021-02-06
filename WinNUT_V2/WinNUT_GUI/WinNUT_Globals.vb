Module WinNUT_Globals
    Public LongProgramName As String
    Public ProgramName As String
    Public ProgramVersion As String
    Public GitHubURL As String
    Public Copyright As String
    Public Directory_AppData As String
    Public IsConnected As Boolean
    Public LogFile As String
    Public AppIcon As Dictionary(Of Integer, Icon)
    Public StrLog As New List(Of String)
    Public LogFilePath As String
    Public Sub Init_Globals()
        LongProgramName = My.Application.Info.Description
        ProgramName = My.Application.Info.ProductName
        ProgramVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString
        GitHubURL = My.Application.Info.Trademark
        Copyright = My.Application.Info.Copyright
        Directory_AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\WinNUT-Client"
        LogFilePath = Directory_AppData & "\WinNUT-CLient.log"
        If Not System.IO.Directory.Exists(Directory_AppData) Then
            My.Computer.FileSystem.CreateDirectory(Directory_AppData)
        End If
        IsConnected = False

        'Add Main Gui's Strings
        StrLog.Insert(AppResxStr.STR_MAIN_OLDINI_RENAMED, My.Resources.Frm_Main_Str_01)
        StrLog.Insert(AppResxStr.STR_MAIN_OLDINI, My.Resources.Frm_Main_Str_02)
        StrLog.Insert(AppResxStr.STR_MAIN_RECONNECT, My.Resources.Frm_Main_Str_03)
        StrLog.Insert(AppResxStr.STR_MAIN_RETRY, My.Resources.Frm_Main_Str_04)
        StrLog.Insert(AppResxStr.STR_MAIN_NOTCONN, My.Resources.Frm_Main_Str_05)
        StrLog.Insert(AppResxStr.STR_MAIN_CONN, My.Resources.Frm_Main_Str_06)
        StrLog.Insert(AppResxStr.STR_MAIN_OL, My.Resources.Frm_Main_Str_07)
        StrLog.Insert(AppResxStr.STR_MAIN_OB, My.Resources.Frm_Main_Str_08)
        StrLog.Insert(AppResxStr.STR_MAIN_LOWBAT, My.Resources.Frm_Main_Str_09)
        StrLog.Insert(AppResxStr.STR_MAIN_BATOK, My.Resources.Frm_Main_Str_10)
        StrLog.Insert(AppResxStr.STR_MAIN_UNKNOWN_UPS, My.Resources.Frm_Main_Str_11)
        StrLog.Insert(AppResxStr.STR_MAIN_LOSTCONNECT, My.Resources.Frm_Main_Str_12)
        StrLog.Insert(AppResxStr.STR_MAIN_INVALIDLOGIN, My.Resources.Frm_Main_Str_13)

        'Add Update Gui's Strings
        StrLog.Insert(AppResxStr.STR_UP_AVAIL, My.Resources.Frm_Update_Str_01)
        StrLog.Insert(AppResxStr.STR_UP_SHOW, My.Resources.Frm_Update_Str_02)
        StrLog.Insert(AppResxStr.STR_UP_HIDE, My.Resources.Frm_Update_Str_03)
        StrLog.Insert(AppResxStr.STR_UP_UPMSG, My.Resources.Frm_Update_Str_04)
        StrLog.Insert(AppResxStr.STR_UP_DOWNFROM, My.Resources.Frm_Update_Str_05)

        'Add Shutdown Gui's Strings
        StrLog.Insert(AppResxStr.STR_SHUT_STAT, My.Resources.Frm_Shutdown_Str_01)

        'Add App Event's Strings 
        StrLog.Insert(AppResxStr.STR_APP_SHUT, My.Resources.App_Event_Str_01)

        'Add Log's Strings
        StrLog.Insert(AppResxStr.STR_LOG_PREFS, My.Resources.Log_Str_01)
        StrLog.Insert(AppResxStr.STR_LOG_CONNECTED, My.Resources.Log_Str_02)
        StrLog.Insert(AppResxStr.STR_LOG_CON_FAILED, My.Resources.Log_Str_03)
        StrLog.Insert(AppResxStr.STR_LOG_CON_RETRY, My.Resources.Log_Str_04)
        StrLog.Insert(AppResxStr.STR_LOG_LOGOFF, My.Resources.Log_Str_05)
        StrLog.Insert(AppResxStr.STR_LOG_NEW_RETRY, My.Resources.Log_Str_06)
        StrLog.Insert(AppResxStr.STR_LOG_STOP_RETRY, My.Resources.Log_Str_07)
        StrLog.Insert(AppResxStr.STR_LOG_SHUT_START, My.Resources.Log_Str_08)
        StrLog.Insert(AppResxStr.STR_LOG_SHUT_STOP, My.Resources.Log_Str_09)
        StrLog.Insert(AppResxStr.STR_LOG_NO_UPDATE, My.Resources.Log_Str_10)
        StrLog.Insert(AppResxStr.STR_LOG_UPDATE, My.Resources.Log_Str_11)
        StrLog.Insert(AppResxStr.STR_LOG_NUT_FSD, My.Resources.Log_Str_12)
    End Sub

End Module
