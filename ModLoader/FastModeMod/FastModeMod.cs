using Harmony;
using System;


namespace FastModeMod
{

    [HarmonyPatch(typeof(Constructable), "GetEfficiencyMultiplier", new Type[] { typeof(Worker) })]
    internal static class InstantDigAndBuildMod
    {

        private static void Postfix(Constructable __instance, Worker worker, ref float __result)
        {
            //Debug.Log(" === GetEfficiencyMultiplier InstantDigAndBuildMod === " + __instance.GetType().ToString());
            if (__instance.GetType().Equals(typeof(Constructable))
                || __instance.GetType().Equals(typeof(Diggable)))
            {
                __result = 100.0f;
            }
        }
    }

    /*
    [HarmonyPatch(typeof(Database.AttributeConverters), "Create", new Type[] { typeof(string), typeof(string), typeof(string), typeof(Klei.AI.Attribute), typeof(float), typeof(float), typeof(IAttributeFormatter) })]
    internal class InstantDigAndBuildMod
    {
        private static bool Prefix(Database.AttributeConverters __instance, string id, string name, string description, Klei.AI.Attribute attribute, ref float multiplier, float base_value, IAttributeFormatter formatter)
        {
            //Debug.Log(" === GetEfficiencyMultiplier InstantDigAndBuildMod loaded === " + id + " "+multiplier);
            if (id.Equals("ConstructionSpeed") || id.Equals("DiggingSpeed"))
            {
                multiplier = 100.0f;
            }
            return true;
        }
    }
    */

}
