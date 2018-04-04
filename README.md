# ONI-Modloader
A modloader for Oxygen Not Included based in Harmony

Forums in Klei:
https://forums.kleientertainment.com/topic/88186-mod01-oni-modloader/

This project uses source code of and is based on:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/spaar/besiege-modloader
* https://github.com/pardeike/Harmony
* https://forums.kleientertainment.com/topic/81296-mod159-materialcolor-onionpatcher/


Projects
--------
* Injector: It injects the call to the modloader in 'Assembly-CSharp.dll', also makes some classes public in 'Assembly-CSharp-firstpass.dll'
* ModLoader: The modloader itself.
* OnionHook: Just a helper to generate IL code for the Injector.


Examples
--------
Midnight-Steam's Mods:
* DefaultDeconstructBuildings: Sets the default settings to "Buildings" when Deconstruct tool is selected
* ElectrolyzerIgnorePressure: Electrolyzer's ignore max pressure
* MainMenuResumeGame: Let's you know if the last save was a Manual or Auto Save
* MaxAttributeMultiplier: Changes the attribute multiplier so Duplicants are faster than The Flash(attributes not changed: ImmuneLevelBoost, TemperatureInsulation, SeedHarvestChance)
* SculptureFlippable: Makes the sculpture flippable so it can stare at Duplicants from both sides
* WaterPurifierOutputTemp: Output water and sand are no longer set to 40 degrees Celsius instead the same as input temperature.

* IgnoreMaxPressure: Electrolyzer's ignore max pressure
* MainMenuResumeGame: Let's the player know if last save was a Manual or Auto Save
* MaxAttributes: Changes the attribute multiplier so Duplicants are faster than The Flash
* SculptureFlippable: Makes the sculpture flippable so it can stare at Duplicants from both sides
* WaterPurifier: Output water and sand are no longer set to 40 degrees Celsius but the same as input temperature.

Original Mods:
* AlternateOrdersMod: The Fabricators and Refineries will alternate between infinity orders.
* CameraControllerMod: Enable further zoom-outs in play and dev mode (taken from Onion patcher).
* CustomWorldMod: Enables the player to user custom world sizes. Stand alone mod currently diabled, please edit the OnionConfig.json file for changes (taken from Onion patcher)
* FastModeMod: Duplicants will build an dig very fast.
* ImprovedGasColourMod: Replaces the oxygen overly with gas colors. Also visualizes the density (taken from Onion patcher, modified).
* InsulatedDoorsMod (Not working): Doors can be constructed using any buildable element (ie: Abyssalite). Also it adds a new element Insulated Pressure Door
* MaterialColor: Adds an overlay option to visualize what a building is made of (taken from Onion patcher).
* PressureDoorMod: Removes the energy need for the mechanized pressure door and makes it buildable from all material.
* SensorsMod: It modifies some ranges y automation sensors (taken from Onion patcher).
* StorageLockerMod: Storage lockers won't need a foundation to be built.
* SpeedControlMod: Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode.
* Patches (Do not use): Some incomplete tests 


Installation
------------
Make sure you're using a fresh install of ONI, meaning you'll need the original/unpatched Assembyl-CSharp.dll and Assembly-CSharp-firstpass.dll as it comes with a clean install.
Note: You'll need to re-run the injector every time ONI gets updated.

Click "Clone or Download" for the current version as the releases are currently not up to date.

1. Copy the contents of the "Managed" folder to: ...\OxygenNotIncluded_Data\Managed\
2. Execute 'Injector.exe'. It will create a backups of 'Assembly-CSharp.dll' and Assembly-CSharp-firstpass.dll with the extension ".orig"  and a new patched set of dlls.
3. You should now find a folder called "Mods" in your ONI main directory. Otherwise, copy the extracted "Mods" folder.
4. Move or copy all the mods you'd like to use from the extracted "Mods" folder to the newly created "Mods" in the ONI main directory
5. Run the game and check ../OxygenNotIncluded_Data/output_log.txt for any errors.

Please don't report bugs you encounter while mods are active. People at Klei work hard and shouldn't be bothered with bug reports which might originate from mods.


Alternative Installation
----------------------
1. Download last version in Release section.
2. Unzip in the ONI main directory.
3. Execute 'Injector.exe' (only if you have the unpatched dlls )
4. Remove all unwanted mods from "Mods" folder. 

Uninstallation
--------------
Just rename 'Assembly-CSharp.dll.orig' to 'Assembly-CSharp.dll' and 'Assembly-CSharp-firstpass.dll.orig' to 'Assembly-CSharp-firstpass.dll'


Requirements
------------
* .NET Framework 3.5
* Harmony Patcher
* Mono.Cecil
* Visual Studio 2015


Creating a Mod
--------------
1. Copy the following files from a Previously Patched ONI folder to the solution folder '\Modloader\lib\'
   * Assembly-CSharp.dll
   * Assembly-CSharp-firstpass.dll
   * Assembly-UnityScript-firstpass.dll
   * UnityEngine.dll
   * UnityEngine.UI.dll
2. Open the solution with Visual Studio.
3. Create a new mod or modify the 'Patches' project.
4. Compile it to generate the mod dll file.

Dlls will be recognized by the mod loader if 
• they reside in the main mod direcotory 
OR
• they are inside a subfolder inside a subfolder names 'Assemblies' (see MaterialColor mod)


Downloads
---------
Choose 'Clone or download'.
See Releases section.


Harmony Tutorials
-----------------
* https://github.com/pardeike/Harmony/wiki/
* https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTTUTORIAL:-Harmony
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Introduction-to-Patching
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Detouring



Disclaimer
----------
We do not take any responsibility for broken saves or any other damage. Use this software at your own risk.
