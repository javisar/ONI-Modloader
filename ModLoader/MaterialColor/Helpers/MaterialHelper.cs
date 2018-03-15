using System;
using UnityEngine;

namespace MaterialColor.Helpers
{
    public static class MaterialHelper
    {
        public static SimHashes GetMaterialFromCell(int cellIndex)
        {
            return Grid.IsValidCell(cellIndex) ? TryCellIndexToSimHash(cellIndex) : SimHashes.Vacuum;
        }

        private static SimHashes TryCellIndexToSimHash(int cellIndex)
        {
            try
            {
                return CellIndexToSimHash(cellIndex);
            }
            catch (Exception e)
            {
                State.Logger.Log("Cell or element index from index failed");
                State.Logger.Log(e);
            }

            return SimHashes.Vacuum;
        }

        private static SimHashes CellIndexToSimHash(int cellIndex)
        {
            byte cell = Grid.ElementIdx[cellIndex];

            byte     cellElementIndex = cell;
            Element element          = ElementLoader.elements?[cellElementIndex];

            if (element != null)
            {
                return element.id;
            }

            State.Logger.Log("Element from cell failed.");

            return SimHashes.Vacuum;
        }

        public static SimHashes ExtractMaterial(Component component)
        {
            PrimaryElement primaryElement = component?.GetComponent<PrimaryElement>();

            if (primaryElement != null)
            {
                return primaryElement.ElementID;
            }

            State.Logger.Log("PrimaryElement not found in: " + component);
            return SimHashes.Vacuum;
        }
    }
}