Public Module WinNUT_Params
    Public Arr_Reg_Key As New Dictionary(Of String, Object)
    Private Arr_Reg_Key_Base As New Dictionary(Of String, Dictionary(Of String, Object))
    Public RegBranch As String
    Private RegPath As String
    Public Sub Init_Params()
        With Arr_Reg_Key
            .Add("ServerAddress", "")
            .Add("Port", 0)
            .Add("UPSName", "")
            .Add("Delay", 0)
            .Add("NutLogin", "")
            .Add("NutPassword", "")
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
            .Add("MinimizeToTray", vbFalse)
            .Add("MinimizeOnStart", vbFalse)
            .Add("CloseToTray", vbFalse)
            .Add("StartWithWindows", vbFalse)
            .Add("UseLogFile", vbFalse)
            .Add("Log Level", 1)
            .Add("ShutdownLimitBatteryCharge", 0)
            .Add("ShutdownLimitUPSRemainTime", 0)
            .Add("ImmediateStopAction", vbFalse)
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
        Dim Arr_Reg_Miscellanous As New Dictionary(Of String, Object)
        Dim Arr_Reg_Logging As New Dictionary(Of String, Object)
        Dim Arr_Reg_Power As New Dictionary(Of String, Object)
        Dim Arr_Reg_Update As New Dictionary(Of String, Object)

        With Arr_Reg_Connexion
            .Add("ServerAddress", "nutserver host")
            .Add("Port", 3493)
            .Add("UPSName", "UPSName")
            .Add("Delay", 5000)
            .Add("NutLogin", "")
            .Add("NutPassword", "")
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
            .Add("Power", Arr_Reg_Power)
            .Add("Logging", Arr_Reg_Logging)
            .Add("Update", Arr_Reg_Update)
        End With

        For Each RegKeys As KeyValuePair(Of String, Dictionary(Of String, Object)) In Arr_Reg_Key_Base
            For Each RegValue As KeyValuePair(Of String, Object) In RegKeys.Value
                RegPath = "WinNUT\" & RegKeys.Key
                If My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, Nothing) Is Nothing Then
                    My.Computer.Registry.CurrentUser.CreateSubKey(RegPath)
                    My.Computer.Registry.SetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
                End If
                WinNUT_Params.Arr_Reg_Key.Item(RegValue.Key) = My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
            Next
        Next
    End Sub

    Public Function Save_Params() As Boolean
        Dim Result As Boolean = False
        Try
            For Each RegKeys As KeyValuePair(Of String, Dictionary(Of String, Object)) In Arr_Reg_Key_Base
                For Each RegValue As KeyValuePair(Of String, Object) In RegKeys.Value
                    RegPath = "WinNUT\" & RegKeys.Key
                    If My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, Nothing) Is Nothing Then
                        My.Computer.Registry.CurrentUser.CreateSubKey(RegPath)
                        My.Computer.Registry.SetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
                    Else
                        My.Computer.Registry.SetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, WinNUT_Params.Arr_Reg_Key.Item(RegValue.Key))
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
