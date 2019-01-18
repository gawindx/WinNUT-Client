#include-once

Global $errorstring = "no error"
Global $haserror = 0
Global $ProgramDesc = "Windows NUT Client"
Global $ProgramVersion = ""

Global $inputv = 0
Global $outputv = 0
Global $inputf = 0
Global $battv = 0
Global $upsch = 0
Global $upsl = 0
Global $inputVol = 0
Global $outputVol = 0
Global $inputFreq = 0
Global $battVol = 0
Global $upsLoad = 0
Global $battCh = 0
Global $battruntime = 0

Global $upsstatus = 0
Global $needle1 = 0
Global $needle2 = 0
Global $needle3  = 0
Global $needle4 = 0
Global $needle5 = 0
Global $needle6 = 0
Global $dial1 = 0
Global $dial2 = 0
Global $dial3  = 0
Global $dial4 = 0
Global $dial5 = 0
Global $dial6 = 0

Global $upsonline = 0
Global $upsonbatt = 0
Global $upslowbatt = 0
Global $upsoverload = 0
Global $mfr = ""
Global $name = ""
Global $serial = ""
Global $firmware = ""
Global $battrtimeStr = "00:00"

Global $TreeView1
Global $varselected
Global $varvalue
Global $vardesc

Global $settingssubMenu,$exitMenu,$editMenu,$aboutMenu,$reconnectMenu,$listvarMenu

Global $guipref = 0
Global $guiabout = 0
Global $gui
Global $upsmfr,$upsmodel,$upsserial,$upsfirmware,$toolb,$exitb

Global $wPanel = 0

Global $idTrayExit,$idTrayPref,$idTrayAbout
Global $runasexe = False

Global $Active_Countdown = 0,$Suspend_Countdown = 0
Global $ShutdownDelay, $grace_time
Global $Running_Shutdown, $lbl_countdown, $lbl_ups_status
Global $guishutdown, $Grace_btn, $Shutdown_btn