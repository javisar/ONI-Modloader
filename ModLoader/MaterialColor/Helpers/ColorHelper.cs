using System;
using MaterialColor.Extensions;
using UnityEngine;

namespace MaterialColor.Helpers
{
    public static class ColorHelper
    {
        public static readonly Color32 DefaultColor =
        new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        public static Color?[] TileColors;


        public static readonly Color32 MissingDebugColor =
            new Color32(Byte.MaxValue, 0, Byte.MaxValue, Byte.MaxValue);

        public static readonly Color32 NoOffset =
            new Color32(0, 0, 0, Byte.MaxValue);



        public static bool TryGetTypeStandardColor(string typeName, out Color32 standardColor)
        {
            Color32 typeStandardColor;
            if (State.TypeColorOffsets.TryGetValue(typeName, out typeStandardColor))
            {
                standardColor = typeStandardColor;
                return true;
            }

            standardColor = State.ConfiguratorState.ShowMissingTypeColorOffsets
                ? MissingDebugColor
                : NoOffset;

            return false;
        }

        public static Color DefaultCellColor
            => new Color(1, 1, 1);

        public static Color InvalidCellColor
            => new Color(1, 0, 0);

        private static void BreakdownGridObjectsComponents(int cellIndex)
        {
            for (int i = 0; i <= 20; i++)
            {
                State.Logger.Log("Starting object from grid component breakdown, index: " + cellIndex);

                try
                {
                    Component[] comps = Grid.Objects[cellIndex, i].GetComponents<Component>();

                    foreach (Component comp in comps)
                    {
                        State.Logger.Log($"Object Layer: {i}, Name: {comp.name}, Type: {comp.GetType()}");
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    State.Logger.Log($"Cell Index: {cellIndex}, Layer: {i}");
                    State.Logger.Log(e);
                }
                //catch { }
            }
        }

        public static Color GetCellColorJson(int cellIndex)
        {
            SimHashes material = MaterialHelper.GetMaterialFromCell(cellIndex);
            return material.ToCellMaterialColor();
        }

        public static Color GetCellColorDebug(int cellIndex)
        {
            Element element = Grid.Element[cellIndex];
            Substance substance = element.substance;

            Color32 debugColor = substance.debugColour;

            debugColor.a = Byte.MaxValue;

            return debugColor;
        }

        public static Color GetCellOverlayColor(int cellIndex)
        {
            Element element   = Grid.Element[cellIndex];
            Substance substance = element.substance;

            Color32 overlayColor = substance.overlayColour;

            overlayColor.a = Byte.MaxValue;

            return overlayColor;
        }
    }
}
