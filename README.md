# ONI-Modloader
A modloader for Oxygen Not Included based in Harmony

This project uses source code and is based on:
* https://github.com/zeobviouslyfakeacc/ModLoaderInstaller
* https://github.com/pardeike/Harmony


Projects
--------
* Injector: It injects the call to the modloader in Assembly-CSharp.dll
* ModLoader: The modloader itself.
* Patches: An example mod.
* Hook: Just a helper to generate IL code for the Injector.

Instructions
------------
1. Copy 'Injector.exe' and 'Mono.Cecil.dll' to the folder: ...\OxygenNotIncluded_Data\Managed\
2. Execute 'Injector.exe'. It will create a backup of Assembly-CSharp.dll in Assembly-CSharp.dll.orig
3. Create the folder: ...\OxygenNotIncluded_Data\Managed\Mods\
4. Move to this folder the following files
   a) 0Harmony.dll
   b) ModLoader.dll
   c) Patches.dll (or any mod you'd like to load)
5. Run the game and check ../OxygenNotIncluded_Data/output_log.txt for any errors.

Tutorials
---------
* https://github.com/pardeike/Harmony/wiki/
* https://github.com/roxxploxx/RimWorldModGuide/wiki/SHORTTUTORIAL:-Harmony
* https://github.com/UnlimitedHugs/RimworldHugsLib/wiki/Introduction-to-Patching

