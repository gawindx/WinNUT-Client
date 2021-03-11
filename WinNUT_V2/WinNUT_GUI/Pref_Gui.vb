' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Imports WinNUT_Params = WinNUT_Client_Common.WinNUT_Params
Imports Logger = WinNUT_Client_Common.Logger
Imports LogLvl = WinNUT_Client_Common.LogLvl
Imports WinNUT_Globals = WinNUT_Client_Common.WinNUT_Globals
Public Class Pref_Gui
    Private IsShowed As Boolean = False
    Private IsSaved As Boolean = False
    Private LogFile As Logger
    Private Sub Btn_Cancel_Click(sender As Object, e As EventArgs) Handles Btn_Cancel.Click
        LogFile.LogTracing("Close Pref Gui from Button Cancel", LogLvl.LOG_DEBUG, Me)
        Me.Close()
    End Sub

    Private Sub Save_Params()
        Try
            Me.IsSaved = False
            LogFile.LogTracing("Save Parameters.", LogLvl.LOG_DEBUG, Me)
            WinNUT_Params.Arr_Reg_Key.Item("ServerAddress") = Tb_Server_IP.Text
            WinNUT_Params.Arr_Reg_Key.Item("Port") = CInt(Tb_Port.Text)
            WinNUT_Params.Arr_Reg_Key.Item("UPSName") = Tb_UPS_Name.Text
            WinNUT_Params.Arr_Reg_Key.Item("Delay") = (CInt(Tb_Delay_Com.Text) * 1000)
            WinNUT_Params.Arr_Reg_Key.Item("NutLogin") = Tb_Login_Nut.Text
            WinNUT_Params.Arr_Reg_Key.Item("NutPassword") = Tb_Pwd_Nut.Text
            WinNUT_Params.Arr_Reg_Key.Item("AutoReconnect") = Cb_Reconnect.Checked
            WinNUT_Params.Arr_Reg_Key.Item("MinInputVoltage") = CInt(Tb_InV_Min.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MaxInputVoltage") = CInt(Tb_InV_Max.Text)
            WinNUT_Params.Arr_Reg_Key.Item("FrequencySupply") = Cbx_Freq_Input.SelectedIndex
            WinNUT_Params.Arr_Reg_Key.Item("MinInputFrequency") = CInt(Tb_InF_Min.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MaxInputFrequency") = CInt(Tb_InF_Max.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MinOutputVoltage") = CInt(Tb_OutV_Min.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MaxOutputVoltage") = CInt(Tb_OutV_Max.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MinUPSLoad") = CInt(Tb_Load_Min.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MaxUPSLoad") = CInt(Tb_Load_Max.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage") = CInt(Tb_BattV_Min.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MaxBattVoltage") = CInt(Tb_BattV_Max.Text)
            WinNUT_Params.Arr_Reg_Key.Item("MinimizeToTray") = CB_Systray.Checked
            WinNUT_Params.Arr_Reg_Key.Item("MinimizeOnStart") = CB_Start_Mini.Checked
            WinNUT_Params.Arr_Reg_Key.Item("CloseToTray") = CB_Close_Tray.Checked
            WinNUT_Params.Arr_Reg_Key.Item("StartWithWindows") = CB_Start_W_Win.Checked
            WinNUT_Params.Arr_Reg_Key.Item("UseLogFile") = CB_Use_Logfile.Checked
            WinNUT_Params.Arr_Reg_Key.Item("Log Level") = Cbx_LogLevel.SelectedIndex
            WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitBatteryCharge") = CInt(Tb_BattLimit_Load.Text)
            WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitUPSRemainTime") = CInt(Tb_BattLimit_Time.Text)
            WinNUT_Params.Arr_Reg_Key.Item("ImmediateStopAction") = Cb_ImmediateStop.Checked
            WinNUT_Params.Arr_Reg_Key.Item("Follow_FSD") = CB_Follow_FSD.Checked
            WinNUT_Params.Arr_Reg_Key.Item("TypeOfStop") = Cbx_TypeStop.SelectedIndex
            WinNUT_Params.Arr_Reg_Key.Item("DelayToShutdown") = CInt(Tb_Delay_Stop.Text)
            WinNUT_Params.Arr_Reg_Key.Item("AllowExtendedShutdownDelay") = Cb_ExtendTime.Checked
            WinNUT_Params.Arr_Reg_Key.Item("ExtendedShutdownDelay") = CInt(Tb_GraceTime.Text)
            WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdate") = Cb_Verify_Update.Checked
            WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdateAtStart") = Cb_Update_At_Start.Checked
            WinNUT_Params.Arr_Reg_Key.Item("DelayBetweenEachVerification") = Cbx_Delay_Verif.SelectedIndex
            WinNUT_Params.Arr_Reg_Key.Item("StableOrDevBranch") = Cbx_Branch_Update.SelectedIndex
            WinNUT_Params.Save_Params()
            If CB_Start_W_Win.Checked Then
                If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", Application.ProductName, Nothing) Is Nothing Then
                    My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).SetValue(Application.ProductName, Application.ExecutablePath)
                    LogFile.LogTracing("WinNUT Added to Startup.", LogLvl.LOG_DEBUG, Me)
                End If
            Else
                If Not My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", Application.ProductName, Nothing) Is Nothing Then
                    My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).DeleteValue(Application.ProductName)
                    LogFile.LogTracing("WinNUT Removed From Startup.", LogLvl.LOG_DEBUG, Me)
                End If
            End If
            If CB_Use_Logfile.Checked Then
                WinNUT.LogFile.WriteLog = True
                LogFile.LogTracing("LogFile Enabled.", LogLvl.LOG_DEBUG, Me)
            Else
                WinNUT.LogFile.WriteLog = False
                LogFile.LogTracing("LogFile Disabled.", LogLvl.LOG_DEBUG, Me)
            End If
            WinNUT.LogFile.LogLevel = Cbx_LogLevel.SelectedIndex

            WinNUT.LogFile.LogTracing("Pref_Gui Params Saved", 1, Me)
            If My.Computer.FileSystem.FileExists(WinNUT_Globals.LogFilePath) Then
                Btn_ViewLog.Enabled = True
                Btn_DeleteLog.Enabled = True
            Else
                Btn_ViewLog.Enabled = False
                Btn_DeleteLog.Enabled = False
            End If
            WinNUT.WinNUT_PrefsChanged()
            Me.IsSaved = True
        Catch e As Exception
            Me.IsSaved = False
        End Try
    End Sub

    Private Sub Btn_Apply_Click(sender As Object, e As EventArgs) Handles Btn_Apply.Click
        Me.Save_Params()
        If Me.IsSaved Then
            Me.Btn_Apply.Enabled = False
        End If
    End Sub

    Private Sub Btn_Ok_Click(sender As Object, e As EventArgs) Handles Btn_Ok.Click
        If Not Me.IsSaved Then
            Me.Save_Params()
        End If
        Me.Close()
    End Sub

    Private Sub Pref_Gui_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try
            Me.IsShowed = False
            Tb_Server_IP.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("ServerAddress"))
            Tb_Port.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("Port"))
            Tb_UPS_Name.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("UPSName"))
            Tb_Delay_Com.Text = CStr(Math.Round(WinNUT_Params.Arr_Reg_Key.Item("Delay") / 1000))
            Tb_Login_Nut.Text = WinNUT_Params.Arr_Reg_Key.Item("NutLogin")
            Tb_Pwd_Nut.Text = WinNUT_Params.Arr_Reg_Key.Item("NutPassword")
            Cb_Reconnect.Checked = WinNUT_Params.Arr_Reg_Key.Item("AutoReconnect")
            Tb_InV_Min.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MinInputVoltage"))
            Tb_InV_Max.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MaxInputVoltage"))
            Cbx_Freq_Input.SelectedIndex = WinNUT_Params.Arr_Reg_Key.Item("FrequencySupply")
            Tb_InF_Min.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MinInputFrequency"))
            Tb_InF_Max.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MaxInputFrequency"))
            Tb_OutV_Min.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MinOutputVoltage"))
            Tb_OutV_Max.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MaxOutputVoltage"))
            Tb_Load_Min.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MinUPSLoad"))
            Tb_Load_Max.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MaxUPSLoad"))
            Tb_BattV_Min.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MinBattVoltage"))
            Tb_BattV_Max.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("MaxBattVoltage"))
            CB_Systray.Checked = WinNUT_Params.Arr_Reg_Key.Item("MinimizeToTray")
            CB_Start_Mini.Checked = WinNUT_Params.Arr_Reg_Key.Item("MinimizeOnStart")
            CB_Close_Tray.Checked = WinNUT_Params.Arr_Reg_Key.Item("CloseToTray")
            CB_Start_W_Win.Checked = WinNUT_Params.Arr_Reg_Key.Item("StartWithWindows")
            CB_Use_Logfile.Checked = WinNUT_Params.Arr_Reg_Key.Item("UseLogFile")
            Cbx_LogLevel.SelectedIndex = WinNUT_Params.Arr_Reg_Key.Item("Log Level")
            Tb_BattLimit_Load.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitBatteryCharge"))
            Tb_BattLimit_Time.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("ShutdownLimitUPSRemainTime"))
            Cb_ImmediateStop.Checked = WinNUT_Params.Arr_Reg_Key.Item("ImmediateStopAction")
            CB_Follow_FSD.Checked = WinNUT_Params.Arr_Reg_Key.Item("Follow_FSD")
            Cbx_TypeStop.SelectedIndex = WinNUT_Params.Arr_Reg_Key.Item("TypeOfStop")
            Tb_Delay_Stop.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("DelayToShutdown"))
            Cb_ExtendTime.Checked = WinNUT_Params.Arr_Reg_Key.Item("AllowExtendedShutdownDelay")
            Tb_GraceTime.Text = CStr(WinNUT_Params.Arr_Reg_Key.Item("ExtendedShutdownDelay"))
            Cb_Verify_Update.Checked = WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdate")
            Cb_Update_At_Start.Checked = WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdateAtStart")
            Cbx_Delay_Verif.SelectedIndex = WinNUT_Params.Arr_Reg_Key.Item("DelayBetweenEachVerification")
            Cbx_Branch_Update.SelectedIndex = WinNUT_Params.Arr_Reg_Key.Item("StableOrDevBranch")
            If CB_Systray.Checked Then
                CB_Start_Mini.Enabled = True
                CB_Close_Tray.Enabled = True
            Else
                CB_Start_Mini.Enabled = False
                CB_Close_Tray.Enabled = False
            End If
            If Cb_ImmediateStop.Checked Then
                Tb_Delay_Stop.Enabled = False
            Else
                Tb_Delay_Stop.Enabled = True
            End If
            If Cb_ExtendTime.Checked Then
                Tb_GraceTime.Enabled = True
            Else
                Tb_GraceTime.Enabled = False
            End If
            If Cb_Verify_Update.Checked Then
                Cb_Update_At_Start.Enabled = True
                Cbx_Delay_Verif.Enabled = True
                Cbx_Branch_Update.Enabled = True
            Else
                Cb_Update_At_Start.Enabled = False
                Cbx_Delay_Verif.Enabled = False
                Cbx_Branch_Update.Enabled = False
            End If

            For Each TabCtrl In Me.TabControl_Options.Controls.OfType(Of TabPage)()
                Dim TBoxes = TabCtrl.Controls.OfType(Of TextBox)()
                Dim ChkBoxes = TabCtrl.Controls.OfType(Of CheckBox)()
                Dim CmbBoxes = TabCtrl.Controls.OfType(Of ComboBox)()
                For Each TBox In TBoxes
                    AddHandler TBox.TextChanged, AddressOf Event_Ctrl_Value_Changed
                Next
                For Each ChkBox In ChkBoxes
                    AddHandler ChkBox.CheckedChanged, AddressOf Event_Ctrl_Value_Changed
                Next
                For Each CmbBox In CmbBoxes
                    AddHandler CmbBox.SelectedIndexChanged, AddressOf Event_Ctrl_Value_Changed
                Next
            Next
            Me.Btn_Apply.Enabled = False
            Me.IsShowed = True
            LogFile.LogTracing("Pref Gui Opened.", LogLvl.LOG_DEBUG, Me)
        Catch Except As Exception
            Me.IsShowed = False
            Me.Close()
            LogFile.LogTracing("Error on Opening Pref_Gui.", LogLvl.LOG_ERROR, Me)
        End Try
    End Sub

    Private Sub CB_Systray_CheckedChanged(sender As Object, e As EventArgs) Handles CB_Systray.CheckedChanged
        If CB_Systray.Checked Then
            CB_Start_Mini.Enabled = True
            CB_Close_Tray.Enabled = True
        Else
            CB_Start_Mini.Enabled = False
            CB_Close_Tray.Enabled = False
        End If
    End Sub

    Private Sub Cb_ImmediateStop_CheckedChanged(sender As Object, e As EventArgs) Handles Cb_ImmediateStop.CheckedChanged
        If Cb_ImmediateStop.Checked Then
            Tb_Delay_Stop.Enabled = False
        Else
            Tb_Delay_Stop.Enabled = True
            Number_Validating(Tb_Delay_Stop, New System.ComponentModel.CancelEventArgs())
        End If
    End Sub

    Private Sub Cb_ExtendTime_CheckedChanged(sender As Object, e As EventArgs) Handles Cb_ExtendTime.CheckedChanged
        If Cb_ExtendTime.Checked Then
            Tb_GraceTime.Enabled = True
            Number_Validating(Tb_GraceTime, New System.ComponentModel.CancelEventArgs())
        Else
            Tb_GraceTime.Enabled = False
        End If
    End Sub

    Private Sub Cb_Verify_Update_CheckedChanged(sender As Object, e As EventArgs) Handles Cb_Verify_Update.CheckedChanged
        If Cb_Verify_Update.Checked Then
            Cb_Update_At_Start.Enabled = True
            Cbx_Delay_Verif.Enabled = True
            Cbx_Branch_Update.Enabled = True
        Else
            Cb_Update_At_Start.Enabled = False
            Cbx_Delay_Verif.Enabled = False
            Cbx_Branch_Update.Enabled = False
        End If
    End Sub

    Private Sub Number_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Tb_Port.Validating, Tb_OutV_Min.Validating, Tb_OutV_Max.Validating, Tb_Load_Min.Validating, Tb_Load_Max.Validating, Tb_InV_Min.Validating, Tb_InV_Max.Validating, Tb_InF_Min.Validating, Tb_InF_Max.Validating, Tb_GraceTime.Validating, Tb_Delay_Stop.Validating, Tb_Delay_Com.Validating, Tb_BattV_Min.Validating, Tb_BattV_Max.Validating, Tb_BattLimit_Time.Validating, Tb_BattLimit_Load.Validating
        If Me.IsShowed Then
            Dim StrTest As String = sender.Text
            Dim Result As Object = 0
            Dim MinValue, MaxValue As Integer

            LogFile.LogTracing(String.Format("Check that the value of {0} for {1} is correct.", sender.Text, sender.Name), LogLvl.LOG_DEBUG, Me)
            Select Case sender.Name
                Case "Tb_Delay_Com"
                    MinValue = 0
                    MaxValue = 60
                Case "Tb_Port"
                    MinValue = 1
                    MaxValue = 65536
                Case "Tb_OutV_Min", "Tb_OutV_Max", "Tb_InV_Min", "Tb_InV_Max", "Tb_BattV_Min", "Tb_BattV_Max"
                    MinValue = 0
                    MaxValue = 999
                Case "Tb_Load_Min", "Tb_Load_Max", "Tb_InF_Min", "Tb_InF_Max", "Tb_BattLimit_Load"
                    MinValue = 0
                    MaxValue = 100
                Case "Tb_BattLimit_Time"
                    MinValue = 0
                    MaxValue = 3600
            'Min value has to be 1 as 0 can't be assigned to a timer interval (used in Shutdown_Gui)
                Case "Tb_GraceTime", "Tb_Delay_Stop"
                    MinValue = 1
                    MaxValue = 3600
            End Select

            If sender.Text = "" Then
                sender.Text = MinValue
            End If

            If Integer.TryParse(sender.Text, Result) Then
                If (Result >= MinValue And Result <= MaxValue) Then
                    LogFile.LogTracing(String.Format("Value of {0} for {1} is valid.", Result, sender.Name), LogLvl.LOG_DEBUG, Me)
                    sender.BackColor = Color.White
                Else
                    LogFile.LogTracing(String.Format("Value of {0} for {1} is invalid.", Result, sender.Name), LogLvl.LOG_ERROR, Me)
                    e.Cancel = True
                    sender.BackColor = Color.Red
                End If
            Else
                e.Cancel = True
                sender.BackColor = Color.Red
            End If
        End If
    End Sub
    Private Sub Correct_IP_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Tb_Server_IP.Validating
        LogFile.LogTracing("Check that the Nut Host address is valid.", LogLvl.LOG_DEBUG, Me)
        Dim Pattern As String
        Dim StrTest As String = sender.Text
        Dim Is_Correct As Boolean = False
        'Test IPV4
        Pattern = "^(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)$"
        If System.Text.RegularExpressions.Regex.IsMatch(sender.Text, Pattern) Then
            Is_Correct = True
            LogFile.LogTracing("The Nut Host address is a valid IPV4 address.", LogLvl.LOG_WARNING, Me)
        End If
        'Test IPV6
        Pattern = "^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$"
        If (System.Text.RegularExpressions.Regex.IsMatch(sender.Text, Pattern) And Not Is_Correct) Then
            Is_Correct = True
            LogFile.LogTracing("The Nut Host address is a valid IPV6 address.", LogLvl.LOG_WARNING, Me)
        End If
        'Test fqdn
        Pattern = "^(?:(?!\d+\.|-)[a-zA-Z0-9_\-]{1,63}(?<!-)\.?)+(?:[a-zA-Z]{2,})$"
        If (System.Text.RegularExpressions.Regex.IsMatch(sender.Text, Pattern) And Not Is_Correct) Then
            Is_Correct = True
            LogFile.LogTracing("The Nut Host address is a valid FQDN address.", LogLvl.LOG_WARNING, Me)
        End If

        'Result
        If Is_Correct Then
            sender.BackColor = Color.White
        Else
            LogFile.LogTracing("The Nut Host address is a invalid", LogLvl.LOG_ERROR, Me)
            e.Cancel = True
            sender.BackColor = Color.Red
        End If
    End Sub
    Private Sub Pref_Gui_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = False
    End Sub

    Private Sub TabControl_Options_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControl_Options.Selecting
        If My.Computer.FileSystem.FileExists(WinNUT_Globals.LogFilePath) Then
            Btn_ViewLog.Enabled = True
            Btn_DeleteLog.Enabled = True
        Else
            Btn_ViewLog.Enabled = False
            Btn_DeleteLog.Enabled = False
        End If
    End Sub

    Private Sub Btn_DeleteLog_Click(sender As Object, e As EventArgs) Handles Btn_DeleteLog.Click
        LogFile.LogTracing("Delete LogFile", LogLvl.LOG_DEBUG, Me)
        If My.Computer.FileSystem.FileExists(WinNUT_Globals.LogFilePath) Then
            WinNUT.LogFile.WriteLog = False
            My.Computer.FileSystem.DeleteFile(WinNUT_Globals.LogFilePath)
            WinNUT.LogFile.WriteLog = WinNUT_Params.Arr_Reg_Key.Item("UseLogFile")
            Btn_ViewLog.Enabled = True
            Btn_DeleteLog.Enabled = True
            LogFile.LogTracing("LogFile Deleted", LogLvl.LOG_DEBUG, Me)
        Else
            LogFile.LogTracing("LogFile does not exists", LogLvl.LOG_WARNING, Me)
            Btn_ViewLog.Enabled = False
            Btn_DeleteLog.Enabled = False
        End If
    End Sub

    Private Sub Btn_ViewLog_Click(sender As Object, e As EventArgs) Handles Btn_ViewLog.Click
        LogFile.LogTracing("Show LogFile", LogLvl.LOG_DEBUG, Me)
        If My.Computer.FileSystem.FileExists(WinNUT_Globals.LogFilePath) Then
            Process.Start(WinNUT_Globals.LogFilePath)
        Else
            LogFile.LogTracing("LogFile does not exists", LogLvl.LOG_WARNING, Me)
            Btn_ViewLog.Enabled = False
            Btn_DeleteLog.Enabled = False
        End If
    End Sub

    Private Sub Pref_Gui_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = WinNUT.Icon
        Me.LogFile = WinNUT.LogFile
        LogFile.LogTracing("Load Pref Gui", LogLvl.LOG_DEBUG, Me)
    End Sub

    Private Sub Event_Ctrl_Value_Changed(sender As Object, e As EventArgs)
        If Me.IsShowed Then
            Me.IsSaved = False
            Me.Btn_Apply.Enabled = True
        End If
    End Sub
End Class
