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
        Me.TView_UPSVar.Location = New System.Drawing.Point(6, 6)
        Me.TView_UPSVar.Name = "TView_UPSVar"
        Me.TView_UPSVar.PathSeparator = "."
        Me.TView_UPSVar.Size = New System.Drawing.Size(360, 170)
        Me.TView_UPSVar.TabIndex = 0
        '
        'GB1
        '
        Me.GB1.Controls.Add(Me.Lbl_D_Value)
        Me.GB1.Controls.Add(Me.Lbl_V_Value)
        Me.GB1.Controls.Add(Me.Lbl_N_Value)
        Me.GB1.Controls.Add(Me.Lbl_D)
        Me.GB1.Controls.Add(Me.Lbl_V)
        Me.GB1.Controls.Add(Me.Lbl_Name)
        Me.GB1.Location = New System.Drawing.Point(6, 182)
        Me.GB1.Name = "GB1"
        Me.GB1.Size = New System.Drawing.Size(360, 110)
        Me.GB1.TabIndex = 1
        Me.GB1.TabStop = False
        Me.GB1.Text = "Item Properties"
        '
        'Lbl_D_Value
        '
        Me.Lbl_D_Value.AutoSize = True
        Me.Lbl_D_Value.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_D_Value.Location = New System.Drawing.Point(100, 80)
        Me.Lbl_D_Value.MinimumSize = New System.Drawing.Size(250, 0)
        Me.Lbl_D_Value.Name = "Lbl_D_Value"
        Me.Lbl_D_Value.Size = New System.Drawing.Size(250, 15)
        Me.Lbl_D_Value.TabIndex = 5
        '
        'Lbl_V_Value
        '
        Me.Lbl_V_Value.AutoSize = True
        Me.Lbl_V_Value.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_V_Value.Location = New System.Drawing.Point(100, 50)
        Me.Lbl_V_Value.MinimumSize = New System.Drawing.Size(250, 0)
        Me.Lbl_V_Value.Name = "Lbl_V_Value"
        Me.Lbl_V_Value.Size = New System.Drawing.Size(250, 15)
        Me.Lbl_V_Value.TabIndex = 4
        '
        'Lbl_N_Value
        '
        Me.Lbl_N_Value.AutoSize = True
        Me.Lbl_N_Value.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_N_Value.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Lbl_N_Value.Location = New System.Drawing.Point(100, 20)
        Me.Lbl_N_Value.MinimumSize = New System.Drawing.Size(250, 0)
        Me.Lbl_N_Value.Name = "Lbl_N_Value"
        Me.Lbl_N_Value.Size = New System.Drawing.Size(250, 15)
        Me.Lbl_N_Value.TabIndex = 3
        '
        'Lbl_D
        '
        Me.Lbl_D.AutoSize = True
        Me.Lbl_D.Location = New System.Drawing.Point(6, 80)
        Me.Lbl_D.Name = "Lbl_D"
        Me.Lbl_D.Size = New System.Drawing.Size(66, 13)
        Me.Lbl_D.TabIndex = 2
        Me.Lbl_D.Text = "Description :"
        '
        'Lbl_V
        '
        Me.Lbl_V.AutoSize = True
        Me.Lbl_V.Location = New System.Drawing.Point(6, 50)
        Me.Lbl_V.Name = "Lbl_V"
        Me.Lbl_V.Size = New System.Drawing.Size(40, 13)
        Me.Lbl_V.TabIndex = 1
        Me.Lbl_V.Text = "Value :"
        '
        'Lbl_Name
        '
        Me.Lbl_Name.AutoSize = True
        Me.Lbl_Name.Location = New System.Drawing.Point(6, 20)
        Me.Lbl_Name.Name = "Lbl_Name"
        Me.Lbl_Name.Size = New System.Drawing.Size(41, 13)
        Me.Lbl_Name.TabIndex = 0
        Me.Lbl_Name.Text = "Name :"
        '
        'Btn_Reload
        '
        Me.Btn_Reload.Location = New System.Drawing.Point(9, 300)
        Me.Btn_Reload.Name = "Btn_Reload"
        Me.Btn_Reload.Size = New System.Drawing.Size(75, 23)
        Me.Btn_Reload.TabIndex = 2
        Me.Btn_Reload.Text = "Reload"
        Me.Btn_Reload.UseVisualStyleBackColor = True
        '
        'Btn_Clear
        '
        Me.Btn_Clear.Location = New System.Drawing.Point(102, 300)
        Me.Btn_Clear.Name = "Btn_Clear"
        Me.Btn_Clear.Size = New System.Drawing.Size(75, 23)
        Me.Btn_Clear.TabIndex = 3
        Me.Btn_Clear.Text = "Clear"
        Me.Btn_Clear.UseVisualStyleBackColor = True
        '
        'Btn_Close
        '
        Me.Btn_Close.Location = New System.Drawing.Point(289, 300)
        Me.Btn_Close.Name = "Btn_Close"
        Me.Btn_Close.Size = New System.Drawing.Size(75, 23)
        Me.Btn_Close.TabIndex = 4
        Me.Btn_Close.Text = "Close"
        Me.Btn_Close.UseVisualStyleBackColor = True
        '
        'Timer_Update_List
        '
        Me.Timer_Update_List.Enabled = True
        Me.Timer_Update_List.Interval = 1000
        '
        'Btn_Clip
        '
        Me.Btn_Clip.Location = New System.Drawing.Point(197, 300)
        Me.Btn_Clip.Name = "Btn_Clip"
        Me.Btn_Clip.Size = New System.Drawing.Size(75, 23)
        Me.Btn_Clip.TabIndex = 5
        Me.Btn_Clip.Text = "To Clipboard"
        Me.Btn_Clip.UseVisualStyleBackColor = True
        '
        'List_Var_Gui
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(374, 336)
        Me.Controls.Add(Me.Btn_Clip)
        Me.Controls.Add(Me.Btn_Close)
        Me.Controls.Add(Me.Btn_Clear)
        Me.Controls.Add(Me.Btn_Reload)
        Me.Controls.Add(Me.GB1)
        Me.Controls.Add(Me.TView_UPSVar)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "List_Var_Gui"
        Me.Text = "List UPS Variables"
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
