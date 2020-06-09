Global $socket = 0 

Func ProcessData($data)
	
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

Func CheckErr($upsresp)
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
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	Sleep(500)
	$data = TCPRecv($socket , 4096)
	if $data == "" Then ;connection lost
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$err = CheckErr($data)
	if $err <> "OK" Then
		$errorstring = $err
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			WriteLog(StringFormat(__("UPS %s doesn't exist"), $upsId))
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
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$data = TCPRecv($socket , 4096)
	if $data == "" Then ;connection lost
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$err = CheckErr($data)
	if $err <> "OK" Then
		$errorstring = $err
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			WriteLog(StringFormat(__("UPS %s doesn't exist"), $upsId))
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
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$data = TCPRecv($socket , 4096)
	if $data == "" Then ;connection lost
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		return -1
	EndIf
	$err = CheckErr($data)
	if $err <> "OK" Then
		$errorstring = $err
		if StringInStr($errorstring,"UNKNOWN-UPS") <> 0 Then
			WriteLog(StringFormat(__("UPS %s doesn't exist"), $upsId))
		EndIf
		$upsVar = "0"
		return -1
	EndIf
	$upsVar = ProcessData($data)
	return 0
EndFunc

Func DeletePortProxy()
	If $ipv6mode Then
		Local $PortProxycmd = "netsh interface portproxy delete v4tov6 listenaddress=" & $PortProxyAddress & " listenport=" & $PortProxyPort
		Local $PortProxyFullcmd = @ComSpec & " /c " & $PortProxycmd
		MsgBox($MB_SYSTEMMODAL, "", $PortProxyFullcmd)
		Local $iReturn = RunWait($PortProxyFullcmd, "", @SW_HIDE,  $STDERR_MERGED)
    	If @error = 0 Then
    		MsgBox($MB_SYSTEMMODAL, "", "Le code de retour était: " & $iReturn)
			WriteLog(__("PortProxy Deleted on") & " " & $PortProxyAddress & ":" & $PortProxyPort)
		EndIf
	EndIf
EndFunc

Func DisconnectServer()
	If $socket <> 0 then ;already connected
		WriteLog(__("Disconnecting from server"))
		TCPSend($socket,"LOGOUT")
		TCPCloseSocket($socket) ;disconnect first
		DeletePortProxy()
		$socket = 0
	EndIf
	Return $socket
EndFunc

Func ConnectServer()
	if $socket <> 0 then ;already connected
		WriteLog(__("Disconnecting from server"))
		TCPSend($socket,"LOGOUT")
		TCPCloseSocket($socket) ;disconnect first
		DeletePortProxy()
		$socket = 0
	EndIf
	Opt("TCPTimeout",10)
	WriteLog(__("Connecting to NUT Server"))
	If IsFQDN(GetOption("ipaddr")) Then
		$ResolvedHost = ResolveFQDN(GetOption("ipaddr"))
	Else
		$ResolvedHost = GetOption("ipaddr")
	EndIf
	If IsIPV4($ResolvedHost) Then
		;WriteLog("Nut Server Is IPV4 Address")
		$ipv6mode = False
	ElseIf IsIPV6($ResolvedHost) Then
		;WriteLog("Nut Server Is IPV6 Address")
		$ipv6mode = True
	EndIf
	If $ipv6mode Then
		;create portproxy
		$PortProxyPort = Random($PortProxyPortMin, $PortProxyPortMax, 1)
		Local $PortProxycmd = "netsh interface portproxy add v4tov6 listenaddress=" & $PortProxyAddress & " listenport=" & $PortProxyPort & " connectport=" & GetOption("port") &" connectaddress=" & $ResolvedHost
		Local $PortProxyFullcmd = @ComSpec & " /c " & $PortProxycmd
		Local $iReturn = RunWait($PortProxyFullcmd, "", @SW_HIDE,  $STDERR_MERGED)
    	If @error = 0 Then
    		WriteLog(__("PortProxy Created on") & " " & $PortProxyAddress & ":" & $PortProxyPort)
			$socket = TCPConnect($PortProxyAddress, $PortProxyPort)
		EndIf
	Else
		$ipaddr = $ResolvedHost
		$socket = TCPConnect($ipaddr, GetOption("port"))
	EndIf
	if $socket == -1 Then;connection failed
		$haserror = 1
		$errorstring = __("Connection failed")
		WriteLog($errorstring)
		return -1
	Else
		WriteLog(__("Connection Established"))
		return 0
	EndIf
EndFunc

