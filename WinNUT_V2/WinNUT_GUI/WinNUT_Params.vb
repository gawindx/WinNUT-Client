﻿' WinNUT is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Module WinNUT_Params
    Public Arr_Reg_Key As New Dictionary(Of String, Object)
    Private Arr_Reg_Key_Base As New Dictionary(Of String, Dictionary(Of String, Object))
    Public RegBranch As String
    Private RegPath As String
    Private Cryptor As New CryptData()
    Public Sub Init_Params()

        With Arr_Reg_Key
            .Add("ServerAddress", "")
            .Add("Port", 0)
            .Add("UPSName", "")
            .Add("Delay", 0)
            .Add("NutLogin", Cryptor.EncryptData(""))
            .Add("NutPassword", Cryptor.EncryptData(""))
            .Add("AutoReconnect", vbFalse)
            .Add("MinInputVoltage", 0)
            .Add("MaxInputVoltage", 0)
            .Add("FrequencySupply", 1)
            .Add("MinInputFrequency", 0)
            .Add("MaxInputFrequency", 0)
            .Add("MinOutputVoltage", 0)
            .Add("MaxOutputVoltage", 0)
            .Add("MinUPSLoad", 0)
            .Add("MaxUPSLoad", 0)
            .Add("MinBattVoltage", 0)
            .Add("MaxBattVoltage", 0)
            .Add("VarInputVoltage", "input.voltage")
            .Add("VarFrequency", "input.frequency")
            .Add("VarOutputVoltage", "output.voltage")
            .Add("VarBatteryCharge", "battery.charge")
            .Add("VarUPSLoad", "ups.load")
            .Add("VarBattVoltage", "battery.voltage")
            .Add("MinimizeToTray", vbFalse)
            .Add("MinimizeOnStart", vbFalse)
            .Add("CloseToTray", vbFalse)
            .Add("StartWithWindows", vbFalse)
            .Add("UseLogFile", vbFalse)
            .Add("Log Level", 1)
            .Add("ShutdownLimitBatteryCharge", 0)
            .Add("ShutdownLimitUPSRemainTime", 0)
            .Add("ImmediateStopAction", vbFalse)
            .Add("Follow_FSD", vbFalse)
            .Add("TypeOfStop", 1)
            .Add("DelayToShutdown", 0)
            .Add("AllowExtendedShutdownDelay", vbFalse)
            .Add("ExtendedShutdownDelay", 0)
            .Add("VerifyUpdate", vbFalse)
            .Add("VerifyUpdateAtStart", vbFalse)
            .Add("DelayBetweenEachVerification", 1)
            .Add("StableOrDevBranch", 1)
            .Add("LastDateVerification", "")
        End With
        RegBranch = "HKEY_CURRENT_USER\SOFTWARE\"
    End Sub

    Public Sub Load_Params()
        Dim Arr_Reg_Connexion As New Dictionary(Of String, Object)
        Dim Arr_Reg_Calibration As New Dictionary(Of String, Object)
        Dim Arr_Reg_Variables As New Dictionary(Of String, Object)
        Dim Arr_Reg_Miscellanous As New Dictionary(Of String, Object)
        Dim Arr_Reg_Logging As New Dictionary(Of String, Object)
        Dim Arr_Reg_Power As New Dictionary(Of String, Object)
        Dim Arr_Reg_Update As New Dictionary(Of String, Object)


        With Arr_Reg_Connexion
            .Add("ServerAddress", "nutserver host")
            .Add("Port", 3493)
            .Add("UPSName", "UPSName")
            .Add("Delay", 5)
            .Add("NutLogin", Cryptor.EncryptData(""))
            .Add("NutPassword", Cryptor.EncryptData(""))
            .Add("AutoReconnect", vbFalse)
        End With
        With Arr_Reg_Calibration
            .Add("MinInputVoltage", 210)
            .Add("MaxInputVoltage", 270)
            .Add("FrequencySupply", 0)
            .Add("MinInputFrequency", 40)
            .Add("MaxInputFrequency", 60)
            .Add("MinOutputVoltage", 210)
            .Add("MaxOutputVoltage", 250)
            .Add("MinUPSLoad", 0)
            .Add("MaxUPSLoad", 100)
            .Add("MinBattVoltage", 6)
            .Add("MaxBattVoltage", 18)
        End With
        With Arr_Reg_Variables
            .Add("VarInputVoltage", "input.voltage")
            .Add("VarFrequency", "input.frequency")
            .Add("VarOutputVoltage", "output.voltage")
            .Add("VarBatteryCharge", "battery.charge")
            .Add("VarUPSLoad", "ups.load")
            .Add("VarBattVoltage", "battery.voltage")
        End With
        With Arr_Reg_Miscellanous
            .Add("MinimizeToTray", vbFalse)
            .Add("MinimizeOnStart", vbFalse)
            .Add("CloseToTray", vbFalse)
            .Add("StartWithWindows", vbFalse)
        End With
        With Arr_Reg_Logging
            .Add("UseLogFile", vbFalse)
            .Add("Log Level", 0)
        End With
        With Arr_Reg_Power
            .Add("ShutdownLimitBatteryCharge", 30)
            .Add("ShutdownLimitUPSRemainTime", 120)
            .Add("ImmediateStopAction", vbFalse)
            .Add("Follow_FSD", vbFalse)
            .Add("TypeOfStop", 0)
            .Add("DelayToShutdown", 15)
            .Add("AllowExtendedShutdownDelay", vbFalse)
            .Add("ExtendedShutdownDelay", 15)
        End With
        With Arr_Reg_Update
            .Add("VerifyUpdate", vbFalse)
            .Add("VerifyUpdateAtStart", vbFalse)
            .Add("DelayBetweenEachVerification", 2)
            .Add("StableOrDevBranch", 0)
            .Add("LastDateVerification", "")
        End With
        With Arr_Reg_Key_Base
            .Add("Connexion", Arr_Reg_Connexion)
            .Add("Appareance", Arr_Reg_Miscellanous)
            .Add("Calibration", Arr_Reg_Calibration)
            .Add("Variables", Arr_Reg_Variables)
            .Add("Power", Arr_Reg_Power)
            .Add("Logging", Arr_Reg_Logging)
            .Add("Update", Arr_Reg_Update)
        End With

        'Verify if non encoded Login/Password Exist
        'if not, create it
        Dim WinnutConnRegPath = WinNUT_Params.RegBranch & "WinNUT\Connexion"
        Dim LoginValue = My.Computer.Registry.GetValue(WinnutConnRegPath, "NutLogin", "")
        Dim PasswordValue = My.Computer.Registry.GetValue(WinnutConnRegPath, "NutPassword", "")
        If (LoginValue Is Nothing) Or Not (Cryptor.IsCryptedtData(LoginValue)) Then
            My.Computer.Registry.SetValue(WinnutConnRegPath, "NutLogin", Cryptor.EncryptData(LoginValue))
        End If

        If (PasswordValue Is Nothing) Or Not (Cryptor.IsCryptedtData(PasswordValue)) Then
            My.Computer.Registry.SetValue(WinnutConnRegPath, "NutPassword", Cryptor.EncryptData(PasswordValue))
        End If

        'Read Data from registry
        For Each RegKeys As KeyValuePair(Of String, Dictionary(Of String, Object)) In Arr_Reg_Key_Base
            For Each RegValue As KeyValuePair(Of String, Object) In RegKeys.Value
                RegPath = "WinNUT\" & RegKeys.Key
                If My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, Nothing) Is Nothing Then
                    My.Computer.Registry.CurrentUser.CreateSubKey(RegPath)
                    My.Computer.Registry.SetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
                End If
                If (RegValue.Key = "NutLogin" Or RegValue.Key = "NutPassword") Then
                    Dim WinReg = My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
                    If String.IsNullOrEmpty(WinReg) Or Not (Cryptor.IsCryptedtData(WinReg)) Then
                        WinReg = Cryptor.EncryptData("")
                    End If
                    WinNUT_Params.Arr_Reg_Key.Item(RegValue.Key) = Cryptor.DecryptData(WinReg)
                Else
                    WinNUT_Params.Arr_Reg_Key.Item(RegValue.Key) = My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
                End If
            Next
        Next
    End Sub

    Public Function Save_Params() As Boolean
        Dim Cryptor As New CryptData()
        Dim Result As Boolean = False
        Try
            For Each RegKeys As KeyValuePair(Of String, Dictionary(Of String, Object)) In Arr_Reg_Key_Base
                For Each RegValue As KeyValuePair(Of String, Object) In RegKeys.Value
                    RegPath = "WinNUT\" & RegKeys.Key
                    If My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, Nothing) Is Nothing Then
                        My.Computer.Registry.CurrentUser.CreateSubKey(RegPath)
                        My.Computer.Registry.SetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
                    Else
                        If (RegValue.Key = "NutLogin" Or RegValue.Key = "NutPassword") Then
                            My.Computer.Registry.SetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, Cryptor.EncryptData(WinNUT_Params.Arr_Reg_Key.Item(RegValue.Key)))
                        Else
                            My.Computer.Registry.SetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, WinNUT_Params.Arr_Reg_Key.Item(RegValue.Key))
                        End If

                    End If
                Next
            Next
            Result = True
        Catch ex As Exception
            Result = False
        End Try
        Return Result
    End Function
    Public Function ImportIni(ByVal FileName As String) As Boolean
        Dim Result = False
        Try
            Dim FileIni As IniFile = New IniFile()
            FileIni.Load(FileName)
            Dim Old_Ini_Key As New Dictionary(Of String, String)
            With Old_Ini_Key
                .Add("Server address", "ServerAddress")
                .Add("Port", "Port")
                .Add("UPS name", "UPSName")
                .Add("Delay", "Delay")
                .Add("AutoReconnect", "AutoReconnect")
                .Add("Min Input Voltage", "MinInputVoltage")
                .Add("Max Input Voltage", "MaxInputVoltage")
                .Add("Frequency Supply", "FrequencySupply")
                .Add("Min Input Frequency", "MinInputFrequency")
                .Add("Max Input Frequency", "MaxInputFrequency")
                .Add("Min Output Voltage", "MinOutputVoltage")
                .Add("Max Output Voltage", "MaxOutputVoltage")
                .Add("Min UPS Load", "MinUPSLoad")
                .Add("Max UPS Load", "MaxUPSLoad")
                .Add("Min Batt Voltage", "MinBattVoltage")
                .Add("Max Batt Voltage", "MaxBattVoltage")
                .Add("Minimize to tray", "MinimizeToTray")
                .Add("Minimize on Start", "MinimizeOnStart")
                .Add("Close to tray", "CloseToTray")
                .Add("Start with Windows", "StartWithWindows")
                .Add("Use Log File", "UseLogFile")
                .Add("Log Level", "Log Level")
                .Add("Shutdown Limit Battery Charge", "ShutdownLimitBatteryCharge")
                .Add("Shutdown Limit UPS Remain Time", "ShutdownLimitUPSRemainTime")
                .Add("Immediate stop action", "ImmediateStopAction")
                .Add("Type Of Stop", "TypeOfStop")
                .Add("Delay To Shutdown", "DelayToShutdown")
                .Add("Allow Extended Shutdown Delay", "AllowExtendedShutdownDelay")
                .Add("Extended Shutdown Delay", "ExtendedShutdownDelay")
                .Add("Verify Update", "VerifyUpdate")
                .Add("Verify Update At Start", "VerifyUpdateAtStart")
                .Add("Delay Between Each Verification", "DelayBetweenEachVerification")
                .Add("Stable Or Dev Branch", "StableOrDevBranch")
                .Add("Last Date Verification", "LastDateVerification")
                .Add("Default Language", "")
                .Add("Language", "")
                .Add("Clocks Color", "")
                .Add("Panel Color", "")
            End With
            With Arr_Reg_Key
                For Each IniSection In FileIni.Sections
                    For Each k In IniSection.keys
                        Dim newkey = Old_Ini_Key.Item(k.name)
                        If newkey <> "" Then
                            Select Case k.name
                                Case "Port", "Delay", "Min Input Voltage", "Max Input Voltage", "Min Input Frequency",
                                     "Max Input Frequency", "Min Output Voltage", "Max Output Voltage", "Min UPS Load", "Max UPS Load",
                                     "Min Batt Voltage", "Max Batt Voltage", "Shutdown Limit Battery Charge", "Shutdown Limit UPS Remain Time", "Delay To Shutdown"
                                    .Item(newkey) = CInt(k.value)
                                Case "Frequency Supply"
                                    If CInt(k.Value) = 50 Then
                                        .Item(newkey) = 0
                                    Else
                                        .Item(newkey) = 1
                                    End If
                                Case "Last Date Verification"
                                    .Item(newkey) = Convert.ToDateTime(k.value).ToString()
                                Case "AutoReconnect", "Minimize To Tray", "Minimize On Start", "Close To Tray", "Start With Windows", "Use Log File",
                                     "Immediate Stop Action", "Allow Extended Shutdown Delay", "Verify Update", "Verify Update At Start"
                                    If k.value = 0 Then
                                        .Item(newkey) = vbFalse
                                    Else
                                        .Item(newkey) = vbTrue
                                    End If
                                Case "Log Level"
                                    Select Case k.Value
                                        Case 1
                                            .Item(newkey) = 0
                                        Case 2
                                            .Item(newkey) = 1
                                        Case 4
                                            .Item(newkey) = 2
                                        Case 8
                                            .Item(newkey) = 3
                                    End Select
                                Case "Type Of Stop"
                                    Select Case k.value
                                        Case 17
                                            .Item(newkey) = 0
                                        Case 32
                                            .Item(newkey) = 1
                                        Case 64
                                            .Item(newkey) = 2
                                    End Select
                                Case "Delay Between Each Verification", "Stable Or Dev Branch"
                                    .Item(newkey) = CInt(k.value) - 1
                                Case Else
                                    .Item(newkey) = k.value
                            End Select
                        End If
                    Next
                Next
            End With
            If Save_Params() Then
                Result = True
            End If
        Catch ex As Exception
            Result = False
        End Try
        Return Result
    End Function

End Module
