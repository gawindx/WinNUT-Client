#pragma compile(FileVersion, 1.8.0.0)
#pragma compile(Icon, .\images\Ico\WinNUT-Updater.ico)
#pragma compile(Out, .\Build\WinNUT-Updater.exe)
#pragma compile(Compression, 9)
#pragma compile(UPX, False)
#pragma compile(Comments, 'Windows NUT Updater')
#pragma compile(FileDescription, Windows NUT Updater. Tools for Download New Version Of WinNut and Replace Old Existing Version.)
#pragma compile(LegalCopyright, Freeware)
#pragma compile(ProductName, WinNUT-Updater)
#pragma compile(Compatibility, win7, win8, win81, win10)

#include <AutoItConstants.au3>
#include <GUIConstants.au3>
#include <Array.au3>
#include <InetConstants.au3>
#include <MsgBoxConstants.au3>
#include <WinAPIFiles.au3>
#include <FileConstants.au3>

Global const $URLStable = "https://github.com/gawindx/WinNUT-Client/master/Dev/Build/WinNut-client.exe"
Global const $URLDev = "https://github.com/gawindx/WinNUT-Client/raw/Dev/Build/WinNut-client.exe"
Global $WinNutExeURL = ""
Global const $DestDir = @ScriptDir & "\..\"
Global const $DestFile = $DestDir & "WinNUT-client.exe"

If UBound($CmdLine) > 1 Then
	If $CmdLine[1] == "Dev" Then
		$WinNutExeURL = $URLDev
	Else
		$WinNutExeURL = $URLStable
	EndIf
Else
	$WinNutExeURL = $URLStable
EndIf

If $WinNutExeURL == $URLDev Then
	Local $BeCarefulBox = MsgBox($MB_OKCANCEL, 'ATTENTION', 'Updating from Development version may cause uncertain operation')
	If $BeCarefulBox == $IDCANCEL Then
		Exit
	EndIf
EndIf

Local $guiupdate = GUICreate("WinNut Updater", 300, 100, -1, -1, -1, -1)
Local $lbltext = GUICtrlCreateLabel("", 16, 20, 179, 17, BitOR($SS_BLACKRECT, $SS_GRAYFRAME, $SS_LEFTNOWORDWRAP))
Local $progBar = GUICtrlCreateProgress(16, 40, 268, 16, $PBS_SMOOTH)
GUISetState(@SW_SHOW, $guiupdate)

Local $sFilePath = _WinAPI_GetTempFileName(@TempDir)
Local $progBarOffset = 0
If ProcessExists("WinNut-client.exe") Then
	$progBarOffset = 10
	GUICtrlSetData($progBar, $progBarOffset)
	GUICtrlSetData($lbltext, "Close WinNut")
	Sleep(2000)
	ProcessClose("WinNut-client.exe")
EndIf

Local $hDownload = InetGet($WinNutExeURL, $sFilePath, BitOr($INET_FORCERELOAD, $INET_IGNORESSL),  $INET_DOWNLOADBACKGROUND)

Do
	Local $InetGetInfo = InetGetInfo($hDownload)
	GUICtrlSetData($lbltext, "Download New Version")
	GUICtrlSetData($progBar, $progBarOffset + Int((90 - $progBarOffset) * ($InetGetInfo[$INET_DOWNLOADREAD] / $InetGetInfo[$INET_DOWNLOADSIZE])))
	sleep(20)
Until InetGetInfo($hDownload, $INET_DOWNLOADCOMPLETE)
sleep(1000)

Local $iBytesSize = InetGetInfo($hDownload, $INET_DOWNLOADREAD)
Local $iFileSize = FileGetSize($sFilePath)
InetClose($hDownload)

If $iBytesSize == $iFileSize And $iFileSize == $InetGetInfo[$INET_DOWNLOADSIZE] Then
	GUICtrlSetData($lbltext, "Replace WinNUT-Client.exe")
	FileCopy($sFilePath, $DestFile, BitOr($FC_OVERWRITE, $FC_CREATEPATH))
	GUICtrlSetData($progBar, 100)
	Sleep(1000)
	FileDelete($sFilePath)
Else
	MsgBox("","","Download Error")
EndIf

If ShellExecute($DestFile, '', $DestDir, $SHEX_OPEN, @SW_MAXIMIZE) == 0 Then
	MsgBox("","","WinNut Error Launch")
EndIf