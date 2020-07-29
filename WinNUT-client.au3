#pragma compile(FileVersion, 1.8.0.3)
#pragma compile(Icon, .\images\Ico\WinNut.ico)
#pragma compile(Out, .\Build\WinNUT-client.exe)
#pragma compile(Compression, 9)
#pragma compile(UPX, False)
#pragma compile(Comments, 'Windows NUT Client')
#pragma compile(FileDescription, Windows NUT Client. This is a NUT windows client for monitoring your ups hooked up to your favorite linux server.)
#pragma compile(LegalCopyright, Freeware)
#pragma compile(ProductName, WinNUT-Client)
#pragma compile(Compatibility, win7, win8, win81, win10)

#RequireAdmin
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
#include "nutOption.au3"
#include "nutNetwork.au3"
#Include "nutTreeView.au3"

If UBound(ProcessList(@ScriptName)) > 2 Then Exit

Opt("WinSearchChildren", 1)

;This function repaints all needles when required and passes on control
;to internal AUTOIT repaint handler
;This is registered for WM_PAINT event
Func rePaint()
	;Logging removed due to too many writes
	If $gui <> 0 Then
		repaintNeedle($needle1, $inputVol, $dial1, getOption("mininputv"), getOption("maxinputv"))
		repaintNeedle($needle2, $outputVol, $dial2, getOption("minoutputv"), getOption("maxoutputv"))
		repaintNeedle($needle3, $inputFreq, $dial3, getOption("mininputf"), getOption("maxinputf"))
		repaintNeedle($needle4, $battVol, $dial4, getOption("minbattv"), getOption("maxbattv"))
		repaintNeedle($needle5, $upsLoad, $dial5, getOption("minupsl"), getOption("maxupsl"))
		repaintNeedle($needle6, $battCh, $dial6, 0, 100)
	EndIf
	return $GUI_RUNDEFMSG
EndFunc ;==> rePaint

Func updateVarList()
	WriteLog("Enter updateVarList Function", $LOG2FILE, $DBG_DEBUG)
	$selected = _GUICtrlTreeViewGetTree1($TreeView1, ".", 0)
	GuiCtrlSetData($varselected, $selected)
	$upsval = ""
	$upsdesc = ""
	$checkstatus1 = GetUPSVar(GetOption("upsname"), $selected, $upsval, __("Unknown"))
	$checkstatus2 = GetUPSDescVar(GetOption("upsname"), $selected, $upsdesc)
	If $checkstatus1 == -1 or $checkstatus2 == -1 Then
		$upsval = ""
		$upsdesc = ""
	EndIf
	If GuiCtrlRead($varvalue ) <> $upsval Then
		GuiCtrlSetData($varvalue, $upsval)
	EndIf
	If GuiCtrlRead($vardesc ) <> $upsdesc Then
		GuiCtrlSetData($vardesc, $upsdesc)
	EndIf
EndFunc ;==> updateVarList

Func varlistGui()
	WriteLog("Enter varlistGui Function", $LOG2FILE, $DBG_DEBUG)
	$varlist = ""
	$templist = ""
	AdlibUnregister("Update")
	WriteLog("Update Fonction UnRegister", $LOG2FILE, $DBG_DEBUG)
	$status1 = ListUPSVars(GetOption("upsname"), $varlist)
	$varlist = StringReplace($varlist, GetOption("upsname"), "")
	$vars = StringSplit($varlist, "VAR", 1)
	AdlibRegister("Update",1000)
	WriteLog("Update Fonction Register", $LOG2FILE, $DBG_DEBUG)
	$guilistvar = GUICreate(__("LIST UPS Variables"), 365, 331, 196, 108, -1 , -1 , $gui)

	SetIconGuiTray($guilistvar)
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
	For $i = 3 To $varcount
		If $i == $varcount Then
			ContinueLoop
		EndIf
		$templist = StringSplit($vars[$i], '"')
		$curpath = StringStripWS($templist[1], 3)
		_addPath($TreeView1, $curpath)
	Next
	_SetIcons($TreeView1, 0)
	_GUICtrlTreeView_Expand($TreeView1, 0, false)

	AdlibUnregister("Update")
	WriteLog("Update Fonction UnRegister", $LOG2FILE, $DBG_DEBUG)
	AdlibRegister("updateVarList", 500)
	WriteLog("updateVarList Fonction Register", $LOG2FILE, $DBG_DEBUG)
	While 1
		$nMsg = GUIGetMsg(1)
		Switch $nMsg[0]
			Case $GUI_EVENT_CLOSE
				WriteLog("varlistGui Event Close", $LOG2FILE, $DBG_DEBUG)
				AdlibUnregister("updateVarList")
				WriteLog("updateVarList Fonction UnRegister", $LOG2FILE, $DBG_DEBUG)
				GuiSetState(@SW_ENABLE, $gui)
				GuiDelete($guilistvar)
				WriteLog("Delete guilistvar", $LOG2FILE, $DBG_DEBUG)
				WinActivate($gui)
				WriteLog("Activate WinNut Gui", $LOG2FILE, $DBG_DEBUG)
				AdlibRegister("Update", $RefreshInterval)
				WriteLog("Update Fonction Register", $LOG2FILE, $DBG_DEBUG)
				return
			Case $Clear_Btn
				WriteLog("varlistGui Event Clear", $LOG2FILE, $DBG_DEBUG)
				_GUICtrlTreeView_Expand($TreeView1, 0, false)
			Case $Reload_Btn
				WriteLog("varlistGui Event Reload", $LOG2FILE, $DBG_DEBUG)
				AdlibUnRegister("updateVarList")
				WriteLog("updateVarList Fonction UnRegister", $LOG2FILE, $DBG_DEBUG)
				_GUICtrlTreeView_DeleteAll($TreeView1)
				For $i = 3 To $varcount
					If $i == $varcount Then
						ContinueLoop
					EndIf
					$templist = StringSplit($vars[$i], '"')
					$curpath = StringStripWS($templist[1], 3)
					_addPath($TreeView1, $curpath)
				Next
				_SetIcons($TreeView1, 0)
				_GUICtrlTreeView_Expand($TreeView1, 0, false)
				AdlibRegister("updateVarList", 500)
				WriteLog("updateVarList Fonction Register", $LOG2FILE, $DBG_DEBUG)
		EndSwitch
	WEnd
EndFunc ;==> varlistGui

Func GetUPSInfo()
	WriteLog("Enter GetUPSInfo Function", $LOG2FILE, $DBG_DEBUG)
	Local $status = 0
	$mfr = ""
	$name = ""
	$serial = ""
	$firmware = ""
	If $socket == 0 Then ; not connected to server/connection lost
		WriteLog("Not Connected To Nut server", $LOG2FILE, $DBG_ERROR)
		Return
	EndIf
	$status = GetUPSVar(GetOption("upsname"), "ups.mfr", $mfr, __("Unknown"))
	If $status = -1 then ;UPS name wrong or variable not supported or connection lost
		WriteLog("UPS name wrong or variable not supported or connection lost", $LOG2FILE, $DBG_WARNING)
		if $socket == 0 Then
			WriteLog("Disconnecting from server", $LOG2FILE, $DBG_ERROR)
			Return
		EndIf
		If StringInStr($errorstring, "UNKNOWN-UPS") <> 0 Then
			$mfr = ""
			WriteLog("Disconnecting from server", $LOG2FILE, $DBG_ERROR)
			TCPSend($socket,"LOGOUT")
			TCPCloseSocket($socket)
			DeletePortProxy()
			$socket = 0
			ResetGui()
			Return
		EndIf
	EndIf

	$status = GetUPSVar(GetOption("upsname"), "ups.model", $name, __("Unknown"))
	If $status = -1 Then
		WriteLog("GetUPSVar ups->model Return -1", $LOG2FILE, $DBG_WARNING)
		If $socket == 0 Then
			WriteLog("Disconnected from server", $LOG2FILE, $DBG_ERROR)
			Return
		EndIf
		$name = ""
	EndIf
	;trim $name
	$name = StringStripWS($name, $STR_STRIPLEADING + $STR_STRIPTRAILING)

	$status = GetUPSVar(GetOption("upsname"), "ups.serial", $serial, __("Unknown"))
	WriteLog("GetUPSVar ups->serial Return -1", $LOG2FILE, $DBG_WARNING)
	If $status = -1 Then
		if $socket == 0 Then
			WriteLog("Disconnected from server", $LOG2FILE, $DBG_ERROR)
			Return
		EndIf
		$serial = ""
	EndIf

	$status = GetUPSVar(GetOption("upsname"), "ups.firmware", $firmware, __("Unknown"))
	if $status = -1 then
		WriteLog("GetUPSVar ups->firmware Return -1", $LOG2FILE, $DBG_WARNING)
		if $socket == 0 Then
			WriteLog("Disconnected from server", $LOG2FILE, $DBG_ERROR)
			Return
		EndIf
		$firmware = ""
	EndIf
EndFunc ;==> GetUPSInfo

Func SetUPSInfo()
	WriteLog("Enter SetUPSInfo Function", $LOG2FILE, $DBG_DEBUG)
	If $socket == 0 Then ;if not connected or connection lost
		$mfr = ""
		$name = ""
		$serial = ""
		$firmware = ""
	EndIf
	Local $arrvalue[4] = [$mfr, $name, $serial, $firmware]
	Local $arr[2] = ["GuiCtrlSetData : mfr->%s, name->%s, serial->%s, firmware->%s", $arrvalue]
	WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	GuiCtrlSetData($upsmfr, $mfr)
	GuiCtrlSetData($upsmodel, $name)
	GuiCtrlSetData($upsserial, $serial)
	GuiCtrlSetData($upsfirmware, $firmware)
EndFunc ;==> SetUPSInfo

Func GetUPSData()
	WriteLog("Enter GetUPSData Function", $LOG2FILE, $DBG_DEBUG)
	$ups_name = GetOption("upsname")
	If $socket == 0 Then $status = -1
	If GetUPSVar($ups_name, "battery.charge", $battch, "255", 50) == -1 Then 
		$status = -1
		WriteLog("GetUPSData battery->charge Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData battery->charge Return %s", $battch]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "battery.voltage", $battVol, "12")  == -1 Then 
		$status = -1
		WriteLog("GetUPSData battery->voltage Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData battery->voltage Return %s", $battVol]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "battery.runtime", $battruntime, "86400") == -1 Then 
		$status = -1
		WriteLog("GetUPSData battery->runtime Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData battery->runtime Return %s", $battruntime]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "battery.capacity", $batcapacity, "7") == -1 Then 
		$status = -1
		WriteLog("GetUPSData battery->capacity Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData battery->capacity Return %s", $batcapacity]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "input.frequency", $inputFreq) == -1 Then
		WriteLog("GetUPSData input->frequency Return -1", $LOG2FILE, $DBG_DEBUG)
		If $socket <> 0 Then
			Local $arr[2] = ["Nut Server connection Is Ok \n Set input->frequency with Value %s", GetOption("frequencysupply")]
			WriteLog($arr, $LOG2FILE, $DBG_WARNING)
			$inputFreq = GetOption("frequencysupply")
			$mininputf = $inputFreq - 10
			$maxinputf = $inputFreq + 10
		Else
			WriteLog("Nut Server connection Is Not Ok \n Set input->frequency with Value 0", $LOG2FILE, $DBG_ERROR)
			$inputFreq = 0
			Local $tmpinputFreq = GetOption("frequencysupply")
			$mininputf = $tmpinputFreq - 10
			$maxinputf = $tmpinputFreq + 10
		EndIf
	Else
		Local $arr[2] = ["GetUPSData input->frequency Return %s", $inputFreq]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
		$halfinputFreq = $inputFreq / 2
		If (($halfinputFreq >= 22.5) And ($halfinputFreq <= 27.5)) Then
			If (GetOption("frequencysupply") <> 50 ) Then
				SetOption("frequencysupply", 50, "number")
				WriteParams()
				WriteLog("Param frequencysupply fixed at 50Hz", $LOG2FILE, $DBG_DEBUG)
			EndIf
		ElseIf (($halfinputFreq >= 27.6) And ($halfinputFreq <= 32.5)) Then
			If (GetOption("frequencysupply") <> 60 ) Then
				SetOption("frequencysupply", 60, "number")
				WriteParams()
				WriteLog("Param frequencysupply fixed at 60Hz", $LOG2FILE, $DBG_DEBUG)
			EndIf
		EndIf
	EndIf
	Local $Fallback_InputVol
	If GetOption("mininputv") > 200 And GetOption("maxinputv") < 300 Then
		$Fallback_InputVol = "230"
	Else
		$Fallback_InputVol = "110"
	EndIf
	If GetUPSVar($ups_name, "input.voltage", $inputVol, $Fallback_InputVol) == -1 Then
		$status = -1
		WriteLog("GetUPSData input->voltage Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData input->voltage Return %s", $inputVol]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "output.voltage", $outputVol, $inputVol)  == -1 Then
		$status = -1
		WriteLog("GetUPSData output->voltage Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData output->voltage Return %s", $outputVol]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "ups.load", $upsLoad, "100") == -1 Then
		$status = -1
		WriteLog("GetUPSData ups->load Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData ups->load Return %s", $upsLoad]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "ups.status", $upsstatus, "OL") == -1 Then
		$status = -1
		WriteLog("GetUPSData ups->status Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData ups->status Return %s", $upsstatus]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	If GetUPSVar($ups_name, "ups.realpower.nominal", $upsoutpower, __("Unknown")) == -1 Then
		$status = -1
		WriteLog("GetUPSData ups->realpower->nominal Return -1", $LOG2FILE, $DBG_DEBUG)
	Else
		Local $arr[2] = ["GetUPSData ups->realpower->nominal Return %s", $upsoutpower]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
		If $upsoutpower == __("Unknown") Then
			Local $inputcurrent
			If GetUPSVar($ups_name, "ups.current.nominal", $inputcurrent, 1) == -1 Then
				$status = -1
				WriteLog("GetUPSData ups->current->nominal Return -1", $LOG2FILE, $DBG_DEBUG)
			Else
				#comments-start
					Because this inverter does not provide information on its power,
					we will determine it according to the elements and defaults at our disposal
					For this, we will consider an input and output yield of 70% (rather low yield) and a power factor of 0.6
					(((($inputVol * $inputcurrent)/($yield_In*$yield_Out))/$PF)*($upsLoad)
					In this way, the power obtained should be lower than the real characteristic of the UPS and there will be no risk of late shutdown
					$upsoutpower = (((($inputVol * $inputcurrent)/(0.7*0.7))/0.6)*($upsLoad/100))
				#comments-end
				Local $arr[2] = ["GetUPSData ups->current->nominal Return %s", $inputcurrent]
				WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
				$upsoutpower = ($inputVol * 0.95 * $inputcurrent)
				Local $arr[2] = ["ups->realpower->nominal estimed at %s", $upsoutpower]
				WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
			EndIf
		Else
			$upsoutpower = (($upsoutpower / $upsPF) * $upsLoad ) / 100
			Local $arr[2] = ["ups->realpower->nominal <> Unknown \n instant upsoutpower determined at %s", $upsoutpower]
			WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
		EndIf
	EndIf
EndFunc ;==> GetUPSData

Func UpdateValue(ByRef $needle, $value, $label, $whandle, $min = 170, $max = 270, $force = 0, $Text_And_Graph = True)
	WriteLog("Enter UpdateValue Function", $LOG2FILE, $DBG_DEBUG)
	$oldval = Round(GuiCtrlRead($label))
	If $oldval < $min Then
		$oldval = $min
	EndIf
	If $oldval > $max Then
		$oldval = $max
	EndIf
	If $oldval == Round($value) and $force == 0 Then
		Return
	EndIf
	GuiCtrlSetData($label, $value)
	Local $arrvalue[2] = [ControlGetText($whandle,'', $whandle), $value]
	Local $arr[2] = ["GuiCtrlSetData : %s->%s", $arrvalue]
	WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	If $Text_And_Graph Then
		$value = Round($value)
		If $value < $min Then
			$value = $min
		EndIf
		If $value > $max Then
			$value = $max
		EndIf
		$oldneedle = ($oldval - $min) / (($max - $min ) / 100)
		if $oldneedle > 0 or $oldneedle == 0 then
			DrawNeedle(15 + $oldneedle, $clock_bkg_bgr, $whandle, $needle)
		EndIf
		$setneedle = ($value - $min) / (($max - $min ) / 100)
		DrawNeedle(15 + $setneedle, 0x0, $whandle, $needle)
	EndIf
EndFunc ;==> UpdateValue

Func ResetGui()
	WriteLog("Enter SetUPSInfo Function", $LOG2FILE, $DBG_DEBUG)
	if $socket == 0  Then
		$battVol = 0
		$battCh = 0
		$upsLoad = 0
		$inputVol = 0
		$outputVol = 0
		$inputFreq = 0
		WriteLog("Nut Server Not Connected. Set alla value to 0", $LOG2FILE, $DBG_WARNING)
	EndIf
	UpdateValue($needle1, 0, $inputv, $dial1, getOption("mininputv"), getOption("maxinputv"))
	UpdateValue($needle2, 0, $outputv, $dial2, getOption("minoutputv"), getOption("maxoutputv"))
	UpdateValue($needle3, 0, $inputf, $dial3, getOption("mininputf"), getOption("maxinputf"))
	UpdateValue($needle4, 0, $battv,$dial4, getOption("minbattv"), getOption("maxbattv"))
	UpdateValue($needle5, 0, $upsl, $dial5, getOption("minupsl"), getOption("maxupsl"))
	UpdateValue($needle5, 0, $realoutpower, $dial5, null, null, 0, False)
	UpdateValue($needle6, 0, $upsch, $dial6, 0, 100)
	GuiCtrlSetBkColor($upsonline, $gray)
	GuiCtrlSetBkColor($upsonbatt, $gray)
	GUICtrlSetBkColor($upsoverload, $gray)
	GUICtrlSetBkColor($upslowbatt, $gray)
	If ($socket <> 0 ) Then
		SetUPSInfo()
	EndIf
	rePaint()
EndFunc ;==> ResetGui

Func Update()
	WriteLog("Enter Update Function", $LOG2FILE, $DBG_DEBUG)
	GetUPSData()
	$ActualIcon_IDX = 0
	If $socket == 0 and $LastSocket <> 0 Then
		WriteLog("Connection is lost from last Update Loop", $LOG2FILE, $DBG_ERROR)
		$ReconnectTry = 0
		ResetGui()
		If GetOption("autoreconnect") == 0 Or $ReconnectTry >= $MaxReconnectTry Then
			$ActualIcon_IDX = $IDX_ICO_OFFLINE
		Else 
			$ActualIcon_IDX = $IDX_ICO_RETRY
		EndIf
		If GetOption("autoreconnect") == 1 Then
			ReconnectNut()
			AdlibRegister("ReconnectNut", $ReconnectDelay)
			WriteLog("ReconnectNut Fonction Registered", $LOG2FILE, $DBG_WARNING)
			$ActualIcon_IDX = $IDX_ICO_RETRY
		EndIf
		AdlibUnregister("Update")
		WriteLog("Update Fonction UnRegister", $LOG2FILE, $DBG_DEBUG)
		GUICtrlSetState($DisconnectMenu, $GUI_DISABLE)
		SetIconGuiTray()
		Return
	ElseIf $socket == 0 Then
		WriteLog("Connection is lost so throw all needles to left", $LOG2FILE, $DBG_WARNING)
		ResetGui()
		AdlibUnregister("Update")
		WriteLog("Update Fonction UnRegister", $LOG2FILE, $DBG_DEBUG)
		GUICtrlSetState($DisconnectMenu, $GUI_DISABLE)
		$ActualIcon_IDX = $IDX_ICO_OFFLINE
		SetIconGuiTray()
		Return
	Else
		$LastSocket = $socket
		$ReconnectTry = 0
		GUICtrlSetState($DisconnectMenu, $GUI_ENABLE)
	EndIf
	If $upsstatus == "OL" Then
		WriteLog("UPS Status Is OL", $LOG2FILE, $DBG_WARNING)
		SetColor($green, $wPanel, $upsonline)
		SetColor(0xffffff, $wPanel, $upsonbatt)
		$ActualIcon_IDX = BitOR($ActualIcon_IDX, $IDX_OL)
	Else
		WriteLog("UPS Status Is Not OL", $LOG2FILE, $DBG_WARNING)
		SetColor($yellow, $wPanel, $upsonbatt)
		SetColor(0xffffff, $wPanel, $upsonline)
	EndIf
	Local $PowerDivider = 0.5
	If $upsLoad > 100 Then
		SetColor($red, $wPanel, $upsoverload)
		WriteLog("UPS Is OverLoad", $LOG2FILE, $DBG_WARNING)
	Else
		SetColor(0xffffff, $wPanel, $upsoverload)
		Switch $upsLoad
			Case 76 To 100
				$PowerDivider = 0.4
			Case 51 To 75
				$PowerDivider = 0.3
		EndSwitch
	EndIf
	#comments-start
		In case of that your inverter does not provide the State of Charge, he will be estimated.
		The calculation method used is linear and considers that a fully charged 12V battery has
		a voltage of 13.6V while the voltage of a fully discharged battery is only 11.6V .
		In this way each percentage of Charge level corresponds to 0.02V.
		This method is not accurate but offers a consistent approximation.
	#comments-end
	If ($battCh = 255) Then
		Local $nbattery = Floor($battVol / 12)
		$battCh = Floor(($battVol - (11.6 * $nbattery)) / (0.02 * $nbattery))
		Local $arr[2] = ["battch Value need to be Estimated\nResult : %s", $battCh]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	#comments-start
		In case your inverter does not provide a consistent value for its runtime,
		he will also be determined by the calculation
		The calculation takes into account the capacity of the batteries,
		the instantaneous charge, the battery voltage, their state of charge,
		the Power Factor as well as a coefficient allowing to take into account
		a large instantaneous charge (this limits the runtime ).
	#comments-end
	If ($battruntime >= 86400 ) Then
		Local $BattInstantCurrent = ($upsoutpower * $upsLoad) / ( $battVol* 100)
		$battruntime = Floor(($batcapacity * $upsPF * $battCh * (1-$PowerDivider) * 3600) / ($BattInstantCurrent * 100))
		Local $arr[2] = ["battruntime Value need to be Estimated\nResult : %s", $battruntime]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	EndIf
	Switch $battch
		Case 76 To 100
			$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_100)
			SetColor(0xffffff, $wPanel, $upslowbatt)
		Case 51 To 75
			$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_75)
			SetColor(0xffffff, $wPanel, $upslowbatt)
		Case 40 To 50
			$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_50)
			SetColor(0xffffff, $wPanel, $upslowbatt)
		Case 26 To 39
			$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_50)
			SetColor($red, $wPanel, $upslowbatt)
		Case 11 To 25
			$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_25)
			SetColor($red, $wPanel, $upslowbatt)
		Case 0 To 10
			If IsPairSeconds() Then
				$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_25)
			Else
				$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_0)
			EndIf
			SetColor($red, $wPanel, $upslowbatt)
		Case Else
			$ActualIcon_IDX = BitOr($ActualIcon_IDX, $IDX_BATT_0)
			SetColor($red, $wPanel, $upslowbatt)
	EndSwitch
	$battrtimeStr = TimeToStr($battruntime)
	GuiCtrlSetData($remainTimeLabel, $battrtimeStr)
	UpdateValue($needle1, $inputVol, $inputv, $dial1, getOption("mininputv"), getOption("maxinputv"))
	UpdateValue($needle2, $outputVol, $outputv, $dial2, getOption("minoutputv"), getOption("maxoutputv"))
	UpdateValue($needle3, $inputFreq, $inputf, $dial3, getOption("mininputf"), getOption("maxinputf"))
	UpdateValue($needle4, $battVol, $battv, $dial4, getOption("minbattv"), getOption("maxbattv"))
	UpdateValue($needle5, $upsLoad, $upsl, $dial5, getOption("minupsl"), getOption("maxupsl"))
	UpdateValue($needle5, $upsoutpower, $realoutpower, $dial5, null, null, 0, False)
	UpdateValue($needle6, $battCh, $upsch, $dial6, 0, 100)
	rePaint()
	#comments-start
		If connection to UPS is in fact alive and charge below shutdown setting and ups is not online
		add different from status 0 when UPS not connected but NUT is running
	#comments-end
	If (IsShutdownCondition()) Then
		WriteLog("Shutdown Condition reach", $LOG2FILE, $DBG_NOTICE)
		$InstantAction = GetOption("InstantAction")
		If $InstantAction = 1 Then
			Local $arr[2] = ["Proceed to Instant Shutdown Action : %s", GetOption("TypeOfStop")]
			WriteLog($arr, $LOG2FILE, $DBG_NOTICE)
			FileClose($hLOGFile)
			Shutdown(GetOption("TypeOfStop"))
		Else
			WriteLog("Delayed Shutdown", $LOG2FILE, $DBG_NOTICE)
			ShutdownGui()
		EndIf
	EndIf
	SetIconGuiTray()
EndFunc ;==> Update

Func SetIconGuiTray($id_gui = $gui, $ForceUpdate = False, $HasFocus = True)
	;Logging removed due to too many writes
	If $LastIcon_IDX <> $ActualIcon_IDX Or $ForceUpdate Then
		If $HasFocus Then
			Local $tmp_Gui_IDX = BitOr($ActualIcon_IDX, $IDX_ICON_BASE_APP)
			Local $tmp_Tray_IDX = BitOr($ActualIcon_IDX, $IDX_ICON_BASE_SYS)
			TraySetIcon($IconDLL, $tmp_Tray_IDX)
			GUISetIcon($IconDLL, $tmp_Gui_IDX, $id_gui)
			Local $arrValue[2] = [$tmp_Gui_IDX, $tmp_Tray_IDX]
			Local $arr[2] = ["Gui/Tray Icon Updated to : %s/%s", $arrValue]
			WriteLog($arr, $LOG2FILE, $DBG_NOTICE)
			$LastIcon_IDX = $ActualIcon_IDX
		Else
			Local $tmp_Gui_IDX = BitXOR(BitOr($ActualIcon_IDX, $IDX_ICON_BASE_APP), $WIN_DARK)
			Local $tmp_Tray_IDX = BitOr($ActualIcon_IDX, $IDX_ICON_BASE_SYS)
			TraySetIcon($IconDLL, $tmp_Tray_IDX)
			GUISetIcon($IconDLL, $tmp_Gui_IDX, $id_gui)
			Local $arrValue[2] = [$tmp_Gui_IDX, $tmp_Tray_IDX]
			Local $arr[2] = ["Gui/Tray Icon Updated to : %s/%s", $arrValue]
			WriteLog($arr, $LOG2FILE, $DBG_NOTICE)
			$LastIcon_IDX = $ActualIcon_IDX
		EndIf
	EndIf
EndFunc

Func SetTrayIconText()
	WriteLog("Enter SetTrayIconText Function", $LOG2FILE, $DBG_DEBUG)
	$trayStatus  = ""
	If $socket > 0 Then
		If $battCh < 40 Then
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
		$trayStatus = __("Connection lost") & @LF & StringFormat(__("%d attempts remaining"), ($MaxReconnectTry - $ReconnectTry))
	Else
		$trayStatus = __("Not Connected")
	EndIf
	Local $trayStr = $ProgramDesc & " - " & $ProgramVersion & $trayStatus
	Local $arr[2] = ["TraySetToolTip Updated To :\n %s", $trayStr]
	WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	TraySetToolTip($trayStr)
EndFunc ;==> SetTrayIconText

Func ReconnectNut()
	WriteLog("Enter ReconnectNut Function", $LOG2FILE, $DBG_DEBUG)
	Local $NewSocket = ConnectServer()
	Opt("TCPTimeout", 3000)
	$ReconnectTry += 1
	If $ReconnectTry >= $MaxReconnectTry Then
		WriteLog("Max Retry reached", $LOG2FILE, $DBG_ERROR)
		AdlibUnregister("ReconnectNut")
		WriteLog("ReconnectNut Function UnRegistered", $LOG2FILE, $DBG_DEBUG)
		DisconnectServer()
		$ActualIcon_IDX = $IDX_ICO_OFFLINE
		SetIconGuiTray()
		MsgBox(0, __("Alert"), __("Connection to Nut server could not be reestablished within the specified time"), 30, $gui)
	ElseIf $NewSocket >= 0  Then
		GetUPSInfo()
		SetUPSInfo()
		Update()
		AdlibRegister("Update", $RefreshInterval)
		WriteLog("Update Function Registered", $LOG2FILE, $DBG_DEBUG)
		AdlibUnregister("ReconnectNut")
		WriteLog("ReconnectNut Function UnRegistered", $LOG2FILE, $DBG_DEBUG)
	EndIf
EndFunc ;==> ReconnectNut

Func ShutdownGui_Event($hWnd, $Msg, $wParam, $lParam)
	WriteLog("Enter ShutdownGui_Event Function", $LOG2FILE, $DBG_DEBUG)
	$nNotifyCode = BitShift($wParam, 16)
	$nID = BitAnd($wParam, 0x0000FFFF)
	$hCtrl = $lParam
	writelog("notify " & $nNotifyCode)
	If $nID = $guishutdown Then
		Switch $nNotifyCode
			Case $LBN_DBLCLK
				WriteLog("Double-Click to ShutdownGui", $LOG2FILE, $DBG_DEBUG)
		EndSwitch
	EndIf
	If $nID = $Grace_btn Then
		Switch $nNotifyCode
			Case $LBN_DBLCLK
				WriteLog("Double-Click to Grace Button", $LOG2FILE, $DBG_DEBUG)
		EndSwitch
	EndIf
	If $nID = $Shutdown_btn Then
		Switch $nNotifyCode
			Case $LBN_DBLCLK
				WriteLog("Double-Click to Shutdown Button", $LOG2FILE, $DBG_DEBUG)
		EndSwitch
	EndIf
EndFunc ;==> ShutdownGui_Event

Func setTrayMode()
	WriteLog("Enter setTrayMode Function", $LOG2FILE, $DBG_DEBUG)
	$minimizetray = GetOption("minimizetray")
	If $minimizetray == 1 Then
		SetIconGuiTray()
		TraySetState($TRAY_ICONSTATE_SHOW)
		;The script is not paused when the icon in the notification area is selected.
		Opt("TrayAutoPause", 0)
		;Items are not checked when selected.
		Opt("TrayMenuMode", 3)
		TraySetClick(8)
		TraySetToolTip($ProgramDesc & " - " & $ProgramVersion )
	Else
		TraySetState($TRAY_ICONSTATE_HIDE)
	EndIf
EndFunc ;==> setTrayMode

Func InitLanguage()
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
	WriteLog("Language initialization complete", $LOG2FILE, $DBG_DEBUG)
EndFunc

Func InitSystray()
	;Create and intialize Systray Icon
	TraySetState($TRAY_ICONSTATE_HIDE)
	setTrayMode()
	If $LangChanged == 1 Then
		TrayItemSetText($idTrayPref, __("Preferences"))
		TrayItemSetText($idTrayAbout, __("Check Update"))
		TrayItemSetText($idTrayAbout, __("About"))
		TrayItemSetText($idTrayExit, __("Exit"))
	Else
		$idTrayPref = TrayCreateItem(__("Preferences"))
		TrayCreateItem("")
		$idTrayUpdate = TrayCreateItem(__("Check Update"))
		TrayCreateItem("")
		$idTrayAbout = TrayCreateItem(__("About"))
		TrayCreateItem("")
		$idTrayExit = TrayCreateItem(__("Exit"))
	EndIf
	WriteLog("Systray initialization completed", $LOG2FILE, $DBG_DEBUG)
EndFunc ;==> InitSystray

Func OnExitWinNUT()
	AdlibUnregister("Update")
	TCPSend($socket, "LOGOUT")
	TCPCloseSocket($socket)
	DeletePortProxy()
	TCPShutdown()
	WriteLog("Close WinNut", $LOG2FILE, $DBG_NOTICE)
	WriteLog($START_LOG_STR, $LOG2FILE, $DBG_NOTICE)
	FileClose($hLOGFile)
EndFunc ;==> OnExitWinNUT

Func On_WM_ACTIVATE($hWnd, $Msg, $wParam, $lParam)
	Local $iState = BitAnd($wParam, 0x0000FFFF), $iMinimize = BitShift($wParam, 16)
	If $iState Then
		SetIconGuiTray($hWnd, True, True)
		WriteLog($hWnd & " Get Focus", $LOG2FILE, $DBG_DEBUG)
	Else
		SetIconGuiTray($hWnd, True, False)
		WriteLog($hWnd & " Lost Focus", $LOG2FILE, $DBG_DEBUG)
	EndIf
	Return $GUI_RUNDEFMSG
EndFunc ;==> On_WM_ACTIVATE

Func mainLoop()
	WriteLog("Enter mainLoop Function", $LOG2FILE, $DBG_DEBUG)
	Local $minimizetray = GetOption("minimizetray")
	While 1
		If $sChangeLog <> null Then
			WriteLog("Update detected. Open updateGui", $LOG2FILE, $DBG_DEBUG)
			updateGui()
		EndIf
		If ($minimizetray == 1) Then
			$tMsg = TrayGetMsg()
			Switch $tMsg
				Case $TRAY_EVENT_PRIMARYDOUBLE
					GuiSetState(@SW_SHOW, $gui)
					GuiSetState(@SW_RESTORE ,$gui)
					TraySetState($TRAY_ICONSTATE_HIDE)
					WriteLog("Double-Click on Tray Icon", $LOG2FILE, $DBG_DEBUG)
				Case $idTrayExit
					Exit
				Case $idTrayAbout
					WriteLog("Open aboutGui From Tray Icon", $LOG2FILE, $DBG_DEBUG)
					aboutGui()
				Case $idTrayUpdate
					WriteLog("Check Update Manually From Tray Icon", $LOG2FILE, $DBG_DEBUG)
					VerifyUpdate(True)
				Case $idTrayPref
					WriteLog("Open prefGui From Tray Icon", $LOG2FILE, $DBG_DEBUG)
					AdlibUnregister("Update")
					WriteLog("Update Function UnRegistered", $LOG2FILE, $DBG_DEBUG)
					$changedprefs = prefGui()
					If $changedprefs == 1 Then
						WriteLog("Preferences Changed", $LOG2FILE, $DBG_DEBUG)
						$painting = 1
						GuiDelete($dial1)
						GuiDelete($dial2)
						GuiDelete($dial3)
						GuiDelete($dial4)
						GuiDelete($dial5)
						GuiDelete($dial6)
						DrawError(160, 70, "Delete")
						$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100)
						$dial1 = DrawDial(160, 70, GetOption("mininputv"), __("Input Voltage"), "V", $inputv, $needle1, null, null, $calc, 20, 70)
						$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100)
						$dial2 = DrawDial(480, 70, GetOption("minoutputv"), __("Output Voltage)", "V", $outputv, $needle2, null, null, $calc, 20, 70)
						$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100)
						$dial3 = DrawDial(320, 70, GetOption("mininputf"), __("Input Frequency"), "Hz", $inputf, $needle3, null, null, $calc, 20, 70)
						$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100)
						$dial4 = DrawDial(480, 200, GetOption("minbattv"), __("Battery Voltage"), "V", $battv, $needle4, null, null, $calc, 20, 120)
						$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100)
						$dial5 = DrawDial(320, 200, 0, __("UPS Load"), "%", $upsl, $needle5, $realoutpower, "W", $calc, -1, 80)
						$dial6 = DrawDial(160, 200, 0, __("Battery Charge"), "%", $upsch, $needle6, null, null, 1, 30, 101)
						$painting = 0
					EndIf
					If $haserror == 0 Then
						Update()
						AdlibRegister("Update",1000)
						WriteLog("Update Function Registered", $LOG2FILE, $DBG_DEBUG)
					EndIf
			EndSwitch
		EndIf
		SetIconGuiTray()
		$nMsg = GUIGetMsg(1)
		If GetOption("closetotray") == 0 Then
			If ($nMsg[0] == $GUI_EVENT_CLOSE And $nMsg[1]==$gui) Or $nMsg[0] == $exitMenu Or $nMsg[0] == $exitb Then
				WriteLog("Exit WinNut From RedCross GUI", $LOG2FILE, $DBG_DEBUG)
				Exit
			EndIf
		Else
			If $nMsg[0] == $exitMenu Or $nMsg[0] == $exitb Then
				WriteLog("Exit WinNut", $LOG2FILE, $DBG_DEBUG)
				Exit
			EndIf
			If ($nMsg[0] == $GUI_EVENT_CLOSE And $nMsg[1]==$gui) Then
				GuiSetState(@SW_HIDE , $gui)
				TraySetState($TRAY_ICONSTATE_SHOW)
				WriteLog("Reduce to Tray Icon", $LOG2FILE, $DBG_DEBUG)
			EndIf
		EndIf
		If ($nMsg[0] == $GUI_EVENT_MINIMIZE And $nMsg[1]==$gui And $minimizetray ==1) Then
			GuiSetState(@SW_HIDE , $gui)
			TraySetState($TRAY_ICONSTATE_SHOW)
			WriteLog("Reduce to Tray Icon", $LOG2FILE, $DBG_DEBUG)
		EndIf
		If $nMsg[0] == $toolb Or $nMsg[0] == $settingssubMenu Then
			WriteLog("Open prefGui From Menu/Button", $LOG2FILE, $DBG_DEBUG)
			AdlibUnregister("Update")
			WriteLog("Update Function UnRegistered", $LOG2FILE, $DBG_DEBUG)
			$changedprefs = prefGui()
			if $changedprefs == 1 Then
				WriteLog("Preferences Changed", $LOG2FILE, $DBG_DEBUG)
				$painting = 1
				GuiDelete($dial1)
				GuiDelete($dial2)
				GuiDelete($dial3)
				GuiDelete($dial4)
				GuiDelete($dial5)
				GuiDelete($dial6)
				DrawError(160, 70, "Delete")
				$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100)
				$dial1 = DrawDial(160, 70, GetOption("mininputv"), __("Input Voltage"), "V", $inputv, $needle1, null, null, $calc, 20, 70)
				$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100)
				$dial2 = DrawDial(480, 70, GetOption("minoutputv"), __("Output Voltage"), "V", $outputv, $needle2, null, null, $calc, 20, 70)
				$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100)
				$dial3 = DrawDial(320, 70, GetOption("mininputf"), __("Input Frequency"), "Hz", $inputf, $needle3, null, null, $calc, 20, 70)
				$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100)
				$dial4 = DrawDial(480, 200, GetOption("minbattv"), __("Battery Voltage"), "V", $battv, $needle4, null, null, $calc, 20, 120)
				$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100)
				$dial5 = DrawDial(320, 200, 0, __("UPS Load"), "%", $upsl, $needle5, $realoutpower, "W", $calc, -1, 80)
				$dial6 = DrawDial(160, 200, 0, __("Battery Charge"), "%", $upsch, $needle6, null, null, 1, 30, 101)
				$painting = 0
			EndIf
			If $haserror == 0 Then
				Update()
				AdlibRegister("Update",1000)
				WriteLog("Update Function Registered", $LOG2FILE, $DBG_DEBUG)
			EndIf
		EndIf
		If $nMsg[0] == $aboutMenu Then
			WriteLog("Open aboutGui From Menu", $LOG2FILE, $DBG_DEBUG)
			aboutGui()
		EndIf
		If $nMsg[0] == $updateMenu Then
			WriteLog("Check Update Manually From  Menu", $LOG2FILE, $DBG_DEBUG)
			VerifyUpdate(True)
		EndIf
		If $nMsg[0] == $listvarMenu Then
			WriteLog("Open varlistGui From Menu", $LOG2FILE, $DBG_DEBUG)
			varlistGui()
		EndIf
		If $nMsg[0]== $reconnectMenu Then
			WriteLog("Reconnect From Menu", $LOG2FILE, $DBG_DEBUG)
			$ActualIcon_IDX = $IDX_ICO_RETRY
			SetIconGuiTray($gui)
			AdlibUnregister("Update")
			WriteLog("Update Function UnRegistered", $LOG2FILE, $DBG_DEBUG)
			$status = ConnectServer()
			Opt("TCPTimeout", 3000)
			GetUPSInfo()
			SetUPSInfo()
			GetUPSData()
			Update()
			GuiRegisterMsg(0x000F, "rePaint")
			AdlibRegister("GetUPSData", GetOption("delay"))
			WriteLog("GetUPSData Function Registered", $LOG2FILE, $DBG_DEBUG)
			AdlibRegister("Update", $RefreshInterval)
			WriteLog("Update Function Registered", $LOG2FILE, $DBG_DEBUG)
			AdlibRegister("SetTrayIconText", $RefreshInterval)
			WriteLog("SetTrayIconText Function Registered", $LOG2FILE, $DBG_DEBUG)
		EndIf
		If $nMsg[0]== $DisconnectMenu Then
			WriteLog("Disconnect From Menu", $LOG2FILE, $DBG_DEBUG)
			AdlibUnregister("Update")
			WriteLog("Update Function UnRegistered", $LOG2FILE, $DBG_DEBUG)
			GUICtrlSetState($DisconnectMenu, $GUI_DISABLE)
			$socket = DisconnectServer()
			ResetGui()
			SetUPSInfo()
			GuiCtrlSetData($remainTimeLabel, "")
			$ActualIcon_IDX = $IDX_ICO_OFFLINE
			SetIconGuiTray($gui)
		EndIf
		For $vKey In $MenuLangListhwd
			If $nMsg[0] == $MenuLangListhwd.Item($vKey) Then
				If GetOption("language") <> $vKey Then 
					$LangChanged = 1
					GUICtrlSetState($MenuLangListhwd.Item($vKey), $GUI_CHECKED)
					SetOption("language", $vKey, "string")
					WriteParams()
					Local $arr[2] = ["Language Change To : %s", $vKey]
					WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
					For $vxKey In $MenuLangListhwd
						If $vxKey == $vKey Then
							GUICtrlSetState($MenuLangListhwd.Item($vxKey), $GUI_CHECKED)
						Else
							GUICtrlSetState($MenuLangListhwd.Item($vxKey), $GUI_UNCHECKED)
						EndIf
					Next
					InitLanguage()
					InitSystray()
					OpenMainWindow()

					Local $arr[2] = ["Language switched to : %s", $vKey]
					WriteLog($arr, $LOG2FILE, $DBG_NOTICE)
				EndIf
				ExitLoop
			EndIf
		Next
	WEnd
	FileClose($hLOGFile)
EndFunc ;==> mainLoop

Func WinNut_Init()
	;Initialize all Options Data
	InitOptionDATA()
	If $status == -1 Then
		WriteLog("Critical Error - Couldn't initialize Options", $LOG2FILE, $DBG_ERROR)
		MsgBox(48, "Critical Error", "Couldn't initialize Options")
		Exit
	EndIf

	;load/create ini file
	ReadParams()
	;If new params Added From New Version
	WriteParams()

	If GetOption("uselogfile") == 1 Then
		AdlibRegister("WriteLogToDisk", $DELAY_WRITE_LOG)
		If $hLogFile == null Then
			If Not FileExists($LogFile) Then
				_FileCreate($LogFile)
			EndIf
			$hLogFile = FileOpen($LogFile, $FO_APPEND)
		EndIf
	EndIf

	WriteLog($START_LOG_STR, $LOG2FILE, $DBG_NOTICE)
	WriteLog("Enter WinNut_Init Function", $LOG2FILE, $DBG_DEBUG)

	OnAutoItExitRegister("OnExitWinNUT")

	If Not FileExists(@ScriptDir & "\Language") Then
		DirCreate(@ScriptDir & "\Language")
		WriteLog("Directory Language Created.", $LOG2FILE, $DBG_DEBUG)
	EndIf

	If Not FileExists(@ScriptDir & "\Resources") Then
		DirCreate(@ScriptDir & "\Resources")
		WriteLog("Directory Resources Created.", $LOG2FILE, $DBG_DEBUG)
	EndIf

	;Install all needed Files
	;icon
	Fileinstall(".\images\Jpg\ups.jpg", @ScriptDir & "\Resources\ups.jpg", 1)
	;DLL_Icon
	Fileinstall(".\WinNUT_Icons\bin\Debug\netstandard2.0\WinNUT_Icons.dll", @ScriptDir & "\Resources\WinNUT_Icons.dll", 1)
	;Updater
	Fileinstall(".\Build\WinNUT-Updater.exe", @ScriptDir & "\Resources\WinNUT-Updater.exe", 1)

	;Get Info from Regedit for Icon Theme
	Local $RegResult_APP = RegRead("HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme")
	If @error <> 0 Then
		WriteLog("Cannot Read AppsUseLightTheme", $LOG2FILE, $DBG_WARNING)
		$RegResult_APP = 0
	EndIf
	If $RegResult_APP == 0 Then
		$IDX_ICON_BASE_APP = BitOr($IconIdxOffset, $WIN_DARK)
		WriteLog("Windows App Use Dark Theme", $LOG2FILE, $DBG_NOTICE)
	Else
		$IDX_ICON_BASE_APP = $IconIdxOffset
		WriteLog("Windows App Use Light Theme", $LOG2FILE, $DBG_NOTICE)
	EndIf

	Local $RegResult_SYS = RegRead("HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme")
	If @error <> 0 Then
		WriteLog("Cannot Read SystemUsesLightTheme", $LOG2FILE, $DBG_WARNING)
		$RegResult_SYS = 0
	EndIf
	If $RegResult_SYS == 0 Then
		$IDX_ICON_BASE_SYS = BitOr($IconIdxOffset, $WIN_DARK)
		WriteLog("Windows Use Dark Theme", $LOG2FILE, $DBG_NOTICE)
	Else
		$IDX_ICON_BASE_SYS = $IconIdxOffset
		WriteLog("Windows Use Light Theme", $LOG2FILE, $DBG_NOTICE)
	EndIf

	;Language
	; function to auto include all language file
	_ListFileInstallFolder(".\Language", "\Language", 0, "*.lng", "include", True)
	;Now file is generated so include it
	#Include "include.au3"
	WriteLog("All necessary files have been installed", $LOG2FILE, $DBG_DEBUG)

	;Get Script Version
	$ProgramVersion = _GetScriptVersion()

	;HERE STARTS MAIN SCRIPT
	$status = TCPStartup()
	If $status == False Then
		WriteLog("Critical Error - Couldn't startup TCP", $LOG2FILE, $DBG_ERROR)
		MsgBox(48,"Critical Error", "Couldn't startup TCP")
		Exit
	EndIf
	Opt("GUIDataSeparatorChar", ".")

	;Determine if running as exe or script
	If @Compiled Then 
		$runasexe = True
		WriteLog("WinNut Run as Compiled Version", $LOG2FILE, $DBG_NOTICE)
	Else
		WriteLog("WinNut Run as Scripted Version", $LOG2FILE, $DBG_NOTICE)
	EndIf

	InitLanguage()
	InitSystray()

	OpenMainWindow()
	If (GetOption("minimizeonstart") == 1 And GetOption("minimizetray") == 1) Then
		GuiSetState(@SW_HIDE, $gui)
		TraySetState($TRAY_ICONSTATE_SHOW)
	Else
		GuiSetState(@SW_SHOW, $gui)
		TraySetState($TRAY_ICONSTATE_HIDE)
	EndIf

	;Initilize connexion to Nut/Ups
	$status = ConnectServer()
	Opt("TCPTimeout", 3000)
	GetUPSInfo()
	SetUPSInfo()
	GetUPSData()
	Update()
	GuiRegisterMsg(0x000F, "rePaint")
	;GuiRegisterMsg($WM_SETFOCUS, "On_WM_ACTIVATE")
	;GuiRegisterMsg($WM_KILLFOCUS, "On_WM_ACTIVATE")
	GuiRegisterMsg($WM_ACTIVATE, "On_WM_ACTIVATE")
	AdlibRegister("GetUPSData", GetOption("delay"))
	WriteLog("GetUPSData Function Registered", $LOG2FILE, $DBG_DEBUG)
	AdlibRegister("Update", $RefreshInterval)
	WriteLog("Update Function Registered", $LOG2FILE, $DBG_DEBUG)
	AdlibRegister("SetTrayIconText", $RefreshInterval)
	WriteLog("SetTrayIconText Function Registered", $LOG2FILE, $DBG_DEBUG)
EndFunc ;==> WinNut_Init

WinNut_Init()
If GetOption("VerifyUpdateAtStart") == 1 Then
	VerifyUpdate()
EndIf
mainLoop()