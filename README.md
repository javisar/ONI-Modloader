# ONI-Modloader
A modloader for Oxygen Not Included based in Harmony

Forums in Klei:
https://forums.kleientertainment.com/topic/88186-mod01-oni-modloader/

This project uses source code of and is based on:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/spaar/besiege-modloader
* https://github.com/pardeike/Harmony
* https://forums.kleientertainment.com/topic/81296-mod159-materialcolor-onionpatcher/


NOTE: Please don't report bugs you encounter while mods are active. People at Klei work hard and shouldn't be bothered with bug reports which might originate from mods.

NOTE: BE AWARE that many of the mods are still a WIP and may fail. Many saves can be broken if you mess around with the mods.

Projects
--------
* Injector: It injects the call to the modloader in 'Assembly-CSharp.dll'
* ModLoader: The modloader itself.
* OnionHook: Just a helper to generate IL code for the Injector.


Examples
--------
* AlternateOrdersMod (WIP): The Fabricators and Refineries will alternate between infinity orders.
* CameraControllerMod: Enable further zoom-outs in play and dev mode (taken from Onion patcher).
* CritterNumberSensor: Sensor for the critter number in a room (thanks to R9MX4 from Klei forum)
* CustomWorldMod (WIP): Enables the player to user custom world sizes. Stand alone mod currently diabled, please edit the OnionConfig.json file for changes (taken from Onion patcher)
* DraggablePanelMod (WIP): Makes panels draggable. REQUIRES ONI-Common.
* FastModeMod: Duplicants will build an dig very fast.
* ImprovedGasColourMod: Replaces the oxygen overly with gas colors. Also visualizes the density (taken from Onion patcher, modified). REQUIRES ONI-Common.
* InstantResearchMod (WIP): Forces instant research without Debug mode.
* InsulatedDoorsMod (WIP): Doors can be constructed using any buildable element (ie: Abyssalite). Also it adds a new element Insulated Pressure Door
* InverseElectrolyzerMod: Combines hydrogen and oxygen into steam.
* LiquidTankMod: Storage for liquids.
* MaterialColor: Adds an overlay option to visualize what a building is made of (taken from Onion patcher). REQUIRES ONI-Common.
* NoSteamMod: Prevents game closing if Steam is not installed.
* ONI-Common: Common code for Onion Patches and other mods.
* OnionPatches: Custom world seeds. DebugHandler hook. REQUIRES ONI-Common.
* Patches (WIP): Some incomplete tests.
* PressureDoorMod: Removes the energy need for the mechanized pressure door and makes it buildable from all material.
* SensorsMod: It modifies some ranges y automation sensors (taken from Onion patcher).
* SpeedControlMod: Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode.
* StorageLockerMod: Storage lockers won't need a foundation to be built.


Change Log
----------
* 0.3.3
  * The Mods are no longer included in the Release file
  * Cleanup
  * Added CritterNumberSensor
  * Fixed issues
    * https://github.com/javisar/ONI-Modloader/issues/6
	* https://github.com/javisar/ONI-Modloader/issues/7
* 0.3.2
  * Refactoring. Make Injector independent from Onion patches.
  * Added NoSteamMod
  * Deleted not working mods from the release.
* 0.3.1
  * Added Onion Mods and other mods from Killface1980
  * Added CameraControllerMod
  * Added CustomWorldMod
  * Added DraggablePanelMod
  * Added ImprovedGasColourMod
  * Added MaterialColor Mod
  * Added ONI-Common
  * Added PressureDoorMod
  * Added SensorsMod
  * Added StorageLockerMod
* 0.3
  * First stable version.


Installation
------------
NOTE: Make sure you're using a fresh install of ONI, meaning you'll need the original/unpatched Assembyl-CSharp.dll and Assembly-CSharp-firstpass.dll as it comes with a clean install.
NOTE: You'll need to re-run the injector every time ONI gets updated.

1. Download latest version in Release section.
2. Unzip in the ONI main directory.
3. Execute 'Injector.exe'. It will create a backups of 'Assembly-CSharp.dll' and Assembly-CSharp-firstpass.dll with the extension ".orig"  and a new patched set of dlls.
4. Download from https://github.com/javisar/ONI-Modloader/tree/master/Dist and move to the Mods folder the desired mods. I recommend to move all config folders.


Example of a Final file/folder structure:
* OxygenNotIncluded
  * OxygenNotIncluded_Data
    * Managed
      * 0Harmony.dll
	  * Injector.exe
	  * ModLoader.dll
	  * Mono.Cecil.dll
	  * ...
  * Mods
    * MaterialColor (and children)
	* OnionPatcher (and children)
	* Overlays (and children)
	* Sprites (and children)
	* ImprovedGasColourMod.dll (requieres ONI-Common.dll)
	* MaterialColor.dll (requieres ONI-Common.dll)
	* ONI-Common.dll


Creating a Mod
--------------
1. Click "Clone or Download" for the current version as the releases are currently not up to date.
2. Copy the following files from a Previously Patched ONI folder to the solution folder '\Modloader\lib\'
   * Assembly-CSharp.dll
   * Assembly-CSharp-firstpass.dll
   * Assembly-UnityScript-firstpass.dll
   * UnityEngine.dll
   * UnityEngine.UI.dll
3. Open the solution with Visual Studio.
4. Create a new project. Use the examples as template.
5. Compile it to generate the mod dll file. NOTE: If you compile the Solution with Visual Studio the files will also be copied directly to the ONI folder.
6. Execute 'Injector.exe' (only if you have the unpatched dlls )
7. You should now find a folder called "Mods" in your ONI main directory. Otherwise, copy the extracted "Mods" folder.
8. Move or copy all the mods you'd like to use to the "Mods" folder in the ONI main directory
9. Run the game and check "../OxygenNotIncluded_Data/output_log.txt" and "../OxygenNotIncluded/Mods/_Logs/..." for any errors.


NOTE: Dlls will be recognized by the mod loader if 
• they reside in the main mod directory 
OR
• they are inside a subfolder inside a subfolder names 'Assemblies'.


Uninstallation
--------------
Just rename 'Assembly-CSharp.dll.orig' to 'Assembly-CSharp.dll' and 'Assembly-CSharp-firstpass.dll.orig' to 'Assembly-CSharp-firstpass.dll'


Requirements
------------
* .NET Framework 3.5
* Harmony Patcher
* Mono.Cecil
* Visual Studio 2015


Downloads
---------
* Modloader: Choose 'Clone or download' from the Release section. Or download from https://github.com/javisar/ONI-Modloader/tree/master/Dist/OxygenNotIncluded_Data/Managed
* Mods: Download from https://github.com/javisar/ONI-Modloader/tree/master/Dist/Mods



Harmony/ONI Tutorials
-----------------
* https://github.com/pardeike/Harmony/wiki/
* https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTTUTORIAL:-Harmony
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Introduction-to-Patching
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Detouring
* https://oxygennotincluded.gamepedia.com/Guide/Working_with_the_Game_Files



Disclaimer
----------
We do not take any responsibility for broken saves or any other damage. Use this software at your own risk.
