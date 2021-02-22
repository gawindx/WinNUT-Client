' WinNUT is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Enum LogLvl
    LOG_NOTICE
    LOG_WARNING
    LOG_ERROR
    LOG_DEBUG
End Enum
Public Enum AppIconIdx
    IDX_BATT_0 = 1
    IDX_BATT_25 = 2
    IDX_BATT_50 = 4
    IDX_BATT_75 = 8
    IDX_BATT_100 = 16
    IDX_OL = 32
    WIN_DARK = 64
    IDX_ICO_OFFLINE = 128
    IDX_ICO_RETRY = 256
    IDX_ICO_VIEWLOG = 2001
    IDX_ICO_DELETELOG = 2002
    IDX_OFFSET = 1024
End Enum

Public Enum AppResxStr
    STR_MAIN_OLDINI_RENAMED
    STR_MAIN_OLDINI
    STR_MAIN_RECONNECT
    STR_MAIN_RETRY
    STR_MAIN_NOTCONN
    STR_MAIN_CONN
    STR_MAIN_OL
    STR_MAIN_OB
    STR_MAIN_LOWBAT
    STR_MAIN_BATOK
    STR_MAIN_UNKNOWN_UPS
    STR_MAIN_LOSTCONNECT
    STR_MAIN_INVALIDLOGIN
    STR_MAIN_EXITSLEEP
    STR_MAIN_GOTOSLEEP
    STR_UP_AVAIL
    STR_UP_SHOW
    STR_UP_HIDE
    STR_UP_UPMSG
    STR_UP_DOWNFROM
    STR_SHUT_STAT
    STR_APP_SHUT
    STR_LOG_PREFS
    STR_LOG_CONNECTED
    STR_LOG_CON_FAILED
    STR_LOG_CON_RETRY
    STR_LOG_LOGOFF
    STR_LOG_NEW_RETRY
    STR_LOG_STOP_RETRY
    STR_LOG_SHUT_START
    STR_LOG_SHUT_STOP
    STR_LOG_NO_UPDATE
    STR_LOG_UPDATE
    STR_LOG_NUT_FSD
End Enum
Public Class WinNUT
    Public Shared WithEvents LogFile As New Logger(False, 0)
    Public LogLvls As LogLvl
    Public WithEvents UPS_Network As New UPS_Network(LogFile)
    Public WithEvents FrmBuild As Update_Gui
    Public ToastPopup As New ToastPopup
    Public UPS_Mfr As String
    Public UPS_Model As String
    Public UPS_Serial As String
    Public UPS_Firmware As String
    Public UPS_BattCh As Double
    Public UPS_BattV As Double
    Public UPS_BattRuntime As Double
    Public UPS_BattCapacity As Double
    Public UPS_InputF As Double
    Public UPS_InputV As Double
    Public UPS_OutputV As Double
    Public UPS_Load As Double
    Public UPS_Status As String
    Public UPS_OutPower As Double
    Public UPS_InputA As Double
    Private WindowsVersion As Version = Version.Parse(My.Computer.Info.OSVersion)
    Private MinOsVersionToast As Version = Version.Parse("10.0.18362.0")
    Private AllowToast As Boolean = False
    Private LastAppIconIdx As Integer = -1
    Private ActualAppIconIdx As Integer
    Private WinDarkMode As Boolean = False
    Private AppDarkMode As Boolean = False
    Private HasFocus As Boolean = True
    Private mUpdate As Boolean = False
    Private FormText As String
    Private WinNUT_Crashed As Boolean = False
    Private Event On_Battery()
    Private Event On_Line()
    Private Event UpdateNotifyIconStr(ByVal Reason As String, ByVal Message As String)
    Private Event UpdateBatteryState(ByVal Reason As String)
    Declare Function SetSystemPowerState Lib "kernel32" (ByVal fSuspend As Integer, ByVal fForce As Integer) As Integer

    Public Property UpdateMethod() As String
        Get
            If Me.mUpdate Then
                Me.mUpdate = False
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal Value As String)
            Me.mUpdate = Value
        End Set
    End Property
    Public WriteOnly Property HasCrased() As Boolean
        Set(ByVal Value As Boolean)
            Me.WinNUT_Crashed = Value
        End Set
    End Property

    Private Sub WinNUT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LogFile.WriteLog = True
        LogFile.LogLevel = 3

        AddHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged

        'Init WinNUT Variables
        WinNUT_Globals.Init_Globals()

        'Init WinNUT Parameters
        WinNUT_Params.Init_Params()

        'Load WinNUT Parameters
        WinNUT_Params.Load_Params()

        'Init Log File
        LogFile.WriteLog = WinNUT_Params.Arr_Reg_Key.Item("UseLogFile")
        LogFile.LogLevel = WinNUT_Params.Arr_Reg_Key.Item("Log Level")
        Me.LogLvls = New LogLvl
        LogFile.LogTracing("Initialisation Globals Variables Complete", LogLvl.LOG_DEBUG, Me)
        LogFile.LogTracing("Initialisation Params Complete", LogLvl.LOG_DEBUG, Me)
        LogFile.LogTracing("Loaded Params Complete", LogLvl.LOG_DEBUG, Me)

        'Init Systray
        Me.NotifyIcon.Text = WinNUT_Globals.LongProgramName & " - " & WinNUT_Globals.ShortProgramVersion
        Me.NotifyIcon.Visible = False
        LogFile.LogTracing("NotifyIcons Initialised", LogLvl.LOG_DEBUG, Me)

        'Verify If Toast Compatible

        If Me.MinOsVersionToast.CompareTo(Me.WindowsVersion) < 0 Then
            Me.AllowToast = True
            LogFile.LogTracing("Windows 10 Toast Notification Available", LogLvl.LOG_DEBUG, Me)
            'Dim ico As Icon = Me.Icon
            'Dim file As System.IO.FileStream = New System.IO.FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\WinNUT-Client\WinNut.ico", System.IO.FileMode.OpenOrCreate)
            'ico.Save(file)
            'file.Close()
            'ico.Dispose()
            'ToastPopup.CreateToastCollection(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\WinNUT-Client\WinNut.ico")
        Else
            LogFile.LogTracing("Windows 10 Toast Notification Not Available. Too Old Windows Version", LogLvl.LOG_DEBUG, Me)
        End If

        'Init Connexion to UPS
        UPS_Network.NutHost = WinNUT_Params.Arr_Reg_Key.Item("ServerAddress")
        UPS_Network.NutPort = WinNUT_Params.Arr_Reg_Key.Item("Port")
        UPS_Network.NutUPS = WinNUT_Params.Arr_Reg_Key.Item("UPSName")
        UPS_Network.NutDelay = WinNUT_Params.Arr_Reg_Key.Item("Delay")
        UPS_Network.NutLogin = WinNUT_Params.Arr_Reg_Key.Item("NutLogin")
        UPS_Network.NutPassword = WinNUT_Params.Arr_Reg_Key.Item("NutPassword")
        UPS_Network.AutoReconnect = WinNUT_Params.Arr_Reg_Key.Item("AutoReconnect")
        UPS_Network.Battery_Limit = WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitBatteryCharge")
        UPS_Network.Backup_Limit = WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitUPSRemainTime")
        UPS_Network.UPS_Follow_FSD = WinNUT_Params.Arr_Reg_Key.Item("Follow_FSD")

        'Force Positionning Text Label Because Unknow auto positionning property ???
        Lbl_RTime.Location = New Point(6, 116)
        Lbl_VRTime.Location = New Point(6, 136)
        Lbl_Mfr.Location = New Point(6, 156)
        Lbl_VMfr.Location = New Point(6, 176)
        Lbl_Name.Location = New Point(6, 196)
        Lbl_VName.Location = New Point(6, 216)
        Lbl_Serial.Location = New Point(6, 236)
        Lbl_VSerial.Location = New Point(6, 256)
        Lbl_Firmware.Location = New Point(6, 276)
        Lbl_VFirmware.Location = New Point(6, 296)

        'Add DialGraph
        With AG_InV
            .Location = New Point(6, 26)
            .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxInputVoltage")
            .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinInputVoltage")
            .Value1 = Me.UPS_InputV
            .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
        End With
        With AG_InF
            .Location = New Point(6, 26)
            .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxInputFrequency")
            .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinInputFrequency")
            .Value1 = Me.UPS_InputF
            .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
        End With
        With AG_OutV
            .Location = New Point(6, 26)
            .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxOutputVoltage")
            .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinOutputVoltage")
            .Value1 = Me.UPS_OutputV
            .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
        End With
        With AG_BattCh
            .Location = New Point(6, 26)
            .MaxValue = 100
            .MinValue = 0
            .Value1 = Me.UPS_BattCh
            .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
        End With
        With AG_Load
            .Location = New Point(6, 26)
            .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxUPSLoad")
            .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinUPSLoad")
            .Value1 = Me.UPS_Load
            .Value2 = Me.UPS_OutPower
            .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
        End With
        With AG_BattV
            .Location = New Point(6, 26)
            .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxBattVoltage")
            .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage")
            .Value1 = Me.UPS_BattV
            .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
        End With

        'Verify Is Windows is In Dark Mode
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0) = 0 Then
            LogFile.LogTracing("Windows App Use Dark Theme", LogLvl.LOG_DEBUG, Me)
            AppDarkMode = True
        Else
            LogFile.LogTracing("Windows App Use Light Theme", LogLvl.LOG_DEBUG, Me)
            AppDarkMode = False
        End If
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0) = 0 Then
            LogFile.LogTracing("Windows Use Dark Theme", LogLvl.LOG_DEBUG, Me)
            WinDarkMode = True
        Else
            LogFile.LogTracing("Windows Use Light Theme", LogLvl.LOG_DEBUG, Me)
            WinDarkMode = False
        End If

        'Adapts the icon according to the Win / App Dark Mode 
        Dim Start_App_Icon
        Dim Start_Tray_Icon
        If AppDarkMode Then
            Start_App_Icon = AppIconIdx.IDX_ICO_OFFLINE Or AppIconIdx.WIN_DARK Or AppIconIdx.IDX_OFFSET
        Else
            Start_App_Icon = AppIconIdx.IDX_ICO_OFFLINE Or AppIconIdx.IDX_OFFSET
        End If
        Me.Icon = GetIcon(Start_App_Icon)
        If WinDarkMode Then
            Start_Tray_Icon = AppIconIdx.IDX_ICO_OFFLINE Or AppIconIdx.WIN_DARK Or AppIconIdx.IDX_OFFSET
        Else
            Start_Tray_Icon = AppIconIdx.IDX_ICO_OFFLINE Or AppIconIdx.IDX_OFFSET
        End If

        'Initializes the state of the NotifyICon, the connection to the Nut server and the application icons
        NotifyIcon.Visible = False
        NotifyIcon.Icon = GetIcon(Start_Tray_Icon)
        RaiseEvent UpdateNotifyIconStr(Nothing, Nothing)
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
        UpdateIcon_NotifyIcon()
        Start_Tray_Icon = Nothing

        'Run Update
        If WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdate") = True And WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdateAtStart") = True Then
            LogFile.LogTracing("Run Automatic Update", LogLvl.LOG_DEBUG, Me)
            Dim Update_Frm = New Update_Gui()
            Update_Frm.Activate()
            Update_Frm.Visible = True
            HasFocus = False
        End If
        'ToastPopup.CreateToastCollection()
    End Sub

    Private Sub SystemEvents_PowerModeChanged(ByVal sender As Object, ByVal e As Microsoft.Win32.PowerModeChangedEventArgs)
        Select Case e.Mode
            Case Microsoft.Win32.PowerModes.Resume
                LogFile.LogTracing("Restarting WinNUT after waking up from Windows", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_EXITSLEEP))
                If WinNUT_Params.Arr_Reg_Key.Item("AutoReconnect") = True Then
                    UPS_Network.Connect()
                End If
            Case Microsoft.Win32.PowerModes.Suspend
                LogFile.LogTracing("Windows standby, WinNUT will disconnect", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_GOTOSLEEP))
                UPS_Network.Disconnect()
        End Select
    End Sub

    Private Sub WinNUT_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
        UpdateIcon_NotifyIcon()
        If WinNUT_Params.Arr_Reg_Key.Item("MinimizeToTray") = True And WinNUT_Params.Arr_Reg_Key.Item("MinimizeOnStart") = True Then
            LogFile.LogTracing("Minimize WinNut On Start", LogLvl.LOG_DEBUG, Me)
            Me.WindowState = FormWindowState.Minimized
            Me.NotifyIcon.Visible = True
        Else
            LogFile.LogTracing("Show WinNut Main Gui", LogLvl.LOG_DEBUG, Me)
            Me.NotifyIcon.Visible = False
        End If
        If WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdate") = True Then
            Me.Menu_Help_Sep1.Visible = True
            Me.Menu_Update.Visible = True
            Me.Menu_Update.Visible = Enabled = True
        Else
            Me.Menu_Help_Sep1.Visible = False
            Me.Menu_Update.Visible = False
            Me.Menu_Update.Visible = Enabled = False
        End If
        LogFile.LogTracing("Init Connexion to NutServer", LogLvl.LOG_DEBUG, Me)
        UPS_Network.Connect()
    End Sub

    Private Sub WinNUT_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If WinNUT_Params.Arr_Reg_Key.Item("CloseToTray") = True And WinNUT_Params.Arr_Reg_Key.Item("MinimizeToTray") = True Then
            LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
            UpdateIcon_NotifyIcon()
            LogFile.LogTracing("Minimize Main Gui To Notify Icon", LogLvl.LOG_DEBUG, Me)
            Me.WindowState = FormWindowState.Minimized
            Me.Visible = False
            Me.NotifyIcon.Visible = True
            e.Cancel = True
        Else
            LogFile.LogTracing("Init Disconnecting Before Close WinNut", LogLvl.LOG_DEBUG, Me)
            UPS_Network.Disconnect()
            LogFile.LogTracing("WinNut Is now Closed", LogLvl.LOG_DEBUG, Me)
            RemoveHandler Microsoft.Win32.SystemEvents.PowerModeChanged, AddressOf SystemEvents_PowerModeChanged
            End
        End If

    End Sub

    Private Sub Menu_Quit_Click_1(sender As Object, e As EventArgs) Handles Menu_Quit.Click
        LogFile.LogTracing("Close WinNut From Menu Quit", LogLvl.LOG_DEBUG, Me)
        End
    End Sub

    Private Sub Menu_Settings_Click(sender As Object, e As EventArgs) Handles Menu_Settings.Click
        LogFile.LogTracing("Open Pref Gui From Menu", LogLvl.LOG_DEBUG, Me)
        Pref_Gui.Activate()
        Pref_Gui.Visible = True
        HasFocus = False
    End Sub

    Private Sub Menu_Sys_Exit_Click(sender As Object, e As EventArgs) Handles Menu_Sys_Exit.Click
        LogFile.LogTracing("Close WinNut From Systray", LogLvl.LOG_DEBUG, Me)
        End
    End Sub

    Private Sub Menu_Sys_Settings_Click(sender As Object, e As EventArgs) Handles Menu_Sys_Settings.Click
        LogFile.LogTracing("Open Pref Gui From Systray", LogLvl.LOG_DEBUG, Me)
        Pref_Gui.Activate()
        Pref_Gui.Visible = True
        HasFocus = False
    End Sub

    Private Sub NotifyIcon_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon.MouseClick, NotifyIcon.MouseDoubleClick
        If e.Button <> MouseButtons.Right Then
            LogFile.LogTracing("Restore Main Gui On Mouse Click Notify Icon", LogLvl.LOG_DEBUG, Me)
            Me.Visible = True
            Me.NotifyIcon.Visible = False
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub WinNUT_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If sender.WindowState = FormWindowState.Minimized Then
            If WinNUT_Params.Arr_Reg_Key.Item("MinimizeToTray") = True Then
                LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
                UpdateIcon_NotifyIcon()
                LogFile.LogTracing("Minimize Main Gui To Notify Icon", LogLvl.LOG_DEBUG, Me)
                Me.WindowState = FormWindowState.Minimized
                Me.Visible = False
                Me.NotifyIcon.Visible = True
            End If
            If Me.NotifyIcon.Visible = False Then
                Me.Text = Me.FormText
            Else
                Me.Text = "WinNUT"
            End If
        ElseIf sender.WindowState = FormWindowState.Maximized Or sender.WindowState = FormWindowState.Normal Then
            Me.Text = "WinNUT"
        End If
    End Sub

    Private Sub NewRetry_NotifyIcon() Handles UPS_Network.NewRetry
        Dim Message As String = String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_RETRY), UPS_Network.UPS_Retry, UPS_Network.UPS_MaxRetry)
        RaiseEvent UpdateNotifyIconStr("Retry", Message)
        UpdateIcon_NotifyIcon()
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
    End Sub

    Private Sub Reconnect_NotifyIcon() Handles UPS_Network.Connected
        Me.Menu_UPS_Var.Enabled = True
        UpdateIcon_NotifyIcon()
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
        RaiseEvent UpdateNotifyIconStr("Connected", Nothing)
    End Sub

    Private Sub Deconnected_NotifyIcon() Handles UPS_Network.Deconnected
        ActualAppIconIdx = AppIconIdx.IDX_ICO_OFFLINE
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
        UpdateIcon_NotifyIcon()
        RaiseEvent UpdateNotifyIconStr("Deconnected", Nothing)
        RaiseEvent UpdateBatteryState("Deconnected")
    End Sub

    Private Sub Event_UpdateNotifyIconStr(ByVal Optional Reason As String = Nothing, ByVal Optional Message As String = Nothing) Handles Me.UpdateNotifyIconStr
        Dim ShowVersion As String = WinNUT_Globals.ShortProgramVersion
        Dim NotifyStr As String = WinNUT_Globals.ProgramName & " - " & ShowVersion & vbNewLine
        Dim FormText As String = WinNUT_Globals.ProgramName
        Select Case Reason
            Case Nothing
                If Not UPS_Network.IsConnected Then
                    NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_NOTCONN)
                    FormText &= " - " & WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_NOTCONN)
                End If
            Case "Retry"
                NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_RECONNECT) & vbNewLine
                NotifyStr &= Message
                FormText &= " - Bat: " & Me.UPS_BattCh & "% - " & WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_RECONNECT) & " - " & Message
            Case "Connected"
                NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_CONN)
                FormText &= " - Bat: " & Me.UPS_BattCh & "% - " & WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_CONN)
            Case "Deconnected"
                NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_NOTCONN)
                FormText &= " - " & WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_NOTCONN)
            Case "Unknown UPS"
                NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_UNKNOWN_UPS)
                FormText &= " - " & WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_UNKNOWN_UPS)
            Case "Lost Connect"
                Dim TmpStr = String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_LOSTCONNECT), UPS_Network.NutHost, UPS_Network.NutPort)
                FormText &= " - " & TmpStr
                If (NotifyStr.Length + TmpStr.Length) > 64 Then
                    TmpStr = TmpStr.Substring(0, (64 - NotifyStr.Length - 4)) & "..."
                End If
                NotifyStr &= TmpStr
            Case "Update Data"
                FormText &= " - Bat: " & Me.UPS_BattCh & "% - " & WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_CONN) & " - "
                NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_CONN) & vbNewLine
                If Me.UPS_Status.Trim().StartsWith("OL") Or StrReverse(Me.UPS_Status.Trim()).StartsWith("LO") Then
                    NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_OL) & vbNewLine
                    FormText &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_OL) & " - "
                Else
                    NotifyStr &= String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_OB), UPS_Network.UPS_BattCh) & vbNewLine
                    FormText &= String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_OB), UPS_Network.UPS_BattCh) & " - "
                End If
                Select Case UPS_Network.UPS_BattCh
                    Case 0 To 40
                        NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_LOWBAT)
                        FormText &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_LOWBAT)
                    Case 41 To 100
                        NotifyStr &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_BATOK)
                        FormText &= WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_BATOK)
                End Select
        End Select
        Me.NotifyIcon.Text = NotifyStr
        If Me.WindowState = System.Windows.Forms.FormWindowState.Minimized And Me.NotifyIcon.Visible = False Then
            Me.Text = FormText
        Else
            Me.Text = WinNUT_Globals.LongProgramName
        End If
        Me.FormText = FormText

        LogFile.LogTracing("NotifyIcon Text => " & vbNewLine & NotifyStr, LogLvl.LOG_DEBUG, Me)
    End Sub

    Private Sub Event_UpdateBatteryState(ByVal Optional Reason As String = Nothing) Handles Me.UpdateBatteryState
        Static Dim Old_Battery_Value As Integer = Me.UPS_BattCh
        Dim Status As String = "Unknown"
        Select Case Reason
            Case Nothing, "Deconnected", "Lost Connect"
                If Not UPS_Network.IsConnected Then
                    Me.PBox_Battery_State.Image = Nothing
                End If
                Status = "Unknown"
            Case "Update Data"
                If Me.UPS_BattCh = 100 Then
                    Me.PBox_Battery_State.Image = My.Resources.Battery_Charged
                    Status = "Charged"
                Else
                    If Me.UPS_Status.Trim().StartsWith("OL") Or StrReverse(Me.UPS_Status.Trim()).StartsWith("LO") Then
                        Me.PBox_Battery_State.Image = My.Resources.Battery_Charging
                        Status = "Charging"
                    Else
                        Me.PBox_Battery_State.Image = My.Resources.Battery_Discharging
                        Status = "Discharging"
                    End If
                End If
        End Select
        Old_Battery_Value = Me.UPS_BattCh
        LogFile.LogTracing("Battery Status => " & Status, LogLvl.LOG_DEBUG, Me)
    End Sub
    Private Sub Event_Unknown_UPS() Handles UPS_Network.Unknown_UPS
        ActualAppIconIdx = AppIconIdx.IDX_ICO_OFFLINE
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
        UpdateIcon_NotifyIcon()
        RaiseEvent UpdateNotifyIconStr("Unknown UPS", Nothing)
        LogFile.LogTracing("Cannot Connect : Unknow UPS Name", LogLvl.LOG_DEBUG, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_UNKNOWN_UPS))
        Me.Menu_UPS_Var.Enabled = False
    End Sub

    Private Sub Menu_About_Click(sender As Object, e As EventArgs) Handles Menu_About.Click
        LogFile.LogTracing("Open About Gui From Menu", LogLvl.LOG_DEBUG, Me)
        About_Gui.Activate()
        About_Gui.Visible = True
        HasFocus = False
    End Sub

    Private Sub Menu_Sys_About_Click(sender As Object, e As EventArgs) Handles Menu_Sys_About.Click
        LogFile.LogTracing("Open About Gui From Systray", LogLvl.LOG_DEBUG, Me)
        About_Gui.Activate()
        About_Gui.Visible = True
        HasFocus = False
    End Sub

    Private Sub UPS_Lostconnect() Handles UPS_Network.LostConnect
        LogFile.LogTracing("Nut Server Lost Connection", LogLvl.LOG_ERROR, Me)
        LogFile.LogTracing("Fix All data to null/empty String", LogLvl.LOG_DEBUG, Me)
        Me.UPS_Mfr = ""
        Me.UPS_Model = ""
        Me.UPS_Serial = ""
        Me.UPS_Firmware = ""
        Lbl_VOL.BackColor = Color.White
        Lbl_VOB.BackColor = Color.White
        Lbl_VOLoad.BackColor = Color.White
        Lbl_VBL.BackColor = Color.White
        Lbl_VRTime.Text = ""
        Lbl_VMfr.Text = Me.UPS_Mfr
        Lbl_VName.Text = Me.UPS_Model
        Lbl_VSerial.Text = Me.UPS_Serial
        Lbl_VFirmware.Text = Me.UPS_Firmware
        If UPS_Network.AutoReconnect And UPS_Network.UPS_Retry <= UPS_Network.UPS_MaxRetry Then
            ActualAppIconIdx = AppIconIdx.IDX_ICO_RETRY
        Else
            ActualAppIconIdx = AppIconIdx.IDX_ICO_OFFLINE
        End If
        LogFile.LogTracing("Fix All Dial Data to Min Value/0", LogLvl.LOG_DEBUG, Me)
        AG_InV.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinInputVoltage")
        AG_InF.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinInputFrequency")
        AG_OutV.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinOutputVoltage")
        AG_BattCh.Value1 = 0
        AG_Load.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinUPSLoad")
        AG_Load.Value2 = 0
        AG_BattV.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage")
        UpdateIcon_NotifyIcon()
        RaiseEvent UpdateNotifyIconStr("Lost Connect", Nothing)
        RaiseEvent UpdateBatteryState("Lost Connect")
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
    End Sub

    Public Sub EventOn_ChangeStatus() Handles Me.On_Battery, Me.On_Line, UPS_Network.LostConnect, UPS_Network.Connected, UPS_Network.Deconnected, UPS_Network.NewRetry, UPS_Network.Unknown_UPS, UPS_Network.InvalidLogin
        Me.NotifyIcon.BalloonTipText = Me.NotifyIcon.Text
        If Me.AllowToast And Me.NotifyIcon.BalloonTipText <> "" Then
            Dim Toastparts As String() = Me.NotifyIcon.BalloonTipText.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
            ToastPopup.SendToast(Toastparts)
        ElseIf Me.NotifyIcon.Visible = True And Me.NotifyIcon.BalloonTipText <> "" Then
            Me.NotifyIcon.ShowBalloonTip(10000)
        End If
    End Sub

    Private Sub Update_UPS_Data() Handles UPS_Network.DataUpdated
        If Me.UPS_Mfr = "" And Me.UPS_Model = "" And Me.UPS_Serial = "" And Me.UPS_Firmware = "" Then
            LogFile.LogTracing("Retrieve UPS Informations", LogLvl.LOG_DEBUG, Me)
            Me.UPS_Mfr = UPS_Network.UPS_Mfr
            Me.UPS_Model = UPS_Network.UPS_Model
            Me.UPS_Serial = UPS_Network.UPS_Serial
            Me.UPS_Firmware = UPS_Network.UPS_Firmware
            Lbl_VMfr.Text = Me.UPS_Mfr
            Lbl_VName.Text = Me.UPS_Model
            Lbl_VSerial.Text = Me.UPS_Serial
            Lbl_VFirmware.Text = Me.UPS_Firmware
        End If
        Me.UPS_BattCh = UPS_Network.UPS_BattCh
        Me.UPS_BattV = UPS_Network.UPS_BattV
        Me.UPS_BattRuntime = UPS_Network.UPS_BattRuntime
        Me.UPS_BattCapacity = UPS_Network.UPS_BattCapacity
        Me.UPS_InputF = UPS_Network.UPS_PowerFreq
        Me.UPS_InputV = UPS_Network.UPS_InputV
        Me.UPS_OutputV = UPS_Network.UPS_OutputV
        Me.UPS_Load = UPS_Network.UPS_Load
        Me.UPS_Status = UPS_Network.UPS_Status
        Me.UPS_OutPower = UPS_Network.UPS_OutPower

        If Me.UPS_Status <> Nothing Then
            If Me.UPS_Status.Trim().StartsWith("OL") Or StrReverse(Me.UPS_Status.Trim()).StartsWith("LO") Then
                LogFile.LogTracing("UPS is plugged", LogLvl.LOG_DEBUG, Me)
                Lbl_VOL.BackColor = Color.Green
                Lbl_VOB.BackColor = Color.White
                ActualAppIconIdx = AppIconIdx.IDX_OL
            Else
                LogFile.LogTracing("UPS is unplugged", LogLvl.LOG_DEBUG, Me)
                Lbl_VOL.BackColor = Color.Yellow
                Lbl_VOB.BackColor = Color.Green
                ActualAppIconIdx = 0
            End If

            If Me.UPS_Load > 100 Then
                LogFile.LogTracing("UPS Overload", LogLvl.LOG_ERROR, Me)
                Lbl_VOLoad.BackColor = Color.Red
            Else
                Lbl_VOLoad.BackColor = Color.White
            End If

            Select Case Me.UPS_BattCh
                Case 76 To 100
                    Lbl_VBL.BackColor = Color.White
                    ActualAppIconIdx = ActualAppIconIdx Or AppIconIdx.IDX_BATT_100
                    LogFile.LogTracing("Battery Charged", LogLvl.LOG_DEBUG, Me)
                Case 51 To 75
                    Lbl_VBL.BackColor = Color.White
                    ActualAppIconIdx = ActualAppIconIdx Or AppIconIdx.IDX_BATT_75
                    LogFile.LogTracing("Battery Charged", LogLvl.LOG_DEBUG, Me)
                Case 40 To 50
                    Lbl_VBL.BackColor = Color.White
                    ActualAppIconIdx = ActualAppIconIdx Or AppIconIdx.IDX_BATT_50
                    LogFile.LogTracing("Battery Charged", LogLvl.LOG_DEBUG, Me)
                Case 26 To 39
                    Lbl_VBL.BackColor = Color.Red
                    ActualAppIconIdx = ActualAppIconIdx Or AppIconIdx.IDX_BATT_50
                    LogFile.LogTracing("Low Battery", LogLvl.LOG_DEBUG, Me)
                Case 11 To 25
                    Lbl_VBL.BackColor = Color.Red
                    ActualAppIconIdx = ActualAppIconIdx Or AppIconIdx.IDX_BATT_25
                    LogFile.LogTracing("Low Battery", LogLvl.LOG_DEBUG, Me)
                Case 0 To 10
                    Lbl_VBL.BackColor = Color.Red
                    ActualAppIconIdx = ActualAppIconIdx Or AppIconIdx.IDX_BATT_0
                    LogFile.LogTracing("Low Battery", LogLvl.LOG_DEBUG, Me)
            End Select

            Dim iSpan As TimeSpan = TimeSpan.FromSeconds(Me.UPS_BattRuntime)
            Lbl_VRTime.Text = iSpan.Hours.ToString.PadLeft(2, "0"c) & ":" &
                                    iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                                    iSpan.Seconds.ToString.PadLeft(2, "0"c)
        End If
        LogFile.LogTracing("Update Dial", LogLvl.LOG_DEBUG, Me)
        AG_InV.Value1 = Me.UPS_InputV
        AG_InF.Value1 = Me.UPS_InputF
        AG_OutV.Value1 = Me.UPS_OutputV
        AG_BattCh.Value1 = Me.UPS_BattCh
        AG_Load.Value1 = Me.UPS_Load
        AG_Load.Value2 = Me.UPS_OutPower
        AG_BattV.Value1 = Me.UPS_BattV
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
        UpdateIcon_NotifyIcon()
        RaiseEvent UpdateNotifyIconStr("Update Data", Nothing)
        RaiseEvent UpdateBatteryState("Update Data")
        If Me.UPS_Status = "OL" And UPS_Network.UPS_Status = "OB" Then
            RaiseEvent On_Battery()
        End If
        If Me.UPS_Status = "OB" And UPS_Network.UPS_Status = "OL" Then
            RaiseEvent On_Line()
        End If
    End Sub

    Private Sub Menu_Disconnect_Click(sender As Object, e As EventArgs) Handles Menu_Disconnect.Click
        LogFile.LogTracing("Force Disconnect from menu", LogLvl.LOG_DEBUG, Me)
        UPS_Network.Disconnect(True)
        ReInitDisplayValues()
        ActualAppIconIdx = AppIconIdx.IDX_ICO_OFFLINE
        LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
        UpdateIcon_NotifyIcon()
        RaiseEvent UpdateNotifyIconStr("Deconnected", Nothing)
        RaiseEvent UpdateBatteryState("Deconnected")
    End Sub

    Private Sub ReInitDisplayValues()
        LogFile.LogTracing("Update all informations displayed ton empty values", LogLvl.LOG_DEBUG, Me)
        Me.UPS_Mfr = ""
        Me.UPS_Model = ""
        Me.UPS_Serial = ""
        Me.UPS_Firmware = ""
        Lbl_VOL.BackColor = Color.White
        Lbl_VOB.BackColor = Color.White
        Lbl_VOLoad.BackColor = Color.White
        Lbl_VBL.BackColor = Color.White
        Lbl_VRTime.Text = ""
        Lbl_VMfr.Text = Me.UPS_Mfr
        Lbl_VName.Text = Me.UPS_Model
        Lbl_VSerial.Text = Me.UPS_Serial
        Lbl_VFirmware.Text = Me.UPS_Firmware
        AG_InV.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinInputVoltage")
        AG_InF.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinInputFrequency")
        AG_OutV.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinOutputVoltage")
        AG_BattCh.Value1 = 0
        AG_Load.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinUPSLoad")
        AG_Load.Value2 = 0
        AG_BattV.Value1 = WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage")
    End Sub

    Private Sub Menu_Reconnect_Click(sender As Object, e As EventArgs) Handles Menu_Reconnect.Click
        LogFile.LogTracing("Force Reconnect from menu", LogLvl.LOG_DEBUG, Me)
        UPS_Network.Connect()
    End Sub

    Private Sub WinNUT_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        If WinNUT_Crashed Then
            Me.Hide()
        Else
            LogFile.LogTracing("Main Gui Lose Focus", LogLvl.LOG_DEBUG, Me)
            HasFocus = False
            Dim Tmp_App_Mode As Integer
            If Not AppDarkMode Then
                Tmp_App_Mode = AppIconIdx.WIN_DARK Or AppIconIdx.IDX_OFFSET
            Else
                Tmp_App_Mode = AppIconIdx.IDX_OFFSET
            End If
            Dim TmpGuiIDX = ActualAppIconIdx Or Tmp_App_Mode
            Me.Icon = GetIcon(TmpGuiIDX)
            LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
            UpdateIcon_NotifyIcon()
        End If
    End Sub

    Private Sub WinNUT_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        If WinNUT_Crashed Then
            Me.Hide()
        Else
            LogFile.LogTracing("Main Gui Has Focus", LogLvl.LOG_DEBUG, Me)
            HasFocus = True
            Dim Tmp_App_Mode As Integer
            If AppDarkMode Then
                Tmp_App_Mode = AppIconIdx.WIN_DARK Or AppIconIdx.IDX_OFFSET
            Else
                Tmp_App_Mode = AppIconIdx.IDX_OFFSET
            End If
            Dim TmpGuiIDX = ActualAppIconIdx Or Tmp_App_Mode
            Me.Icon = GetIcon(TmpGuiIDX)
            LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
            UpdateIcon_NotifyIcon()
        End If
    End Sub

    Public Sub WinNUT_PrefsChanged()
        LogFile.LogTracing("WinNut Preferences Changed", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_PREFS))
        Dim NeedReconnect As Boolean = False
        If WinNUT_Params.Arr_Reg_Key.Item("AutoReconnect") <> UPS_Network.AutoReconnect Then
            If WinNUT_Params.Arr_Reg_Key.Item("AutoReconnect") Then
                UPS_Network.AutoReconnect = True
            Else
                UPS_Network.AutoReconnect = False
            End If
        End If
        If UPS_Network.NutHost <> WinNUT_Params.Arr_Reg_Key.Item("ServerAddress") Then
            NeedReconnect = True
            UPS_Network.NutHost = WinNUT_Params.Arr_Reg_Key.Item("ServerAddress")
        End If
        If UPS_Network.NutPort <> WinNUT_Params.Arr_Reg_Key.Item("Port") Then
            NeedReconnect = True
            UPS_Network.NutPort = WinNUT_Params.Arr_Reg_Key.Item("Port")
        End If
        If UPS_Network.NutUPS <> WinNUT_Params.Arr_Reg_Key.Item("UPSName") Then
            NeedReconnect = True
            UPS_Network.NutUPS = WinNUT_Params.Arr_Reg_Key.Item("UPSName")
        End If
        If UPS_Network.NutDelay <> WinNUT_Params.Arr_Reg_Key.Item("Delay") Then
            NeedReconnect = True
            UPS_Network.NutDelay = WinNUT_Params.Arr_Reg_Key.Item("Delay")
        End If
        If UPS_Network.NutLogin <> WinNUT_Params.Arr_Reg_Key.Item("NutLogin") Then
            NeedReconnect = True
            UPS_Network.NutLogin = WinNUT_Params.Arr_Reg_Key.Item("NutLogin")
        End If
        If UPS_Network.NutPassword <> WinNUT_Params.Arr_Reg_Key.Item("NutPassword") Then
            NeedReconnect = True
            UPS_Network.NutPassword = WinNUT_Params.Arr_Reg_Key.Item("NutPassword")
        End If
        If UPS_Network.UPS_Follow_FSD <> WinNUT_Params.Arr_Reg_Key.Item("Follow_FSD") Then
            UPS_Network.UPS_Follow_FSD = WinNUT_Params.Arr_Reg_Key.Item("Follow_FSD")
        End If
        UPS_Network.Battery_Limit = WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitBatteryCharge")
        UPS_Network.Backup_Limit = WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitUPSRemainTime")
        If NeedReconnect And UPS_Network.IsConnected Then
            LogFile.LogTracing("Connection parameters Changed. Force Disconnect", LogLvl.LOG_DEBUG, Me)
            UPS_Network.Disconnect(True, True)
            ReInitDisplayValues()
            ActualAppIconIdx = AppIconIdx.IDX_ICO_OFFLINE
            LogFile.LogTracing("Update Icon", LogLvl.LOG_DEBUG, Me)
            UpdateIcon_NotifyIcon()
            LogFile.LogTracing("New Parameter Applyed. Force Reconnect", LogLvl.LOG_DEBUG, Me)
            UPS_Network.Connect()
        ElseIf Not UPS_Network.IsConnected Then
            LogFile.LogTracing("New Parameter Applyed. Force Reconnect", LogLvl.LOG_DEBUG, Me)
            UPS_Network.Connect()
        End If
        NeedReconnect = Nothing
        With AG_InV
            If (.MaxValue <> WinNUT_Params.Arr_Reg_Key.Item("MaxInputVoltage")) Or (.MinValue <> WinNUT_Params.Arr_Reg_Key.Item("MinInputVoltage")) Then
                LogFile.LogTracing("Parameter Dial Input Voltage Need to be Updated", LogLvl.LOG_DEBUG, Me)
                .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxInputVoltage")
                .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinInputVoltage")
                .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
                LogFile.LogTracing("Parameter Dial Input Voltage Updated", LogLvl.LOG_DEBUG, Me)
            End If
        End With
        With AG_InF
            If (.MaxValue <> WinNUT_Params.Arr_Reg_Key.Item("MaxInputFrequency")) Or (.MinValue <> WinNUT_Params.Arr_Reg_Key.Item("MinInputFrequency")) Then
                LogFile.LogTracing("Parameter Dial Input Frequency Need to be Updated", LogLvl.LOG_DEBUG, Me)
                .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxInputFrequency")
                .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinInputFrequency")
                .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
                LogFile.LogTracing("Parameter Dial Input Frequency Updated", LogLvl.LOG_DEBUG, Me)
            End If
        End With
        With AG_OutV
            If (.MaxValue <> WinNUT_Params.Arr_Reg_Key.Item("MaxOutputVoltage")) Or (.MinValue <> WinNUT_Params.Arr_Reg_Key.Item("MinOutputVoltage")) Then
                LogFile.LogTracing("Parameter Dial Output Voltage Need to be Updated", LogLvl.LOG_DEBUG, Me)
                .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxOutputVoltage")
                .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinOutputVoltage")
                .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
                LogFile.LogTracing("Parameter Dial Output Voltage Updated", LogLvl.LOG_DEBUG, Me)
            End If
        End With
        With AG_Load
            If (.MaxValue <> WinNUT_Params.Arr_Reg_Key.Item("MaxUPSLoad")) Or (.MinValue <> WinNUT_Params.Arr_Reg_Key.Item("MinUPSLoad")) Then
                LogFile.LogTracing("Parameter Dial UPS Load Need to be Updated", LogLvl.LOG_DEBUG, Me)
                .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxUPSLoad")
                .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinUPSLoad")
                .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
                LogFile.LogTracing("Parameter Dial UPS Load Updated", LogLvl.LOG_DEBUG, Me)
            End If
        End With
        With AG_BattV
            If (.MaxValue <> WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage")) Or (.MinValue <> WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage")) Then
                LogFile.LogTracing("Parameter Dial Voltage Battery Need to be Updated", LogLvl.LOG_DEBUG, Me)
                .MaxValue = WinNUT_Params.Arr_Reg_Key.Item("MaxBattVoltage")
                .MinValue = WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage")
                .ScaleLinesMajorStepValue = CInt((.MaxValue - .MinValue) / 5)
                LogFile.LogTracing("Parameter Dial Voltage Battery Updated", LogLvl.LOG_DEBUG, Me)
            End If
        End With
        If WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdate") = True Then
            Me.Menu_Help_Sep1.Visible = True
            Me.Menu_Update.Visible = True
            Me.Menu_Update.Visible = Enabled = True
        Else
            Me.Menu_Help_Sep1.Visible = False
            Me.Menu_Update.Visible = False
            Me.Menu_Update.Visible = Enabled = False
        End If
    End Sub

    Private Sub UpdateIcon_NotifyIcon()
        Dim Tmp_Win_Mode As Integer
        Dim Tmp_App_Mode As Integer
        If (ActualAppIconIdx <> LastAppIconIdx) Then
            LogFile.LogTracing("Status Icon Changed", LogLvl.LOG_DEBUG, Me)
            If WinDarkMode Then
                Tmp_Win_Mode = AppIconIdx.WIN_DARK Or AppIconIdx.IDX_OFFSET
            Else
                Tmp_Win_Mode = AppIconIdx.IDX_OFFSET
            End If
            If AppDarkMode Then
                Tmp_App_Mode = AppIconIdx.WIN_DARK Or AppIconIdx.IDX_OFFSET
            Else
                Tmp_App_Mode = AppIconIdx.IDX_OFFSET
            End If
            Dim TmpGuiIDX = ActualAppIconIdx Or Tmp_App_Mode
            If Not HasFocus Then
                TmpGuiIDX = TmpGuiIDX Or AppIconIdx.WIN_DARK
            End If
            Dim TmpTrayIDX = ActualAppIconIdx Or Tmp_Win_Mode
            LogFile.LogTracing("New Icon Value For Systray : " & TmpTrayIDX.ToString, LogLvl.LOG_DEBUG, Me)
            LogFile.LogTracing("New Icon Value For Gui : " & TmpGuiIDX.ToString, LogLvl.LOG_DEBUG, Me)
            NotifyIcon.Icon = GetIcon(TmpTrayIDX)
            Me.Icon = GetIcon(TmpGuiIDX)
            LastAppIconIdx = ActualAppIconIdx
        End If
    End Sub

    Private Function GetIcon(ByVal IconIdx As Integer) As Icon
        Select Case IconIdx
            Case 1025
                Return My.Resources._1025
            Case 1026
                Return My.Resources._1026
            Case 1028
                Return My.Resources._1028
            Case 1032
                Return My.Resources._1032
            Case 1040
                Return My.Resources._1040
            Case 1057
                Return My.Resources._1057
            Case 1058
                Return My.Resources._1058
            Case 1060
                Return My.Resources._1060
            Case 1064
                Return My.Resources._1064
            Case 1072
                Return My.Resources._1072
            Case 1079
                Return My.Resources._1079
            Case 1080
                Return My.Resources._1080
            Case 1092
                Return My.Resources._1092
            Case 1096
                Return My.Resources._1096
            Case 1104
                Return My.Resources._1096
            Case 1121
                Return My.Resources._1121
            Case 1122
                Return My.Resources._1122
            Case 1124
                Return My.Resources._1124
            Case 1128
                Return My.Resources._1128
            Case 1136
                Return My.Resources._1136
            Case 1152
                Return My.Resources._1152
            Case 1216
                Return My.Resources._1216
            Case 1280
                Return My.Resources._1280
            Case 1344
                Return My.Resources._1344
            Case Else
                Return My.Resources._1136
        End Select
    End Function

    Private Sub Menu_UPS_Var_Click(sender As Object, e As EventArgs) Handles Menu_UPS_Var.Click
        LogFile.LogTracing("Open List Var Gui", LogLvl.LOG_DEBUG, Me)
        List_Var_Gui.Activate()
        List_Var_Gui.Visible = True
        HasFocus = False
    End Sub

    Public Shared Sub Update_InstantLog(ByVal sender As System.Object) Handles LogFile.NewData
        Dim Message As String = LogFile.CurrentLogData
        Static Dim Event_Id = 1
        LogFile.LogTracing("New Log to CB_Current Log : " & Message, LogLvl.LOG_DEBUG, sender.ToString)
        Message = "[Id " & Event_Id & ": " & Format(Now, "General Date") & "] " & Message
        Event_Id += 1
        WinNUT.CB_CurrentLog.Items.Insert(0, Message)
        WinNUT.CB_CurrentLog.SelectedIndex = 0
        If WinNUT.CB_CurrentLog.Items.Count > 10 Then
            For i = 10 To (WinNUT.CB_CurrentLog.Items.Count - 1) Step 1
                WinNUT.CB_CurrentLog.Items.Remove(i)
            Next
        End If
    End Sub

    Private Sub Shutdown_Event() Handles UPS_Network.Shutdown_Condition
        If WinNUT_Params.Arr_Reg_Key.Item("ImmediateStopAction") Then
            Shutdown_Action()
        Else
            LogFile.LogTracing("Open Shutdown Gui", LogLvl.LOG_DEBUG, Me)
            Shutdown_Gui.Activate()
            Shutdown_Gui.Visible = True
            HasFocus = False
        End If
    End Sub

    Private Sub Stop_Shutdown_Event() Handles UPS_Network.Stop_Shutdown
        Shutdown_Gui.Shutdown_Timer.Stop()
        Shutdown_Gui.Shutdown_Timer.Enabled = False
        Shutdown_Gui.Grace_Timer.Stop()
        Shutdown_Gui.Grace_Timer.Enabled = False
        Shutdown_Gui.Hide()
        Shutdown_Gui.Close()
    End Sub

    Public Sub Shutdown_Action()
        Select Case WinNUT_Params.Arr_Reg_Key.Item("TypeOfStop")
            Case 0
                Process.Start("C:\WINDOWS\system32\Shutdown.exe", "-f -s -t 0")
            Case 1
                SetSystemPowerState(True, 0)
            Case 2
                SetSystemPowerState(False, 0)
        End Select
    End Sub

    Private Sub Menu_Update_Click(sender As Object, e As EventArgs) Handles Menu_Update.Click
        Me.mUpdate = True
        'Dim th As System.Threading.Thread = New Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf Run_Update))
        'th.SetApartmentState(System.Threading.ApartmentState.STA)
        'th.Start(Me.UpdateMethod)
        LogFile.LogTracing("Open About Gui From Menu", LogLvl.LOG_DEBUG, Me)
        Dim Update_Frm = New Update_Gui(Me.mUpdate)
        Update_Frm.Activate()
        Update_Frm.Visible = True
        HasFocus = False
    End Sub

    Private Sub Menu_Import_Ini_Click(sender As Object, e As EventArgs) Handles Menu_Import_Ini.Click
        Dim SelectIni As OpenFileDialog = New OpenFileDialog()
        Dim IniFile As String = ""
        With SelectIni
            .Title = "Locate ups.ini"
            If System.IO.Directory.Exists("C:\Winnut") Then
                .InitialDirectory = "C:\Winnut\"
            Else
                .InitialDirectory = "C:\"
            End If
            .Filter = "ups.ini|ups.ini|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = DialogResult.OK Then
                IniFile = .FileName
            Else
                Return
            End If
        End With

        If WinNUT_Params.ImportIni(IniFile) Then
            LogFile.LogTracing("Import Old IniFile : Success", LogLvl.LOG_DEBUG, Me)
            If Not IniFile.EndsWith("old") Then
                My.Computer.FileSystem.MoveFile(IniFile, IniFile & ".old")
                MsgBox(String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_OLDINI_RENAMED), IniFile))
            Else
                MsgBox(String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_MAIN_OLDINI), IniFile))
            End If

        Else
            LogFile.LogTracing("Failed To import old IniFile", LogLvl.LOG_DEBUG, Me)
            LogFile.LogTracing("Initialisation Params Complete", LogLvl.LOG_DEBUG, Me)
            LogFile.LogTracing("Loaded Params Complete", LogLvl.LOG_DEBUG, Me)
        End If
        Me.WinNUT_PrefsChanged()
        UPS_Network.Connect()
    End Sub

End Class

