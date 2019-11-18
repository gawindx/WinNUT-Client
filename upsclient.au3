#pragma compile(FileVersion, 1.7.2.2)
#pragma compile(Icon, .\images\upsicon.ico)
#pragma compile(Out, .\Build\upsclient.exe)
#pragma compile(Compression, 1)
#pragma compile(Comments, 'Windows NUT Client')
#pragma compile(FileDescription, Windows NUT Client. This is a NUT windows client for monitoring your ups hooked up to your favorite linux server.)
#pragma compile(LegalCopyright, Freeware)
#pragma compile(ProductName, WinNUT-Client)
#pragma compile(Compatibility, win7, win8, win81, win10)
;
#include-once
#include <GUIConstants.au3>
#include <Misc.au3>
#Include <Constants.au3>
#include <Array.au3>
#include <TrayConstants.au3>
#include <Timers.au3>
#include <File.au3>
#include "nutGlobal.au3"
#include "nutGui.au3"
#Include "nuti18n.au3"
#include "nutDraw.au3"
#include "nutColor.au3"
#include "nutOption.au3"
#include "nutNetwork.au3"
#Include "nutTreeView.au3"

If UBound(ProcessList(@ScriptName)) > 2 Then Exit

;This function repaints all needles when required and passes on control
;to internal AUTOIT repaint handler
;This is registered for WM_PAINT event
Func rePaint()
	repaintNeedle($needle6, $battCh, $dial6, 0, 100)
	repaintNeedle($needle4, $battVol, $dial4, getOption("minbattv"), getOption("maxbattv"))
	repaintNeedle($needle5, $upsLoad, $dial5, getOption("minupsl"), getOption("maxupsl"))
	repaintNeedle($needle1, $inputVol, $dial1, getOption("mininputv"), getOption("maxinputv"))
	repaintNeedle($needle2, $outputVol, $dial2, getOption("minoutputv"), getOption("maxoutputv"))
	repaintNeedle($needle3, $inputFreq, $dial3, getOption("mininputf"), getOption("maxinputf"))
	return $GUI_RUNDEFMSG
EndFunc

Func updateVarList()
	$selected = _GUICtrlTreeViewGetTree1($TreeView1, "." , 0)
	GuiCtrlSetData($varselected , $selected)
	$upsval = ""
	$upsdesc = ""
	$checkstatus1 = GetUPSVar(GetOption("upsname") , $selected , $upsval, __("Unknown"))
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
	AdlibUnregister("Update")
	$status1 = ListUPSVars(GetOption("upsname") , $varlist)
	$varlist = StringReplace($varlist , GetOption("upsname") , "")
	$vars = StringSplit($varlist , "VAR",1)
	AdlibRegister("Update",1000)
	$guilistvar = GUICreate(__("LIST UPS Variables"), 365, 331, 196, 108, -1 , -1 , $gui)
	GUISetIcon(@tempdir & "upsicon.ico")
	$TreeView1 = GUICtrlCreateTreeView(0, 8, 361, 169)

	$Group1 = GUICtrlCreateGroup(__("Item properties"), 0, 184, 361, 105, $BS_CENTER)
	$Label1 = GUICtrlCreateLabel(__("Name :"), 8, 200, 38, 17)
	$Label2 = GUICtrlCreateLabel(__("Value :"), 8, 232, 37, 17)
	$Label3 = GUICtrlCreateLabel(__("Description :"), 8, 264, 63, 17)
	$varselected = GUICtrlCreateLabel("", 50, 200, 291, 17, $SS_SUNKEN)
	$varvalue = GUICtrlCreateLabel("", 50, 232, 291, 17, $SS_SUNKEN)
	$vardesc = GUICtrlCreateLabel("", 72, 264, 283, 17, $SS_SUNKEN)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	$Reload_Btn = GUICtrlCreateButton(__("Reload"), 80, 296, 65, 25, 0)
	$Clear_Btn = GUICtrlCreateButton(__("Clear"), 200, 296, 65, 25, 0)
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
	_GUICtrlTreeView_Expand($TreeView1,0,false)

	AdlibUnregister("Update")
	AdlibRegister("updateVarList",500)
	While 1
		$nMsg = GUIGetMsg(1)
		Switch $nMsg[0]
			Case $GUI_EVENT_CLOSE
				AdlibUnregister("updateVarList")
				GuiSetState(@SW_ENABLE,$gui )
				GuiDelete($guilistvar)
				WinActivate($gui)
				AdlibRegister("Update",1000)
				return
			Case $Clear_Btn
				_GUICtrlTreeView_Expand($TreeView1,0,false)
			Case $Reload_Btn
				AdlibUnRegister("updateVarList")
				_GUICtrlTreeView_DeleteAll($TreeView1)
				for $i = 3 to $varcount
					if $i == $varcount Then
						ContinueLoop
					EndIf
					$templist = StringSplit($vars[$i],'"')
					$curpath = StringStripWS($templist[1],3)
					_addPath($TreeView1, $curpath )
				Next
				_SetIcons($TreeView1, 0)
				_GUICtrlTreeView_Expand($TreeView1,0,false)
				AdlibRegister("updateVarList",500)
		EndSwitch
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
	$status = GetUPSVar(GetOption("upsname") ,"ups.mfr" , $mfr, __("Unknown"))
	if $status = -1 then ;UPS name wrong or variable not supported or connection lost
		if $socket == 0 Then
			Return
		EndIf
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			$mfr = ""
			WriteLog(__("Disconnecting from server"))
			TCPSend($socket,"LOGOUT")
			TCPCloseSocket($socket)
			$socket = 0
			ResetGui()
			Return
		EndIf
	EndIf

	$status = GetUPSVar(GetOption("upsname") ,"ups.model" , $name, __("Unknown"))
	if $status = -1 then
		if $socket == 0 Then
			Return
		EndIf
		$name =""
	EndIf
	;trim $name
	$name = StringStripWS($name, $STR_STRIPLEADING + $STR_STRIPTRAILING)

	$status = GetUPSVar(GetOption("upsname"), "ups.serial", $serial, __("Unknown"))
	if $status = -1 then
		if $socket == 0 Then
			Return
		EndIf
		$serial =""
	EndIf

	$status = GetUPSVar(GetOption("upsname"), "ups.firmware", $firmware, __("Unknown"))
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

Func GetUPSData()
	;$status = 0
	$ups_name = GetOption("upsname")
	If $socket == 0 Then $status = -1
	If GetUPSVar($ups_name, "battery.charge", $battch, "255", 50) == -1 Then $status = -1
	If GetUPSVar($ups_name, "battery.voltage", $battVol, "12")  == -1 Then $status = -1
	If GetUPSVar($ups_name, "battery.runtime", $battruntime, "86400") == -1 Then $status = -1
	If GetUPSVar($ups_name, "battery.capacity", $batcapacity, "7") == -1 Then $status = -1
	If GetUPSVar($ups_name, "input.frequency", $inputFreq, "50") == -1 Then $status = -1
	If GetUPSVar($ups_name, "input.voltage", $inputVol, "220") == -1 Then $status = -1
	If GetUPSVar($ups_name, "output.voltage", $outputVol, $inputVol)  == -1 Then $status = -1
	If GetUPSVar($ups_name, "ups.load", $upsLoad, "100") == -1 Then $status = -1
	If GetUPSVar($ups_name, "ups.status", $upsstatus, "OL") == -1 Then $status = -1
	If GetUPSVar($ups_name, "ups.realpower.nominal", $upsoutpower, __("Unknown")) == -1 Then 
		$status = -1
	ElseIf $upsoutpower == __("Unknown") Then
		Local $inputcurrent
		If GetUPSVar($ups_name, "ups.current.nominal", $inputcurrent, 1) == -1 Then
			$status = -1
		Else 
			;Because this inverter does not provide information on its power,
			;we will determine it according to the elements and defaults at our disposal
			;For this, we will consider an input and output yield of 70% (rather low yield) and a power factor of 0.6
			;(((($inputVol * $inputcurrent)/($yield_In*$yield_Out))/$PF)*($upsLoad)
			;In this way, the power obtained should be lower than the real characteristic of the UPS and there will be no risk of late shutdown
			;$upsoutpower = (((($inputVol * $inputcurrent)/(0.7*0.7))/0.6)*($upsLoad/100))
			$upsoutpower = ($inputVol * 0.95 *$inputcurrent)
		EndIf
	Else
		$upsoutpower = $upsoutpower / $upsPF
	EndIf

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
	UpdateValue($needle4, 0, $battv,$dial4, getOption("minbattv"), getOption("maxbattv"))
	UpdateValue($needle5, 0, $upsl, $dial5, getOption("minupsl"), getOption("maxupsl"))
	UpdateValue($needle6, 0, $upsch, $dial6, 0, 100)
	UpdateValue($needle1, 0, $inputv, $dial1, getOption("mininputv"), getOption("maxinputv"))
	UpdateValue($needle2, 0, $outputv,$dial2, getOption("minoutputv"), getOption("maxoutputv"))
	UpdateValue($needle3, 0, $inputf, $dial3, getOption("mininputf"), getOption("maxinputf"))
	GuiCtrlSetBkColor($upsonline, $gray)
	GuiCtrlSetBkColor($upsonbatt, $gray)
	GUICtrlSetBkColor($upsoverload, $gray)
	GUICtrlSetBkColor($upslowbatt, $gray)
	if ($socket <> 0 ) Then
		SetUPSInfo()
	EndIf
	rePaint()
EndFunc

Func Update()
	GetUPSData()
	if $socket == 0 and $LastSocket <> 0 Then
		;Connection is lost from last Update Loop
		$ReconnectTry = 0
		ResetGui()
		If GetOption("autoreconnect") == 1 Then
			ReconnectNut()
			AdlibRegister("ReconnectNut", 30000)
		EndIf
		AdlibUnregister("Update")
		Return
	ElseIf $socket == 0 then ; connection lost so throw all needles to left
		ResetGui()
		AdlibUnregister("Update")
		Return
	Else
		$LastSocket = $socket
		$ReconnectTry = 0
	EndIf
	If $upsstatus == "OL" Then
		SetColor($green , $wPanel , $upsonline )
		SetColor(0xffffff , $wPanel , $upsonbatt )
	Else
		SetColor($yellow , $wPanel , $upsonbatt )
		SetColor(0xffffff , $wPanel , $upsonline )
	EndIf
	Local $PowerDivider = 0.5
	If $upsLoad > 100 Then
		SetColor($red , $wPanel , $upsoverload )
	Else
		SetColor(0xffffff , $wPanel , $upsoverload )
		If $upsLoad > 75 Then
			$PowerDivider = 0.4
		ElseIf $upsLoad > 50 Then
			$PowerDivider = 0.3
		EndIf
	EndIf
	;In case of that your inverter does not provide the State of Charge, he will be estimated.
	;The calculation method used is linear and considers that a fully charged 12V battery has
	;a voltage of 13.6V while the voltage of a fully discharged battery is only 11.6V .
	;In this way each percentage of Charge level corresponds to 0.02V.
	;This method is not accurate but offers a consistent approximation.
	If ($battCh = 255) Then
		Local $nbattery = Floor($battVol / 12)
		$battCh = Floor(($battVol - (11.6 * $nbattery)) / (0.02 * $nbattery))
	EndIF
	;In case your inverter does not provide a consistent value for its runtime, 
	;he will also be determined by the calculation
	;The calculation takes into account the capacity of the batteries, 
	; the instantaneous charge, the battery voltage, their state of charge,
	; the Power Factor as well as a coefficient allowing to take into account
	;a large instantaneous charge (this limits the runtime ).
	If ($battruntime >= 86400 ) Then
		Local $BattInstantCurrent = ($upsoutpower * $upsLoad) / ( $battVol* 100)
		$battruntime = Floor(($batcapacity * $upsPF * $battCh * (1-$PowerDivider) * 3600) / ($BattInstantCurrent * 100))
	EndIf
	if $battCh < 40 Then
		SetColor($red , $wPanel , $upslowbatt )
	Else
		SetColor(0xffffff , $wPanel , $upslowbatt )
	EndIf
	$battrtimeStr = TimeToStr($battruntime) 
	GuiCtrlSetData($remainTimeLabel,$battrtimeStr)
	UpdateValue($needle1, $inputVol, $inputv, $dial1,getOption("mininputv"), getOption("maxinputv"))
	UpdateValue($needle2, $outputVol, $outputv, $dial2, getOption("minoutputv"), getOption("maxoutputv"))
	UpdateValue($needle3, $inputFreq, $inputf, $dial3, getOption("mininputf"), getOption("maxinputf"))
	UpdateValue($needle4, $battVol, $battv, $dial4, getOption("minbattv"), getOption("maxbattv"))
	UpdateValue($needle5, $upsLoad, $upsl, $dial5, getOption("minupsl"), getOption("maxupsl"))
	UpdateValue($needle6, $battCh, $upsch, $dial6, 0, 100)
	rePaint()
	;if connection to UPS is in fact alive and charge below shutdown setting and ups is not online
	;add different from status 0 when UPS not connected but NUT is running
	if (IsShutdownCondition()) Then
		$InstantShutdown = GetOption("InstantShutdown")
		If $InstantShutdown = 1 Then
			Shutdown(17)
		Else
			ShutdownGui()
		EndIf
	EndIf
EndFunc

Func SetTrayIconText()
	$trayStatus  = ""
	If $socket > 0 Then
		if $battCh < 40 Then
			$trayStatus  = $trayStatus & @LF &  __("Low Battery")
		Else
			$trayStatus  = $trayStatus & @LF &  __("Battery OK")
		EndIf
		If $upsstatus == "OL" Then
			$trayStatus  = $trayStatus & @LF & __("UPS On Line")
		Else
			$trayStatus  = $trayStatus & @LF & __("UPS On Battery") & "(" & $battCh & "%)"
		EndIf
	ElseIf $ReconnectTry <> 0 Then
		$trayStatus = __("Connection lost") & @LF & StringFormat(__("%d attempts remaining"), (30 - $ReconnectTry))
	Else
		$trayStatus = __("Not Connected")
	EndIf
	TraySetToolTip($ProgramDesc & " - " & $ProgramVersion & $trayStatus )
EndFunc

Func ReconnectNut()
	local $NewSocket = ConnectServer()
	Opt("TCPTimeout",3000)
	$ReconnectTry = $ReconnectTry + 1
	If $ReconnectTry > 29 Then
		AdlibUnregister("ReconnectNut")
	ElseIf $NewSocket >= 0  Then
		GetUPSInfo()
		SetUPSInfo()
		Update()
		AdlibRegister("Update",1000)
		AdlibUnregister("ReconnectNut")
	EndIf
EndFunc

Func DrawDial($left, $top, $basescale, $title, $units, ByRef $value, ByRef $needle, $scale = 1, $leftG = 20, $rightG = 70)
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
					GuiCtrlCreateLabel($scalevalue, $x*1.2 + 13, 25, 20, 10)
				Case 10 to 99
					GuiCtrlCreateLabel($scalevalue, $x*1.2 + 10, 25, 20, 10)
				Case 100 to 1000
					GuiCtrlCreateLabel($scalevalue, $x*1.2 + 7, 25, 20, 10)
			EndSwitch
			GUICtrlSetFont(-1,7)
		Else
			GUICtrlCreateLabel("", $x*1.2+15, 15, 1, 5, $SS_BLACKRECT)
			GuiCtrlSetState(-1,$GUI_DISABLE)
			$test = GUICtrlCreateLabel("", $x*1.2+16, 15, 11, 5, 0)
			;GuiCtrlSetState(-1,$GUI_DISABLE)
			if $x < $rightG and $x > $leftG then
				GUICtrlSetBkColor($test , 0x00ff00)
			Else
				GUICtrlSetBkColor($test , 0xff0000)
			EndIf
		EndIf
	Next
	if $units =="%" then
		$value = GUICtrlCreateLabel(0, 10, 100, 40, 15, $SS_LEFT)
	Else
		$value = GUICtrlCreateLabel(220, 10, 100, 40, 15, $SS_LEFT)
	EndIf
	$label2 = GUICtrlCreateLabel($units, 116, 100, 25, 15,$SS_RIGHT)
	$needle = GUICtrlCreateGraphic(10, 35, 120, 60)
	;GUICtrlSetBkColor(-1,$aqua)
	;$fill = GuiCtrlCreateGraphic(0 , 0 , 150 , 120)
	If BitAND(WinGetState($gui), $WIN_STATE_MINIMIZED) Then
		GuiSetState(@SW_HIDE,$group)
	Else
		GuiSetState(@SW_SHOW,$group)
	EndIf
	;GuiCtrlSetBkColor(-1,0x00ffff)
	$result  = $group
	return $group
EndFunc

Func ShutdownGui_Event($hWnd, $Msg, $wParam, $lParam)
	$nNotifyCode = BitShift($wParam, 16)
	$nID = BitAnd($wParam, 0x0000FFFF)
	$hCtrl = $lParam
	writelog("notify " & $nNotifyCode)
	If $nID = $guishutdown Then
		Switch $nNotifyCode
			Case $LBN_DBLCLK
				writelog("bidule")
		EndSwitch
	EndIf
		If $nID = $Grace_btn Then
		Switch $nNotifyCode
			Case $LBN_DBLCLK
				writelog("grace")
		EndSwitch
	EndIf
		If $nID = $Shutdown_btn Then
		Switch $nNotifyCode
			Case $LBN_DBLCLK
				writelog("shutdown")
		EndSwitch
	EndIf
EndFunc

Func setTrayMode()
	$minimizetray = GetOption("minimizetray")
	if $minimizetray == 1 Then
		TraySetIcon(@tempdir & "upsicon.ico")
		TraySetState($TRAY_ICONSTATE_SHOW)
		Opt("TrayAutoPause", 0) ; Le script n'est pas mis en pause lors de la sélection de l'icône de la zone de notification.
		Opt("TrayMenuMode", 3) ; Les items ne sont pas cochés lorsqu'ils sont sélectionnés.
		TraySetClick(8)
		TraySetToolTip($ProgramDesc & " - " & $ProgramVersion )
	Else
		TraySetState($TRAY_ICONSTATE_HIDE)
	EndIf
EndFunc

Func mainLoop()
	$minimizetray = GetOption("minimizetray")
	While 1
		if ($minimizetray == 1) Then
			$tMsg = TrayGetMsg()
			Switch $tMsg
				Case $TRAY_EVENT_PRIMARYDOUBLE
					GuiSetState(@SW_SHOW, $gui)
					GuiSetState(@SW_RESTORE ,$gui)
					TraySetState($TRAY_ICONSTATE_HIDE)
				Case $idTrayExit
					TCPSend($socket,"LOGOUT")
					TCPCloseSocket($socket)
					TCPShutdown()
					Exit
				Case $idTrayAbout
					aboutGui()
				Case $idTrayPref
					AdlibUnregister("Update")
					$changedprefs = prefGui()
					if $changedprefs == 1 Then
						$painting = 1
						GuiDelete($dial1)
						GuiDelete($dial2)
						GuiDelete($dial3)
						GuiDelete($dial5)
						GuiDelete($dial4)
						DrawError(160, 70, "Delete")
						$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100)
						$dial1 = DrawDial(160, 70, GetOption("mininputv"), "Input Voltage", "V", $inputv, $needle1, $calc)
						$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100)
						$dial2 = DrawDial(480, 70, GetOption("minoutputv"), "Output Voltage", "V", $outputv, $needle2, $calc)
						$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100)
						$dial3 = DrawDial(320, GetOption("maxinputf"), GetOption("mininputf"), "Input Frequency", "Hz", $inputf, $needle3, $calc )
						$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100)
						$dial4 = DrawDial(480, 200, GetOption("minbattv"), "Battery Voltage", "V", $battv, $needle4, $calc, 20, 120)
						$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100)
						$dial5 = DrawDial(320, 200, 0, "UPS Load", "%", $upsl, $needle5, $calc, -1, 80)
						$painting = 0
					EndIf
					if $haserror == 0 Then
						Update()
						AdlibRegister("Update",1000)
					EndIf
			EndSwitch
		EndIf
		$nMsg = GUIGetMsg(1)
		if GetOption("closetotray") == 0 Then
			if ($nMsg[0] == $GUI_EVENT_CLOSE and $nMsg[1]==$gui)  or $nMsg[0] == $exitMenu or $nMsg[0] == $exitb then
				TCPSend($socket,"LOGOUT")
				TCPCloseSocket($socket)
				TCPShutdown()
				Exit
			EndIf
		Else
			if $nMsg[0] == $exitMenu or $nMsg[0] == $exitb then
				TCPSend($socket,"LOGOUT")
				TCPCloseSocket($socket)
				TCPShutdown()
				Exit
			EndIf
			if ($nMsg[0] == $GUI_EVENT_CLOSE and $nMsg[1]==$gui) Then
				GuiSetState(@SW_HIDE , $gui)
				TraySetState($TRAY_ICONSTATE_SHOW)
			EndIf
		EndIf
		if ($nMsg[0] == $GUI_EVENT_MINIMIZE and $nMsg[1]==$gui and $minimizetray ==1) Then;minimize to tray
			GuiSetState(@SW_HIDE , $gui)
			TraySetState($TRAY_ICONSTATE_SHOW)
		EndIf
		if $nMsg[0] == $toolb or $nMsg[0] == $settingssubMenu Then
			AdlibUnregister("Update")
			$changedprefs = prefGui()
			if $changedprefs == 1 Then
				$painting = 1
				GuiDelete($dial1)
				GuiDelete($dial2)
				GuiDelete($dial3)
				GuiDelete($dial5)
				GuiDelete($dial4)
				DrawError(160, 70, "Delete")
				$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100)
				$dial1 = DrawDial(160, 70, GetOption("mininputv"), "Input Voltage", "V", $inputv, $needle1, $calc)
				$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100)
				$dial2 = DrawDial(480, 70, GetOption("minoutputv"), "Output Voltage", "V", $outputv, $needle2, $calc)
				$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100)
				$dial3 = DrawDial(320, GetOption("maxinputf"), GetOption("mininputf"), "Input Frequency", "Hz", $inputf, $needle3, $calc )
				$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100)
				$dial4 = DrawDial(480, 200, GetOption("minbattv"), "Battery Voltage", "V", $battv, $needle4, $calc, 20, 120)
				$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100)
				$dial5 = DrawDial(320, 200, 0, "UPS Load", "%", $upsl, $needle5, $calc, -1, 80)
				$painting = 0
			EndIf
			if $haserror == 0 Then
				Update()
				AdlibRegister("Update",1000)
			EndIf
		EndIf
		if $nMsg[0] == $aboutMenu Then
			aboutGui()
		EndIf
		if ($nMsg[0] == $listvarMenu) Then
			varlistGui()
		EndIf
		If $nMsg[0]== $reconnectMenu then
			AdlibUnregister("Update")
			$socket = ConnectServer() ;;aaaaa
			Opt("TCPTimeout",3000)
			GetUPSInfo()
			SetUPSInfo()
			AdlibRegister("Update",1000)
		EndIf
		For $vKey In $MenuLangListhwd
			If $nMsg[0] == $MenuLangListhwd.Item($vKey) Then
				GUICtrlSetState($MenuLangListhwd.Item($vKey), $GUI_CHECKED)
				SetOption("language", $vKey, "string")
				MsgBox($MB_SYSTEMMODAL, "",  __("The language change will be effective after restarting WinNut"))
				WriteParams()
				For $vxKey In $MenuLangListhwd
					If $vxKey == $vKey Then
						GUICtrlSetState($MenuLangListhwd.Item($vxKey), $GUI_CHECKED)
					Else
						GUICtrlSetState($MenuLangListhwd.Item($vxKey), $GUI_UNCHECKED)
					EndIf
				Next
				;_i18n_SetLanguage($vKey)
				ExitLoop
			EndIf
		Next

	WEnd
EndFunc

Func WinNut_Init()
	;Install all needed Files
	;icon
	Fileinstall(".\images\ups.jpg", @tempdir & "ups.jpg",1)
	Fileinstall(".\images\upsicon.ico", @tempdir & "upsicon.ico",1)

	If Not FileExists(@ScriptDir & "\Language") Then
		DirCreate(@ScriptDir & "\Language")
	EndIf

	;Language
	; function to auto include all language file
		_ListFileInstallFolder(".\Language", "\Language", 0, "*.lng", "include", True)
		;Now file is generated so include it
		#Include "include.au3"

	;Get Script Version
	$ProgramVersion = _GetScriptVersion()

	;HERE STARTS MAIN SCRIPT
	$status = TCPStartup()
	if $status == false Then
		MsgBox(48,"Critical Error","Couldn't startup TCP")
		Exit
	EndIf
	Opt("GUIDataSeparatorChar", ".")

	;Initialize all Option Data
	InitOptionDATA()
	if $status == -1 Then
		MsgBox(48, "Critical Error", "Couldn't initialize Options")
		Exit
	EndIf

	;Determine if running as exe or script
	If @Compiled Then $runasexe = True

	;load/create ini file
	ReadParams()

	;Define default language and language file directory
	_i18n_SetLangBase(@ScriptDir & "\Language")
	Local $sDefLang = GetOption("defaultlang")
	If $sDefLang == -1 Then
		$sDefLang = 'en-US'
		SetOption("defaultlang", $sDefLang, "string")
	EndIf
	_i18n_SetDefault($sDefLang)
	Local $sLanguage = GetOption("language")
	If $sLanguage == -1 Then
		$sLanguage = 'system'
		SetOption("language", $sLanguage, "string")
	EndIf
	_i18n_SetLanguage($sLanguage)
	
	;Create and iitialize Systray Icon
	TraySetState($TRAY_ICONSTATE_HIDE)
	setTrayMode()
	$idTrayPref = TrayCreateItem(__("Preferences"))
	TrayCreateItem("")
	$idTrayAbout = TrayCreateItem(__("About"))
	TrayCreateItem("")
	$idTrayExit = TrayCreateItem(__("Exit"))
	
	OpenMainWindow()
	if ( GetOption("minimizeonstart") == 1 and GetOption("minimizetray") == 1 ) Then
		GuiSetState(@SW_HIDE, $gui)
		TraySetState($TRAY_ICONSTATE_SHOW)
	Else
		GuiSetState(@SW_SHOW, $gui)
		TraySetState($TRAY_ICONSTATE_HIDE)
	EndIf

	;Initilize connexion to Nut/Ups
	$status = ConnectServer()
	Opt("TCPTimeout",3000)
	GetUPSInfo()
	SetUPSInfo()
	GetUPSData()
	Update()
	GuiRegisterMsg(0x000F,"rePaint")
	AdlibRegister("GetUPSData",GetOption("delay"))
	AdlibRegister("Update",1000)
	AdlibRegister("SetTrayIconText",1000)
EndFunc

WinNut_Init()
mainLoop()