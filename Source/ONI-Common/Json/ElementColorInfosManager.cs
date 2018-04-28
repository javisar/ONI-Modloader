namespace ONI_Common.Json
{
    using ONI_Common.Data;
    using ONI_Common.IO;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ElementColorInfosManager : BaseManager
    {
        public ElementColorInfosManager(JsonManager manager, Logger logger = null)
        : base(manager, logger)
        {
        }

        /// <summary>
        /// Loads ElementColorInfos assoctiated with material from the configuration files
        /// </summary>
        /// <returns></returns>
        public Dictionary<SimHashes, ElementColorInfo> LoadElementColorInfosDirectory(string directoryPath = null)
        {
            if (directoryPath == null)
            {
                directoryPath = Paths.ElementColorInfosDirectory;
            }

            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            FileInfo[]    files     = directory.GetFiles("*.json");

            Dictionary<SimHashes, ElementColorInfo> result = new Dictionary<SimHashes, ElementColorInfo>();

            foreach (FileInfo file in files)
            {
                string                                  filePath = Path.Combine(directoryPath, file.Name);
                Dictionary<SimHashes, ElementColorInfo> resultFromCurrentFile;

                try
                {
                    resultFromCurrentFile = this.LoadSingleElementColorInfosFile(filePath);
                }
                catch (Exception e)
                {
                    if (this._logger != null)
                    {
                        this._logger.Log($"Error loading {filePath} as ElementColorInfo configuration file.");
                        this._logger.Log(e);
                    }

                    continue;
                }

                foreach (KeyValuePair<SimHashes, ElementColorInfo> entry in resultFromCurrentFile)
                {
                    if (result.ContainsKey(entry.Key))
                    {
                        result[entry.Key] = entry.Value;
                    }
                    else
                    {
                        result.Add(entry.Key, entry.Value);
                    }
                }

                this._logger?.Log($"Loaded {filePath} as ElementColorInfo configuration file.");
            }

            return result;
        }

        public Dictionary<SimHashes, ElementColorInfo> LoadSingleElementColorInfosFile(string filePath)
        {
            return this._manager.Deserialize<Dictionary<SimHashes, ElementColorInfo>>(filePath);
        }

        public void SaveElementsColorInfo(Dictionary<SimHashes, ElementColorInfo> dictionary, string path = null)
        {
            if (path == null)
            {
                path = Paths.DefaultElementColorInfosPath;
            }

            this._manager.Serialize(dictionary, path);
        }
    }
}