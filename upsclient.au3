#pragma compile(FileVersion, 1.7.0.5)
#pragma compile(Icon, .\images\upsicon.ico)
#pragma compile(Out, .\Build\upsclient.exe)
#pragma compile(Compression, 1)
#pragma compile(Comments, 'Windows NUT Client')
#pragma compile(FileDescription, Windows NUT Client. This is a NUT windows client for monitoring your ups hooked up to your favorite linux server.)
#pragma compile(LegalCopyright, Freeware)
#pragma compile(ProductName, WinNUT-Client)
#pragma compile(Compatibility, win7, win8, win81, win10)
;
#include <GUIConstants.au3>
#include <Misc.au3>
#Include <Constants.au3>
#include <Array.au3>
#include <TrayConstants.au3>
#include <Timers.au3>
#include "nutGlobal.au3"
#include "nutDraw.au3"
#include "nutColor.au3"
#include "nutOption.au3"
#include "nutGui.au3"
#include "nutNetwork.au3"
#Include "nutTreeView.au3"

If UBound(ProcessList(@ScriptName)) > 2 Then Exit

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
	$minimizetray = GetOption("minimizetray")
	If $minimizetray == 1 Then
		TraySetClick(0)
	EndIf
	$guipref = GUICreate("Preferences", 364, 331, 190, 113,-1,-1,$gui  )
	GUISetIcon(@tempdir & "upsicon.ico")
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
	$lblstartminimized = GUICtrlCreateLabel("Start Minimized", 16, 84, 99, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$chstartminimized = GUICtrlCreateCheckbox("StartMinimized", 224, 81, 17, 17,BitOr($BS_AUTOCHECKBOX,$WS_TABSTOP ),$WS_EX_STATICEDGE)
	$lblclosetotray = GUICtrlCreateLabel("Close to Tray", 16, 126, 99, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$chclosetotray = GUICtrlCreateCheckbox("ClosetoTray", 224, 123, 17, 17,BitOr($BS_AUTOCHECKBOX,$WS_TABSTOP ),$WS_EX_STATICEDGE)
	if GetOption("minimizetray") == 0 Then
		GuiCtrlSetState($chMinimizeTray, $GUI_UNCHECKED)
		GuiCtrlSetState($chstartminimized, $GUI_UNCHECKED)
		GuiCtrlSetState($chclosetotray, $GUI_UNCHECKED)
		GUICtrlSetState($lblstartminimized, $GUI_DISABLE)
		GUICtrlSetState($chstartminimized, $GUI_DISABLE)
		GUICtrlSetState($lblclosetotray, $GUI_DISABLE)
		GUICtrlSetState($chclosetotray, $GUI_DISABLE)
	Else
		GuiCtrlSetState($chMinimizeTray , $GUI_CHECKED)
		GUICtrlSetState($lblstartminimized,$GUI_ENABLE)
		GUICtrlSetState($chstartminimized,$GUI_ENABLE)
		GUICtrlSetState($lblclosetotray, $GUI_ENABLE)
		GUICtrlSetState($chclosetotray, $GUI_ENABLE)
		if GetOption("minimizeonstart") == 0 Then
			GuiCtrlSetState($chstartminimized, $GUI_UNCHECKED)
		Else
			GuiCtrlSetState($chstartminimized, $GUI_CHECKED)
		EndIf
		if GetOption("closetotray") == 0 Then
			GuiCtrlSetState($chclosetotray, $GUI_UNCHECKED)
		Else
			GuiCtrlSetState($chclosetotray, $GUI_CHECKED)
		EndIf
	EndIf

	$lblstartwithwindows = GUICtrlCreateLabel("Start with Windows", 16, 168, 99, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$chStartWithWindows = GUICtrlCreateCheckbox("Startwithwindows", 224, 167, 17, 17,BitOr($BS_AUTOCHECKBOX,$WS_TABSTOP ),$WS_EX_STATICEDGE)
	if $runasexe == True Then
		GUICtrlSetState($chStartWithWindows,$GUI_ENABLE)
		GUICtrlSetState($lblstartwithwindows,$GUI_ENABLE)
		if GetOption("startwithwindows") == 0 Then
			GuiCtrlSetState($chStartWithWindows , $GUI_UNCHECKED)
		Else
			GuiCtrlSetState($chStartWithWindows, $GUI_CHECKED)
		EndIf
	Else
		GuiCtrlSetState($chStartWithWindows, $GUI_UNCHECKED)
		GUICtrlSetState($lblstartwithwindows,$GUI_DISABLE)
		GUICtrlSetState($chStartWithWindows,$GUI_DISABLE)
	EndIf

	$TSShutdown = GUICtrlCreateTabItem("Shutdown Options")
	GUICtrlCreateLabel("Shutdown if battery lower than", 16, 39, 179, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$lshutdownpcbatt = GUICtrlCreateInput(GetOption("shutdownpcbatt"), 217, 36, 25, 23)
	GUICtrlCreateLabel("%", 248, 39, 15, 19)
	GUICtrlCreateLabel("Shutdown if runtime lower than (sec)", 16, 81, 179, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$lshutdownrtime = GUICtrlCreateInput(GetOption("shutdownpctime"), 217, 78, 40, 23)
	$lblInstantShutdown = GUICtrlCreateLabel("Shutdown Immediately", 16, 123, 179, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$chInstantShutdown = GUICtrlCreateCheckbox("Shutdown Immediately", 224, 122, 17, 17,BitOr($BS_AUTOCHECKBOX,$WS_TABSTOP ),$WS_EX_STATICEDGE)
	$lbldelayshutdown = GUICtrlCreateLabel("Delay to Shutdown", 16, 165, 179, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$ldelayshutdown = GUICtrlCreateInput(GetOption("ShutdownDelay"), 217, 162, 40, 23)
	$lblAllowGrace = GUICtrlCreateLabel("Allow Extended Shutdown Time", 16, 207, 179, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$chAllowGrace = GUICtrlCreateCheckbox("AllowExtendedShutdownTime", 224, 206, 17, 17,BitOr($BS_AUTOCHECKBOX,$WS_TABSTOP ),$WS_EX_STATICEDGE)
	$lbldelaygrace = GUICtrlCreateLabel("Grace Delay to Shutdown", 16, 249, 179, 17, BitOR($SS_BLACKRECT,$SS_GRAYFRAME,$SS_LEFTNOWORDWRAP))
	$ldelaygrace = GUICtrlCreateInput(GetOption("GraceDelay"), 217, 246, 40, 23)
	if GetOption("InstantShutdown") == 0 Then
		GuiCtrlSetState($chInstantShutdown, $GUI_UNCHECKED)
		GUICtrlSetState($lbldelayshutdown,$GUI_ENABLE)
		GUICtrlSetState($ldelayshutdown,$GUI_ENABLE)
	Else
		GuiCtrlSetState($chInstantShutdown, $GUI_CHECKED)
		GUICtrlSetState($lbldelayshutdown,$GUI_DISABLE)
		GUICtrlSetState($ldelayshutdown,$GUI_DISABLE)
	EndIf
	if GetOption("AllowGrace") == 0 Then
		GuiCtrlSetState($chAllowGrace, $GUI_UNCHECKED)
		GUICtrlSetState($lbldelaygrace,$GUI_DISABLE)
		GUICtrlSetState($ldelaygrace,$GUI_DISABLE)
	Else
		GuiCtrlSetState($chAllowGrace, $GUI_CHECKED)
		GUICtrlSetState($lbldelaygrace,$GUI_ENABLE)
		GUICtrlSetState($ldelaygrace,$GUI_ENABLE)
	EndIf

	GUICtrlCreateTabItem("")
	GuiSetState(@SW_DISABLE,$gui )
	GUISetState(@SW_SHOW,$guipref)

	While 1
		$nMsg1 = GUIGetMsg()
		Switch $nMsg1
			Case $GUI_EVENT_CLOSE
				ExitLoop
			Case $Bapply, $Bok
				SetOption("ipaddr", GuiCtrlRead($Iipaddr), "string")
				SetOption("port", GuiCtrlRead($Iport), "number")
				SetOption("upsname", GuiCtrlRead($Iupsname), "string")
				SetOption("delay", GuiCtrlRead($Idelay), "number")
				SetOption("mininputv", GuiCtrlRead($lminInputVoltage), "number")
				SetOption("maxinputv", GuiCtrlRead($lmaxInputVoltage), "number")
				SetOption("minoutputv", GuiCtrlRead($lminOutputVoltage), "number")
				SetOption("maxoutputv", GuiCtrlRead($lmaxOutputVoltage), "number")
				SetOption("mininputf", GuiCtrlRead($lminInputFreq), "number")
				SetOption("maxinputf", GuiCtrlRead($lmaxInputFreq), "number")
				SetOption("minupsl", GuiCtrlRead($lminUpsLoad), "number")
				SetOption("maxupsl", GuiCtrlRead($lmaxUpsLoad), "number")
				SetOption("minbattv", GuiCtrlRead($lminBattVoltage), "number")
				SetOption("maxbattv", GuiCtrlRead($lmaxBattVoltage), "number")
				SetOption("shutdownpcbatt", GuiCtrlRead($lshutdownpcbatt), "number")
				$guiminruntime = GuiCtrlRead($lshutdownrtime)
				If $guiminruntime < 60 Then
					$guiminruntime = 60
					GUICtrlSetData($lshutdownrtime, $guiminruntime)
				EndIf
				SetOption("shutdownpctime", $guiminruntime, "number")
				$InstantShutdown = GuiCtrlRead($chInstantShutdown)
				If $InstantShutdown == $GUI_CHECKED Then
					SetOption("InstantShutdown", 1, "number")
				Else
					SetOption("InstantShutdown", 0, "number")
				EndIf
				$guidelayshutdown = GuiCtrlRead($ldelayshutdown)
				If $guidelayshutdown > $guiminruntime Then 
					$guidelayshutdown = $guiminruntime
					GUICtrlSetData($ldelayshutdown, $guidelayshutdown)
				EndIf
				SetOption("ShutdownDelay", $guidelayshutdown, "number")
				$guigracedelay = GuiCtrlRead($ldelaygrace)
				If $guigracedelay > ($guiminruntime - $guidelayshutdown) Then
					$guigracedelay = ($guiminruntime - $guidelayshutdown)
					GUICtrlSetData($ldelaygrace, $guigracedelay)
				ElseIf $guigracedelay > $guidelayshutdown Then
					$guigracedelay = $guidelayshutdown
					GUICtrlSetData($ldelaygrace, $guigracedelay)
				EndIf
				$AllowGrace = GuiCtrlRead($chAllowGrace)
				If ($AllowGrace == $GUI_UNCHECKED) or ($guigracedelay = 0) Then
					SetOption("AllowGrace", 0, "number")
				Else
					SetOption("AllowGrace", 1, "number")
				EndIf
				SetOption("GraceDelay", $guigracedelay, "number")
				if GetOption("AllowGrace") == 0 Then
					GuiCtrlSetState($chAllowGrace, $GUI_UNCHECKED)
					GUICtrlSetState($lbldelaygrace,$GUI_DISABLE)
					GUICtrlSetState($ldelaygrace,$GUI_DISABLE)
				Else
					GuiCtrlSetState($chAllowGrace, $GUI_CHECKED)
					GUICtrlSetState($lbldelaygrace,$GUI_ENABLE)
					GUICtrlSetState($ldelaygrace,$GUI_ENABLE)
				EndIf
				$minimizetray = GuiCtrlRead($chMinimizeTray)
				If $minimizetray == $GUI_CHECKED Then
					SetOption("minimizetray",1 , "number")
					$startminimized = GuiCtrlRead($chstartminimized)
					If $startminimized == $GUI_CHECKED Then
						SetOption("minimizeonstart",1 , "number")
					Else
						SetOption("minimizeonstart",0 , "number")
					EndIf
					$closetotray = GuiCtrlRead($chclosetotray)
					If $closetotray == $GUI_CHECKED Then
						SetOption("closetotray",1 , "number")
					Else
						SetOption("closetotray",0 , "number")
					EndIf
				Else
					SetOption("minimizetray",0 , "number")
					SetOption("minimizeonstart",0 , "number")
					SetOption("closetotray",0 , "number")
				EndIf
				If $runasexe == True Then
					$startwithwindows = GuiCtrlRead($chStartWithWindows)
					$linkexe = @StartupDir & "\Upsclient.lnk"
					if $startwithwindows == $GUI_CHECKED Then
						SetOption("startwithwindows" , 1 , "number")
						if FileExists($linkexe) == 0 Then
							FileCreateShortcut(@ScriptFullPath, $linkexe)
						EndIf
					Else
						if FileExists($linkexe) <> 0 Then
							FileDelete($linkexe)
						EndIf
						SetOption("startwithwindows" , 0 , "number")
					EndIf
				Else
					SetOption("startwithwindows" , 0 , "number")
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
				setTrayMode()
				if $nMsg1 == $Bok then
					ExitLoop
				EndIf
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
			Case $chMinimizeTray
				$minimizetray = GuiCtrlRead($chMinimizeTray)
				If $minimizetray == $GUI_CHECKED Then
					GUICtrlSetState($lblstartminimized,$GUI_ENABLE)
					GUICtrlSetState($chstartminimized,$GUI_ENABLE)
					GUICtrlSetState($lblclosetotray, $GUI_ENABLE)
					GUICtrlSetState($chclosetotray, $GUI_ENABLE)
				Else
					GuiCtrlSetState($chstartminimized, $GUI_UNCHECKED)
					GuiCtrlSetState($chclosetotray, $GUI_UNCHECKED)
					GUICtrlSetState($lblstartminimized,$GUI_DISABLE)
					GUICtrlSetState($chstartminimized,$GUI_DISABLE)
					GUICtrlSetState($lblclosetotray, $GUI_DISABLE)
					GUICtrlSetState($chclosetotray, $GUI_DISABLE)
				EndIf
			Case $chInstantShutdown
				$instantshutdown = GuiCtrlRead($chInstantShutdown)
				If $instantshutdown == $GUI_CHECKED Then
					GUICtrlSetState($lbldelayshutdown,$GUI_DISABLE)
					GUICtrlSetState($ldelayshutdown,$GUI_DISABLE)
				Else
					GUICtrlSetState($lbldelayshutdown,$GUI_ENABLE)
					GUICtrlSetState($ldelayshutdown,$GUI_ENABLE)
				EndIf
			Case $chAllowGrace
				$AllowGrace = GuiCtrlRead($chAllowGrace)
				if $AllowGrace == $GUI_CHECKED Then
					GUICtrlSetState($lbldelaygrace,$GUI_ENABLE)
					GUICtrlSetState($ldelaygrace,$GUI_ENABLE)
				Else
					GUICtrlSetState($lbldelaygrace,$GUI_DISABLE)
					GUICtrlSetState($ldelaygrace,$GUI_DISABLE)
				EndIf
		EndSwitch
	WEnd
	If $minimizetray == 1 Then
		TraySetClick(8)
	EndIf
	GuiDelete($guipref)
	If (WinGetState($gui) <> 17 ) Then
		GuiSetState(@SW_ENABLE,$gui )
		WinActivate($gui)
	EndIf
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
	AdlibUnregister("Update")
	$status1 = ListUPSVars(GetOption("upsname") , $varlist)
	$varlist = StringReplace($varlist , GetOption("upsname") , "")
	$vars = StringSplit($varlist , "VAR",1)
	AdlibRegister("Update",1000)
	$guilistvar = GUICreate("LIST UPS Variables", 365, 331, 196, 108, -1 , -1 , $gui)
	GUISetIcon(@tempdir & "upsicon.ico")
	$TreeView1 = GUICtrlCreateTreeView(0, 8, 361, 169)

	$Group1 = GUICtrlCreateGroup("Item properties", 0, 184, 361, 105, $BS_CENTER)
	$Label1 = GUICtrlCreateLabel("Name :", 8, 200, 38, 17)
	$Label2 = GUICtrlCreateLabel("Value :", 8, 232, 37, 17)
	$Label3 = GUICtrlCreateLabel("Description :", 8, 264, 63, 17)
	$varselected = GUICtrlCreateLabel("", 50, 200, 291, 17, $SS_SUNKEN)
	$varvalue = GUICtrlCreateLabel("", 50, 232, 291, 17, $SS_SUNKEN)
	$vardesc = GUICtrlCreateLabel("", 72, 264, 283, 17, $SS_SUNKEN)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	$Reload_Btn = GUICtrlCreateButton("Reload", 80, 296, 65, 25, 0)
	$Clear_Btn = GUICtrlCreateButton("Clear", 200, 296, 65, 25, 0)
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
	;trim $name
	$name = StringStripWS ( $name, $STR_STRIPLEADING + $STR_STRIPTRAILING )

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
	GetUPSVar($ups_name ,"battery.runtime",$battruntime)
	GetUPSVar($ups_name ,"battery.capacity",$batcapacity)
	GetUPSVar($ups_name ,"input.frequency",$inputFreq)
	GetUPSVar($ups_name ,"input.voltage",$inputVol)
	GetUPSVar($ups_name ,"output.voltage",$outputVol)
	GetUPSVar($ups_name ,"ups.load",$upsLoad)
	GetUPSVar($ups_name ,"ups.status",$upsstatus)
	GetUPSVar($ups_name ,"ups.realpower.nominal",$upsoutpower)
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
	;if $socket == 0 Then
	;	Return
	;EndIf
	$status = GetData()
	if $socket == 0 then ; connection lost so throw all needles to left
		ResetGui()
		Return
	EndIf
	$trayStatus  = ""
	If $upsstatus == "OL" Then
		SetColor($green , $wPanel , $upsonline )
		SetColor(0xffffff , $wPanel , $upsonbatt )
		$trayStatus  = $trayStatus & "UPS On Line"
	Else
		SetColor($yellow , $wPanel , $upsonbatt )
		SetColor(0xffffff , $wPanel , $upsonline )
		$trayStatus  = $trayStatus & "UPS On Batt"
	EndIf
	Local $PowerDivider = 0.9
	If $upsLoad > 100 Then
		SetColor($red , $wPanel , $upsoverload )
	Else
		SetColor(0xffffff , $wPanel , $upsoverload )
		If $upsLoad > 75 Then
			$PowerDivider = 0.8
		ElseIf $upsLoad > 50 Then
			$PowerDivider = 0.85
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
		Local $RealLoad = ($upsoutpower*($upsLoad/100))
		Local $InstantCurrent = $RealLoad / $battVol
		$battruntime = Floor(((($batcapacity / $InstantCurrent)* $upsPF) * ($battCh/100) * $PowerDivider)*3600)
	EndIf
	if $battCh < 40 Then
		SetColor($red , $wPanel , $upslowbatt )
		$trayStatus  = $trayStatus & @LF &  "Low Battery"
	Else
		SetColor(0xffffff , $wPanel , $upslowbatt )
		$trayStatus  = $trayStatus & @LF &  "Battery OK"
	EndIf
	$battrtimeStr = TimeToStr($battruntime)
	GuiCtrlSetData($remainTimeLabel,$battrtimeStr) 
	UpdateValue($needle4 , $battVol , $battv , $dial4 , getOption("minbattv") , getOption("maxbattv") )
	UpdateValue($needle5 , $upsLoad , $upsl , $dial5 , getOption("minupsl") , getOption("maxupsl") )
	UpdateValue($needle6 , $battCh , $upsch , $dial6 , 0 , 100 )
	UpdateValue($needle1 , $inputVol, $inputv , $dial1 ,getOption("mininputv") , getOption("maxinputv"))
	UpdateValue($needle2 , $outputVol, $outputv , $dial2 , getOption("minoutputv") , getOption("maxoutputv"))
	UpdateValue($needle3 , $inputFreq , $inputf , $dial3 , getOption("mininputf") , getOption("maxinputf") )
	rePaint()
	TraySetToolTip($ProgramDesc & " - " & $ProgramVersion & @LF &  $trayStatus )
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
	$gui = GUICreate($ProgramDesc, 640, 380, -1 , -1,Bitor($GUI_SS_DEFAULT_GUI ,$WS_CLIPCHILDREN))
	GUISetIcon(@tempdir & "upsicon.ico")
	$fileMenu = GUICtrlCreateMenu("&File")
	$listvarMenu = GuiCtrlCreateMenuItem("&List UPS Vars",$fileMenu)
	$exitMenu = GUICtrlCreateMenuItem("&Exit", $fileMenu)
	$editMenu = GUICtrlCreateMenu("&Connection")
	$reconnectMenu = GUICtrlCreateMenuItem("&Reconnect",$editMenu)
	$settingsMenu = GUICtrlCreateMenu("&Settings")
	$settingssubMenu = GUICtrlCreateMenuItem("&Preferences",$settingsMenu)
	$helpMenu = GUICtrlCreateMenu("&Help")
	$aboutMenu = GUICtrlCreateMenuItem("About", $helpMenu)
	$log = GUICtrlCreateCombo("", 5, 335, 630, 25,Bitor($CBS_DROPDOWNLIST,0))
	$wPanel = GUICreate("", 150, 250,0, 70,BitOR($WS_CHILD, $WS_DLGFRAME), $WS_EX_CLIENTEDGE, $gui)
	GUISetBkColor($panel_bkg , $wPanel)
	$Label1 = GUICtrlCreateLabel("UPS On Line", 8, 8, 110, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsonline = GUICtrlCreateLabel("", 121, 6, 16, 16, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$Label2 = GUICtrlCreateLabel("UPS On Battery", 8, 28, 110, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsonbatt = GUICtrlCreateLabel("", 121, 26, 16, 16, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$Label3 = GUICtrlCreateLabel("UPS Overload", 8, 48, 110, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsoverload = GUICtrlCreateLabel("", 121, 46, 16, 16, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$Label4 = GUICtrlCreateLabel("UPS Battery low", 8, 68, 110, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upslowbatt = GUICtrlCreateLabel("", 121, 66, 16, 16, BitOR($SS_CENTER,$SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$labelUpsRemain = GUICtrlCreateLabel("Remaining Time", 8, 88, 110, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	$remainTimeLabel = GUICtrlCreateLabel($battrtimeStr, 8, 104, 130, 16,Bitor($SS_RIGHT,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label5 = GUICtrlCreateLabel("Manufacturer :", 8, 122, 130, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsmfr = GUICtrlCreateLabel($mfr, 8, 138, 130, 16,Bitor($SS_RIGHT,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label14 = GUICtrlCreateLabel("Name :", 8, 154, 130, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsmodel = GUICtrlCreateLabel($name, 8, 170, 130, 16,Bitor($SS_RIGHT,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label15 = GUICtrlCreateLabel("Serial :", 8, 186, 130, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsserial = GUICtrlCreateLabel($serial, 8, 202, 130, 16,Bitor($SS_RIGHT,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label16 = GUICtrlCreateLabel("Firmware :", 8, 218, 130, 16,Bitor($SS_LEFTNOWORDWRAP,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsfirmware = GUICtrlCreateLabel($firmware, 8, 234, 130, 16,Bitor($SS_RIGHT,$GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")

	GuiSwitch($gui)
	$Group8 = GUICreate("", 638, 60,0, 0,BitOR($WS_CHILD, $WS_BORDER), 0, $gui)
	$exitb = GUICtrlCreateButton("Exit", 10, 10, 73, 40, 0)
	$toolb = GUICtrlCreateButton("Settings", 102, 10, 73, 40, 0)
	GUISetState(@SW_SHOW,$Group8)
	GUISetState(@SW_SHOW,$wPanel)
	$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100 )
	$dial1 = DrawDial(160, 70 , GetOption("mininputv") , "Input Voltage" , "V" , $inputv , $needle1 , $calc)
	$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100 )
	$dial2 = DrawDial(480, 70 , GetOption("minoutputv") , "Output Voltage" , "V" , $outputv , $needle2 , $calc)
	$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100 )
	$dial3 = DrawDial(320, GetOption("maxinputf") , GetOption("mininputf") , "Input Frequency" , "Hz" , $inputf , $needle3 , $calc )
	$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100 )
	$dial4 = DrawDial(480, 200 , GetOption("minbattv") , "Battery Voltage" , "V" , $battv , $needle4 , $calc , 20 , 120)
	$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100 )
	$dial5 = DrawDial(320, 200 , 0 , "UPS Load" , "%" , $upsl , $needle5 , $calc , -1 , 80)
	$dial6 = DrawDial(160, 200 , 0 , "Battery Charge" , "%" , $upsch , $needle6 , 1 , 30 , 101)
EndFunc

Func aboutGui()
	AdlibUnregister("Update")
	$minimizetray = GetOption("minimizetray")
	If $minimizetray == 1 Then
		TraySetClick(0)
	EndIf
	$guiabout = GUICreate("About", 324, 220, 271, 178)
	GUISetIcon(@tempdir & "upsicon.ico")
	$GroupBox1 = GUICtrlCreateGroup("", 8, 0, 308, 184)
	$Image1 = GUICtrlCreatePic(@tempdir & "ups.jpg", 16, 16, 104, 104, BitOR($SS_NOTIFY,$WS_GROUP))
	$Label10 = GUICtrlCreateLabel($ProgramDesc, 128, 16, 180 , 18, $WS_GROUP)
	$Label11 = GUICtrlCreateLabel("Version " & $ProgramVersion, 128, 34, 180, 18, $WS_GROUP)
	$Label12 = GUICtrlCreateLabel("Copyright Michael Liberman" & @LF & "2006-2007", 128, 52, 270, 44, $WS_GROUP)
	$Label13 = GUICtrlCreateLabel("Based from Winnut Sf https://sourceforge.net/projects/winnutclient/", 16, 128, 270, 44, $WS_GROUP)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	$Button11 = GUICtrlCreateButton("&OK", 126, 188, 72, 24)
	GUISetState(@SW_SHOW,$guiabout)
	GuiSetState(@SW_DISABLE,$gui)
	While 1
		$nMsg2 = GUIGetMsg()
		Switch $nMsg2
			Case $GUI_EVENT_CLOSE, $Button11
				GuiDelete($guiabout)
				$guiabout = 0
				If (WinGetState($gui) <> 17 ) Then
					GuiSetState(@SW_ENABLE,$gui )
					WinActivate($gui)
				EndIf
				if $haserror == 0 Then
					AdlibRegister("Update",1000)
				EndIf
				If $minimizetray == 1 Then
					TraySetClick(8)
				EndIf
				ExitLoop
		EndSwitch
	WEnd
EndFunc

Func ShutdownGui()
	Local $Halt, $msg

	$ShutdownDelay = GetOption("ShutdownDelay")
	$grace_time = GetOption("GraceDelay")
	$AllowGrace = GetOption("AllowGrace")
	$guishutdown = GUICreate("Shutdown", (290 + $AllowGrace * 110) , 120,-1,-1, Bitor($WS_EX_TOPMOST, $WS_POPUP))
	$Grace_btn = GUICtrlCreateButton("Grace Time", 10, 10, 100, 100,BitOR($BS_NOTIFY, $GUI_SS_DEFAULT_BUTTON))
	If $AllowGrace = 0 Then
		GUICtrlSetState($Grace_btn, $GUI_HIDE)
	EndIf
	$Shutdown_btn = GUICtrlCreateButton("Shutdown Immediately" & @CRLF & "Double-Click to Shutdown", (180 + $AllowGrace * 110), 10, 100, 100,BitOR($BS_MULTILINE, $BS_NOTIFY, $GUI_SS_DEFAULT_BUTTON))
	$lbl_ups_status = GUICtrlCreateLabel("", (8 + $AllowGrace * 110), 10, 170, 40)
	$lbl_countdown = GUICtrlCreateLabel("", (8 + $AllowGrace * 110), 45, 170, 70)
	GUICtrlSetFont(-1, 48, 800, 0, "MS SansSerif")
	GUICtrlSetBkColor(-1, $GUI_BKCOLOR_TRANSPARENT)
	GUICtrlSetColor(-1, 0x000000)
	GUISetState(@SW_SHOW,$guishutdown)
	Reset_Shutdown_Timer()
	Init_Shutdown_Timer()
	$sec = @SEC
	$REDText = 1
	While 1
		$nMsg3 = GUIGetMsg()
		Select
			Case $nMsg3 = $Grace_btn
				GUICtrlSetState($Grace_btn,$GUI_DISABLE)
				_Timer_SetTimer($guishutdown, $grace_time*1000, "_Restart_Compteur")
				$Suspend_Countdown = 1
				AdlibUnregister("Update_compteur")
			Case $nMsg3 = $Shutdown_btn or $en_cours = 0
				GUICtrlSetState($Grace_btn,$GUI_DISABLE)
				GUICtrlSetState($Shutdown_btn,$GUI_DISABLE)
				AdlibUnregister("Update_compteur")
				Reset_Shutdown_Timer()
				Shutdown(17)
				Exit
			Case IsShutdownCondition() = False
				AdlibUnregister("Update_compteur")
				Reset_Shutdown_Timer()
				GuiDelete($guishutdown)
				ExitLoop
			Case $Suspend_Countdown = 1
				If @SEC <> $sec Then
					$sec = @SEC
					If $REDText Then
						GUICtrlSetColor($lbl_countdown, 0xffffff)
					Else
						GUICtrlSetColor($lbl_countdown, 0xff0000)
					EndIf
					$REDText = Not $REDText
				EndIf
		EndSelect
	WEnd
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
					GuiSetState(@SW_RESTORE ,$gui )
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
						DrawError(160 , 70 , "Delete")
						$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100 )
						$dial1 = DrawDial(160, 70 , GetOption("mininputv") , "Input Voltage" , "V" , $inputv , $needle1 , $calc)
						$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100 )
						$dial2 = DrawDial(480, 70 , GetOption("minoutputv") , "Output Voltage" , "V" , $outputv , $needle2 , $calc)
						$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100 )
						$dial3 = DrawDial(320, GetOption("maxinputf") , GetOption("mininputf") , "Input Frequency" , "Hz" , $inputf , $needle3 , $calc )
						$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100 )
						$dial4 = DrawDial(480, 200 , GetOption("minbattv") , "Battery Voltage" , "V" , $battv , $needle4 , $calc , 20 , 120)
						$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100 )
						$dial5 = DrawDial(320, 200 , 0 , "UPS Load" , "%" , $upsl , $needle5 , $calc , -1 , 80)
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
		if $nMsg[0] == $toolb or $nMsg[0]==$settingssubMenu Then
			AdlibUnregister("Update")
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
				$dial4 = DrawDial(480, 200 , GetOption("minbattv") , "Battery Voltage" , "V" , $battv , $needle4 , $calc , 20 , 120)
				$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100 )
				$dial5 = DrawDial(320, 200 , 0 , "UPS Load" , "%" , $upsl , $needle5 , $calc , -1 , 80)
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
			ConnectServer() ;;aaaaa
			Opt("TCPTimeout",3000)
			GetUPSInfo()
			SetUPSInfo()
			AdlibRegister("Update",1000)
		EndIf
	WEnd
EndFunc

;HERE STARTS MAIN SCRIPT
Fileinstall(".\images\ups.jpg", @tempdir & "ups.jpg",1)
Fileinstall(".\images\upsicon.ico", @tempdir & "upsicon.ico",1)

TraySetState($TRAY_ICONSTATE_HIDE)
$status = TCPStartup()
if $status == false Then
	MsgBox(48,"Critical Error","Couldn't startup TCP")
	Exit
EndIf

Opt("GUIDataSeparatorChar", ".")
$status = InitOptionDATA()
if $status == -1 Then
	MsgBox(48, "Critical Error", "Couldn't initialize Options")
	Exit
EndIf

ReadParams()
setTrayMode()

;Determine if running as exe or script
If StringRight( @ScriptName, 4 ) == ".exe" Then
	$runasexe = True
EndIf

$idTrayPref = TrayCreateItem("Preferences")
TrayCreateItem("")
$idTrayAbout = TrayCreateItem("About")
TrayCreateItem("")
$idTrayExit = TrayCreateItem("Exit")

OpenMainWindow()
if ( GetOption("minimizeonstart") == 1 and GetOption("minimizetray") == 1 ) Then
	GuiSetState(@SW_HIDE, $gui)
	TraySetState($TRAY_ICONSTATE_SHOW)
Else
	GuiSetState(@SW_SHOW, $gui)
	TraySetState($TRAY_ICONSTATE_HIDE)
EndIf
$ProgramVersion = _GetScriptVersion()
$status = ConnectServer()
Opt("TCPTimeout",3000)
GetUPSInfo()
SetUPSInfo()
Update()
GuiRegisterMsg(0x000F,"rePaint")
AdlibRegister("Update",GetOption("delay"))
;ShutdownGui()
mainLoop()