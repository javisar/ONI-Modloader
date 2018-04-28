namespace ONI_Common.OnionHooks
{
    using ONI_Common.Data;
    using ONI_Common.IO;
    using ONI_Common.Json;
    using System;
    using System.IO;

    public static class Hooks
    {
        private static readonly Logger _logger = new IO.Logger(Paths.OnionLogFileName);

        private static OnionState _config;

        private static ConfiguratorStateManager _stateManager;

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




        public static void OnInitRandom(ref int worldSeed, ref int layoutSeed, ref int terrainSeed, ref int noiseSeed)
        {
            if (!Config.Enabled || !Config.CustomSeeds)
            {
                return;
            }

            worldSeed   = Config.WorldSeed   >= 0 ? Config.WorldSeed : worldSeed;
            layoutSeed  = Config.LayoutSeed  >= 0 ? Config.LayoutSeed : layoutSeed;
            terrainSeed = Config.TerrainSeed >= 0 ? Config.TerrainSeed : terrainSeed;
            noiseSeed   = Config.NoiseSeed   >= 0 ? Config.NoiseSeed : noiseSeed;

            if (!Config.LogSeed)
            {
                return;
            }

            string message = string.Format(
                                           $"Size: {Config.Width}x{Config.Height} World Seed: {worldSeed} Layout Seed: {layoutSeed}"
                                         + $"Terrain Seed: {terrainSeed} Noise Seed: {noiseSeed}");

            _logger.Log(message);
        }



        private static void OnConfigChanged(object sender, FileSystemEventArgs e)
        {
            _logger.Log("Config changed");

            TryLoadConfig();
        }

        private static void StartConfigFileWatcher()
        {
            FileChangeNotifier.StartFileWatch(Paths.OnionStateFileName, Paths.OnionConfigPath, OnConfigChanged);

            _logger.Log("Config file watcher started");
        }

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
    }
}