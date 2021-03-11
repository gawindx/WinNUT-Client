' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Class Logger
    Private ReadOnly LogFile As New Microsoft.VisualBasic.Logging.FileLogTraceListener()
    Private ReadOnly TEventCache As New TraceEventCache()
    ' Enable writing to a log file.
    Public WriteLogValue As Boolean
    Public LogLevelValue As LogLvl
    Private L_CurrentLogData As String
    Private LastEventsList As New List(Of Object)
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
    Public ReadOnly Property LastEvents() As List(Of Object)
        Get
            Return Me.LastEventsList
        End Get
    End Property
    Public Sub New(ByVal WriteLog As Boolean, ByVal LogLevel As LogLvl)
        Me.WriteLogValue = WriteLog
        Me.LogLevelValue = LogLevel
        Me.LogFile.TraceOutputOptions = TraceOptions.DateTime Or TraceOptions.ProcessId
        Me.LogFile.Append = True
        Me.LogFile.AutoFlush = True
        Me.LogFile.BaseFileName = "WinNUT-CLient"
        Me.LogFile.LogFileCreationSchedule = Logging.LogFileCreationScheduleOption.Daily
        Me.LogFile.Location = Microsoft.VisualBasic.Logging.LogFileLocation.Custom
        Me.LogFile.CustomLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\WinNUT-Client"
        Me.LastEventsList.Capacity = 50
        WinNUT_Globals.LogFilePath = Me.LogFile.FullLogFileName
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
        Dim EventTime = Now.ToLocalTime
        Dim FinalMsg = EventTime & " Pid: " & Pid & " " & SenderName & " : " & message

        'Update LogFilePath to make sure it's still the correct path
        WinNUT_Globals.LogFilePath = Me.LogFile.FullLogFileName

        ' Always write log messages to the attached debug messages window.
#If DEBUG Then
       Debug.WriteLine(FinalMsg)
#End If

        'Create Event in EventList in case of crash for generate Report
        If Me.LastEventsList.Count = Me.LastEventsList.Capacity Then
            Me.LastEventsList.RemoveAt(0)
        End If

        Me.LastEventsList.Add(FinalMsg)

        If Me.WriteLogValue AndAlso Me.LogLevel >= LvlError Then
            LogFile.WriteLine(FinalMsg)
        End If
        'If LvlError = LogLvl.LOG_NOTICE Then
        If LogToDisplay IsNot Nothing Then
            Me.L_CurrentLogData = LogToDisplay
            RaiseEvent NewData(sender)
        End If
    End Sub
End Class
