using Harmony;
using System.Reflection;

namespace FallingWaterMod
{
    [HarmonyPatch(typeof(FallingWater))]
    [HarmonyPatch("gravityScale", PropertyMethod.Getter)]
    internal static class FallingWaterMod
    {
        private static readonly FieldInfo gravityScale = AccessTools.Field(typeof(FallingWater), "gravityScale");

        [HarmonyPostfix]
        public static void NormalGravity(FallingWater __instance, ref float __result)
        {
            Debug.Log(" === FallingWaterMod INI === ");

            __result = 1f;

            Debug.Log(" === FallingWaterMod END === ");
        }
    }

    /*
    [HarmonyPatch(typeof(FallingWater), "onSpawn", new Type[0])]
    internal class FallingWaterMod
    {
        private static void Postfix(FallingWater __instance)
        {
            Debug.Log(" === FallingWaterMod INI === "+ __instance.GetType().ToString());

            PropertyInfo gravityScale = AccessTools.Property(typeof(FallingWater), "gravityScale");
            gravityScale.SetValue(__instance, 1.0f,null);

            Debug.Log(" === FallingWaterMod END === ");
        }
    }
    */
}