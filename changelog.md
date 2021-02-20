## History:

### Version 2.0.7721
#### Fix :
  - Fixed an error preventing WinNUT from starting on a new installation (crash in the encryption function due to a string = Nothing)
  Related to issue #62
  - Since the logging was improved, the view / delete functions of the current log file no longer pointed to the correct files.
  
#### Added :
  - Addition of a small signage icon indicating if the battery is in Charge / Discharge / Charged state.
  When the status is unknown (Connection not established or lost), no status is displayed.
  - Added Russian translation (ru-RU) (thanks to NoGood - #65)
  - Addition of the possibility of creating a Bug Report when WinNUT encounters a critical error in order to retrieve the information necessary to resolve this bug.

#### Changed :
  - Password is now hidden in TextBox (#63)
  - Windows Title Modified to "Windows NUT Client", like older versions (#61) and is now a translatable string
  - Settings Poll Delay are now Expressed In seconds (#64), also renaming "Delay" parameters to "Polling Interval"
  - The translations have been modified accordingly for these 3 fixes
  - Change the way the program version is displayed in notifications
  Only major and minor versions are displayed due to limitations on the number of characters that can be displayed in notifications
  - Toast Popup : Use Short Program Version Instead of complete Program Version
  - Some improvements on Preferences Gui
  - Improved logging when entering or exiting Windows from sleep.
  - The required .Net Framework version is now 4.7.2 (instead of 4.5.2) (Windows 7 sp1, 8, 8.1, 10)
  - Addition of a daily rotation of the logs (previously required a deletion of the journal file when it exceeded 5000KB or else no longer journaling new elements)
  - Translations Improvements


### Version 2.0.7710
#### Fix :
  - Fixed an error generating a bad loading of the default parameters during the first launch following a new installation (without any version previously installed)
  - Fix an error preventing the detection of certain updates if the name of the release does not include the revision version number

#### Added :
  - Username and password are now stored in an encrypted way in registry. (issue #37)
  The conversion of old authentification data is automaticThe conversion of old authentication data is automatic at first launch of WinNUTs new version.
  - Added FSD support that can be provided by NUT server.
  If the NUT server informs that it is initiating a Forced Shutdown, WinNUT will take this into account and initiate the shutdown process in the same way as if it determines that the conditions are required to do so.
  The option must be activated in the settings of WinNUT so that it takes into account the FSD signal (Follow FSD signal).

#### Changed :
  - WinNUT is now distributed under the GNU GPL v3 license (and later).
  - When an update is present, the Changelog window will indicate the Changelog content of each version (both stable and development) present between the currently installed version and the new version.
  In this way, it is easier to take into account all the evolutions / corrections applied from the installed version.


### Version 2.0.7706
#### Fix : 
  - App crash when disconnecting with null streams (Thanks to tgp1994) (Issue #48)
  - Correction of a bad state of the Notify Icon Text when the return value of ups status is not only "OL" (Issue #45)
  - Add -f parameter to force shutdown (Issue #46)
  - Some modifications provided by tgp1994
  - Fixed a bug that could occur when using a value of "0" for the grace period (thanks to jcsmook - pull requests #55)
  - Fixed a bug generating an infinite connection / reconnection loop when the load value is retrieved to "0" during connection (thanks to jcsmook, pull requests #55)
  - Correction of a code error causing the event time not to be updated in the logs.
  - Fixed an error causing an unhandled exception when you do not want to apply the update immediately and the installation file already exists at the location specified for saving.
  - In the case of a left click on the systray icon, WinNUT was restored to its original size and the context menu was opened at the same time.
  This behavior has been corrected and only the context menu opens.
  Restoring the WinNUT window to its original size is caused by a single or double click only.

#### Added : 
  - Addition of a directory containing the translations at the repository level.
	Makes it easier to submit a new translation (or correction) via a fork / pull request (nonPointer idea - issue #35)
  - Translation of the "List UPS Variable's" interface (translation forgot during v2.0)

#### Changed :
  - Some changes on how logs work (Thanks to tgp1994)
  - Modification of the code to recover the power supply frequency of the UPS when it only provides the output frequency (modification made in response to an unsuccessful commit of pull requests #55 - problem encountered by jcsmook)
  - Modification of the generated installer:
		- Addition of a custom image banner
		- Removal of the dependent Windows libraries installed in the WinNUT directory
  - The update process is no longer based on the "changelog.txt" of the repository but on the list of releases via the GitHub APIs.
  This modification has been implemented to avoid reproducing issue #53.
  This modification also brings the possibility of receiving both stable and development updates for those who choose to follow the development channel (the more recent of the two being the proposed update).
  - The changes made to the update process allow the implementation of a more suitable versioning of the type
    [Major Version].[Minor Version].[Automatic Build Version].[Automatic Revision Version]
  - Complete syntax review of Changelog.txt file

### Version 2.0.4.0
#### Fix : 
  - When the "Minimize to tray" option is disabled but the "Start Minimized" and "close to tray" options are active, reducing or closing sends the application to the systray without the possibility of having access to the icon notification
  - When the name of the UPS in the parameters is incorrect, it is no longer possible to display the UPS variable information window.
  - Fixed a typo preventing shutdown at the end of the timeout
  - When the name of the UPS in the parameters is incorrect, the connection is not established and a notification is displayed for 10 seconds.

#### Added :
  - EXPERIMENTAL - Connection function to the nut server with identification and password.
  - When the application is minimized in the systray and a change of connection state to the Nut server or a change of state of the UPS occurs, a notification popup is displayed for 10 seconds.
  - When the application is minimized without being sent to the systray (in task bar), the connection and battery status is displayed in the text of the window in order to be quickly visualized when the application icon is hovered in the task bar.

#### Changed :
  - The application icon has been modified to add an outline to the white shape in order to be visible when it is on a white background
  - The arrows of the icon displayed during reconnection are now in yellow to be more visible due to their small size.
  - The import of an old Ini file has been modified. The function is no longer automatic due to a problem with access rights to the default installation directory. The import is now started manually from the File menu
  - The behavior of the extinction count progress bar has been corrected and behaves in a more fluid and consistent way
  - Restoring the main window from the notification icon only requires a single click (double click previously required)

### Version 2.0.3.0
#### Fix : 
  - Application crash when opening the 'UPS Variable' window
  - No feedback when checking for update (manual or automatic)

### Version 2.0.2.0
#### Fix :
  - All hard-coded translations are translated
  - The log file is now correctly created

#### Added :
  - Some Translations

### Version 2.0.1.0
#### Fix : 
  - Several problems when loading WinNUt as well as an incorrect value for the instantaneous power displayed on the graph of the instantaneous load (Thanks to Ririx02)

### Version 2.0.0.0
Complete rewrite of WinNUT in Visual Basic (the .Net Framework 4.5 is required to run WInNUT)
Modification of the graphic part using the AGauge component (https://github.com/Code-Artist/AGauge) under MIT license.
The AGauge component has been modified to adapt it to the needs of WinNUT and all the sources are supplied with WinNUT.
For the automatic import of the Ini file, the Class written by L. Jerabek was used (https://www.codeproject.com/Articles/21896/INI-Reader-Writer-Class-for-C-VB-NET-and-VBScript)
All the functions present in the AutoIt version are still present in V2 with some additions / modifications:

- Possibilities to export all variables from the UPS to the clipboard (from the "UPS variable" window)
- It is no longer necessary to run WinNUT as administrator
- The configuration of WinNUT is no longer in an ini file but directly stored in the registry (CURRENT_USER)
- Ability to automatically import your old config file (ups.ini). The import is automatic when WinNut is launched 
  if an ups.in file is present in the same directory as the executable.
- The Update part no longer uses a second program to perform the update, the releases being in the form of an msi file, 
  WinNUT is closed during the update and then reopened once the new version is installed.
- It is now possible to uninstall WinNUT from the "Program / Functionality" part of Windows
- WinNUT no longer suffers from detection as a Virus since it is no longer developed under AutoIT
- WinNUT is also natively multi-language, so it is no longer necessary to select the language from the interface.
  The default language, however, is always EN-Us in case your language is not supported.
  An Xls file is available to send me a translation in a new language.
  This will require an addition in a new build and therefore an update of WinNut

### Version 1.8.0.3
#### Changed : 
  - WinNUT is ready to update to version 2

### Version 1.8.0.2
#### Fix :
  - Correction of a bad state of the UPS when the return value is not only "OL"

### Version 1.8.0.1
#### Fix :
  - Rounded Value for second displayed value in Graph

### Version 1.8.0.3
#### Changed :
  - WinNUT is ready to update to version 2

### Version 1.8.0.2
#### Fix :
  - Correction of a bad state of the UPS when the return value is not only "OL"

### Version 1.8.0.1
#### Fix :
  - Rounded Value for second displayed value in Graph

### Version 1.8.0.0
#### Fix :
  - Significant slowdown caused by the Logging functionality and memory leak caused by this same functionality
  - Working Fallback Value on GetUPSVar
  - When the preferences are open but no modification is made, there is no longer a reconnection when clicking on the OK / Apply buttons (which could generate a flickering of the dials and unnecessary entries in the logs)
  - Incorrect calculation of the instantaneous power used

#### Added :
  - Compatibility with IPV6 (Experimental)
  - Capacity to update directly from WinNut (Addition of an executable Updater packaged in WinNUT and therefore installed with WinNut on its first launch)
  - Ability to display a second value on graphs (text value only and value not already used)

#### Changed :
  - The executable is no longer called "upsclient.exe" but "WinNUT-client.exe"
  - DrawDial function moved to nutDraw.au3 file
  - When changing languages, it is no longer necessary to restart WinNUT. The window closes and reopens automatically with the new language.

### Version 1.7.9.9
Not Released
#### Change/Add :
  - Graphic redesign of the icons as well as the setting of dynamic icons according to the connection/reconnection state, the connection state of the inverter and also the charge/discharge state of the battery.
  All of the icons are packaged in a DLL which will be installed with the executable WinNut during its first launch.
  - Logging Features

### Version 1.7.3.1
#### Fix :
  - Disconnect menu option is now disabled when connection to NUT server is not established
  - Correction of a problem with information refresh when the connection is established after starting WinNUT

#### Added :
  - A notification message is displayed when the connection could not be re-established within the time allowed

#### Changed :
  - Better syntactic respect of the code

### Version 1.7.3.0
#### Added :
  - Compatibility with IPV6 (Experimental)
  - Disconnect Option in Menu

### Version 1.7.2.2
No Build Release
#### Fix :
  - Fixed some graphical issues

#### Added :
  - German translation
  - Ability to set the input frequency manually in case the UPS does not return this information. The value is set automatically when the UPS supplies it.
  - Possibility to choose the type of stop action (Shutdown/Sleep/Hibernate)

### Version 1.7.2.1
#### Fix :
  - Missing Translation

#### Changed :
  - Some default parameter values
  - README.md 

### Version 1.7.2.0
#### Added :
  - Support for multi language
  - Auto reconnection 

### Version 1.7.0.5
#### Fix :
  - Some Source Cleanup

#### Added :
  - Added ability to estimate the load level and run time of the UPS when he does not provide the information itself (or is inconsistent).

### Version 1.7.0.0
#### Fix :
  - Ups VarList Gui

#### Added :
  - Custom Shutdown Options.
  - Allow software to not shutdown immediately and permit backup data before shutdown

### Version 1.6.6.0
#### Added :
  - Option Close to tray
  - Minimize on startup
  - Aut2exe directive added (#pragma directive)
  - AutoItWrapper Directive removed

### Version 1.6.5.0
#### Fix :
  - Show/Hide Tray Icon when Gui is visible or not

### Version 1.6.4.0
#### Fix : 
  - Change adlibenable / disable to register / unregister and some other code modification to make it compil on AutoIt v3.3.14

#### Added :
  - Tray icon tooltip with load, charge and status
  - Start with Windows Option
  - Prevent shutdown when UPS not connected but NUT is running (from v1.6.2 written by crazytiti see https://github.com/crazytiti/Windows-Nut)

#### Changed : 
  - Some code optimisation
  - Some aesthetic improvement of GUI
  - Tray icon improvment

### Version 1.5.0.0
#### Added : 
  - Calibration for all clocks except battery charge.
  Now you can suite the program to your ups readings.
  For example if you are in USA and your UPS supplies 30V when fully charged set the values as follows :
    - Input Voltage Min 50 , Input Voltage Max 150
    - Output Voltage Min 50 , Output Voltage Max 150 
    - Input Frequency Min 30 , Input Frequency Max 80 (Center at 60Hz).
    - Battery Voltage Min 0 , Battery Voltage Max 50
  This is of course only an example and you may set the values the way you like.
  At this moment no checking is done on values used so please be reasonable with what you enter (no negative numbers , max less the min etc).
  In order to change calibration go to Settings->Preferences.
  Select Calibration TAB and change the values as required.
  Then press OK or APPLY.
  After you close the Preferences window clocks will be refreshed and updated with new settings.
  - Minimize to tray (Requested by druidtaliesin).
  In order to toggle go to Settings->Preferences.
  Select Misc TAB and check the appropriate option.
  Click OK or APPLY to save the changes
  It is off by default.
  - Shutdown PC if battery charge lower then specified value
  In order to change go to Settings->Preferences.
  Select Misc TAB and type in the battery charge percentage at which
  you want your pc to shutdown if UPS is offline.
  Click OK or APPLY to save the changes.
  ATTENTION this option is DANGEROUS!
  The computer is shutdown forcibly so if you have any unsaved documents and set the value too high while UPS was offline , you might lose data.
  Please use with care.
  By default the value is 0 which effectively disables this option.
  Notice : Compiled with latest release of AutoIT which hopefully should speed things up a little bit.
  Notice : Rewritten most of configuration management code.
  It is much more readable now and also much easier to add new options.
