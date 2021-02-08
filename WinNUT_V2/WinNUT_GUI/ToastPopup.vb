' WinNUT is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Class ToastPopup
    'Public toastCollectionId As String = "WinNUTToastCollection"
    'Create a toast collection
    'Public Async Sub CreateToastCollection(ByVal IconUri As String)
    'Public Sub CreateToastCollection(ByVal IconUri As String)
    'Dim displayName As String = "WinNUT"
    'Dim launchArg As String = "WinNUTNotifications"
    'Dim IconToast As Uri = New Uri(IconUri)

    'Constructor
    'Dim WinNutToastCollection As Windows.UI.Notifications.ToastCollection = New Windows.UI.Notifications.ToastCollection(
    'Me.toastCollectionId,
    '       displayName,
    '       launchArg,
    '       IconToast)
    'Calls the platform to create the collection
    '   Await Windows.UI.Notifications.ToastNotificationManager.GetDefault.GetToastCollectionManager.SaveToastCollectionAsync(WinNutToastCollection).GetResults()
    'GetDefault().GetToastCollectionManager().SaveToastCollectionAsync(WinNutToastCollection)
    'Windows.UI.Notifications.ToastNotificationManager.GetDefault().GetToastCollectionManager().SaveToastCollectionAsync(WinNutToastCollection)
    ' End Sub
    Public Sub SendToast(ByVal ToastParts As String())
        'Get a toast XML template
        Dim TemplateToast As Windows.UI.Notifications.ToastTemplateType
        If ToastParts.Count >= 3 Then
            TemplateToast = Windows.UI.Notifications.ToastTemplateType.ToastText04
        Else
            TemplateToast = Windows.UI.Notifications.ToastTemplateType.ToastText02
        End If

        Dim toastXml As Windows.Data.Xml.Dom.XmlDocument = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(TemplateToast)

        'Fill in the text elements
        Dim stringElements As Windows.Data.Xml.Dom.XmlNodeList = toastXml.GetElementsByTagName("text")
        For i = 0 To ((ToastParts.Count - 1) And (stringElements.Count - 1)) Step 1
            stringElements.Item(i).InnerText = ToastParts.ElementAt(i)
        Next

        'Specify the absolute path to an image
        'Dim imagePath As String = "pack://application:,,,/Resources/WinNut.ico"
        'Dim imageElements As Windows.Data.Xml.Dom.XmlNodeList = toastXml.GetElementsByTagName("image")
        'imageElements.Item(0).Attributes.GetNamedItem("src").NodeValue = imagePath

        'Create the toast And attach event listeners
        Dim toast As Windows.UI.Notifications.ToastNotification = New Windows.UI.Notifications.ToastNotification(toastXml)
        'toast.Activated += ToastActivated
        'toast.Dismissed += ToastDismissed
        'toast.Failed += ToastFailed

        'Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
        Dim AppId As String = WinNUT_Globals.ProgramName & " - " & WinNUT_Globals.ProgramVersion
        Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier(AppId).Show(toast)
    End Sub
End Class
