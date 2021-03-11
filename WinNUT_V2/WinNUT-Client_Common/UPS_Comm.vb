' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Class UPS_Comm
    Private LogFile As Logger
    Private ConnectionStatus As Boolean = False
    Private Server As String
    Private Port As Integer
    Private UPSName As String
    Private Delay As Integer
    Private Login As String
    Private Password As String
    Private Mfr As String
    Private Model As String
    Private Serial As String
    Private Firmware As String
    Private BattCh As Double
    Private BattV As Double
    Private BattRuntime As Double
    Private BattCapacity As Double
    Private PowerFreq As Double
    Private InputV As Double
    Private OutputV As Double
    Private Load As Double
    Private Status As String
    Private OutPower As Double
    Private InputA As Double
    Private Low_Batt As Integer
    Private Low_Backup As Integer
    Private LConnect As Boolean = False
    Private AReconnect As Boolean = False
    Private MaxRetry As Integer = 30
    Private Retry As Integer = 0
    Private ErrorStatus As Boolean = False
    Private ErrorMsg As String = ""
    Private Update_Nut As New System.Windows.Forms.Timer
    Private Reconnect_Nut As New System.Windows.Forms.Timer
    Private NutSocket As System.Net.Sockets.Socket
    Private NutTCP As System.Net.Sockets.TcpClient
    Private NutStream As System.Net.Sockets.NetworkStream
    Private ReaderStream As System.IO.StreamReader
    Private WriterStream As System.IO.StreamWriter
    Private ShutdownStatus As Boolean = False
    Private Follow_FSD As Boolean = False
    Private Unknown_UPS_Name As Boolean = False
    Private Invalid_Data As Boolean = False
    Private Invalid_Auth_Data As Boolean = False
    Private Const CosPhi As Double = 0.6

    Public Event Unknown_UPS()
    Public Event LostConnect()
    Public Event Connected()
    Public Event DataUpdated()
    Public Event NewRetry()
    Public Event Deconnected()
    Public Event Shutdown_Condition()
    Public Event Stop_Shutdown()
    Public Event InvalidLogin()

    Public Sub New(ByRef LogFile As Logger)
        Me.Server = ""
        Me.UPSName = ""
        Me.Port = 0
        Me.Delay = 0
        Me.LogFile = LogFile

        Update_Nut.Interval = 1000
        Update_Nut.Enabled = False
        AddHandler Update_Nut.Tick, AddressOf Retrieve_UPS_Data

        Reconnect_Nut.Interval = 30000
        Reconnect_Nut.Enabled = False
        AddHandler Reconnect_Nut.Tick, AddressOf Reconnect_UPS
    End Sub
    Public Property NutHost() As String
        Get
            Return Me.Server
        End Get
        Set(ByVal Value As String)
            Me.Server = Value
        End Set
    End Property
    Public Property NutUPS() As String
        Get
            Return Me.UPSName
        End Get
        Set(ByVal Value As String)
            Me.UPSName = Value
        End Set
    End Property

    Public Property NutPort() As Integer
        Get
            Return Me.Port
        End Get
        Set(ByVal Value As Integer)
            Me.Port = Value
        End Set
    End Property

    Public Property NutDelay() As Integer
        Get
            Return Me.Delay
        End Get
        Set(ByVal Value As Integer)
            Me.Delay = Value
        End Set
    End Property

    Public Property NutLogin() As String
        Get
            Return Me.Login
        End Get
        Set(ByVal Value As String)
            Me.Login = Value
        End Set
    End Property

    Public Property NutPassword() As String
        Get
            Return Me.Password
        End Get
        Set(ByVal Value As String)
            Me.Password = Value
        End Set
    End Property
    Public Property AutoReconnect() As Boolean
        Get
            Return Me.AReconnect
        End Get
        Set(ByVal Value As Boolean)
            Me.AReconnect = Value
        End Set
    End Property
    Public ReadOnly Property IsConnected() As Boolean
        Get
            Return Me.ConnectionStatus
        End Get
        'Set(ByVal Value As Boolean)
        '    Me.ConnectionStatus = Value
        'End Set
    End Property
    Public Property UPS_Mfr() As String
        Get
            Return Me.Mfr
        End Get
        Set(ByVal Value As String)
            Me.Mfr = Value
        End Set
    End Property
    Public Property UPS_Model() As String
        Get
            Return Me.Model
        End Get
        Set(ByVal Value As String)
            Me.Model = Value
        End Set
    End Property
    Public Property UPS_Serial() As String
        Get
            Return Me.Serial
        End Get
        Set(ByVal Value As String)
            Me.Serial = Value
        End Set
    End Property
    Public Property UPS_Firmware() As String
        Get
            Return Me.Firmware
        End Get
        Set(ByVal Value As String)
            Me.Firmware = Value
        End Set
    End Property

    Public Property UPS_BattCh() As Double
        Get
            Return Me.BattCh
        End Get
        Set(ByVal Value As Double)
            Me.BattCh = Value
        End Set
    End Property
    Public Property UPS_BattV() As Double
        Get
            Return Me.BattV
        End Get
        Set(ByVal Value As Double)
            Me.BattV = Value
        End Set
    End Property
    Public Property UPS_BattRuntime() As Double
        Get
            Return Me.BattRuntime
        End Get
        Set(ByVal Value As Double)
            Me.BattRuntime = Value
        End Set
    End Property
    Public Property UPS_BattCapacity() As Double
        Get
            Return Me.BattCapacity
        End Get
        Set(ByVal Value As Double)
            Me.BattCapacity = Value
        End Set
    End Property
    Public Property UPS_PowerFreq() As Double
        Get
            Return Me.PowerFreq
        End Get
        Set(ByVal Value As Double)
            Me.PowerFreq = Value
        End Set
    End Property
    Public Property UPS_InputV() As Double
        Get
            Return Me.InputV
        End Get
        Set(ByVal Value As Double)
            Me.InputV = Value
        End Set
    End Property
    Public Property UPS_OutputV() As Double
        Get
            Return Me.OutputV
        End Get
        Set(ByVal Value As Double)
            Me.OutputV = Value
        End Set
    End Property
    Public Property UPS_Load() As Double
        Get
            Return Me.Load
        End Get
        Set(ByVal Value As Double)
            Me.Load = Value
        End Set
    End Property
    Public Property UPS_Status() As String
        Get
            Return Me.Status
        End Get
        Set(ByVal Value As String)
            Me.Status = Value
        End Set
    End Property
    Public Property UPS_OutPower() As Double
        Get
            Return Me.OutPower
        End Get
        Set(ByVal Value As Double)
            Me.OutPower = Value
        End Set
    End Property
    Public Property UPS_InputA() As Double
        Get
            Return Me.InputA
        End Get
        Set(ByVal Value As Double)
            Me.InputA = Value
        End Set
    End Property
    Public Property UPS_Retry() As Integer
        Get
            Return Me.Retry
        End Get
        Set(ByVal Value As Integer)
            Me.Retry = Value
        End Set
    End Property
    Public Property UPS_MaxRetry() As Integer
        Get
            Return Me.MaxRetry
        End Get
        Set(ByVal Value As Integer)
            Me.MaxRetry = Value
        End Set
    End Property

    Public Property Battery_Limit() As Integer
        Get
            Return Me.Low_Batt
        End Get
        Set(ByVal Value As Integer)
            Me.Low_Batt = Value
        End Set
    End Property

    Public Property UPS_Follow_FSD() As Boolean
        Get
            Return Me.Follow_FSD
        End Get
        Set(ByVal Value As Boolean)
            Me.Follow_FSD = Value
        End Set
    End Property

    Public Property Backup_Limit() As Integer
        Get
            Return Me.Low_Backup
        End Get
        Set(ByVal Value As Integer)
            Me.Low_Backup = Value
        End Set
    End Property

    Public ReadOnly Property HasError As Boolean
        Get
            Return Me.ErrorStatus
        End Get
    End Property

    Private Function Create_Socket() As Boolean
        Try
            Me.NutSocket = New System.Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.ProtocolType.IP)
            Me.NutTCP = New System.Net.Sockets.TcpClient(Me.Server, Me.Port)
            Me.NutStream = NutTCP.GetStream
            Me.ReaderStream = New IO.StreamReader(NutStream)
            Me.WriterStream = New IO.StreamWriter(NutStream)
            Me.ConnectionStatus = True
        Catch Excep As Exception
            Me.ConnectionStatus = False
        End Try
        Return Me.ConnectionStatus
    End Function

    Private Function Close_Socket() As Boolean
        Try
            Me.WriterStream.Close()
            Me.ReaderStream.Close()
            Me.NutStream.Close()
            Me.NutTCP.Close()
            Me.NutSocket.Close()
        Catch Excep As Exception
        End Try
        Me.ConnectionStatus = False
        Return Nothing
    End Function

    Public Function Query_Data(Query_Msg As String) As (Data As String, Response As NUTResponse)
        Dim Response As NUTResponse
        Dim DataResult As String = ""
        Try
            Me.WriterStream.WriteLine(Query_Msg) 'TODO: Move to generic request/response functions?
            Me.WriterStream.Flush()
            Threading.Thread.Sleep(50)
            DataResult = Me.ReaderStream.ReadLine()

            If DataResult = Nothing Then
                Throw New System.Exception("Connection to Nut Host seem broken when querying : " & Query_Msg)
            End If
            ' Parse and enumerate a NUT protocol response.
            ' Remove hyphens to prepare for parsing.
            Dim SanitisedString = DataResult.Replace("-", String.Empty)
            ' Break the response down so we can get specifics.
            Dim SplitString = SanitisedString.Split(" "c)

            Select Case SplitString(0)
                Case "OK", "VAR", "BEGIN", "DESC"
                    Response = NUTResponse.OK
                Case "ERR"
                    Response = DirectCast([Enum].Parse(GetType(NUTResponse), SplitString(1)), NUTResponse)
                Case Else
                    ' We don't recognize the response, throw an error.
                    Throw New Exception("Unknown response from NUT server: " & Response)
            End Select
            Return (DataResult, Response)
        Catch Excep As Exception
            Return (DataResult, Response)
        End Try
    End Function


    '' Parse and enumerate a NUT protocol response.
    'Function EnumResponse(ByVal Response As String) As NUTResponse
    '    ' Remove hyphens to prepare for parsing.
    '    Dim SanitisedString = Response.Replace("-", String.Empty)
    '    ' Break the response down so we can get specifics.
    '    Dim SplitString = SanitisedString.Split(" "c)

    '    Select Case SplitString(0)
    '        Case "OK", "VAR", "BEGIN", "DESC"
    '            Return NUTResponse.OK
    '        Case "ERR"
    '            Return DirectCast([Enum].Parse(GetType(NUTResponse), SplitString(1)), NUTResponse)
    '        Case Else
    '            ' We don't recognize the response, throw an error.
    '            Throw New Exception("Unknown response from NUT server: " & Response)
    '    End Select
    'End Function
    Public Function Connect() As Object
        Me.ErrorStatus = False
        Try
            'TODO: Use LIST UPS protocol command to get valid UPSs.
            If Me.Server <> "" And Me.Port <> 0 And Me.Delay <> 0 And Me.UPSName <> "" Then
                If Not Create_Socket() Then
                    Throw New System.Exception("Cannot Create Connection to Nut Server.")
                End If
                GetUPSProductInfo()
                If Me.Unknown_UPS_Name Then 'TODO: Use LIST UPS protocol command to get valid UPSs.
                    Throw New System.Exception("Unknown UPS Name.")
                ElseIf Me.Invalid_Data Then
                    Throw New System.Exception("The Server Nut is accessible but the data cannot be recovered.")
                End If
                Update_Nut.Interval = Me.Delay
                Update_Nut.Start()
                Me.LConnect = False
                RaiseEvent Connected()
                Retrieve_UPS_Data(Nothing, Nothing)
                Return Nothing
            Else
                Return New System.Exception("Error On Connect Function To Nut")
            End If
        Catch Excep As Exception
            Me.ErrorStatus = True
            Enter_Reconnect_Process(Excep, "Error When Connection to Nut Host : ")
            Return False
        End Try
    End Function

    Public Sub AuthLogin()
        Try
            'Dim SendData = "USERNAME " & Me.Login & vbCr 'TODO: Is carriage return necessary?
            'Me.WriterStream.WriteLine(SendData) 'TODO: Move to generic request/response functions?
            'Me.WriterStream.Flush()
            'Dim DataResult As String = Me.ReaderStream.ReadLine()
            'Dim NUTResult = EnumResponse(DataResult)
            'If NUTResult <> NUTResponse.OK Then
            '    If NUTResult = NUTResponse.INVALIDUSERNAME Then
            '        Me.Invalid_Auth_Data = True
            '        RaiseEvent InvalidLogin()
            '        Throw New Exception("Invalid Username.")
            '    ElseIf NUTResult = NUTResponse.ACCESSDENIED Then
            '        Throw New Exception("Access is denied.")
            '    Else
            '        Me.AReconnect = False
            '        Throw New Exception("Unhandled login error: " & DataResult)
            '    End If
            'End If

            Me.Invalid_Auth_Data = False
            Dim Nut_Query = Query_Data("USERNAME " & Me.Login & vbCr)

            If Nut_Query.Response <> NUTResponse.OK Then
                If Nut_Query.Response = NUTResponse.INVALIDUSERNAME Then
                    Me.Invalid_Auth_Data = True
                    RaiseEvent InvalidLogin()
                    Throw New Exception("Invalid Username.")
                ElseIf Nut_Query.Response = NUTResponse.ACCESSDENIED Then
                    Throw New Exception("Access is denied.")
                Else
                    Me.AReconnect = False
                    Throw New Exception("Unhandled login error: " & Nut_Query.Data)
                End If
            End If

            'SendData = "PASSWORD " & Me.Password & vbCr
            'Me.WriterStream.WriteLine(SendData)
            'Me.WriterStream.Flush()
            'DataResult = Me.ReaderStream.ReadLine()
            'NUTResult = EnumResponse(DataResult)

            'If NUTResult <> NUTResponse.OK Then
            '    If NUTResult = NUTResponse.INVALIDPASSWORD Then
            '        Me.Invalid_Auth_Data = True
            '        RaiseEvent InvalidLogin()
            '        Throw New Exception("Invalid Password.")
            '    ElseIf NUTResult = NUTResponse.ACCESSDENIED Then
            '        Throw New Exception("Access is denied.")
            '    Else
            '        Me.AReconnect = False
            '        Throw New Exception("Unhandled login error: " & DataResult)
            '    End If
            'End If
            Nut_Query = Query_Data("PASSWORD " & Me.Password & vbCr)

            If Nut_Query.Response <> NUTResponse.OK Then
                If Nut_Query.Response = NUTResponse.INVALIDPASSWORD Then
                    Me.Invalid_Auth_Data = True
                    RaiseEvent InvalidLogin()
                    Throw New Exception("Invalid Password.")
                ElseIf Nut_Query.Response = NUTResponse.ACCESSDENIED Then
                    Throw New Exception("Access is denied.")
                Else
                    Me.AReconnect = False
                    Throw New Exception("Unhandled login error: " & Nut_Query.Data)
                End If
            End If
            'Return True
            'Return Nothing
        Catch Excep As Exception
            Me.Invalid_Auth_Data = True
            Me.ErrorStatus = True
            Me.Disconnect(True)
            'LogFile.LogTracing(Excep.Message, LogLvl.LOG_ERROR, Me, String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_CON_FAILED), Me.Server, Me.Port, Excep.Message))
            'Return False
            'Return Excep
        End Try
        'Return New System.Exception("Error on AuthLogin Function")
    End Sub

    Public Sub Enter_Reconnect_Process(ByVal Excep As Exception, ByVal Message As String)
        'Me.ConnectionStatus = False
        If Not Me.Unknown_UPS_Name And Not Me.Invalid_Auth_Data Then
            'LogFile.LogTracing(Message & Excep.Message, LogLvl.LOG_ERROR, Me, String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_CON_FAILED), Me.Server, Me.Port, Excep.Message))
            Me.LConnect = True
            If Me.AReconnect Then
                LogFile.LogTracing("Autoreconnect Enable. Run Autoreconnect Process", LogLvl.LOG_DEBUG, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_CON_RETRY))
                Reconnect_Nut.Enabled = True
                Reconnect_Nut.Start()
                Update_Nut.Stop()
                Update_Nut.Enabled = False
                RaiseEvent LostConnect()
            End If
        End If
    End Sub

    Public Sub Disconnect(Optional ByVal ForceDisconnect = False, Optional ByVal DisableAutoRetry = False)
        'If Not Me.ConnectionStatus Or ForceDisconnect Then
        If Not Me.IsConnected Or ForceDisconnect Then
            If Not Me.Unknown_UPS_Name And Not Me.Invalid_Auth_Data Then
                'LogFile.LogTracing("Deconnect from Nut Host", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_LOGOFF))
            End If
            If ForceDisconnect Then
                'LogFile.LogTracing("Force Disconnect", LogLvl.LOG_WARNING, Me)
            Else
                ' LogFile.LogTracing("Normal Disconnect", LogLvl.LOG_WARNING, Me)
            End If
            'Me.ConnectionStatus = False

            'If Me.WriterStream IsNot Nothing Then
            'Me.WriterStream.Close()
            'End If
            'If Me.ReaderStream IsNot Nothing Then
            'Me.ReaderStream.Close()
            'End If
            'If Me.NutStream IsNot Nothing Then
            'Me.NutStream.Close()
            'End If

            Update_Nut.Stop()
            Me.Mfr = ""
            Me.Model = ""
            Me.Serial = ""
            Me.Firmware = ""
            Update_Nut.Enabled = False
            If Not Me.Unknown_UPS_Name And Not Me.Invalid_Auth_Data Then
                If Not DisableAutoRetry Then
                    If Me.AReconnect Then
                        RaiseEvent NewRetry()
                    Else
                        RaiseEvent Deconnected()
                    End If
                End If
            End If
            Close_Socket()
        End If
    End Sub

    Public Function GetUPSVar(ByVal varNAme As String, Optional ByVal Fallback_value As Object = "", Optional ByVal post_send_delay As Integer = vbNull) As String
        Try
            'LogFile.LogTracing("Enter GetUPSVar", LogLvl.LOG_DEBUG, Me)
            'If Not Me.ConnectionStatus Then
            If Not Me.IsConnected Then
                Throw New System.Exception("Connection to Nut Host seem broken when Retrieving " & varNAme)
                Return Nothing
            Else
                Dim Nut_Query = Query_Data("GET VAR " & Me.UPSName & " " & varNAme & vbCr)

                Select Case Nut_Query.Response
                    Case NUTResponse.OK
                        Me.Unknown_UPS_Name = False
                        Me.Invalid_Data = False
                        LogFile.LogTracing("Process Result With " & varNAme & " : " & Nut_Query.Data, LogLvl.LOG_DEBUG, Me)
                        Return ProcessData(Nut_Query.Data)
                    Case NUTResponse.UNKNOWNUPS
                        Me.Invalid_Data = False
                        Me.Unknown_UPS_Name = True
                        RaiseEvent Unknown_UPS()
                        Throw New System.Exception("Unknown UPS Name.")
                        Return Nothing
                    Case NUTResponse.VARNOTSUPPORTED
                        Me.Unknown_UPS_Name = False
                        Me.Invalid_Data = False
                        If Not String.IsNullOrEmpty(Fallback_value) Then
                            LogFile.LogTracing("Apply Fallback Value when retrieving " & varNAme, LogLvl.LOG_WARNING, Me)
                            Dim FakeData = "VAR " & Me.UPSName & " " & varNAme & " " & """" & Fallback_value & """"
                            Return ProcessData(FakeData)
                        Else
                            LogFile.LogTracing("Error Result On Retrieving  " & varNAme & " : " & Nut_Query.Data, LogLvl.LOG_ERROR, Me)
                            Return Nothing
                        End If
                    Case NUTResponse.DATASTALE
                        Me.Invalid_Data = True
                        LogFile.LogTracing("Error Result On Retrieving  " & varNAme & " : " & Nut_Query.Data, LogLvl.LOG_ERROR, Me)
                        Throw New System.Exception(varNAme & " : " & Nut_Query.Data)
                        Return Nothing
                    Case Else
                        Return Nothing
                End Select
            End If
        Catch Excep As Exception
            Me.Disconnect(True)
            Return Nothing
        End Try
    End Function

    Public Function GetUPSDescVar(ByVal VarName As String) As String
        Try
            LogFile.LogTracing("Enter GetUPSDescVar", LogLvl.LOG_DEBUG, Me)
            'If Not Me.ConnectionStatus Then
            If Not Me.IsConnected Then
                Throw New System.Exception("Connection to Nut Host seem broken when Retrieving " & VarName)
            Else
                Dim Nut_Query = Query_Data("GET DESC " & Me.UPSName & " " & VarName & vbCr)
                Select Case Nut_Query.Response
                    Case NUTResponse.OK
                        LogFile.LogTracing("Process Result With " & VarName & " : " & Nut_Query.Data, LogLvl.LOG_DEBUG, Me)
                        Return ProcessData(Nut_Query.Data)
                    Case Else
                        LogFile.LogTracing("Error Result On Retrieving  " & VarName & " : " & Nut_Query.Data, LogLvl.LOG_ERROR, Me)
                        Return Nothing
                End Select
            End If
        Catch Excep As Exception
            Me.Disconnect(True)
            Return Nothing
        End Try
    End Function

    Public Function ListUPSVars() As List(Of UPS_Var_Node)
        Dim List_Datas As New List(Of String)
        Try
            LogFile.LogTracing("Enter GetUPSDescVar", LogLvl.LOG_DEBUG, Me)
            'If Not Me.ConnectionStatus Then
            If Not Me.IsConnected Then
                Throw New System.Exception("Connection to Nut Host seem broken when Retrieving ListUPSVars")
                Return Nothing
            Else
                'Dim Nut_Query = Query_Data("LIST VAR " & Me.UPSName & vbCr)
                Dim SendData = "LIST VAR " & Me.UPSName & vbCr
                Me.WriterStream.WriteLine(SendData)
                Me.WriterStream.Flush()
                Threading.Thread.Sleep(500)
                Dim DataResult As String = ""
                Dim start As DateTime = DateTime.Now
                While DateTime.Now.Subtract(start).Seconds < 30
                    DataResult = Me.ReaderStream.ReadLine()
                    If DataResult = Nothing Or InStr(DataResult, "END LIST") <> 0 Then
                        Exit While
                    Else
                        Dim NUTResult = EnumResponse(DataResult)
                        If NUTResult <> NUTResponse.OK Then
                            LogFile.LogTracing("Error Result On Retrieving LIST VAR " & Me.UPSName & " : " & DataResult, LogLvl.LOG_ERROR, Me)
                            Return Nothing
                        End If
                        List_Datas.Add(DataResult)
                    End If
                End While
                If List_Datas.Count = 0 Then
                    Throw New System.Exception("Connection to Nut Host seem broken when Retrieving LIST VAR " & Me.UPSName)
                End If
                Dim List_Result As New List(Of UPS_Var_Node)
                Dim Key As String
                Dim Value As String
                For Each Line In List_Datas
                    If InStr(Line, "LIST") = 0 Then
                        Dim ArrayStr = Strings.Split(Line, " ")
                        Key = Strings.Replace(ArrayStr(2), """", "")
                        Value = Strings.Replace(ArrayStr(3), """", "")
                        List_Result.Add(New UPS_Var_Node With {.VarKey = Key, .VarValue = Value, .VarDesc = GetUPSDescVar(Key)})
                    End If
                Next
                Return List_Result
            End If
        Catch Excep As Exception
            Me.Disconnect(True)
            Enter_Reconnect_Process(Excep, "Error When Retieving ListUPSVars : ")
            Return Nothing
        End Try
    End Function

    Private Sub GetUPSProductInfo()
        Me.Mfr = GetUPSVar("ups.mfr", "Unknown")
        'If Me.ConnectionStatus Then
        If Me.IsConnected Then
            Me.Model = GetUPSVar("ups.model", "Unknown")
            Me.Serial = GetUPSVar("ups.serial", "Unknown")
            Me.Firmware = GetUPSVar("ups.firmware", "Unknown")
        End If
    End Sub

    Private Function ProcessData(ByVal UPSData)
        Dim StrArray = Strings.Split(UPSData, """")

        If StrArray.Count < 2 Then
            'Me.ConnectionStatus = False
        End If
        Return Strings.Trim(StrArray(1))
    End Function

    Private Sub Retrieve_UPS_Data(sender As Object, e As EventArgs)
        Dim ciClone As System.Globalization.CultureInfo = CType(System.Globalization.CultureInfo.InvariantCulture.Clone(), System.Globalization.CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."
        Try
            Static Freq_Fallback As Double
            LogFile.LogTracing("Enter Retrieve_UPS_Data", LogLvl.LOG_DEBUG, Me)
            If Not Me.LConnect Then

                If Freq_Fallback = 0 Then
                    Freq_Fallback = Double.Parse(GetUPSVar("output.frequency.nominal", (50 + CInt(WinNUT_Params.Arr_Reg_Key.Item("FrequencySupply")) * 10)), ciClone)
                End If

                Me.BattCh = Double.Parse(GetUPSVar("battery.charge", 255, 50), ciClone)
                Me.BattV = Double.Parse(GetUPSVar("battery.voltage", 12), ciClone)
                Me.BattRuntime = Double.Parse(GetUPSVar("battery.runtime", 86400), ciClone)
                Me.BattCapacity = Double.Parse(GetUPSVar("battery.capacity", 7), ciClone)
                Me.PowerFreq = Double.Parse(GetUPSVar("input.frequency", Double.Parse(GetUPSVar("output.frequency", Freq_Fallback), ciClone)), ciClone)
                Me.InputV = Double.Parse(GetUPSVar("input.voltage", 220), ciClone)
                Me.OutputV = Double.Parse(GetUPSVar("output.voltage", Me.UPS_InputV), ciClone)
                Me.Load = Double.Parse(GetUPSVar("ups.load", 100), ciClone)
                Me.Status = GetUPSVar("ups.status", "OL")
                Me.OutPower = Double.Parse((GetUPSVar("ups.realpower.nominal", 0)), ciClone)
                If Me.OutPower = 0 Then
                    Me.InputA = Double.Parse(GetUPSVar("ups.current.nominal", 1), ciClone)
                    Me.OutPower = Math.Round(Me.UPS_InputV * 0.95 * Me.UPS_InputA * CosPhi)
                Else
                    Me.OutPower = Math.Round(Me.UPS_OutPower * (Me.UPS_Load / 100))
                End If
                Dim PowerDivider As Double = 0.5
                Select Case Me.Load
                    Case 76 To 100
                        PowerDivider = 0.4
                    Case 51 To 75
                        PowerDivider = 0.3
                End Select
                If Me.BattCh = 255 Then
                    Dim nBatt = Math.Floor(Me.BattV / 12)
                    Me.BattCh = Math.Floor((Me.BattV - (11.6 * nBatt)) / (0.02 * nBatt))
                End If
                If Me.BattRuntime >= 86400 Then
                    'If Load is 0, the calculation results in infinity. This causes an exception in DataUpdated(), causing Me.Disconnect to run in the exception handler below.
                    'Thus a connection is established, but is forcefully disconneced almost immediately. This cycle repeats on each connect until load is <> 0
                    '(Example: I have a 0% load if only Pi, Microtik Router, Wifi AP and switches are running)
                    Me.Load = If(Me.Load <> 0, Me.Load, 0.1)
                    Dim BattInstantCurrent = (Me.OutputV * Me.Load) / (Me.BattV * 100)
                    Me.BattRuntime = Math.Floor(Me.BattCapacity * 0.6 * Me.BattCh * (1 - PowerDivider) * 3600 / (BattInstantCurrent * 100))
                End If

                Dim StatusArr = Me.Status.Trim().Split(" ")
                For Each State In StatusArr
                    Select Case State
                        Case "OL"
                            If Not Update_Nut.Interval = Me.Delay Then
                                Update_Nut.Stop()
                                Update_Nut.Interval = Me.Delay
                                Update_Nut.Start()
                            End If
                            If ShutdownStatus Then
                                LogFile.LogTracing("Stop condition Canceled", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_SHUT_STOP))
                                ShutdownStatus = False
                                RaiseEvent Stop_Shutdown()
                            End If
                        Case "OB"
                            If Update_Nut.Interval = Me.Delay Then
                                Update_Nut.Stop()
                                Update_Nut.Interval = If((Math.Floor(Me.Delay / 5) < 1000), 1000, Math.Floor(Me.Delay / 5))
                                Update_Nut.Start()
                            End If
                            If ((Me.BattCh <= Me.Low_Batt Or Me.BattRuntime <= Me.Backup_Limit) And Not ShutdownStatus) Then
                                LogFile.LogTracing("Stop condition reached", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_SHUT_START))
                                RaiseEvent Shutdown_Condition()
                                ShutdownStatus = True
                            End If
                        Case "FSD"
                            LogFile.LogTracing("Stop condition imposed by the NUT server", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_NUT_FSD))
                            RaiseEvent Shutdown_Condition()
                            ShutdownStatus = True
                        Case "LB", "HB"
                            LogFile.LogTracing("High/Low Battery on UPS", LogLvl.LOG_NOTICE, Me)
                        Case "CHRG"
                            LogFile.LogTracing("Battery is Charging on UPS", LogLvl.LOG_NOTICE, Me)
                        Case "DISCHRG"
                            LogFile.LogTracing("Battery is Discharging on UPS", LogLvl.LOG_NOTICE, Me)
                        Case "BYPASS"
                            LogFile.LogTracing("UPS bypass circuit is active - no battery protection is available", LogLvl.LOG_NOTICE, Me)
                        Case "CAL"
                            LogFile.LogTracing("UPS is currently performing runtime calibration (on battery)", LogLvl.LOG_NOTICE, Me)
                        Case "OFF"
                            LogFile.LogTracing("UPS is offline and is not supplying power to the load", LogLvl.LOG_NOTICE, Me)
                        Case "OVER"
                            LogFile.LogTracing("UPS is overloaded", LogLvl.LOG_NOTICE, Me)
                        Case "TRIM"
                            LogFile.LogTracing("UPS is trimming incoming voltage", LogLvl.LOG_NOTICE, Me)
                        Case "BOOST"
                            LogFile.LogTracing("UPS is boosting incoming voltage", LogLvl.LOG_NOTICE, Me)
                    End Select
                Next
                RaiseEvent DataUpdated()
            End If
        Catch Excep As Exception
            Me.Disconnect(True)
            Enter_Reconnect_Process(Excep, "Error When Retrieve_UPS_Data : ")
        End Try
    End Sub

    Private Sub Reconnect_UPS(sender As Object, e As EventArgs)
        Me.Retry += 1
        If Me.Retry <= Me.MaxRetry And Not Me.Unknown_UPS_Name Then
            RaiseEvent NewRetry()
            LogFile.LogTracing(String.Format("Try Reconnect {0} / {1}", Me.Retry, Me.MaxRetry), LogLvl.LOG_NOTICE, Me, String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_NEW_RETRY), Me.Retry, Me.MaxRetry))
            Me.Connect()
            If Me.IsConnected Then
                LogFile.LogTracing("Nut Host Reconnected", LogLvl.LOG_DEBUG, Me)
                Reconnect_Nut.Enabled = False
                Reconnect_Nut.Stop()
                Me.Retry = 0
                Update_Nut.Start()
                Update_Nut.Enabled = True
                Me.LConnect = False
                RaiseEvent Connected()
                Retrieve_UPS_Data(Nothing, Nothing)
            End If
        ElseIf Me.Unknown_UPS_Name Or Me.Invalid_Auth_Data Then
            Reconnect_Nut.Enabled = False
            Reconnect_Nut.Stop()
            If Me.Unknown_UPS_Name Then
                RaiseEvent Unknown_UPS()
            End If
            If Me.Invalid_Auth_Data Then
                RaiseEvent InvalidLogin()
            End If
        Else
            LogFile.LogTracing("Max Retry reached. Stop Process Autoreconnect and wait for manual Reconnection", LogLvl.LOG_ERROR, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_STOP_RETRY))
            Reconnect_Nut.Enabled = False
            Reconnect_Nut.Stop()
            RaiseEvent Deconnected()
        End If
    End Sub
End Class
