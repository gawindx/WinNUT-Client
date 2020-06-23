# WinNUT-Client
This is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
Now you don't need to vnc or ssh into the box to see how your ups is doing!

WinNut is now compatible with a Nut server accessible in IPV6 (Experimental - See Below).
This change requires launching the program with Administrator rights

This project is originally forked from https://sourceforge.net/projects/winnutclient/

# Installation
In order to use please perform the following steps :
1. Extract the files from the zip
2. Edit the ups.ini to suit your NUT server settings
(You may skip this step and do it later from the gui)
3. Go to Settings->Language and choose your preferred Language
4. Run the exe and Go to Settings->Preferences to set your connection properties or change Color of the panels.

# Translation
Since version 1.7.2.0, WinNut supports multilanguage.
The language files are integrated as a resource in the executable.

### To add a new language

 1. Copy the file en-US.lng and rename the copy according to the new language
 2. For each sentence, the reference sentence is to the left of the equal sign and the translation into your language is to the right of the equal sign.
Replace each part on the right with your translation.
 3. Restart WinNut for your file to be taken into account.
 4. If the language file corresponds to the system language, it should
    be taken into account and select when starting WinNut
 5. If this is not the case, go to the "Settings-> Language" menu to
    select your language. You will then be prompted to restart WinNut
    again (I am currently working on an instantaneous takeover of the
    language change, coming in a future version).
##### IMPORTANT: The "Language_Description" key is the name of your language.

### If you want your translation file to be integrated too:
#### Solution 1 
Contact me and send me your translation file for integration at the repository level.

# Manual Install - You do not wish to make the community benefit (not good ...).

 1. Download the source.
 2. Install "Autoit V3.3" if you do not have it
 3. Add your translation file to the Language directory (See above)
 4. Run the script a first time to update the file "include.au3"
 5. Compile WinNUT-Updater.au3 to generate Updater (Required for step 6)
 6. Now you can compile the script (WinNUT-client.au3), your language file will be integrated into the executable.

# IPV6
WinNUT is now compatible with IPV6 but to make this possible, it requires an increase in performance rights.
For this reason, it is now necessary to validate the Windows UAC when it is launched (whether you are in IPV4 or IPV6).

In any case, WinNUT will always try to go through IPV4 (which is its natural mode) before going into IPV6.

The IPV6 mode is obtained by creating a portProxy via the Windows netsh command, that is to say that it creates a connection between your computer and your Nut server, then WinNut connects to the portProxy created on your Computer which then forwards all communications to your Nut Server.

This Portproxy is automatically destroyed when communication between WinNut and the Nut server is lost or when WinNUT is closed

# WinNut update

Since version 1.8.0.0, WinNut incorporates an update verification process.
This process can be started automatically at startup or manually on demand (and you can select if you want to update from the Stable or development version)

During this update, WinNut is automatically closed then, once the executable is updated, automatically restarted with the existing parameters.

# Detected as Virus
Some Anitviruses can detect WinNUT-client.exe (as well as WinNUT-Updater.exe) because they were compiled with AutoIt and are therefore recognized as programs that can execute scripts (see this link: https://www.autoitscript.com/wiki/AutoIt_and_Malware).
If you do not wish to trust the releases pre-compiled by me, I recommend that you install it using method No. 2 which will allow you to ensure that this program is safe.
In this way, you are very careful to check that the source code does not contain any virus or malicious script.

Warning: Whatever your installation method, your antivirus will always be able to detect a malicious program in the compiled program.

In this case, you just need to create an exception within your AntiVirus - the simplest being to exclude the entire directory otherwise you will be obliged to create an exception for both the WinNUT-client part and for WinNUT-Updater.

## Virus Total Report
The 2 executables have been subjected to the Virus detection engine of the VirusTotal site and you will find below the links for the executables of the current version (1.8.0.0):
- WinNUT-client.exe : https://www.virustotal.com/gui/file/9c8f478c04bdc3be40b80b833bee4f30ab35e3c2f5ae5bbb2a68cb47594f977d/detection
- WinNUT-Updater.exe : https://www.virustotal.com/gui/file/2b1cc1bd8f4f7c7155e6363ba9a255976dc702fe71124448ffbdec1c845d5ae9/detection

## Addition of an Exclusion

### Microsoft WinDefender
See here : https://support.microsoft.com/en-us/help/4028485/windows-10-add-an-exclusion-to-windows-security
