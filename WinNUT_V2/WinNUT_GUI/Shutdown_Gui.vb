' WinNUT is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Class Shutdown_Gui
    Private LogFile As Logger
    Private RedText As Boolean = True
    Private ReadOnly Shutdown_PBar As New WinFormControls.CProgressBar
    Private Start_Shutdown As DateTime
    Private Offset_STimer As Double = 0
    Private STimer As Double = 0
    Private Remained As Double = 0
    Public Grace_Timer As New Timer
    Public Shutdown_Timer As New Timer

    Private Sub Grace_Button_Click(sender As Object, e As EventArgs) Handles Grace_Button.Click
        Me.Shutdown_Timer.Stop()
        Me.Shutdown_Timer.Enabled = False
        Grace_Button.Enabled = False
        Me.Grace_Timer.Enabled = True
        Me.Grace_Timer.Start()
        Me.Offset_STimer = WinNUT_Params.Arr_Reg_Key.Item("ExtendedShutdownDelay")
    End Sub

    Private Sub Shutdown_Gui_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = WinNUT.Icon
        Me.LogFile = WinNUT.LogFile
        LogFile.LogTracing("Load ShutDown Gui", LogLvl.LOG_DEBUG, Me)
        Me.Grace_Timer.Enabled = False
        Me.Grace_Timer.Stop()
        'If ExtendedShutdownDelay = 0 (the default value), the next line fails and the whole shutdown sequence fails - Thus no shutdown
        'Moved next line lower down
        'Me.Grace_Timer.Interval = (WinNUT_Params.Arr_Reg_Key.Item("ExtendedShutdownDelay") * 1000)
        Shutdown_Timer.Interval = (WinNUT_Params.Arr_Reg_Key.Item("DelayToShutdown") * 1000)
        Me.STimer = WinNUT_Params.Arr_Reg_Key.Item("DelayToShutdown")
        Me.Remained = Me.STimer
        If WinNUT_Params.Arr_Reg_Key.Item("AllowExtendedShutdownDelay") Then
            Grace_Button.Enabled = True
            'Moved here so it is only used if grace period is allowed
            Try
                Me.Grace_Timer.Interval = (WinNUT_Params.Arr_Reg_Key.Item("ExtendedShutdownDelay") * 1000)
            Catch ex As Exception
                'Disable Grace peroid option if Interval is set to 0
                Grace_Button.Enabled = False
            End Try
        Else
            Grace_Button.Enabled = False
        End If
        Start_Shutdown = Now
        With Shutdown_PBar
            .Location = New Point(10, 150)
            .Size = New Point(400, 23)
            .Style = ProgressBarStyle.Continuous
            .Font = New Font(.Font, .Font.Style Or FontStyle.Bold)
            .ForeColor = Color.Black
            Dim TimeToShow As String
            Dim iSpan As TimeSpan = TimeSpan.FromSeconds(Shutdown_Timer.Interval / 1000)
            If Shutdown_Timer.Interval = (3600 * 1000) Then
                TimeToShow = iSpan.Hours.ToString.PadLeft(2, "0"c) & ":" &
                                        iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                                        iSpan.Seconds.ToString.PadLeft(2, "0"c)
            Else
                TimeToShow = iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                             iSpan.Seconds.ToString.PadLeft(2, "0"c)
            End If
            .Text = TimeToShow
            .Value = 0
        End With
        Me.Controls.Add(Shutdown_PBar)
        AddHandler Grace_Timer.Tick, AddressOf Grace_Timer_Tick
        AddHandler Shutdown_Timer.Tick, AddressOf Shutdown_Timer_Tick
    End Sub

    Private Sub Shutdown_Gui_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Shutdown_Timer.Enabled = True
        Me.Shutdown_Timer.Start()
        lbl_UPSStatus.Text = String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_SHUT_STAT), WinNUT.UPS_BattCh.ToString(), WinNUT.Lbl_VRTime.Text)
    End Sub

    Private Sub Grace_Timer_Tick(sender As Object, e As EventArgs)
        Me.Shutdown_Timer.Interval = Me.Remained * 1000
        Me.Shutdown_Timer.Enabled = True
        Me.Shutdown_Timer.Start()
    End Sub

    Private Sub ShutDown_Btn_Click(sender As Object, e As EventArgs) Handles ShutDown_Btn.Click
        WinNUT.Shutdown_Action()
    End Sub

    Private Sub Shutdown_Timer_Tick(sender As Object, e As EventArgs)
        Shutdown_PBar.Value = 100
        System.Threading.Thread.Sleep(1000)
        WinNUT.Shutdown_Action()
        Run_Timer.Enabled = False
    End Sub

    Private Sub Run_Timer_Tick(sender As Object, e As EventArgs) Handles Run_Timer.Tick
        If RedText Then
            lbl_UPSStatus.ForeColor = Color.Red
            RedText = False
        Else
            lbl_UPSStatus.ForeColor = Color.Black
            RedText = True
        End If
        If Me.Shutdown_Timer.Enabled = True And Me.Remained > 0 Then
            Me.Remained = Int(STimer + Offset_STimer - Now.Subtract(Start_Shutdown).TotalSeconds)
            Dim NewValue As Integer = 100
            If Me.Remained > 0 Then
                NewValue -= (100 * (Me.Remained / Me.STimer))
                If NewValue > 100 Then
                    NewValue = 100
                End If
            End If
            Dim TimeToShow As String
            Dim iSpan As TimeSpan = TimeSpan.FromSeconds(Me.Remained)
            If Me.Shutdown_Timer.Interval = (3600 * 1000) Then
                TimeToShow = iSpan.Hours.ToString.PadLeft(2, "0"c) & ":" &
                                        iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                                        iSpan.Seconds.ToString.PadLeft(2, "0"c)
            Else
                TimeToShow = iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                             iSpan.Seconds.ToString.PadLeft(2, "0"c)
            End If
            Shutdown_PBar.Text = TimeToShow
            Shutdown_PBar.Value = NewValue
            lbl_UPSStatus.Text = String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_SHUT_STAT), WinNUT.UPS_BattCh.ToString(), WinNUT.Lbl_VRTime.Text)
        End If
    End Sub

    Private Sub Shutdown_Gui_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me.Visible Then
            e.Cancel = True
        End If
    End Sub
End Class