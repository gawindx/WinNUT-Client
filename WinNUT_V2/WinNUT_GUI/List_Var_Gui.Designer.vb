<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class List_Var_Gui
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(List_Var_Gui))
        Me.TView_UPSVar = New System.Windows.Forms.TreeView()
        Me.GB1 = New System.Windows.Forms.GroupBox()
        Me.Lbl_D_Value = New System.Windows.Forms.Label()
        Me.Lbl_V_Value = New System.Windows.Forms.Label()
        Me.Lbl_N_Value = New System.Windows.Forms.Label()
        Me.Lbl_D = New System.Windows.Forms.Label()
        Me.Lbl_V = New System.Windows.Forms.Label()
        Me.Lbl_Name = New System.Windows.Forms.Label()
        Me.Btn_Reload = New System.Windows.Forms.Button()
        Me.Btn_Clear = New System.Windows.Forms.Button()
        Me.Btn_Close = New System.Windows.Forms.Button()
        Me.Timer_Update_List = New System.Windows.Forms.Timer(Me.components)
        Me.Btn_Clip = New System.Windows.Forms.Button()
        Me.GB1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TView_UPSVar
        '
        resources.ApplyResources(Me.TView_UPSVar, "TView_UPSVar")
        Me.TView_UPSVar.Name = "TView_UPSVar"
        Me.TView_UPSVar.PathSeparator = "."
        '
        'GB1
        '
        Me.GB1.Controls.Add(Me.Lbl_D_Value)
        Me.GB1.Controls.Add(Me.Lbl_V_Value)
        Me.GB1.Controls.Add(Me.Lbl_N_Value)
        Me.GB1.Controls.Add(Me.Lbl_D)
        Me.GB1.Controls.Add(Me.Lbl_V)
        Me.GB1.Controls.Add(Me.Lbl_Name)
        resources.ApplyResources(Me.GB1, "GB1")
        Me.GB1.Name = "GB1"
        Me.GB1.TabStop = False
        '
        'Lbl_D_Value
        '
        resources.ApplyResources(Me.Lbl_D_Value, "Lbl_D_Value")
        Me.Lbl_D_Value.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_D_Value.Name = "Lbl_D_Value"
        '
        'Lbl_V_Value
        '
        resources.ApplyResources(Me.Lbl_V_Value, "Lbl_V_Value")
        Me.Lbl_V_Value.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_V_Value.Name = "Lbl_V_Value"
        '
        'Lbl_N_Value
        '
        resources.ApplyResources(Me.Lbl_N_Value, "Lbl_N_Value")
        Me.Lbl_N_Value.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_N_Value.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Lbl_N_Value.Name = "Lbl_N_Value"
        '
        'Lbl_D
        '
        resources.ApplyResources(Me.Lbl_D, "Lbl_D")
        Me.Lbl_D.Name = "Lbl_D"
        '
        'Lbl_V
        '
        resources.ApplyResources(Me.Lbl_V, "Lbl_V")
        Me.Lbl_V.Name = "Lbl_V"
        '
        'Lbl_Name
        '
        resources.ApplyResources(Me.Lbl_Name, "Lbl_Name")
        Me.Lbl_Name.Name = "Lbl_Name"
        '
        'Btn_Reload
        '
        resources.ApplyResources(Me.Btn_Reload, "Btn_Reload")
        Me.Btn_Reload.Name = "Btn_Reload"
        Me.Btn_Reload.UseVisualStyleBackColor = True
        '
        'Btn_Clear
        '
        resources.ApplyResources(Me.Btn_Clear, "Btn_Clear")
        Me.Btn_Clear.Name = "Btn_Clear"
        Me.Btn_Clear.UseVisualStyleBackColor = True
        '
        'Btn_Close
        '
        resources.ApplyResources(Me.Btn_Close, "Btn_Close")
        Me.Btn_Close.Name = "Btn_Close"
        Me.Btn_Close.UseVisualStyleBackColor = True
        '
        'Timer_Update_List
        '
        Me.Timer_Update_List.Interval = 1000
        '
        'Btn_Clip
        '
        resources.ApplyResources(Me.Btn_Clip, "Btn_Clip")
        Me.Btn_Clip.Name = "Btn_Clip"
        Me.Btn_Clip.UseVisualStyleBackColor = True
        '
        'List_Var_Gui
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Btn_Clip)
        Me.Controls.Add(Me.Btn_Close)
        Me.Controls.Add(Me.Btn_Clear)
        Me.Controls.Add(Me.Btn_Reload)
        Me.Controls.Add(Me.GB1)
        Me.Controls.Add(Me.TView_UPSVar)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "List_Var_Gui"
        Me.GB1.ResumeLayout(False)
        Me.GB1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TView_UPSVar As TreeView
    Friend WithEvents GB1 As GroupBox
    Friend WithEvents Lbl_D_Value As Label
    Friend WithEvents Lbl_V_Value As Label
    Friend WithEvents Lbl_N_Value As Label
    Friend WithEvents Lbl_D As Label
    Friend WithEvents Lbl_V As Label
    Friend WithEvents Lbl_Name As Label
    Friend WithEvents Btn_Reload As Button
    Friend WithEvents Btn_Clear As Button
    Friend WithEvents Btn_Close As Button
    Friend WithEvents Timer_Update_List As Timer
    Friend WithEvents Btn_Clip As Button
End Class
