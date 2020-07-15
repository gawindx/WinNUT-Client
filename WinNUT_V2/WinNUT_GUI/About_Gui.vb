Public Class About_Gui
    Private LogFile As Logger
    Public Enum LogLvl
        LOG_NOTICE
        LOG_WARNING
        LOG_ERROR
        LOG_DEBUG
    End Enum
    Private Sub About_Gui_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Lbl_ProgNameVersion.Text = WinNUT_Globals.LongProgramName & vbNewLine & "Version " & WinNUT_Globals.ProgramVersion
        Lbl_Copyright_2019.Text = "Copyright Gawinx (Decaux Nicolas)" & vbNewLine & "2019-" & Now.Year
        Me.Icon = WinNUT.Icon
        Me.LogFile = WinNUT.LogFile
        LogFile.LogTracing("Load About Gui", LogLvl.LOG_DEBUG, Me)
    End Sub

    Private Sub Btn_OK_Click(sender As Object, e As EventArgs) Handles Btn_OK.Click
        LogFile.LogTracing("Close About Gui", LogLvl.LOG_DEBUG, Me)
        Me.Close()
    End Sub

    Private Sub LkLbl_Github_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LkLbl_Github.LinkClicked
        System.Diagnostics.Process.Start(sender.Text)
    End Sub
End Class