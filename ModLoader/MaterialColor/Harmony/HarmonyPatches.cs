using Common;
using Core;
using Core.IO;
using Harmony;
using MaterialColor.Extensions;
using MaterialColor.Helpers;
using Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Common.Json;
using MaterialColor.TemperatureOverlay;
using ONI_Common;
using ONI_Common.Core;
using ONI_Common.Data;
using ONI_Common.IO;
using ProcGen;
using UnityEngine;

namespace MaterialColor
{
    internal static class HarmonyPatches
    {
        #region Private Fields

        private static readonly Color NotGasColor = new Color(0.6f, 0.6f, 0.6f);

        private static bool _configuratorStateChanged;

        // private static void OnBuildingsCompletesAdd(BuildingComplete building)
        //     => ColorHelper.UpdateBuildingColor(building);
        private static bool _elementColorInfosChanged;

        private static bool _initialized;
        private static bool _typeColorOffsetsChanged;

        #endregion Private Fields

        #region Public Methods

        public static void OnLoad()
        {
            //  HarmonyInstance harmony = HarmonyInstance.Create("com.oni.materialcolor");
            //   harmony.PatchAll(Assembly.GetExecutingAssembly());

            //  try
            //  {
            //      Components.BuildingCompletes.OnAdd += OnBuildingsCompletesAdd;
            //
            //  }
            //  catch (Exception e)
            //  {
            //      var message = "Injection failed\n" + e.Message + '\n';
            //
            //      State.Logger.Log(message);
            //      State.Logger.Log(e);
            //
            //      Debug.LogError(message);
            // }
            //try
            //{
            //    SaveTemperatureThresholdsAsDefault();
            //
            //    if (State.TemperatureOverlayState.LogThresholds)
            //    {
            //        LogTemperatureThresholds();
            //    }
            //
            //    UpdateTemperatureThresholds();
            //}
            //catch (Exception e)
            //{
            //    State.Logger.Log("Custom temperature overlay init error");
            //    State.Logger.Log(e);
            //}
        }

        public static void RefreshMaterialColor()
        {
            UpdateBuildingsColors();
            RebuildAllTiles();
        }

        public static void UpdateBuildingColor(BuildingComplete building)
        {
            string buildingName = building.name.Replace("Complete", string.Empty);
            SimHashes material = MaterialHelper.ExtractMaterial(building);

            Color32 color;

            if (State.ConfiguratorState.Enabled)
            {
                switch (State.ConfiguratorState.ColorMode)
                {
                    case ColorMode.Json:
                        color = material.GetMaterialColorForType(buildingName);
                        break;

                    case ColorMode.DebugColor:
                        color = material.ToDebugColor();
                        break;

                    default:
                        color = ColorHelper.DefaultColor;
                        break;
                }
            }
            else
            {
                color = ColorHelper.DefaultColor;
            }

            if (State.TileNames.Contains(buildingName))
            {
                try
                {
                    if (ColorHelper.TileColors == null)
                    {
                        ColorHelper.TileColors = new Color?[Grid.CellCount];
                    }

                    ColorHelper.TileColors[Grid.PosToCell(building.gameObject)] = color;

                    return;
                }
                catch (Exception e)
                {
                    State.Logger.Log("Error while aquiring cell color");
                    State.Logger.Log(e);
                }
            }

            Color32 dimmedColor = color.SetBrightness(color.GetBrightness() / 2);

            // storagelocker
            StorageLocker storageLocker = building.GetComponent<StorageLocker>();

            if (storageLocker != null)
            {
                SetFilteredStorageColors(storageLocker.filteredStorage, color, dimmedColor);
            }
            else // ownable
            {
                Ownable ownable = building.GetComponent<Ownable>();

                if (ownable != null)
                {
                    ownable.ownedTint = color;
                    ownable.unownedTint = dimmedColor;
                    ownable.UpdateTint();
                }
                else // rationbox
                {
                    RationBox rationBox = building.GetComponent<RationBox>();

                    if (rationBox != null)
                    {
                        SetFilteredStorageColors(rationBox.filteredStorage, color, dimmedColor);
                    }
                    else // refrigerator
                    {
                        Refrigerator fridge = building.GetComponent<Refrigerator>();

                        if (fridge != null)
                        {
                            SetFilteredStorageColors(fridge.filteredStorage, color, dimmedColor);
                        }
                        else // anything else
                        {
                            KAnimControllerBase kAnimControllerBase = building.GetComponent<KAnimControllerBase>();

                            if (kAnimControllerBase != null)
                            {
                                kAnimControllerBase.TintColour = color;
                            }
                            else
                            {
                                UnityEngine.Debug.LogError($"Can't find KAnimControllerBase component in <{buildingName}> and its not a registered tile.");
                            }
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static void Initialize()
        {
            SubscribeToFileChangeNotifier();
            _initialized = true;
        }

        private static void LogTemperatureThresholds()
        {
            System.Diagnostics.Debug.Assert(SimDebugView.Instance.temperatureThresholds != null,
                                            "SimDebugView.Instance.temperatureThresholds != null");
            for (int i = 0; i < SimDebugView.Instance.temperatureThresholds.Length; i++)
            {
                State.Logger.Log(SimDebugView.Instance.temperatureThresholds[i].value.ToString());
                State.Logger.Log(SimDebugView.Instance.temperatureThresholds[i].color.ToString());
            }
        }

        private static void OnBuildingsCompletesAdd(BuildingComplete building)
            => UpdateBuildingColor(building);

        private static void OnElementColorsInfosChanged(object sender, FileSystemEventArgs e)
        {
            bool reloadColorInfosResult = false;

            try
            {
                reloadColorInfosResult = State.TryReloadElementColorInfos();
            }
            catch (Exception ex)
            {
                State.Logger.Log("ReloadElementColorInfos failed.");
                State.Logger.Log(ex);
            }

            if (reloadColorInfosResult)
            {
                _elementColorInfosChanged = true;

                const string message = "Element color infos changed.";

                State.Logger.Log(message);
                UnityEngine.Debug.LogError(message);
            }
            else
            {
                State.Logger.Log("Reload element color infos failed");
            }
        }

        private static void OnMaterialStateChanged(object sender, FileSystemEventArgs e)
        {
            if (!State.TryReloadConfiguratorState())
            {
                return;
            }

            _configuratorStateChanged = true;

            const string message = "Configurator state changed.";

            State.Logger.Log(message);
            UnityEngine.Debug.LogError(message);
        }

        // TODO: log failed reload on other eventhandlers
        private static void OnTemperatureStateChanged(object sender, FileSystemEventArgs e)
        {
            string message;

            if (State.TryReloadTemperatureState())
            {
                UpdateTemperatureThresholds();
                message = "Temperature overlay state changed.";
            }
            else
            {
                message = "Temperature overlay state load failed.";
            }

            State.Logger.Log(message);
            UnityEngine.Debug.LogError(message);
        }

        private static void OnTypeColorOffsetsChanged(object sender, FileSystemEventArgs e)
        {
            if (!State.TryReloadTypeColorOffsets())
            {
                return;
            }

            _typeColorOffsetsChanged = true;

            const string message = "Type colors changed.";

            State.Logger.Log(message);
            UnityEngine.Debug.LogError(message);
        }

        private static void RebuildAllTiles()
        {
            for (int i = 0; i < Grid.CellCount; i++)
            {
                World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, i);
            }
            State.Logger.Log("All tiles rebuilt.");
        }

        private static void SaveTemperatureThresholdsAsDefault()
        {
            if (SimDebugView.Instance?.temperatureThresholds != null)
            {
                foreach (SimDebugView.ColorThreshold threshold in SimDebugView.Instance?.temperatureThresholds)
                {
                    State.DefaultTemperatureColors.Add(threshold.color);
                    State.DefaultTemperatures.Add(threshold.value);
                }
            }
        }

        private static void SetFilteredStorageColors(FilteredStorage storage, Color32 color, Color32 dimmedColor)
        {
            storage.filterTint = color;
            storage.noFilterTint = dimmedColor;
            storage.FilterChanged();
        }

        private static void SubscribeToFileChangeNotifier()
        {
            const string jsonFilter = "*.json";

            try
            {
                FileChangeNotifier.StartFileWatch(jsonFilter, Paths.ElementColorInfosDirectory, OnElementColorsInfosChanged);
                FileChangeNotifier.StartFileWatch(jsonFilter, Paths.TypeColorOffsetsDirectory, OnTypeColorOffsetsChanged);

                FileChangeNotifier.StartFileWatch(Paths.MaterialColorStateFileName, Paths.MaterialConfigPath, OnMaterialStateChanged);

                if (State.TemperatureOverlayState.CustomRangesEnabled)
                {
                    FileChangeNotifier.StartFileWatch(Paths.TemperatureStateFileName, Paths.OverlayConfigPath, OnTemperatureStateChanged);
                }
            }
            catch (Exception e)
            {
                State.Logger.Log("SubscribeToFileChangeNotifier failed");
                State.Logger.Log(e);
            }
        }

        private static void UpdateBuildingsColors()
        {
            State.Logger.Log($"Trying to update {Components.BuildingCompletes.Count} buildings.");

            try
            {
                foreach (BuildingComplete building in Components.BuildingCompletes)
                {
                    OnBuildingsCompletesAdd(building);
                }
                State.Logger.Log("Buildings updated successfully.");
            }
            catch (Exception e)
            {
                State.Logger.Log("Buildings colors update failed.");
                State.Logger.Log(e);
            }
        }

        private static void UpdateTemperatureThresholds()
        {
            if (SimDebugView.Instance == null) { return; }
            List<float> newTemperatures = (State.TemperatureOverlayState.CustomRangesEnabled
                                           ? State.TemperatureOverlayState.Temperatures
                                           : State.DefaultTemperatures);

            for (int i = 0; i < newTemperatures.Count; i++)
            {
                {
                    if (SimDebugView.Instance.temperatureThresholds != null)
                    {
                        SimDebugView.Instance.temperatureThresholds[i] =
                        new SimDebugView.ColorThreshold
                        {
                            color = State.DefaultTemperatureColors[i],
                            value = newTemperatures[i]
                        };
                    }
                }
            }

            Array.Sort(SimDebugView.Instance.temperatureThresholds, new ColorThresholdTemperatureSorter());
        }

        #endregion Private Methods

        #region Public Classes

        [HarmonyPatch(typeof(BlockTileRenderer), nameof(BlockTileRenderer.GetCellColour))]
        public static class BlockTileRenderer_GetCellColour
        {
            #region Public Methods

            public static bool Prefix(int cell, SimHashes element, BlockTileRenderer __instance, ref Color __result)
            {
                try
                {
                    Color tileColor;

                    if (State.ConfiguratorState.Enabled)
                    {
                        if (State.ConfiguratorState.LegacyTileColorHandling)
                        {
                            switch (State.ConfiguratorState.ColorMode)
                            {
                                case ColorMode.Json:
                                    tileColor = ColorHelper.GetCellColorJson(cell);
                                    break;

                                case ColorMode.DebugColor:
                                    tileColor = ColorHelper.GetCellColorDebug(cell);
                                    break;

                                default:
                                    tileColor = ColorHelper.DefaultCellColor;
                                    break;
                            }
                        }
                        else
                        {
                            if (ColorHelper.TileColors.Length > cell && ColorHelper.TileColors[cell].HasValue)
                            {
                                tileColor = ColorHelper.TileColors[cell].Value;
                            }
                            else
                            {
                                if (cell == __instance.invalidPlaceCell)
                                {
                                    __result = ColorHelper.InvalidCellColor;
                                    return false;
                                }
                                tileColor = ColorHelper.DefaultCellColor;
                            }
                        }
                    }
                    else
                    {
                        tileColor = ColorHelper.DefaultCellColor;
                    }

                    if (cell == __instance.selectedCell)
                    {
                        __result = tileColor * 1.5f;
                        return false;
                    }

                    if (cell == __instance.highlightCell)
                    {
                        __result = tileColor * 1.25f;
                        return false;
                    }

                    __result = tileColor;
                    return false;
                }
                catch (Exception e)
                {
                    State.Logger.Log("EnterCell failed.");
                    State.Logger.Log(e);
                }
                return true;
            }

            #endregion Public Methods
        }

        [HarmonyPatch(typeof(Deconstructable), "OnCompleteWork")]
        public static class Deconstructable_OnCompleteWork_MatCol
        {
            #region Public Methods

            public static void Postfix(Deconstructable __instance)
            {
                ResetCell(__instance.GetCell());
            }

            public static void ResetCell(int cellIndex)
            {
                if (ColorHelper.TileColors.Length > cellIndex)
                {
                    ColorHelper.TileColors[cellIndex] = null;
                }
            }

            #endregion Public Methods
        }

        [HarmonyPatch(typeof(Game), "Update")]
        public static class Game_Update_EnterEveryUpdate
        {
            #region Public Methods

            public static void Prefix()
            {
                try
                {
                    if (_elementColorInfosChanged || _typeColorOffsetsChanged || _configuratorStateChanged)
                    {
                        RefreshMaterialColor();
                        _elementColorInfosChanged = _typeColorOffsetsChanged = _configuratorStateChanged = false;
                    }
                }
                catch (Exception e)
                {
                    State.Logger.Log("EnterEveryUpdate failed.");
                    State.Logger.Log(e);
                }
            }

            #endregion Public Methods
        }

        [HarmonyPatch(typeof(Game), "Update")]
        public static class Game_Update_EnterEveryUpdate_CoreUpdateQueueManager
        {
            #region Public Methods

            public static void Prefix()
            {
                UpdateQueueManager.OnGameUpdate();
            }

            #endregion Public Methods
        }

        [HarmonyPatch(typeof(Global), "GenerateDefaultBindings")]
        public static class Global_GenerateDefaultBindings
        {
            #region Public Methods

            public static void Postfix(ref BindingEntry[] __result)
            {
                try
                {
                    List<BindingEntry> bind = __result.ToList();
                    BindingEntry entry = new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F6, Modifier.Alt,
                                                 (Action)IDs.ToggleMaterialColorOverlayAction, true, true);
                    bind.Add(entry);
                    __result = bind.ToArray();
                }
                catch (Exception e)
                {
                    State.Logger.Log("Keybindings failed:\n" + e);
                    throw;
                }
            }

            #endregion Public Methods
        }

        // Todo: add options ingamem, make optional
        [HarmonyPatch(typeof(LogicWireBridgeConfig), nameof(LogicWireBridgeConfig.CreateBuildingDef))]
        public static class LogicWireBridgeConfig_CreateBuildingDef
        {
            #region Public Methods

            public static void Postfix(LogicWireBridgeConfig __instance, BuildingDef __result)
            {
                __result.ObjectLayer = ObjectLayer.Backwall;
            }

            #endregion Public Methods
        }

        [HarmonyPatch(typeof(OverlayMenu), "InitializeToggles")]
        public static class OverlayMenu_InitializeToggles
        {
            #region Public Methods

            // TODO: read from file instead
            public static void Postfix(OverlayMenu __instance, ref List<KIconToggleMenu.ToggleInfo> __result)
            {
                __result.Add(
                             new OverlayMenu.OverlayToggleInfo(
                                                               "Toggle MaterialColor",
                                                               "overlay_materialcolor",
                                                               (SimViewMode)IDs.ToggleMaterialColorOverlayID,
                                                               string.Empty,
                                                               (Action)IDs.ToggleMaterialColorOverlayAction,
                                                               "Toggles MaterialColor overlay",
                                                               "MaterialColor"){getSpriteCB = () => GetUISprite() });
            }

            private static Sprite GetUISprite()
            {
                return FileManager.LoadSpriteFromFile(Paths.MaterialColorOverlayIconPath, 256, 256);
            }

            #endregion Public Methods
        }

        [HarmonyPatch(typeof(OverlayMenu), "OnOverlayChanged")]
        public static class OverlayMenu_OnOverlayChanged_OverlayChangedEntry
        {
            #region Public Methods

            public static void Prefix()
            {
                try
                {
                    switch (OverlayScreen.Instance.GetMode())
                    {
                        case SimViewMode.PowerMap:
                        case SimViewMode.GasVentMap:
                        case SimViewMode.LiquidVentMap:
                        case SimViewMode.Logic:
                            RefreshMaterialColor();
                            break;
                    }
                }
                catch (Exception e)
                {
                    State.Logger.Log("OverlayChangedEntry failed");
                    State.Logger.Log(e);
                }
            }

            #endregion Public Methods
        }

        [HarmonyPatch(typeof(OverlayMenu), "OnToggleSelect")]
        public static class OverlayMenu_OnToggleSelect_MatCol
        {
            #region Public Methods

            [HarmonyPrefix]
            // ReSharper disable once InconsistentNaming
            public static bool EnterToggle(OverlayMenu __instance, KIconToggleMenu.ToggleInfo toggle_info)
            {
                try
                {
                    bool toggleMaterialColor = ((OverlayMenu.OverlayToggleInfo)toggle_info).simView == (SimViewMode)IDs.ToggleMaterialColorOverlayID;

                    if (!toggleMaterialColor)
                    {
                        return true;
                    }

                    State.ConfiguratorState.Enabled = !State.ConfiguratorState.Enabled;

                    RefreshMaterialColor();

                    return false;
                }
                catch (Exception e)
                {
                    State.Logger.Log("EnterToggle failed.");
                    State.Logger.Log(e);
                    return true;
                }
            }

            #endregion Public Methods
        }


        // Custom map size, todo: make it optional and accessable
      //  [HarmonyPatch(typeof(GridSettings), "Reset")]
      //  public static class GridSettings_Reset
      //  {
      //      public static void Prefix(ref int width, ref int height)
      //      {
      //          if (Config.Enabled && Config.CustomWorldSize)
      //          {
      //              try
      //              {
      //                  width = Config.Width * 32;
      //                  height = Config.Height * 32;
      //
      //                  _logger.Log("Custom world dimensions applied");
      //              }
      //              catch (Exception e)
      //              {
      //                  _logger.Log(e);
      //                  _logger.Log("On do offline world gen failed");
      //              }
      //          }
      //      }
      //  }

        [HarmonyPatch(typeof(SimDebugView), "GetOxygenMapColour")]
        public static class SimDebugView_GetOxygenMapColour
        {
            #region Public Methods
            public static Color32 unbreathableColour = new Color(0.5f, 0f, 0f);
            public static bool Prefix(int cell, ref Color __result)
            {
                float minMass = State.ConfiguratorState.GasPressureStart;
                float maxMass = State.ConfiguratorState.GasPressureEnd;

                Element element = Grid.Element[cell];

                if (!element.IsGas)
                {
                    __result = NotGasColor;
                    return false;
                }

                Color gasColor = ColorHelper.GetCellOverlayColor(cell);

                float gasMass = Grid.Mass[cell];

                gasMass -= minMass;

                if (gasMass < 0)
                {
                    gasMass = 0;
                }

                maxMass -= minMass;

                if (maxMass < float.Epsilon)
                {
                    maxMass = float.Epsilon;
                }

                float intensity;
                ColorHSV gasColorHSV = gasColor;
                    float mass = global::Grid.Mass[cell];
                if (element.id == global::SimHashes.Oxygen || element.id == global::SimHashes.ContaminatedOxygen)
                {
                    float optimallyBreathable = global::SimDebugView.optimallyBreathable;
                    intensity = Mathf.Clamp((mass - global::SimDebugView.minimumBreathable) / optimallyBreathable, 0.05f, 1f);

                    // To red for thin air
                    if (intensity < 1f)
                    {
                        gasColorHSV.v = Mathf.Min(gasColorHSV.v+1f-intensity, 0.9f);
                    }

                }
                else
                {
                intensity = GetGasColorIntensity(gasMass, maxMass);
                }
                //Pop ear drum avoider
                if (mass > 2.5f)
                {
                    gasColorHSV.h += 0.035f* Mathf.InverseLerp(2.5f, 3.5f, mass);
                    if (gasColorHSV.h > 1f)
                    {
                        gasColorHSV.h -= 1f;

                    }
                    float intens = Mathf.InverseLerp(20f, 3.5f, mass);

                    gasColorHSV.v = Mathf.Max(0.5f, gasColorHSV.v * intens);

                }


                // New code, use the saturation of a color for the pressure
                gasColorHSV.s = intensity*0.8f;
                __result = gasColorHSV;

                return false;

                // gasColor *= intensity;
                // gasColor.a = 1;
                // __result = gasColor;
            }

            #endregion Public Methods

            #region Private Methods

            private static float GetGasColorIntensity(float mass, float maxMass)
            {
                float minIntensity = State.ConfiguratorState.MinimumGasColorIntensity;

                float intensity = mass / maxMass;

                intensity = Mathf.Sqrt(intensity);

                intensity = Mathf.Clamp01(intensity);
                intensity *= 1 - minIntensity;
                intensity += minIntensity;

                return intensity;
            }

            #endregion Private Methods
        }

        /// <summary>
        /// Material + element color
        /// </summary>
        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public static class SimDebugView_OnPrefabInit_EnterOnce
        {
            #region Private Methods

            [HarmonyPostfix]
            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            private static void Postfix()
            {
                {
                    try
                    {
                        Components.BuildingCompletes.OnAdd += OnBuildingsCompletesAdd;

                        if (!_initialized)
                        {
                            Initialize();
                        }

                        _elementColorInfosChanged = _typeColorOffsetsChanged = _configuratorStateChanged = true;
                    }
                    catch (Exception e)
                    {
                        string message = "Injection failed\n" + e.Message + '\n';

                        State.Logger.Log(message);
                        State.Logger.Log(e);

                        UnityEngine.Debug.LogError(message);
                    }
                    try
                    {
                        SaveTemperatureThresholdsAsDefault();

                        if (State.TemperatureOverlayState.LogThresholds)
                        {
                            LogTemperatureThresholds();
                        }

                        UpdateTemperatureThresholds();
                    }
                    catch (Exception e)
                    {
                        State.Logger.Log("Custom temperature overlay init error");
                        State.Logger.Log(e);
                    }
                }
            }

            #endregion Private Methods
        }

        #endregion Public Classes
    }
}