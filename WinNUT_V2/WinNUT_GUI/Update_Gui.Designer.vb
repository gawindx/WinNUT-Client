<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Update_Gui
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
        Me.GB1 = New System.Windows.Forms.GroupBox()
        Me.TB_ChgLog = New System.Windows.Forms.TextBox()
        Me.Lbl = New System.Windows.Forms.Label()
        Me.Update_Btn = New System.Windows.Forms.Button()
        Me.ShowLog_Button = New System.Windows.Forms.Button()
        Me.Close_Btn = New System.Windows.Forms.Button()
        Me.GB1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GB1
        '
        Me.GB1.Controls.Add(Me.TB_ChgLog)
        Me.GB1.Controls.Add(Me.Lbl)
        Me.GB1.Location = New System.Drawing.Point(12, 12)
        Me.GB1.Name = "GB1"
        Me.GB1.Size = New System.Drawing.Size(513, 212)
        Me.GB1.TabIndex = 0
        Me.GB1.TabStop = False
        '
        'TB_ChgLog
        '
        Me.TB_ChgLog.Location = New System.Drawing.Point(6, 42)
        Me.TB_ChgLog.Multiline = True
        Me.TB_ChgLog.Name = "TB_ChgLog"
        Me.TB_ChgLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TB_ChgLog.Size = New System.Drawing.Size(500, 160)
        Me.TB_ChgLog.TabIndex = 1
        '
        'Lbl
        '
        Me.Lbl.AutoSize = True
        Me.Lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lbl.Location = New System.Drawing.Point(6, 16)
        Me.Lbl.MaximumSize = New System.Drawing.Size(500, 20)
        Me.Lbl.MinimumSize = New System.Drawing.Size(500, 20)
        Me.Lbl.Name = "Lbl"
        Me.Lbl.Size = New System.Drawing.Size(500, 20)
        Me.Lbl.TabIndex = 0
        Me.Lbl.Text = "Update Available : Version {}"
        Me.Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Update_Btn
        '
        Me.Update_Btn.Location = New System.Drawing.Point(18, 230)
        Me.Update_Btn.Name = "Update_Btn"
        Me.Update_Btn.Size = New System.Drawing.Size(145, 23)
        Me.Update_Btn.TabIndex = 2
        Me.Update_Btn.Text = "Update"
        Me.Update_Btn.UseVisualStyleBackColor = True
        '
        'ShowLog_Button
        '
        Me.ShowLog_Button.Location = New System.Drawing.Point(196, 230)
        Me.ShowLog_Button.Name = "ShowLog_Button"
        Me.ShowLog_Button.Size = New System.Drawing.Size(145, 23)
        Me.ShowLog_Button.TabIndex = 3
        Me.ShowLog_Button.Text = "Show ChangeLog"
        Me.ShowLog_Button.UseVisualStyleBackColor = True
        '
        'Close_Btn
        '
        Me.Close_Btn.Location = New System.Drawing.Point(374, 230)
        Me.Close_Btn.Name = "Close_Btn"
        Me.Close_Btn.Size = New System.Drawing.Size(145, 23)
        Me.Close_Btn.TabIndex = 4
        Me.Close_Btn.Text = "Close"
        Me.Close_Btn.UseVisualStyleBackColor = True
        '
        'Update_Gui
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 264)
        Me.Controls.Add(Me.Close_Btn)
        Me.Controls.Add(Me.GB1)
        Me.Controls.Add(Me.Update_Btn)
        Me.Controls.Add(Me.ShowLog_Button)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Update_Gui"
        Me.Text = "Update"
        Me.GB1.ResumeLayout(False)
        Me.GB1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GB1 As GroupBox
    Friend WithEvents Lbl As Label
    Friend WithEvents Close_Btn As Button
    Friend WithEvents ShowLog_Button As Button
    Friend WithEvents Update_Btn As Button
    Friend WithEvents TB_ChgLog As TextBox
End Class
