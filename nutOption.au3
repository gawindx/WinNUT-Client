#include-once
#include "nutColor.au3"
#include "nutDraw.au3"

Global $optionList = "int port;int delay;char ipaddr[64];char upsname[64];"
$optionList = $optionList & "int mininputv;int maxinputv;"
$optionList = $optionList & "int minoutputv;int maxoutputv;"
$optionList = $optionList & "int mininputf;int maxinputf;"
$optionList = $optionList & "int minupsl;int maxupsl;"
$optionList = $optionList & "int minbattv;int maxbattv;"
$optionList = $optionList & "int minimizetray;int startwithwindows;int shutdownpc;"

Global $optionsStruct = 0
Global $inipath = @ScriptDir & "\" & "ups.ini"
Global $panel_bkg = 0
Global $clock_bkg_bgr = 0
Global $panel_bkg_bgr = 0

Func _GetScriptVersion()
	If @Compiled Then
		Return FileGetVersion(@ScriptFullPath)
	Else
		Local $sText = FileRead(@ScriptFullPath)
		If @error Then Return SetError(1, 0, "0.0.0.0") ; File couldn't be read
WriteLog(@ScriptFullPath)
WriteLog($sText)
		Local $asRet = StringRegExp($sText, "(?si)(?:\A|\n)\#AutoIt3Wrapper\_Res\_Fileversion\=(.*?)(?:\z|\n)", 3)
		If @error ==1 Then Return SetError(2, 0, "0.1.0.0") ; No version number found
		If @error ==2 Then Return SetError(2, 0, "0.2.0.0") ; No version number found

		Return $asRet[0]
	EndIf
EndFunc;==>_GetScriptVersion

Func InitOptionDATA()
	$result = 0
	$optionsStruct = 0 ;reset the variable if was inited earlier
	$optionsStruct = DllStructCreate($optionList)
	if IsDllStruct($optionsStruct) == 0 Then
		return -1
	EndIf	
	return 0

EndFunc


Func GetOption($optionName )
	$result = DllStructGetData($optionsStruct , $optionName);
	if ($result == 0) Then
		if @error <> 0 Then
			return -1
		EndIf
	Else
		return $result
	EndIf
	
	return $result
	
EndFunc


Func SetOption($optionName , $value , $type )
	if $type == "string" Then
		$value = String($value)
	EndIf
	
	if $type == "number" Then
		$value = Number($value)
	EndIf
	
	$result = DllStructSetData($optionsStruct , $optionName , $value)
	if $result == 0 and @error <> 0 Then
		return -1
	EndIf
	
	return $result
EndFunc


;This function reads parameters from ini file
;Used to read UPS connection settings
;Will also read color and other preferences that might be added in the future
;If ini file is not found in script's directory , default values are set for connection
;settings of UPS
Func Readparam($paramName , $sectionName , $type ,$defaultValue , $iniName)
	
	$optionValue = IniRead($inipath , $sectionName,$iniName,"error")
	if $optionValue == "error" Then 
		SetOption( $paramName , $defaultValue , $type )
		IniWrite($inipath , $sectionName,$iniName,GetOption($paramName))
		return $defaultValue
	Else
		SetOption( $paramName , $optionValue , $type )
		return $optionValue
	EndIf
	
EndFunc


Func ReadParams()
	if FileExists($inipath) == 0 then ; file not created yet/doesn't exist
									  ;then create ini file and write them to that file
		$clock_bkg = String($gray)
		$panel_bkg = String($gray)
		SetOption("ipaddr", "nutserver host", "string")
		SetOption("upsname", "ups", "string")
		SetOption("port", 3493, "number")
		SetOption("delay", 5000, "number")
		SetOption("mininputv", 170, "number")
		SetOption("maxinputv", 270, "number")
		SetOption("minoutputv", 170, "number")
		SetOption("maxoutputv", 270, "number")
		SetOption("mininputf", 20, "number")
		SetOption("maxinputf", 70, "number")
		SetOption("minupsl", 0, "number")
		SetOption("maxupsl", 100, "number")
		SetOption("minbattv", 0, "number")
		SetOption("maxbattv", 20, "number")
		SetOption("minimizetray", 0, "number")
		SetOption("startwithwindows", 0, "number")
		SetOption("shutdownpc", 0, "number")
		WriteParams()
	Else
		Readparam("ipaddr" , "Connection" , "string" , "nutserver host" , "Server address")		
		Readparam("port","Connection" , "number" , "3493" , "Port")
		Readparam("upsname", "Connection" , "string" , "ups" , "UPS name")
		Readparam("delay" , "Connection" , "number" , "5000" , "Delay")
		
		ReadParam("mininputv" , "Calibration" , "number" , "170" , "Min Input Voltage")
		ReadParam("maxinputv" , "Calibration" , "number" , "270" , "Max Input Voltage")

		ReadParam("minoutputv" , "Calibration" , "number" , "170" , "Min Output Voltage")
		ReadParam("maxoutputv" , "Calibration" , "number" , "270" , "Max Output Voltage")
		
		ReadParam("mininputf" , "Calibration" , "number" , "20" , "Min Input Frequency")
		ReadParam("maxinputf" , "Calibration" , "number" , "70" , "Max Input Frequency")
		
		ReadParam("minupsl" , "Calibration" , "number" , "0" , "Min UPS Load")
		ReadParam("maxupsl" , "Calibration" , "number" , "100" , "Max UPS Load")
		
		ReadParam("minbattv" , "Calibration" , "number" , "0" , "Min Batt Voltage")
		ReadParam("maxbattv" , "Calibration" , "number" , "20" , "Max Batt Voltage")
		
		ReadParam("minimizetray" , "Appearance" , "number" , "0" , "Minimize to tray")
		ReadParam("startwithwindows" , "Appearance" , "number" , "0" , "Start with Windows")
		
		ReadParam("shutdownpc" , "Power" , "number" , "0" , "Shutdown Computer")
		$clock_bkg = IniRead($inipath , "Colors","Clocks Color","error")
		
		if $clock_bkg == "error" Then
			$clock_bkg = $gray
			IniWrite($inipath , "Colors","Clocks Color" , "0x" & Hex($clock_bkg))
		Else
			$clock_bkg = Number($clock_bkg)
		EndIf
		$clock_bkg = Number($clock_bkg)
		$clock_bkg_bgr = RGBtoBGR($clock_bkg)
		;;;;;;;;;;;;;;;;;;;;;;;;;;
		$panel_bkg = IniRead($inipath , "Colors","Panel Color","error")
		
		if $panel_bkg == "error" Then
			$panel_bkg = $gray
			IniWrite($inipath , "Colors","Panel Color" , "0x" & Hex($panel_bkg))
		Else
			$panel_bkg = Number($panel_bkg)
		EndIf
		$panel_bkg = Number($panel_bkg)
		;$clock_bkg_bgr = RGBtoBGR($clock_bkg)

	EndIf
	;WriteLog("Done")
EndFunc



;This function writes parameters to ini file
;This is after these were set in the gui and apply or OK button was hit there
Func WriteParams()
	IniWrite($inipath, "Connection", "Server address", GetOption("ipaddr"))
	IniWrite($inipath, "Connection", "Port", GetOption("port"))
	IniWrite($inipath, "Connection", "UPS name", GetOption("upsname"))
	IniWrite($inipath, "Connection", "Delay", GetOption("delay"))
	IniWrite($inipath, "Colors", "Clocks Color", "0x" & Hex($clock_bkg))
	IniWrite($inipath, "Colors", "Panel Color", "0x" & Hex($panel_bkg))
	IniWrite($inipath, "Appearance", "Minimize to tray", GetOption("minimizetray"))
	IniWrite($inipath, "Appearance", "Start with Windows", GetOption("startwithwindows"))
	IniWrite($inipath, "Power", "Shutdown Computer", GetOption("shutdownpc"))
	IniWrite($inipath, "Calibration", "Min Input Voltage", GetOption("mininputv"))
	IniWrite($inipath, "Calibration", "Max Input Voltage", GetOption("maxinputv"))
	IniWrite($inipath, "Calibration", "Min Output Voltage", GetOption("minoutputv"))
	IniWrite($inipath, "Calibration", "Max Output Voltage", GetOption("maxoutputv"))
	IniWrite($inipath, "Calibration", "Min Input Frequency", GetOption("mininputf"))
	IniWrite($inipath, "Calibration", "Max Input Frequency", GetOption("maxinputf"))
	IniWrite($inipath, "Calibration", "Min UPS Load", GetOption("minupsl"))
	IniWrite($inipath, "Calibration", "Max UPS Load", GetOption("maxupsl"))
	IniWrite($inipath, "Calibration", "Min Batt Voltage", GetOption("minbattv"))
	IniWrite($inipath, "Calibration", "Max Batt Voltage", GetOption("maxbattv"))
EndFunc
