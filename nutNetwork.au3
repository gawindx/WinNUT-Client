#include-once
#include <nutGlobal.au3>
#include <nutGui.au3>
#include <nutOption.au3>
Global $socket = 0 

Func ProcessData($data )
	
	Local $strs
	
	$strs = StringSplit($data , '"')
	if UBound($strs)<2 Then ;ERROR string returned or other unexpected condition
		return -1 ;return -1 which means the something bad happened and value
	EndIf ;returned from NUT is unusable
	if StringLeft($strs[2],1) == "0" Then
		return StringTrimLeft($strs[2],1)
	Else
		return $strs[2]
	EndIf
	
EndFunc

Func CheckErr($upsresp )
	
	Local $strs
	if StringLeft($upsresp,3)=="ERR" Then
		$strs = StringSplit($upsresp , " ")
		if UBound($strs)<2 Then
			return "Uknown Error"
		EndIf
		return $strs[2]
	Else
		return "OK"
	EndIf
	
EndFunc

Func ListUPSVars($upsId , byref $upsVar)
	
	Local $sendstring , $sent , $data
	if $socket == 0 then
		$upsVar = "0"
		return -1
	EndIf
	$sendstring ="LIST VAR " & $upsID  & @CRLF
	$sent = TCPSend($socket , $sendstring )
	if $sent == 0 Then ;connection lost
		$errorstring ="Connection lost"
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	Sleep(500)
	$data = TCPRecv($socket , 4096)
	if $data == "" Then ;connection lost
		$errorstring ="Connection lost"
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$err = CheckErr($data)
	if $err <> "OK" Then
		$errorstring = $err
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			WriteLog("UPS " & $upsId & " doesn't exist")
		EndIf
		$upsVar = "0"
		return -1
	EndIf
	$upsVar = $data
	return 0	
	
EndFunc

Func GetUPSDescVar($upsId , $varName , byref $upsVar)
	
	Local $sendstring , $sent , $data
	if $socket == 0 then
		$upsVar = "0"
		return -1
	EndIf
	$sendstring ="GET DESC " & $upsID & " " & $varName & @CRLF
	$sent = TCPSend($socket , $sendstring )
	if $sent == 0 Then ;connection lost
		$errorstring ="Connection lost"
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$data = TCPRecv($socket , 4096)
	if $data == "" Then ;connection lost
		$errorstring ="Connection lost"
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$err = CheckErr($data)
	if $err <> "OK" Then
		$errorstring = $err
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			WriteLog("UPS " & $upsId & " doesn't exist")
		EndIf
		$upsVar = "0"
		return -1
	EndIf
	$upsVar = ProcessData($data)
	return 0	
	
EndFunc

Func GetUPSVar($upsId , $varName , byref $upsVar)
	
	Local $sendstring , $sent , $data
	if $socket == 0 then
		$upsVar = "0"
		return -1
	EndIf
	$sendstring ="GET VAR " & $upsID & " " & $varName & @CRLF
	$sent = TCPSend($socket , $sendstring )
	if $sent == 0 Then ;connection lost
		$errorstring ="Connection lost"
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$data = TCPRecv($socket , 4096)
	if $data == "" Then ;connection lost
		$errorstring ="Connection lost"
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$err = CheckErr($data)
	if $err <> "OK" Then
		$errorstring = $err
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			WriteLog("UPS " & $upsId & " doesn't exist")
		EndIf
		$upsVar = "0"
		return -1
	EndIf
	$upsVar = ProcessData($data)
	return 0
	
EndFunc

Func ConnectServer()
	if $socket <>0 then ;already connected
		WriteLog("Disconnecting from server")
		TCPSend($socket,"LOGOUT")
		TCPCloseSocket($socket) ;disconnect first
		$socket = 0
	EndIf
	Opt("TCPTimeout",10)
	WriteLog("Connecting to NUT Server")
	$ipaddr = TCPNameToIP ( GetOption("ipaddr") )
	$socket = TCPConnect($ipaddr,GetOption("port"))
	if $socket == -1 Then;connection failed
		$haserror = 1
		$errorstring = "connection failed"
		WriteLog("Connection failed")
		return -1
	Else
		WriteLog("Connection Established")
		return 0
	EndIf

EndFunc

