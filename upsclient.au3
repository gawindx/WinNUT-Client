#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_icon=upsicon.ico
#AutoIt3Wrapper_outfile=upsclient.exe
#AutoIt3Wrapper_Compression=4
#AutoIt3Wrapper_Res_Comment=Windows NUT Client
#AutoIt3Wrapper_Res_Description=WinNutClient
#AutoIt3Wrapper_Res_Fileversion=1.6.0.0
#AutoIt3Wrapper_Res_LegalCopyright=Freeware
#AutoIt3Wrapper_Res_Language=1033
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_Allow_Decompile=n
#Region converted Directives from D:\winnut\winnut\upsclient.au3.ini
#EndRegion converted Directives from D:\winnut\winnut\upsclient.au3.ini
;
#include <GUIConstants.au3>
#include <Color.au3>
#Include <Constants.au3>
#include <Array.au3>
#include <nutGlobal.au3> ;not really required
#include <nutDraw.au3>
#include <nutColor.au3>
#include <nutOption.au3>
#include <nutGui.au3>
#include <nutNetwork.au3>
#Include <nutTreeView.au3>
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
Global $mfr = ""
Global $name = ""
Global $serial = ""
Global $firmware = ""

Global $TreeView1
Global $varselected
Global $varvalue
Global $vardesc

Global $settingssubMenu,$exitMenu,$editMenu,$aboutMenu,$reconnectMenu,$listvarMenu

Global $guipref = 0
Global $upsmfr,$upsmodel,$upsserial,$upsfirmware,$toolb,$exitb

Global $wPanel = 0

;This function repaints all needles when required and passes on control
;to internal AUTOIT repaint handler
;This is registered for WM_PAINT event
Func rePaint()

	repaintNeedle($needle6 , $battCh ,$dial6 ,0 , 100 )
	repaintNeedle($needle4 ,  $battVol ,$dial4 ,getOption("minbattv") , getOption("maxbattv") )
	repaintNeedle($needle5 , $upsLoad ,$dial5 ,getOption("minupsl") , getOption("maxupsl") )
	repaintNeedle($needle1 , $inputVol ,$dial1, getOption("mininputv") , getOption("maxinputv") )
	repaintNeedle($needle2 , $outputVol , $dial2, getOption("minoutputv") , getOption("maxoutputv"))
	repaintNeedle($needle3 , $inputFreq , $dial3 ,getOption("mininputf"), getOption("maxinputf") )
	return $GUI_RUNDEFMSG
EndFunc

Func prefGui()
	Local $Iipaddr = 0
	Local $Iport = 0
	Local $Iupsname = 0
	Local $Idelay = 0
	Local $tempcolor1 , $tempcolor2
	Local $result = 0
	$tempcolor2 = $clock_bkg
	$tempcolor1 = $panel_bkg
	ReadParams()
	if $guipref <> 0 Then
		GuiDelete($guipref)
		$guipref = 0
	EndIf
	$guipref = GUICreate("Preferences", 364, 331, 190, 113,-1,-1,$gui  )
	GUISetIcon(@ScriptDir & "\upsicon.ico")
	$Bcancel = GUICtrlCreateButton("Cancel", 286, 298, 75, 25, 0)
	$Bapply = GUICtrlCreateButton("Apply", 206, 298, 75, 25, 0)
	$Bok = GUICtrlCreateButton("OK", 126, 298, 75, 25, 0)
	$Tconnection = GUICtrlCreateTab(0, 0, 361, 289)
	$TSconnection = GUICtrlCreateTabItem("Connection")
	$Iipaddr = GUICtrlCreateInput(GetOption("ipaddr"), 74, 37, 249, 21)
	$Lipaddr = GUICtrlCreateLabel("UPS host :", 16, 40, 59, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	$Lport = GUICtrlCreateLabel("UPS port :", 16, 82, 59, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	$Iport = GUICtrlCreateInput(GetOption("port"), 74, 77, 73, 21)
	$Lname = GUICtrlCreateLabel("UPS name :", 16, 122, 59, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	$Iupsname = GUICtrlCreateInput(GetOption("upsname"), 74, 120, 249, 21)
	$Ldelay = GUICtrlCreateLabel("Delay :", 16, 162, 51, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	$Idelay = GUICtrlCreateInput(GetOption("delay"), 74, 159, 73, 21)
	$Checkbox1 = GUICtrlCreateCheckbox("ACheckbox1", 334, 256, 17, 17,BitOr($BS_AUTOCHECKBOX,$WS_TABSTOP ),$WS_EX_STATICEDGE )
	$Label9 = GUICtrlCreateLabel("Re-establish connection", 217, 256, 115, 17)
	
;	GUICtrlCreateTabItem("")
	$TabSheet2 = GUICtrlCreateTabItem("Colors")
	GUICtrlCreateLabel("Panel background color", 16, 48, 131, 25)
	GUICtrlCreateLabel("Analogue background color", 16, 106, 179, 25)
	$colorchoose1 = GUICtrlCreateLabel("", 232, 40, 25, 25,Bitor($SS_SUNKEN,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetBkColor(-1, $panel_bkg)
	$colorchoose2 = GUICtrlCreateLabel("", 232, 104, 25, 25,Bitor($SS_SUNKEN,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetBkColor(-1, $clock_bkg)
	
	GUICtrlCreateTabItem("Calibration")
	GUICtrlCreateLabel("Input Voltage", 16, 56, 75, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel("Input Frequency", 16, 96, 91, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel("Output Voltage", 16, 136, 91, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel("UPS Load", 16, 176, 91, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel("Battery Voltage", 16, 216, 91, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))	
	GUICtrlCreateLabel("Min", 136, 32, 23, 19)
	GUICtrlCreateLabel("Max", 219, 32, 25, 19)
	$lminInputVoltage = GUICtrlCreateInput(GetOption("mininputv"), 121, 56, 49, 23)
	$lmaxInputVoltage = GUICtrlCreateInput(GetOption("maxinputv"), 210, 56, 49, 23)
	$lminInputFreq = GUICtrlCreateInput(GetOption("mininputf"), 121, 96, 49, 23)
	$lmaxInputFreq = GUICtrlCreateInput(GetOption("maxinputf"), 210, 96, 49, 23)
	$lminOutputVoltage = GUICtrlCreateInput(GetOption("minoutputv"), 121, 136, 49, 23)
	$lmaxOutputVoltage = GUICtrlCreateInput(GetOption("maxoutputv"), 210, 136, 49, 23)
	$lminUpsLoad = GUICtrlCreateInput(GetOption("minupsl"), 121, 176, 49, 23)
	$lmaxUpsLoad = GUICtrlCreateInput(GetOption("maxupsl"), 210, 176, 49, 23)
	$lminBattVoltage = GUICtrlCreateInput(GetOption("minbattv"), 121, 216, 49, 23)
	$lmaxBattVoltage = GUICtrlCreateInput(GetOption("maxbattv"), 210, 216, 49, 23)

	$TabSheet1 = GUICtrlCreateTabItem("Misc")
	GUICtrlCreateLabel("Minimize to tray", 16, 42, 99, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$chMinimizeTray = GUICtrlCreateCheckbox("MinimizeTray", 224, 39, 17, 17,BitOr($BS_AUTOCHECKBOX,$WS_TABSTOP ),$WS_EX_STATICEDGE)
	if GetOption("minimizetray") == 0 Then
		GuiCtrlSetState($chMinimizeTray , $GUI_UNCHECKED)
	Else
		GuiCtrlSetState($chMinimizeTray , $GUI_CHECKED)
	EndIf
	GUICtrlCreateLabel("Shutdown if battery lower then", 16, 84, 179, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$lshutdownPC = GUICtrlCreateInput(GetOption("shutdownpc"), 217, 81, 25, 23)
	GUICtrlCreateLabel("%", 248, 84, 15, 19)

	GUICtrlCreateTabItem("")	
	GuiSetState(@SW_DISABLE,$gui )
	GUISetState(@SW_SHOW,$guipref)

	While 1
		$nMsg1 = GUIGetMsg()
		Switch $nMsg1
			
			Case $GUI_EVENT_CLOSE
				ExitLoop
				
			Case $Bapply
				SetOption("ipaddr",GuiCtrlRead($Iipaddr ) , "string")
				SetOption("port",GuiCtrlRead($Iport ) , "number")
				SetOption("upsname",GuiCtrlRead($Iupsname ) , "string")
				SetOption("delay",GuiCtrlRead($Idelay ) , "number")
				SetOption("mininputv",GuiCtrlRead($lminInputVoltage ) , "number")
				SetOption("maxinputv",GuiCtrlRead($lmaxInputVoltage ) , "number")
				SetOption("minoutputv",GuiCtrlRead($lminOutputVoltage ) , "number")
				SetOption("maxoutputv",GuiCtrlRead($lmaxOutputVoltage ) , "number")
				SetOption("mininputf",GuiCtrlRead($lminInputFreq ) , "number")
				SetOption("maxinputf",GuiCtrlRead($lmaxInputFreq ) , "number")
				SetOption("minupsl",GuiCtrlRead($lminUpsLoad ) , "number")
				SetOption("maxupsl",GuiCtrlRead($lmaxUpsLoad ) , "number")
				SetOption("minbattv",GuiCtrlRead($lminBattVoltage ) , "number")
				SetOption("maxbattv",GuiCtrlRead($lmaxBattVoltage ) , "number")
				SetOption("shutdownpc",GuiCtrlRead($lshutdownPC ) , "number")
				$minimizetray = GuiCtrlRead($chMinimizeTray)
				if $minimizetray == $GUI_CHECKED Then
					SetOption("minimizetray",1 , "number")
				Else
					SetOption("minimizetray",0 , "number")
				EndIf
				$panel_bkg = $tempcolor1
				$clock_bkg = $tempcolor2
				$clock_bkg_bgr = RGBtoBGR($clock_bkg)
				GuiSetBkColor($clock_bkg , $dial1)
				GuiSetBkColor($clock_bkg , $dial2)
				GuiSetBkColor($clock_bkg , $dial3)
				GuiSetBkColor($clock_bkg , $dial4)
				GuiSetBkColor($clock_bkg , $dial5)
				GuiSetBkColor($clock_bkg , $dial6)
				GUISetBkColor($panel_bkg ,  $wPanel )
				$result = 1
				WriteParams()
			Case $Bok
				SetOption("ipaddr",GuiCtrlRead($Iipaddr ) , "string")
				SetOption("port",GuiCtrlRead($Iport ) , "number")
				SetOption("upsname",GuiCtrlRead($Iupsname ) , "string")
				SetOption("delay",GuiCtrlRead($Idelay ) , "number")
				SetOption("mininputv",GuiCtrlRead($lminInputVoltage ) , "number")
				SetOption("maxinputv",GuiCtrlRead($lmaxInputVoltage ) , "number")
				SetOption("minoutputv",GuiCtrlRead($lminOutputVoltage ) , "number")
				SetOption("maxoutputv",GuiCtrlRead($lmaxOutputVoltage ) , "number")
				SetOption("mininputf",GuiCtrlRead($lminInputFreq ) , "number")
				SetOption("maxinputf",GuiCtrlRead($lmaxInputFreq ) , "number")
				SetOption("minupsl",GuiCtrlRead($lminUpsLoad ) , "number")
				SetOption("maxupsl",GuiCtrlRead($lmaxUpsLoad ) , "number")
				SetOption("minbattv",GuiCtrlRead($lminBattVoltage ) , "number")
				SetOption("maxbattv",GuiCtrlRead($lmaxBattVoltage ) , "number")
				SetOption("shutdownpc",GuiCtrlRead($lshutdownPC ) , "number")
				$minimizetray = GuiCtrlRead($chMinimizeTray) 
				if $minimizetray == $GUI_CHECKED Then
					SetOption("minimizetray",1 , "number")
				Else
					SetOption("minimizetray",0 , "number")
				EndIf
				$panel_bkg = $tempcolor1
				$clock_bkg = $tempcolor2
				$clock_bkg_bgr = RGBtoBGR($clock_bkg)
				GuiSetBkColor($clock_bkg , $dial1)
				GuiSetBkColor($clock_bkg , $dial2)
				GuiSetBkColor($clock_bkg , $dial3)
				GuiSetBkColor($clock_bkg , $dial4)
				GuiSetBkColor($clock_bkg , $dial5)
				GuiSetBkColor($clock_bkg , $dial6)
				GUISetBkColor($panel_bkg ,  $wPanel )
				WriteParams()
				$result = 1
				ExitLoop
			Case $Bcancel
				
				ExitLoop
			Case $colorchoose1
				$tempcolor1 = _ChooseColor ( 2, 0,2)
				if $tempcolor1 <> -1 Then
					;$panel_bkg = $tempcolor
					GuiCtrlSetBkColor($colorchoose1,$tempcolor1)
				Else
					$tempcolor1 = $panel_bkg
				EndIf
				$result = 1
			Case $colorchoose2
				$tempcolor2 = _ChooseColor ( 2, 0,2)
				if $tempcolor2 <> -1 Then
					;$panel_bkg = $tempcolor
					GuiCtrlSetBkColor($colorchoose2,$tempcolor2)
				Else
					$tempcolor2 = $clock_bkg
				EndIf
				$result = 1 
		EndSwitch
	WEnd
	GuiSetState(@SW_ENABLE,$gui )
	GuiDelete($guipref)
	WinActivate($gui)
	$guipref = 0
	return $result
EndFunc


Func updateVarList()

	$selected = _GUICtrlTreeViewGetTree1($TreeView1, "." , 0)
	GuiCtrlSetData($varselected , $selected)
	$upsval = ""
	$upsdesc = ""
	$checkstatus1 = GetUPSVar(GetOption("upsname") , $selected , $upsval)
	$checkstatus2 = GetUPSDescVar(GetOption("upsname") , $selected , $upsdesc)
	if $checkstatus1 == -1 or $checkstatus2 == -1 Then
		$upsval = ""
		$upsdesc = ""
	EndIf
	if GuiCtrlRead($varvalue ) <> $upsval Then
		GuiCtrlSetData($varvalue , $upsval)
	EndIf
	if GuiCtrlRead($vardesc ) <> $upsdesc Then
		GuiCtrlSetData($vardesc , $upsdesc)
	EndIf
	
EndFunc


Func varlistGui()
	
	$varlist = ""
	$templist = ""
	AdlibDisable()
	$status1 = ListUPSVars(GetOption("upsname") , $varlist)
	$varlist = StringReplace($varlist , GetOption("upsname") , "")
	$vars = StringSplit($varlist , "VAR",1)
	AdlibEnable("Update",1000)
	$guilistvar = GUICreate("LIST UPS Variables", 365, 331, 196, 108, -1 , -1 , $gui)
	GUISetIcon(@ScriptDir & "\upsicon.ico")
	$TreeView1 = GUICtrlCreateTreeView(0, 8, 361, 169)
	;$state = GUICtrlSetImage($TreeView1, @ScriptDir & "\light.ico", -1 , 4)

	$Group1 = GUICtrlCreateGroup("Item properties", 0, 184, 361, 105, $BS_CENTER)
	$Label1 = GUICtrlCreateLabel("Name :", 8, 200, 38, 17)
	$Label2 = GUICtrlCreateLabel("Value :", 8, 232, 37, 17)
	$Label3 = GUICtrlCreateLabel("Description :", 8, 264, 63, 17)
	$varselected = GUICtrlCreateLabel("", 50, 200, 291, 17, $SS_SUNKEN)
	$varvalue = GUICtrlCreateLabel("", 50, 232, 291, 17, $SS_SUNKEN)
	$vardesc = GUICtrlCreateLabel("", 72, 264, 283, 17, $SS_SUNKEN)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	$Button1 = GUICtrlCreateButton("Reload", 80, 296, 65, 25, 0)
	$Button2 = GUICtrlCreateButton("Clear", 200, 296, 65, 25, 0)
	GuiSetState(@SW_DISABLE,$gui )
	GUISetState(@SW_SHOW , $guilistvar)

	$varcount = Ubound($vars) - 2
	$varlist = "";
	for $i = 3 to $varcount
		
		if $i == $varcount Then
			ContinueLoop
		EndIf
		$templist = StringSplit($vars[$i],'"')
		$curpath = StringStripWS($templist[1],3)
		_addPath($TreeView1, $curpath )
	Next
	_SetIcons($TreeView1, 0)
	_GUICtrlTreeView_Expand($TreeView1)
	
	AdlibDisable()
	AdlibEnable("updateVarList",500)
	While 1
		$nMsg = GUIGetMsg(1)
		if ($nMsg[0] == $GUI_EVENT_CLOSE) Then
			AdlibDisable()
			GuiSetState(@SW_ENABLE,$gui )
			GuiDelete($guilistvar)
			WinActivate($gui)
			AdlibEnable("Update",1000)

			return 
		EndIf
		if ($nMsg[0] == $Button2) Then
			;_GUICtrlTreeViewDeleteAllItems ( $TreeView1 )
		EndIf
	WEnd	
EndFunc

Func GetUPSInfo()
	Local $status = 0
	$mfr = ""
	$name = ""
	$serial = ""
	$firmware = ""
	if $socket == 0 Then ; not connected to server/connection lost
		Return
	EndIf
	$status = GetUPSVar(GetOption("upsname") ,"ups.mfr" , $mfr)
	if $status = -1 then ;UPS name wrong or variable not supported or connection lost

		if $socket == 0 Then
			Return
		EndIf
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			$mfr = ""
			WriteLog("Disconnecting from server")
			TCPSend($socket,"LOGOUT")
			TCPCloseSocket($socket)
			WriteLog("Disconnected from server")
			$socket = 0
			ResetGui()
			Return
		EndIf
	EndIf

	$status = GetUPSVar(GetOption("upsname") ,"ups.model" , $name)
	if $status = -1 then
		if $socket == 0 Then
			Return
		EndIf
		$name =""
	EndIf

	$status = GetUPSVar(GetOption("upsname") ,"ups.serial" , $serial)
	if $status = -1 then
		if $socket == 0 Then
			Return
		EndIf
		$serial =""
	EndIf

	$status = GetUPSVar(GetOption("upsname") ,"ups.firmware" , $firmware)
	if $status = -1 then
		if $socket == 0 Then
			Return
		EndIf
		$firmware =""
	EndIf
	
EndFunc

Func SetUPSInfo()
	
	if $socket == 0 Then ;if not connected or connection lost
		$mfr = ""
		$name = ""
		$serial = ""
		$firmware = ""
	EndIf
	GuiCtrlSetData($upsmfr,$mfr)
	GuiCtrlSetData($upsmodel,$name)
	GuiCtrlSetData($upsserial,$serial)
	GuiCtrlSetData($upsfirmware,$firmware)

EndFunc


Func GetData()
	
	Local $status = "0"
	$ups_name = GetOption("upsname")
	if $socket == 0 then
		return -1
	EndIf
	GetUPSVar($ups_name ,"battery.charge" , $battch)
	GetUPSVar($ups_name ,"battery.voltage",$battVol)
	GetUPSVar($ups_name ,"input.frequency",$inputFreq)
	GetUPSVar($ups_name ,"input.voltage",$inputVol)
	GetUPSVar($ups_name ,"output.voltage",$outputVol)
	GetUPSVar($ups_name ,"ups.load",$upsLoad)
	GetUPSVar($ups_name ,"ups.status",$upsstatus)
	return 0
EndFunc

Func Nothing()
	
	Return
EndFunc



Func UpdateValue(byref $needle , $value , $label , $whandle ,$min = 170, $max = 270, $force = 0)
	
	$oldval = Round (GuiCtrlRead($label))
	if $oldval < $min Then
		$oldval = $min
	EndIf
	if $oldval > $max Then
		$oldval = $max
	EndIf
	if $oldval == Round($value) and $force == 0 Then
		Return
	EndIf
	
	GuiCtrlSetData($label , $value )
	$value = Round($value )
	if $value < $min Then
		$value = $min
	EndIf
	if $value > $max Then
		$value = $max
	EndIf
	$oldneedle = ($oldval - $min) / ( ($max - $min ) / 100 )
	if $oldneedle > 0 or $oldneedle == 0 then
		DrawNeedle(15 + $oldneedle ,$clock_bkg_bgr , $whandle , $needle)
	EndIf

	$setneedle =($value - $min)/ ( ($max - $min ) / 100 )
	DrawNeedle(15 + $setneedle ,0x0 , $whandle , $needle)
	
EndFunc

Func ResetGui()
	if $socket == 0  Then
		$battVol = 0
		$battCh = 0
		$upsLoad = 0
		$inputVol = 0
		$outputVol = 0
		$inputFreq = 0
	EndIf
	UpdateValue($needle4 , 0 , $battv ,$dial4 , getOption("minbattv"), getOption("maxbattv") )
	UpdateValue($needle5 , 0 , $upsl , $dial5 , getOption("minupsl") , getOption("maxupsl") )
	UpdateValue($needle6 , 0 , $upsch , $dial6 , 0 , 100 )
	UpdateValue($needle1 , 0, $inputv ,$dial1 , getOption("mininputv") , getOption("maxinputv"))
	UpdateValue($needle2 , 0, $outputv ,$dial2 , getOption("minoutputv") , getOption("maxoutputv"))
	UpdateValue($needle3 , 0, $inputf , $dial3 , getOption("mininputf") , getOption("maxinputf") )

	GuiCtrlSetBkColor( $upsonline , $gray )
	GuiCtrlSetBkColor($upsonbatt , $gray )
	GUICtrlSetBkColor($upsoverload , $gray )
	GUICtrlSetBkColor($upslowbatt , $gray )
	if ($socket <> 0 ) Then
		SetUPSInfo()
	EndIf
	rePaint()

EndFunc


Func Update()
	
	;if $socket == 0 Then
	;	Return
	;EndIf
	$status =GetData()
	if $socket == 0 then ; connection lost so throw all needles to left
		ResetGui()
		Return
	EndIf

	if $upsstatus == "OL" Then
		SetColor($green , $wPanel , $upsonline )
		SetColor(0xffffff , $wPanel , $upsonbatt )
	Else
		SetColor($yellow , $wPanel , $upsonbatt )
		SetColor(0xffffff , $wPanel , $upsonline )
	EndIf
	if $upsLoad > 100 Then
		SetColor($red , $wPanel , $upsoverload )
	Else
		SetColor(0xffffff , $wPanel , $upsoverload )
	EndIf
	if $battCh < 40 Then
		SetColor($red , $wPanel , $upslowbatt )
	Else
		SetColor(0xffffff , $wPanel , $upslowbatt )
	EndIf
	UpdateValue($needle4 , $battVol , $battv , $dial4 , getOption("minbattv") , getOption("maxbattv") )
	UpdateValue($needle5 , $upsLoad , $upsl , $dial5 , getOption("minupsl") , getOption("maxupsl") )
	UpdateValue($needle6 , $battCh , $upsch , $dial6 , 0 , 100 )
	UpdateValue($needle1 , $inputVol, $inputv , $dial1 ,getOption("mininputv") , getOption("maxinputv"))
	UpdateValue($needle2 , $outputVol, $outputv , $dial2 , getOption("minoutputv") , getOption("maxoutputv"))
	UpdateValue($needle3 , $inputFreq , $inputf , $dial3 , getOption("mininputf") , getOption("maxinputf") )
	rePaint()
	;if connection to UPS is in fact alive and charge below shutdown setting and ups is not online
	if ($battch <  GetOption("shutdownpc")) and ($upsstatus <>  "OL" and $socket <> 0) Then
		Shutdown(13) ;Shutdown PC if battery charge lower then given percentage and UPS offline
	EndIf
EndFunc


Func DrawDial($left , $top , $basescale , $title , $units , ByRef $value , ByRef $needle , $scale = 1 , $leftG = 20, $rightG = 70)
	
	Local $group = 0
	$group = GUICreate(" " & $title, 150, 120, $left, $top, BitOR($WS_CHILD, $WS_DLGFRAME), $WS_EX_CLIENTEDGE, $gui)
	GUISetBkColor($clock_bkg,$group)
	GuiSwitch($group)
	GuiCtrlCreateLabel($title,0,0,150,14,$SS_CENTER )

	for $x = 0 to 100 step 10
		if StringinStr($x / 20,".") = 0 Then
			GUICtrlCreateLabel("",$x * 1.2 + 15 , 15  , 1 , 15 , $SS_BLACKRECT)
			GuiCtrlSetState(-1,$GUI_DISABLE)
			if $x < 100 Then
				$test = GUICtrlCreateLabel("",$x * 1.2  + 16,15  , 11 , 5 , 0 )
				GuiCtrlSetState(-1,$GUI_DISABLE)
				if $x < $rightG and $x > $leftG then
					GUICtrlSetBkColor($test , 0x00ff00)
				Else
					GUICtrlSetBkColor($test , 0xff0000)
				EndIf
			EndIf
			$scalevalue = $basescale + $x / $scale
			switch $scalevalue
				Case 0 to 9
					GuiCtrlCreateLabel($scalevalue , $x * 1.2 + 13 , 25 , 20 , 10 )
				Case 10 to 99
					GuiCtrlCreateLabel($scalevalue , $x * 1.2 + 10 , 25 , 20 , 10 )
				Case 100 to 1000
					GuiCtrlCreateLabel($scalevalue , $x * 1.2 + 7 , 25 , 20 , 10 )
			EndSwitch
			GUICtrlSetFont(-1,7)
		Else
			GUICtrlCreateLabel("",$x * 1.2  + 15 , 15  , 1 , 5 , $SS_BLACKRECT )
			GuiCtrlSetState(-1,$GUI_DISABLE)
			$test = GUICtrlCreateLabel("", $x *1.2  + 16 ,15  , 11 , 5 , 0 )
			;GuiCtrlSetState(-1,$GUI_DISABLE)
			if $x < $rightG and $x > $leftG then
				GUICtrlSetBkColor($test , 0x00ff00)
			Else
				GUICtrlSetBkColor($test , 0xff0000)
			EndIf
		EndIf
	Next
	if $units =="%" then
		$value = GUICtrlCreateLabel(0 , 10  ,100  , 40 , 15, $SS_LEFT)
	Else
		$value = GUICtrlCreateLabel(220 , 10  ,100  , 40 , 15, $SS_LEFT)
	EndIf
	$label2 = GUICtrlCreateLabel($units , 116  ,100  , 25 , 15 ,$SS_RIGHT )
	$needle = GUICtrlCreateGraphic(10  ,35  , 120 , 60 )
	;GUICtrlSetBkColor(-1,$aqua)
	;$fill = GuiCtrlCreateGraphic(0 , 0 , 150 , 120)

	GuiSetState(@SW_SHOW,$group)
	;GuiCtrlSetBkColor(-1,0x00ffff)
	$result  = $group
	return $group
EndFunc


Func OpenMainWindow()
	
	$gui = GUICreate("WinNutClient", 640, 380, -1 , -1,Bitor($GUI_SS_DEFAULT_GUI ,$WS_CLIPCHILDREN))
	GUISetIcon(@ScriptDir & "\upsicon.ico")
	$fileMenu = GUICtrlCreateMenu("&File")
	$listvarMenu = GuiCtrlCreateMenuItem("&List UPS Vars",$fileMenu)
	$exitMenu = GUICtrlCreateMenuItem("&Exit", $fileMenu)
	$editMenu = GUICtrlCreateMenu("&Connection")
	$reconnectMenu = GUICtrlCreateMenuItem("&Reconnect",$editMenu)
	$settingsMenu = GUICtrlCreateMenu("&Settings")
	$settingssubMenu = GUICtrlCreateMenuItem("&Preferences",$settingsMenu)
	$helpMenu = GUICtrlCreateMenu("&Help")
	$aboutMenu = GUICtrlCreateMenuItem("About", $helpMenu)
	$log = GUICtrlCreateCombo("", 0, 335, 640, 25,Bitor($CBS_DROPDOWNLIST,0))
	;$Group8 = GUICtrlCreateGroup("", 0, 0, 598 + 25, 65)

	;	GuiSwitch($gui)
	$wPanel = GUICreate("", 150, 250,0, 70,BitOR($WS_CHILD, $WS_DLGFRAME), $WS_EX_CLIENTEDGE, $gui)
	GUISetBkColor($panel_bkg , $wPanel)
	$Label1 = GUICtrlCreateLabel("UPS On Line", 8, 18, 83, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$Label2 = GUICtrlCreateLabel("UPS On Battery", 8, 115 - 70, 83, 18,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$Label3 = GUICtrlCreateLabel("UPS Overload", 8, 142 - 70, 83, 18,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$Label4 = GUICtrlCreateLabel("UPS Battery low", 8, 170 - 70, 79, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$Label5 = GUICtrlCreateLabel("Manufact :", 8, 128, 51, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsonline = GUICtrlCreateLabel("", 121, 18, 20, 17, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$upsonbatt = GUICtrlCreateLabel("", 121, 45, 20, 17, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$upsoverload = GUICtrlCreateLabel("", 96 + 25, 142 - 70, 20, 17, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$upslowbatt = GUICtrlCreateLabel("", 96 + 25, 170 - 70, 20, 17, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$upsmfr = GUICtrlCreateLabel($mfr, 62, 198 - 70, 59, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$upsmodel = GUICtrlCreateLabel($name, 46, 225 - 70, 75 + 35, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$upsserial = GUICtrlCreateLabel($serial, 46, 252 - 70, 100, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$upsfirmware = GUICtrlCreateLabel($firmware, 62, 277 - 70, 59, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label14 = GUICtrlCreateLabel("Name :", 8, 225 - 70, 35, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$Label15 = GUICtrlCreateLabel("Serial :", 8, 252 - 70, 35, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$Label16 = GUICtrlCreateLabel("Firmware :", 8, 277 - 70, 51, 17,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	;$wPanel = GUICtrlCreateGroup("", 0, 70, 130 + 25, 240)
	GuiSwitch($gui)
	$Group8 = GUICreate("", 638, 60,0, 0,BitOR($WS_CHILD, $WS_BORDER), 0, $gui)
	$exitb = GUICtrlCreateButton("Exit", 10, 10, 73, 40, 0)
	$toolb = GUICtrlCreateButton("Settings", 102, 10, 73, 40, 0)
	GUISetState(@SW_SHOW,$Group8)
	GUISetState(@SW_SHOW,$wPanel)
	GUISetState(@SW_SHOW,$gui)
	;ConsoleWrite($calc & @CRLF)
	$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100 )
	$dial1 = DrawDial(160, 70 , GetOption("mininputv") , "Input Voltage" , "V" , $inputv , $needle1 , $calc)
	$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100 )
	$dial2 = DrawDial(480, 70 , GetOption("minoutputv") , "Output Voltage" , "V" , $outputv , $needle2 , $calc)
	$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100 )
	$dial3 = DrawDial(320, GetOption("maxinputf") , GetOption("mininputf") , "Input Frequency" , "Hz" , $inputf , $needle3 , $calc )
	$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100 )
	$dial4 = DrawDial(480, 200 , 0 , "Battery Voltage" , "V" , $battv , $needle4 , $calc , 20 , 120)
	$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100 )
	$dial5 = DrawDial(320, 200 , 0 , "UPS Load" , "%" , $upsl , $needle5 , $calc , -1 , 80)
	$dial6 = DrawDial(160, 200 , 0 , "Battery Charge" , "%" , $upsch , $needle6 , 1 , 30 , 101)
	;$testb = GUICtrlCreateButton("TEST", 180, 16, 73, 41, 0)
	
;	$minimizetray = GetOption("minimizetray")
;	if $minimizetray == 1 Then

;		TraySetIcon(@ScriptDir & "\upsicon.ico")
;		TraySetState (1)
;		Opt("TrayMenuMode",1)
;	EndIf	
		
EndFunc




Func aboutGui()
	AdlibDisable()
	$about = GUICreate("About", 324, 241, 271, 178)
	GUISetIcon(@ScriptDir & "\upsicon.ico")
	$GroupBox1 = GUICtrlCreateGroup("", 8, 8, 305, 185)
	$Image1 = GUICtrlCreatePic(@ScriptDir & "\ups.jpg", 16, 24, 105, 97, BitOR($SS_NOTIFY,$WS_GROUP))
	$Label10 = GUICtrlCreateLabel("WinNutClient", 152, 24, 72, 17, $WS_GROUP)
	$Label12 = GUICtrlCreateLabel("Version 1.6.0", 152, 48, 100, 17, $WS_GROUP)
	$Label14 = GUICtrlCreateLabel("Windows NUT Client", 16, 160, 170, 17, $WS_GROUP)
	$Label13 = GUICtrlCreateLabel("Copyright Michael Liberman 2006-2007", 16, 136, 270, 17, $WS_GROUP)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	$Button11 = GUICtrlCreateButton("&OK", 112, 208, 75, 25)
	GUISetState(@SW_SHOW,$about)
	GuiSetState(@SW_DISABLE,$gui)
	While 1
		$nMsg2 = GUIGetMsg()
		Switch $nMsg2
			
			Case $GUI_EVENT_CLOSE
				
				GuiSetState(@SW_ENABLE,$gui )
				GuiDelete($about)
				WinActivate($gui)
				if $haserror == 0 Then
					AdlibEnable("Update",1000)
				EndIf
				ExitLoop
			Case $Button11
				
				GuiSetState(@SW_ENABLE,$gui )
				GuiDelete($about)
				WinActivate($gui)
				if $haserror == 0 Then
					AdlibEnable("Update",1000)
				EndIf
				ExitLoop
		EndSwitch
	WEnd
	
EndFunc



func mainLoop()
	
	$minimizetray = GetOption("minimizetray")
	While 1
	if ($minimizetray == 1) Then
		$tMsg = TrayGetMsg()
		if $tMsg == $TRAY_EVENT_PRIMARYDOUBLE Then
			GuiSetState(@SW_SHOW, $gui)
			GuiSetState(@SW_RESTORE ,$gui )
		EndIf
	EndIf
	$nMsg = GUIGetMsg(1)
	if ($nMsg[0] == $GUI_EVENT_CLOSE and $nMsg[1]==$gui)  or $nMsg[0]==$exitMenu or $nMsg[0]== $exitb then
		TCPSend($socket,"LOGOUT")
		TCPCloseSocket($socket)
		TCPShutdown()
		Exit
	EndIf
	
	if ($nMsg[0] == $GUI_EVENT_MINIMIZE and $nMsg[1]==$gui and $minimizetray ==1) Then;minimize to tray
		GuiSetState(@SW_HIDE , $gui)
	EndIf
	
	if $nMsg[0] == $toolb or $nMsg[0]==$settingssubMenu Then
		AdlibDisable()
		$changedprefs = prefGui()
		if $changedprefs == 1 Then
			$painting = 1
			GuiDelete($dial1)
			GuiDelete($dial2)
			GuiDelete($dial3)
			GuiDelete($dial5)
			GuiDelete($dial4)
			DrawError(160 , 70 , "Delete")
			$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100 )			
			$dial1 = DrawDial(160, 70 , GetOption("mininputv") , "Input Voltage" , "V" , $inputv , $needle1 , $calc)
			$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100 )
			$dial2 = DrawDial(480, 70 , GetOption("minoutputv") , "Output Voltage" , "V" , $outputv , $needle2 , $calc)
			$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100 )
			$dial3 = DrawDial(320, GetOption("maxinputf") , GetOption("mininputf") , "Input Frequency" , "Hz" , $inputf , $needle3 , $calc )
			$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100 )
			$dial4 = DrawDial(480, 200 , 0 , "Battery Voltage" , "V" , $battv , $needle4 , $calc , 20 , 120)
			$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100 )
			$dial5 = DrawDial(320, 200 , 0 , "UPS Load" , "%" , $upsl , $needle5 , $calc , -1 , 80)
			$painting = 0
		EndIf
		if $haserror == 0 Then
			Update()
			AdlibEnable("Update",1000)
		EndIf		
	EndIf
	if $nMsg[0] == $aboutMenu Then
		aboutGui()
	EndIf
	
	if ($nMsg[0] == $listvarMenu) Then
		varlistGui()
	EndIf
	
	If $nMsg[0]== $reconnectMenu then
		AdlibDisable()
		ConnectServer() ;;aaaaa
		Opt("TCPTimeout",3000)
		GetUPSInfo()
		SetUPSInfo()
		AdlibEnable("Update",1000)
	EndIf
	
WEnd

EndFunc

func setTrayMode()
	
	$minimizetray = GetOption("minimizetray")
	if $minimizetray == 1 Then
		TraySetIcon(@ScriptDir & "\upsicon.ico")
		TraySetState (1)
		Opt("TrayMenuMode",1)
	Else
		TraySetState(2)
	EndIf

EndFunc

;HERE STARTS MAIN SCRIPT
TraySetState(2)

;AutoItSetOption ( "PixelCoordMode" , 0 )
$status = TCPStartup ( )
if $status ==false Then
	MsgBox(48,"Critical Error","Couldn't startup TCP")
	Exit
EndIf
Opt("GUIDataSeparatorChar", ".")
$status = InitOptionDATA()
if $status == -1 Then
	MsgBox(48,"Critical Error","Couldn't initialize Options")
	Exit
EndIf

ReadParams()
setTrayMode()

OpenMainWindow()
$status = ConnectServer()
Opt("TCPTimeout",3000)
GetUPSInfo()
SetUPSInfo()
Update()
GuiRegisterMsg(0x000F,"rePaint")
AdlibEnable("Update",GetOption("delay"))
mainLoop()
