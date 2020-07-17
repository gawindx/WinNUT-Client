Public Class Update_Gui
    Private ChangeLogByteSize As Long
    Private MSIByteSize As Long
    Private LogFile As Logger
    Private Const URLStable As String = "https://raw.githubusercontent.com/gawindx/WinNUT-Client/master/changelog.txt"
    Private Const URLDev As String = "https://raw.githubusercontent.com/gawindx/WinNUT-Client/Dev/changelog.txt"
    Private MSIStable As String = "https://github.com/gawindx/WinNUT-Client/releases/download/v{0}/WinNUT-Setup.msi"
    Private MSIDev As String = "https://github.com/gawindx/WinNUT-Client/releases/download/v{0}-dev/WinNUT-Setup.msi"
    Private WithEvents WebC As New System.Net.WebClient
    Private ChangeLogFile As Object
    Private sChangeLog As String
    Private ReadOnly ManualUpdate As Boolean = False
    Private UpdateInfoRetrieved As Boolean = False
    Private HasUpdate As Boolean = False
    Private NewVersion As String
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
            Me.LogFile = WinNUT.LogFile
            VerifyUpdate()
        End If
    End Sub

    Public Function GetDownloadSize(ByVal URL As String) As Long
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
            Dim Bidule = WinNUT_Params.Arr_Reg_Key.Item("DelayBetweenEachVerification")
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
                Me.ChangeLogFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString()
                Dim URL_Changelog As String = ""
                If WinNUT_Params.Arr_Reg_Key.Item("StableOrDevBranch") = 0 Then
                    URL_Changelog = URLStable
                    LogFile.LogTracing("Verify Update from Stable branch", LogLvl.LOG_DEBUG, Me)
                Else
                    URL_Changelog = URLDev
                    LogFile.LogTracing("Verify Update from Developpment branch", LogLvl.LOG_DEBUG, Me)

                End If
                Me.ChangeLogByteSize = GetDownloadSize(URL_Changelog)

                AddHandler WebC.DownloadFileCompleted, AddressOf Changelog_Downloaded
                WebC.DownloadFileAsync(New Uri(URL_Changelog), ChangeLogFile)
            Else
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Changelog_Downloaded()
        Dim ChgLogInfo = New System.IO.FileInfo(ChangeLogFile)
        Dim srFileReader As System.IO.StreamReader
        If ChangeLogByteSize = ChgLogInfo.Length() Then
            Dim ChangeLogDiff As New List(Of String)
            Dim newline As String = ""
            Dim sInputLine As String
            Dim HighestVersion As Integer = Nothing
            srFileReader = System.IO.File.OpenText(ChangeLogFile)
            Dim ActualVersion As Integer = CInt(Strings.Replace(WinNUT_Globals.ProgramVersion, ".", ""))
            ActualVersion = 1600
            Do
                sInputLine = srFileReader.ReadLine()
                If Strings.InStr(sInputLine, "History") = 0 Then
                    If sInputLine <> "" Then
                        Dim LogVersion As Integer
                        Dim sPattern As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("[Vv]ersion.*(\d+).*(\d+).*(\d+).*(\d+)$")
                        Dim RegExResult = sPattern.Match(sInputLine)
                        If RegExResult.Groups.Count > 1 Then
                            Dim ResultVersion As String = ""
                            Dim Index As Integer = 1
                            For Each Match In sPattern.Match(sInputLine).Groups
                                If Match.Value <> sPattern.Match(sInputLine).Groups(0).Value Then
                                    Index += 1
                                    ResultVersion &= Match.Value
                                    If sPattern.Match(sInputLine).Groups.Count > Index Then
                                        ResultVersion &= "."
                                    End If
                                End If
                            Next
                            If HighestVersion = Nothing Then
                                HighestVersion = CInt(Strings.Replace(ResultVersion, ".", ""))
                                Me.NewVersion = ResultVersion
                            End If
                            LogVersion = CInt(Strings.Replace(ResultVersion, ".", ""))
                            If LogVersion > ActualVersion Then
                                If ChangeLogDiff Is Nothing Then
                                    ChangeLogDiff.Add(sInputLine)
                                ElseIf ChangeLogDiff.Count >= 1 Then
                                    ChangeLogDiff.Add("")
                                End If
                                ChangeLogDiff.Add(sInputLine)
                            Else
                                Exit Do
                            End If
                        Else
                            If LogVersion > ActualVersion Then
                                sPattern = New System.Text.RegularExpressions.Regex("^.*:.*$")
                                RegExResult = sPattern.Match(sInputLine)
                                Dim ResultVersion As String = ""
                                For Each Match In sPattern.Match(sInputLine).Groups
                                    ResultVersion &= Match.Value
                                Next
                                If RegExResult.Groups.Count = 1 Then
                                    If newline <> "" Then
                                        ChangeLogDiff.Add(newline)
                                        newline = ""
                                    End If
                                    newline &= System.Text.RegularExpressions.Regex.Replace(Trim(sInputLine), "\s+", " ")
                                Else
                                    newline &= System.Text.RegularExpressions.Regex.Replace(Trim(sInputLine), "\s+", " ")
                                End If
                            End If
                        End If
                    Else
                        ChangeLogDiff.Add(newline)
                        newline = ""
                    End If

                End If
            Loop Until sInputLine Is Nothing
            If ChangeLogDiff IsNot Nothing Then
                If ChangeLogDiff.Count > 0 Then
                    Me.sChangeLog = ""
                    For Each Line In ChangeLogDiff
                        Me.sChangeLog &= Line & vbNewLine
                    Next
                    Me.HasUpdate = True
                    LogFile.LogTracing(Format("New Version Available : {0}", HighestVersion), LogLvl.LOG_DEBUG, Me)
                Else
                    HighestVersion = Nothing
                    LogFile.LogTracing("No Update Available", LogLvl.LOG_DEBUG, Me)

                End If
            End If
            srFileReader.Close()
        Else
            LogFile.LogTracing("Cannot download changelog.txt", LogLvl.LOG_ERROR, Me)
        End If
        Try
            My.Computer.FileSystem.DeleteFile(ChangeLogFile)
        Catch excep As Exception
            LogFile.LogTracing(excep.Message, LogLvl.LOG_ERROR, Me)
        End Try
        WinNUT_Params.Arr_Reg_Key.Item("LastDateVerification") = Now.ToString
        WinNUT_Params.Save_Params()
        Me.UpdateInfoRetrieved = True
        Me.Show()
    End Sub

    Private Sub Update_Gui_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        TB_ChgLog.Text = sChangeLog
        TB_ChgLog.Visible = False
        Lbl.Text = String.Format("Update Available : Version {0}", Me.NewVersion)
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
            ShowLog_Button.Text = "Show ChangeLog"
            GB1.Size = New Point(GB1.Size.Width, GB1.Size.Height - 160)
            Update_Btn.Location = New Point(Update_Btn.Location.X, Update_Btn.Location.Y - 160)
            ShowLog_Button.Location = New Point(ShowLog_Button.Location.X, ShowLog_Button.Location.Y - 160)
            Close_Btn.Location = New Point(Close_Btn.Location.X, Close_Btn.Location.Y - 160)
            Me.Size = New Point(Me.Size.Width, Me.Size.Height - 160)
        Else
            TB_ChgLog.Visible = True
            ShowLog_Button.Text = "Hide ChangeLog"
            GB1.Size = New Point(GB1.Size.Width, GB1.Size.Height + 160)
            Update_Btn.Location = New Point(Update_Btn.Location.X, Update_Btn.Location.Y + 160)
            ShowLog_Button.Location = New Point(ShowLog_Button.Location.X, ShowLog_Button.Location.Y + 160)
            Close_Btn.Location = New Point(Close_Btn.Location.X, Close_Btn.Location.Y + 160)
            Me.Size = New Point(Me.Size.Width, Me.Size.Height + 160)
        End If
    End Sub

    Private Sub Update_Btn_Click(sender As Object, e As EventArgs) Handles Update_Btn.Click
        Dim MSIURL As String = Nothing
        Me.NewVersion = "2.0.0.0"
        If WinNUT_Params.Arr_Reg_Key.Item("StableOrDevBranch") = 0 Then
            MSIURL = String.Format(Me.MSIStable, Me.NewVersion)
        Else
            MSIURL = String.Format(Me.MSIDev, Me.NewVersion)
        End If
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
            .Text = "Download New Version from :" & vbNewLine & MSIURL
            .Size = New Point(280, (.Size.Height * 2))
        End With
        Download_Form.Show()
        'Me.MSIByteSize = GetDownloadSize(MSIURL)
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
        Dim BytesToReceive = e.TotalBytesToReceive
        If BytesToReceive = 0 Then
            BytesToReceive = Me.MSIByteSize
        End If
        DPBar.Text = String.Format("{0:N}Kb / {1:N}Kb", (e.BytesReceived / 1024), (BytesToReceive / 1024))
    End Sub

    Private Sub New_Version_Downloaded(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)
        Dim Filename = sender.QueryString.Item("FileName")
        Dim Question As String = "Update WinNUT-Client Now?" & vbNewLine
        Question &= "Ok to Close WinNut and Install New Version" & vbNewLine
        Question &= "Cancel to Save Msi and Install Later"
        Select Case MsgBox(Question, vbOKCancel, "Update Now")
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
                    My.Computer.FileSystem.MoveFile(Filename, SaveFile.FileName)
                End If
                DPBar.Value = 100
                Threading.Thread.Sleep(1000)
                Me.Close()
        End Select
    End Sub
End Class