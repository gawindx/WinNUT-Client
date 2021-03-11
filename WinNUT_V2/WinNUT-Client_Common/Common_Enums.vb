' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Public Enum AppIconIdx
    IDX_BATT_0 = 1
    IDX_BATT_25 = 2
    IDX_BATT_50 = 4
    IDX_BATT_75 = 8
    IDX_BATT_100 = 16
    IDX_OL = 32
    WIN_DARK = 64
    IDX_ICO_OFFLINE = 128
    IDX_ICO_RETRY = 256
    IDX_ICO_VIEWLOG = 2001
    IDX_ICO_DELETELOG = 2002
    IDX_OFFSET = 1024
End Enum

Public Enum LogLvl
    LOG_NOTICE
    LOG_WARNING
    LOG_ERROR
    LOG_DEBUG
End Enum

'Define Resource Str
Public Enum AppResxStr
    STR_MAIN_OLDINI_RENAMED
    STR_MAIN_OLDINI
    STR_MAIN_RECONNECT
    STR_MAIN_RETRY
    STR_MAIN_NOTCONN
    STR_MAIN_CONN
    STR_MAIN_OL
    STR_MAIN_OB
    STR_MAIN_LOWBAT
    STR_MAIN_BATOK
    STR_MAIN_UNKNOWN_UPS
    STR_MAIN_LOSTCONNECT
    STR_MAIN_INVALIDLOGIN
    STR_MAIN_EXITSLEEP
    STR_MAIN_GOTOSLEEP
    STR_UP_AVAIL
    STR_UP_SHOW
    STR_UP_HIDE
    STR_UP_UPMSG
    STR_UP_DOWNFROM
    STR_SHUT_STAT
    STR_APP_SHUT
    STR_LOG_PREFS
    STR_LOG_CONNECTED
    STR_LOG_CON_FAILED
    STR_LOG_CON_RETRY
    STR_LOG_LOGOFF
    STR_LOG_NEW_RETRY
    STR_LOG_STOP_RETRY
    STR_LOG_SHUT_START
    STR_LOG_SHUT_STOP
    STR_LOG_NO_UPDATE
    STR_LOG_UPDATE
    STR_LOG_NUT_FSD
End Enum

' Define possible responses according to NUT protcol v1.2
Public Enum NUTResponse
    OK
    VAR
    ACCESSDENIED
    UNKNOWNUPS
    VARNOTSUPPORTED
    CMDNOTSUPPORTED
    INVALIDARGUMENT
    INSTCMDFAILED
    SETFAILED
    [READONLY]
    TOOLONG
    FEATURENOTSUPPORTED
    FEATURENOTCONFIGURED
    ALREADYSSLMODE
    DRIVERNOTCONNECTED
    DATASTALE
    ALREADYLOGGEDIN
    INVALIDPASSWORD
    ALREADYSETPASSWORD
    INVALIDUSERNAME
    ALREADYSETUSERNAME
    USERNAMEREQUIRED
    PASSWORDREQUIRED
    UNKNOWNCOMMAND
    INVALIDVALUE
    NORESPONSE
    BEGINLIST
    ENDLIST
End Enum

Public Enum Nut_Exception_Value
    <StringValue("Unable to create connection :")>
    CONNECT_ERROR
    <StringValue("Invalid Username.")>
    INVALID_USERNAME
    <StringValue("Invalid Password.")>
    INVALID_PASSWORD
    <StringValue("Access is denied.")>
    ACCESS_DENIED
    <StringValue("Incorrect login ID / password.")>
    INVALID_AUTH_DATA
    <StringValue("Authentification errork")>
    LOGIN_ERROR
    <StringValue("Connection to Nut Host seem broken when querying : ")>
    SOCKET_BROKEN
    <StringValue("Unknown Login error: ")>
    UNKNOWN_LOGIN_ERROR
    <StringValue("Unknown UPS Name.")>
    UNKNOWN_UPS
End Enum

Public Enum UPS_States
    OL = 1
    OB = 2
    LBHB = 4
    CHRG = 8
    DISCHRG = 16
    FSD = 32
    BYPASS = 64
    CAL = 128
    OFF = 256
    OVER = 512
    TRIM = 1024
    BOOST = 2048
End Enum
