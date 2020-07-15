Public Class Shutdown_Gui
    Private LogFile As Logger
    Private RedText As Boolean = True
    Private Elapsed As Double
    Private Shutdown_PBar As New WinFormControls.CProgressBar
    Private Start_Shutdown As DateTime
    Public Grace_Timer As New Timer
    Public Shutdown_Timer As New Timer
    Private Sub Grace_Button_Click(sender As Object, e As EventArgs) Handles Grace_Button.Click
        Me.Shutdown_Timer.Stop()
        Me.Shutdown_Timer.Enabled = False
        Grace_Button.Enabled = False
        Me.Grace_Timer.Enabled = True
        Me.Grace_Timer.Start()
        Me.Elapsed = Now.Subtract(Start_Shutdown).TotalSeconds
    End Sub

    Private Sub Shutdown_Gui_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = WinNUT.Icon
        Me.LogFile = WinNUT.LogFile
        LogFile.LogTracing("Load ShutDown Gui", LogLvl.LOG_DEBUG, Me)
        Me.Grace_Timer.Enabled = False
        Me.Grace_Timer.Stop()
        Me.Grace_Timer.Interval = (WinNUT_Params.Arr_Reg_Key.Item("ExtendedShutdownDelay") * 1000)
        Shutdown_Timer.Interval = (WinNUT_Params.Arr_Reg_Key.Item("DelayToShutdown") * 1000)
        If WinNUT_Params.Arr_Reg_Key.Item("AllowExtendedShutdownDelay") Then
            Grace_Button.Enabled = True
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
    End Sub

    Private Sub Grace_Timer_Tick(sender As Object, e As EventArgs)
        Start_Shutdown = Now
        Me.Shutdown_Timer.Interval -= ((Me.Elapsed) * 1000)
        Me.Shutdown_Timer.Enabled = True
        Me.Shutdown_Timer.Start()

    End Sub

    Private Sub ShutDown_Btn_Click(sender As Object, e As EventArgs) Handles ShutDown_Btn.Click
        WinNUT.Shutdown_Action()
    End Sub

    Private Sub Shutdown_Timer_Tick(sender As Object, e As EventArgs)
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
        If Me.Shutdown_Timer.Enabled = True Then
            Me.Elapsed = (Shutdown_Timer.Interval / 1000) - Now.Subtract(Start_Shutdown).TotalSeconds
            Dim TimeToShow As String
            Dim iSpan As TimeSpan = TimeSpan.FromSeconds(Elapsed)
            If Me.Shutdown_Timer.Interval = (3600 * 1000) Then
                TimeToShow = iSpan.Hours.ToString.PadLeft(2, "0"c) & ":" &
                                        iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                                        iSpan.Seconds.ToString.PadLeft(2, "0"c)
            Else
                TimeToShow = iSpan.Minutes.ToString.PadLeft(2, "0"c) & ":" &
                             iSpan.Seconds.ToString.PadLeft(2, "0"c)
            End If
            Shutdown_PBar.Text = TimeToShow
            Dim NewValue As Integer = 100 - (100 * (Me.Elapsed / (Me.Shutdown_Timer.Interval / 1000)))
            If NewValue > 100 Then
                NewValue = 100
            End If
            Shutdown_PBar.Value = NewValue
            lbl_UPSStatus.Text = "Battery_Charge : " & WinNUT.UPS_BattCh & vbNewLine & "Remaining Time : " & WinNUT.Lbl_VRTime.Text
        End If
    End Sub

    Private Sub Shutdown_Gui_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me.Visible Then
            e.Cancel = True
        End If
    End Sub
End Class