using Common.Json;
using System.Collections.Generic;
using JetBrains.Annotations;
using ONI_Common;
using ONI_Common.Data;
using UnityEngine;
using TemperatureOverlayState = ONI_Common.Data.TemperatureOverlayState;

namespace MaterialColor
{
    public static class State
    {
        [NotNull] private static readonly JsonFileLoader JsonLoader = new JsonFileLoader(new JsonManager(), Logger);

        [NotNull]
        public static ONI_Common.IO.Logger Logger => _logger ?? (_logger = new ONI_Common.IO.Logger(Paths.MaterialColorLogFileName));

        private static ONI_Common.IO.Logger _logger;

        [NotNull]
        public static Dictionary<string, Color32> TypeColorOffsets
        {
            get
            {
                if (_typeColorOffsets != null)
                {
                    return _typeColorOffsets;
                }

                JsonLoader.TryLoadTypeColorOffsets(out _typeColorOffsets);

                return _typeColorOffsets;
            }
            private set => _typeColorOffsets = value;
        }

        private static Dictionary<string, Color32> _typeColorOffsets;

        [NotNull]
        public static Dictionary<SimHashes, ElementColorInfo> ElementColorInfos
        {
            get
            {
                if (_elementColorInfos != null)
                {
                    return _elementColorInfos;
                }

               // Dictionary<SimHashes, ElementColorInfo> colorInfos;
                JsonLoader.TryLoadElementColorInfos(out _elementColorInfos);

                return _elementColorInfos;
            }
            private set => _elementColorInfos = value;
        }

        private static Dictionary<SimHashes, ElementColorInfo> _elementColorInfos;

        [NotNull]
        public static MaterialColorState ConfiguratorState
        {
            get
            {
                if (_configuratorState != null)
                {
                    return _configuratorState;
                }

                JsonLoader.TryLoadConfiguratorState(out _configuratorState);

                return _configuratorState;
            }
            private set => _configuratorState = value;
        }

        private static MaterialColorState _configuratorState;

        public static bool TryReloadConfiguratorState()
        {
            if (!JsonLoader.TryLoadConfiguratorState(out MaterialColorState state))
            {
                return false;
            }

            ConfiguratorState = state;

            return true;
        }

        public static bool TryReloadTypeColorOffsets()
        {
            if (!JsonLoader.TryLoadTypeColorOffsets(out Dictionary<string, Color32> colorOffsets))
            {
                return false;
            }

            TypeColorOffsets = colorOffsets;

            return true;
        }

        public static bool TryReloadElementColorInfos()
        {
            if (!JsonLoader.TryLoadElementColorInfos(out Dictionary<SimHashes, ElementColorInfo> colorInfos))
            {
                return false;
            }

            ElementColorInfos = colorInfos;

            return true;
        }

        // TODO: load from file instead
        [NotNull] public static readonly List<string> TileNames = new List<string>
        {
            "Tile", "MeshTile", "InsulationTile", "GasPermeableMembrane", "TilePOI", "PlasticTile", "MetalTile"
        };

        [NotNull]
        public static TemperatureOverlayState TemperatureOverlayState
        {
            get
            {
                if (_temperatureOvelayState != null)
                {
                    return _temperatureOvelayState;
                }

                JsonLoader.TryLoadTemperatureState(out _temperatureOvelayState);

                return _temperatureOvelayState;
            }
            private set => _temperatureOvelayState = value;
        }

        [NotNull] public static readonly List<float> DefaultTemperatures = new List<float>();

        [NotNull] public static readonly List<Color> DefaultTemperatureColors = new List<Color>();

        private static TemperatureOverlayState _temperatureOvelayState;

        public static bool TryReloadTemperatureState()
        {
            if (!JsonLoader.TryLoadTemperatureState(out TemperatureOverlayState temperatureState))
            {
                return false;
            }

            TemperatureOverlayState = temperatureState;

            return true;
        }
    }
}
