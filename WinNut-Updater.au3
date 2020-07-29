#pragma compile(FileVersion, 1.8.0.3)
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
Global const $URLStable2 = "https://github.com/gawindx/WinNUT-Client/releases/download/v%s/Setup.msi"
Global const $URLDev2 = "https://github.com/gawindx/WinNUT-Client/releases/download/v%s-dev/Setup.msi"
Global $WinNutExeURL = ""
Global const $DestDir = StringLeft(@ScriptDir, StringInStr(@ScriptDir,"\",0,-1)-1) & "\"
Global const $DestFile = $DestDir & "WinNUT-client.exe"
Global $BetaMode = False

Switch UBound($CmdLine)
	Case 1
		;Stable - no beta
		$WinNutExeURL = $URLStable
	Case 2
		;dev - no beta
		$WinNutExeURL = $URLDev
	Case 3
		;Stable - beta
		$WinNutExeURL = StringFormat($URLStable2, $CmdLine[2])
		$BetaMode = True
	Case 4
		;Stable - beta
		$WinNutExeURL = StringFormat($URLDev2, $CmdLine[3])
		$BetaMode = True
EndSwitch

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
	If $BetaMode == True Then
		GUICtrlSetData($lbltext, "Run WinNUT-Setup.msi")
		GUICtrlSetData($progBar, 100)
		Sleep(1000)
		FileCopy($sFilePath, $DestDir & "WinNUT-Setup.msi", BitOr($FC_OVERWRITE, $FC_CREATEPATH))
		Sleep(1000)
		Local $ShellUpdater = ShellExecuteWait($DestDir & "WinNUT-Setup.msi", "", "", $SHEX_OPEN, @SW_MAXIMIZE)
		If @error Then
        	MsgBox($MB_SYSTEMMODAL, "", "Error installing WinNUt." & @CRLF & "The setup file is saved here:" & @CRLF & $DestDir & "WinNUT-Setup.msi")
    	Else
			Local $sFileSelectFolder = FileSelectFolder("Indicate the installation directory of WinNut V2", @ProgramFilesDir, 0, "WinNUT-client")
			If @error Then
				Local $Message = "No directory selected for importing the Ini file."
				$Message &= @CRLF & "You can copy it manually to the installation directory of WinNUT V2"
				$Message &= @CRLF & "and it will be imported automatically when running WinNUT V2 for the first time."
				$Message &= @CRLF & "Otherwise, you can directly launch WinNUT V2 and recreate your configuration manually afterwards."
				$Message &= @CRLF & @CRLF & StringFormat("You can also delete the WinNUT V1 directory (%s) as it is no longer needed.", $DestDir)
        		MsgBox($MB_SYSTEMMODAL, "", $Message)
    		Else
    			if FileCopy( $DestDir & "ups.ini", $sFileSelectFolder & "\ups.ini", BitOr($FC_OVERWRITE, $FC_CREATEPATH)) == 1 Then
    				Local $Message = "Your configuration file has been copied into the installation directory"
					$Message &= @CRLF & "of WinNUT V2 and your configuration will be imported during its first launch."
					$Message &= @CRLF & @CRLF & StringFormat("You can now delete the WinNUT V1 directory (% s) as it is no longer needed.", $DestDir)
				Else
					Local $Message = "Error copying your configuration file"
					$Message &= @CRLF & "You can copy it manually to the installation directory of WinNUT V2"
					$Message &= @CRLF & "and it will be imported automatically when running WinNUT V2 for the first time."
					$Message &= @CRLF & "Otherwise, you can directly launch WinNUT V2 and recreate your configuration manually afterwards."
					$Message &= @CRLF & @CRLF & StringFormat("You can also delete the WinNUT V1 directory (%s) as it is no longer needed.", $DestDir)
				EndIf 
        		MsgBox($MB_SYSTEMMODAL, "", $Message)
    		EndIf
    	EndIf
	Else
		GUICtrlSetData($lbltext, "Replace WinNUT-Client.exe")
		FileCopy($sFilePath, $DestFile, BitOr($FC_OVERWRITE, $FC_CREATEPATH))
		GUICtrlSetData($progBar, 100)
		Sleep(1000)
		FileDelete($sFilePath)
	EndIF
Else
	MsgBox("","","Download Error")
EndIf

If $BetaMode == False Then
	If ShellExecute($DestFile, '', $DestDir, $SHEX_OPEN, @SW_MAXIMIZE) == 0 Then
		MsgBox("","","WinNut Error Launch")
	EndIf
EndIf