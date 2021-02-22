# WinNUT-Client

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/donate?hosted_button_id=FAFJ3ZKMENGCU)

## Installation
To use it, please follow the following steps:
1. Get the [last available Releases](https://github.com/gawindx/WinNUT-Client/releases)
2. Install WinNUT-Client using the "WinNUT-Setup.msi" file obtained previously
3. If you were using an older version of WinNUT (v1.x), copy your "ups.ini" configuration file to the WinNUT-Client installation directory (by default "C:\Program Files(x86)\WinNUT-Client ") for an automatic import of your parameters during the first launch
4. Start WinNUT V2 and modify the parameters according to your needs

## Specific Configuration

### For Synology NAS 
If your NUT server is hosted on a Synology NAS, be sure to provide the following connection information (default):
Login: upsmon
Password: secret

It will probably be necessary to allow the WinNUT-Client IP to communicate with the NUT server.
*See issue 47 for more information, specifically [this commentary](https://github.com/gawindx/WinNUT-Client/issues/47#issuecomment-759180793).*

### 
## Translation
WinNUT-Client V2 is natively multilingual, so it is no longer necessary to select your language from the software interface.
Currently, WinNUT-Client supports:
- English
- German
- French
- Simplified Chinese
- Russian

#### To add / correct a language

##### Method 1 (preferred)
1. Fork this repository
2. In the translation directory:

	For a new translation:
	1. Use the new_translation.csv file to translate the texts
	2. Save this file in xx-XX corresponding to the language code

	For a correction:
	1. Edit the wrong language file
	2. Make the necessary corrections

3. Save it instead
4. Create a pull request on this repository to take into account the translation.

##### Method 2
  1. Get the file [new_translation.csv](./Translation/new_translation.csv)
  2. Perform the necessary translations
  3. Save this file in csv format (IMPORTANT)
  4. Create a gist via [gist github](https://gist.github.com) and paste the contents of the previously created csv file
  5. Open a new issue and tell me:
	- the link of the gist
	- the language to create / correct

Your translation / correction will be added on a new version and will thus be available to the entire community.

## Update WinNUT-Client

Since version 1.8.0.0, WinNUT-Client includes a process for checking for updates.
This process can be started automatically on startup or manually on demand (and you can choose whether you want to update from the stable or development version)

During this update, the new installation file will be automatically downloaded and you can easily update your version of WinNUT-Client.

This process is fully integrated and no longer requires a second executable.

## Third Party Components / Acknowledgments

WinNUT-Client uses:
- a modified version of AGauge initially developed by [Code-Artist](https://github.com/Code-Artist/AGauge) and under [MIT license](https://opensource.org/licenses/MIT)
- Class IniReader developed by [Ludvik Jerabek](https://www.codeproject.com/Articles/21896/INI-Reader-Writer-Class-for-C-VB-NET-and-VBScript) and under [The Code Project Open License](http://www.codeproject.com/info/cpol10.aspx)
- Newtonsoft.Json Library is used in this Project [Newtonsoft.json Website](https://www.newtonsoft.com/json) and under [MIT license](https://opensource.org/licenses/MIT)

## License

WinNUT-Client is a NUT windows client for monitoring your ups hooked up to your favorite linux server.
Copyright (C) 2019-2021 Gawindx (Decaux Nicolas)

This program is free software: you can redistribute it and/or modify it under the terms of the
GNU General Public License as published by the Free Software Foundation, either version 3 of the
License, or any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY

## Donation
If you want to support this project or reward the work done, you can do so here:

[![paypal](https://www.paypalobjects.com/en_US/FR/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate?hosted_button_id=FAFJ3ZKMENGCU)
