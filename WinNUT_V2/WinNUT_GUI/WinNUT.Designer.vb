<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WinNUT
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WinNUT))
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenu_Systray = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Menu_Sys_Settings = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Sys_Sep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.Menu_Sys_Update = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Sys_Sep2 = New System.Windows.Forms.ToolStripSeparator()
        Me.Menu_Sys_About = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Sys_Sep3 = New System.Windows.Forms.ToolStripSeparator()
        Me.Menu_Sys_Exit = New System.Windows.Forms.ToolStripMenuItem()
        Me.Main_Menu = New System.Windows.Forms.MenuStrip()
        Me.Menu_File = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Import_Ini = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_UPS_Var = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Quit = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Connection = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Reconnect = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Disconnect = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Settings = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Help = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_About = New System.Windows.Forms.ToolStripMenuItem()
        Me.Menu_Help_Sep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.Menu_Update = New System.Windows.Forms.ToolStripMenuItem()
        Me.GB_Status = New System.Windows.Forms.GroupBox()
        Me.Lbl_VSerial = New System.Windows.Forms.Label()
        Me.Lbl_VFirmware = New System.Windows.Forms.Label()
        Me.Lbl_VName = New System.Windows.Forms.Label()
        Me.Lbl_VMfr = New System.Windows.Forms.Label()
        Me.Lbl_VRTime = New System.Windows.Forms.Label()
        Me.Lbl_VOB = New System.Windows.Forms.Label()
        Me.Lbl_VOLoad = New System.Windows.Forms.Label()
        Me.Lbl_VBL = New System.Windows.Forms.Label()
        Me.Lbl_VOL = New System.Windows.Forms.Label()
        Me.Lbl_Firmware = New System.Windows.Forms.Label()
        Me.Lbl_Serial = New System.Windows.Forms.Label()
        Me.Lbl_Name = New System.Windows.Forms.Label()
        Me.Lbl_Mfr = New System.Windows.Forms.Label()
        Me.Lbl_RTime = New System.Windows.Forms.Label()
        Me.Lbl_BL = New System.Windows.Forms.Label()
        Me.Lbl_OLoad = New System.Windows.Forms.Label()
        Me.Lbl_OB = New System.Windows.Forms.Label()
        Me.Lbl_OL = New System.Windows.Forms.Label()
        Me.GB_InV_Dial = New System.Windows.Forms.GroupBox()
        Me.Lbl_InV_Dial = New System.Windows.Forms.Label()
        Me.GB_OutV_Dial = New System.Windows.Forms.GroupBox()
        Me.Lbl_OutV_Dial = New System.Windows.Forms.Label()
        Me.GB_BattCh_Dial = New System.Windows.Forms.GroupBox()
        Me.PBox_Battery_State = New System.Windows.Forms.PictureBox()
        Me.Lbl_BattCh_Dial = New System.Windows.Forms.Label()
        Me.GB_Load_Dial = New System.Windows.Forms.GroupBox()
        Me.Lbl_Load_Dial = New System.Windows.Forms.Label()
        Me.GB_BattV_Dial = New System.Windows.Forms.GroupBox()
        Me.Lbl_BattV_Dial = New System.Windows.Forms.Label()
        Me.GB_InF_Dial = New System.Windows.Forms.GroupBox()
        Me.Lbl_InF_Dial = New System.Windows.Forms.Label()
        Me.CB_CurrentLog = New System.Windows.Forms.ComboBox()
        Me.AG_InF = New System.Windows.Forms.AGauge()
        Me.AG_InV = New System.Windows.Forms.AGauge()
        Me.AG_BattV = New System.Windows.Forms.AGauge()
        Me.AG_Load = New System.Windows.Forms.AGauge()
        Me.AG_OutV = New System.Windows.Forms.AGauge()
        Me.AG_BattCh = New System.Windows.Forms.AGauge()
        Me.ContextMenu_Systray.SuspendLayout()
        Me.Main_Menu.SuspendLayout()
        Me.GB_Status.SuspendLayout()
        Me.GB_InV_Dial.SuspendLayout()
        Me.GB_OutV_Dial.SuspendLayout()
        Me.GB_BattCh_Dial.SuspendLayout()
        CType(Me.PBox_Battery_State, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GB_Load_Dial.SuspendLayout()
        Me.GB_BattV_Dial.SuspendLayout()
        Me.GB_InF_Dial.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotifyIcon
        '
        Me.NotifyIcon.ContextMenuStrip = Me.ContextMenu_Systray
        resources.ApplyResources(Me.NotifyIcon, "NotifyIcon")
        '
        'ContextMenu_Systray
        '
        Me.ContextMenu_Systray.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Menu_Sys_Settings, Me.Menu_Sys_Sep1, Me.Menu_Sys_Update, Me.Menu_Sys_Sep2, Me.Menu_Sys_About, Me.Menu_Sys_Sep3, Me.Menu_Sys_Exit})
        Me.ContextMenu_Systray.Name = "ContextMenuStrip1"
        resources.ApplyResources(Me.ContextMenu_Systray, "ContextMenu_Systray")
        '
        'Menu_Sys_Settings
        '
        Me.Menu_Sys_Settings.Name = "Menu_Sys_Settings"
        resources.ApplyResources(Me.Menu_Sys_Settings, "Menu_Sys_Settings")
        '
        'Menu_Sys_Sep1
        '
        Me.Menu_Sys_Sep1.Name = "Menu_Sys_Sep1"
        resources.ApplyResources(Me.Menu_Sys_Sep1, "Menu_Sys_Sep1")
        '
        'Menu_Sys_Update
        '
        Me.Menu_Sys_Update.Name = "Menu_Sys_Update"
        resources.ApplyResources(Me.Menu_Sys_Update, "Menu_Sys_Update")
        '
        'Menu_Sys_Sep2
        '
        Me.Menu_Sys_Sep2.Name = "Menu_Sys_Sep2"
        resources.ApplyResources(Me.Menu_Sys_Sep2, "Menu_Sys_Sep2")
        '
        'Menu_Sys_About
        '
        Me.Menu_Sys_About.Name = "Menu_Sys_About"
        resources.ApplyResources(Me.Menu_Sys_About, "Menu_Sys_About")
        '
        'Menu_Sys_Sep3
        '
        Me.Menu_Sys_Sep3.Name = "Menu_Sys_Sep3"
        resources.ApplyResources(Me.Menu_Sys_Sep3, "Menu_Sys_Sep3")
        '
        'Menu_Sys_Exit
        '
        Me.Menu_Sys_Exit.Name = "Menu_Sys_Exit"
        resources.ApplyResources(Me.Menu_Sys_Exit, "Menu_Sys_Exit")
        '
        'Main_Menu
        '
        resources.ApplyResources(Me.Main_Menu, "Main_Menu")
        Me.Main_Menu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Menu_File, Me.Menu_Connection, Me.Menu_Settings, Me.Menu_Help})
        Me.Main_Menu.Name = "Main_Menu"
        '
        'Menu_File
        '
        Me.Menu_File.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Menu_Import_Ini, Me.Menu_UPS_Var, Me.Menu_Quit})
        Me.Menu_File.Name = "Menu_File"
        resources.ApplyResources(Me.Menu_File, "Menu_File")
        '
        'Menu_Import_Ini
        '
        Me.Menu_Import_Ini.Name = "Menu_Import_Ini"
        resources.ApplyResources(Me.Menu_Import_Ini, "Menu_Import_Ini")
        '
        'Menu_UPS_Var
        '
        Me.Menu_UPS_Var.Name = "Menu_UPS_Var"
        resources.ApplyResources(Me.Menu_UPS_Var, "Menu_UPS_Var")
        '
        'Menu_Quit
        '
        Me.Menu_Quit.Name = "Menu_Quit"
        resources.ApplyResources(Me.Menu_Quit, "Menu_Quit")
        '
        'Menu_Connection
        '
        Me.Menu_Connection.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Menu_Reconnect, Me.Menu_Disconnect})
        Me.Menu_Connection.Name = "Menu_Connection"
        resources.ApplyResources(Me.Menu_Connection, "Menu_Connection")
        '
        'Menu_Reconnect
        '
        Me.Menu_Reconnect.Name = "Menu_Reconnect"
        resources.ApplyResources(Me.Menu_Reconnect, "Menu_Reconnect")
        '
        'Menu_Disconnect
        '
        Me.Menu_Disconnect.Name = "Menu_Disconnect"
        resources.ApplyResources(Me.Menu_Disconnect, "Menu_Disconnect")
        '
        'Menu_Settings
        '
        Me.Menu_Settings.Name = "Menu_Settings"
        resources.ApplyResources(Me.Menu_Settings, "Menu_Settings")
        '
        'Menu_Help
        '
        Me.Menu_Help.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Menu_About, Me.Menu_Help_Sep1, Me.Menu_Update})
        Me.Menu_Help.Name = "Menu_Help"
        resources.ApplyResources(Me.Menu_Help, "Menu_Help")
        '
        'Menu_About
        '
        Me.Menu_About.Name = "Menu_About"
        resources.ApplyResources(Me.Menu_About, "Menu_About")
        '
        'Menu_Help_Sep1
        '
        Me.Menu_Help_Sep1.Name = "Menu_Help_Sep1"
        resources.ApplyResources(Me.Menu_Help_Sep1, "Menu_Help_Sep1")
        '
        'Menu_Update
        '
        Me.Menu_Update.Name = "Menu_Update"
        resources.ApplyResources(Me.Menu_Update, "Menu_Update")
        '
        'GB_Status
        '
        resources.ApplyResources(Me.GB_Status, "GB_Status")
        Me.GB_Status.Controls.Add(Me.Lbl_VSerial)
        Me.GB_Status.Controls.Add(Me.Lbl_VFirmware)
        Me.GB_Status.Controls.Add(Me.Lbl_VName)
        Me.GB_Status.Controls.Add(Me.Lbl_VMfr)
        Me.GB_Status.Controls.Add(Me.Lbl_VRTime)
        Me.GB_Status.Controls.Add(Me.Lbl_VOB)
        Me.GB_Status.Controls.Add(Me.Lbl_VOLoad)
        Me.GB_Status.Controls.Add(Me.Lbl_VBL)
        Me.GB_Status.Controls.Add(Me.Lbl_VOL)
        Me.GB_Status.Controls.Add(Me.Lbl_Firmware)
        Me.GB_Status.Controls.Add(Me.Lbl_Serial)
        Me.GB_Status.Controls.Add(Me.Lbl_Name)
        Me.GB_Status.Controls.Add(Me.Lbl_Mfr)
        Me.GB_Status.Controls.Add(Me.Lbl_RTime)
        Me.GB_Status.Controls.Add(Me.Lbl_BL)
        Me.GB_Status.Controls.Add(Me.Lbl_OLoad)
        Me.GB_Status.Controls.Add(Me.Lbl_OB)
        Me.GB_Status.Controls.Add(Me.Lbl_OL)
        Me.GB_Status.Name = "GB_Status"
        Me.GB_Status.TabStop = False
        '
        'Lbl_VSerial
        '
        Me.Lbl_VSerial.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VSerial, "Lbl_VSerial")
        Me.Lbl_VSerial.Name = "Lbl_VSerial"
        '
        'Lbl_VFirmware
        '
        Me.Lbl_VFirmware.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VFirmware, "Lbl_VFirmware")
        Me.Lbl_VFirmware.Name = "Lbl_VFirmware"
        Me.Lbl_VFirmware.UseCompatibleTextRendering = True
        '
        'Lbl_VName
        '
        Me.Lbl_VName.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VName, "Lbl_VName")
        Me.Lbl_VName.Name = "Lbl_VName"
        '
        'Lbl_VMfr
        '
        Me.Lbl_VMfr.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VMfr, "Lbl_VMfr")
        Me.Lbl_VMfr.Name = "Lbl_VMfr"
        '
        'Lbl_VRTime
        '
        Me.Lbl_VRTime.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VRTime, "Lbl_VRTime")
        Me.Lbl_VRTime.Name = "Lbl_VRTime"
        '
        'Lbl_VOB
        '
        Me.Lbl_VOB.BackColor = System.Drawing.Color.White
        Me.Lbl_VOB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_VOB.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VOB, "Lbl_VOB")
        Me.Lbl_VOB.Name = "Lbl_VOB"
        '
        'Lbl_VOLoad
        '
        Me.Lbl_VOLoad.BackColor = System.Drawing.Color.White
        Me.Lbl_VOLoad.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_VOLoad.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VOLoad, "Lbl_VOLoad")
        Me.Lbl_VOLoad.Name = "Lbl_VOLoad"
        '
        'Lbl_VBL
        '
        Me.Lbl_VBL.BackColor = System.Drawing.Color.White
        Me.Lbl_VBL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_VBL.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VBL, "Lbl_VBL")
        Me.Lbl_VBL.Name = "Lbl_VBL"
        '
        'Lbl_VOL
        '
        Me.Lbl_VOL.BackColor = System.Drawing.Color.White
        Me.Lbl_VOL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Lbl_VOL.CausesValidation = False
        resources.ApplyResources(Me.Lbl_VOL, "Lbl_VOL")
        Me.Lbl_VOL.Name = "Lbl_VOL"
        '
        'Lbl_Firmware
        '
        Me.Lbl_Firmware.CausesValidation = False
        resources.ApplyResources(Me.Lbl_Firmware, "Lbl_Firmware")
        Me.Lbl_Firmware.Name = "Lbl_Firmware"
        Me.Lbl_Firmware.UseCompatibleTextRendering = True
        '
        'Lbl_Serial
        '
        Me.Lbl_Serial.CausesValidation = False
        resources.ApplyResources(Me.Lbl_Serial, "Lbl_Serial")
        Me.Lbl_Serial.Name = "Lbl_Serial"
        '
        'Lbl_Name
        '
        Me.Lbl_Name.CausesValidation = False
        resources.ApplyResources(Me.Lbl_Name, "Lbl_Name")
        Me.Lbl_Name.Name = "Lbl_Name"
        '
        'Lbl_Mfr
        '
        Me.Lbl_Mfr.CausesValidation = False
        resources.ApplyResources(Me.Lbl_Mfr, "Lbl_Mfr")
        Me.Lbl_Mfr.Name = "Lbl_Mfr"
        '
        'Lbl_RTime
        '
        Me.Lbl_RTime.CausesValidation = False
        resources.ApplyResources(Me.Lbl_RTime, "Lbl_RTime")
        Me.Lbl_RTime.Name = "Lbl_RTime"
        '
        'Lbl_BL
        '
        Me.Lbl_BL.CausesValidation = False
        resources.ApplyResources(Me.Lbl_BL, "Lbl_BL")
        Me.Lbl_BL.Name = "Lbl_BL"
        '
        'Lbl_OLoad
        '
        Me.Lbl_OLoad.CausesValidation = False
        resources.ApplyResources(Me.Lbl_OLoad, "Lbl_OLoad")
        Me.Lbl_OLoad.Name = "Lbl_OLoad"
        '
        'Lbl_OB
        '
        Me.Lbl_OB.CausesValidation = False
        resources.ApplyResources(Me.Lbl_OB, "Lbl_OB")
        Me.Lbl_OB.Name = "Lbl_OB"
        '
        'Lbl_OL
        '
        Me.Lbl_OL.CausesValidation = False
        resources.ApplyResources(Me.Lbl_OL, "Lbl_OL")
        Me.Lbl_OL.Name = "Lbl_OL"
        '
        'GB_InV_Dial
        '
        resources.ApplyResources(Me.GB_InV_Dial, "GB_InV_Dial")
        Me.GB_InV_Dial.Controls.Add(Me.AG_InV)
        Me.GB_InV_Dial.Controls.Add(Me.Lbl_InV_Dial)
        Me.GB_InV_Dial.Name = "GB_InV_Dial"
        Me.GB_InV_Dial.TabStop = False
        '
        'Lbl_InV_Dial
        '
        resources.ApplyResources(Me.Lbl_InV_Dial, "Lbl_InV_Dial")
        Me.Lbl_InV_Dial.Name = "Lbl_InV_Dial"
        '
        'GB_OutV_Dial
        '
        resources.ApplyResources(Me.GB_OutV_Dial, "GB_OutV_Dial")
        Me.GB_OutV_Dial.Controls.Add(Me.AG_OutV)
        Me.GB_OutV_Dial.Controls.Add(Me.Lbl_OutV_Dial)
        Me.GB_OutV_Dial.Name = "GB_OutV_Dial"
        Me.GB_OutV_Dial.TabStop = False
        '
        'Lbl_OutV_Dial
        '
        resources.ApplyResources(Me.Lbl_OutV_Dial, "Lbl_OutV_Dial")
        Me.Lbl_OutV_Dial.Name = "Lbl_OutV_Dial"
        '
        'GB_BattCh_Dial
        '
        resources.ApplyResources(Me.GB_BattCh_Dial, "GB_BattCh_Dial")
        Me.GB_BattCh_Dial.Controls.Add(Me.PBox_Battery_State)
        Me.GB_BattCh_Dial.Controls.Add(Me.Lbl_BattCh_Dial)
        Me.GB_BattCh_Dial.Controls.Add(Me.AG_BattCh)
        Me.GB_BattCh_Dial.Name = "GB_BattCh_Dial"
        Me.GB_BattCh_Dial.TabStop = False
        '
        'PBox_Battery_State
        '
        resources.ApplyResources(Me.PBox_Battery_State, "PBox_Battery_State")
        Me.PBox_Battery_State.Name = "PBox_Battery_State"
        Me.PBox_Battery_State.TabStop = False
        '
        'Lbl_BattCh_Dial
        '
        resources.ApplyResources(Me.Lbl_BattCh_Dial, "Lbl_BattCh_Dial")
        Me.Lbl_BattCh_Dial.Name = "Lbl_BattCh_Dial"
        '
        'GB_Load_Dial
        '
        resources.ApplyResources(Me.GB_Load_Dial, "GB_Load_Dial")
        Me.GB_Load_Dial.Controls.Add(Me.AG_Load)
        Me.GB_Load_Dial.Controls.Add(Me.Lbl_Load_Dial)
        Me.GB_Load_Dial.Name = "GB_Load_Dial"
        Me.GB_Load_Dial.TabStop = False
        '
        'Lbl_Load_Dial
        '
        resources.ApplyResources(Me.Lbl_Load_Dial, "Lbl_Load_Dial")
        Me.Lbl_Load_Dial.Name = "Lbl_Load_Dial"
        '
        'GB_BattV_Dial
        '
        resources.ApplyResources(Me.GB_BattV_Dial, "GB_BattV_Dial")
        Me.GB_BattV_Dial.Controls.Add(Me.AG_BattV)
        Me.GB_BattV_Dial.Controls.Add(Me.Lbl_BattV_Dial)
        Me.GB_BattV_Dial.Name = "GB_BattV_Dial"
        Me.GB_BattV_Dial.TabStop = False
        '
        'Lbl_BattV_Dial
        '
        resources.ApplyResources(Me.Lbl_BattV_Dial, "Lbl_BattV_Dial")
        Me.Lbl_BattV_Dial.Name = "Lbl_BattV_Dial"
        '
        'GB_InF_Dial
        '
        resources.ApplyResources(Me.GB_InF_Dial, "GB_InF_Dial")
        Me.GB_InF_Dial.Controls.Add(Me.AG_InF)
        Me.GB_InF_Dial.Controls.Add(Me.Lbl_InF_Dial)
        Me.GB_InF_Dial.Name = "GB_InF_Dial"
        Me.GB_InF_Dial.TabStop = False
        '
        'Lbl_InF_Dial
        '
        resources.ApplyResources(Me.Lbl_InF_Dial, "Lbl_InF_Dial")
        Me.Lbl_InF_Dial.Name = "Lbl_InF_Dial"
        '
        'CB_CurrentLog
        '
        Me.CB_CurrentLog.FormattingEnabled = True
        resources.ApplyResources(Me.CB_CurrentLog, "CB_CurrentLog")
        Me.CB_CurrentLog.Name = "CB_CurrentLog"
        '
        'AG_InF
        '
        Me.AG_InF.BaseArcColor = System.Drawing.Color.Gray
        Me.AG_InF.BaseArcRadius = 45
        Me.AG_InF.BaseArcStart = 135
        Me.AG_InF.BaseArcSweep = 270
        Me.AG_InF.BaseArcWidth = 5
        Me.AG_InF.Center = New System.Drawing.Point(74, 70)
        Me.AG_InF.GaugeAutoSize = False
        Me.AG_InF.GradientColor = System.Windows.Forms.AGauge.GradientType.RedGreen
        Me.AG_InF.GradientColorOrientation = System.Windows.Forms.AGauge.GradientOrientation.BottomToUp
        resources.ApplyResources(Me.AG_InF, "AG_InF")
        Me.AG_InF.MaxValue = 100.0!
        Me.AG_InF.MinValue = 0!
        Me.AG_InF.Name = "AG_InF"
        Me.AG_InF.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray
        Me.AG_InF.NeedleColor2 = System.Drawing.Color.DimGray
        Me.AG_InF.NeedleRadius = 32
        Me.AG_InF.NeedleType = System.Windows.Forms.NeedleType.Advance
        Me.AG_InF.NeedleWidth = 2
        Me.AG_InF.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.AG_InF.ScaleLinesInterInnerRadius = 40
        Me.AG_InF.ScaleLinesInterOuterRadius = 48
        Me.AG_InF.ScaleLinesInterWidth = 1
        Me.AG_InF.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.AG_InF.ScaleLinesMajorInnerRadius = 40
        Me.AG_InF.ScaleLinesMajorOuterRadius = 48
        Me.AG_InF.ScaleLinesMajorStepValue = 50.0!
        Me.AG_InF.ScaleLinesMajorWidth = 2
        Me.AG_InF.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.AG_InF.ScaleLinesMinorInnerRadius = 42
        Me.AG_InF.ScaleLinesMinorOuterRadius = 48
        Me.AG_InF.ScaleLinesMinorTicks = 9
        Me.AG_InF.ScaleLinesMinorWidth = 1
        Me.AG_InF.ScaleNumbersColor = System.Drawing.Color.Black
        Me.AG_InF.ScaleNumbersFormat = Nothing
        Me.AG_InF.ScaleNumbersRadius = 60
        Me.AG_InF.ScaleNumbersRotation = 0
        Me.AG_InF.ScaleNumbersStartScaleLine = 0
        Me.AG_InF.ScaleNumbersStepScaleLines = 1
        Me.AG_InF.UnitValue1 = System.Windows.Forms.AGauge.UnitValue.Hertz
        Me.AG_InF.UnitValue2 = System.Windows.Forms.AGauge.UnitValue.None
        Me.AG_InF.Value1 = 0!
        Me.AG_InF.Value2 = 0!
        '
        'AG_InV
        '
        Me.AG_InV.BaseArcColor = System.Drawing.Color.Gray
        Me.AG_InV.BaseArcRadius = 45
        Me.AG_InV.BaseArcStart = 135
        Me.AG_InV.BaseArcSweep = 270
        Me.AG_InV.BaseArcWidth = 5
        Me.AG_InV.Center = New System.Drawing.Point(74, 70)
        Me.AG_InV.GaugeAutoSize = False
        Me.AG_InV.GradientColor = System.Windows.Forms.AGauge.GradientType.RedGreen
        Me.AG_InV.GradientColorOrientation = System.Windows.Forms.AGauge.GradientOrientation.BottomToUp
        resources.ApplyResources(Me.AG_InV, "AG_InV")
        Me.AG_InV.MaxValue = 100.0!
        Me.AG_InV.MinValue = 0!
        Me.AG_InV.Name = "AG_InV"
        Me.AG_InV.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray
        Me.AG_InV.NeedleColor2 = System.Drawing.Color.DimGray
        Me.AG_InV.NeedleRadius = 32
        Me.AG_InV.NeedleType = System.Windows.Forms.NeedleType.Advance
        Me.AG_InV.NeedleWidth = 2
        Me.AG_InV.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.AG_InV.ScaleLinesInterInnerRadius = 40
        Me.AG_InV.ScaleLinesInterOuterRadius = 48
        Me.AG_InV.ScaleLinesInterWidth = 1
        Me.AG_InV.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.AG_InV.ScaleLinesMajorInnerRadius = 40
        Me.AG_InV.ScaleLinesMajorOuterRadius = 48
        Me.AG_InV.ScaleLinesMajorStepValue = 50.0!
        Me.AG_InV.ScaleLinesMajorWidth = 2
        Me.AG_InV.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.AG_InV.ScaleLinesMinorInnerRadius = 42
        Me.AG_InV.ScaleLinesMinorOuterRadius = 48
        Me.AG_InV.ScaleLinesMinorTicks = 9
        Me.AG_InV.ScaleLinesMinorWidth = 1
        Me.AG_InV.ScaleNumbersColor = System.Drawing.Color.Black
        Me.AG_InV.ScaleNumbersFormat = Nothing
        Me.AG_InV.ScaleNumbersRadius = 60
        Me.AG_InV.ScaleNumbersRotation = 0
        Me.AG_InV.ScaleNumbersStartScaleLine = 0
        Me.AG_InV.ScaleNumbersStepScaleLines = 1
        Me.AG_InV.UnitValue1 = System.Windows.Forms.AGauge.UnitValue.Volts
        Me.AG_InV.UnitValue2 = System.Windows.Forms.AGauge.UnitValue.None
        Me.AG_InV.Value1 = 0!
        Me.AG_InV.Value2 = 0!
        '
        'AG_BattV
        '
        Me.AG_BattV.BaseArcColor = System.Drawing.Color.Gray
        Me.AG_BattV.BaseArcRadius = 45
        Me.AG_BattV.BaseArcStart = 135
        Me.AG_BattV.BaseArcSweep = 270
        Me.AG_BattV.BaseArcWidth = 5
        Me.AG_BattV.Center = New System.Drawing.Point(74, 70)
        Me.AG_BattV.GaugeAutoSize = False
        Me.AG_BattV.GradientColor = System.Windows.Forms.AGauge.GradientType.RedGreen
        Me.AG_BattV.GradientColorOrientation = System.Windows.Forms.AGauge.GradientOrientation.BottomToUp
        resources.ApplyResources(Me.AG_BattV, "AG_BattV")
        Me.AG_BattV.MaxValue = 100.0!
        Me.AG_BattV.MinValue = 0!
        Me.AG_BattV.Name = "AG_BattV"
        Me.AG_BattV.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray
        Me.AG_BattV.NeedleColor2 = System.Drawing.Color.DimGray
        Me.AG_BattV.NeedleRadius = 32
        Me.AG_BattV.NeedleType = System.Windows.Forms.NeedleType.Advance
        Me.AG_BattV.NeedleWidth = 2
        Me.AG_BattV.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.AG_BattV.ScaleLinesInterInnerRadius = 40
        Me.AG_BattV.ScaleLinesInterOuterRadius = 48
        Me.AG_BattV.ScaleLinesInterWidth = 1
        Me.AG_BattV.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.AG_BattV.ScaleLinesMajorInnerRadius = 40
        Me.AG_BattV.ScaleLinesMajorOuterRadius = 48
        Me.AG_BattV.ScaleLinesMajorStepValue = 50.0!
        Me.AG_BattV.ScaleLinesMajorWidth = 2
        Me.AG_BattV.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.AG_BattV.ScaleLinesMinorInnerRadius = 42
        Me.AG_BattV.ScaleLinesMinorOuterRadius = 48
        Me.AG_BattV.ScaleLinesMinorTicks = 9
        Me.AG_BattV.ScaleLinesMinorWidth = 1
        Me.AG_BattV.ScaleNumbersColor = System.Drawing.Color.Black
        Me.AG_BattV.ScaleNumbersFormat = Nothing
        Me.AG_BattV.ScaleNumbersRadius = 60
        Me.AG_BattV.ScaleNumbersRotation = 0
        Me.AG_BattV.ScaleNumbersStartScaleLine = 0
        Me.AG_BattV.ScaleNumbersStepScaleLines = 1
        Me.AG_BattV.UnitValue1 = System.Windows.Forms.AGauge.UnitValue.Volts
        Me.AG_BattV.UnitValue2 = System.Windows.Forms.AGauge.UnitValue.None
        Me.AG_BattV.Value1 = 0!
        Me.AG_BattV.Value2 = 0!
        '
        'AG_Load
        '
        Me.AG_Load.BaseArcColor = System.Drawing.Color.Gray
        Me.AG_Load.BaseArcRadius = 45
        Me.AG_Load.BaseArcStart = 135
        Me.AG_Load.BaseArcSweep = 270
        Me.AG_Load.BaseArcWidth = 5
        Me.AG_Load.Center = New System.Drawing.Point(74, 70)
        Me.AG_Load.GaugeAutoSize = False
        Me.AG_Load.GradientColor = System.Windows.Forms.AGauge.GradientType.RedGreen
        Me.AG_Load.GradientColorOrientation = System.Windows.Forms.AGauge.GradientOrientation.RightToLeft
        resources.ApplyResources(Me.AG_Load, "AG_Load")
        Me.AG_Load.MaxValue = 100.0!
        Me.AG_Load.MinValue = 0!
        Me.AG_Load.Name = "AG_Load"
        Me.AG_Load.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray
        Me.AG_Load.NeedleColor2 = System.Drawing.Color.DimGray
        Me.AG_Load.NeedleRadius = 32
        Me.AG_Load.NeedleType = System.Windows.Forms.NeedleType.Advance
        Me.AG_Load.NeedleWidth = 2
        Me.AG_Load.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.AG_Load.ScaleLinesInterInnerRadius = 40
        Me.AG_Load.ScaleLinesInterOuterRadius = 48
        Me.AG_Load.ScaleLinesInterWidth = 1
        Me.AG_Load.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.AG_Load.ScaleLinesMajorInnerRadius = 40
        Me.AG_Load.ScaleLinesMajorOuterRadius = 48
        Me.AG_Load.ScaleLinesMajorStepValue = 50.0!
        Me.AG_Load.ScaleLinesMajorWidth = 2
        Me.AG_Load.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.AG_Load.ScaleLinesMinorInnerRadius = 42
        Me.AG_Load.ScaleLinesMinorOuterRadius = 48
        Me.AG_Load.ScaleLinesMinorTicks = 9
        Me.AG_Load.ScaleLinesMinorWidth = 1
        Me.AG_Load.ScaleNumbersColor = System.Drawing.Color.Black
        Me.AG_Load.ScaleNumbersFormat = Nothing
        Me.AG_Load.ScaleNumbersRadius = 60
        Me.AG_Load.ScaleNumbersRotation = 0
        Me.AG_Load.ScaleNumbersStartScaleLine = 0
        Me.AG_Load.ScaleNumbersStepScaleLines = 1
        Me.AG_Load.UnitValue1 = System.Windows.Forms.AGauge.UnitValue.Percent
        Me.AG_Load.UnitValue2 = System.Windows.Forms.AGauge.UnitValue.Watts
        Me.AG_Load.Value1 = 0!
        Me.AG_Load.Value2 = 0!
        '
        'AG_OutV
        '
        Me.AG_OutV.BaseArcColor = System.Drawing.Color.Gray
        Me.AG_OutV.BaseArcRadius = 45
        Me.AG_OutV.BaseArcStart = 135
        Me.AG_OutV.BaseArcSweep = 270
        Me.AG_OutV.BaseArcWidth = 5
        Me.AG_OutV.Center = New System.Drawing.Point(74, 70)
        Me.AG_OutV.GaugeAutoSize = False
        Me.AG_OutV.GradientColor = System.Windows.Forms.AGauge.GradientType.RedGreen
        Me.AG_OutV.GradientColorOrientation = System.Windows.Forms.AGauge.GradientOrientation.BottomToUp
        resources.ApplyResources(Me.AG_OutV, "AG_OutV")
        Me.AG_OutV.MaxValue = 100.0!
        Me.AG_OutV.MinValue = 0!
        Me.AG_OutV.Name = "AG_OutV"
        Me.AG_OutV.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray
        Me.AG_OutV.NeedleColor2 = System.Drawing.Color.DimGray
        Me.AG_OutV.NeedleRadius = 32
        Me.AG_OutV.NeedleType = System.Windows.Forms.NeedleType.Advance
        Me.AG_OutV.NeedleWidth = 2
        Me.AG_OutV.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.AG_OutV.ScaleLinesInterInnerRadius = 40
        Me.AG_OutV.ScaleLinesInterOuterRadius = 48
        Me.AG_OutV.ScaleLinesInterWidth = 1
        Me.AG_OutV.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.AG_OutV.ScaleLinesMajorInnerRadius = 40
        Me.AG_OutV.ScaleLinesMajorOuterRadius = 48
        Me.AG_OutV.ScaleLinesMajorStepValue = 50.0!
        Me.AG_OutV.ScaleLinesMajorWidth = 2
        Me.AG_OutV.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.AG_OutV.ScaleLinesMinorInnerRadius = 42
        Me.AG_OutV.ScaleLinesMinorOuterRadius = 48
        Me.AG_OutV.ScaleLinesMinorTicks = 9
        Me.AG_OutV.ScaleLinesMinorWidth = 1
        Me.AG_OutV.ScaleNumbersColor = System.Drawing.Color.Black
        Me.AG_OutV.ScaleNumbersFormat = Nothing
        Me.AG_OutV.ScaleNumbersRadius = 60
        Me.AG_OutV.ScaleNumbersRotation = 0
        Me.AG_OutV.ScaleNumbersStartScaleLine = 0
        Me.AG_OutV.ScaleNumbersStepScaleLines = 1
        Me.AG_OutV.UnitValue1 = System.Windows.Forms.AGauge.UnitValue.Volts
        Me.AG_OutV.UnitValue2 = System.Windows.Forms.AGauge.UnitValue.None
        Me.AG_OutV.Value1 = 0!
        Me.AG_OutV.Value2 = 0!
        '
        'AG_BattCh
        '
        Me.AG_BattCh.BaseArcColor = System.Drawing.Color.Gray
        Me.AG_BattCh.BaseArcRadius = 45
        Me.AG_BattCh.BaseArcStart = 135
        Me.AG_BattCh.BaseArcSweep = 270
        Me.AG_BattCh.BaseArcWidth = 5
        Me.AG_BattCh.Center = New System.Drawing.Point(74, 70)
        Me.AG_BattCh.GaugeAutoSize = False
        Me.AG_BattCh.GradientColor = System.Windows.Forms.AGauge.GradientType.RedGreen
        Me.AG_BattCh.GradientColorOrientation = System.Windows.Forms.AGauge.GradientOrientation.LeftToRight
        resources.ApplyResources(Me.AG_BattCh, "AG_BattCh")
        Me.AG_BattCh.MaxValue = 100.0!
        Me.AG_BattCh.MinValue = 0!
        Me.AG_BattCh.Name = "AG_BattCh"
        Me.AG_BattCh.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Gray
        Me.AG_BattCh.NeedleColor2 = System.Drawing.Color.DimGray
        Me.AG_BattCh.NeedleRadius = 32
        Me.AG_BattCh.NeedleType = System.Windows.Forms.NeedleType.Advance
        Me.AG_BattCh.NeedleWidth = 2
        Me.AG_BattCh.ScaleLinesInterColor = System.Drawing.Color.Black
        Me.AG_BattCh.ScaleLinesInterInnerRadius = 40
        Me.AG_BattCh.ScaleLinesInterOuterRadius = 48
        Me.AG_BattCh.ScaleLinesInterWidth = 1
        Me.AG_BattCh.ScaleLinesMajorColor = System.Drawing.Color.Black
        Me.AG_BattCh.ScaleLinesMajorInnerRadius = 40
        Me.AG_BattCh.ScaleLinesMajorOuterRadius = 48
        Me.AG_BattCh.ScaleLinesMajorStepValue = 50.0!
        Me.AG_BattCh.ScaleLinesMajorWidth = 2
        Me.AG_BattCh.ScaleLinesMinorColor = System.Drawing.Color.Gray
        Me.AG_BattCh.ScaleLinesMinorInnerRadius = 42
        Me.AG_BattCh.ScaleLinesMinorOuterRadius = 48
        Me.AG_BattCh.ScaleLinesMinorTicks = 9
        Me.AG_BattCh.ScaleLinesMinorWidth = 1
        Me.AG_BattCh.ScaleNumbersColor = System.Drawing.Color.Black
        Me.AG_BattCh.ScaleNumbersFormat = Nothing
        Me.AG_BattCh.ScaleNumbersRadius = 60
        Me.AG_BattCh.ScaleNumbersRotation = 0
        Me.AG_BattCh.ScaleNumbersStartScaleLine = 0
        Me.AG_BattCh.ScaleNumbersStepScaleLines = 1
        Me.AG_BattCh.UnitValue1 = System.Windows.Forms.AGauge.UnitValue.Percent
        Me.AG_BattCh.UnitValue2 = System.Windows.Forms.AGauge.UnitValue.None
        Me.AG_BattCh.Value1 = 0!
        Me.AG_BattCh.Value2 = 0!
        '
        'WinNUT
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.CB_CurrentLog)
        Me.Controls.Add(Me.GB_InF_Dial)
        Me.Controls.Add(Me.GB_InV_Dial)
        Me.Controls.Add(Me.GB_BattV_Dial)
        Me.Controls.Add(Me.GB_Load_Dial)
        Me.Controls.Add(Me.GB_OutV_Dial)
        Me.Controls.Add(Me.GB_Status)
        Me.Controls.Add(Me.GB_BattCh_Dial)
        Me.Controls.Add(Me.Main_Menu)
        Me.DoubleBuffered = True
        Me.MainMenuStrip = Me.Main_Menu
        Me.MaximizeBox = False
        Me.Name = "WinNUT"
        Me.ContextMenu_Systray.ResumeLayout(False)
        Me.Main_Menu.ResumeLayout(False)
        Me.Main_Menu.PerformLayout()
        Me.GB_Status.ResumeLayout(False)
        Me.GB_InV_Dial.ResumeLayout(False)
        Me.GB_InV_Dial.PerformLayout()
        Me.GB_OutV_Dial.ResumeLayout(False)
        Me.GB_OutV_Dial.PerformLayout()
        Me.GB_BattCh_Dial.ResumeLayout(False)
        Me.GB_BattCh_Dial.PerformLayout()
        CType(Me.PBox_Battery_State, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GB_Load_Dial.ResumeLayout(False)
        Me.GB_Load_Dial.PerformLayout()
        Me.GB_BattV_Dial.ResumeLayout(False)
        Me.GB_BattV_Dial.PerformLayout()
        Me.GB_InF_Dial.ResumeLayout(False)
        Me.GB_InF_Dial.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents Menu_Check_Update As ToolStripMenuItem
    Friend WithEvents NotifyIcon As NotifyIcon
    Friend WithEvents Main_Menu As MenuStrip
    Friend WithEvents Menu_File As ToolStripMenuItem
    Friend WithEvents Menu_UPS_Var As ToolStripMenuItem
    Friend WithEvents Menu_Quit As ToolStripMenuItem
    Friend WithEvents Menu_Connection As ToolStripMenuItem
    Friend WithEvents Menu_Settings As ToolStripMenuItem
    Friend WithEvents Menu_Help As ToolStripMenuItem
    Friend WithEvents Menu_About As ToolStripMenuItem
    Friend WithEvents Menu_Help_Sep1 As ToolStripSeparator
    Friend WithEvents Menu_Update As ToolStripMenuItem
    Friend WithEvents Menu_Reconnect As ToolStripMenuItem
    Friend WithEvents Menu_Disconnect As ToolStripMenuItem
    Friend WithEvents ContextMenu_Systray As ContextMenuStrip
    Friend WithEvents Menu_Sys_Settings As ToolStripMenuItem
    Friend WithEvents Menu_Sys_Sep1 As ToolStripSeparator
    Friend WithEvents Menu_Sys_Update As ToolStripMenuItem
    Friend WithEvents Menu_Sys_Sep2 As ToolStripSeparator
    Friend WithEvents Menu_Sys_About As ToolStripMenuItem
    Friend WithEvents Menu_Sys_Sep3 As ToolStripSeparator
    Friend WithEvents Menu_Sys_Exit As ToolStripMenuItem
    Friend WithEvents GB_Status As GroupBox
    Friend WithEvents Lbl_VSerial As Label
    Friend WithEvents Lbl_VFirmware As Label
    Friend WithEvents Lbl_VName As Label
    Friend WithEvents Lbl_VMfr As Label
    Friend WithEvents Lbl_VRTime As Label
    Friend WithEvents Lbl_VOB As Label
    Friend WithEvents Lbl_VOLoad As Label
    Friend WithEvents Lbl_VBL As Label
    Friend WithEvents Lbl_VOL As Label
    Friend WithEvents Lbl_Firmware As Label
    Friend WithEvents Lbl_Serial As Label
    Friend WithEvents Lbl_Name As Label
    Friend WithEvents Lbl_Mfr As Label
    Friend WithEvents Lbl_RTime As Label
    Friend WithEvents Lbl_BL As Label
    Friend WithEvents Lbl_OLoad As Label
    Friend WithEvents Lbl_OB As Label
    Friend WithEvents Lbl_OL As Label
    Friend WithEvents GB_BattCh_Dial As GroupBox
    Friend WithEvents Lbl_BattCh_Dial As Label
    Friend WithEvents GB_OutV_Dial As GroupBox
    Friend WithEvents Lbl_OutV_Dial As Label
    Friend WithEvents GB_Load_Dial As GroupBox
    Friend WithEvents Lbl_Load_Dial As Label
    Friend WithEvents GB_BattV_Dial As GroupBox
    Friend WithEvents Lbl_BattV_Dial As Label
    Friend WithEvents GB_InV_Dial As GroupBox
    Friend WithEvents Lbl_InV_Dial As Label
    Friend WithEvents AG_InV As AGauge
    Friend WithEvents AG_OutV As AGauge
    Friend WithEvents AG_BattCh As AGauge
    Friend WithEvents AG_Load As AGauge
    Friend WithEvents AG_BattV As AGauge
    Friend WithEvents GB_InF_Dial As GroupBox
    Friend WithEvents AG_InF As AGauge
    Friend WithEvents Lbl_InF_Dial As Label
    Friend WithEvents CB_CurrentLog As ComboBox
    Friend WithEvents Menu_Import_Ini As ToolStripMenuItem
    Friend WithEvents PBox_Battery_State As PictureBox
End Class
