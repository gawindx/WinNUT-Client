' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Module WinNUT_Globals
    Public LongProgramName As String
    Public ProgramName As String
    Public ProgramVersion As String
    Public ShortProgramVersion As String
    Public GitHubURL As String
    Public Copyright As String
    Public Directory_AppData As String
    Public IsConnected As Boolean
    Public LogFile As String
    Public AppIcon As Dictionary(Of Integer, System.Drawing.Icon)
    Public StrLog As New List(Of String)
    Public LogFilePath As String
    Public Sub Init_Globals()
        LongProgramName = My.Application.Info.Description
        ProgramName = My.Application.Info.ProductName
        ProgramVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString
        ShortProgramVersion = ProgramVersion.Substring(0, ProgramVersion.IndexOf(".", ProgramVersion.IndexOf(".") + 1))
        GitHubURL = My.Application.Info.Trademark
        Copyright = My.Application.Info.Copyright
        Directory_AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\WinNUT-Client"

        If Not System.IO.Directory.Exists(Directory_AppData) Then
            My.Computer.FileSystem.CreateDirectory(Directory_AppData)
        End If
        IsConnected = False

        'Add Main Gui's Strings
        'StrLog.Insert(AppResxStr.STR_MAIN_OLDINI_RENAMED, Resources.Frm_Main_Str_01)
        'StrLog.Insert(AppResxStr.STR_MAIN_OLDINI, Resources.Frm_Main_Str_02)
        'StrLog.Insert(AppResxStr.STR_MAIN_RECONNECT, Resources.Frm_Main_Str_03)
        'StrLog.Insert(AppResxStr.STR_MAIN_RETRY, Resources.Frm_Main_Str_04)
        'StrLog.Insert(AppResxStr.STR_MAIN_NOTCONN, Resources.Frm_Main_Str_05)
        'StrLog.Insert(AppResxStr.STR_MAIN_CONN, Resources.Frm_Main_Str_06)
        'StrLog.Insert(AppResxStr.STR_MAIN_OL, Resources.Frm_Main_Str_07)
        'StrLog.Insert(AppResxStr.STR_MAIN_OB, Resources.Frm_Main_Str_08)
        'StrLog.Insert(AppResxStr.STR_MAIN_LOWBAT, Resources.Frm_Main_Str_09)
        'StrLog.Insert(AppResxStr.STR_MAIN_BATOK, Resources.Frm_Main_Str_10)
        'StrLog.Insert(AppResxStr.STR_MAIN_UNKNOWN_UPS, Resources.Frm_Main_Str_11)
        'StrLog.Insert(AppResxStr.STR_MAIN_LOSTCONNECT, Resources.Frm_Main_Str_12)
        'StrLog.Insert(AppResxStr.STR_MAIN_INVALIDLOGIN, Resources.Frm_Main_Str_13)
        'StrLog.Insert(AppResxStr.STR_MAIN_EXITSLEEP, Resources.Frm_Main_Str_14)
        'StrLog.Insert(AppResxStr.STR_MAIN_GOTOSLEEP, Resources.Frm_Main_Str_15)

        'Add Update Gui's Strings
        'StrLog.Insert(AppResxStr.STR_UP_AVAIL, Resources.Frm_Update_Str_01)
        'StrLog.Insert(AppResxStr.STR_UP_SHOW, Resources.Frm_Update_Str_02)
        'StrLog.Insert(AppResxStr.STR_UP_HIDE, Resources.Frm_Update_Str_03)
        'StrLog.Insert(AppResxStr.STR_UP_UPMSG, Resources.Frm_Update_Str_04)
        'StrLog.Insert(AppResxStr.STR_UP_DOWNFROM, Resources.Frm_Update_Str_05)

        'Add Shutdown Gui's Strings
        'StrLog.Insert(AppResxStr.STR_SHUT_STAT, Resources.Frm_Shutdown_Str_01)

        'Add App Event's Strings 
        'StrLog.Insert(AppResxStr.STR_APP_SHUT, Resources.App_Event_Str_01)

        'Add Log's Strings
        'StrLog.Insert(AppResxStr.STR_LOG_PREFS, Resources.Log_Str_01)
        'StrLog.Insert(AppResxStr.STR_LOG_CONNECTED, Resources.Log_Str_02)
        'StrLog.Insert(AppResxStr.STR_LOG_CON_FAILED, Resources.Log_Str_03)
        'StrLog.Insert(AppResxStr.STR_LOG_CON_RETRY, Resources.Log_Str_04)
        'StrLog.Insert(AppResxStr.STR_LOG_LOGOFF, Resources.Log_Str_05)
        'StrLog.Insert(AppResxStr.STR_LOG_NEW_RETRY, Resources.Log_Str_06)
        'StrLog.Insert(AppResxStr.STR_LOG_STOP_RETRY, Resources.Log_Str_07)
        'StrLog.Insert(AppResxStr.STR_LOG_SHUT_START, Resources.Log_Str_08)
        'StrLog.Insert(AppResxStr.STR_LOG_SHUT_STOP, Resources.Log_Str_09)
        'StrLog.Insert(AppResxStr.STR_LOG_NO_UPDATE, Resources.Log_Str_10)
        'StrLog.Insert(AppResxStr.STR_LOG_UPDATE, Resources.Log_Str_11)
        'StrLog.Insert(AppResxStr.STR_LOG_NUT_FSD, Resources.Log_Str_12)
    End Sub

End Module
