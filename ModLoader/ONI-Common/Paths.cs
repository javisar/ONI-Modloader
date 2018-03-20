namespace ONI_Common
{
    using System.IO;

    // TODO: refactor, split
    public static class Paths
    {
        public const string CommonLogFileName = "CommonLog.txt";

        public const string ConfiguratorLogFileName = "ConfiguratorLog.txt";

        public const string CoreLogFileName = "CoreLog.txt";

        public const string DefaultElementColorInfosFileName = "0-default.json";

        public const string DefaultTypeColorOffsetsFileName = "0-default.json";

        public const string DraggableUIStateFileName = "DraggableUI.json";

        public const string InjectorLogFileName = "InjectorLog.txt";

        public const string InjectorStateFileName = "InjectorState.json";

        // TODO: fix filename
        public const string MaterialColorLogFileName = "MaterialCoreLog.txt";

        public const string MaterialColorOverlayIconFileName = "overlay_materialColor.png";

        public const string MaterialColorStateFileName = "MaterialColorState.json";

        public const string ModsDirectory = "Mods";

        public const string OnionLogFileName = "OnionLog.txt";

        public const string OnionStateFileName = "OnionState.json";

        public const string TemperatureStateFileName = "TemperatureOverlayState.json";

        public static readonly string OnionMainPath = ModsDirectory + Path.DirectorySeparatorChar + "OnionPatcher";

        public static readonly string OnionConfigPath = OnionMainPath + Path.DirectorySeparatorChar + "Config";

        public static readonly string OnionStatePath =
        OnionConfigPath + Path.DirectorySeparatorChar + OnionStateFileName;

        public static readonly string OverlayMainPath = ModsDirectory + Path.DirectorySeparatorChar + "Overlays";

        public static readonly string OverlayConfigPath = OverlayMainPath + Path.DirectorySeparatorChar + "Config";

        public static readonly string TemperatureStatePath =
        OverlayConfigPath + Path.DirectorySeparatorChar + TemperatureStateFileName;

        public static readonly string DraggableUIStatePath =
        OverlayConfigPath + Path.DirectorySeparatorChar + DraggableUIStateFileName;

        public static readonly string MaterialMainPath = ModsDirectory + Path.DirectorySeparatorChar + "MaterialColor";

        public static readonly string MaterialConfigPath = MaterialMainPath + Path.DirectorySeparatorChar + "Config";

        public static readonly string ElementColorInfosDirectory =
        MaterialConfigPath + Path.DirectorySeparatorChar + "ElementColorInfos";

        public static readonly string TypeColorOffsetsDirectory =
        MaterialConfigPath + Path.DirectorySeparatorChar + "TypeColorOffsets";

        public static readonly string DefaultElementColorInfosPath =
        ElementColorInfosDirectory + Path.DirectorySeparatorChar + DefaultElementColorInfosFileName;

        public static readonly string DefaultTypeColorOffsetsPath =
        TypeColorOffsetsDirectory + Path.DirectorySeparatorChar + DefaultTypeColorOffsetsFileName;

        public static readonly string MaterialColorStatePath =
        MaterialConfigPath + Path.DirectorySeparatorChar + MaterialColorStateFileName;

        public static readonly string InjectorStatePath =
        MaterialConfigPath + Path.DirectorySeparatorChar + InjectorStateFileName;

        public static readonly string LogsPath = ModsDirectory + Path.DirectorySeparatorChar + "_Logs";

        public static readonly string SpritesPath = ModsDirectory + Path.DirectorySeparatorChar + "Sprites";

        public static readonly string MaterialColorOverlayIconPath =
        SpritesPath + Path.DirectorySeparatorChar + MaterialColorOverlayIconFileName;
    }
}