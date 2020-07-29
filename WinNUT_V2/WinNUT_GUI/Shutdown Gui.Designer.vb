<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Shutdown_Gui
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Grace_Button = New System.Windows.Forms.Button()
        Me.ShutDown_Btn = New System.Windows.Forms.Button()
        Me.lbl_UPSStatus = New System.Windows.Forms.Label()
        Me.Run_Timer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Grace_Button
        '
        Me.Grace_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Grace_Button.Location = New System.Drawing.Point(10, 70)
        Me.Grace_Button.Name = "Grace_Button"
        Me.Grace_Button.Size = New System.Drawing.Size(195, 74)
        Me.Grace_Button.TabIndex = 0
        Me.Grace_Button.Text = "Grace Time"
        Me.Grace_Button.UseVisualStyleBackColor = True
        '
        'ShutDown_Btn
        '
        Me.ShutDown_Btn.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShutDown_Btn.Location = New System.Drawing.Point(216, 70)
        Me.ShutDown_Btn.Name = "ShutDown_Btn"
        Me.ShutDown_Btn.Size = New System.Drawing.Size(195, 74)
        Me.ShutDown_Btn.TabIndex = 1
        Me.ShutDown_Btn.Text = "Immediate Shutdown"
        Me.ShutDown_Btn.UseVisualStyleBackColor = True
        '
        'lbl_UPSStatus
        '
        Me.lbl_UPSStatus.AutoSize = True
        Me.lbl_UPSStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_UPSStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_UPSStatus.Location = New System.Drawing.Point(10, 10)
        Me.lbl_UPSStatus.MaximumSize = New System.Drawing.Size(400, 50)
        Me.lbl_UPSStatus.MinimumSize = New System.Drawing.Size(400, 50)
        Me.lbl_UPSStatus.Name = "lbl_UPSStatus"
        Me.lbl_UPSStatus.Size = New System.Drawing.Size(400, 50)
        Me.lbl_UPSStatus.TabIndex = 2
        Me.lbl_UPSStatus.Text = "Battery_Charge : {WinNUT.UPS_BattCh}" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Remaining Time : {WinNUT.Lbl_VRTime.Text}"
        Me.lbl_UPSStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Run_Timer
        '
        Me.Run_Timer.Enabled = True
        Me.Run_Timer.Interval = 1000
        '
        'Shutdown_Gui
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 184)
        Me.Controls.Add(Me.lbl_UPSStatus)
        Me.Controls.Add(Me.ShutDown_Btn)
        Me.Controls.Add(Me.Grace_Button)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Shutdown_Gui"
        Me.Text = "Shutdown Gui"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Grace_Button As Button
    Friend WithEvents ShutDown_Btn As Button
    Friend WithEvents lbl_UPSStatus As Label
    Friend WithEvents Run_Timer As Timer
End Class
