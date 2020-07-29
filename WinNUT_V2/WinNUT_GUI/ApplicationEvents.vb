Namespace My
    ' Les événements suivants sont disponibles pour MyApplication :
    ' Startup : Déclenché au démarrage de l'application avant la création du formulaire de démarrage.
    ' Shutdown : Déclenché après la fermeture de tous les formulaires de l'application.  Cet événement n'est pas déclenché si l'application se termine de façon anormale.
    ' UnhandledException : Déclenché si l'application rencontre une exception non gérée.
    ' StartupNextInstance : Déclenché lors du lancement d'une application à instance unique et si cette application est déjà active. 
    ' NetworkAvailabilityChanged : Déclenché quand la connexion réseau est connectée ou déconnectée.
    Partial Friend Class MyApplication
        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            My.Application.Log.WriteException(e.Exception,
                TraceEventType.Critical,
                WinNUT_Globals.StrLog.Item(AppResxStr.STR_APP_SHUT) &
                My.Computer.Clock.GmtTime.ToString)
        End Sub
    End Class
End Namespace
