<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class About_Gui
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About_Gui))
        Me.GBox = New System.Windows.Forms.GroupBox()
        Me.LkLbl_Github = New System.Windows.Forms.LinkLabel()
        Me.Lbl_Github = New System.Windows.Forms.Label()
        Me.Lbl_Sf = New System.Windows.Forms.Label()
        Me.Lbl_Copyright_2019 = New System.Windows.Forms.Label()
        Me.Lbl_Copyright_2006 = New System.Windows.Forms.Label()
        Me.Lbl_ProgNameVersion = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Btn_OK = New System.Windows.Forms.Button()
        Me.Label_License = New System.Windows.Forms.Label()
        Me.GBox.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GBox
        '
        Me.GBox.Controls.Add(Me.Label_License)
        Me.GBox.Controls.Add(Me.LkLbl_Github)
        Me.GBox.Controls.Add(Me.Lbl_Github)
        Me.GBox.Controls.Add(Me.Lbl_Sf)
        Me.GBox.Controls.Add(Me.Lbl_Copyright_2019)
        Me.GBox.Controls.Add(Me.Lbl_Copyright_2006)
        Me.GBox.Controls.Add(Me.Lbl_ProgNameVersion)
        Me.GBox.Controls.Add(Me.PictureBox1)
        Me.GBox.Location = New System.Drawing.Point(4, 4)
        Me.GBox.Name = "GBox"
        Me.GBox.Size = New System.Drawing.Size(360, 364)
        Me.GBox.TabIndex = 0
        Me.GBox.TabStop = False
        '
        'LkLbl_Github
        '
        Me.LkLbl_Github.AutoSize = True
        Me.LkLbl_Github.Location = New System.Drawing.Point(8, 167)
        Me.LkLbl_Github.Name = "LkLbl_Github"
        Me.LkLbl_Github.Size = New System.Drawing.Size(215, 13)
        Me.LkLbl_Github.TabIndex = 6
        Me.LkLbl_Github.TabStop = True
        Me.LkLbl_Github.Text = "https://github.com/gawindx/WinNUT-Client"
        '
        'Lbl_Github
        '
        Me.Lbl_Github.AutoSize = True
        Me.Lbl_Github.Location = New System.Drawing.Point(8, 154)
        Me.Lbl_Github.Name = "Lbl_Github"
        Me.Lbl_Github.Size = New System.Drawing.Size(146, 13)
        Me.Lbl_Github.TabIndex = 5
        Me.Lbl_Github.Text = "Source Available from GitHub"
        '
        'Lbl_Sf
        '
        Me.Lbl_Sf.AutoSize = True
        Me.Lbl_Sf.Location = New System.Drawing.Point(8, 119)
        Me.Lbl_Sf.Name = "Lbl_Sf"
        Me.Lbl_Sf.Size = New System.Drawing.Size(220, 26)
        Me.Lbl_Sf.TabIndex = 4
        Me.Lbl_Sf.Text = "Based from Winnut Sf" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "https://sourceforge.net/projects/winnutclient"
        '
        'Lbl_Copyright_2019
        '
        Me.Lbl_Copyright_2019.AutoSize = True
        Me.Lbl_Copyright_2019.Location = New System.Drawing.Point(122, 90)
        Me.Lbl_Copyright_2019.Name = "Lbl_Copyright_2019"
        Me.Lbl_Copyright_2019.Size = New System.Drawing.Size(179, 26)
        Me.Lbl_Copyright_2019.TabIndex = 3
        Me.Lbl_Copyright_2019.Text = "Copyright Gawindx (Decaux Nicolas)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2019-2020"
        '
        'Lbl_Copyright_2006
        '
        Me.Lbl_Copyright_2006.AutoSize = True
        Me.Lbl_Copyright_2006.Location = New System.Drawing.Point(122, 53)
        Me.Lbl_Copyright_2006.Name = "Lbl_Copyright_2006"
        Me.Lbl_Copyright_2006.Size = New System.Drawing.Size(137, 26)
        Me.Lbl_Copyright_2006.TabIndex = 2
        Me.Lbl_Copyright_2006.Text = "Copyright Michael Liberman" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "©  2006-2007"
        '
        'Lbl_ProgNameVersion
        '
        Me.Lbl_ProgNameVersion.AutoSize = True
        Me.Lbl_ProgNameVersion.Location = New System.Drawing.Point(119, 20)
        Me.Lbl_ProgNameVersion.Name = "Lbl_ProgNameVersion"
        Me.Lbl_ProgNameVersion.Size = New System.Drawing.Size(184, 13)
        Me.Lbl_ProgNameVersion.TabIndex = 1
        Me.Lbl_ProgNameVersion.Text = "WinNUT_Globals.LongProgramName"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.WinNUT_client.My.Resources.Resources.ups_104x104
        Me.PictureBox1.Location = New System.Drawing.Point(8, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(104, 104)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Btn_OK
        '
        Me.Btn_OK.Location = New System.Drawing.Point(157, 372)
        Me.Btn_OK.Name = "Btn_OK"
        Me.Btn_OK.Size = New System.Drawing.Size(70, 23)
        Me.Btn_OK.TabIndex = 1
        Me.Btn_OK.Text = "OK"
        Me.Btn_OK.UseVisualStyleBackColor = True
        '
        'Label_License
        '
        Me.Label_License.AutoSize = True
        Me.Label_License.Location = New System.Drawing.Point(8, 200)
        Me.Label_License.Name = "Label_License"
        Me.Label_License.Size = New System.Drawing.Size(344, 156)
        Me.Label_License.TabIndex = 7
        Me.Label_License.Text = resources.GetString("Label_License.Text")
        '
        'About_Gui
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 401)
        Me.Controls.Add(Me.Btn_OK)
        Me.Controls.Add(Me.GBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "About_Gui"
        Me.Text = "About"
        Me.GBox.ResumeLayout(False)
        Me.GBox.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GBox As GroupBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Lbl_ProgNameVersion As Label
    Friend WithEvents Lbl_Copyright_2019 As Label
    Friend WithEvents Lbl_Copyright_2006 As Label
    Friend WithEvents Lbl_Github As Label
    Friend WithEvents Lbl_Sf As Label
    Friend WithEvents LkLbl_Github As LinkLabel
    Friend WithEvents Btn_OK As Button
    Friend WithEvents Label_License As Label
End Class
