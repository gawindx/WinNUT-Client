Module WinNUT_Globals
    Public LongProgramName As String
    Public ProgramName As String
    Public ProgramVersion As String
    Public IsConnected As Boolean
    Public LogFile As String
    Public AppIcon As Dictionary(Of Integer, Icon)
    Public Sub Init_Globals()
        LongProgramName = My.Resources.LongProgramName
        ProgramName = My.Resources.ProgramName
        ProgramVersion = My.Resources.ProgramVersion
        IsConnected = False
        LogFile = Application.StartupPath() & "\WinNUT-CLient.log"
    End Sub

End Module
