using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace UncompressedSaveMod
{
    class Class1
    {
        [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Save))]
        public static class UncompressedSaveMod
        {

            public static void Prefix(SaveLoader __instance)
            {
                AccessTools.Field(typeof(SaveLoader), "compressSaveData").SetValue(__instance, false);
            }

            public static void Postfix(SaveLoader __instance)
            {
                AccessTools.Field(typeof(SaveLoader), "compressSaveData").SetValue(__instance, true);
            }
        }

    }
}
