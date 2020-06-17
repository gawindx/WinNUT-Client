;Miscellanous nutOption
Global $optionsStruct = 0
Global $inipath = @ScriptDir & "\ups.ini"
Global $panel_bkg = 0
Global $clock_bkg_bgr = 0
Global $panel_bkg_bgr = 0
Global $optionList = ""
Global $runasexe = False
Global $errorstring = "no error"
Global $haserror = 0
Global $ProgramDesc = "Windows NUT Client"
Global $ProgramName = "WinNUT"
Global $ProgramVersion = ""
Global $status = 0

;Icon's Consts
Global const $IconDLL = @ScriptDir & "\Resources\WinNUT_Icons.dll"
Global const $IDX_ICO_OL100 = 101
Global const $IDX_ICO_OL75 = 102
Global const $IDX_ICO_OL50 = 103
Global const $IDX_ICO_OL25 = 104
Global const $IDX_ICO_OL0 = 105
Global const $IDX_ICO_OB100 = 106
Global const $IDX_ICO_OB75 = 107
Global const $IDX_ICO_OB50 = 108
Global const $IDX_ICO_OB25 = 109
Global const $IDX_ICO_OB0 = 110

Global const $IDX_BATT_0 = 1
Global const $IDX_BATT_25 = 2
Global const $IDX_BATT_50 = 4
Global const $IDX_BATT_75 = 8
Global const $IDX_BATT_100 = 16
Global const $IDX_OL = 32
Global const $WIN_DARK = 64
Global const $IDX_ICO_OFFLINE = 128
Global const $IDX_ICO_RETRY = 256
Global const $IDX_ICO_VIEWLOG = 2001
Global const $IDX_ICO_DELETELOG = 2002
Global const $IconIdxOffset = 1024
Global $IDX_ICON_BASE_APP
Global $IDX_ICON_BASE_SYS
Global $ActualIcon_IDX
Global $LastIcon_IDX = $IDX_ICO_OFFLINE

;GUI_WinNUT's Variables
Global $gui = 0
Global $guipref = 0
Global $guiabout = 0
Global $guishutdown = 0

;Color's Consts
Global const $green = 0x00FF00
Global const $red = 0xFF0000
Global const $yellow = 0xffff00
Global const $gray = 0xD4D0C8
Global const $aqua = 0x00ffff

;MainGui_Menu's Variables
Global $fileMenu, $listvarMenu, $exitMenu, $editMenu, $reconnectMenu, $settingsMenu
Global $exitMenu, $editMenu, $aboutMenu, $reconnectMenu, $DisconnectMenu, $listvarMenu
Global $settingssubMenu, $LanguageSettings, $LangSubMenuSystem, $helpMenu, $aboutMenu
Global $Label1, $Label2, $Label3, $Label4, $labelUpsRemain, $Label5
Global $Label14, $Label15, $Label16, $exitb, $toolb
Global $wPanel = 0
Global $MenuLangListhwd
Global $idTrayExit, $idTrayPref, $idTrayAbout

;GUI_TreeView's Variables
Global Const $s_TVITEMEX = "uint;uint;uint;uint;ptr;int;int;int;int;uint;int"
Global $TVM_SETITEM = 0
Global $TreeView1
Global $varselected
Global $varvalue
Global $vardesc

;Graphical's Variables
Global $painting = 0
Global $clock_bkg = 0
Global $needle1 = 0
Global $needle2 = 0
Global $needle3  = 0
Global $needle4 = 0
Global $needle5 = 0
Global $needle6 = 0
Global $dial1 = 0
Global $dial2 = 0
Global $dial3  = 0
Global $dial4 = 0
Global $dial5 = 0
Global $dial6 = 0

;UPS_Data's Variables
Global $upsPF = 0.6
Global $inputv = 0
Global $outputv = 0
Global $inputf = 0
Global $battv = 0
Global $upsch = 0
Global $upsl = 0
Global $inputVol = 0
Global $outputVol = 0
Global $inputFreq = 0
Global $battVol = 0
Global $upsLoad = 0
Global $battCh = 0
Global $battruntime = 0
Global $batcapacity = 0
Global $upsstatus = 0
Global $upsonline = 0
Global $upsonbatt = 0
Global $upslowbatt = 0
Global $upsoverload = 0
Global $upsoutpower = 0
Global $remainTimeLabel = 0
Global $mfr = ""
Global $name = ""
Global $serial = ""
Global $firmware = ""
Global $battrtimeStr = "00:00"
Global $upsmfr, $upsmodel, $upsserial, $upsfirmware

;IPV4/IPV6's Variables 
Global $ipv6mode = False
Global $ResolvedHost = ""
Global $PortProxyAddress = "127.0.0.1"
Global $PortProxyPortMin = 20000
Global $PortProxyPortMax = 30000
Global $PortProxyPort = 0

;ShutDown's Variables
Global $Active_Countdown = 0, $Suspend_Countdown = 0
Global $ShutdownDelay, $grace_time
Global $Running_Shutdown, $lbl_countdown, $lbl_ups_status
Global $Grace_btn, $Shutdown_btn
Global $In_progress

;Language's Variables
Global $_i18n_LangBase = @ScriptDir
Global $_i18n_Default = 'en-US'
Global $_i18n_Language = 'system'
Global $_i18n_TranslationFile4 = ""
Global $_i18n_TranslationFile2 = ""
Global $_i18n_TranslationFileDefault = ""
Global $i18n_locale = ""
Global $i18n_locale2 = ""

;Reconnect's Variable
Global $ReconnectTry = 0
Global $MaxReconnectTry = 30
Global $ReconnectDelay = 30000
Global $LastSocket = 0
Global $socket = 0

;Debug's Consts
Global const $DBG_NOTICE = 1
Global const $DBG_WARNING = 2
Global const $DBG_ERROR = 4
Global const $DBG_DEBUG = 8
Global $oDBG_LEVEL = ObjCreate("Scripting.Dictionary")
Global $oDBG_LVL_TXT = ObjCreate("Scripting.Dictionary")
	$oDBG_LVL_TXT.Add($DBG_NOTICE, " [NOTICE] : ")
	$oDBG_LVL_TXT.Add($DBG_WARNING, " [WARNING] : ")
	$oDBG_LVL_TXT.Add($DBG_ERROR, " [ERROR] : ")
	$oDBG_LVL_TXT.Add($DBG_DEBUG, " [DEBUG] : ")
Global const $LOG_GUI = 16
Global const $LOG2FILE = 32
Global const $START_LOG_STR = "====================================================================="
Global $LogFile = @ScriptDir & "\Log.txt"