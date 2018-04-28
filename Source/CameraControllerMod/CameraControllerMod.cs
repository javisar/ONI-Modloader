namespace ModLoader
{
    using Harmony;

    [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
    public static class CameraControllerMod
    {
        public static void Prefix(CameraController __instance)
        {
            Debug.Log(" === CameraControllerMod INI === ");

            AccessTools.Field(typeof(CameraController), "maxOrthographicSize").SetValue(__instance, 100f);
            AccessTools.Field(typeof(CameraController), "maxOrthographicSizeDebug").SetValue(__instance, 300f);

            // Traverse.Create<CameraController>().Property("maxOrthographicSize").SetValue(100.0);
            // Traverse.Create<CameraController>().Property("maxOrthographicSizeDebug").SetValue(200.0);
            Debug.Log(" === CameraControllerMod END === ");
        }
    }
    /// <summary>
    /// Needed when a new world is generated. SetMaxOrthographicSize is run once and only then.
    /// </summary>
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.SetMaxOrthographicSize))]
    public static class CameraControllerMod2
    {
        public static void Prefix(CameraController __instance, ref float size)
        {
            size = 100f;
        }
    }
}