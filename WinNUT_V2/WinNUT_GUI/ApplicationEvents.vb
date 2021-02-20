' WinNUT is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Namespace My
    ' Les événements suivants sont disponibles pour MyApplication :
    ' Startup : Déclenché au démarrage de l'application avant la création du formulaire de démarrage.
    ' Shutdown : Déclenché après la fermeture de tous les formulaires de l'application.  Cet événement n'est pas déclenché si l'application se termine de façon anormale.
    ' UnhandledException : Déclenché si l'application rencontre une exception non gérée.
    ' StartupNextInstance : Déclenché lors du lancement d'une application à instance unique et si cette application est déjà active. 
    ' NetworkAvailabilityChanged : Déclenché quand la connexion réseau est connectée ou déconnectée.
    Partial Friend Class MyApplication
        Private CrashBug_Form As New Form
        Private BtnClose As New Button
        Private BtnGenerate As New Button
        Private Msg_Crash As New Label
        Private Msg_Error As New TextBox
        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            e.ExitApplication = False

            Dim Frms As New FormCollection
            Frms = Application.OpenForms()

            With Msg_Crash
                .Location = New Point(6, 6)
                .Text = "WinNUT has encountered a critical error and will close soon." & vbNewLine &
                    "You can :" & vbNewLine &
                    "- generate a crash report which will contain most of the configured parameters (without sensitive" & vbNewLine &
                    "  information such as your connection information to your NUT server), the last 50 events logged" & vbNewLine &
                    "  and the error message displayed below." & vbNewLine &
                    "  This information will Then be copied To your clipboard For easy reporting." & vbNewLine &
                    "- simply close WinNUT without generating a report."
                .Size = New Point(470, 100)
            End With

            Dim Exception_data As String = String.Format("Exception type: {0}" & vbNewLine & "Exception message: {1}" & vbNewLine & "Exception stack trace: " & vbNewLine & "{2}",
                                               e.Exception.GetType.ToString, e.Exception.Message, e.Exception.StackTrace)
            With Msg_Error
                .Location = New Point(6, 110)
                .Multiline = True
                .ScrollBars = ScrollBars.Vertical
                .ReadOnly = True
                .Text = Exception_data
                .Size = New Point(470, 300)
            End With

            With BtnClose
                .Location = New Point(370, 425)
                .TextAlign = ContentAlignment.MiddleCenter
                .Text = "Close WinNUT"
                .Size = New Point(100, 25)
            End With

            With BtnGenerate
                .Location = New Point(160, 425)
                .TextAlign = ContentAlignment.MiddleCenter
                .Text = "Generate Report and Close WinNUT"
                .Size = New Point(200, 25)
            End With

            With CrashBug_Form
                .Icon = Resources.WinNut
                .Size = New Point(500, 500)
                .FormBorderStyle = FormBorderStyle.Sizable
                .MaximizeBox = False
                .MinimizeBox = False
                .StartPosition = FormStartPosition.CenterParent
                .Text = "Critical Error Occurred in WinNUT"
                .Controls.Add(Msg_Crash)
                .Controls.Add(Msg_Error)
                .Controls.Add(BtnClose)
                .Controls.Add(BtnGenerate)
            End With

            AddHandler BtnClose.Click, AddressOf My.Application.Close_Button_Click
            AddHandler BtnGenerate.Click, AddressOf My.Application.Generate_Button_Click
            AddHandler CrashBug_Form.FormClosing, AddressOf My.Application.CrashBug_FormClosing

            CrashBug_Form.Show()
            CrashBug_Form.BringToFront()
            WinNUT.HasCrased = True
        End Sub

        Private Sub CrashBug_FormClosing(sender As Object, e As FormClosingEventArgs)
            End
        End Sub
        Private Sub Close_Button_Click(sender As Object, e As EventArgs)
            End
        End Sub

        Private Sub Generate_Button_Click(sender As Object, e As EventArgs)
            'Generate a bug report with all essential datas 
            Dim Crash_Report As String = "WinNUT Bug Report" & vbNewLine
            Dim WinNUT_Config As New Dictionary(Of String, Object)(WinNUT_Params.Arr_Reg_Key)
            Dim WinNUT_UserData_Dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\WinNUT-Client"
            Dim CrashLog_Dir = WinNUT_UserData_Dir & "\CrashLog"
            Dim CrashLog_Filename As String = "Crash_Report_" & Format(Now, "dd-MM-yyyy") & "_" &
                String.Format("{0}-{1}-{2}.txt", Now.Hour.ToString("00"), Now.Minute.ToString("00"), Now.Second.ToString("00"))

            For Each kvp As KeyValuePair(Of String, Object) In WinNUT_Params.Arr_Reg_Key
                Select Case kvp.Key
                    Case "ServerAddress", "Port", "UPSName", "NutLogin", "NutPassword"
                        WinNUT_Config.Remove(kvp.Key)
                End Select
            Next

            Crash_Report &= "Os Version : " & My.Computer.Info.OSVersion & vbNewLine
            Crash_Report &= "WinNUT Version : " & System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString & vbNewLine

            Crash_Report &= vbNewLine & "WinNUT Parameters : " & vbNewLine

            Crash_Report &= Newtonsoft.Json.JsonConvert.SerializeObject(WinNUT_Config, Newtonsoft.Json.Formatting.Indented) & vbNewLine
            Crash_Report &= vbNewLine & "Error Message : " & vbNewLine
            Crash_Report &= Msg_Error.Text & vbNewLine & vbNewLine
            Crash_Report &= "Last Events :" & vbNewLine

            For Each WinNUT_Event In WinNUT.LogFile.LastEvents
                Crash_Report &= WinNUT_Event & vbNewLine
            Next
            My.Computer.Clipboard.SetText(Crash_Report)

            If Not My.Computer.FileSystem.DirectoryExists(CrashLog_Dir) Then
                My.Computer.FileSystem.CreateDirectory(CrashLog_Dir)
            End If

            Dim CrashLog_Report As System.IO.StreamWriter
            CrashLog_Report = My.Computer.FileSystem.OpenTextFileWriter(CrashLog_Dir & "\" & CrashLog_Filename, True)
            CrashLog_Report.WriteLine(Crash_Report)
            CrashLog_Report.Close()
            End
        End Sub
    End Class
End Namespace
