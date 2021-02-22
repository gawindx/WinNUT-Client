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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Update_Gui))
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
        resources.ApplyResources(Me.GB1, "GB1")
        Me.GB1.Controls.Add(Me.TB_ChgLog)
        Me.GB1.Controls.Add(Me.Lbl)
        Me.GB1.Name = "GB1"
        Me.GB1.TabStop = False
        '
        'TB_ChgLog
        '
        resources.ApplyResources(Me.TB_ChgLog, "TB_ChgLog")
        Me.TB_ChgLog.Name = "TB_ChgLog"
        '
        'Lbl
        '
        resources.ApplyResources(Me.Lbl, "Lbl")
        Me.Lbl.Name = "Lbl"
        '
        'Update_Btn
        '
        resources.ApplyResources(Me.Update_Btn, "Update_Btn")
        Me.Update_Btn.Name = "Update_Btn"
        Me.Update_Btn.UseVisualStyleBackColor = True
        '
        'ShowLog_Button
        '
        resources.ApplyResources(Me.ShowLog_Button, "ShowLog_Button")
        Me.ShowLog_Button.Name = "ShowLog_Button"
        Me.ShowLog_Button.UseVisualStyleBackColor = True
        '
        'Close_Btn
        '
        resources.ApplyResources(Me.Close_Btn, "Close_Btn")
        Me.Close_Btn.Name = "Close_Btn"
        Me.Close_Btn.UseVisualStyleBackColor = True
        '
        'Update_Gui
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Close_Btn)
        Me.Controls.Add(Me.GB1)
        Me.Controls.Add(Me.Update_Btn)
        Me.Controls.Add(Me.ShowLog_Button)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Update_Gui"
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
