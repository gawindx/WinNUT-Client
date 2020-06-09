Global $errorstring = "no error"
Global $haserror = 0
Global $ProgramDesc = "Windows NUT Client"
Global $ProgramVersion = ""

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
Global $ipv6mode = False
Global $ResolvedHost = ""
Global $PortProxyAddress = "127.0.0.1"
Global $PortProxyPortMin = 20000
Global $PortProxyPortMax = 30000
Global $PortProxyPort = 0

Global $upsstatus = 0
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

Global $TreeView1
Global $varselected
Global $varvalue
Global $vardesc

Global $settingssubMenu, $exitMenu, $editMenu, $aboutMenu, $reconnectMenu, $DisconnectMenu, $listvarMenu

Global $guipref = 0
Global $guiabout = 0
Global $gui = 0
Global $fileMenu, 	$listvarMenu, $exitMenu, $editMenu, $reconnectMenu,	$settingsMenu
Global $settingssubMenu, $LanguageSettings, $LangSubMenuSystem, $helpMenu, $aboutMenu
Global $Label1, $Label2, $Label3, $Label4, $labelUpsRemain, $Label5
Global $Label14, $Label15, $Label16, $exitb, $toolb
Global $upsmfr, $upsmodel, $upsserial, $upsfirmware

Global $wPanel = 0

Global $idTrayExit,$idTrayPref,$idTrayAbout
Global $runasexe = False

Global $Active_Countdown = 0, $Suspend_Countdown = 0
Global $ShutdownDelay, $grace_time
Global $Running_Shutdown, $lbl_countdown, $lbl_ups_status
Global $guishutdown, $Grace_btn, $Shutdown_btn

Global $ReconnectTry = 0
Global $LastSocket = 0

;nutOption
Global $optionsStruct = 0
Global $inipath = @ScriptDir & "\" & "ups.ini"
Global $panel_bkg = 0
Global $clock_bkg_bgr = 0
Global $panel_bkg_bgr = 0
Global $optionList = ""

Global $MenuLangListhwd
Global $en_cours
