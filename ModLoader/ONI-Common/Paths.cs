using System.IO;

namespace ONI_Common
{
    // TODO: refactor, split
    public static class Paths
    {
        public const string ModsDirectory = "Mods";

        #region OnionPatcher

        public static readonly string OnionMainPath = ModsDirectory + Path.DirectorySeparatorChar + "OnionPatcher";
        public static readonly string OnionConfigPath = OnionMainPath + Path.DirectorySeparatorChar + "Config";

        public const string OnionStateFileName = "OnionState.json";

        public static readonly string OnionStatePath = OnionConfigPath + Path.DirectorySeparatorChar + OnionStateFileName;

        #endregion

        public static readonly string OverlayMainPath = ModsDirectory + Path.DirectorySeparatorChar + "Overlays";
        public static readonly string OverlayConfigPath = OverlayMainPath + Path.DirectorySeparatorChar + "Config";

        public const string TemperatureStateFileName = "TemperatureOverlayState.json";

        public static readonly string TemperatureStatePath = OverlayConfigPath + Path.DirectorySeparatorChar + TemperatureStateFileName;

        public const string DraggableUIStateFileName = "DraggableUI.json";

        public static readonly string DraggableUIStatePath = OverlayConfigPath + Path.DirectorySeparatorChar + DraggableUIStateFileName;

        #region MaterialColor

        public static readonly string MaterialMainPath = ModsDirectory + Path.DirectorySeparatorChar + "MaterialColor";
        public static readonly string MaterialConfigPath = MaterialMainPath + Path.DirectorySeparatorChar + "Config";

        public const string DefaultElementColorInfosFileName = "0-default.json";
        public const string DefaultTypeColorOffsetsFileName = "0-default.json";
        public const string MaterialColorStateFileName = "MaterialColorState.json";

        public static readonly string ElementColorInfosDirectory = MaterialConfigPath + Path.DirectorySeparatorChar + "ElementColorInfos";
        public static readonly string TypeColorOffsetsDirectory = MaterialConfigPath + Path.DirectorySeparatorChar + "TypeColorOffsets";

        public static readonly string DefaultElementColorInfosPath = ElementColorInfosDirectory + Path.DirectorySeparatorChar + DefaultElementColorInfosFileName;
        public static readonly string DefaultTypeColorOffsetsPath = TypeColorOffsetsDirectory + Path.DirectorySeparatorChar + DefaultTypeColorOffsetsFileName;
        public static readonly string MaterialColorStatePath = MaterialConfigPath + Path.DirectorySeparatorChar + MaterialColorStateFileName;

        #endregion

        #region Injector

        public const string InjectorStateFileName = "InjectorState.json";

        public static readonly string InjectorStatePath = MaterialConfigPath + Path.DirectorySeparatorChar + InjectorStateFileName;

        #endregion

        #region Logs

        public static readonly string LogsPath = ModsDirectory + Path.DirectorySeparatorChar + "_Logs";

        public const string CoreLogFileName = "CoreLog.txt";

        // TODO: fix filename
        public const string MaterialColorLogFileName = "MaterialCoreLog.txt";
        public const string ConfiguratorLogFileName = "ConfiguratorLog.txt";
        public const string InjectorLogFileName = "InjectorLog.txt";
        public const string OnionLogFileName = "OnionLog.txt";
        public const string CommonLogFileName = "CommonLog.txt";

        #endregion

        #region Sprite

        public static readonly string SpritesPath = ModsDirectory + Path.DirectorySeparatorChar + "Sprites";

        public const string MaterialColorOverlayIconFileName = "overlay_materialColor.png";

        public static readonly string MaterialColorOverlayIconPath = SpritesPath + Path.DirectorySeparatorChar + MaterialColorOverlayIconFileName;

        #endregion
    }
}
