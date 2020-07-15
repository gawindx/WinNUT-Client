Public Class Logger
    Private LogFile As New FileLogTraceListener()
    Private TEventCache As New TraceEventCache()
    Public WriteLogValue As Boolean
    Public LogLevelValue As LogLvl
    Private L_CurrentLogData As String
    Public Event NewData()
    Public Enum LogLvl
        LOG_NOTICE
        LOG_WARNING
        LOG_ERROR
        LOG_DEBUG
    End Enum
    Public Property CurrentLogData() As String
        Get
            Dim Tmp_Data = Me.L_CurrentLogData
            Me.L_CurrentLogData = Nothing
            Return Tmp_Data
        End Get
        Set(ByVal Value As String)
            Me.L_CurrentLogData = Value
        End Set
    End Property
    Public Sub New(ByVal WriteLog As Boolean, ByVal LogLevel As LogLvl)
        Me.WriteLogValue = WriteLog
        Me.LogLevelValue = LogLevel
        Me.LogFile.TraceOutputOptions = TraceOptions.DateTime Or TraceOptions.ProcessId
        Me.LogFile.Append = True
        Me.LogFile.AutoFlush = True
        Me.LogFile.BaseFileName = "WinNUT-CLient"
        Me.LogFile.Location = LogFileLocation.ExecutableDirectory
    End Sub

    Public Property WriteLog() As Boolean
        Get
            Return Me.WriteLogValue
        End Get
        Set(ByVal Value As Boolean)
            Me.WriteLogValue = Value
            If Not Me.WriteLogValue Then
                LogFile.Dispose()
            End If
        End Set
    End Property

    Public Property LogLevel() As LogLvl
        Get
            Return Me.LogLevelValue
        End Get
        Set(ByVal Value As LogLvl)
            Me.LogLevelValue = Value
        End Set
    End Property

    Public Sub LogTracing(ByVal message As String, ByVal LvlError As Int16, sender As Object)
        Dim Pid = TEventCache.ProcessId
        Dim SenderName = sender.GetType.Name
        Dim EventTime = TEventCache.DateTime
        If Me.WriteLogValue Then
            If Me.LogLevel >= LvlError Then
                LogFile.WriteLine(EventTime & " " & Pid & " " & " " & SenderName & " : " & message)
            End If
        End If
        If LvlError = LogLvl.LOG_NOTICE Then
            Me.L_CurrentLogData = message
            RaiseEvent NewData()
        End If
    End Sub
End Class
