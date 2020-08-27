# Installation
To use it, please follow the following steps:
1. Get the [last available Releases](https://github.com/gawindx/WinNUT-Client/releases)
2. Install WinNUT using the "WinNUT-Setup.msi" file obtained previously
3. If you were using an older version of WinNUT (v1.x), copy your "ups.ini" configuration file to the WinNUT installation directory (by default "C: \ Program Files (x86) \ WinNUT-client ") for an automatic import of your parameters during the first launch
4. Start WinNUT V2 and modify the parameters according to your needs

# Translation
WinNUT V2 is natively multilingual, so it is no longer necessary to select your language from the software interface.
Currently, WinNUT supports:
- English
- German
- French

### To add / correct a language

  1. Get the file [Base_Translation.xls](./Base_Translation.xls)
  2. Perform the necessary translations
  3. Save this file in csv format (IMPORTANT)
  4. Create a gist via [gist github](https://gist.github.com) and paste the contents of the previously created csv file
  5. Open a new issue and tell me:
- the link of the gist
- the language to create / correct

Your translation / correction will be added on a new version and will thus be available to the entire community.

# Update WinNut

Since version 1.8.0.0, WinNut includes a process for checking for updates.
This process can be started automatically on startup or manually on demand (and you can choose whether you want to update from the stable or development version)

During this update, the new installation file will be automatically downloaded and you can easily update your version of WinNut.

This process is fully integrated and no longer requires a second executable.

# Third Party Components / Acknowledgments

WinNUT uses:
- a modified version of AGauge initially developed by [Code-Artist](https://github.com/Code-Artist/AGauge) and under MIT license
- Class IniReader developed by [Ludvik Jerabek](https://www.codeproject.com/Articles/21896/INI-Reader-Writer-Class-for-C-VB-NET-and-VBScript)
