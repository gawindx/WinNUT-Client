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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Shutdown_Gui))
        Me.Grace_Button = New System.Windows.Forms.Button()
        Me.ShutDown_Btn = New System.Windows.Forms.Button()
        Me.lbl_UPSStatus = New System.Windows.Forms.Label()
        Me.Run_Timer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Grace_Button
        '
        resources.ApplyResources(Me.Grace_Button, "Grace_Button")
        Me.Grace_Button.Name = "Grace_Button"
        Me.Grace_Button.UseVisualStyleBackColor = True
        Me.Grace_Button.UseWaitCursor = True
        '
        'ShutDown_Btn
        '
        resources.ApplyResources(Me.ShutDown_Btn, "ShutDown_Btn")
        Me.ShutDown_Btn.Name = "ShutDown_Btn"
        Me.ShutDown_Btn.UseVisualStyleBackColor = True
        Me.ShutDown_Btn.UseWaitCursor = True
        '
        'lbl_UPSStatus
        '
        resources.ApplyResources(Me.lbl_UPSStatus, "lbl_UPSStatus")
        Me.lbl_UPSStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lbl_UPSStatus.Name = "lbl_UPSStatus"
        Me.lbl_UPSStatus.UseWaitCursor = True
        '
        'Run_Timer
        '
        Me.Run_Timer.Enabled = True
        Me.Run_Timer.Interval = 1000
        '
        'Shutdown_Gui
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lbl_UPSStatus)
        Me.Controls.Add(Me.ShutDown_Btn)
        Me.Controls.Add(Me.Grace_Button)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Shutdown_Gui"
        Me.TopMost = True
        Me.UseWaitCursor = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Grace_Button As Button
    Friend WithEvents ShutDown_Btn As Button
    Friend WithEvents lbl_UPSStatus As Label
    Friend WithEvents Run_Timer As Timer
End Class
