using System;
using System.IO;
using Common.Json;
using ONI_Common.Core;
using ONI_Common.Data;
using ONI_Common.IO;

namespace ONI_Common.OnionHooks
{
    public static class Hooks
    {
        public static OnionState Config
        {
            get
            {
                if (_config != null)
                {
                    return _config;
                }

                try
                {
                    _stateManager = _stateManager ?? new ConfiguratorStateManager(new JsonManager());

                    TryLoadConfig();
                    StartConfigFileWatcher();
                }
                catch (Exception e)
                {
                    _logger.Log("State load/init failed");
                    _logger.Log(e);
                    _config = new OnionState();
                }

                return _config;
            }
            private set
            {
                _config = value;
            }
        }

        private static OnionState _config;
        private static ConfiguratorStateManager _stateManager;
        private static readonly ONI_Common.IO.Logger _logger = new ONI_Common.IO.Logger(Paths.OnionLogFileName);

        private static CameraController _cameraController;

        private static bool TryLoadConfig()
        {
            try
            {
                Config = _stateManager.LoadOnionState();

                _logger.Log("OnionState.json loaded");

                return true;
            }
            catch
            {
                Config = new OnionState();

                const string Message = "OnionState.json loading failed";

                _logger.Log(Message);
                Debug.LogError(Message);

                return false;
            }
        }

        private static void StartConfigFileWatcher()
        {
            FileChangeNotifier.StartFileWatch(Paths.OnionStateFileName, Paths.OnionConfigPath, OnConfigChanged);

            _logger.Log("Config file watcher started");
        }

        private static void OnConfigChanged(object sender, FileSystemEventArgs e)
        {
            _logger.Log("Config changed");

            TryLoadConfig();
            UpdateDebugHandler();
        }

        public static void OnInitRandom(ref int worldSeed, ref int layoutSeed, ref int terrainSeed, ref int noiseSeed)
        {
            if (!Config.Enabled || !Config.CustomSeeds)
            {
                return;
            }

            worldSeed = Config.WorldSeed >= 0 ? Config.WorldSeed : worldSeed;
            layoutSeed = Config.LayoutSeed >= 0 ? Config.LayoutSeed : layoutSeed;
            terrainSeed = Config.TerrainSeed >= 0 ? Config.TerrainSeed : terrainSeed;
            noiseSeed = Config.NoiseSeed >= 0 ? Config.NoiseSeed : noiseSeed;

            if (!Config.LogSeed)
            {
                return;
            }

            string message = string.Format($"Size: {Config.Width}x{Config.Height} World Seed: {worldSeed} Layout Seed: {layoutSeed}" +
                                        $"Terrain Seed: {terrainSeed} Noise Seed: {noiseSeed}");

            _logger.Log(message);
        }

        public static void OnDoOfflineWorldGen()
        {
            if (Config.Enabled && Config.CustomWorldSize)
            {
                try
                {
                    ResetGridSettingsChunks(Config.Width, Config.Height);

                    _logger.Log("Custom world dimensions applied");
                }
                catch (Exception e)
                {
                    _logger.Log(e);
                    _logger.Log("On do offline world gen failed");
                }
            }
            else
            {
                OnionState defaultConfig = new OnionState();

                ResetGridSettingsChunks(defaultConfig.Width, defaultConfig.Height);
            }
        }

        private static void ResetGridSettingsChunks(int width, int height)
        {
            GridSettings.Reset(width * 32, height * 32);
        }

        public static void OnDebugHandlerCtor()
        {
            try
            {
                UpdateDebugHandler();
            }
            catch (Exception e)
            {
                _logger.Log("On debug handler constructor failed");
                _logger.Log(e);
            }
        }


        public static void UpdateDebugHandler()
        {
#if !Patch
            DebugHandler.enabled = Config.Enabled && Config.Debug;
#endif
            DebugHandler.FreeCameraMode = Config.Enabled && Config.FreeCamera;
        }

    }
}
