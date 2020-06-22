#include-once
#include <WinAPIFiles.au3>
#include <InetConstants.au3>

Func IsFQDN($IPAddress)
	WriteLog("Enter IsFQDN Function", $LOG2FILE, $DBG_DEBUG)
	Local $sPattern = "^(?:(?!\d+\.|-)[a-zA-Z0-9_\-]{1,63}(?<!-)\.?)+(?:[a-zA-Z]{2,})$"
	If StringRegExp($IPAddress, $sPattern) Then
		Local $arr[2] = ["%s Is an FQDN Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_NOTICE)
		Return True
	Else
		Local $arr[2] = ["%s Is not an FQDN Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
		Return False
	EndIF
EndFunc ;==> IsFQDN

Func IsIPV4($IPAddress)
	WriteLog("Enter IsIPV4 Function", $LOG2FILE, $DBG_DEBUG)
	Local $sPattern = "^(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[0-1]?[0-9][0-9]?)$"
	Local $RegExResult = StringRegExp($IPAddress, $sPattern, $STR_REGEXPARRAYGLOBALFULLMATCH, 1)
	If @error Then
		Local $arr[2] = ["%s Is not an IPV4 Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
		Return False
	ElseIf ($RegExResult[0])[0] = $IPAddress  Then
		Local $arr[2] = ["%s Is an IPV4 Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_NOTICE)
		Return True
	Else
		Local $arr[2] = ["%s Is not an IPV4 Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
		Return False
	EndIF
EndFunc ;==> IsIPV4

Func IsIPV6($IPAddress)
	WriteLog("Enter IsIPV6 Function", $LOG2FILE, $DBG_DEBUG)
	Local $sPattern = "^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$"
	Local $RegExResult = StringRegExp($IPAddress, $sPattern, $STR_REGEXPARRAYGLOBALFULLMATCH, 1)
	If @error Then
		Local $arr[2] = ["%s Is not an IPV6 Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
		Return False
	ElseIf ($RegExResult[0])[0] = $IPAddress  Then
		Local $arr[2] = ["%s Is an IPV6 Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_NOTICE)
		Return True
	Else
		Local $arr[2] = ["%s Is not an IPV6 Address", $IPAddress]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
		Return False
	EndIF
EndFunc ;==> IsIPV6

Func ResolveFQDN($IPAddress)
	WriteLog("Enter ResolveFQDN Function", $LOG2FILE, $DBG_DEBUG)
	Local $TypeSearch = [ 'A' , 'AAAA' ]
	Local $ResultAddress = ""
	For $IpType In $TypeSearch
		Local $nscmd = @ComSpec & " /c " & "nslookup -type=" & $IpType & " " & $IPAddress
		Local $iPID = Run($nscmd, "", @SW_HIDE,  $STDERR_MERGED)
		If @error = 0 Then
			ProcessWaitClose($iPID)
			Local $sOutput = StdoutRead($iPID)
			Local $nsResultArray = StringSplit($sOutput, @CRLF)
			Local $idResult = null
			If UBound($nsResultArray) > 5 Then
				Local $countAddress = 0
				For $i = 0 To (UBound($nsResultArray) - 1)
					If StringRegExp($nsResultArray[$i], "Address:") Then
						$countAddress += 1
						If $countAddress = 2 Then
							$idResult=$i
							ExitLoop
						EndIf
					endif
				Next
			Else
				ContinueLoop
			EndIf
			If $idResult = null Then
				Local $arr[2] = ["Error nslookup Search Type %s", $IpType]
				WriteLog($arr, $LOG2FILE, $DBG_WARNING)
				ContinueLoop
			Else
				If $IpType = 'A' Then
					$ResultAddress = StringStripWS((StringSplit($nsResultArray[$idResult], ":"))[2], $STR_STRIPALL)
				Else
					Local $tmpAddress = $nsResultArray[$idResult]
					$ResultAddress = StringStripWS(StringRight($tmpAddress, StringLen($tmpAddress) - StringInStr($tmpAddress, ':')), $STR_STRIPALL)
				EndIf
				Local $arr[2] = ["Resolved Address : %s", $ResultAddress]
				WriteLog($arr, BitOr($LOG_GUI, $LOG2FILE), $DBG_NOTICE)
				ExitLoop
			EndIf
		Else
			Local $arr[2] = ["Error nslookup Search Type %s", $IpType]
			WriteLog($arr, $LOG2FILE, $DBG_WARNING)
		EndIf
	Next
	Return $ResultAddress
EndFunc ;==> ResolveFQDN

Func DeletePortProxy()
	WriteLog("Enter DeletePortProxy Function", $LOG2FILE, $DBG_DEBUG)
	If $ipv6mode Then
		WriteLog("IPV6 Mode", $LOG2FILE, $DBG_DEBUG)
		Local $PortProxycmd = "netsh interface portproxy delete v4tov6 listenaddress=" & $PortProxyAddress & " listenport=" & $PortProxyPort
		Local $PortProxyFullcmd = @ComSpec & " /c " & $PortProxycmd
		Local $iReturn = RunWait($PortProxyFullcmd, "", @SW_HIDE, $STDERR_MERGED)
		If @error = 0 Then
			Local $arrValue[2] = [$PortProxyAddress, $PortProxyPort]
			Local $arr[2] = ["PortProxy Deleted on %s:%s" , $arrValue]
			WriteLog($arr, BitOr($LOG_GUI, $LOG2FILE), $DBG_NOTICE)
		Else
			Local $arrValue[2] = [$PortProxyAddress, $PortProxyPort]
			Local $arr[2] = ["Unable to delete PortProxy on %s:%s" , $arrValue]
			WriteLog($arr, $LOG2FILE, $DBG_ERROR)
		EndIf
	Else
		WriteLog("IPV4 Mode", $LOG2FILE, $DBG_DEBUG)
	EndIf
EndFunc ;==> DeletePortProxy

Func ChangeFormatDate($sDate)
	Local $arrDate
	Local $arrTime
	_DateTimeSplit($sDate, $arrDate, $arrTime)
	Return StringFormat("%04d/%02d/%02d", $arrDate[3], $arrDate[2], $arrDate[1])
EndFunc

Func VerifyUpdate($ManualUpdate = False)
	WriteLog("Enter VerifyUpdate Function", $LOG2FILE, $DBG_DEBUG)
	If GetOption("VerifyUpdate") Or $ManualUpdate Then
		Local $sDelayVerif
		Switch GetOption("DelayVerif")
			Case 1
				$sDelayVerif = "D"
			Case 2
				$sDelayVerif = "w"
			Case 3
				$sDelayVerif = "M"
			Case Else
				$sDelayVerif = "M"
		EndSwitch
		Local $Today = ChangeFormatDate(_DateTimeFormat(_NowCalcDate(), 0))
		$diff = _DateDiff($sDelayVerif, ChangeFormatDate(GetOption("LastVerif")), $Today)
		If $diff >= 1 Or $ManualUpdate Then
			Local $ChangeLog = _WinAPI_GetTempFileName(@TempDir)
			If GetOption("VerifyBranch") == 1 Then
				$URL_Changelog = $URLStable
				WriteLog("VerifyUpdate from Stable branch", $LOG2FILE, $DBG_DEBUG)
			Else
				$URL_Changelog = $URLDev
				WriteLog("VerifyUpdate from Stable branch", $LOG2FILE, $DBG_DEBUG)
			EndIf
			Local $ChangeLogByteSize = InetGet($URLDev, $ChangeLog, BitOr($INET_FORCERELOAD, $INET_IGNORESSL), $INET_DOWNLOADWAIT)
			If $ChangeLogByteSize <> 0 Then
				Local $ArrChangeLog
				Local $newline = ""
				_FileReadToArray($ChangeLog, $ArrChangeLog, $FRTA_NOCOUNT)
				;extract data from array
				Local $ChangeLogDiff[0]
				For $line in $ArrChangeLog
					If StringInStr($line, "History") == 0 Then
						If $line <> "" Then
							Local $LogVersion
							Local $ActualVersion = Number(StringReplace($ProgramVersion, ".", ""))
							Local $sPattern = "^[Vv]ersion.*(\d).*(\d).*(\d).*(\d)$"
							Local $RegExResult = StringRegExp($line, $sPattern, $STR_REGEXPARRAYFULLMATCH, 1)
							If Not @error Then
								_ArrayDelete($RegExResult, 0)
								If $HighestVersion == null Then
									$HighestVersion = _ArrayToString($RegExResult, ".")
								EndIf
								$LogVersion = Number(_ArrayToString($RegExResult, ""))
								If $LogVersion > $ActualVersion Then
									If UBound($ChangeLogDiff) >= 1 Then
										_ArrayAdd($ChangeLogDiff, "")
									EndIf
									_ArrayAdd($ChangeLogDiff, $line)
								Else
									ExitLoop
								EndIf
							Else
								If $LogVersion > $ActualVersion Then
									Local $sPattern = "^.*:.*$"
									Local $RegExResult = StringRegExp($line, $sPattern, $STR_REGEXPMATCH, 1)
									If Not @error And $RegExResult == 1 Then
										If $newline <> "" Then
											_ArrayAdd($ChangeLogDiff, $newline)
											$newline = ""
										EndIf
										$newline &= StringStripWS($line, BitOr($STR_STRIPLEADING, $STR_STRIPTRAILING, $STR_STRIPSPACES))
									Else
										$newline &= StringStripWS($line, BitOr($STR_STRIPLEADING, $STR_STRIPTRAILING, $STR_STRIPSPACES))
									EndIf
								EndIf
							EndIf
						EndIf
					EndIf
				Next
				If UBound($ChangeLogDiff) > 0 Then
					$sChangeLog = _ArrayToString($ChangeLogDiff, @CRLF)
					Local $arr[2] = ["New Version Available : %s", $HighestVersion]
					WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
					WriteLog(StringFormat(__("New Version Available : %s"), $HighestVersion), $LOG_GUI, $DBG_DEBUG)
				Else
					$HighestVersion = null
					WriteLog(__("No Update Available"), $LOG_GUI, $DBG_DEBUG)
				EndIf
			Else
				WriteLog("Cannot download changelog.txt", $LOG2FILE, $DBG_DEBUG)
			EndIf
			FileDelete($ChangeLog)
			SetOption("LastVerif", $Today, "string")
			WriteParams()
		EndIf
	EndIf
EndFunc

Func ProcessData($data)
	WriteLog("Enter ProcessData Function", $LOG2FILE, $DBG_DEBUG)
	Local $strs
	$strs = StringSplit($data, '"')
	#comments-start
		ERROR string returned or other unexpected condition
		return -1 which means the something bad happened and value
		returned from NUT is unusable
	#comments-end
	If UBound($strs) < 2 Then
		WriteLog("Bad Value Returned", $LOG2FILE, $DBG_ERROR)
		Return -1 
	EndIf
	If StringLeft($strs[2], 1) == "0" Then
		Return StringTrimLeft($strs[2], 1)
	Else
		Return $strs[2]
	EndIf
EndFunc ;==> ProcessData

Func CheckErr($upsresp)
	WriteLog("Enter CheckErr Function", $LOG2FILE, $DBG_DEBUG)
	Local $strs
	If StringLeft($upsresp, 3) == "ERR" Then
		$strs = StringSplit($upsresp, " ")
		if UBound($strs) < 2 Then
			WriteLog("Unknown Error", $LOG2FILE, $DBG_ERROR)
			Return "Uknown Error"
		EndIf
		Return $strs[2]
	Else
		Return "OK"
	EndIf
EndFunc ;==> CheckErr

Func ListUPSVars($upsId, byref $upsVar)
	WriteLog("Enter ListUPSVars Function", $LOG2FILE, $DBG_DEBUG)
	Local $sendstring, $sent, $data
	If $socket == 0 Then
		$upsVar = "0"
		WriteLog("Connection to the server is not established", $LOG2FILE, $DBG_WARNING)
		Return -1
	EndIf
	$sendstring ="LIST VAR " & $upsID  & @CRLF
	$sent = TCPSend($socket, $sendstring )
	If $sent == 0 Then
		$errorstring = "Connection lost"
		$socket = 0
		$upsVar = "0"
		WriteLog($errorstring, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		Return -1
	EndIf
	Sleep(500)
	$data = TCPRecv($socket, 4096)
	If $data == "" Then
		$errorstring = "Connection lost"
		$socket = 0
		$upsVar = "0"
		WriteLog($errorstring, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		Return -1
	EndIf
	$err = CheckErr($data)
	If $err <> "OK" Then
		$errorstring = $err
		If StringInStr($errorstring, "UNKNOWN-UPS") <> 0 Then
			Local $arr[2] = ["UPS %s doesn't exist", $upsId]
			WriteLog($arr, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		EndIf
		$upsVar = "0"
		Return -1
	EndIf
	$upsVar = $data
	Return 0	
EndFunc ;==> ListUPSVars

Func GetUPSDescVar($upsId, $varName, byref $upsVar)
	WriteLog("Enter GetUPSDescVar Function", $LOG2FILE, $DBG_DEBUG)
	Local $sendstring, $sent, $data
	If $socket == 0 Then
		$upsVar = "0"
		WriteLog("Connection to the server is not established", $LOG2FILE, $DBG_WARNING)
		Return -1
	EndIf
	$sendstring ="GET DESC " & $upsID & " " & $varName & @CRLF
	$sent = TCPSend($socket, $sendstring )
	If $sent == 0 Then
		$errorstring = "Connection lost"
		$socket = 0
		$upsVar = "0"
		WriteLog($errorstring, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		Return -1
	EndIf
	$data = TCPRecv($socket, 4096)
	If $data == "" Then
		$errorstring = "Connection lost"
		$socket = 0
		$upsVar = "0"
		WriteLog($errorstring, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		Return -1
	EndIf
	$err = CheckErr($data)
	If $err <> "OK" Then
		$errorstring = $err
		If StringInStr($errorstring, "UNKNOWN-UPS") <> 0 Then
			Local $arr[2] = ["UPS %s doesn't exist", $upsId]
			WriteLog($arr, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		EndIf
		$upsVar = "0"
		Return -1
	EndIf
	$upsVar = ProcessData($data)
	Return 0	
EndFunc ;==> GetUPSDescVar

Func GetUPSVar($upsId, $varName, byref $upsVar, $fallback_value = null, $post_send_delay = null)
	WriteLog("Enter GetUPSVar Function", $LOG2FILE, $DBG_DEBUG)
	Local $sendstring, $sent, $data
	If $socket == 0 Then
		$upsVar = "0"
		WriteLog("Connection to the server is not established", $LOG2FILE, $DBG_WARNING)
		Return -1
	EndIf
	$sendstring ="GET VAR " & $upsID & " " & $varName & @CRLF
	$sent = TCPSend($socket, $sendstring )
	If $sent == 0 Then
		$errorstring = "Connection lost"
		$socket = 0
		$upsVar = "0"
		WriteLog($errorstring, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		Return -1
	EndIf
	If $post_send_delay Then Sleep($post_send_delay)
	$data = TCPRecv($socket , 4096)
	If StringInStr($data, "ERR") <> 0 And $fallback_value <> null Then
		$data = "VAR " & $upsID & " " & $varName & " " & '"' & $fallback_value & '"'
		Local $arrValue[3] = [$varName, $fallback_value, $data]
		Local $arr[2] = ["VarName %s doesn't exist.\n Replaced by fallback Value : %s \nSimulated Data : %s", $arrValue]
		WriteLog($arr, $LOG2FILE, $DBG_ERROR)
	ElseIf $data == "" Then ;connection lost
		$socket = 0
		$upsVar = "0"
		Local $arr[2] = ["Error - Disconnected when retrieved %s", $varname]
		WriteLog($arr, $LOG2FILE, $DBG_ERROR)
		Return -1
	EndIf
	$err = CheckErr($data)
	If $err <> "OK" Then
		$errorstring = $err
		If StringInStr($errorstring, "UNKNOWN-UPS") <> 0 Then
			Local $arr[2] = ["UPS %s doesn't exist", $upsId]
			WriteLog($arr, BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		EndIf
		$upsVar = "0"
		Return -1
	EndIf
	$upsVar = ProcessData($data)
	Return 0
EndFunc ;==> GetUPSVar

Func DisconnectServer()
	WriteLog("Enter DisconnectServer Function", $LOG2FILE, $DBG_DEBUG)
	If $socket <> 0 Then ;already connected
		WriteLog("Connection's Valid to the Nut Server", $LOG2FILE, $DBG_DEBUG)
		TCPSend($socket, "LOGOUT")
		TCPCloseSocket($socket) ;disconnect first
		DeletePortProxy()
		$socket = 0
		WriteLog("Disconnecting from server", BitOr($LOG_GUI, $LOG2FILE), $DBG_NOTICE)
	EndIf
	Return $socket
EndFunc ;==> DisconnectServer

Func ConnectServer()
	WriteLog("Enter ConnectServer Function", $LOG2FILE, $DBG_DEBUG)
	If $socket <> 0 Then ;already connected
		WriteLog("Nut Server already connected - Disconnect", $LOG2FILE, $DBG_DEBUG)
		TCPSend($socket, "LOGOUT")
		TCPCloseSocket($socket) ;disconnect first
		DeletePortProxy()
		$socket = 0
		WriteLog("Disconnecting from server", BitOr($LOG_GUI, $LOG2FILE), $DBG_NOTICE)
	EndIf
	Opt("TCPTimeout",10)
	WriteLog("Connecting to NUT Server", BitOr($LOG_GUI, $LOG2FILE), $DBG_NOTICE)
	If IsFQDN(GetOption("ipaddr")) Then
		$ResolvedHost = ResolveFQDN(GetOption("ipaddr"))
	Else
		$ResolvedHost = GetOption("ipaddr")
	EndIf
	If IsIPV4($ResolvedHost) Then
		$ipv6mode = False
	ElseIf IsIPV6($ResolvedHost) Then
		$ipv6mode = True
	EndIf
	If $ipv6mode Then
		WriteLog("IPV6 Mode", $LOG2FILE, $DBG_DEBUG)
		$PortProxyPort = Random($PortProxyPortMin, $PortProxyPortMax, 1)
		Local $PortProxycmd = "netsh interface portproxy add v4tov6 listenaddress=" & $PortProxyAddress & " listenport=" & $PortProxyPort & " connectport=" & GetOption("port") &" connectaddress=" & $ResolvedHost
		Local $PortProxyFullcmd = @ComSpec & " /c " & $PortProxycmd
		Local $iReturn = RunWait($PortProxyFullcmd, "", @SW_HIDE,  $STDERR_MERGED)
		If @error = 0 Then
			$socket = TCPConnect($PortProxyAddress, $PortProxyPort)
			Local $arrValue[2] = [$PortProxyAddress, $PortProxyPort]
			Local $arr[2] = ["PortProxy Created on %s:%s" , $arrValue]
			WriteLog($arr, BitOr($LOG_GUI, $LOG2FILE), $DBG_NOTICE)
		EndIf
	Else
		WriteLog("IPV4 Mode", $LOG2FILE, $DBG_DEBUG)
		$ipaddr = $ResolvedHost
		$socket = TCPConnect($ipaddr, GetOption("port"))
	EndIf
	If $socket == -1 Then;connection failed
		$haserror = 1
		WriteLog("Connection failed", BitOr($LOG_GUI, $LOG2FILE), $DBG_ERROR)
		Return -1
	Else
		Return 0
	EndIf
EndFunc ;==>ConnectServer