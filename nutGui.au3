#include <Date.au3>
#Include <GuiComboBox.au3>
#include <GuiComboBoxEx.au3>
#include <String.au3>
#include "Base64.au3"

Func WriteLogToDisk()
	FileFlush($hLogFile)
EndFunc

Func WriteLog($msg, $Dest_Log = BitOr($LOG_GUI, $LOG2FILE), $Level = $DBG_NOTICE)
	Local $gui_msg, $sLogMsg
	If BitAnd($Dest_Log, $LOG_GUI) Then
		$gui_msg = _Now() & " : "
		If IsArray($msg) Then
			$gui_msg &= StringFormat(__($msg[0]), $msg[1])
		Else
			$gui_msg &= __($msg)
		EndIf
		ControlCommand($gui, "", $log, "AddString", $gui_msg)
		ControlCommand($gui, "", $log, "SetCurrentSelection", _GUICtrlComboBox_GetCount($log) - 1)
	EndIf
	If BitAnd($Dest_Log, $LOG2FILE) And GetOption("uselogfile") == 1 Then
		If Not $msg == $START_LOG_STR Then
			FileWriteLine($hLogFile, $START_LOG_STR)
		EndIf
		If $Level <= GetOption("loglevel") Then
			$sLogMsg = _Now() & $oDBG_LVL_TXT.Item($Level)
			If IsArray($msg) Then
				If IsArray($msg[1]) Then
					Local $tmp_sLogMsg = ""
					Local $arr_sLogMsg = StringSplit($msg[0], '%s', BitOr($STR_NOCOUNT, $STR_ENTIRESPLIT))
					_ArrayPop($arr_sLogMsg)
					For $i = 0 To (UBound($arr_sLogMsg) - 1) Step 1
						Local $tmpValue = StringStripWS(($msg[1])[$i], BitOr($STR_STRIPLEADING, $STR_STRIPTRAILING, $STR_STRIPSPACES))
						If $tmpValue == "" Then
							$tmpValue = "Undefined"
						EndIf
						$tmp_sLogMsg &= StringFormat($arr_sLogMsg[$i] & "%s" , $tmpValue)
					Next
					$sLogMsg &= $tmp_sLogMsg
				Else
					$sLogMsg &= StringFormat($msg[0], $msg[1])
				EndIf 
			Else
				$sLogMsg &= $msg
			EndIf
			Local $sPattern = '\n'
			$sLogMsg = StringRegExpReplace($sLogMsg, $sPattern, @CRLF)
			Local $arrStr = StringSplit($sLogMsg, @CRLF, $STR_ENTIRESPLIT)
			If $arrStr[0] > 1 Then
				_ArrayDelete($arrStr, 0)
				$sLogMsg = _ArrayToString($arrStr, @CRLF & @TAB & @TAB & @TAB & @TAB & @TAB & @TAB & "    ")
			EndIf
			FileWriteLine($hLogFile, $sLogMsg)
		EndIf
	EndIf
EndFunc ;==> WriteLog

Func prefGui()
	WriteLog("Enter prefGui Function", $LOG2FILE, $DBG_DEBUG)
	Local $Iipaddr = 0
	Local $Iport = 0
	Local $Iupsname = 0
	Local $Idelay = 0
	Local $tempcolor1 , $tempcolor2
	Local $result = 0
	Local $b64_IniFile

	$tempcolor2 = $clock_bkg
	$tempcolor1 = $panel_bkg
	ReadParams()
	If $guipref <> 0 Then
		GuiDelete($guipref)
		$guipref = 0
	EndIf
	$minimizetray = GetOption("minimizetray")
	If $minimizetray == 1 Then
		TraySetClick(0)
	EndIf
	$guipref = GUICreate(__("Preferences"), 364, 331, -1, -1, -1, -1, $gui)
	;$guipref = GUICreate(__("Preferences"), 364, 331, 190, 113, -1, -1, $gui)
	SetIconGuiTray($guipref)
	$Bcancel = GUICtrlCreateButton(__("Cancel"), 286, 298, 75, 25, 0)
	$Bapply = GUICtrlCreateButton(__("Apply"), 206, 298, 75, 25, 0)
	$Bok = GUICtrlCreateButton(__("Ok"), 126, 298, 75, 25, 0)
	$Tconnection = GUICtrlCreateTab(0, 0, 361, 289)
	$TSconnection = GUICtrlCreateTabItem(__("Connection"))
	GUICtrlCreateLabel(__("UPS host :"), 16, 40, 80, 17, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	$Iipaddr = GUICtrlCreateInput(GetOption("ipaddr"), 140, 37, 209, 21)
	GUICtrlCreateLabel(__("UPS port :"), 16, 72, 80, 17, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	$Iport = GUICtrlCreateInput(GetOption("port"), 276, 69, 73, 21)
	GUICtrlCreateLabel(__("UPS name :"), 16, 104, 80, 17, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	$Iupsname = GUICtrlCreateInput(GetOption("upsname"), 140, 101, 209, 21)
	GUICtrlCreateLabel(__("Delay :"), 16, 136, 80, 17, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	$Idelay = GUICtrlCreateInput(GetOption("delay"), 276, 133, 73, 21)
	GUICtrlCreateLabel(__("Re-establish connection"), 217, 256, 115, 17, Bitor($SS_RIGHT, $GUI_SS_DEFAULT_LABEL))
	$Checkbox1 = GUICtrlCreateCheckbox("ACheckbox1", 334, 256, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE )
	
	If GetOption("autoreconnect") == 0 Then
		GuiCtrlSetState($Checkbox1, $GUI_UNCHECKED)
	Else
		GuiCtrlSetState($Checkbox1 , $GUI_CHECKED)
	EndIf

	$TabSheet2 = GUICtrlCreateTabItem(__("Colors"))
	GUICtrlCreateLabel(__("Panel background color"), 16, 48, 131, 25)
	GUICtrlCreateLabel(__("Analogue background color"), 16, 106, 179, 25)
	$colorchoose1 = GUICtrlCreateLabel("", 232, 40, 25, 25, Bitor($SS_SUNKEN, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetBkColor(-1, $panel_bkg)
	$colorchoose2 = GUICtrlCreateLabel("", 232, 104, 25, 25, Bitor($SS_SUNKEN, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetBkColor(-1, $clock_bkg)
	GUICtrlCreateTabItem(__("Calibration"))
	GUICtrlCreateLabel(__("Input Voltage"), 16, 56, 120, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel(__("Frequency Supply"), 16, 96, 120, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel(__("Input Frequency"), 16, 136, 120, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel(__("Output Voltage"), 16, 176, 120, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel(__("UPS Load"), 16, 216, 120, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel(__("Battery Voltage"), 16, 256, 120, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	GUICtrlCreateLabel(__("Min"), 175, 32, 23, 19)
	GUICtrlCreateLabel(__("Max"), 255, 32, 25, 19)
	$lminInputVoltage = GUICtrlCreateInput(GetOption("mininputv"), 160, 56, 49, 23)
	$lmaxInputVoltage = GUICtrlCreateInput(GetOption("maxinputv"), 250, 56, 49, 23)
	$cbfrequencysupply = GUICtrlCreateCombo("", 250, 96, 49, 23, Bitor($CBS_DROPDOWNLIST, 0))
	$lminInputFreq = GUICtrlCreateInput(GetOption("mininputf"), 160, 136, 49, 23)
	$lmaxInputFreq = GUICtrlCreateInput(GetOption("maxinputf"), 250, 136, 49, 23)
	$lminOutputVoltage = GUICtrlCreateInput(GetOption("minoutputv"), 160, 176, 49, 23)
	$lmaxOutputVoltage = GUICtrlCreateInput(GetOption("maxoutputv"), 250, 176, 49, 23)
	$lminUpsLoad = GUICtrlCreateInput(GetOption("minupsl"), 160, 216, 49, 23)
	$lmaxUpsLoad = GUICtrlCreateInput(GetOption("maxupsl"), 250, 216, 49, 23)
	$lminBattVoltage = GUICtrlCreateInput(GetOption("minbattv"), 160, 256, 49, 23)
	$lmaxBattVoltage = GUICtrlCreateInput(GetOption("maxbattv"), 250, 256, 49, 23)
	GUICtrlSetData($cbfrequencysupply, "50.60", GetOption("frequencysupply"))

	$TabSheet1 = GUICtrlCreateTabItem(__("Misc"))
	GUICtrlCreateLabel(__("Minimize to tray"), 16, 42, 150, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chMinimizeTray = GUICtrlCreateCheckbox("MinimizeTray", 224, 39, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$lblstartminimized = GUICtrlCreateLabel(__("Start Minimized"), 16, 84, 150, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chstartminimized = GUICtrlCreateCheckbox("StartMinimized", 224, 81, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$lblclosetotray = GUICtrlCreateLabel(__("Close to Tray"), 16, 126, 150, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chclosetotray = GUICtrlCreateCheckbox("ClosetoTray", 224, 123, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)

	If GetOption("minimizetray") == 0 Then
		GuiCtrlSetState($chMinimizeTray, $GUI_UNCHECKED)
		GuiCtrlSetState($chstartminimized, $GUI_UNCHECKED)
		GuiCtrlSetState($chclosetotray, $GUI_UNCHECKED)
		GUICtrlSetState($lblstartminimized, $GUI_DISABLE)
		GUICtrlSetState($chstartminimized, $GUI_DISABLE)
		GUICtrlSetState($lblclosetotray, $GUI_DISABLE)
		GUICtrlSetState($chclosetotray, $GUI_DISABLE)
	Else
		GuiCtrlSetState($chMinimizeTray, $GUI_CHECKED)
		GUICtrlSetState($lblstartminimized, $GUI_ENABLE)
		GUICtrlSetState($chstartminimized, $GUI_ENABLE)
		GUICtrlSetState($lblclosetotray, $GUI_ENABLE)
		GUICtrlSetState($chclosetotray, $GUI_ENABLE)
		If GetOption("minimizeonstart") == 0 Then
			GuiCtrlSetState($chstartminimized, $GUI_UNCHECKED)
		Else
			GuiCtrlSetState($chstartminimized, $GUI_CHECKED)
		EndIf
		If GetOption("closetotray") == 0 Then
			GuiCtrlSetState($chclosetotray, $GUI_UNCHECKED)
		Else
			GuiCtrlSetState($chclosetotray, $GUI_CHECKED)
		EndIf
	EndIf

	$lblstartwithwindows = GUICtrlCreateLabel(__("Start with Windows"), 16, 168, 150, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chStartWithWindows = GUICtrlCreateCheckbox("Startwithwindows", 224, 167, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$lblCreateLogFile = GUICtrlCreateLabel(__("Create LogFile"), 16, 210, 150, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chCreateLogFile = GUICtrlCreateCheckbox("UseLogFile", 224, 209, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$BtnViewLogFile = GUICtrlCreateButton(__("View LogFile"), 248, 198, 48, 40, $BS_ICON)
	GUICtrlSetImage($BtnViewLogFile, $IconDLL, $IDX_ICO_VIEWLOG)
	GUICtrlSetTip ($BtnViewLogFile, __("View LogFile"), null, $TIP_NOICON, $TIP_BALLOON)
	$BtnDeleteLogFile = GUICtrlCreateButton(__("Delete LogFile"), 302, 198, 48, 40, $BS_ICON)
	GUICtrlSetImage($BtnDeleteLogFile, $IconDLL, $IDX_ICO_DELETELOG)
	GUICtrlSetTip ($BtnDeleteLogFile, __("Delete LogFile"), null, $TIP_NOICON, $TIP_BALLOON)
	$lbLogLevel = GUICtrlCreateLabel(__("Logging Level"), 16, 252, 150, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$cbLogLevel = GUICtrlCreateCombo("", 224, 252, 126, 23, Bitor($CBS_DROPDOWNLIST, 0))

	If GetOption("uselogfile") == 0 Then
		GuiCtrlSetState($chCreateLogFile, $GUI_UNCHECKED)
	Else
		GuiCtrlSetState($chCreateLogFile, $GUI_CHECKED)
	EndIf
	If $oDBG_LEVEL.Count > 0 Then
		$oDBG_LEVEL.RemoveAll
	EndIf
	$oDBG_LEVEL.Add($DBG_NOTICE, __("Notice"))
	$oDBG_LEVEL.Add($DBG_WARNING, __("Warning"))
	$oDBG_LEVEL.Add($DBG_ERROR, __("Error"))
	$oDBG_LEVEL.Add($DBG_DEBUG, __("Debug"))
	Local $cbLogLevelstr = ""
	For $vKey In $oDBG_LEVEL
		$cbLogLevelstr &= $oDBG_LEVEL.Item($vKey) & "."
	Next

	GUICtrlSetData($cbLogLevel, $cbLogLevelstr, $oDBG_LEVEL.Item(GetOption("loglevel")))

	If FileExists($LogFile) Then
		GUICtrlSetState($BtnViewLogFile, $GUI_ENABLE)
		GUICtrlSetState($BtnDeleteLogFile, $GUI_ENABLE)
	Else
		GUICtrlSetState($BtnViewLogFile, $GUI_DISABLE)
		GUICtrlSetState($BtnDeleteLogFile, $GUI_DISABLE)
	EndIf

	If $runasexe == True Then
		GUICtrlSetState($chStartWithWindows, $GUI_ENABLE)
		GUICtrlSetState($lblstartwithwindows, $GUI_ENABLE)
		If GetOption("startwithwindows") == 0 Then
			GuiCtrlSetState($chStartWithWindows, $GUI_UNCHECKED)
		Else
			GuiCtrlSetState($chStartWithWindows, $GUI_CHECKED)
		EndIf
	Else
		GuiCtrlSetState($chStartWithWindows, $GUI_UNCHECKED)
		GUICtrlSetState($lblstartwithwindows, $GUI_DISABLE)
		GUICtrlSetState($chStartWithWindows, $GUI_DISABLE)
	EndIf

	$TSShutdown = GUICtrlCreateTabItem(__("Shutdown Options"))
	GUICtrlCreateLabel(__("Shutdown if battery lower than"), 16, 39, 179, 34, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$lshutdownpcbatt = GUICtrlCreateInput(GetOption("shutdownpcbatt"), 217, 36, 25, 23)
	GUICtrlCreateLabel("%", 248, 39, 15, 19)
	GUICtrlCreateLabel(__("Shutdown if runtime lower than (sec)"), 16, 81, 179, 34, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$lshutdownrtime = GUICtrlCreateInput(GetOption("shutdownpctime"), 217, 81, 40, 23)
	$lblInstantAction = GUICtrlCreateLabel(__("Immediate stop action"), 16, 123, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chInstantAction = GUICtrlCreateCheckbox("Immediate stop action", 224, 122, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$lblActionType = GUICtrlCreateLabel(__("Type of Stop"), 16, 153, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$cbActionType = GUICtrlCreateCombo("", 200, 150, 150, 17, Bitor($CBS_DROPDOWNLIST, 0))
	$lbldelayshutdown = GUICtrlCreateLabel(__("Delay to Shutdown"), 16, 185, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$ldelayshutdown = GUICtrlCreateInput(GetOption("ShutdownDelay"), 217, 182, 40, 23)
	$lblAllowGrace = GUICtrlCreateLabel(__("Allow Extended Shutdown Time"), 16, 227, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chAllowGrace = GUICtrlCreateCheckbox("AllowExtendedShutdownTime", 224, 226, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$lbldelaygrace = GUICtrlCreateLabel(__("Grace Delay to Shutdown"), 16, 254, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$ldelaygrace = GUICtrlCreateInput(GetOption("GraceDelay"), 217, 251, 40, 23)
	Switch GetOption("TypeOfStop")
		Case 17
			$cbTypeOfStopValue = __("Shutdown")
		Case 32
			$cbTypeOfStopValue = __("Sleep")
		Case 64
			$cbTypeOfStopValue = __("Hibernate")
		Case Else
			$cbTypeOfStopValue = __("Shutdown")
	EndSwitch
	$listItemActionType = __("Shutdown") & "." & __("Sleep") & "." & __("Hibernate")
	GUICtrlSetData($cbActionType, $listItemActionType, $cbTypeOfStopValue)
	If GetOption("InstantAction") == 0 Then
		GuiCtrlSetState($chInstantAction, $GUI_UNCHECKED)
		GUICtrlSetState($lbldelayshutdown, $GUI_ENABLE)
		GUICtrlSetState($ldelayshutdown, $GUI_ENABLE)
	Else
		GuiCtrlSetState($chInstantAction, $GUI_CHECKED)
		GUICtrlSetState($lbldelayshutdown, $GUI_DISABLE)
		GUICtrlSetState($ldelayshutdown, $GUI_DISABLE)
	EndIf
	If GetOption("AllowGrace") == 0 Then
		GuiCtrlSetState($chAllowGrace, $GUI_UNCHECKED)
		GUICtrlSetState($lbldelaygrace,$GUI_DISABLE)
		GUICtrlSetState($ldelaygrace, $GUI_DISABLE)
	Else
		GuiCtrlSetState($chAllowGrace, $GUI_CHECKED)
		GUICtrlSetState($lbldelaygrace, $GUI_ENABLE)
		GUICtrlSetState($ldelaygrace, $GUI_ENABLE)
	EndIf

	$TSUpdate = GUICtrlCreateTabItem(__("Update"))
	$lblVerifUpdate = GUICtrlCreateLabel(__("Verify Update"), 16, 39, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chVerifUpdate = GUICtrlCreateCheckbox("Verify Update", 224, 38, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$lblVerifAtStart = GUICtrlCreateLabel(__("Verify Update At Start"), 16, 81, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$chVerifAtStart = GUICtrlCreateCheckbox("Verify Update At Start", 224, 80, 17, 17, BitOr($BS_AUTOCHECKBOX, $WS_TABSTOP), $WS_EX_STATICEDGE)
	$lblDelayVerif = GUICtrlCreateLabel(__("Delay Between Verification"), 16, 123, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$cbDelayVerif = GUICtrlCreateCombo("", 200, 124, 150, 17, Bitor($CBS_DROPDOWNLIST, 0))
	$lblBranchVerif = GUICtrlCreateLabel(__("Stable Or Dev Branch"), 16, 165, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
	$cbBranchVerif = GUICtrlCreateCombo("", 200, 166, 150, 17, Bitor($CBS_DROPDOWNLIST, 0))

	If GetOption("VerifyUpdate") == 0 Then
		GuiCtrlSetState($chVerifUpdate, $GUI_UNCHECKED)
		GuiCtrlSetState($chVerifAtStart, $GUI_DISABLE)
		GuiCtrlSetState($cbDelayVerif, $GUI_DISABLE)
		GuiCtrlSetState($cbBranchVerif, $GUI_DISABLE)
	Else
		GuiCtrlSetState($chVerifUpdate, $GUI_CHECKED)
		GuiCtrlSetState($chVerifAtStart, $GUI_ENABLE)
		GuiCtrlSetState($cbDelayVerif, $GUI_ENABLE)
		GuiCtrlSetState($cbBranchVerif, $GUI_ENABLE)
	EndIf
	If GetOption("VerifyUpdateAtStart") == 0 Then
		GuiCtrlSetState($chVerifAtStart, $GUI_UNCHECKED)
	Else
		GuiCtrlSetState($chVerifAtStart, $GUI_CHECKED)
	EndIf
	If $oUPDATE_BRANCH.Count > 0 Then
		$oUPDATE_BRANCH.RemoveAll
	EndIf
	If $oUPDATE_DELAY.Count > 0 Then
		$oUPDATE_DELAY.RemoveAll
	EndIf
	$oUPDATE_DELAY.Add(1, __("Daily"))
	$oUPDATE_DELAY.Add(2, __("Weekly"))
	$oUPDATE_DELAY.Add(3, __("Monthly"))
	Local $cbDelayVerifstr = ""
	For $vKey In $oUPDATE_DELAY
		$cbDelayVerifstr &= $oUPDATE_DELAY.Item($vKey) & "."
	Next
	GUICtrlSetData($cbDelayVerif, $cbDelayVerifstr, $oUPDATE_DELAY.Item(GetOption("DelayVerif")))
	$oUPDATE_BRANCH.Add(1, __("Stable"))
	$oUPDATE_BRANCH.Add(2, __("Devellopement"))
	Local $cbBranchVerifstr = ""
	For $vKey In $oUPDATE_BRANCH
		$cbBranchVerifstr &= $oUPDATE_BRANCH.Item($vKey) & "."
	Next
	GUICtrlSetData($cbBranchVerif, $cbBranchVerifstr, $oUPDATE_BRANCH.Item(GetOption("VerifyBranch")))

	GUICtrlCreateTabItem("")
	GuiSetState(@SW_DISABLE, $gui)
	GUISetState(@SW_SHOW, $guipref)
	WriteLog("Pref Gui hWnd : " & $guipref, $LOG2FILE, $DBG_DEBUG)
	WriteLog("creation of prefGui is complete", $LOG2FILE, $DBG_DEBUG)
	$b64_IniFile = _Base64Encode(FileRead($inipath, -1))

	While 1
		$nMsg1 = GUIGetMsg()
		Switch $nMsg1
			Case $GUI_EVENT_CLOSE
				WriteLog("prefGui Closed", $LOG2FILE, $DBG_DEBUG)
				ExitLoop
			Case $Bapply, $Bok
				WriteLog("prefGui Apply/OK", $LOG2FILE, $DBG_DEBUG)
				SetOption("ipaddr", GuiCtrlRead($Iipaddr), "string")
				SetOption("upsname", GuiCtrlRead($Iupsname), "string")
				SetOption("delay", GuiCtrlRead($Idelay), "number")
				$AutoReconnectCB = GuiCtrlRead($Checkbox1)
				If $AutoReconnectCB == $GUI_CHECKED Then
					SetOption("autoreconnect", 1, "number")
				Else
					SetOption("autoreconnect", 0, "number")
				EndIf

				SetOption("mininputv", GuiCtrlRead($lminInputVoltage), "number")
				SetOption("maxinputv", GuiCtrlRead($lmaxInputVoltage), "number")
				SetOption("minoutputv", GuiCtrlRead($lminOutputVoltage), "number")
				SetOption("maxoutputv", GuiCtrlRead($lmaxOutputVoltage), "number")
				SetOption("frequencysupply", GuiCtrlRead($cbfrequencysupply), "number")
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
				$InstantAction = GuiCtrlRead($chInstantAction)
				If $InstantAction == $GUI_CHECKED Then
					SetOption("InstantAction", 1, "number")
				Else
					SetOption("InstantAction", 0, "number")
				EndIf
				Switch GuiCtrlRead($cbActionType)
					Case __("Shutdown")
						$TypeOfStop = 17
					Case __("Sleep")
						$TypeOfStop = 32
					Case __("Hibernate")
						$TypeOfStop = 64
					Case Else
						$TypeOfStop = 17
				EndSwitch
				SetOption("TypeOfStop", $TypeOfStop, "number")
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
				If GetOption("AllowGrace") == 0 Then
					GuiCtrlSetState($chAllowGrace, $GUI_UNCHECKED)
					GUICtrlSetState($lbldelaygrace, $GUI_DISABLE)
					GUICtrlSetState($ldelaygrace, $GUI_DISABLE)
				Else
					GuiCtrlSetState($chAllowGrace, $GUI_CHECKED)
					GUICtrlSetState($lbldelaygrace, $GUI_ENABLE)
					GUICtrlSetState($ldelaygrace, $GUI_ENABLE)
				EndIf
				$minimizetray = GuiCtrlRead($chMinimizeTray)
				If $minimizetray == $GUI_CHECKED Then
					SetOption("minimizetray", 1, "number")
					$startminimized = GuiCtrlRead($chstartminimized)
					If $startminimized == $GUI_CHECKED Then
						SetOption("minimizeonstart", 1, "number")
					Else
						SetOption("minimizeonstart", 0, "number")
					EndIf
					$closetotray = GuiCtrlRead($chclosetotray)
					If $closetotray == $GUI_CHECKED Then
						SetOption("closetotray", 1, "number")
					Else
						SetOption("closetotray", 0, "number")
					EndIf
				Else
					SetOption("minimizetray", 0, "number")
					SetOption("minimizeonstart", 0, "number")
					SetOption("closetotray", 0, "number")
				EndIf
				$UseLogFile = GuiCtrlRead($chCreateLogFile)
				If $UseLogFile == $GUI_CHECKED Then
					SetOption("uselogfile", 1, "number")
					If $hLogFile == null Then
						If Not FileExists($LogFile) Then
							_FileCreate($LogFile)
						EndIf
						$hLogFile = FileOpen($LogFile, $FO_APPEND)
					EndIf
					WriteLog($START_LOG_STR, $LOG2FILE, $DBG_NOTICE)
					GUICtrlSetState($BtnViewLogFile, $GUI_ENABLE)
					GUICtrlSetState($BtnDeleteLogFile, $GUI_ENABLE)
					AdlibRegister("WriteLogToDisk", $DELAY_WRITE_LOG)
				Else
					SetOption("uselogfile", 0, "number")
					WriteLogToDisk()
					FileClose($hLogFile)
					$hLogFile = null
					AdlibUnRegister("WriteLogToDisk")
				EndIf
				Local $LogLevelValue = GuiCtrlRead($cbLogLevel)
				For $vKey In $oDBG_LEVEL
					If $LogLevelValue == $oDBG_LEVEL.Item($vKey) Then
						SetOption("loglevel", $vKey, "number")
       				EndIf
    			Next
				If $runasexe == True Then
					$startwithwindows = GuiCtrlRead($chStartWithWindows)
					$linkexe = @StartupDir & "\Upsclient.lnk"
					if $startwithwindows == $GUI_CHECKED Then
						SetOption("startwithwindows", 1, "number")
						if FileExists($linkexe) == 0 Then
							FileCreateShortcut(@ScriptFullPath, $linkexe)
						EndIf
					Else
						if FileExists($linkexe) <> 0 Then
							FileDelete($linkexe)
						EndIf
						SetOption("startwithwindows", 0, "number")
					EndIf
				Else
					SetOption("startwithwindows", 0, "number")
				EndIf
				$VerifyUpdateCh = GuiCtrlRead($chVerifUpdate)
				If $VerifyUpdateCh == $GUI_CHECKED Then
					SetOption("VerifyUpdate", 1, "number")
				Else
					SetOption("VerifyUpdate", 0, "number")
				EndIf
				$VerifyUpdateAtStartCh = GuiCtrlRead($chVerifAtStart)
				If $VerifyUpdateAtStartCh == $GUI_CHECKED Then
					SetOption("VerifyUpdateAtStart", 1, "number")
				Else
					SetOption("VerifyUpdateAtStart", 0, "number")
				EndIf
				Switch GuiCtrlRead($cbDelayVerif)
					Case __("Daily")
						$DelayVerifcb = 1
					Case __("Weekly")
						$DelayVerifcb = 2
					Case __("Monthly")
						$DelayVerifcb = 3
					Case Else
						$DelayVerifcb = 3
				EndSwitch
				SetOption("DelayVerif", $DelayVerifcb, "number")
				Switch GuiCtrlRead($cbBranchVerif)
					Case __("Stable")
						$TypeofBranchcb = 1
					Case __("Devellopement")
						$TypeofBranchcb = 2
					Case Else
						$TypeofBranchcb = 1
				EndSwitch
				SetOption("VerifyBranch", $TypeofBranchcb, "number")
				WriteParams()
				setTrayMode()
				AdlibUnregister("GetUPSData")
				AdlibRegister("GetUPSData", GetOption("delay"))
				If $nMsg1 == $Bok Then
					$panel_bkg = $tempcolor1
					$clock_bkg = $tempcolor2
					$clock_bkg_bgr = RGBtoBGR($clock_bkg)
					GuiSetBkColor($clock_bkg, $dial1)
					GuiSetBkColor($clock_bkg, $dial2)
					GuiSetBkColor($clock_bkg, $dial3)
					GuiSetBkColor($clock_bkg, $dial4)
					GuiSetBkColor($clock_bkg, $dial5)
					GuiSetBkColor($clock_bkg, $dial6)
					GUISetBkColor($panel_bkg,  $wPanel)
					WriteLog("prefGui Closed - OK Button", $LOG2FILE, $DBG_DEBUG)
					ExitLoop
				EndIf
			Case $Bcancel
				WriteLog("prefGui Canceled", $LOG2FILE, $DBG_DEBUG)
				ExitLoop
			Case $colorchoose1
				WriteLog("prefGui Choose Color1", $LOG2FILE, $DBG_DEBUG)
				$tempcolor1 = _ChooseColor(2, 0, 2)
				If $tempcolor1 <> -1 Then
					GuiCtrlSetBkColor($colorchoose1, $tempcolor1)
				Else
					$tempcolor1 = $panel_bkg
				EndIf
			Case $colorchoose2
				WriteLog("prefGui Choose Color2", $LOG2FILE, $DBG_DEBUG)
				$tempcolor2 = _ChooseColor(2, 0, 2)
				If $tempcolor2 <> -1 Then
					GuiCtrlSetBkColor($colorchoose2, $tempcolor2)
				Else
					$tempcolor2 = $clock_bkg
				EndIf
			Case $chMinimizeTray
				WriteLog("prefGui Change Minimize To Tray", $LOG2FILE, $DBG_DEBUG)
				$minimizetray = GuiCtrlRead($chMinimizeTray)
				If $minimizetray == $GUI_CHECKED Then
					GUICtrlSetState($lblstartminimized, $GUI_ENABLE)
					GUICtrlSetState($chstartminimized, $GUI_ENABLE)
					GUICtrlSetState($lblclosetotray, $GUI_ENABLE)
					GUICtrlSetState($chclosetotray, $GUI_ENABLE)
				Else
					GuiCtrlSetState($chstartminimized, $GUI_UNCHECKED)
					GuiCtrlSetState($chclosetotray, $GUI_UNCHECKED)
					GUICtrlSetState($lblstartminimized, $GUI_DISABLE)
					GUICtrlSetState($chstartminimized, $GUI_DISABLE)
					GUICtrlSetState($lblclosetotray, $GUI_DISABLE)
					GUICtrlSetState($chclosetotray, $GUI_DISABLE)
				EndIf
			Case $chInstantAction
				WriteLog("prefGui Change Instant Action", $LOG2FILE, $DBG_DEBUG)
				$InstantAction = GuiCtrlRead($chInstantAction)
				If $InstantAction == $GUI_CHECKED Then
					GUICtrlSetState($lbldelayshutdown, $GUI_DISABLE)
					GUICtrlSetState($ldelayshutdown, $GUI_DISABLE)
				Else
					GUICtrlSetState($lbldelayshutdown, $GUI_ENABLE)
					GUICtrlSetState($ldelayshutdown, $GUI_ENABLE)
				EndIf
			Case $chAllowGrace
				WriteLog("prefGui Change Allow Grace", $LOG2FILE, $DBG_DEBUG)
				$AllowGrace = GuiCtrlRead($chAllowGrace)
				If $AllowGrace == $GUI_CHECKED Then
					GUICtrlSetState($lbldelaygrace, $GUI_ENABLE)
					GUICtrlSetState($ldelaygrace, $GUI_ENABLE)
				Else
					GUICtrlSetState($lbldelaygrace, $GUI_DISABLE)
					GUICtrlSetState($ldelaygrace, $GUI_DISABLE)
				EndIf
			Case $chCreateLogFile
				If FileExists($LogFile) Then
					GUICtrlSetState($BtnViewLogFile, $GUI_ENABLE)
					GUICtrlSetState($BtnDeleteLogFile, $GUI_ENABLE)
				Else
					GUICtrlSetState($BtnViewLogFile, $GUI_DISABLE)
					GUICtrlSetState($BtnDeleteLogFile, $GUI_DISABLE)
				EndIf
			Case $BtnViewLogFile
				WriteLog("prefGui View Log File", $LOG2FILE, $DBG_DEBUG)
				If FileExists($LogFile) Then
					WriteLogToDisk()
					Run("notepad.exe " & _PathFull($LogFile), @WindowsDir)
				Else
					GUICtrlSetState($BtnViewLogFile, $GUI_DISABLE)
					GUICtrlSetState($BtnDeleteLogFile, $GUI_DISABLE)
				EndIf
			Case $BtnDeleteLogFile
				WriteLog("prefGui Delete Log File", $LOG2FILE, $DBG_DEBUG)
				FileClose($hLogFile)
				FileDelete($LogFile)
				If GetOption("uselogfile") == 1 Then
					WriteLog($START_LOG_STR, $LOG2FILE, $DBG_NOTICE)
					AdlibRegister("WriteLogToDisk", $DELAY_WRITE_LOG)
					GUICtrlSetState($BtnViewLogFile, $GUI_ENABLE)
					GUICtrlSetState($BtnDeleteLogFile, $GUI_ENABLE)
				Else
					GUICtrlSetState($BtnViewLogFile, $GUI_DISABLE)
					GUICtrlSetState($BtnDeleteLogFile, $GUI_DISABLE)
				EndIf
			Case $chVerifUpdate
				If GuiCtrlRead($chVerifUpdate) == $GUI_CHECKED Then
					GuiCtrlSetState($chVerifAtStart, $GUI_ENABLE)
					GuiCtrlSetState($cbDelayVerif, $GUI_ENABLE)
					GuiCtrlSetState($cbBranchVerif, $GUI_ENABLE)
				Else
					GuiCtrlSetState($chVerifAtStart, $GUI_DISABLE)
					GuiCtrlSetState($cbDelayVerif, $GUI_DISABLE)
					GuiCtrlSetState($cbBranchVerif, $GUI_DISABLE)
				EndIf
		EndSwitch
	WEnd
	If $minimizetray == 1 Then
		TraySetClick(8)
	EndIf
	GuiDelete($guipref)
	If (WinGetState($gui) <> 17 ) Then
		GuiSetState(@SW_ENABLE, $gui)
		WinActivate($gui)
	EndIf
	If $b64_IniFile <> _Base64Encode(FileRead($inipath, -1)) Then
		$result = 1
		If IsFQDN(GuiCtrlRead($Iipaddr)) Then
			$ResolvedHost = ResolveFQDN(GuiCtrlRead($Iipaddr))
		EndIf
		If IsIPV4($ResolvedHost) Then
			$ipv6mode = False
		ElseIf IsIPV6($ResolvedHost) Then
			WriteLog("Switch to IPV6 Mode")
			$ipv6mode = True
		EndIf
	EndIf
	$guipref = 0
	Return $result
EndFunc ;==> prefGui

Func OpenMainWindow()
	WriteLog("Enter OpenMainWindow Function", $LOG2FILE, $DBG_DEBUG)
	Local $aLanguageList = _i18n_GetLocaleList()
	Local $Reload = False
	Local $sLogText[0]
	If $gui <> 0 Then
		For $i = 0 To (_GUICtrlComboBox_GetCount($log) - 1) Step 1
			ReDim $sLogText[UBound($sLogText) + 1] 
			_GUICtrlComboBoxEx_GetItemText($log, $i, $sLogText[$i])
		Next
		GUIDelete($gui)
		$Reload = True
		$gui = GUICreate($ProgramDesc, 640, 380, -1 , -1, Bitor($GUI_SS_DEFAULT_GUI, $WS_CLIPCHILDREN))
	Else
		$gui = GUICreate($ProgramDesc, 640, 380, -1 , -1, Bitor($GUI_SS_DEFAULT_GUI, $WS_CLIPCHILDREN))
	EndIf
	
	SetIconGuiTray()
	$fileMenu = GUICtrlCreateMenu("&" & __("File"))
	$listvarMenu = GuiCtrlCreateMenuItem("&" & __("List UPS Vars"), $fileMenu)
	$exitMenu = GUICtrlCreateMenuItem("&" & __("Exit"), $fileMenu)
	$editMenu = GUICtrlCreateMenu("&" & __("Connection"))
	$reconnectMenu = GUICtrlCreateMenuItem("&" & __("Reconnect"), $editMenu)
	$DisconnectMenu = GUICtrlCreateMenuItem("&" & __("Disconnect"), $editMenu)
	$settingsMenu = GUICtrlCreateMenu("&" & __("Settings"))
	$settingssubMenu = GUICtrlCreateMenuItem("&" & __("Preferences"), $settingsMenu)
	$LanguageSettings = GUICtrlCreateMenu("&" & __("Language"), $settingsMenu)
	$LangSubMenuSystem = GUICtrlCreateMenuItem("&" & __("System"), $LanguageSettings)
	If Ubound($aLanguageList) > 1 Then
		; Create dictionary object
		$MenuLangListhwd = ObjCreate("Scripting.Dictionary")
		$MenuLangListhwd.Add('system', $LangSubMenuSystem)
		GUICtrlCreateMenuItem("", $LanguageSettings)
		If Not @error Then
			For $l = 1 To $aLanguageList[0]
				Local $langid = _StringBetween($aLanguageList[$l], '(', ')', $STR_ENDNOTSTART, False)
			    $MenuLangListhwd.Add($langid[0], GUICtrlCreateMenuItem("&" & $aLanguageList[$l], $LanguageSettings))
			Next
		EndIf
	EndIf
	Local $ActualLang = GetOption("language")
	For $vKey In $MenuLangListhwd
		If $ActualLang == $vKey Then
			GUICtrlSetState($MenuLangListhwd.Item($vKey), $GUI_CHECKED)
		Else
			GUICtrlSetState($MenuLangListhwd.Item($vKey), $GUI_UNCHECKED)
		EndIf
	Next
	If  $ActualLang == 'system' Then
		GUICtrlSetState($LangSubMenuSystem, $GUI_CHECKED)
	Else
		GUICtrlSetState($LangSubMenuSystem, $GUI_UNCHECKED)
	EndIf
	$helpMenu = GUICtrlCreateMenu("&" & __("Help"))
	$aboutMenu = GUICtrlCreateMenuItem(__("About"), $helpMenu)
	GUICtrlCreateMenuItem("", $helpMenu)
	$updateMenu = GUICtrlCreateMenuItem(__("Check Update"), $helpMenu)

	$log = GUICtrlCreateCombo("", 5, 335, 630, 25, Bitor($CBS_DROPDOWNLIST, 0))
	$wPanel = GUICreate("", 150, 250,0, 70, BitOR($WS_CHILD, $WS_DLGFRAME), $WS_EX_CLIENTEDGE, $gui)
	GUISetBkColor($panel_bkg, $wPanel)
	$Label1 = GUICtrlCreateLabel(__("UPS On Line"), 8, 8, 110, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsonline = GUICtrlCreateLabel("", 121, 6, 16, 16, BitOR($SS_CENTER, $SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$Label2 = GUICtrlCreateLabel(__("UPS On Battery"), 8, 28, 110, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsonbatt = GUICtrlCreateLabel("", 121, 26, 16, 16, BitOR($SS_CENTER, $SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$Label3 = GUICtrlCreateLabel(__("UPS Overload"), 8, 48, 110, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsoverload = GUICtrlCreateLabel("", 121, 46, 16, 16, BitOR($SS_CENTER, $SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$Label4 = GUICtrlCreateLabel(__("UPS Battery low"), 8, 68, 110, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upslowbatt = GUICtrlCreateLabel("", 121, 66, 16, 16, BitOR($SS_CENTER, $SS_SUNKEN))
	GUICtrlSetBkColor(-1, $gray)
	$labelUpsRemain = GUICtrlCreateLabel(__("Remaining Time"), 8, 88, 110, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	$remainTimeLabel = GUICtrlCreateLabel($battrtimeStr, 8, 104, 130, 16, Bitor($SS_RIGHT, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label5 = GUICtrlCreateLabel(__("Manufacturer :"), 8, 122, 130, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsmfr = GUICtrlCreateLabel($mfr, 8, 138, 130, 16, Bitor($SS_RIGHT, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label14 = GUICtrlCreateLabel(__("Name :"), 8, 154, 130, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsmodel = GUICtrlCreateLabel($name, 8, 170, 130, 16, Bitor($SS_RIGHT, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label15 = GUICtrlCreateLabel(__("Serial :"), 8, 186, 130, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsserial = GUICtrlCreateLabel($serial, 8, 202, 130, 16, Bitor($SS_RIGHT, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")
	$Label16 = GUICtrlCreateLabel(__("Firmware :"), 8, 218, 130, 16, Bitor($SS_LEFTNOWORDWRAP, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 400, 0, "MS SansSerif")
	$upsfirmware = GUICtrlCreateLabel($firmware, 8, 234, 130, 16, Bitor($SS_RIGHT, $GUI_SS_DEFAULT_LABEL))
	GUICtrlSetFont(-1, 8, 800, 0, "MS SansSerif")

	$Group8 = GUICreate("", 638, 60, 0, 0, BitOR($WS_CHILD, $WS_BORDER), 0, $gui)
	$exitb = GUICtrlCreateButton(__("Exit"), 10, 10, 73, 40, 0)
	$toolb = GUICtrlCreateButton(__("Settings"), 102, 10, 73, 40, 0)
	$calc = 1 / ((GetOption("maxinputv") - GetOption("mininputv")) / 100 )
	$dial1 = DrawDial(160, 70, GetOption("mininputv"), __("Input Voltage"), "V", $inputv, $needle1, null, null, $calc, 20, 70)
	$calc = 1 / ((GetOption("maxoutputv") - GetOption("minoutputv")) / 100 )
	$dial2 = DrawDial(480, 70, GetOption("minoutputv"), __("Output Voltage"), "V", $outputv, $needle2, null, null, $calc, 20, 70)
	$calc = 1 / ((GetOption("maxinputf") - GetOption("mininputf")) / 100 )
	$dial3 = DrawDial(320, 70, GetOption("mininputf"), __("Input Frequency"), "Hz", $inputf, $needle3, null, null, $calc, 20, 70)
	$calc = 1 / ((GetOption("maxbattv") - GetOption("minbattv")) / 100 )
	$dial4 = DrawDial(480, 200, GetOption("minbattv"), __("Battery Voltage"), "V", $battv, $needle4, null, null, $calc, 20, 120)
	$calc = 1 / ((GetOption("maxupsl") - GetOption("minupsl")) / 100 )
	$dial5 = DrawDial(320, 200, 0, __("UPS Load"), "%", $upsl, $needle5, $realoutpower, "W", $calc, -1, 80)
	$dial6 = DrawDial(160, 200, 0, __("Battery Charge"), "%", $upsch, $needle6, null, null, 1, 30, 101)
	GUISetState(@SW_SHOW, $Group8)
	GUISetState(@SW_SHOW, $wPanel)
	If $Reload Then
		GUISetState(@SW_SHOW, $gui)
		For $i = 0 To (UBound($sLogText) - 1) Step 1
			ControlCommand($gui, "", $log, "AddString", $sLogText[$i])
		Next
		ControlCommand($gui, "", $log, "SetCurrentSelection", _GUICtrlComboBox_GetCount($log) - 1)
		GuiRegisterMsg(0x000F, "rePaint")
		GuiRegisterMsg($WM_ACTIVATE, "On_WM_ACTIVATE")
		$Reload = False
	EndIf
	GuiSwitch($gui)
	WriteLog("Main Gui hWnd : " & $gui, $LOG2FILE, $DBG_DEBUG)
	WriteLog("creation of MainWindow is complete", $LOG2FILE, $DBG_DEBUG)
EndFunc ;==> OpenMainWindow

Func aboutGui()
	WriteLog("Enter aboutGui Function", $LOG2FILE, $DBG_DEBUG)
	$minimizetray = GetOption("minimizetray")
	If $minimizetray == 1 Then
		TraySetClick(0)
	EndIf
	$guiabout = GUICreate(__("About"), 340, 240, 271, 178)
	SetIconGuiTray($guiabout)
	$GroupBox1 = GUICtrlCreateGroup("", 8, 0, 324, 204)
	$Image1 = GUICtrlCreatePic(@ScriptDir & "\Resources\ups.jpg", 16, 16, 104, 104, BitOR($SS_NOTIFY, $WS_GROUP))
	$Label10 = GUICtrlCreateLabel($ProgramDesc, 128, 16, 180 , 18, $WS_GROUP)
	$Label11 = GUICtrlCreateLabel(__("Version ") & $ProgramVersion, 128, 34, 180, 18, $WS_GROUP)
	$Label12 = GUICtrlCreateLabel("Copyright Michael Liberman" & @LF & "2006-2007", 128, 52, 200, 44, $WS_GROUP)
	$Label12 = GUICtrlCreateLabel("Copyright Gawindx (Decaux Nicolas)" & @LF & "2019-" & @YEAR, 128, 88, 200, 44, $WS_GROUP)
	$Label13 = GUICtrlCreateLabel("Based from Winnut Sf https://sourceforge.net/projects/winnutclient/", 16, 128, 270, 40, $WS_GROUP)
	$Label13 = GUICtrlCreateLabel("Source Available from GitHub https://github.com/gawindx/WinNUT-Client", 16, 160, 270, 40, $WS_GROUP)
	GUICtrlCreateGroup("", -99, -99, 1, 1)
	$AboutBtnOk = GUICtrlCreateButton("&OK", 134, 208, 72, 24)
	GUISetState(@SW_SHOW,$guiabout)
	GuiSetState(@SW_DISABLE,$gui)
	WriteLog("about Gui hWnd : " & $guiabout, $LOG2FILE, $DBG_DEBUG)
	WriteLog("Creation of aboutGui is complete", $LOG2FILE, $DBG_DEBUG)
	While 1
		$nMsg2 = GUIGetMsg()
		Switch $nMsg2
			Case $GUI_EVENT_CLOSE, $AboutBtnOk
				WriteLog("Close aboutGui - Close/Ok Button", $LOG2FILE, $DBG_DEBUG)
				GuiDelete($guiabout)
				$guiabout = 0
				If (WinGetState($gui) <> 17 ) Then
					GuiSetState(@SW_ENABLE, $gui)
					WinActivate($gui)
					WriteLog("Reactivate Main Gui", $LOG2FILE, $DBG_DEBUG)
				EndIf
				If $minimizetray == 1 Then
					TraySetClick(8)
				EndIf
				WriteLog("aboutGui Closed", $LOG2FILE, $DBG_DEBUG)
				ExitLoop
		EndSwitch
	WEnd
EndFunc ;==> aboutGui

Func updateGui()
	WriteLog("Enter updateGui Function", $LOG2FILE, $DBG_DEBUG)
	$minimizetray = GetOption("minimizetray")
	If $minimizetray == 1 Then
		TraySetClick(0)
	EndIf
	If $sChangeLog <> null Then
		$guiupdate = GUICreate(__("Update"), 240, 118, -1, -1)
		SetIconGuiTray($guiupdate)
		$GroupBox1 = GUICtrlCreateGroup("", 8, 0, 224, 82)
		$Label10 = GUICtrlCreateLabel(StringFormat(__("Current version of WinNut : %s"), $ProgramVersion), 30, 16, 180 , 18, BitOr($WS_GROUP, $SS_CENTER))
		$Label11 = GUICtrlCreateLabel(StringFormat(__("New Version Available : %s"), $HighestVersion), 30, 34, 180, 18, BitOr($WS_GROUP, $SS_CENTER))
		$EditChLog = GUICtrlCreateEdit($sChangeLog, 16, 52, 224, 1, BitOr($WS_GROUP, $ES_AUTOVSCROLL, $WS_VSCROLL))
		$DownloadButton = GUICtrlCreateButton("&" & __("Downlad New Version"), 16, 52, 208, 24)
		GUICtrlSetState($EditChLog, $GUI_HIDE)
		GUICtrlCreateGroup("", -99, -99, 1, 1)
		$UpdateBtnOk = GUICtrlCreateButton("&OK", 18, 86, 92, 24)
		$showLogBtn = GUICtrlCreateButton("&" & __("Show Changelog"), 130, 86, 92, 24)
		GUISetState(@SW_SHOW, $guiupdate)
		GuiSetState(@SW_DISABLE,$gui)
		WriteLog("Creation of updateGui is complete with Update Available", $LOG2FILE, $DBG_DEBUG)
	Else
		$guiupdate = GUICreate(__("Update"), 240, 90, -1, -1)
		SetIconGuiTray($guiupdate)
		$GroupBox1 = GUICtrlCreateGroup("", 8, 0, 224, 54)
		$Label10 = GUICtrlCreateLabel(StringFormat(__("No Update Available"), $ProgramVersion), 30, 16, 180 , 18, $WS_GROUP)
		GUICtrlCreateGroup("", -99, -99, 1, 1)
		$AboutBtnOk = GUICtrlCreateButton("&OK", 84, 58, 72, 24)
		GUISetState(@SW_SHOW, $guiupdate)
		GuiSetState(@SW_DISABLE, $gui)
		WriteLog("Update Gui hWnd : " & $guiupdate, $LOG2FILE, $DBG_DEBUG)
		WriteLog("Creation of updateGui is complete with No Update Available", $LOG2FILE, $DBG_DEBUG)
	EndIf
	While 1
		$nMsg2 = GUIGetMsg()
		Switch $nMsg2
			Case $GUI_EVENT_CLOSE, $UpdateBtnOk
				WriteLog("Close updateGui - Close/Ok Button", $LOG2FILE, $DBG_DEBUG)
				GuiDelete($guiupdate)
				$guiabout = 0
				If (WinGetState($gui) <> 17 ) Then
					GuiSetState(@SW_ENABLE, $gui)
					WinActivate($gui)
					WriteLog("Reactivate Main Gui", $LOG2FILE, $DBG_DEBUG)
				EndIf
				If $minimizetray == 1 Then
					TraySetClick(8)
				EndIf
				$sChangeLog = null
				WriteLog("updateGui Closed", $LOG2FILE, $DBG_DEBUG)
				ExitLoop
			Case $showLogBtn
				Local $guiupdatePos = WinGetPos($guiupdate)
				If $guiupdatePos[2] >= 240 And $guiupdatePos[2] <= 250 Then
					WinMove($guiupdate, __("Update"), $guiupdatePos[0] - 150, $guiupdatePos[1], 540, 298)
					GUICtrlSetPos($GroupBox1, 8, 0, 516, 232)
					GUICtrlSetState($EditChLog, $GUI_SHOW)
					GUICtrlSetPos($Label10, 180, 16, 180 , 18)
					GUICtrlSetPos($Label11, 180, 34, 180, 18)
					GUICtrlSetPos($DownloadButton, 166, 52, 208, 24)
					GUICtrlSetPos($EditChLog, 16, 84, 500, 140)
					GUICtrlSetPos($UpdateBtnOk, 118, 238, 92, 24)
					GUICtrlSetPos($showLogBtn, 318, 238, 112, 24)
					GUICtrlSetData($showLogBtn,"&" & __("Hide Changelog"))
				Else
					WinMove($guiupdate, __("Update"), $guiupdatePos[0] + 150, $guiupdatePos[1], 248, 148)
					GUICtrlSetPos($GroupBox1, 8, 0, 224, 82)
					GUICtrlSetState($EditChLog, $GUI_HIDE)
					GUICtrlSetPos($Label10, 30, 16, 180 , 18)
					GUICtrlSetPos($Label11, 30, 34, 180, 18)
					GUICtrlSetPos($DownloadButton, 16, 52, 208, 24)
					GUICtrlSetPos($EditChLog, 16, 52, 224, 1)
					GUICtrlSetPos($UpdateBtnOk, 18, 86, 92, 24)
					GUICtrlSetPos($showLogBtn, 130, 86, 92, 24)
					GUICtrlSetData($showLogBtn,"&" & __("Show Changelog"))
				EndIf
			Case $DownloadButton
				WriteLog("updateGui - Download Button", $LOG2FILE, $DBG_DEBUG)
				Local $sArg = ""
				If GetOption("VerifyBranch") == 2 Then
					$sArg = "Dev"
				EndIf
				If Number(StringReplace($HighestVersion, ".", "")) > 2000 Then
					If $sArg <> "" Then
						$sArg &= " "
					EndIF
					$sArg &= StringFormat("beta %s", $HighestVersion)
				EndIF
				Local $hActive = WinGetHandle("[ACTIVE]")
				Local $ShellUpdater = ShellExecute(@Scriptdir & "\Resources\WinNUT-Updater.exe", $sArg, @Scriptdir & "\Resources\", $SHEX_OPEN, @SW_MAXIMIZE)
				Local $UpdaterPath = @Scriptdir & "\Resources\WinNUT-Updater.exe"
		EndSwitch
	WEnd
EndFunc ;==> updateGui

Func ShutdownGui()
	WriteLog("Enter ShutdownGui Function", $LOG2FILE, $DBG_DEBUG)
	Local $Halt, $msg
	$ShutdownDelay = GetOption("ShutdownDelay")
	$grace_time = GetOption("GraceDelay")
	$AllowGrace = GetOption("AllowGrace")
	$guishutdown = GUICreate("Shutdown", (290 + $AllowGrace * 110), 120, -1, -1, Bitor($WS_EX_TOPMOST, $WS_POPUP))
	$Grace_btn = GUICtrlCreateButton("Grace Time", 10, 10, 100, 100, BitOR($BS_NOTIFY, $GUI_SS_DEFAULT_BUTTON))
	If $AllowGrace = 0 Then GUICtrlSetState($Grace_btn, $GUI_HIDE)
	$Shutdown_btn = GUICtrlCreateButton(__("Immediate stop action") & @CRLF & __("Double Click for Stop Action"), (180 + $AllowGrace * 110), 10, 100, 100, BitOR($BS_MULTILINE, $BS_NOTIFY, $GUI_SS_DEFAULT_BUTTON))
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
	WriteLog("Creation of ShutdownGui is complete", $LOG2FILE, $DBG_DEBUG)
	While 1
		$nMsg3 = GUIGetMsg()
		Select
			Case $nMsg3 = $Grace_btn
				WriteLog("Click On Grace Button", $LOG2FILE, $DBG_DEBUG)
				GUICtrlSetState($Grace_btn, $GUI_DISABLE)
				_Timer_SetTimer($guishutdown, $grace_time*1000, "_Restart_counter")
				$Suspend_Countdown = 1
				AdlibUnregister("Update_counter")
				WriteLog("Update_counter Fonction UnRegistered", $LOG2FILE, $DBG_DEBUG)
			Case $nMsg3 = $Shutdown_btn or $In_progress = 0
				WriteLog("Shutdown Button or Time Elapsed", $LOG2FILE, $DBG_DEBUG)
				GUICtrlSetState($Grace_btn, $GUI_DISABLE)
				GUICtrlSetState($Shutdown_btn, $GUI_DISABLE)
				AdlibUnregister("Update_counter")
				WriteLog("Update_counter Fonction UnRegistered", $LOG2FILE, $DBG_DEBUG)
				Reset_Shutdown_Timer()
				Local $arr[2] = ["Execute Action Shutdown - Type : %s", GetOption("TypeOfStop")]
				WriteLog($arr, $LOG2FILE, $DBG_WARNING)
				Shutdown(GetOption("TypeOfStop"))
				WriteLog("Exit WinNut", $LOG2FILE, $DBG_DEBUG)
				Exit
			Case IsShutdownCondition() = False
				WriteLog("ShutDown Condition Resolved", $LOG2FILE, $DBG_DEBUG)
				AdlibUnregister("Update_counter")
				WriteLog("Update_counter Fonction UnRegistered", $LOG2FILE, $DBG_DEBUG)
				Reset_Shutdown_Timer()
				GuiDelete($guishutdown)
				WriteLog("Close ShutDownGui", $LOG2FILE, $DBG_DEBUG)
				ExitLoop
			Case $Suspend_Countdown = 1
				WriteLog("Suspend Countdown", $LOG2FILE, $DBG_DEBUG)
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
EndFunc ;==> ShtdownGui