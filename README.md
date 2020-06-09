# WinNUT-Client
This is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
Now you don't need to vnc or ssh into the box to see how your ups is doing!

WinNut is now compatible with a Nut server accessible in IPV6 (Experimental).
This change requires launching the program with Administrator rights

This project is forked from https://sourceforge.net/projects/winnutclient/

# Installation
In order to use please perform the following steps :
1. Extract the files from the zip
2. Edit the ups.ini to suit your NUT server settings
(You may skip this step and do it later from the gui)
3. Run the exe and Go to Settings->Preferences to set your connection properties or change Color of the panels.

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

#### Solution 2 - You do not wish to make the community benefit (not good ...).

 1. Download the source.
 2. Install "Autoit V3.3" if you do not have it
 3. Add your translation file to the Language directory
 4. Run the script a first time to update the file "include.au3"
 5. Now you can compile the script, your file will be integrated into the executable.

And VOILA!!!
