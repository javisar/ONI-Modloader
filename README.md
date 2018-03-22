# ONI-Modloader
A modloader for Oxygen Not Included based in Harmony

Forums in Klei:
https://forums.kleientertainment.com/topic/88186-mod01-oni-modloader/

This project uses source code from:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/pardeike/Harmony
* https://forums.kleientertainment.com/topic/81296-mod159-materialcolor-onionpatcher/


Projects
--------
* Injector: It injects the call to the modloader in 'Assembly-CSharp.dll'
* ModLoader: The modloader itself.
* Hook: Just a helper to generate IL code for the Injector.


Examples
--------
* AlternateOrdersMod: Now, the Fabricators and Refineries will alternate between infinity orders.
* InsulatedDoorsMod: Doors can be constructed using any buildable element (ie: Abyssalite). Also it adds a new element Insulated Pressure Door.
* FastModeMod: Duplicants will build an dig very fast.
* SensorsMod: It modifies some ranges y automation sensors (taken from Onion patcher).
* SpeedControlMod: Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode.
* Patches: Some incomplete tests.


Installation
------------
1. Copy 'Injector.exe' and 'Mono.Cecil.dll' to the folder: ..\OxygenNotIncluded_Data\Managed\
2. Execute 'Injector.exe'. It will create a backup of 'Assembly-CSharp.dll' in 'Assembly-CSharp.dll.orig' and a new already patched 'Assembly-CSharp.dll'
3. Create the folder: ...\OxygenNotIncluded_Data\Managed\Mods\
4. Move to this folder the following files:
   * 0Harmony.dll
   * ModLoader.dll
5. Also move to this folder all the mods you want to run.
6. Run the game and check ../OxygenNotIncluded_Data/output_log.txt for any errors.


Uninstallation
--------------
Just rename 'Assembly-CSharp.dll.orig' to 'Assembly-CSharp.dll'


Requirements
------------
* .NET Framework 3.5
* Harmony Patcher
* Mono.Cecil
* Visual Studio 2015


Creating a Mod
--------------
1. Copy the following files from ONI folder to the solution folder '\Modloader\lib\'
   * Assembly-CSharp.dll
   * Assembly-CSharp-firstpass.dll
   * Assembly-UnityScript-firstpass.dll
   * UnityEngine.dll
   * UnityEngine.UI.dll
2. Open the solution with Visual Studio.
3. Create a new mod or modify the 'Patches' project.
4. Compile it to generate the mod dll file.


Downloads
---------
See Releases section.


Harmony Tutorials
-----------------
* https://github.com/pardeike/Harmony/wiki/
* https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTTUTORIAL:-Harmony
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Introduction-to-Patching
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Detouring



Disclaimer
----------
I do not take any responsibility for broken saves or any other damage. Use this software at your own risk.

