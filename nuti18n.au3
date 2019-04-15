Global $_i18n_LangBase = @ScriptDir
Global $_i18n_Default = 'en-US'
Global $_i18n_Language = 'system'
Global $_i18n_TranslationFile4 = ""
Global $_i18n_TranslationFile2 = ""
Global $_i18n_TranslationFileDefault = ""
Global $i18n_locale = ""
Global $i18n_locale2 = ""

Func _i18n_GetLocaleList()
	Local $LocaleFileList, $i, $sFnameLanguage
	Local $aLocalList[1]
	$LocaleFileList = _FileListToArray ( $_i18n_LangBase, "*.lng", $FLTA_FILESFOLDERS, True)
	$aLocalList[0] = 0
	If @error == 1 Then Return -1
	For $i = 1 to UBound($LocaleFileList) - 1
		$sFnameLanguage = StringRegExpReplace($LocaleFileList[$i], "^.*\\|\..*$", "")
		$sLangDesc = IniRead($LocaleFileList[$i], $sFnameLanguage, "Language_Description", "bidule")
		If $sLangDesc <> "" Then
			$aLocalList[0] = $aLocalList[0] + 1
			ReDim $aLocalList[$aLocalList[0] + 1]
			$iMaxLang = UBound($aLocalList)
			$aLocalList[$iMaxLang - 1] = $sLangDesc & " (" & $sFnameLanguage & ")"
		EndIf
	Next
	Return $aLocalList
EndFunc

Func _i18n_GetLocale()
	If $_i18n_Language == 'system' Then
		Local $aRet = DllCall("Kernel32.dll", "int", "LCIDToLocaleName", "int", "0x" & @OSLang, "wstr", "", "int", 85, "dword", 0)
		Return $aRet[2]
	Else
		Return $_i18n_Language
	EndIf
EndFunc   ;==>LCIDToLocaleName

Func _i18n_GetLocale2()
	Return StringSplit(_i18n_GetLocale(), "-")[1]
EndFunc

Func _i18n_SetLangBase($sFolder)
	$_i18n_LangBase = $sFolder
EndFunc

Func _i18n_SetDefault($sDefault)
	$_i18n_Default = $sDefault
EndFunc

Func _i18n_SetLanguage($sLanguage)
	$_i18n_Language = $sLanguage
EndFunc

Func __($sText)
	$workingdir = @WorkingDir
	FileChangeDir($_i18n_LangBase)
	If ($_i18n_TranslationFile4 = "") Then
		$i18n_locale = _i18n_GetLocale()
		$i18n_locale2 = _i18n_GetLocale2()
		$_i18n_TranslationFile4 = __i18n_GetTranslationFile($i18n_locale)
		$_i18n_TranslationFile2 = __i18n_GetTranslationFile($i18n_locale2)
		$_i18n_TranslationFileDefault = __i18n_GetTranslationFile($_i18n_Default)
	EndIf
	$sControl = IniRead($_i18n_TranslationFile4, $i18n_locale, $sText, "I18N ERROR NO TRANSLATION") ; 4 chars lang code
	If $sControl = "I18N ERROR NO TRANSLATION" Then $sControl = IniRead($_i18n_TranslationFile2, $i18n_locale2, $sText, "I18N ERROR NO TRANSLATION") ; 2 chars lang code
	If $sControl = "I18N ERROR NO TRANSLATION" Then	$sControl = IniRead($_i18n_TranslationFileDefault, $_i18n_Default, $sText, $sText) ; fallback lang
	FileChangeDir($workingdir)
	Return StringReplace($sControl, "\n", @CRLF)
EndFunc

Func __i18n_GetTranslationFile($locale)
	If FileExists($_i18n_LangBase & "\" & $locale & ".lng") Then
		Return $_i18n_LangBase & "\" & $locale & ".lng"
	Else
		Return $_i18n_LangBase & "\" & $_i18n_Default & ".lng"
	EndIf
EndFunc