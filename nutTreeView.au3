#include-once
#include <GUIConstantsEX.au3>
#include <TreeViewConstants.au3>
#include <WinAPI.au3>
#Include <GuiTreeView.au3>

Global Const $s_TVITEMEX = "uint;uint;uint;uint;ptr;int;int;int;int;uint;int"
Global $TVM_SETITEM = 0

Func _GUICtrlTreeViewSetStateIcon($i_treeview, $h_item = 0, $s_iconfile = "", $i_iconID = 0)
	$h_item = _GUICtrlTreeView_GetItemHandle($i_treeview , $h_item)
    ;$h_item = _TreeViewGetItemHandle($i_treeview, $h_item)
    If $h_item = 0 Or $s_iconfile = "" Then Return SetError(1, 1, False)

    Local $st_TVITEM    = DllStructCreate($s_TVITEMEX)
    If @error Then Return SetError(1, 1, False)

    Local $st_icon = DllStructCreate("int")
    Local $i_count = DllCall("shell32.dll", "int", "ExtractIconEx", _
                                                "str", $s_iconfile, _
                                                "int", $i_iconID, _
                                                "ptr", 0, _
                                                "ptr", DllStructGetPtr($st_icon), _
                                                "int", 1)
    If $i_count[0] = 0 Then Return 0

    Local $h_imagelist = GUICtrlSendMsg($i_treeview, $TVM_GETIMAGELIST, 0, 0)
    If $h_imagelist = 0 Then
        $h_imagelist = DllCall("comctl32.dll", "hwnd", "ImageList_Create", _
                                                    "int", 16, _
                                                    "int", 16, _
                                                    "int", 0x0021, _
                                                    "int", 0, _
                                                    "int", 1)
        $h_imagelist = $h_imagelist[0]
        If $h_imagelist = 0 Then Return SetError(1, 1, False)

        GUICtrlSendMsg($i_treeview, $TVM_SETIMAGELIST, 2, $h_imagelist)
                            EndIf

    Local $h_icon = DllStructGetData($st_icon, 1)
    Local $i_icon = DllCall("comctl32.dll", "int", "ImageList_AddIcon", _
                                                "hwnd", $h_imagelist, _
                                                "hwnd", $h_icon)
    $i_icon = $i_icon[0]

    ; Index 0 is invalid for the state image so we add the icon again
    ; to get an index greater than zero.
    If $i_icon = 0 Then
        $i_icon = DllCall("comctl32.dll", "int", "ImageList_AddIcon", _
                                                    "hwnd", $h_imagelist, _
                                                    "hwnd", $h_icon)
        $i_icon = $i_icon[0]
    EndIf

    DllCall("user32.dll", "int", "DestroyIcon", "hwnd", $h_icon)

    Local $u_mask = $TVIF_STATE

    DllStructSetData($st_TVITEM, 1, $u_mask)
    DllStructSetData($st_TVITEM, 2, $h_item)
    ; The index needs shifted left 12 bits.
    DllStructSetData($st_TVITEM, 3, BitShift($i_icon, -12))
    DllStructSetData($st_TVITEM, 4, $TVIS_STATEIMAGEMASK)
	
    Return GUICtrlSendMsg($i_treeview, $TVM_SETITEM, 0, DllStructGetPtr($st_TVITEM))
EndFunc   ;==>_GUICtrlTreeViewSetStateIcon


Func _GUICtrlTreeView_FindItemEx1($hWnd, $sPath, $sDelimiter = ".", $hStart = 0)
	Local $iIndex, $aParts

	$iIndex = 1
	$aParts = StringSplit($sPath, $sDelimiter)
	If $hStart = 0 Then $hStart = _GUICtrlTreeView_GetFirstItem($hWnd)
	While ($iIndex <= $aParts[0]) And ($hStart <> 0)
		If StringStripWS(_GUICtrlTreeView_GetText($hWnd, $hStart), 3) = StringStripWS($aParts[$iIndex], 3) Then
			If $iIndex = $aParts[0] Then Return $hStart
			$iIndex += 1
			_GUICtrlTreeView_Expand($hWnd, $TVE_EXPAND, $hStart)
			$hStart = _GUICtrlTreeView_GetFirstChild($hWnd, $hStart)
		Else
			$hStart = _GUICtrlTreeView_GetNextSibling($hWnd, $hStart)
			_GUICtrlTreeView_Expand($hWnd, $TVE_COLLAPSE, $hStart)
		EndIf
	WEnd
EndFunc   ;==>_GUICtrlTreeView_FindItemEx


Func _GUICtrlTreeViewGetTree1($i_treeview, $s_sep_char , $h_item)
	If Not _WinAPI_IsClassName ($i_treeview, "SysTreeView32") Then
		Return SetError(-1, -1, "")
	EndIf
	Local $szPath = "", $hParent
	If Not IsHWnd($i_treeview) Then $i_treeview = GUICtrlGetHandle($i_treeview)

	If $h_item = 0 Then $h_item = _SendMessage($i_treeview, $TVM_GETNEXTITEM, $TVGN_CARET, 0)
	;$h_item = GUICtrlSendMsg($i_treeview, $TVM_GETNEXTITEM, $TVGN_CARET, 0)
	If $h_item > 0 Then
		$szPath = _GUICtrlTreeView_GetText($i_treeview, $h_item)

		Do; Get now the parent item handle if there is one

			$hParent = _SendMessage($i_treeview, $TVM_GETNEXTITEM, $TVGN_PARENT, $h_item)
			If $hParent > 0 Then $szPath = _GUICtrlTreeView_GetText($i_treeview, $hParent) & $s_sep_char & $szPath
			$h_item = $hParent
		Until $h_item <= 0
	EndIf

	Return $szPath
EndFunc   ;==>_GUICtrlTreeViewGetTree


func _addPath($i_treeview , $path )

	Local $prevpath , $i ,$tempath
	if $path == "" Then ;empty path so nothing to add
		return 0
	EndIf

	$pathexist = _GUICtrlTreeView_FindItemEx1($i_treeview, $path);check path already exists in tree
	if $pathexist <> 0 Then
		return $pathexist ;return the path itself (it is required for correct recursive operation)
	EndIf

	$pathitems = StringSplit($path , ".")
	if ($pathitems[0] == 1 ) Then; recursion stop condition (path is single leaf attached to root)
		return _GUICtrlTreeView_InsertItem ($i_treeview  , $pathitems[1])
	Else
		$tempath =""
		for $i = 1 to $pathitems[0] -1
			$tempath = $tempath & $pathitems[$i] & "."
		Next
		$tempath = StringLeft($tempath , StringLen($tempath) -1) ; strip the last "." which is not needed
		$temptree = _addPath($i_treeview , $tempath)
		return _GUICtrlTreeView_InsertItem($i_treeview , $pathitems[$pathitems[0]] ,$temptree) ; attach the current item to subtree
	EndIf
EndFunc



Func _SetIcons($i_treeview, $h_item)
	Local $h_child , $cur_text , $next_item
	if ($h_item == 0) then
		$h_item = GUICtrlSendMsg($i_treeview, $TVM_GETNEXTITEM, $TVGN_CHILD, $h_item)
	EndIf


	While $h_item > 0
		$h_child = GUICtrlSendMsg($i_treeview, $TVM_GETNEXTITEM, $TVGN_CHILD, $h_item)
		If $h_child > 0 Then
			_GUICtrlTreeViewSetStateIcon($i_treeview ,$h_item , "shell32.dll", 4)
			_SetIcons($i_treeview, $h_child)
		EndIf
		$h_item = GUICtrlSendMsg($i_treeview, $TVM_GETNEXTITEM, $TVGN_NEXT, $h_item)
	WEnd
	return
EndFunc   ;==>_TreeViewExpandTree