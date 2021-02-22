' WinNUT is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Class Update_Gui

    Private ChangeLogByteSize As Long
    Public Shared WithEvents LogFile As Logger
    Private Const GitApiURL As String = "https://api.github.com/repos/gawindx/WinNUT-Client/releases"
    Private WithEvents WebC As New System.Net.WebClient
    Private JSONReleaseFile As Object
    Private sChangeLog As String
    Private ReadOnly ManualUpdate As Boolean = False
    Private UpdateInfoRetrieved As Boolean = False
    Private HasUpdate As Boolean = False
    Private NewVersion As String = Nothing
    Private NewVersionMsiURL As String
    Private Download_Form As Form
    Private DPBar As WinFormControls.CProgressBar
    Private Dlbl As Label

    Public Sub New(Optional ByRef mUpdate As Boolean = False)
        InitializeComponent()
        ManualUpdate = mUpdate
    End Sub

    Protected Overrides Sub SetVisibleCore(value As Boolean)
        If Me.UpdateInfoRetrieved Then
            If Me.HasUpdate Then
                MyBase.SetVisibleCore(True)
            Else
                Me.Close()
            End If
        Else
            MyBase.SetVisibleCore(False)
            WinNUT.NotifyIcon.Visible = False
            Me.Icon = WinNUT.Icon
            LogFile = WinNUT.LogFile
            VerifyUpdate()
        End If
    End Sub

    Public Function GetDownloadSize(ByVal URL As String) As Long
        Net.ServicePointManager.Expect100Continue = True
        Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
        Dim Req As Net.WebRequest = Net.WebRequest.Create(URL)
        Req.Method = Net.WebRequestMethods.Http.Head
        Using Resp = Req.GetResponse()
            Return Resp.ContentLength
        End Using
    End Function
    Public Sub VerifyUpdate()
        LogFile.LogTracing("Verify Update", LogLvl.LOG_DEBUG, Me)
        If WinNUT_Params.Arr_Reg_Key.Item("VerifyUpdate") Or Me.ManualUpdate Then
            Dim DelayVerif As DateInterval = DateInterval.Month
            Select Case WinNUT_Params.Arr_Reg_Key.Item("DelayBetweenEachVerification")
                Case 0
                    DelayVerif = DateInterval.Day
                Case 1
                    DelayVerif = DateInterval.Weekday
                Case 2
                    DelayVerif = DateInterval.Month
            End Select
            Dim Today As DateTime = Now
            Dim Diff As Integer = 1
            If WinNUT_Params.Arr_Reg_Key.Item("LastDateVerification") <> "" Then
                Dim LastVerif As DateTime = Convert.ToDateTime(WinNUT_Params.Arr_Reg_Key.Item("LastDateVerification"))
                Diff = DateAndTime.DateDiff(DelayVerif, LastVerif, Today, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1)
            End If
            If Diff >= 1 Or ManualUpdate Then
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
                WebC.Headers.Add(System.Net.HttpRequestHeader.Accept, "application/json")
                WebC.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.141 Safari/537.36 OPR/73.0.3856.344")
                WebC.Headers.Add(System.Net.HttpRequestHeader.AcceptLanguage, "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7")
                AddHandler WebC.DownloadStringCompleted, AddressOf Changelog_Downloaded
                WebC.DownloadStringAsync(New Uri(GitApiURL))
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Changelog_Downloaded(ByVal sender As Object, ByVal e As System.Net.DownloadStringCompletedEventArgs)
        Dim ChangeLogDiff As String = Nothing

        Try
            If (Not e.Cancelled And e.Error Is Nothing) Then
                If e.Result.Length <> 0 Then
                    Dim JSONReleases = Newtonsoft.Json.JsonConvert.DeserializeObject(e.Result)
                    Dim HighestVersion As String = Nothing
                    Dim ActualVersion As Version = Version.Parse(WinNUT_Globals.ProgramVersion)
                    Dim sPattern As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("[Vv](\d+\.\d+\.\d+\.?\d+).*$")
                    For Each JSONRelease In JSONReleases
                        Dim PreRelease = Convert.ToBoolean(JSONRelease("prerelease").ToString)
                        Dim DraftRelease = Convert.ToBoolean(JSONRelease("draft").ToString)
                        Dim ReleaseName = JSONRelease("name")
                        Dim RegExVersion = sPattern.Match(ReleaseName)

                        If Not DraftRelease And ((PreRelease And WinNUT_Params.Arr_Reg_Key.Item("StableOrDevBranch") = 1) Or Not PreRelease) Then
                            If RegExVersion.Groups.Count > 1 Then
                                Dim ReleaseVersion As Version = Version.Parse(RegExVersion.Groups(1).Value)
                                If ActualVersion.CompareTo(ReleaseVersion) = -1 Then
                                    If HighestVersion = Nothing Or (HighestVersion <> Nothing AndAlso ReleaseVersion.CompareTo(Version.Parse(HighestVersion))) > 0 Then
                                        HighestVersion = RegExVersion.Groups(1).Value
                                        Me.NewVersion = HighestVersion
                                        ChangeLogDiff = "Changelog :" & vbNewLine
                                        ChangeLogDiff &= WinNUT_Globals.ProgramVersion & " => " & ReleaseVersion.ToString & vbNewLine
                                        Me.NewVersionMsiURL = JSONRelease("assets")(0)("browser_download_url").ToString
                                    End If
                                    ChangeLogDiff &= vbNewLine & ReleaseName & vbNewLine & JSONRelease("body").ToString & vbNewLine
                                End If
                            End If
                        ElseIf Not DraftRelease Then
                            ChangeLogDiff &= vbNewLine & ReleaseName & vbNewLine & JSONRelease("body").ToString & vbNewLine
                        End If
                    Next
                    If ChangeLogDiff IsNot Nothing And Me.NewVersionMsiURL IsNot Nothing Then
                        Me.sChangeLog = ChangeLogDiff
                        Me.HasUpdate = True
                        LogFile.LogTracing(String.Format("New Version Available : {0}", Me.NewVersion), LogLvl.LOG_DEBUG, Me, String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_UPDATE), Me.NewVersion))
                    Else
                        HighestVersion = Nothing
                        LogFile.LogTracing("No Update Available", LogLvl.LOG_DEBUG, Me, WinNUT_Globals.StrLog.Item(AppResxStr.STR_LOG_NO_UPDATE))
                    End If
                End If
            End If
        Catch excep As Exception
            WinNUT.LogFile.LogTracing(excep.Message, LogLvl.LOG_ERROR, Me)
        End Try
        WinNUT_Params.Arr_Reg_Key.Item("LastDateVerification") = Now.ToString
        WinNUT_Params.Save_Params()
        Me.UpdateInfoRetrieved = True
        Me.Show()
    End Sub

    Private Sub Update_Gui_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        TB_ChgLog.Text = sChangeLog
        TB_ChgLog.Visible = False
        Lbl.Text = String.Format(WinNUT_Globals.StrLog.Item(AppResxStr.STR_UP_AVAIL), Me.NewVersion)
        GB1.Size = New Point(GB1.Size.Width, GB1.Size.Height - 160)
        Update_Btn.Location = New Point(Update_Btn.Location.X, Update_Btn.Location.Y - 160)
        ShowLog_Button.Location = New Point(ShowLog_Button.Location.X, ShowLog_Button.Location.Y - 160)
        Close_Btn.Location = New Point(Close_Btn.Location.X, Close_Btn.Location.Y - 160)
        Me.Size = New Point(Me.Size.Width, Me.Size.Height - 160)
    End Sub

    Private Sub Close_Btn_Click(sender As Object, e As EventArgs) Handles Close_Btn.Click
        Me.Close()
    End Sub

    Private Sub ShowLog_Button_Click(sender As Object, e As EventArgs) Handles ShowLog_Button.Click
        If TB_ChgLog.Visible Then
            TB_ChgLog.Visible = False
            ShowLog_Button.Text = WinNUT_Globals.StrLog.Item(AppResxStr.STR_UP_SHOW)
            GB1.Size = New Point(GB1.Size.Width, GB1.Size.Height - 160)
            Update_Btn.Location = New Point(Update_Btn.Location.X, Update_Btn.Location.Y - 160)
            ShowLog_Button.Location = New Point(ShowLog_Button.Location.X, ShowLog_Button.Location.Y - 160)
            Close_Btn.Location = New Point(Close_Btn.Location.X, Close_Btn.Location.Y - 160)
            Me.Size = New Point(Me.Size.Width, Me.Size.Height - 160)
        Else
            TB_ChgLog.Visible = True
            ShowLog_Button.Text = WinNUT_Globals.StrLog.Item(AppResxStr.STR_UP_SHOW)
            GB1.Size = New Point(GB1.Size.Width, GB1.Size.Height + 160)
            Update_Btn.Location = New Point(Update_Btn.Location.X, Update_Btn.Location.Y + 160)
            ShowLog_Button.Location = New Point(ShowLog_Button.Location.X, ShowLog_Button.Location.Y + 160)
            Close_Btn.Location = New Point(Close_Btn.Location.X, Close_Btn.Location.Y + 160)
            Me.Size = New Point(Me.Size.Width, Me.Size.Height + 160)
        End If
    End Sub

    Private Sub Update_Btn_Click(sender As Object, e As EventArgs) Handles Update_Btn.Click
        Dim MSIURL As String = Nothing
        MSIURL = Me.NewVersionMsiURL
        Download_Form = New Form
        DPBar = New WinFormControls.CProgressBar
        Dlbl = New Label

        With Download_Form
            .Icon = Me.Icon
            .Size = New Point(320, 150)
            .FormBorderStyle = FormBorderStyle.Sizable
            .MaximizeBox = False
            .MinimizeBox = False
            .Controls.Add(DPBar)
            .Controls.Add(Dlbl)
            .StartPosition = FormStartPosition.CenterParent
        End With
        With DPBar
            .Location = New Point(12, 80)
            .Style = ProgressBarStyle.Continuous
            .Size = New Point(280, .Size.Height)
            .ForeColor = Color.Black
        End With
        With Dlbl
            .Location = New Point(12, 20)
            .TextAlign = ContentAlignment.MiddleCenter
            .Text = WinNUT_Globals.StrLog.Item(AppResxStr.STR_UP_DOWNFROM) & vbNewLine & MSIURL
            .Size = New Point(280, (.Size.Height * 2))
        End With
        Download_Form.Show()
        Dim MSIFile = System.IO.Path.GetTempPath() + "WinNUT_" & Me.NewVersion & "_Setup.msi"

        Using WebC = New Net.WebClient
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
            AddHandler WebC.DownloadFileCompleted, AddressOf New_Version_Downloaded
            AddHandler WebC.DownloadProgressChanged, AddressOf Update_DPBar
            WebC.QueryString.Add("FileName", MSIFile)
            WebC.DownloadFileAsync(New Uri(MSIURL), MSIFile)
        End Using
    End Sub

    Private Sub Update_DPBar(sender As Object, e As System.Net.DownloadProgressChangedEventArgs)
        DPBar.Value = 90 * (e.ProgressPercentage / 100)
        DPBar.Text = String.Format("{0:N}Kb / {1:N}Kb", (e.BytesReceived / 1024), (e.TotalBytesToReceive / 1024))
    End Sub

    Private Sub New_Version_Downloaded(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
        Dim Filename = sender.QueryString.Item("FileName")
        Select Case MsgBox(WinNUT_Globals.StrLog.Item(AppResxStr.STR_UP_UPMSG), vbOKCancel, "Update Now")
            Case vbOK
                DPBar.Value = 100
                Process.Start(Filename)
                End
            Case vbCancel
                Dim SaveFile As SaveFileDialog = New SaveFileDialog
                With SaveFile
                    .Filter = "MSI Files (*.msi)|*.msi"
                    .FileName = System.IO.Path.GetFileName(Filename)
                End With
                If SaveFile.ShowDialog() = DialogResult.OK Then
                    My.Computer.FileSystem.MoveFile(Filename, SaveFile.FileName, True)
                End If
                DPBar.Value = 100
                Threading.Thread.Sleep(1000)
                Me.Download_Form.Close()
                Me.Close()
        End Select
    End Sub
End Class
