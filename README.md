# ONI-Modloader
A modloader for Oxygen Not Included based in Harmony

This project uses source code of and is based on:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/pardeike/Harmony
* https://github.com/Moonkis/onion-patcher
* https://github.com/Moonkis/onion-hooks


Projects
--------
* Injector: It injects the call to the modloader in 'Assembly-CSharp.dll'
* ModLoader: The modloader itself.
* Patches: Mod Examples.
* Hook: Just a helper to generate IL code for the Injector.


Examples
--------
* InstantBuildMod: Duplicants will build very fast.
* InstantDigMod: Duplicants will dig very fast.
* CameraControlMod: Makes the zoom out bigger for the camera (from Onion patcher)
* FallingWaterMod: Sets gravity. (Currently it doesn't working)
* NoOxygenBreatherMod: Duplicants will no longer consume oxygen.
* PressureSensorGasMod: Max Range set to 25 (from Onion patcher)
* PressureSensorLiquidMod: Max Range set to 10000 (from Onion patcher)
* SpeedControlMod: Overwrites the method SpeedControlScreen.OnChange. Fast Speed set to behave like Ultra Speed in debug mode.
* TemperatureSensorMod: Max Temp set to 1573.15 (from Onion patcher)


Installation
------------
1. Copy 'Injector.exe' and 'Mono.Cecil.dll' to the folder: ...\OxygenNotIncluded_Data\Managed\
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
.NET Framework 3.5
Harmony Patcher
Mono.Cecil
Visual Studio 2015


Creating a Mod
--------------
1. Copy the following files from ONI folder to the solution folder '\Modloader\lib\'
   * Assembly-CSharp.dll
   * Assembly-CSharp-firstpass.dll
   * Assembly-UnityScript-firstpass.dll
   * UnityEngine.dll
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


Tips
----
* If you create a Class in your mod with the same name as a Class in ONI code, the Class Loader will load first the class in your mod. This is a way to overwrite entire classes.


Disclaimer
----------
I do not take any responsibility for broken saves or any other damage. Use this software at your own risk.

