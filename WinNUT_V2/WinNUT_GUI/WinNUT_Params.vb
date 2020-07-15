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
                Else
                    WinNUT_Params.Arr_Reg_Key.Item(RegValue.Key) = My.Computer.Registry.GetValue(WinNUT_Params.RegBranch & RegPath, RegValue.Key, RegValue.Value)
                End If
            Next
        Next
    End Sub

    Public Sub Save_Params()
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
    End Sub
End Module
