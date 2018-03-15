using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using ONI_Common;

namespace Common.Json
{
    public class TypeColorOffsetsManager : BaseManager
    {
        public TypeColorOffsetsManager(JsonManager manager, ONI_Common.IO.Logger logger = null) : base(manager, logger) { }

        public void SaveTypesColors(Dictionary<string, Color32> dictionary, string path = null)
        {
            if (path == null)
            {
                path = Paths.DefaultTypeColorOffsetsPath;
            }

            this._manager.Serialize(dictionary, path);
        }

        public Dictionary<string, Color32> LoadTypeColorOffsetsDirectory(string directoryPath = null)
        {
            if (directoryPath == null)
            {
                directoryPath = Paths.TypeColorOffsetsDirectory;
            }

            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            FileInfo[] files = directory.GetFiles("*.json");

            Dictionary<string, Color32> result = new Dictionary<string, Color32>();

            foreach (FileInfo file in files)
            {
                string filePath = Path.Combine(directoryPath, file.Name);
                Dictionary<string, Color32> resultFromCurrentFile;

                try
                {
                    resultFromCurrentFile = this.LoadSingleTypeColorOffsetsFile(filePath);
                }
                catch (Exception e)
                {
                    if (this._logger != null)
                    {
                        this._logger.Log($"Error loading {filePath} as TypeColorOffset configuration file.");
                        this._logger.Log(e);
                    }
                    continue;
                }

                foreach (KeyValuePair<string, Color32> entry in resultFromCurrentFile)
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

                this._logger?.Log($"Loaded {filePath} as TypeColorOffset configuration file.");
            }

            return result;
        }

        public Dictionary<string, Color32> LoadSingleTypeColorOffsetsFile(string path)
        {
            return this._manager.Deserialize<Dictionary<string, Color32>>(path);
        }
    }
}
