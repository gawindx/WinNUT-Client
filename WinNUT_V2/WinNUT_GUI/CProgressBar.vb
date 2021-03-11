' WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
' Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)
'
' This program is free software: you can redistribute it and/or modify it under the terms of the
' GNU General Public License as published by the Free Software Foundation, either version 3 of the
' License, or any later version.
'
' This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

Namespace WinFormControls
    Public Class CProgressBar
        Inherits ProgressBar
        Private Const WmPaint = 15
        Protected Overrides ReadOnly Property CreateParams() As CreateParams
            Get
                Dim result As CreateParams = MyBase.CreateParams
                If (Environment.OSVersion.Platform = PlatformID.Win32NT And Environment.OSVersion.Version.Major >= 6) Then
                    Dim vIn() As Byte = New Byte() {0, 2, 0, 0, 0, 0, 0, 0}
                    result.ExStyle = result.ExStyle Or BitConverter.ToInt32(vIn, 0) 'WS_EX_COMPOSITED 
                End If
                Return result
            End Get
        End Property

        Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
            MyBase.WndProc(m)
            If (m.Msg = WmPaint) Then
                Dim graphics As Graphics = CreateGraphics()
                Dim brush As SolidBrush = New SolidBrush(ForeColor)
                Dim textSize As SizeF = graphics.MeasureString(Text, Font)
                graphics.DrawString(Text, Font, brush, (Width - textSize.Width) / 2, (Height - textSize.Height) / 2)
            End If
        End Sub

        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)>
        <System.ComponentModel.Browsable(True)>
        Public Overrides Property Text As String
            Get
                Return MyBase.Text
            End Get
            Set

                MyBase.Text = Value
                Refresh()
            End Set
        End Property

        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)>
        <System.ComponentModel.Browsable(True)>
        Public Overrides Property Font As Font
            Get

                Return MyBase.Font
            End Get
            Set

                MyBase.Font = Value
                Refresh()
            End Set
        End Property
    End Class
End Namespace
