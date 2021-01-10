Public Class Logger
    Private ReadOnly LogFile As New FileLogTraceListener()
    Private ReadOnly TEventCache As New TraceEventCache()
    ' Enable writing to a log file.
    Public WriteLogValue As Boolean
    Public LogLevelValue As LogLvl
    Private L_CurrentLogData As String
    Public Event NewData(ByVal sender As Object)
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
        Me.LogFile.Location = LogFileLocation.Custom
        Me.LogFile.CustomLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\WinNUT-Client"
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

    Public Sub LogTracing(ByVal message As String, ByVal LvlError As Int16, sender As Object, Optional ByVal LogToDisplay As String = Nothing)
        Dim Pid = TEventCache.ProcessId
        Dim SenderName = sender.GetType.Name
        Dim EventTime = TEventCache.DateTime.ToLocalTime
        Dim FinalMsg = EventTime & " " & Pid & " " & " " & SenderName & " : " & message

        ' Always write log messages to the attached debug messages window.
#If DEBUG Then
        Debug.WriteLine(FinalMsg)
#End If

        If Me.WriteLogValue Then
            If Me.LogLevel >= LvlError Then
                LogFile.WriteLine(EventTime & " " & Pid & " " & " " & SenderName & " : " & message)
            End If
        End If
        'If LvlError = LogLvl.LOG_NOTICE Then
        If LogToDisplay IsNot Nothing Then
            Me.L_CurrentLogData = LogToDisplay
            RaiseEvent NewData(sender)
        End If
    End Sub
End Class
