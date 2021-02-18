<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Pref_Gui
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Pref_Gui))
        Me.TabControl_Options = New System.Windows.Forms.TabControl()
        Me.Tab_Connexion = New System.Windows.Forms.TabPage()
        Me.Tb_Pwd_Nut = New System.Windows.Forms.TextBox()
        Me.Tb_Login_Nut = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Lbl_Pwd_Nut = New System.Windows.Forms.Label()
        Me.Lbl_Login_Nut = New System.Windows.Forms.Label()
        Me.Tb_Delay_Com = New System.Windows.Forms.TextBox()
        Me.Tb_UPS_Name = New System.Windows.Forms.TextBox()
        Me.Tb_Port = New System.Windows.Forms.TextBox()
        Me.Tb_Server_IP = New System.Windows.Forms.TextBox()
        Me.Cb_Reconnect = New System.Windows.Forms.CheckBox()
        Me.Lbl_Delay_Com = New System.Windows.Forms.Label()
        Me.Lbl_Name_UPS = New System.Windows.Forms.Label()
        Me.Lbl_Port = New System.Windows.Forms.Label()
        Me.Lbl_Server_IP = New System.Windows.Forms.Label()
        Me.Tab_Calibrage = New System.Windows.Forms.TabPage()
        Me.Cbx_Freq_Input = New System.Windows.Forms.ComboBox()
        Me.Tb_BattV_Max = New System.Windows.Forms.TextBox()
        Me.Tb_Load_Max = New System.Windows.Forms.TextBox()
        Me.Tb_OutV_Max = New System.Windows.Forms.TextBox()
        Me.Tb_InF_Max = New System.Windows.Forms.TextBox()
        Me.Tb_BattV_Min = New System.Windows.Forms.TextBox()
        Me.Tb_Load_Min = New System.Windows.Forms.TextBox()
        Me.Tb_OutV_Min = New System.Windows.Forms.TextBox()
        Me.Tb_InF_Min = New System.Windows.Forms.TextBox()
        Me.Tb_InV_Max = New System.Windows.Forms.TextBox()
        Me.Tb_InV_Min = New System.Windows.Forms.TextBox()
        Me.Lbl_Maxi = New System.Windows.Forms.Label()
        Me.Lbl_Mini = New System.Windows.Forms.Label()
        Me.Lbl_BattV = New System.Windows.Forms.Label()
        Me.Lbl_LoadUPS = New System.Windows.Forms.Label()
        Me.Lbl_OutputV = New System.Windows.Forms.Label()
        Me.Lbl_InputF = New System.Windows.Forms.Label()
        Me.Lbl_PowerF = New System.Windows.Forms.Label()
        Me.Lbl_InputV = New System.Windows.Forms.Label()
        Me.Tab_Miscellanous = New System.Windows.Forms.TabPage()
        Me.Cbx_LogLevel = New System.Windows.Forms.ComboBox()
        Me.Btn_DeleteLog = New System.Windows.Forms.Button()
        Me.Btn_ViewLog = New System.Windows.Forms.Button()
        Me.Lbl_LevelLog = New System.Windows.Forms.Label()
        Me.CB_Use_Logfile = New System.Windows.Forms.CheckBox()
        Me.CB_Start_W_Win = New System.Windows.Forms.CheckBox()
        Me.CB_Close_Tray = New System.Windows.Forms.CheckBox()
        Me.CB_Start_Mini = New System.Windows.Forms.CheckBox()
        Me.CB_Systray = New System.Windows.Forms.CheckBox()
        Me.Tab_Shutdown = New System.Windows.Forms.TabPage()
        Me.CB_Follow_FSD = New System.Windows.Forms.CheckBox()
        Me.Lbl_Percent = New System.Windows.Forms.Label()
        Me.Tb_GraceTime = New System.Windows.Forms.TextBox()
        Me.Cb_ExtendTime = New System.Windows.Forms.CheckBox()
        Me.Tb_Delay_Stop = New System.Windows.Forms.TextBox()
        Me.Cbx_TypeStop = New System.Windows.Forms.ComboBox()
        Me.Cb_ImmediateStop = New System.Windows.Forms.CheckBox()
        Me.Tb_BattLimit_Time = New System.Windows.Forms.TextBox()
        Me.Tb_BattLimit_Load = New System.Windows.Forms.TextBox()
        Me.Lbl_GraceTime = New System.Windows.Forms.Label()
        Me.Lbl_Delay_Stop = New System.Windows.Forms.Label()
        Me.Lbl_StopType = New System.Windows.Forms.Label()
        Me.Lbl_BattLimit_Time = New System.Windows.Forms.Label()
        Me.Lbl_BattLimit_Load = New System.Windows.Forms.Label()
        Me.Tab_Update = New System.Windows.Forms.TabPage()
        Me.Cbx_Branch_Update = New System.Windows.Forms.ComboBox()
        Me.Cbx_Delay_Verif = New System.Windows.Forms.ComboBox()
        Me.Lbl_Branch_Update = New System.Windows.Forms.Label()
        Me.Lbl_Delay_Verif = New System.Windows.Forms.Label()
        Me.Cb_Update_At_Start = New System.Windows.Forms.CheckBox()
        Me.Cb_Verify_Update = New System.Windows.Forms.CheckBox()
        Me.Btn_Ok = New System.Windows.Forms.Button()
        Me.Btn_Apply = New System.Windows.Forms.Button()
        Me.Btn_Cancel = New System.Windows.Forms.Button()
        Me.Pref_TlTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabControl_Options.SuspendLayout()
        Me.Tab_Connexion.SuspendLayout()
        Me.Tab_Calibrage.SuspendLayout()
        Me.Tab_Miscellanous.SuspendLayout()
        Me.Tab_Shutdown.SuspendLayout()
        Me.Tab_Update.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl_Options
        '
        resources.ApplyResources(Me.TabControl_Options, "TabControl_Options")
        Me.TabControl_Options.Controls.Add(Me.Tab_Connexion)
        Me.TabControl_Options.Controls.Add(Me.Tab_Calibrage)
        Me.TabControl_Options.Controls.Add(Me.Tab_Miscellanous)
        Me.TabControl_Options.Controls.Add(Me.Tab_Shutdown)
        Me.TabControl_Options.Controls.Add(Me.Tab_Update)
        Me.TabControl_Options.Name = "TabControl_Options"
        Me.TabControl_Options.SelectedIndex = 0
        Me.Pref_TlTip.SetToolTip(Me.TabControl_Options, resources.GetString("TabControl_Options.ToolTip"))
        '
        'Tab_Connexion
        '
        resources.ApplyResources(Me.Tab_Connexion, "Tab_Connexion")
        Me.Tab_Connexion.Controls.Add(Me.Tb_Pwd_Nut)
        Me.Tab_Connexion.Controls.Add(Me.Tb_Login_Nut)
        Me.Tab_Connexion.Controls.Add(Me.Label3)
        Me.Tab_Connexion.Controls.Add(Me.Lbl_Pwd_Nut)
        Me.Tab_Connexion.Controls.Add(Me.Lbl_Login_Nut)
        Me.Tab_Connexion.Controls.Add(Me.Tb_Delay_Com)
        Me.Tab_Connexion.Controls.Add(Me.Tb_UPS_Name)
        Me.Tab_Connexion.Controls.Add(Me.Tb_Port)
        Me.Tab_Connexion.Controls.Add(Me.Tb_Server_IP)
        Me.Tab_Connexion.Controls.Add(Me.Cb_Reconnect)
        Me.Tab_Connexion.Controls.Add(Me.Lbl_Delay_Com)
        Me.Tab_Connexion.Controls.Add(Me.Lbl_Name_UPS)
        Me.Tab_Connexion.Controls.Add(Me.Lbl_Port)
        Me.Tab_Connexion.Controls.Add(Me.Lbl_Server_IP)
        Me.Tab_Connexion.Name = "Tab_Connexion"
        Me.Pref_TlTip.SetToolTip(Me.Tab_Connexion, resources.GetString("Tab_Connexion.ToolTip"))
        Me.Tab_Connexion.UseVisualStyleBackColor = True
        '
        'Tb_Pwd_Nut
        '
        resources.ApplyResources(Me.Tb_Pwd_Nut, "Tb_Pwd_Nut")
        Me.Tb_Pwd_Nut.Name = "Tb_Pwd_Nut"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Pwd_Nut, resources.GetString("Tb_Pwd_Nut.ToolTip"))
        Me.Tb_Pwd_Nut.UseSystemPasswordChar = True
        '
        'Tb_Login_Nut
        '
        resources.ApplyResources(Me.Tb_Login_Nut, "Tb_Login_Nut")
        Me.Tb_Login_Nut.Name = "Tb_Login_Nut"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Login_Nut, resources.GetString("Tb_Login_Nut.ToolTip"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.Pref_TlTip.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'Lbl_Pwd_Nut
        '
        resources.ApplyResources(Me.Lbl_Pwd_Nut, "Lbl_Pwd_Nut")
        Me.Lbl_Pwd_Nut.Name = "Lbl_Pwd_Nut"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Pwd_Nut, resources.GetString("Lbl_Pwd_Nut.ToolTip"))
        '
        'Lbl_Login_Nut
        '
        resources.ApplyResources(Me.Lbl_Login_Nut, "Lbl_Login_Nut")
        Me.Lbl_Login_Nut.Name = "Lbl_Login_Nut"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Login_Nut, resources.GetString("Lbl_Login_Nut.ToolTip"))
        '
        'Tb_Delay_Com
        '
        resources.ApplyResources(Me.Tb_Delay_Com, "Tb_Delay_Com")
        Me.Tb_Delay_Com.Name = "Tb_Delay_Com"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Delay_Com, resources.GetString("Tb_Delay_Com.ToolTip"))
        '
        'Tb_UPS_Name
        '
        resources.ApplyResources(Me.Tb_UPS_Name, "Tb_UPS_Name")
        Me.Tb_UPS_Name.Name = "Tb_UPS_Name"
        Me.Pref_TlTip.SetToolTip(Me.Tb_UPS_Name, resources.GetString("Tb_UPS_Name.ToolTip"))
        '
        'Tb_Port
        '
        resources.ApplyResources(Me.Tb_Port, "Tb_Port")
        Me.Tb_Port.Name = "Tb_Port"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Port, resources.GetString("Tb_Port.ToolTip"))
        '
        'Tb_Server_IP
        '
        resources.ApplyResources(Me.Tb_Server_IP, "Tb_Server_IP")
        Me.Tb_Server_IP.Name = "Tb_Server_IP"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Server_IP, resources.GetString("Tb_Server_IP.ToolTip"))
        '
        'Cb_Reconnect
        '
        resources.ApplyResources(Me.Cb_Reconnect, "Cb_Reconnect")
        Me.Cb_Reconnect.Name = "Cb_Reconnect"
        Me.Pref_TlTip.SetToolTip(Me.Cb_Reconnect, resources.GetString("Cb_Reconnect.ToolTip"))
        Me.Cb_Reconnect.UseVisualStyleBackColor = True
        '
        'Lbl_Delay_Com
        '
        resources.ApplyResources(Me.Lbl_Delay_Com, "Lbl_Delay_Com")
        Me.Lbl_Delay_Com.Name = "Lbl_Delay_Com"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Delay_Com, resources.GetString("Lbl_Delay_Com.ToolTip"))
        '
        'Lbl_Name_UPS
        '
        resources.ApplyResources(Me.Lbl_Name_UPS, "Lbl_Name_UPS")
        Me.Lbl_Name_UPS.Name = "Lbl_Name_UPS"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Name_UPS, resources.GetString("Lbl_Name_UPS.ToolTip"))
        '
        'Lbl_Port
        '
        resources.ApplyResources(Me.Lbl_Port, "Lbl_Port")
        Me.Lbl_Port.Name = "Lbl_Port"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Port, resources.GetString("Lbl_Port.ToolTip"))
        '
        'Lbl_Server_IP
        '
        resources.ApplyResources(Me.Lbl_Server_IP, "Lbl_Server_IP")
        Me.Lbl_Server_IP.Name = "Lbl_Server_IP"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Server_IP, resources.GetString("Lbl_Server_IP.ToolTip"))
        '
        'Tab_Calibrage
        '
        resources.ApplyResources(Me.Tab_Calibrage, "Tab_Calibrage")
        Me.Tab_Calibrage.Controls.Add(Me.Cbx_Freq_Input)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_BattV_Max)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_Load_Max)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_OutV_Max)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_InF_Max)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_BattV_Min)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_Load_Min)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_OutV_Min)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_InF_Min)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_InV_Max)
        Me.Tab_Calibrage.Controls.Add(Me.Tb_InV_Min)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_Maxi)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_Mini)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_BattV)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_LoadUPS)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_OutputV)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_InputF)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_PowerF)
        Me.Tab_Calibrage.Controls.Add(Me.Lbl_InputV)
        Me.Tab_Calibrage.Name = "Tab_Calibrage"
        Me.Pref_TlTip.SetToolTip(Me.Tab_Calibrage, resources.GetString("Tab_Calibrage.ToolTip"))
        Me.Tab_Calibrage.UseVisualStyleBackColor = True
        '
        'Cbx_Freq_Input
        '
        resources.ApplyResources(Me.Cbx_Freq_Input, "Cbx_Freq_Input")
        Me.Cbx_Freq_Input.FormattingEnabled = True
        Me.Cbx_Freq_Input.Items.AddRange(New Object() {resources.GetString("Cbx_Freq_Input.Items"), resources.GetString("Cbx_Freq_Input.Items1")})
        Me.Cbx_Freq_Input.Name = "Cbx_Freq_Input"
        Me.Pref_TlTip.SetToolTip(Me.Cbx_Freq_Input, resources.GetString("Cbx_Freq_Input.ToolTip"))
        '
        'Tb_BattV_Max
        '
        resources.ApplyResources(Me.Tb_BattV_Max, "Tb_BattV_Max")
        Me.Tb_BattV_Max.Name = "Tb_BattV_Max"
        Me.Pref_TlTip.SetToolTip(Me.Tb_BattV_Max, resources.GetString("Tb_BattV_Max.ToolTip"))
        '
        'Tb_Load_Max
        '
        resources.ApplyResources(Me.Tb_Load_Max, "Tb_Load_Max")
        Me.Tb_Load_Max.Name = "Tb_Load_Max"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Load_Max, resources.GetString("Tb_Load_Max.ToolTip"))
        '
        'Tb_OutV_Max
        '
        resources.ApplyResources(Me.Tb_OutV_Max, "Tb_OutV_Max")
        Me.Tb_OutV_Max.Name = "Tb_OutV_Max"
        Me.Pref_TlTip.SetToolTip(Me.Tb_OutV_Max, resources.GetString("Tb_OutV_Max.ToolTip"))
        '
        'Tb_InF_Max
        '
        resources.ApplyResources(Me.Tb_InF_Max, "Tb_InF_Max")
        Me.Tb_InF_Max.Name = "Tb_InF_Max"
        Me.Pref_TlTip.SetToolTip(Me.Tb_InF_Max, resources.GetString("Tb_InF_Max.ToolTip"))
        '
        'Tb_BattV_Min
        '
        resources.ApplyResources(Me.Tb_BattV_Min, "Tb_BattV_Min")
        Me.Tb_BattV_Min.Name = "Tb_BattV_Min"
        Me.Pref_TlTip.SetToolTip(Me.Tb_BattV_Min, resources.GetString("Tb_BattV_Min.ToolTip"))
        '
        'Tb_Load_Min
        '
        resources.ApplyResources(Me.Tb_Load_Min, "Tb_Load_Min")
        Me.Tb_Load_Min.Name = "Tb_Load_Min"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Load_Min, resources.GetString("Tb_Load_Min.ToolTip"))
        '
        'Tb_OutV_Min
        '
        resources.ApplyResources(Me.Tb_OutV_Min, "Tb_OutV_Min")
        Me.Tb_OutV_Min.Name = "Tb_OutV_Min"
        Me.Pref_TlTip.SetToolTip(Me.Tb_OutV_Min, resources.GetString("Tb_OutV_Min.ToolTip"))
        '
        'Tb_InF_Min
        '
        resources.ApplyResources(Me.Tb_InF_Min, "Tb_InF_Min")
        Me.Tb_InF_Min.Name = "Tb_InF_Min"
        Me.Pref_TlTip.SetToolTip(Me.Tb_InF_Min, resources.GetString("Tb_InF_Min.ToolTip"))
        '
        'Tb_InV_Max
        '
        resources.ApplyResources(Me.Tb_InV_Max, "Tb_InV_Max")
        Me.Tb_InV_Max.Name = "Tb_InV_Max"
        Me.Pref_TlTip.SetToolTip(Me.Tb_InV_Max, resources.GetString("Tb_InV_Max.ToolTip"))
        '
        'Tb_InV_Min
        '
        resources.ApplyResources(Me.Tb_InV_Min, "Tb_InV_Min")
        Me.Tb_InV_Min.Name = "Tb_InV_Min"
        Me.Pref_TlTip.SetToolTip(Me.Tb_InV_Min, resources.GetString("Tb_InV_Min.ToolTip"))
        '
        'Lbl_Maxi
        '
        resources.ApplyResources(Me.Lbl_Maxi, "Lbl_Maxi")
        Me.Lbl_Maxi.Name = "Lbl_Maxi"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Maxi, resources.GetString("Lbl_Maxi.ToolTip"))
        '
        'Lbl_Mini
        '
        resources.ApplyResources(Me.Lbl_Mini, "Lbl_Mini")
        Me.Lbl_Mini.Name = "Lbl_Mini"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Mini, resources.GetString("Lbl_Mini.ToolTip"))
        '
        'Lbl_BattV
        '
        resources.ApplyResources(Me.Lbl_BattV, "Lbl_BattV")
        Me.Lbl_BattV.Name = "Lbl_BattV"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_BattV, resources.GetString("Lbl_BattV.ToolTip"))
        '
        'Lbl_LoadUPS
        '
        resources.ApplyResources(Me.Lbl_LoadUPS, "Lbl_LoadUPS")
        Me.Lbl_LoadUPS.Name = "Lbl_LoadUPS"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_LoadUPS, resources.GetString("Lbl_LoadUPS.ToolTip"))
        '
        'Lbl_OutputV
        '
        resources.ApplyResources(Me.Lbl_OutputV, "Lbl_OutputV")
        Me.Lbl_OutputV.Name = "Lbl_OutputV"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_OutputV, resources.GetString("Lbl_OutputV.ToolTip"))
        '
        'Lbl_InputF
        '
        resources.ApplyResources(Me.Lbl_InputF, "Lbl_InputF")
        Me.Lbl_InputF.Name = "Lbl_InputF"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_InputF, resources.GetString("Lbl_InputF.ToolTip"))
        '
        'Lbl_PowerF
        '
        resources.ApplyResources(Me.Lbl_PowerF, "Lbl_PowerF")
        Me.Lbl_PowerF.Name = "Lbl_PowerF"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_PowerF, resources.GetString("Lbl_PowerF.ToolTip"))
        '
        'Lbl_InputV
        '
        resources.ApplyResources(Me.Lbl_InputV, "Lbl_InputV")
        Me.Lbl_InputV.Name = "Lbl_InputV"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_InputV, resources.GetString("Lbl_InputV.ToolTip"))
        '
        'Tab_Miscellanous
        '
        resources.ApplyResources(Me.Tab_Miscellanous, "Tab_Miscellanous")
        Me.Tab_Miscellanous.Controls.Add(Me.Cbx_LogLevel)
        Me.Tab_Miscellanous.Controls.Add(Me.Btn_DeleteLog)
        Me.Tab_Miscellanous.Controls.Add(Me.Btn_ViewLog)
        Me.Tab_Miscellanous.Controls.Add(Me.Lbl_LevelLog)
        Me.Tab_Miscellanous.Controls.Add(Me.CB_Use_Logfile)
        Me.Tab_Miscellanous.Controls.Add(Me.CB_Start_W_Win)
        Me.Tab_Miscellanous.Controls.Add(Me.CB_Close_Tray)
        Me.Tab_Miscellanous.Controls.Add(Me.CB_Start_Mini)
        Me.Tab_Miscellanous.Controls.Add(Me.CB_Systray)
        Me.Tab_Miscellanous.Name = "Tab_Miscellanous"
        Me.Pref_TlTip.SetToolTip(Me.Tab_Miscellanous, resources.GetString("Tab_Miscellanous.ToolTip"))
        Me.Tab_Miscellanous.UseVisualStyleBackColor = True
        '
        'Cbx_LogLevel
        '
        resources.ApplyResources(Me.Cbx_LogLevel, "Cbx_LogLevel")
        Me.Cbx_LogLevel.FormattingEnabled = True
        Me.Cbx_LogLevel.Items.AddRange(New Object() {resources.GetString("Cbx_LogLevel.Items"), resources.GetString("Cbx_LogLevel.Items1"), resources.GetString("Cbx_LogLevel.Items2"), resources.GetString("Cbx_LogLevel.Items3")})
        Me.Cbx_LogLevel.Name = "Cbx_LogLevel"
        Me.Pref_TlTip.SetToolTip(Me.Cbx_LogLevel, resources.GetString("Cbx_LogLevel.ToolTip"))
        '
        'Btn_DeleteLog
        '
        resources.ApplyResources(Me.Btn_DeleteLog, "Btn_DeleteLog")
        Me.Btn_DeleteLog.Image = Global.WinNUT_client.My.Resources.Resources.Delete_LogFile_24x24
        Me.Btn_DeleteLog.Name = "Btn_DeleteLog"
        Me.Pref_TlTip.SetToolTip(Me.Btn_DeleteLog, resources.GetString("Btn_DeleteLog.ToolTip"))
        Me.Btn_DeleteLog.UseVisualStyleBackColor = True
        '
        'Btn_ViewLog
        '
        resources.ApplyResources(Me.Btn_ViewLog, "Btn_ViewLog")
        Me.Btn_ViewLog.Image = Global.WinNUT_client.My.Resources.Resources.ViewLogFile_24x24
        Me.Btn_ViewLog.Name = "Btn_ViewLog"
        Me.Pref_TlTip.SetToolTip(Me.Btn_ViewLog, resources.GetString("Btn_ViewLog.ToolTip"))
        Me.Btn_ViewLog.UseVisualStyleBackColor = True
        '
        'Lbl_LevelLog
        '
        resources.ApplyResources(Me.Lbl_LevelLog, "Lbl_LevelLog")
        Me.Lbl_LevelLog.Name = "Lbl_LevelLog"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_LevelLog, resources.GetString("Lbl_LevelLog.ToolTip"))
        '
        'CB_Use_Logfile
        '
        resources.ApplyResources(Me.CB_Use_Logfile, "CB_Use_Logfile")
        Me.CB_Use_Logfile.Name = "CB_Use_Logfile"
        Me.Pref_TlTip.SetToolTip(Me.CB_Use_Logfile, resources.GetString("CB_Use_Logfile.ToolTip"))
        Me.CB_Use_Logfile.UseVisualStyleBackColor = True
        '
        'CB_Start_W_Win
        '
        resources.ApplyResources(Me.CB_Start_W_Win, "CB_Start_W_Win")
        Me.CB_Start_W_Win.Name = "CB_Start_W_Win"
        Me.Pref_TlTip.SetToolTip(Me.CB_Start_W_Win, resources.GetString("CB_Start_W_Win.ToolTip"))
        Me.CB_Start_W_Win.UseVisualStyleBackColor = True
        '
        'CB_Close_Tray
        '
        resources.ApplyResources(Me.CB_Close_Tray, "CB_Close_Tray")
        Me.CB_Close_Tray.Name = "CB_Close_Tray"
        Me.Pref_TlTip.SetToolTip(Me.CB_Close_Tray, resources.GetString("CB_Close_Tray.ToolTip"))
        Me.CB_Close_Tray.UseVisualStyleBackColor = True
        '
        'CB_Start_Mini
        '
        resources.ApplyResources(Me.CB_Start_Mini, "CB_Start_Mini")
        Me.CB_Start_Mini.Name = "CB_Start_Mini"
        Me.Pref_TlTip.SetToolTip(Me.CB_Start_Mini, resources.GetString("CB_Start_Mini.ToolTip"))
        Me.CB_Start_Mini.UseVisualStyleBackColor = True
        '
        'CB_Systray
        '
        resources.ApplyResources(Me.CB_Systray, "CB_Systray")
        Me.CB_Systray.Name = "CB_Systray"
        Me.Pref_TlTip.SetToolTip(Me.CB_Systray, resources.GetString("CB_Systray.ToolTip"))
        Me.CB_Systray.UseVisualStyleBackColor = True
        '
        'Tab_Shutdown
        '
        resources.ApplyResources(Me.Tab_Shutdown, "Tab_Shutdown")
        Me.Tab_Shutdown.Controls.Add(Me.CB_Follow_FSD)
        Me.Tab_Shutdown.Controls.Add(Me.Lbl_Percent)
        Me.Tab_Shutdown.Controls.Add(Me.Tb_GraceTime)
        Me.Tab_Shutdown.Controls.Add(Me.Cb_ExtendTime)
        Me.Tab_Shutdown.Controls.Add(Me.Tb_Delay_Stop)
        Me.Tab_Shutdown.Controls.Add(Me.Cbx_TypeStop)
        Me.Tab_Shutdown.Controls.Add(Me.Cb_ImmediateStop)
        Me.Tab_Shutdown.Controls.Add(Me.Tb_BattLimit_Time)
        Me.Tab_Shutdown.Controls.Add(Me.Tb_BattLimit_Load)
        Me.Tab_Shutdown.Controls.Add(Me.Lbl_GraceTime)
        Me.Tab_Shutdown.Controls.Add(Me.Lbl_Delay_Stop)
        Me.Tab_Shutdown.Controls.Add(Me.Lbl_StopType)
        Me.Tab_Shutdown.Controls.Add(Me.Lbl_BattLimit_Time)
        Me.Tab_Shutdown.Controls.Add(Me.Lbl_BattLimit_Load)
        Me.Tab_Shutdown.Name = "Tab_Shutdown"
        Me.Pref_TlTip.SetToolTip(Me.Tab_Shutdown, resources.GetString("Tab_Shutdown.ToolTip"))
        Me.Tab_Shutdown.UseVisualStyleBackColor = True
        '
        'CB_Follow_FSD
        '
        resources.ApplyResources(Me.CB_Follow_FSD, "CB_Follow_FSD")
        Me.CB_Follow_FSD.Name = "CB_Follow_FSD"
        Me.Pref_TlTip.SetToolTip(Me.CB_Follow_FSD, resources.GetString("CB_Follow_FSD.ToolTip"))
        Me.CB_Follow_FSD.UseVisualStyleBackColor = True
        '
        'Lbl_Percent
        '
        resources.ApplyResources(Me.Lbl_Percent, "Lbl_Percent")
        Me.Lbl_Percent.Name = "Lbl_Percent"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Percent, resources.GetString("Lbl_Percent.ToolTip"))
        '
        'Tb_GraceTime
        '
        resources.ApplyResources(Me.Tb_GraceTime, "Tb_GraceTime")
        Me.Tb_GraceTime.Name = "Tb_GraceTime"
        Me.Pref_TlTip.SetToolTip(Me.Tb_GraceTime, resources.GetString("Tb_GraceTime.ToolTip"))
        '
        'Cb_ExtendTime
        '
        resources.ApplyResources(Me.Cb_ExtendTime, "Cb_ExtendTime")
        Me.Cb_ExtendTime.Name = "Cb_ExtendTime"
        Me.Pref_TlTip.SetToolTip(Me.Cb_ExtendTime, resources.GetString("Cb_ExtendTime.ToolTip"))
        Me.Cb_ExtendTime.UseVisualStyleBackColor = True
        '
        'Tb_Delay_Stop
        '
        resources.ApplyResources(Me.Tb_Delay_Stop, "Tb_Delay_Stop")
        Me.Tb_Delay_Stop.Name = "Tb_Delay_Stop"
        Me.Pref_TlTip.SetToolTip(Me.Tb_Delay_Stop, resources.GetString("Tb_Delay_Stop.ToolTip"))
        '
        'Cbx_TypeStop
        '
        resources.ApplyResources(Me.Cbx_TypeStop, "Cbx_TypeStop")
        Me.Cbx_TypeStop.FormattingEnabled = True
        Me.Cbx_TypeStop.Items.AddRange(New Object() {resources.GetString("Cbx_TypeStop.Items"), resources.GetString("Cbx_TypeStop.Items1"), resources.GetString("Cbx_TypeStop.Items2")})
        Me.Cbx_TypeStop.Name = "Cbx_TypeStop"
        Me.Pref_TlTip.SetToolTip(Me.Cbx_TypeStop, resources.GetString("Cbx_TypeStop.ToolTip"))
        '
        'Cb_ImmediateStop
        '
        resources.ApplyResources(Me.Cb_ImmediateStop, "Cb_ImmediateStop")
        Me.Cb_ImmediateStop.Name = "Cb_ImmediateStop"
        Me.Pref_TlTip.SetToolTip(Me.Cb_ImmediateStop, resources.GetString("Cb_ImmediateStop.ToolTip"))
        Me.Cb_ImmediateStop.UseVisualStyleBackColor = True
        '
        'Tb_BattLimit_Time
        '
        resources.ApplyResources(Me.Tb_BattLimit_Time, "Tb_BattLimit_Time")
        Me.Tb_BattLimit_Time.Name = "Tb_BattLimit_Time"
        Me.Pref_TlTip.SetToolTip(Me.Tb_BattLimit_Time, resources.GetString("Tb_BattLimit_Time.ToolTip"))
        '
        'Tb_BattLimit_Load
        '
        resources.ApplyResources(Me.Tb_BattLimit_Load, "Tb_BattLimit_Load")
        Me.Tb_BattLimit_Load.Name = "Tb_BattLimit_Load"
        Me.Pref_TlTip.SetToolTip(Me.Tb_BattLimit_Load, resources.GetString("Tb_BattLimit_Load.ToolTip"))
        '
        'Lbl_GraceTime
        '
        resources.ApplyResources(Me.Lbl_GraceTime, "Lbl_GraceTime")
        Me.Lbl_GraceTime.Name = "Lbl_GraceTime"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_GraceTime, resources.GetString("Lbl_GraceTime.ToolTip"))
        '
        'Lbl_Delay_Stop
        '
        resources.ApplyResources(Me.Lbl_Delay_Stop, "Lbl_Delay_Stop")
        Me.Lbl_Delay_Stop.Name = "Lbl_Delay_Stop"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Delay_Stop, resources.GetString("Lbl_Delay_Stop.ToolTip"))
        '
        'Lbl_StopType
        '
        resources.ApplyResources(Me.Lbl_StopType, "Lbl_StopType")
        Me.Lbl_StopType.Name = "Lbl_StopType"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_StopType, resources.GetString("Lbl_StopType.ToolTip"))
        '
        'Lbl_BattLimit_Time
        '
        resources.ApplyResources(Me.Lbl_BattLimit_Time, "Lbl_BattLimit_Time")
        Me.Lbl_BattLimit_Time.Name = "Lbl_BattLimit_Time"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_BattLimit_Time, resources.GetString("Lbl_BattLimit_Time.ToolTip"))
        '
        'Lbl_BattLimit_Load
        '
        resources.ApplyResources(Me.Lbl_BattLimit_Load, "Lbl_BattLimit_Load")
        Me.Lbl_BattLimit_Load.Name = "Lbl_BattLimit_Load"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_BattLimit_Load, resources.GetString("Lbl_BattLimit_Load.ToolTip"))
        '
        'Tab_Update
        '
        resources.ApplyResources(Me.Tab_Update, "Tab_Update")
        Me.Tab_Update.Controls.Add(Me.Cbx_Branch_Update)
        Me.Tab_Update.Controls.Add(Me.Cbx_Delay_Verif)
        Me.Tab_Update.Controls.Add(Me.Lbl_Branch_Update)
        Me.Tab_Update.Controls.Add(Me.Lbl_Delay_Verif)
        Me.Tab_Update.Controls.Add(Me.Cb_Update_At_Start)
        Me.Tab_Update.Controls.Add(Me.Cb_Verify_Update)
        Me.Tab_Update.Name = "Tab_Update"
        Me.Pref_TlTip.SetToolTip(Me.Tab_Update, resources.GetString("Tab_Update.ToolTip"))
        Me.Tab_Update.UseVisualStyleBackColor = True
        '
        'Cbx_Branch_Update
        '
        resources.ApplyResources(Me.Cbx_Branch_Update, "Cbx_Branch_Update")
        Me.Cbx_Branch_Update.FormattingEnabled = True
        Me.Cbx_Branch_Update.Items.AddRange(New Object() {resources.GetString("Cbx_Branch_Update.Items"), resources.GetString("Cbx_Branch_Update.Items1")})
        Me.Cbx_Branch_Update.Name = "Cbx_Branch_Update"
        Me.Pref_TlTip.SetToolTip(Me.Cbx_Branch_Update, resources.GetString("Cbx_Branch_Update.ToolTip"))
        '
        'Cbx_Delay_Verif
        '
        resources.ApplyResources(Me.Cbx_Delay_Verif, "Cbx_Delay_Verif")
        Me.Cbx_Delay_Verif.FormattingEnabled = True
        Me.Cbx_Delay_Verif.Items.AddRange(New Object() {resources.GetString("Cbx_Delay_Verif.Items"), resources.GetString("Cbx_Delay_Verif.Items1"), resources.GetString("Cbx_Delay_Verif.Items2")})
        Me.Cbx_Delay_Verif.Name = "Cbx_Delay_Verif"
        Me.Pref_TlTip.SetToolTip(Me.Cbx_Delay_Verif, resources.GetString("Cbx_Delay_Verif.ToolTip"))
        '
        'Lbl_Branch_Update
        '
        resources.ApplyResources(Me.Lbl_Branch_Update, "Lbl_Branch_Update")
        Me.Lbl_Branch_Update.Name = "Lbl_Branch_Update"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Branch_Update, resources.GetString("Lbl_Branch_Update.ToolTip"))
        '
        'Lbl_Delay_Verif
        '
        resources.ApplyResources(Me.Lbl_Delay_Verif, "Lbl_Delay_Verif")
        Me.Lbl_Delay_Verif.Name = "Lbl_Delay_Verif"
        Me.Pref_TlTip.SetToolTip(Me.Lbl_Delay_Verif, resources.GetString("Lbl_Delay_Verif.ToolTip"))
        '
        'Cb_Update_At_Start
        '
        resources.ApplyResources(Me.Cb_Update_At_Start, "Cb_Update_At_Start")
        Me.Cb_Update_At_Start.Name = "Cb_Update_At_Start"
        Me.Pref_TlTip.SetToolTip(Me.Cb_Update_At_Start, resources.GetString("Cb_Update_At_Start.ToolTip"))
        Me.Cb_Update_At_Start.UseVisualStyleBackColor = True
        '
        'Cb_Verify_Update
        '
        resources.ApplyResources(Me.Cb_Verify_Update, "Cb_Verify_Update")
        Me.Cb_Verify_Update.Name = "Cb_Verify_Update"
        Me.Pref_TlTip.SetToolTip(Me.Cb_Verify_Update, resources.GetString("Cb_Verify_Update.ToolTip"))
        Me.Cb_Verify_Update.UseVisualStyleBackColor = True
        '
        'Btn_Ok
        '
        resources.ApplyResources(Me.Btn_Ok, "Btn_Ok")
        Me.Btn_Ok.Name = "Btn_Ok"
        Me.Pref_TlTip.SetToolTip(Me.Btn_Ok, resources.GetString("Btn_Ok.ToolTip"))
        Me.Btn_Ok.UseVisualStyleBackColor = True
        '
        'Btn_Apply
        '
        resources.ApplyResources(Me.Btn_Apply, "Btn_Apply")
        Me.Btn_Apply.Name = "Btn_Apply"
        Me.Pref_TlTip.SetToolTip(Me.Btn_Apply, resources.GetString("Btn_Apply.ToolTip"))
        Me.Btn_Apply.UseVisualStyleBackColor = True
        '
        'Btn_Cancel
        '
        resources.ApplyResources(Me.Btn_Cancel, "Btn_Cancel")
        Me.Btn_Cancel.CausesValidation = False
        Me.Btn_Cancel.Name = "Btn_Cancel"
        Me.Pref_TlTip.SetToolTip(Me.Btn_Cancel, resources.GetString("Btn_Cancel.ToolTip"))
        Me.Btn_Cancel.UseVisualStyleBackColor = True
        Me.Btn_Cancel.UseWaitCursor = True
        '
        'Pref_Gui
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Btn_Cancel)
        Me.Controls.Add(Me.Btn_Apply)
        Me.Controls.Add(Me.Btn_Ok)
        Me.Controls.Add(Me.TabControl_Options)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Pref_Gui"
        Me.Pref_TlTip.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.TabControl_Options.ResumeLayout(False)
        Me.Tab_Connexion.ResumeLayout(False)
        Me.Tab_Connexion.PerformLayout()
        Me.Tab_Calibrage.ResumeLayout(False)
        Me.Tab_Calibrage.PerformLayout()
        Me.Tab_Miscellanous.ResumeLayout(False)
        Me.Tab_Miscellanous.PerformLayout()
        Me.Tab_Shutdown.ResumeLayout(False)
        Me.Tab_Shutdown.PerformLayout()
        Me.Tab_Update.ResumeLayout(False)
        Me.Tab_Update.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl_Options As TabControl
    Friend WithEvents Tab_Connexion As TabPage
    Friend WithEvents Cb_Reconnect As CheckBox
    Friend WithEvents Lbl_Delay_Com As Label
    Friend WithEvents Lbl_Name_UPS As Label
    Friend WithEvents Lbl_Port As Label
    Friend WithEvents Lbl_Server_IP As Label
    Friend WithEvents Tab_Calibrage As TabPage
    Friend WithEvents Tab_Miscellanous As TabPage
    Friend WithEvents Tab_Shutdown As TabPage
    Friend WithEvents Tab_Update As TabPage
    Friend WithEvents Btn_Ok As Button
    Friend WithEvents Btn_Apply As Button
    Friend WithEvents Btn_Cancel As Button
    Friend WithEvents Tb_Server_IP As TextBox
    Friend WithEvents Tb_Port As TextBox
    Friend WithEvents Tb_Delay_Com As TextBox
    Friend WithEvents Tb_UPS_Name As TextBox
    Friend WithEvents Lbl_InputV As Label
    Friend WithEvents Lbl_BattV As Label
    Friend WithEvents Lbl_LoadUPS As Label
    Friend WithEvents Lbl_OutputV As Label
    Friend WithEvents Lbl_InputF As Label
    Friend WithEvents Lbl_PowerF As Label
    Friend WithEvents Tb_InV_Min As TextBox
    Friend WithEvents Lbl_Maxi As Label
    Friend WithEvents Lbl_Mini As Label
    Friend WithEvents Cbx_Freq_Input As ComboBox
    Friend WithEvents Tb_BattV_Max As TextBox
    Friend WithEvents Tb_Load_Max As TextBox
    Friend WithEvents Tb_OutV_Max As TextBox
    Friend WithEvents Tb_InF_Max As TextBox
    Friend WithEvents Tb_BattV_Min As TextBox
    Friend WithEvents Tb_Load_Min As TextBox
    Friend WithEvents Tb_OutV_Min As TextBox
    Friend WithEvents Tb_InF_Min As TextBox
    Friend WithEvents Tb_InV_Max As TextBox
    Friend WithEvents Btn_DeleteLog As Button
    Friend WithEvents Btn_ViewLog As Button
    Friend WithEvents Lbl_LevelLog As Label
    Friend WithEvents CB_Use_Logfile As CheckBox
    Friend WithEvents CB_Start_W_Win As CheckBox
    Friend WithEvents CB_Close_Tray As CheckBox
    Friend WithEvents CB_Start_Mini As CheckBox
    Friend WithEvents CB_Systray As CheckBox
    Friend WithEvents Cbx_LogLevel As ComboBox
    Friend WithEvents Lbl_Delay_Stop As Label
    Friend WithEvents Lbl_StopType As Label
    Friend WithEvents Lbl_BattLimit_Time As Label
    Friend WithEvents Lbl_BattLimit_Load As Label
    Friend WithEvents Lbl_Percent As Label
    Friend WithEvents Tb_GraceTime As TextBox
    Friend WithEvents Cb_ExtendTime As CheckBox
    Friend WithEvents Tb_Delay_Stop As TextBox
    Friend WithEvents Cbx_TypeStop As ComboBox
    Friend WithEvents Cb_ImmediateStop As CheckBox
    Friend WithEvents Tb_BattLimit_Time As TextBox
    Friend WithEvents Tb_BattLimit_Load As TextBox
    Friend WithEvents Lbl_GraceTime As Label
    Friend WithEvents Pref_TlTip As ToolTip
    Friend WithEvents Cbx_Branch_Update As ComboBox
    Friend WithEvents Cbx_Delay_Verif As ComboBox
    Friend WithEvents Lbl_Branch_Update As Label
    Friend WithEvents Lbl_Delay_Verif As Label
    Friend WithEvents Cb_Update_At_Start As CheckBox
    Friend WithEvents Cb_Verify_Update As CheckBox
    Friend WithEvents Tb_Pwd_Nut As TextBox
    Friend WithEvents Tb_Login_Nut As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Lbl_Pwd_Nut As Label
    Friend WithEvents Lbl_Login_Nut As Label
    Friend WithEvents CB_Follow_FSD As CheckBox
End Class
