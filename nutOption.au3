$optionList = "int port; int delay; char ipaddr[64]; char upsname[64]; int autoreconnect;"
$optionList &= "int mininputv; int maxinputv;"
$optionList &= "int minoutputv; int maxoutputv;"
$optionList &= "int mininputf; int maxinputf;"
$optionList &= "int minupsl; int maxupsl;"
$optionList &= "int minbattv; int maxbattv;"
$optionList &= "int minimizetray; int startwithwindows;"
$optionList &= "char defaultlang[64]; char language[64];"
$optionList &= "int minimizeonstart; int closetotray;"
$optionList &= "int shutdownpcbatt; int shutdownpctime;"
$optionList &= "int InstantAction; int AllowGrace;"
$optionList &= "int ShutdownDelay; int GraceDelay;"
$optionList &= "int TypeOfStop; int frequencysupply;"
$optionList &= "int uselogfile; int loglevel;"
$optionList &= "int VerifyUpdateAtStart; int VerifyUpdate;"
$optionList &= "int DelayVerif; int VerifyBranch;"
$optionList &= "char LastVerif[64];"
#comments-start
Reserved for future usage
$optionList &= "int upsauth;"
$optionList &= "char upsusername[64];char upspassword[64]"
#comments-end

#comments-start
#FUNCTION# ;===================================================================================================

 Name...........: _ListFileInstallFolder
 Description ...: List Install file(s) from a folder into au3
 Syntax.........: _ListFileInstallFolder($sSource, $sDest, $nFlag = 0, $sMask = '*', $sName = 'include', $sOverWrite = False, $sCompiled = False)
 Parameters ....:	$sSource = Source folder to get file(s) from
					$sDest   = Destination to install file(s) to
					$nFlag   = According to the flag of FileInstall [Optional]
					$sMask  = Extensions of file(s) to List         [Optional]
					$sName  = Out au3 script name     [Optional]
 Return values .: Success - Returns 1
                  Failure - Returns 0
 Author ........: MrCreator, FireFox
 Modified.......: FireFox, Gawindx
 Remarks .......: this function is faster with _WinAPI_FileFind :
                  [url="http://www.autoitscript.com/forum/index.php?showtopic=90545"]http://www.autoitscript.com/forum/index.php?showtopic=90545[/url]
 Related .......:
 Link ..........;
 Example .......;

===================================================================================================
#comments-end

Func _ListFileInstallFolder($sSource, $sDest, $nFlag = 0, $sMask = '*', $sName = 'include', $sOverWrite = False)
	WriteLog("Enter _ListFileInstallFolder Function", $LOG2FILE, $DBG_DEBUG)
	Local $hSearch, $sNext_File, $sRet_FI_Lines = ''

	If (Not @Compiled) Then
		$hSearch = FileFindFirstFile($sSource & '\' & $sMask)
		If $hSearch = -1 Then Return SetError(1, 0, 'FileFindFirstFile')
		While 1
			$sNext_File = FileFindNextFile($hSearch)
			If @error Then ExitLoop ;No more files
			$sRet_FI_Lines &= @CRLF & _
				'FileInstall("' & $sSource & '\' & $sNext_File & '", @ScriptDir & "' & $sDest & '\' & $sNext_File & '", ' & $nFlag & ')'
		WEnd
		FileClose($hSearch)
	EndIf
	If $sRet_FI_Lines = '' Then Return SetError(2, 0, '')
	If $sOverWrite Then FileDelete(@ScriptDir & '\' & $sName & '.au3')
	Return FileWrite(@ScriptDir & '\' & $sName & '.au3', StringStripWS($sRet_FI_Lines, 3))
EndFunc ;==> _ListFileInstallFolder

Func TimeToStr($TimeValue = 0)
	WriteLog("Enter TimeToStr Function", $LOG2FILE, $DBG_DEBUG)
	Local $hourrtime, $minrtime, $secrtime,$TimeStr
	
	$hourrtime = Floor($TimeValue / 3600)
	$minrtime = Floor(($TimeValue - ($hourrtime * 3600)) / 60 )
	$secrtime = $TimeValue - ($hourrtime * 3600) - ($minrtime * 60)
	If $hourrtime > 23 Then
		$dayrtime = Floor($hourrtime / 24)
		$hourrtime = $hourrtime - ($dayrtime * 24)
		$TimeStr = StringFormat("%02dd%02dh:%02dm:%02ds", $dayrtime, $hourrtime, $minrtime, $secrtime)
	ElseIf ( $hourrtime = 0 ) Then
		$TimeStr = StringFormat("%02dm:%02ds", $minrtime, $secrtime)
	Else
		$TimeStr = StringFormat("%02dh:%02dm:%02ds", $hourrtime, $minrtime, $secrtime)
	EndIf
	Local $arrValue[2] = [$TimeValue, $TimeStr]
	Local $arr[2] = ["Converted %s To %s", $arrValue]
	WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
	Return $TimeStr
EndFunc ;==> TimeToStr

Func _GetScriptVersion()
	WriteLog("Enter _GetScriptVersion Function", $LOG2FILE, $DBG_DEBUG)
	If @Compiled Then
		Return FileGetVersion(@ScriptFullPath)
	Else
		Local $sText = FileRead(@ScriptFullPath)
		If @error Then Return SetError(1, 0, "0.0.0.0")
		$pattern = "(?si)(?:\A|\n)\#pragma compile\(FileVersion, (.*?)(?:\)|\z|\n)"
		Local $asRet = StringRegExp($sText, $pattern, 3)
		If @error Then Return SetError(2, 0, "0.0.0.0")
		Return $asRet[0]
	EndIf
EndFunc ;==>_GetScriptVersion

Func Reset_Shutdown_Timer()
	WriteLog("Enter Reset_Shutdown_Timer Function", $LOG2FILE, $DBG_DEBUG)
	$Active_Countdown = 0
	Update_label()
EndFunc ;==> Reset_Shutdown_Timer

Func Init_Shutdown_Timer()
	WriteLog("Enter Init_Shutdown_Timer Function", $LOG2FILE, $DBG_DEBUG)
	If Not $Active_Countdown Then
		$Active_Countdown = 1
		$In_progress = $ShutdownDelay
		Update_label($In_progress)
		AdlibRegister("Update_counter", 1000)
		WriteLog("Update_counter Function Registered", $LOG2FILE, $DBG_DEBUG)
	EndIf
EndFunc ;==> Init_Shutdown_Timer

Func Update_label($param_string=0)
	WriteLog("Enter Update_label Function", $LOG2FILE, $DBG_DEBUG)
	Local $nMin = Floor($param_string/60)
	Local $nSec = $param_string - $nMin*60
	GUICtrlSetData($lbl_countdown, StringFormat("%02d:%02d", $nMin,$nSec))
	GUICtrlSetData($lbl_ups_status, StringFormat(__("Battery Charge : %02d%%\r\nRemaining Time : %s"), $battCh, $battrtimeStr))
EndFunc ;==> Update_label

Func _Restart_counter($hWnd, $iMsg, $iIDTimer, $iTime)
	WriteLog("Enter _Restart_counter Function", $LOG2FILE, $DBG_DEBUG)
	AdlibRegister("Update_counter", 1000)
	WriteLog("Update_counter Function Registered", $LOG2FILE, $DBG_DEBUG)
	$Suspend_Countdown = 0
	GUICtrlSetColor($lbl_countdown, 0x000000)
EndFunc ;==> _Restart_counter

;==== Main counter management function
Func Update_counter()
	WriteLog("Enter Update_counter Function", $LOG2FILE, $DBG_DEBUG)
	If $Active_Countdown Then
		$In_progress -= 1
		Update_label($In_progress)
		If $In_progress = 0 Then
			AdlibUnregister("Update_counter")
			WriteLog("Update_counter Function UnRegistered", $LOG2FILE, $DBG_DEBUG)
		EndIf
	EndIf
EndFunc ;==> Update_counter

Func InitOptionDATA()
	WriteLog("Enter Update_counter Function", $LOG2FILE, $DBG_DEBUG)
	$optionsStruct = 0 ;reset the variable if was inited earlier
	$optionsStruct = DllStructCreate($optionList)
	if IsDllStruct($optionsStruct) == 0 Then
		WriteLog("Error when InitOptionDATA", $LOG2FILE, $DBG_ERROR)
		$status = -1
		Return
	EndIf
	$status = 0
	Return
EndFunc ;==> InitOptionDATA

Func IsShutdownCondition()
	WriteLog("Enter IsShutdownCondition Function", $LOG2FILE, $DBG_DEBUG)
	If ($upsstatus <> "0") And ($upsstatus <> "OL" And $socket <> 0) Then
		If ($battCh < GetOption("shutdownpcbatt")) And ($battruntime < GetOption("shutdownpctime")) Then
			WriteLog("IsShutdownCondition : True", $LOG2FILE, $DBG_WARNING)
			Return True
		EndIf
	EndIf
	WriteLog("IsShutdownCondition : False", $LOG2FILE, $DBG_DEBUG)
	Return False
EndFunc ;==> IsShutdownCondition

Func GetOption($optionName)
	;No generation of log information in this function in order to avoid a recursion problem.
	$result = DllStructGetData($optionsStruct, $optionName);
	If ($result == 0) Then
		If @error <> 0 Then
			Local $arr[2] = ["Error When GetOption %s", $optionName]
			return -1
		EndIf
	Else
		Return $result
	EndIf
	Return $result
EndFunc ;==> GetOption

Func SetOption($optionName, $value, $type )
	WriteLog("Enter SetOption Function", $LOG2FILE, $DBG_DEBUG)
	If $type == "string" Then
		$value = String($value)
	EndIf
	
	If $type == "number" Then
		$value = Number($value)
	EndIf
	
	$result = DllStructSetData($optionsStruct , $optionName , $value)
	If $result == 0 And @error <> 0 Then
		Local $arr[2] = ["Error When SetOption %s", $optionName]
		WriteLog($arr, $LOG2FILE, $DBG_ERROR)
		Return -1
	EndIf
	Return $result
EndFunc ;==> SetOption

#comments-start
	This function reads parameters from ini file
	Used to read UPS connection settings
	Will also read color and other preferences that might be added in the future
	If ini file is not found in script's directory , default values are set for connection
	settings of UPS
#comments-end
Func Readparam($paramName, $sectionName, $type, $defaultValue, $iniName)
	WriteLog("Enter Readparam Function", $LOG2FILE, $DBG_DEBUG)
	$optionValue = IniRead($inipath, $sectionName, $iniName, "error")
	If $optionValue == "error" Then
		SetOption($paramName, $defaultValue, $type)
		IniWrite($inipath, $sectionName, $iniName, GetOption($paramName))
		Local $arr[2] = ["Error When Readparam %s", $paramName]
		WriteLog($arr, $LOG2FILE, $DBG_ERROR)
		Return $defaultValue
	Else
		SetOption($paramName, $optionValue, $type)
		Return $optionValue
	EndIf
EndFunc ;==> Readparam

Func ReadParams()
	WriteLog("Enter ReadParams Function", $LOG2FILE, $DBG_DEBUG)
	If FileExists($inipath) == 0 then ; file not created yet/doesn't exist
									  ;then create ini file and write them to that file
		$clock_bkg = String($gray)
		$panel_bkg = String($gray)
		SetOption("ipaddr", "nutserver host", "string")
		SetOption("upsname", "ups", "string")
		SetOption("port", 3493, "number")
		SetOption("delay", 5000, "number")
		SetOption("autoreconnect", 0, "number")
		SetOption("mininputv", 210, "number")
		SetOption("maxinputv", 270, "number")
		SetOption("minoutputv", 210, "number")
		SetOption("maxoutputv", 270, "number")
		SetOption("frequencysupply", 50, "number")
		SetOption("mininputf", 40, "number")
		SetOption("maxinputf", 60, "number")
		SetOption("minupsl", 0, "number")
		SetOption("maxupsl", 100, "number")
		SetOption("minbattv", 0, "number")
		SetOption("maxbattv", 20, "number")
		SetOption("minimizetray", 0, "number")
		SetOption("startwithwindows", 0, "number")
		SetOption("defaultlang", "en-US", "string")
		SetOption("language", "system", "string")
		SetOption("minimizeonstart", 0, "number")
		SetOption("closetotray", 0, "number")
		SetOption("shutdownpcbatt", 30, "number")
		SetOption("shutdownpctime", 120, "number")
		SetOption("InstantAction", 1, "number")
		SetOption("TypeOfStop", 17, "number")
		SetOption("ShutdownDelay", 15, "number")
		SetOption("AllowGrace", 0, "number")
		SetOption("GraceDelay", 15, "number")
		SetOption("uselogfile", 0, "number")
		SetOption("loglevel", 1, "number")
		SetOption("VerifyUpdateAtStart", 1, "number")
		SetOption("VerifyUpdate", 1, "number")
		SetOption("DelayVerif", 3, "number")
		SetOption("VerifyBranch", 1, "number")
		SetOption("LastVerif", _DateTimeFormat(_NowCalcDate(), 0), "string")
		WriteLog("Default IniFile Not Exists - Created", $LOG2FILE, $DBG_DEBUG)
		WriteParams()
	Else
		WriteLog("Default IniFile Exists - Reading", $LOG2FILE, $DBG_DEBUG)
		Readparam("ipaddr", "Connection", "string", "nutserver host", "Server address")
		Readparam("port", "Connection", "number", "3493", "Port")
		Readparam("upsname", "Connection", "string", "ups", "UPS name")
		Readparam("delay", "Connection","number", "5000", "Delay")
		Readparam("autoreconnect", "Connection", "number", "0", "AutoReconnect")
		ReadParam("mininputv", "Calibration", "number", "170", "Min Input Voltage")
		ReadParam("maxinputv", "Calibration", "number", "270", "Max Input Voltage")
		ReadParam("minoutputv", "Calibration", "number", "170", "Min Output Voltage")
		ReadParam("maxoutputv", "Calibration", "number", "270", "Max Output Voltage")
		ReadParam("frequencysupply", "Calibration", "number", "50", "Frequency Supply")
		ReadParam("mininputf", "Calibration", "number", "40", "Min Input Frequency")
		ReadParam("maxinputf", "Calibration", "number", "60", "Max Input Frequency")
		ReadParam("minupsl", "Calibration", "number", "0", "Min UPS Load")
		ReadParam("maxupsl", "Calibration", "number", "100", "Max UPS Load")
		ReadParam("minbattv", "Calibration", "number", "0", "Min Batt Voltage")
		ReadParam("maxbattv", "Calibration", "number", "20", "Max Batt Voltage")
		ReadParam("minimizetray", "Appearance", "number", "0", "Minimize to tray")
		ReadParam("closetotray", "Appearance", "number", "0", "Close to tray")
		ReadParam("minimizeonstart", "Appearance", "number", "0", "Minimize on Start")
		ReadParam("startwithwindows", "Appearance", "number", "0", "Start with Windows")
		ReadParam("defaultlang", "Appearance", "string", "en-US", "Default Language")
		ReadParam("language", "Appearance", "string", "system", "Language")
		ReadParam("shutdownpcbatt", "Power", "number", "30", "Shutdown Limit Battery Charge")
		ReadParam("shutdownpctime", "Power", "number", "120", "Shutdown Limit UPS Remain Time")
		ReadParam("InstantAction", "Power", "number", "1", "Immediate stop action")
		ReadParam("TypeOfStop", "Power", "number", "1", "Type of Stop")
		ReadParam("ShutdownDelay", "Power", "number", "15", "Delay To Shutdown")
		ReadParam("AllowGrace", "Power", "number", "0", "Allow Extended Shutdown Delay")
		ReadParam("GraceDelay", "Power", "number", "15", "Extended Shutdown Delay")
		ReadParam("uselogfile", "Logging", "number", "0", "Use Log File")
		ReadParam("loglevel", "Logging", "number", "0", "Log Level")
		ReadParam("VerifyUpdateAtStart", "Update", "number", "1", "Verify Update At Start")
		ReadParam("VerifyUpdate", "Update", "number", "1", "Verify Update")
		ReadParam("DelayVerif", "Update", "number", "3", "Delay Between Each Verification")
		ReadParam("VerifyBranch", "Update", "number", "1", "Stable Or Dev Branch")
		ReadParam("LastVerif", "Update", "string", _DateTimeFormat(_NowCalcDate(), 0), "Last Date Verification")

		$clock_bkg = IniRead($inipath, "Colors", "Clocks Color", "error")
		If $clock_bkg == "error" Then
			$clock_bkg = $gray
			IniWrite($inipath, "Colors", "Clocks Color", "0x" & Hex($clock_bkg))
		Else
			$clock_bkg = Number($clock_bkg)
		EndIf
		$clock_bkg = Number($clock_bkg)
		$clock_bkg_bgr = RGBtoBGR($clock_bkg)
		;;;;;;;;;;;;;;;;;;;;;;;;;;
		$panel_bkg = IniRead($inipath, "Colors", "Panel Color", "error")
		
		If $panel_bkg == "error" Then
			$panel_bkg = $gray
			IniWrite($inipath, "Colors", "Panel Color", "0x" & Hex($panel_bkg))
		Else
			$panel_bkg = Number($panel_bkg)
		EndIf
		$panel_bkg = Number($panel_bkg)
	EndIf
EndFunc ;==> ReadParams

#comments-start
	This function writes parameters to ini file
	This is after these were set in the gui and apply or OK button was hit there
#comments-end
Func Log_WriteParams(ByRef $ArrayValue, $strSection, $strParam, $strValue, ByRef $strLog)
	_ArrayAdd($ArrayValue, $strSection & "/" & $strParam)
	_ArrayAdd($ArrayValue, $strValue)
	$strLog &= "%s -> %s\n"
EndFunc

Func WriteParams()
	WriteLog("Enter WriteParams Function", $LOG2FILE, $DBG_DEBUG)
	Local $arrValue[0]
	Local $arr[2]
	Local $strLog = "Write Params :\n"
	IniWrite($inipath, "Connection", "Server address", GetOption("ipaddr"))
	Log_WriteParams($arrValue, "Connection", "Server address", GetOption("ipaddr"), $strLog)
	IniWrite($inipath, "Connection", "Port", GetOption("port"))
	Log_WriteParams($arrValue, "Connection", "Port", GetOption("port"), $strLog)
	IniWrite($inipath, "Connection", "UPS name", GetOption("upsname"))
	Log_WriteParams($arrValue, "Connection", "UPS name", GetOption("upsname"), $strLog)
	IniWrite($inipath, "Connection", "Delay", GetOption("delay"))
	Log_WriteParams($arrValue, "Connection", "Delay", GetOption("delay"), $strLog)
	IniWrite($inipath, "Connection", "AutoReconnect", GetOption("autoreconnect"))
	Log_WriteParams($arrValue, "Connection", "AutoReconnect", GetOption("autoreconnect"), $strLog)
	IniWrite($inipath, "Colors", "Clocks Color", "0x" & Hex($clock_bkg))
	Log_WriteParams($arrValue, "Connection", "Clocks Color", "0x" & Hex($clock_bkg), $strLog)
	IniWrite($inipath, "Colors", "Panel Color", "0x" & Hex($panel_bkg))
	Log_WriteParams($arrValue, "Connection", "Panel Color", "0x" & Hex($panel_bkg), $strLog)
	IniWrite($inipath, "Appearance", "Minimize to tray", GetOption("minimizetray"))
	Log_WriteParams($arrValue, "Appearance", "Minimize to tray", GetOption("minimizetray"), $strLog)
	IniWrite($inipath, "Appearance", "Close to tray", GetOption("closetotray"))
	Log_WriteParams($arrValue, "Appearance", "Close to tray", GetOption("closetotray"), $strLog)
	IniWrite($inipath, "Appearance", "Minimize on Start", GetOption("minimizeonstart"))
	Log_WriteParams($arrValue, "Appearance", "Minimize on Start", GetOption("minimizeonstart"), $strLog)
	IniWrite($inipath, "Appearance", "Start with Windows", GetOption("startwithwindows"))
	Log_WriteParams($arrValue, "Appearance", "Start with Windows", GetOption("startwithwindows"), $strLog)
	IniWrite($inipath, "Appearance", "Default Language", GetOption("defaultlang"))
	Log_WriteParams($arrValue, "Appearance", "Default Language", GetOption("defaultlang"), $strLog)
	IniWrite($inipath, "Appearance", "Language", GetOption("language"))
	Log_WriteParams($arrValue, "Appearance", "Language", GetOption("language"), $strLog)
	IniWrite($inipath, "Power", "Shutdown Limit Battery Charge", GetOption("shutdownpcbatt"))
	Log_WriteParams($arrValue, "Power", "Shutdown Limit Battery Charge", GetOption("shutdownpcbatt"), $strLog)
	IniWrite($inipath, "Power", "Shutdown Limit UPS Remain Time", GetOption("shutdownpctime"))
	Log_WriteParams($arrValue, "Power", "Shutdown Limit UPS Remain Time", GetOption("shutdownpctime"), $strLog)
	IniWrite($inipath, "Power", "Immediate stop action", GetOption("InstantAction"))
	Log_WriteParams($arrValue, "Power", "Immediate stop action",  GetOption("InstantAction"), $strLog)
	IniWrite($inipath, "Power", "Type Of Stop", GetOption("TypeOfStop"))
	Log_WriteParams($arrValue, "Power", "Type Of Stop", GetOption("TypeOfStop"), $strLog)
	IniWrite($inipath, "Power", "Delay To Shutdown", GetOption("ShutdownDelay"))
	Log_WriteParams($arrValue, "Power", "Delay To Shutdown", GetOption("ShutdownDelay"), $strLog)
	IniWrite($inipath, "Power", "Allow Extended Shutdown Delay", GetOption("AllowGrace"))
	Log_WriteParams($arrValue, "Power", "Allow Extended Shutdown Delay", GetOption("AllowGrace"), $strLog)
	IniWrite($inipath, "Power", "Extended Shutdown Delay", GetOption("GraceDelay"))
	Log_WriteParams($arrValue, "Power", "Extended Shutdown Delay", GetOption("GraceDelay"), $strLog)
	IniWrite($inipath, "Calibration", "Min Input Voltage", GetOption("mininputv"))
	Log_WriteParams($arrValue, "Calibration", "Min Input Voltage", GetOption("mininputv"), $strLog)
	IniWrite($inipath, "Calibration", "Max Input Voltage", GetOption("maxinputv"))
	Log_WriteParams($arrValue, "Calibration", "Max Input Voltage", GetOption("maxinputv"), $strLog)
	IniWrite($inipath, "Calibration", "Min Output Voltage", GetOption("minoutputv"))
	Log_WriteParams($arrValue, "Calibration", "Min Output Voltage", GetOption("minoutputv"), $strLog)
	IniWrite($inipath, "Calibration", "Max Output Voltage", GetOption("maxoutputv"))
	Log_WriteParams($arrValue, "Calibration", "Max Output Voltage", GetOption("maxoutputv"), $strLog)
	IniWrite($inipath, "Calibration", "Frequency Supply", GetOption("frequencysupply"))
	Log_WriteParams($arrValue, "Calibration", "Frequency Supply", GetOption("frequencysupply"), $strLog)
	IniWrite($inipath, "Calibration", "Min Input Frequency", GetOption("mininputf"))
	Log_WriteParams($arrValue, "Calibration", "Min Input Frequency", GetOption("mininputf"), $strLog)
	IniWrite($inipath, "Calibration", "Max Input Frequency", GetOption("maxinputf"))
	Log_WriteParams($arrValue, "Calibration", "Max Input Frequency", GetOption("maxinputf"), $strLog)
	IniWrite($inipath, "Calibration", "Min UPS Load", GetOption("minupsl"))
	Log_WriteParams($arrValue, "Calibration", "Min UPS Load", GetOption("minupsl"), $strLog)
	IniWrite($inipath, "Calibration", "Max UPS Load", GetOption("maxupsl"))
	Log_WriteParams($arrValue, "Calibration", "Max UPS Load", GetOption("maxupsl"), $strLog)
	IniWrite($inipath, "Calibration", "Min Batt Voltage", GetOption("minbattv"))
	Log_WriteParams($arrValue, "Calibration", "Min Batt Voltage", GetOption("minbattv"), $strLog)
	IniWrite($inipath, "Calibration", "Max Batt Voltage", GetOption("maxbattv"))
	Log_WriteParams($arrValue, "Calibration", "Max Batt Voltage", GetOption("maxbattv"), $strLog)
	IniWrite($inipath, "Logging", "Use Log File", GetOption("uselogfile"))
	Log_WriteParams($arrValue, "Logging", "Use Log File", GetOption("uselogfile"), $strLog)
	IniWrite($inipath, "Logging", "Log Level", GetOption("loglevel"))
	Log_WriteParams($arrValue, "Logging", "Log Level", GetOption("loglevel"), $strLog)
	IniWrite($inipath, "Update", "Verify Update At Start", GetOption("VerifyUpdateAtStart"))
	Log_WriteParams($arrValue, "Update", "Verify Update At Start", GetOption("VerifyUpdateAtStart"), $strLog)
	IniWrite($inipath, "Update", "Verify Update", GetOption("VerifyUpdate"))
	Log_WriteParams($arrValue, "Update", "Verify Update", GetOption("VerifyUpdate"), $strLog)
	IniWrite($inipath, "Update", "Delay Between Each Verification", GetOption("DelayVerif"))
	Log_WriteParams($arrValue, "Update", "Delay Between Each Verification", GetOption("DelayVerif"), $strLog)
	IniWrite($inipath, "Update", "Stable Or Dev Branch", GetOption("VerifyBranch"))
	Log_WriteParams($arrValue, "Update", "Stable Or Dev Branch", GetOption("VerifyBranch"), $strLog)
	IniWrite($inipath, "Update", "Last Date Verification", GetOption("LastVerif"))
	Log_WriteParams($arrValue, "Update", "Last Date Verification", GetOption("LastVerif"), $strLog)

	Local $arr[2] = [$strLog, $arrValue]
	WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
EndFunc ;==> WriteParams