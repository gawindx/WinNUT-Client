#include-once
#include <Date.au3>
#Include <GuiComboBox.au3>
#include <GuiComboBoxEx.au3>

Global $gui = 0
Global $log

Func WriteLog($msg )
	
	$msg = _Now() & " : " & $msg
	ControlCommand($gui , "",$log ,"AddString",$msg)
	ControlCommand($gui , "",$log ,"SetCurrentSelection",_GUICtrlComboBox_GetCount($log) - 1)
EndFunc
