Func _i18n_GetLocaleList()
	WriteLog("Enter _i18n_GetLocaleList Function", $LOG2FILE, $DBG_DEBUG)
	Local $LocaleFileList, $i, $sFnameLanguage
	Local $aLocalList[1]
	$LocaleFileList = _FileListToArray($_i18n_LangBase, "*.lng", $FLTA_FILESFOLDERS, True)
	$aLocalList[0] = 0
	If @error == 1 Then Return -1
	For $i = 1 to UBound($LocaleFileList) - 1
		$sFnameLanguage = StringRegExpReplace($LocaleFileList[$i], "^.*\\|\..*$", "")
		$sLangDesc = IniRead($LocaleFileList[$i], $sFnameLanguage, "Language_Description", "")
		If $sLangDesc <> "" Then
			$aLocalList[0] = $aLocalList[0] + 1
			ReDim $aLocalList[$aLocalList[0] + 1]
			$iMaxLang = UBound($aLocalList)
			$aLocalList[$iMaxLang - 1] = $sLangDesc & " (" & $sFnameLanguage & ")"
		EndIf
	Next
	Return $aLocalList
EndFunc ;==> _i18n_GetLocaleList

Func _i18n_GetLocale()
	WriteLog("Enter _i18n_GetLocale Function", $LOG2FILE, $DBG_DEBUG)
	If $_i18n_Language == 'system' Then
		Local $aRet = DllCall("Kernel32.dll", "int", "LCIDToLocaleName", "int", "0x" & @OSLang, "wstr", "", "int", 85, "dword", 0)
		Return $aRet[2]
	Else
		Return $_i18n_Language
	EndIf
EndFunc ;==> _i18n_GetLocale

Func _i18n_GetLocale2()
	WriteLog("Enter _i18n_GetLocale2 Function", $LOG2FILE, $DBG_DEBUG)
	Return StringSplit(_i18n_GetLocale(), "-")[1]
EndFunc ;==> _i18n_GetLocale2

Func _i18n_SetLangBase($sFolder)
	WriteLog("Enter _i18n_SetLangBase Function", $LOG2FILE, $DBG_DEBUG)
	$_i18n_LangBase = $sFolder
EndFunc ;==> _i18n_SetLangBase

Func _i18n_SetDefault($sDefault)
	WriteLog("Enter _i18n_SetDefault Function", $LOG2FILE, $DBG_DEBUG)
	$_i18n_Default = $sDefault
EndFunc ;==> _i18n_SetDefault

Func _i18n_SetLanguage($sLanguage)
	WriteLog("Enter _i18n_SetLanguage Function", $LOG2FILE, $DBG_DEBUG)
	$_i18n_Language = $sLanguage
EndFunc ;==> _i18n_SetLanguage

Func __($sText)
	;Logging removed due to too many writes
	Local $tmpText = $sText
	$workingdir = @WorkingDir
	FileChangeDir($_i18n_LangBase)
	If ($_i18n_TranslationFile4 = "") Or $LangChanged == 1 Then
		$i18n_locale = _i18n_GetLocale()
		$i18n_locale2 = _i18n_GetLocale2()
		$_i18n_TranslationFile4 = __i18n_GetTranslationFile($i18n_locale)
		$_i18n_TranslationFile2 = __i18n_GetTranslationFile($i18n_locale2)
		$_i18n_TranslationFileDefault = __i18n_GetTranslationFile($_i18n_Default)
		$LangChanged = 0
	EndIf
	$sControl = IniRead($_i18n_TranslationFile4, $i18n_locale, $sText, "I18N ERROR NO TRANSLATION")
	If $sControl = "I18N ERROR NO TRANSLATION" Then
		$sControl = IniRead($_i18n_TranslationFile2, $i18n_locale2, $sText, "I18N ERROR NO TRANSLATION")
		Local $arr[2] = ["No Translation With 4 Chars Lang Code - Testing with 2 Chars\nSearched Text : %s", $sText]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
	EndIf
	If $sControl = "I18N ERROR NO TRANSLATION" Then
		$sControl = IniRead($_i18n_TranslationFileDefault, $_i18n_Default, $sText, $sText) ; fallback lang
		Local $arr[2] = ["No Translation With 2 Chars Lang Code - Fallback to Default Language\nSearched Text : %s", $sText]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
	EndIf
	FileChangeDir($workingdir)
	Return StringReplace($sControl, "\n", @CRLF)
EndFunc ;==> __()

Func __i18n_GetTranslationFile($locale)
	WriteLog("Enter __i18n_GetTranslationFile Function", $LOG2FILE, $DBG_DEBUG)
	If FileExists($_i18n_LangBase & "\" & $locale & ".lng") Then
		Local $arr[2] = ["Language File %s Exists", $_i18n_LangBase & "\" & $locale & ".lng"]
		WriteLog($arr, $LOG2FILE, $DBG_DEBUG)
		Return $_i18n_LangBase & "\" & $locale & ".lng"
	Else
		Local $arrValue[2] = [$_i18n_LangBase & "\" & $locale & ".lng", $_i18n_LangBase & "\" & $_i18n_Default & ".lng"]
		Local $arr[2] = ["Language File %s Not Exists\nReturn Default File : %s ", $arrValue]
		WriteLog($arr, $LOG2FILE, $DBG_WARNING)
		Return $_i18n_LangBase & "\" & $_i18n_Default & ".lng"
	EndIf
EndFunc ;==> __i18n_GetTranslationFile