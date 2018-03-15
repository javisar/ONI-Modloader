using System;
using System.Collections.Generic;
using ONI_Common;
using ONI_Common.Data;
using UnityEngine;

namespace Common.Json
{
    public class JsonFileLoader
    {
        public JsonFileLoader(JsonManager jsonManager, ONI_Common.IO.Logger logger = null)
        {
            this._logger = logger;

            this.InitializeManagers(jsonManager);
        }

        private readonly ONI_Common.IO.Logger _logger;

        private ConfiguratorStateManager _configuratorStateManager;
        private ElementColorInfosManager _elementColorInfosManager;
        private TypeColorOffsetsManager _typeColorOffsetsManager;

        private void InitializeManagers(JsonManager manager)
        {
            this._configuratorStateManager = new ConfiguratorStateManager(manager, this._logger);
            this._elementColorInfosManager = new ElementColorInfosManager(manager, this._logger);
            this._typeColorOffsetsManager = new TypeColorOffsetsManager(manager, this._logger);
        }

        public bool TryLoadConfiguratorState(out MaterialColorState state)
        {
            try
            {
                state = this._configuratorStateManager.LoadMaterialColorState();
                return true;
            }
            catch (Exception ex)
            {
                const string Message = "Can't load configurator state.";

                this._logger.Log(ex);
                this._logger.Log(Message);

                UnityEngine.Debug.LogError(Message);

                state = new MaterialColorState();

                return false;
            }
        }

        public bool TryLoadElementColorInfos(out Dictionary<SimHashes, ElementColorInfo> elementColorInfos)
        {
            try
            {
                elementColorInfos = this._elementColorInfosManager.LoadElementColorInfosDirectory();
                return true;
            }
            catch (Exception e)
            {
                const string Message = "Can't load ElementColorInfos";

                UnityEngine.Debug.LogError(Message + '\n' + e.Message + '\n');

                State.Logger.Log(Message);
                State.Logger.Log(e);

                elementColorInfos = new Dictionary<SimHashes, ElementColorInfo>();
                return false;
            }
        }

        public bool TryLoadTypeColorOffsets(out Dictionary<string, Color32> typeColorOffsets)
        {
            try
            {
                typeColorOffsets = this._typeColorOffsetsManager.LoadTypeColorOffsetsDirectory();
                return true;
            }
            catch (Exception e)
            {
                const string Message = "Can't load TypeColorOffsets";

                UnityEngine.Debug.LogError(Message + '\n' + e.Message + '\n');

                State.Logger.Log(Message);
                State.Logger.Log(e);

                typeColorOffsets = new Dictionary<string, Color32>();
                return false;
            }
        }

        public bool TryLoadTemperatureState(out TemperatureOverlayState state)
        {
            try
            {
                state = this._configuratorStateManager.LoadTemperatureState();
                return true;
            }
            catch (Exception e)
            {
                this._logger.Log(e);
                this._logger.Log("Can't load overlay temperature state");

                state = new TemperatureOverlayState();

                return false;
            }
        }
    }
}
