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
