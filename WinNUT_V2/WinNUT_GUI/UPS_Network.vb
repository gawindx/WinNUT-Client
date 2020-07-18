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
    Private MaxRetry As Integer = 1
    Private Retry As Integer = 1
    Private Update_Nut As New Timer
    Private Reconnect_Nut As New Timer
    Private NutSocket As System.Net.Sockets.Socket
    Private NutTCP As System.Net.Sockets.TcpClient
    Private NutStream As System.Net.Sockets.NetworkStream
    Private ReaderStream As System.IO.StreamReader
    Private WriterStream As System.IO.StreamWriter
    Private ShutdownStatus As Boolean = False
    Private Const CosPhi As Double = 0.6

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

    Public Event LostConnect()
    Public Event Reconnected()
    Public Event DataUpdated()
    Public Event NewRetry()
    Public Event Deconnected()
    Public Event Shutdown_Condition()
    Public Event Stop_Shutdown()
    Public Sub Connect()
        Try
            LogFile.LogTracing("Open List Var Gui", LogLvl.LOG_DEBUG, Me)
            If Me.Server <> "" And Me.Port <> 0 And Me.Delay <> 0 And Me.UPSName <> "" Then
                Me.NutSocket = New System.Net.Sockets.Socket(Net.Sockets.AddressFamily.InterNetwork, Net.Sockets.ProtocolType.IP)
                Me.NutTCP = New Net.Sockets.TcpClient(Me.Server, Me.Port)
                Me.NutStream = NutTCP.GetStream
                Me.ReaderStream = New IO.StreamReader(NutStream)
                Me.WriterStream = New IO.StreamWriter(NutStream)
                Me.ConnectionStatus = True
                Me.Mfr = GetUPSVar("ups.mfr", "Unknown")
                Me.Model = GetUPSVar("ups.model", "Unknown")
                Me.Serial = GetUPSVar("ups.serial", "Unknown")
                Me.Firmware = GetUPSVar("ups.firmware", "Unknown")
                Update_Nut.Interval = Me.Delay
                Update_Nut.Start()
                Me.LConnect = False
                LogFile.LogTracing("Connection to Nu Host Established", LogLvl.LOG_NOTICE, Me)
            End If
        Catch Excep As Exception
            LogFile.LogTracing("Error When Connection to Nut Host : " & Excep.Message, LogLvl.LOG_ERROR, Me)
            Me.ConnectionStatus = False
            If Me.AReconnect Then
                LogFile.LogTracing("Autoreconnect Enable. Run Autoreconnect Process", LogLvl.LOG_DEBUG, Me)
                Reconnect_Nut.Enabled = True
                Reconnect_Nut.Start()
                Update_Nut.Stop()
                Update_Nut.Enabled = False
                RaiseEvent LostConnect()
            End If
        End Try
    End Sub

    Public Sub Disconnect(Optional ByVal ForceDisconnect = False)
        If Not Me.ConnectionStatus Or ForceDisconnect Then
            LogFile.LogTracing("Deconnect from Nut Host", LogLvl.LOG_NOTICE, Me)
            If ForceDisconnect Then
                LogFile.LogTracing("Force Disconnect", LogLvl.LOG_WARNING, Me)
            Else
                LogFile.LogTracing("Normal Disconnect", LogLvl.LOG_WARNING, Me)
            End If
            Me.ConnectionStatus = False
            Me.WriterStream.Close()
            Me.ReaderStream.Close()
            Me.NutStream.Close()
            Me.Mfr = ""
            Me.Model = ""
            Me.Serial = ""
            Me.Firmware = ""
            Update_Nut.Stop()
            Update_Nut.Enabled = False
            If Me.AReconnect Then
                RaiseEvent NewRetry()
            Else
                RaiseEvent Deconnected()
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
            LogFile.LogTracing("Error When Retrieving GetUPSVar : " & Excep.Message, LogLvl.LOG_ERROR, Me)
            Me.Disconnect(True)
            Me.LConnect = True
            If Me.AReconnect Then
                LogFile.LogTracing("Autoreconnect Enable. Run Autoreconnect Process", LogLvl.LOG_NOTICE, Me)
                Reconnect_Nut.Enabled = True
                Reconnect_Nut.Start()
                Update_Nut.Stop()
                Update_Nut.Enabled = False
            End If
            RaiseEvent LostConnect()
            Return Nothing
        End Try
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
            LogFile.LogTracing("Error When Retrieving GetUPSDescVar : " & Excep.Message, LogLvl.LOG_ERROR, Me)
            Me.Disconnect(True)
            Me.LConnect = True
            If Me.AReconnect Then
                LogFile.LogTracing("Autoreconnect Enable. Run Autoreconnect Process", LogLvl.LOG_NOTICE, Me)
                Reconnect_Nut.Enabled = True
                Reconnect_Nut.Start()
                Update_Nut.Stop()
                Update_Nut.Enabled = False
            End If
            RaiseEvent LostConnect()
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
            LogFile.LogTracing("Error When Retieving ListUPSVars : " & Excep.Message, LogLvl.LOG_ERROR, Me)
            Me.Disconnect(True)
            Me.LConnect = True
            If Me.AReconnect Then
                LogFile.LogTracing("Autoreconnect Enable. Run Autoreconnect Process", LogLvl.LOG_NOTICE, Me)
                Reconnect_Nut.Enabled = True
                Reconnect_Nut.Start()
                Update_Nut.Stop()
                Update_Nut.Enabled = False
            End If
            RaiseEvent LostConnect()
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
                    'If (Me.BattCh <= Me.Low_Batt Or Me.BattRuntime <= Me.Backup_Limit) And Not ShutdownStatus Then
                    LogFile.LogTracing("Stop condition reached", LogLvl.LOG_NOTICE, Me)
                    RaiseEvent Shutdown_Condition()
                    ShutdownStatus = True
                ElseIf ShutdownStatus And Me.Status.Trim().StartsWith("OL") Then
                    'ElseIf ShutdownStatus Then
                    LogFile.LogTracing("Stop condition Canceled", LogLvl.LOG_NOTICE, Me)
                    ShutdownStatus = False
                    RaiseEvent Stop_Shutdown()
                End If
            End If
        Catch Excep As Exception
            LogFile.LogTracing("Error When Retrieve_UPS_Data : " & Excep.Message, LogLvl.LOG_ERROR, Me)
            Me.Disconnect(True)
            Me.LConnect = True
            If Me.AReconnect Then
                LogFile.LogTracing("Autoreconnect Enable. Run Autoreconnect Process", LogLvl.LOG_NOTICE, Me)
                Reconnect_Nut.Enabled = True
                Reconnect_Nut.Start()
                Update_Nut.Stop()
                Update_Nut.Enabled = False
            End If
            RaiseEvent LostConnect()
        End Try
    End Sub

    Private Sub Reconnect_UPS(sender As Object, e As EventArgs)
        Me.Retry += 1
        If Me.Retry <= Me.MaxRetry Then
            RaiseEvent NewRetry()
            LogFile.LogTracing("Try Reconnect {Me.Retry} / {Me.MaxRetry}", LogLvl.LOG_NOTICE, Me)
            Me.Connect()
            If Me.IsConnected Then
                LogFile.LogTracing("Nut Host Reconnected", LogLvl.LOG_DEBUG, Me)
                Reconnect_Nut.Enabled = False
                Reconnect_Nut.Stop()
                Update_Nut.Start()
                Update_Nut.Enabled = True
                Me.LConnect = False
                RaiseEvent Reconnected()
            End If
        Else
            LogFile.LogTracing("Max Retry reached. Stop Process Autoreconnect and wait for manual Reconnection", LogLvl.LOG_ERROR, Me)
            Reconnect_Nut.Enabled = False
            Reconnect_Nut.Stop()
            RaiseEvent Deconnected()
        End If
    End Sub
End Class
