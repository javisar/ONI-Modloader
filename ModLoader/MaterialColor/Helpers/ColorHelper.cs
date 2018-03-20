namespace MaterialColor.Helpers
{
    using MaterialColor.Extensions;
    using System;
    using UnityEngine;

    public static class ColorHelper
    {
        public static readonly Color32 DefaultColor =
        new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

        public static readonly Color32 MissingDebugColor = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

        public static readonly Color32 NoOffset = new Color32(0, 0, 0, byte.MaxValue);

        public static Color?[] TileColors;

        public static Color DefaultCellColor => new Color(1, 1, 1);

        public static Color InvalidCellColor => new Color(1, 0, 0);

        public static Color GetCellColorDebug(int cellIndex)
        {
            Element   element   = Grid.Element[cellIndex];
            Substance substance = element.substance;

            Color32 debugColor = substance.debugColour;

            debugColor.a = byte.MaxValue;

            return debugColor;
        }

        public static Color GetCellColorJson(int cellIndex)
        {
            SimHashes material = MaterialHelper.GetMaterialFromCell(cellIndex);
            return material.ToCellMaterialColor();
        }

        public static Color GetCellOverlayColor(int cellIndex)
        {
            Element   element   = Grid.Element[cellIndex];
            Substance substance = element.substance;

            Color32 overlayColor = substance.overlayColour;

            overlayColor.a = byte.MaxValue;

            return overlayColor;
        }

        public static bool TryGetTypeStandardColor(string typeName, out Color32 standardColor)
        {
            Color32 typeStandardColor;
            if (ONI_Common.State.TypeColorOffsets.TryGetValue(typeName, out typeStandardColor))
            {
                standardColor = typeStandardColor;
                return true;
            }

            standardColor = ONI_Common.State.ConfiguratorState.ShowMissingTypeColorOffsets
                            ? MissingDebugColor
                            : NoOffset;

            return false;
        }

        private static void BreakdownGridObjectsComponents(int cellIndex)
        {
            for (int i = 0; i <= 20; i++)
            {
                ONI_Common.State.Logger.Log("Starting object from grid component breakdown, index: " + cellIndex);

                try
                {
                    Component[] comps = Grid.Objects[cellIndex, i].GetComponents<Component>();

                    foreach (Component comp in comps)
                    {
                        ONI_Common.State.Logger.Log($"Object Layer: {i}, Name: {comp.name}, Type: {comp.GetType()}");
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    ONI_Common.State.Logger.Log($"Cell Index: {cellIndex}, Layer: {i}");
                    ONI_Common.State.Logger.Log(e);
                }

                // catch { }
            }
        }
    }
}