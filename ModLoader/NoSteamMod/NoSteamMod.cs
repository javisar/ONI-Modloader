using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NoSteamMod
{
	//[HarmonyPatch(typeof(KleiAccount), "OnAuthTicketObtained", new Type[] { typeof(byte[]) })]
	/*
	[HarmonyPatch(typeof(Global), "Awake")]
	internal static class NoSteamMod
	{
		private static void Prefix(Global __instance)
		{
			Debug.Log(" === NoSteamMod Prefix === ");

		}

		private static void Postfix(Global __instance)
		{
			Debug.Log(" === NoSteamMod Postfix === ");
		
			MethodInfo mi = AccessTools.Method(typeof(ThreadedHttps<KleiAccount>), "Start");
			mi.Invoke(ThreadedHttps<KleiAccount>.Instance, new object[] { });
		}
	}
	*/

	[HarmonyPatch(typeof(SteamManager), "Awake")]
	internal static class NoSteamMod2
	{
		private static bool Prefix(SteamManager __instance)
		{
			Debug.Log(" === NoSteamMod2 Prefix === ");
			FieldInfo fi = AccessTools.Field(typeof(SteamManager), "m_bInitialized");
			fi.SetValue(__instance, false);
			return false;
		}

	}

	/*
    [HarmonyPatch(typeof(Global), "Awake", new Type[0])]
    internal static class NoSteamMod
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === NoSteamMod INI === ");

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                CodeInstruction instruction = codes[i];
                if (instruction.opcode == OpCodes.Call && instruction.operand == typeof(DistributionPlatform).GetMethod("Initialize", new Type[0]))
                {
                    break;
                }
                else
                {
                    yield return instruction;
                }
            }
            Debug.Log(" === NoSteamMod END === ");
        }
    }
	*/
	/*
    [HarmonyPatch(typeof(DistributionPlatform), "Initialize", new Type[0])]
    internal class NoSteamMod
    {
        private static bool Prefix(DistributionPlatform __instance)
        {
            Debug.Log(" === NoSteamMod === ");
            return false;
        }
    }
    */
}