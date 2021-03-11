' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

'Original code taken from: https://www.codeguru.com/columns/vb/creating-visual-basic-string-enums.html

Imports System.Reflection

Public Class StringValue
    Inherits System.Attribute
    Private ReadOnly _value As String

    Public Sub New(value As String)
        _value = value
    End Sub

    Public ReadOnly Property Value() As String
        Get
            Return _value
        End Get
    End Property
End Class

Public NotInheritable Class StringEnum
    Private Sub New()
    End Sub

    Public Shared Function GetStringValue(value As [Enum]) As String
        Dim output As String = Nothing
        Dim type As Type = value.[GetType]()
        Dim fi As FieldInfo = type.GetField(value.ToString())
        Dim attrs As StringValue() =
           TryCast(fi.GetCustomAttributes(GetType(StringValue),
           False), StringValue())

        If attrs.Length > 0 Then
            output = attrs(0).Value
        End If
        Return output
    End Function
End Class
