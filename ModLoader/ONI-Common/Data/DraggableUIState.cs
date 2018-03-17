using System;
using System.Collections.Generic;
using Common.Json;
using UnityEngine;

namespace ONI_Common.Data
{
    public class DraggableUIState
    {
        private Dictionary<string, SerializeableVector2> WindowPositions
            => this._windowPositions ?? (this._windowPositions = this.LoadFile());

        private Dictionary<string, SerializeableVector2> _windowPositions;

        public void SaveWindowPosition(GameObject window, Vector2 position)
        {
            string key = this.ExtractKey(window);

            this.WindowPositions[key] = this.VectorToTuple(position);

            this.UpdateFile();
        }

        public bool LoadWindowPosition(GameObject window, out Vector2 position)
        {
            string key = this.ExtractKey(window);

            SerializeableVector2 sVector2;

            bool result = this.WindowPositions.TryGetValue(key, out sVector2);

            position = sVector2.ToVector2();

            return result;
        }

        private string ExtractKey(GameObject window)
            => window.name;

        private void UpdateFile()
        {
            try
            {
                this._jsonManager.Serialize(this.WindowPositions, Paths.DraggableUIStatePath);
            }
            catch (Exception e)
            {

                State.Logger.Log("Draggable UI state save failed.");
                State.Logger.Log(e);
            }
        }

        private Dictionary<string, SerializeableVector2> LoadFile()
        {
            try
            {
                return this._jsonManager.Deserialize<Dictionary<string, SerializeableVector2>>(Paths.DraggableUIStatePath);
            }
            catch (Exception e)
            {
                State.Logger.Log("Draggable UI state load failed.");
                State.Logger.Log(e);

                return new Dictionary<string, SerializeableVector2>();
            }
        }

        private JsonManager _jsonManager = new JsonManager();

        private SerializeableVector2 VectorToTuple(Vector2 vector)
            => new SerializeableVector2(vector.x, vector.y);
    }
}
