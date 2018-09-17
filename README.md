# ONI-Modloader
A modloader for Oxygen Not Included based in Harmony

[**Example mods for ONI-Modloader**](https://github.com/javisar/ONI-Modloader-Mods)

[**Forums in Klei**](https://forums.kleientertainment.com/topic/88186-mod01-oni-modloader/)


Disclaimers
----------
* Please DON'T REPORT BUGS you encounter to Klei while mods are active.
* BE AWARE that many of the mods are still a WIP and may fail. If you are having problems use a clean ONI installation and try to test the mods one by one to narrow the error. Then post a issue in github.
* We do not take any responsibility for broken saves or any other damage. Use this software at your own risk.
* If you load a savegame, it requires that you have exactly the same mods when you saved it.

This project uses source code of and is based on:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/spaar/besiege-modloader
* https://github.com/pardeike/Harmony
* https://forums.kleientertainment.com/topic/81296-mod159-materialcolor-onionpatcher/


**NOTE**: Compiled for **RU-284634**

**Report Bugs** for ONI-Modloader here: https://github.com/javisar/ONI-Modloader/issues


Projects
--------
* **Injector**: It injects the call to the modloader in 'Assembly-CSharp.dll'.
* **ModLoader**: The modloader itself.
* **OnionHook**: Just a helper to generate IL code for the Injector.


Harmony Based Mods
------------
* https://github.com/javisar/ONI-Modloader-Mods
* https://github.com/Cairath/ONI-Mods
* https://github.com/rainbowdesign/OxygenNotIncluded-Mods
* https://github.com/Midnight-Steam/ONI-Modloader


Installation
------------
0. Prerequisites:
   * Make SURE you're using the latest version from Github main branch.
   * Make SURE you're using a fresh install of ONI, meaning you'll need the original/unpatched Assembyl-CSharp.dll and Assembly-CSharp-firstpass.dll as it comes with a clean install.
   * Make sure you deleted all previous files in:
     * \OxygenNotIncluded\Mods\
     * \OxygenNotIncluded\OxygenNotIncluded_Data\Managed\
   * You can use "Verify Integrity Files" function in Steam in the ONI game Properties>LocalFiles tab. https://inxile.zendesk.com/hc/en-us/articles/115004662908-Verify-game-cache-files-Steam-
   * **Note**: You'll need to **re-run the injector** EVERY time ONI is updated from Klei.
1. Click "Clone or Download" and "Download ZIP" for the current version as the releases may not be up to date.
2. Copy the contents of the "Managed" folder to the folder ([Example of Modloader folder structure](https://github.com/javisar/ONI-Modloader/blob/master/.github/modloader.png)):
   * \OxygenNotIncluded\OxygenNotIncluded_Data\Managed\
3. Execute 'Injector.exe'. It will create a backups of 'Assembly-CSharp.dll' and Assembly-CSharp-firstpass.dll with the extension ".orig" and a new patched set of dlls.
4. You should now find a folder called "Mods" in your ONI main directory. Otherwise, create "Mods" folder in the ONI main directory.
   * \OxygenNotIncluded\Mods\
5. Check for error logs in:
   * \OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Mod_Log.txt   


Uninstallation
--------------
Just rename 'Assembly-CSharp.dll.orig' to 'Assembly-CSharp.dll'and 'Assembly-CSharp-firstpass.dll.orig' to 'Assembly-CSharp-firstpass.dll'


[Mods Installation](https://github.com/javisar/ONI-Modloader-Mods/blob/master/README.md#mods-installation)
-----------------


Requirements
------------
* .NET Framework v3.5
* Harmony Patcher v1.1.0
* Mono.Cecil
* Visual Studio 2015


Creating a Mod
--------------
0. Feel free to mess with any of the mods from https://github.com/javisar/ONI-Modloader-Mods
1. 'Clone or download' the project from the mod repo.
2. Copy the following files from ONI Managed folder '\OxygenNotIncluded\OxygenNotIncluded_Data\Managed' to the mod solution folder '\Source\lib\'
   * Assembly-CSharp.dll
   * Assembly-CSharp-firstpass.dll
   * Assembly-UnityScript-firstpass.dll
   * UnityEngine.dll
   * UnityEngine.CoreModule.dll
   * UnityEngine.UI.dll
   * Any needed unity UnityEngine.*.dll   
3. Open the solution with Visual Studio.
4. Create a new class project.
5. Add the previous libs to the references of the project.
6. Compile it to generate the mod dll file.
7. Check the tutorials at the end of the page.

Note: Dlls will be recognized by the mod loader if 
• they reside in the main mod directory 
OR
• they are inside a subfolder inside a subfolder names 'Assemblies'


Downloads
---------
Choose 'Clone or Download'.
See Releases section.


Harmony Tutorials
-----------------
* https://github.com/pardeike/Harmony/wiki/
* https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTTUTORIAL:-Harmony
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Introduction-to-Patching
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Detouring
* https://oxygennotincluded.gamepedia.com/Guide/Working_with_the_Game_Files
