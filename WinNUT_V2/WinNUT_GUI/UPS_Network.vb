Public Class UPS_Var_Node
    Public Property VarKey As String
    Public Property VarValue As String
    Public Property VarDesc As String
End Class
Public Class UPS_Network
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
    Private InputF As Double
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
    Private Update_Nut As New Timer
    Private Reconnect_Nut As New Timer
    Private NutSocket As System.Net.Sockets.Socket
    Private NutTCP As System.Net.Sockets.TcpClient
    Private NutStream As System.Net.Sockets.NetworkStream
    Private ReaderStream As System.IO.StreamReader
    Private WriterStream As System.IO.StreamWriter
    Private ShutdownStatus As Boolean = False
    Private Unknown_UPS_Name As Boolean = False
    Private Invalid_Auth_Data As Boolean = False
    Private Const CosPhi As Double = 0.6

    ' Define possible responses according to NUT protcol v1.2
    Enum NUTResponse
        OK
        ACCESSDENIED
        UNKNOWNUPS
        VARNOTSUPPORTED
        CMDNOTSUPPORTED
        INVALIDARGUMENT
        INSTCMDFAILED
        SETFAILED
        [READONLY]
        TOOLONG
        FEATURENOTSUPPORTED
        FEATURENOTCONFIGURED
        ALREADYSSLMODE
        DRIVERNOTCONNECTED
        DATASTALE
        ALREADYLOGGEDIN
        INVALIDPASSWORD
        ALREADYSETPASSWORD
        INVALIDUSERNAME
        ALREADYSETUSERNAME
        USERNAMEREQUIRED
        PASSWORDREQUIRED
        UNKNOWNCOMMAND
        INVALIDVALUE
    End Enum

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
    Public Property IsConnected() As Boolean
        Get
            Return Me.ConnectionStatus
        End Get
        Set(ByVal Value As Boolean)
            Me.ConnectionStatus = Value
        End Set
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
    Public Property UPS_InputF() As Double
        Get
            Return Me.InputF
        End Get
        Set(ByVal Value As Double)
            Me.InputF = Value
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

    Public Property Backup_Limit() As Integer
        Get
            Return Me.Low_Backup
        End Get
        Set(ByVal Value As Integer)
            Me.Low_Backup = Value
        End Set
    End Property

    Public Event Unknown_UPS()
    Public Event LostConnect()
    Public Event Connected()
    Public Event DataUpdated()
    Public Event NewRetry()
    Public Event Deconnected()
    Public Event Shutdown_Condition()
    Public Event Stop_Shutdown()
    Public Event InvalidLogin()

    Public Sub Connect()
        Try
            LogFile.LogTracing("Connect To Nut Server", LogLvl.LOG_DEBUG, Me)
            If Me.Server <> "" And Me.Port <> 0 And Me.Delay <> 0 And Me.UPSName <> "" Then
                Me.NutSocket = New System.Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.ProtocolType.IP)
                Me.NutTCP = New Net.Sockets.TcpClient(Me.Server, Me.Port)
                Me.NutStream = NutTCP.GetStream
                Me.ReaderStream = New IO.StreamReader(NutStream)
                Me.WriterStream = New IO.StreamWriter(NutStream)
                If Me.Login <> "" And Me.Password <> "" Then
                    If Not AuthLogin() Then
                        Me.Invalid_Auth_Data = True
                        Throw New System.Exception("Failed authentication to Nut server.")
                    End If
                End If
                Me.ConnectionStatus = True
                Me.Mfr = GetUPSVar("ups.mfr", "Unknown")
                If Me.Unknown_UPS_Name Then
                    Throw New System.Exception("Unknown UPS Name.")
                End If
                Me.Model = GetUPSVar("ups.model", "Unknown")
                Me.Serial = GetUPSVar("ups.serial", "Unknown")
                Me.Firmware = GetUPSVar("ups.firmware", "Unknown")
                Update_Nut.Interval = Me.Delay
                Update_Nut.Start()
                Me.LConnect = False
                LogFile.LogTracing("Connection to Nut Host Established", LogLvl.LOG_NOTICE, Me, String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_CONNECTED), Me.Server, Me.Port))
                RaiseEvent Connected()
                Retrieve_UPS_Data(Nothing, Nothing)
            End If
        Catch Excep As Exception
            Enter_Reconnect_Process(Excep, "Error When Connection to Nut Host : ")
        End Try
    End Sub

    Public Sub Enter_Reconnect_Process(ByVal Excep As Exception, ByVal Message As String)
        Me.ConnectionStatus = False
        If Not Me.Unknown_UPS_Name And Not Me.Invalid_Auth_Data Then
            LogFile.LogTracing(Message & Excep.Message, LogLvl.LOG_ERROR, Me, String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_CON_FAILED), Me.Server, Me.Port))
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
        If Not Me.ConnectionStatus Or ForceDisconnect Then
            If Not Me.Unknown_UPS_Name And Not Me.Invalid_Auth_Data Then
                LogFile.LogTracing("Deconnect from Nut Host", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_LOGOFF))
            End If
            If ForceDisconnect Then
                LogFile.LogTracing("Force Disconnect", LogLvl.LOG_WARNING, Me)
            Else
                LogFile.LogTracing("Normal Disconnect", LogLvl.LOG_WARNING, Me)
            End If
            Me.ConnectionStatus = False

            If Me.WriterStream IsNot Nothing Then
                Me.WriterStream.Close()
            End If
            If Me.ReaderStream IsNot Nothing Then
                Me.ReaderStream.Close()
            End If
            If Me.NutStream IsNot Nothing Then
                Me.NutStream.Close()
            End If

            Me.Mfr = ""
            Me.Model = ""
            Me.Serial = ""
            Me.Firmware = ""
            Update_Nut.Stop()
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
        End If
    End Sub

    Public Function GetUPSVar(ByVal varNAme As String, Optional ByVal Fallback_value As String = "", Optional ByVal post_send_delay As Integer = vbNull) As String
        Try
            LogFile.LogTracing("Enter GetUPSVar", LogLvl.LOG_DEBUG, Me)
            If Not Me.ConnectionStatus Then
                Throw New System.Exception("Connection to Nut Host seem broken when Retrieving " & varNAme)
            Else
                Dim SendData = "GET VAR " & Me.UPSName & " " & varNAme & vbCr
                Me.WriterStream.WriteLine(SendData)
                Me.WriterStream.Flush()
                If post_send_delay <> vbNull Then
                    Threading.Thread.Sleep(post_send_delay)
                End If
                Dim DataResult As String = Me.ReaderStream.ReadLine()
                If DataResult = "ERR UNKNOWN-UPS" Then
                    Me.Unknown_UPS_Name = True
                    RaiseEvent Unknown_UPS()
                    Throw New System.Exception("Unknown UPS Name.")
                End If
                Me.Unknown_UPS_Name = False
                If InStr(DataResult, "ERR") <> 0 And Fallback_value <> "" Then
                    LogFile.LogTracing("Apply Fallback Value when retrieving " & varNAme, LogLvl.LOG_WARNING, Me)
                    DataResult = "VAR " & Me.UPSName & " " & varNAme & " " & """" & Fallback_value & """"
                ElseIf DataResult = "" Then
                    Throw New System.Exception("Connection to Nut Host seem broken when Retrieving " & varNAme)
                End If
                If CheckErr(DataResult) <> "OK" Then
                    LogFile.LogTracing("Error Result On Retrieving  " & varNAme & " : " & DataResult, LogLvl.LOG_ERROR, Me)
                    Return Nothing
                Else
                    LogFile.LogTracing("Process Result With " & varNAme & " : " & DataResult, LogLvl.LOG_DEBUG, Me)
                    Return ProcessData(DataResult)
                End If
            End If
        Catch Excep As Exception
            Me.Disconnect(True)
            Enter_Reconnect_Process(Excep, "Error When Retrieving GetUPSVar : ")
            Return Nothing
        End Try
    End Function

    Public Function AuthLogin() As Boolean
        Try
            LogFile.LogTracing("Enter AuthLogin", LogLvl.LOG_DEBUG, Me)
            Dim SendData = "USERNAME " & Me.Login & vbCr
            Me.WriterStream.WriteLine(SendData)
            Me.WriterStream.Flush()
            Dim DataResult As String = Me.ReaderStream.ReadLine()
            Dim NUTResult = EnumResponse(DataResult)

            If DataResult = "ERR INVALID-USERNAME" And Not DataResult = "OK" Then
                Me.Invalid_Auth_Data = True
                RaiseEvent InvalidLogin()
                Throw New System.Exception("Invalid Username.")
            End If
            Me.Invalid_Auth_Data = False
            SendData = "PASSWORD " & Me.Password & vbCr
            Me.WriterStream.WriteLine(SendData)
            Me.WriterStream.Flush()
            DataResult = Me.ReaderStream.ReadLine()
            If DataResult = "ERR INVALID-PASSWORD" And Not DataResult = "OK" Then
                Me.Invalid_Auth_Data = True
                RaiseEvent InvalidLogin()
                Throw New System.Exception("Invalid Password.")
            End If
            Me.Invalid_Auth_Data = False
            Return True
        Catch Excep As Exception
            Me.Disconnect(True)
            Enter_Reconnect_Process(Excep, "Error When Authentifying ")
            Return False
        End Try
    End Function

    ' Parse and enumerate a NUT protocol response.
    Function EnumResponse(ByVal Response As String) As NUTResponse
        ' Remove hyphens to prepare for parsing.
        Dim SanitisedString = Response.Replace("-", String.Empty)
        ' Break the response down so we can get specifics.
        Dim SplitString = SanitisedString.Split(" "c)

        Select Case SplitString(0)
            Case "OK"
                Return NUTResponse.OK
            Case "ERR"
                Return DirectCast([Enum].Parse(GetType(NUTResponse), SplitString(1)), NUTResponse)
            Case Else
                ' We don't recognize the response, throw an error.
                Throw New Exception("Unknown response from NUT server: " & Response)
        End Select
    End Function

    Public Function GetUPSDescVar(ByVal VarName As String) As String
        Try
            LogFile.LogTracing("Enter GetUPSDescVar", LogLvl.LOG_DEBUG, Me)
            If Not Me.ConnectionStatus Then
                Throw New System.Exception("Connection to Nut Host seem broken when Retrieving " & VarName)
            Else
                Dim SendData = "GET DESC " & Me.UPSName & " " & VarName & vbCr
                Me.WriterStream.WriteLine(SendData)
                Me.WriterStream.Flush()
                Dim DataResult As String = ""
                DataResult = Me.ReaderStream.ReadLine()
                If DataResult = Nothing Then
                    Throw New System.Exception("Connection to Nut Host seem broken when Retrieving " & VarName)
                Else
                    If CheckErr(DataResult) <> "OK" Then
                        LogFile.LogTracing("Error Result On Retrieving  " & VarName & " : " & DataResult, LogLvl.LOG_ERROR, Me)
                        Return Nothing
                    Else
                        LogFile.LogTracing("Process Result With " & VarName & " : " & DataResult, LogLvl.LOG_DEBUG, Me)
                        Return ProcessData(DataResult)
                    End If
                End If
            End If
        Catch Excep As Exception
            Me.Disconnect(True)
            Enter_Reconnect_Process(Excep, "Error When Retrieving GetUPSDescVar : ")
            Return Nothing
        End Try
    End Function

    Public Function ListUPSVars() As List(Of UPS_Var_Node)
        Dim List_Datas As New List(Of String)
        Try
            LogFile.LogTracing("Enter GetUPSDescVar", LogLvl.LOG_DEBUG, Me)
            If Not Me.ConnectionStatus Then
                Throw New System.Exception("Connection to Nut Host seem broken when Retrieving ListUPSVars")
                Return Nothing
            Else
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
                        If CheckErr(DataResult) <> "OK" Then
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

    Private Function CheckErr(ByVal NutResp As String)

        If Strings.Left(NutResp, 3) = "ERR" Then
            Dim StrArray = Strings.Split(NutResp, " ")
            If StrArray.Count < 2 Then
                Return "Uknown Error"
            End If
            Return StrArray(1)
        Else
            Return "OK"
        End If
    End Function

    Private Function ProcessData(ByVal UPSData)
        Dim StrArray = Strings.Split(UPSData, """")

        If StrArray.Count < 2 Then
            Me.ConnectionStatus = False
        End If
        Return Strings.Trim(StrArray(1))
    End Function

    Private Sub Retrieve_UPS_Data(sender As Object, e As EventArgs)
        Dim ciClone As System.Globalization.CultureInfo = CType(System.Globalization.CultureInfo.InvariantCulture.Clone(), System.Globalization.CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."
        Try
            LogFile.LogTracing("Enter Retrieve_UPS_Data", LogLvl.LOG_DEBUG, Me)
            If Not Me.LConnect Then
                Dim InputF_Fallback = 50 + CInt(WinNUT_Params.Arr_Reg_Key.Item("FrequencySupply")) * 10

                Me.BattCh = Double.Parse(GetUPSVar("battery.charge", 255, 50), ciClone)
                Me.BattV = Double.Parse(GetUPSVar("battery.voltage", 12), ciClone)
                Me.BattRuntime = Double.Parse(GetUPSVar("battery.runtime", 86400), ciClone)
                Me.BattCapacity = Double.Parse(GetUPSVar("battery.capacity", 7), ciClone)
                Me.InputF = Double.Parse(GetUPSVar("input.frequency", InputF_Fallback), ciClone)
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
                    Dim BattInstantCurrent = (Me.OutputV * Me.Load) / (Me.BattV * 100)
                    Me.BattRuntime = Math.Floor(Me.BattCapacity * 0.6 * Me.BattCh * (1 - PowerDivider) * 3600 / (BattInstantCurrent * 100))
                End If
                RaiseEvent DataUpdated()
                If Not Me.Status.Trim().StartsWith("OL") And (Me.BattCh <= Me.Low_Batt Or Me.BattRuntime <= Me.Backup_Limit) And Not ShutdownStatus Then
                    LogFile.LogTracing("Stop condition reached", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_SHUT_START))
                    RaiseEvent Shutdown_Condition()
                    ShutdownStatus = True
                ElseIf ShutdownStatus And Me.Status.Trim().StartsWith("OL") Then
                    LogFile.LogTracing("Stop condition Canceled", LogLvl.LOG_NOTICE, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_SHUT_STOP))
                    ShutdownStatus = False
                    RaiseEvent Stop_Shutdown()
                End If
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
