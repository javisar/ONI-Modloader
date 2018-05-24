using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using ONI_Common.OnionHooks;
using ProcGenGame;

namespace OnionPatches
{
	public static class OnionPatches
	{
		/*
		[HarmonyPatch(typeof(Global), "Awake")]
		public static class OnionPatchesMod
		{
			public static void Postfix()
			{
				string path = Directory.GetCurrentDirectory();
				Debug.Log("OnionPatches Path: "+path);
				ModuleDefinition onionModule = CecilHelper.GetModule("ONI-Common.dll", path+ "\\OxygenNotIncluded_Data\\Managed\\Mods");
				ModuleDefinition csharpModule = CecilHelper.GetModule("Assembly-CSharp.dll", path + "\\OxygenNotIncluded_Data\\Managed");
				ModuleDefinition firstPassModule = CecilHelper.GetModule("Assembly-CSharp-firstpass.dll", path + "\\OxygenNotIncluded_Data\\Managed");

				InjectorOnion injection = new InjectorOnion(onionModule, csharpModule, firstPassModule);
				injection.Inject();
			}
		}
		*/

		[HarmonyPatch(typeof(DebugHandler))]
		public static class DebugHandlerMod
		{
			public static void Postfix(DebugHandler __instance)
			{
				Debug.Log("DebugHandlerMod");
				//ToDo: debugHandlerEnabledProperty.SetMethod.IsPublic = true;
				Hooks.OnDebugHandlerCtor();
			}
		}

		[HarmonyPatch(typeof(WorldGen), "InitRandom", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) })]
		public static class WorldGenMod
		{
			public static void Postfix(WorldGen __instance, ref int worldSeed, ref int layoutSeed, ref int terrainSeed, ref int noiseSeed)
			{
				Debug.Log("InitRandomMod");
				Hooks.OnInitRandom(ref worldSeed, ref layoutSeed, ref terrainSeed, ref noiseSeed);
			}
		}
		
	}
}
