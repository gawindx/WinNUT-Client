#include <Color.au3>

Func DrawDial($left, $top, $basescale, $title, $units, ByRef $value, ByRef $needle, ByRef $sndvalue, $sndunits = null, $scale = 1, $leftG = 20, $rightG = 70)
	WriteLog("Enter DrawDial Function", $LOG2FILE, $DBG_DEBUG)
	Local $group = 0

	$group = GUICreate(" " & $title, 150, 120, $left, $top, BitOR($WS_CHILD, $WS_DLGFRAME), $WS_EX_CLIENTEDGE, $gui)
	GUISetBkColor($clock_bkg, $group)
	GuiSwitch($group)
	GuiCtrlCreateLabel($title, 0, 0, 150, 14, $SS_CENTER)

	For $x = 0 To 100 Step 10
		If StringinStr($x / 20, ".") = 0 Then
			GUICtrlCreateLabel("", $x * 1.2 + 15, 15, 1, 15, $SS_BLACKRECT)
			GuiCtrlSetState(-1, $GUI_DISABLE)
			If $x < 100 Then
				$test = GUICtrlCreateLabel("", $x * 1.2 + 16, 15, 11, 5, 0)
				GuiCtrlSetState(-1, $GUI_DISABLE)
				If $x < $rightG And $x > $leftG Then
					GUICtrlSetBkColor($test, 0x00ff00)
				Else
					GUICtrlSetBkColor($test, 0xff0000)
				EndIf
			EndIf
			$scalevalue = $basescale + $x / $scale
			Switch $scalevalue
				Case 0 To 9
					GuiCtrlCreateLabel($scalevalue, $x * 1.2 + 13, 25, 20, 10)
				Case 10 To 99
					GuiCtrlCreateLabel($scalevalue, $x * 1.2 + 10, 25, 20, 10)
				Case 100 To 1000
					GuiCtrlCreateLabel($scalevalue, $x * 1.2 + 7, 25, 20, 10)
			EndSwitch
			GUICtrlSetFont(-1, 7)
		Else
			GUICtrlCreateLabel("", $x * 1.2 + 15, 15, 1, 5, $SS_BLACKRECT)
			GuiCtrlSetState(-1, $GUI_DISABLE)
			$test = GUICtrlCreateLabel("", $x * 1.2 + 16, 15, 11, 5, 0)
			If $x < $rightG And $x > $leftG Then
				GUICtrlSetBkColor($test, 0x00ff00)
			Else
				GUICtrlSetBkColor($test, 0xff0000)
			EndIf
		EndIf
	Next
	If $units == "%" Then
		$value = GUICtrlCreateLabel(0, 90, 100, 40, 15, $SS_RIGHT)
	Else
		$value = GUICtrlCreateLabel(220, 90, 100, 40, 15, $SS_RIGHT)
	EndIf
	$label2 = GUICtrlCreateLabel($units, 134, 100, 16, 15, $SS_LEFT)
	If $sndunits <> null Then
		If $sndunits == "%" Then
			$sndvalue = GUICtrlCreateLabel(0, 90, 85, 40, 15, $SS_RIGHT)
		Else
			$sndvalue = GUICtrlCreateLabel(220, 90, 85, 40, 15, $SS_RIGHT)
		EndIf
		$label3 = GUICtrlCreateLabel($sndunits, 134, 85, 16, 15, $SS_LEFT)
	EndIf
	$needle = GUICtrlCreateGraphic(10, 35, 120, 60)
	If BitAND(WinGetState($gui), $WIN_STATE_MINIMIZED) Then
		GuiSetState(@SW_HIDE, $group)
	Else
		GuiSetState(@SW_SHOW, $group)
	EndIf
	$result = $group
	Return $group
EndFunc ;==> DrawDial

#comments-start
	This function returns the x and y coordinates of a point which is located
	on an intersection of a circle with radius of 60 centered at (60,70) and a line
	passing through center of a circle and a point whose x coordinate is passed as a
	parameter .y coordinate of that point is is assumed to be 10
	This is to calculate a point inside analogue clock where needle drawing will end
#comments-end
Func getPoint($coordX )
	Local $result[2]
	Local $coordY = 10
	Local $a1, $b1, $a, $b, $c, $b24ac, $result1X, $result2X, $result1Y, $result2Y
	If $coordX == 65 Then
		$result[0] = 65
		$result[1] = 10
		Return $result
	EndIf
	$a1 = (70 - $coordY ) / (65 - $coordX );determine tangent of a line passing through given point and circle center
	$b1 = 70 - ($a1 * 65 )
	$a = ($a1 * $a1) + 1
	$b = -130 - (140 * $a1) + (2 * $a1 * $b1 )
	$c = ($b1 * $b1 ) - (140 * $b1 ) + 5525
	$b24ac = sqrt(($b * $b) - (4 * $a * $c))
	$result1X = ( -$b + $b24ac) / (2* $a)
	$result2X = ( -$b - $b24ac) / (2* $a)
	$result1Y = $a1 * $result1X + $b1
	$result2Y = $a1 * $result2X + $b1
	If $result1Y < 70 Then
		$result[0] = Round($result1X)
		$result[1] = Round($result1Y)
	Else
		$result[0] = Round($result2X)
		$result[1] = Round($result2Y)
	EndIf
	Return $result
EndFunc ;==> getPoint

#comments-start
	This function draws a needle from center of a circle located at 60,70 to a point
	whose x is passed as a parameter and y assumed to be 10
	$x : X coordinate of the point
	$color : the color of the needle
	$windhandle : handle of the main window ($gui)
	$chandle : handle of the control for the drawing (graphic control)
#comments-end
Func DrawNeedle($x, $color, $winhandle, $chandle)
	If $painting <> 0 Then
		Return
	EndIf
	$painting  = 1
	Local $point[2], $pic_hWnd, $user_dll, $gdi_dll, $l_hdc, $pen, $obj_orig
	$point = getPoint($x)
	$pic_hWnd = ControlGetHandle($winhandle, "", $chandle)
	$user_dll = DllOpen("user32.dll")
	$gdi_dll = DllOpen("gdi32.dll")
	$l_hdc = DLLCall($user_dll, "int", "GetDC", "hwnd", $pic_hWnd)
	$pen = DLLCall($gdi_dll, "int", "CreatePen", "int", 0x0, "int", 1, "int", $color)
	$obj_orig = DLLCall($gdi_dll, "int", "SelectObject", "int", $l_hdc[0], "int", $pen[0])
	DLLCall($gdi_dll, "int", "MoveToEx", "int", $l_hdc[0], "int", $point[0], "int", $point[1], "int", 0)
	DLLCall($gdi_dll, "int", "LineTo", "int", $l_hdc[0], "int", 64, "int", 70) ;draw filled triangle
	DLLCall($gdi_dll, "int", "LineTo", "int", $l_hdc[0], "int", 66, "int", 70) ;which is the needle
	DLLCall($gdi_dll, "int", "LineTo", "int", $l_hdc[0], "int", $point[0], "int", $point[1])
	DLLCall($gdi_dll, "int", "LineTo", "int", $l_hdc[0], "int", 65, "int", 70)
	DLLCall($gdi_dll, "int", "SelectObject", "int", $l_hdc[0], "int", $obj_orig[0])
	DLLCall($gdi_dll, "int", "DeleteObject", "int", $pen[0])
	
	$l_hdc = DLLCall($user_dll, "int", "ReleaseDC", "hwnd", $pic_hWnd, "int", $l_hdc[0])
	DllClose($gdi_dll)
	DllClose($user_dll)
	$painting  = 0
EndFunc ;==> DrawNeedle

Func RGBtoBGR($color)
	Local $redcomponent, $greencomponent, $bluecomponent
	
	$redcomponent = _ColorGetRed($color)
	$greencomponent = _ColorGetGreen($color)
	$bluecomponent = _ColorGetBlue($color)
	
	$bluecomponent = String (Hex($bluecomponent))
	$greencomponent = String(Hex($greencomponent))
	$redcomponent = String(Hex($redcomponent))
	
	$bluecomponent = StringRight($bluecomponent, 2)
	$greencomponent = StringRight($greencomponent, 2)
	$redcomponent = StringRight($redcomponent, 2)
	$result = "0x" & $bluecomponent & $greencomponent & $redcomponent
	Return Number($result)
EndFunc ;==> RGBtoBGR

Func GetColor($window, $control)
	Local $cPos = 0
	Local $wPos = 0
	Local $xPos, $yPos
	$cPos = ControlGetPos($window, "", $control)
	$wPos = WinGetPos($window)
	If @error == 1 Then
		Return -1
	EndIf
	$xPos = Round($cPos[0] + ($cPos[2] / 2)) + $wPos[0]
	$yPos = Round($cPos[1] + ($cPos[3] / 2)) + $wPos[1]	
	$result = PixelGetColor($xPos, $yPos)
	Return $result
EndFunc ;==> GetColor

Func repaintNeedle($needle, $value, $whandle, $min = 170, $max = 270)
	$value = Round($value)
	If $value < $min Then
		$value = $min
	EndIf
	If $value > $max Then
		$value = $max
	EndIf
	$setneedle = ($value - $min) / (($max - $min) / 100)

	DrawNeedle(15 + $setneedle, 0x0, $whandle, $needle)
EndFunc ;==> repaintNeedle


Func SetColor($color, $window, $control)
	Local $prevColor
	$prevColor = getColor($window, $control)
	If $prevColor == $color Then ;has same color so no need to update
		Return
	EndIf
	GuiCtrlSetBkColor($control, $color)
EndFunc ;==> SetColor

Func DrawError($left, $top, $message) ;message to write over the dial if some error happened
	$result = GuiCtrlCreateLabel($message , $left + 25, $top + 50, 110, 50, $BS_CENTER)
	GuiCtrlSetColor(-1,0xff0000)
	Local $arr[2] = ["Draw Error - Message : %s", $message]
	WriteLog($arr, $LOG2FILE, $DBG_ERROR)
	Return $result
EndFunc ;==> DrawError