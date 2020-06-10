Global $socket = 0

Func ProcessData($data)
	Local $strs
	$strs = StringSplit($data, '"')
	If UBound($strs) < 2 Then ;ERROR string returned or other unexpected condition
		Return -1 ;return -1 which means the something bad happened and value
	EndIf ;returned from NUT is unusable
	If StringLeft($strs[2], 1) == "0" Then
		Return StringTrimLeft($strs[2], 1)
	Else
		Return $strs[2]
	EndIf
EndFunc ;==> ProcessData

Func CheckErr($upsresp)
	Local $strs
	If StringLeft($upsresp,3) == "ERR" Then
		$strs = StringSplit($upsresp, " ")
		if UBound($strs) < 2 Then
			Return "Uknown Error"
		EndIf
		Return $strs[2]
	Else
		Return "OK"
	EndIf
EndFunc ;==> CheckErr

Func ListUPSVars($upsId, byref $upsVar)
	Local $sendstring, $sent, $data
	If $socket == 0 Then
		$upsVar = "0"
		Return -1
	EndIf
	$sendstring ="LIST VAR " & $upsID  & @CRLF
	$sent = TCPSend($socket, $sendstring )
	If $sent == 0 Then ;connection lost
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		Return -1
	EndIf
	Sleep(500)
	$data = TCPRecv($socket, 4096)
	If $data == "" Then ;connection lost
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		Return -1
	EndIf
	$err = CheckErr($data)
	If $err <> "OK" Then
		$errorstring = $err
		If StringInStr($errorstring, "UNKNOWN-UPS") <> 0 Then
			WriteLog(StringFormat(__("UPS %s doesn't exist"), $upsId))
		EndIf
		$upsVar = "0"
		Return -1
	EndIf
	$upsVar = $data
	Return 0	
EndFunc ;==> ListUPSVars

Func GetUPSDescVar($upsId, $varName, byref $upsVar)
	Local $sendstring, $sent, $data
	If $socket == 0 Then
		$upsVar = "0"
		Return -1
	EndIf
	$sendstring ="GET DESC " & $upsID & " " & $varName & @CRLF
	$sent = TCPSend($socket, $sendstring )
	If $sent == 0 Then ;connection lost
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		Return -1
	EndIf
	$data = TCPRecv($socket, 4096)
	If $data == "" Then ;connection lost
		$errorstring = __("Connection lost")
		WriteLog($errorstring)
		$socket = 0
		$upsVar = "0"
		Return -1
	EndIf
	$err = CheckErr($data)
	If $err <> "OK" Then
		$errorstring = $err
		If StringInStr($errorstring, "UNKNOWN-UPS") <> 0 Then
			WriteLog(StringFormat(__("UPS %s doesn't exist"), $upsId))
		EndIf
		$upsVar = "0"
		Return -1
	EndIf
	$upsVar = ProcessData($data)
	Return 0	
EndFunc ;==> GetUPSDescVar

Func GetUPSVar($upsId, $varName, byref $upsVar, $fallback_value=Null, $post_send_delay=Null)
	Local $sendstring, $sent, $data
	If $socket == 0 Then
		$upsVar = "0"
		Return -1
	EndIf
	$sendstring ="GET VAR " & $upsID & " " & $varName & @CRLF
	$sent = TCPSend($socket, $sendstring )
	If $sent == 0 Then ;connection lost
		WriteLog(__("Connection lost"))
		$socket = 0
		$upsVar = "0"
		Return -1
	EndIf
	If $post_send_delay Then Sleep($post_send_delay)
	$data = TCPRecv($socket , 4096)
	If $data == "" And $fallback_value Then
		$data = "VAR " & $upsID & " " & $varName & " " & '"' & $fallback_value & '"'
	ElseIf $data == "" Then ;connection lost
		WriteLog(__("Connection lost at " & $varName))
		$socket = 0
		$upsVar = "0"
		Return -1
	EndIf
	$err = CheckErr($data)
	If $err <> "OK" Then
		$errorstring = $err
		If StringInStr($errorstring, "UNKNOWN-UPS") <> 0 Then
			WriteLog(StringFormat(__("UPS %s doesn't exist"), $upsId))
		EndIf
		$upsVar = "0"
		Return -1
	EndIf
	$upsVar = ProcessData($data)
	Return 0
EndFunc ;==> GetUPSVar

Func DeletePortProxy()
	If $ipv6mode Then
		Local $PortProxycmd = "netsh interface portproxy delete v4tov6 listenaddress=" & $PortProxyAddress & " listenport=" & $PortProxyPort
		Local $PortProxyFullcmd = @ComSpec & " /c " & $PortProxycmd
		Local $iReturn = RunWait($PortProxyFullcmd, "", @SW_HIDE, $STDERR_MERGED)
		If @error = 0 Then
			WriteLog(__("PortProxy Deleted on") & " " & $PortProxyAddress & ":" & $PortProxyPort)
		EndIf
	EndIf
EndFunc ;==> DeletePortProxy

Func DisconnectServer()
	If $socket <> 0 Then ;already connected
		WriteLog(__("Disconnecting from server"))
		TCPSend($socket, "LOGOUT")
		TCPCloseSocket($socket) ;disconnect first
		DeletePortProxy()
		$socket = 0
	EndIf
	Return $socket
EndFunc ;==> DisconnectServer

Func ConnectServer()
	If $socket <> 0 Then ;already connected
		WriteLog(__("Disconnecting from server"))
		TCPSend($socket, "LOGOUT")
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
	If $socket == -1 Then;connection failed
		$haserror = 1
		WriteLog(__("Connection failed"))
		Return -1
	Else
		WriteLog(__("Connection Established"))
		Return 0
	EndIf
EndFunc ;==>ConnectServer