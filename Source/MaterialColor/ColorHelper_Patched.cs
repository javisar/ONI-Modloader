using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using MaterialColor.Extensions;
using MaterialColor.Helpers;
using UnityEngine;

namespace MaterialColor
{
    public static class ColorHelper_Patched
    {
        public static Color?[] TileColors;

        public static readonly Color32 DefaultColor =
        new Color32(Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue);

        public static void UpdateBuildingColor(BuildingComplete building)
        {
            var buildingName = building.name.Replace("Complete", String.Empty);
            var material = MaterialHelper.ExtractMaterial(building);

            Color32 color;

            if (State.ConfiguratorState.Enabled)
            {
                switch (State.ConfiguratorState.ColorMode)
                {
                    case ColorMode.Json:
                        color = material.GetMaterialColorForType(buildingName);
                        break;

                    case ColorMode.DebugColor:
                        color = material.ToDebugColor();
                        break;

                    case ColorMode.None:
                    default:
                        color = DefaultColor;
                        break;
                }
            }
            else color = DefaultColor;

            if (State.TileNames.Contains(buildingName))
            {
                try
                {
                    if (TileColors == null)
                    {
                        TileColors = new Color?[Grid.CellCount];
                    }

                    TileColors[Grid.PosToCell(building.gameObject)] = color;

                    return;
                }
                catch (Exception e)
                {
                    State.Logger.Log("Error while aquiring cell color");
                    State.Logger.Log(e);
                }
            }

            Color32 dimmedColor = color.SetBrightness(color.GetBrightness() / 2);

            // storagelocker
            var storageLocker = building.GetComponent<StorageLocker>();

            if (storageLocker != null)
            {
                SetFilteredStorageColors(storageLocker.filteredStorage, color, dimmedColor);
            }
            else // ownable
            {
                var ownable = building.GetComponent<Ownable>();

                if (ownable != null)
                {
                    ownable.ownedTint = color;
                    ownable.unownedTint = dimmedColor;
                    ownable.UpdateTint();
                }
                else // rationbox
                {
                    var rationBox = building.GetComponent<RationBox>();

                    if (rationBox != null)
                    {
                        SetFilteredStorageColors(rationBox.filteredStorage, color, dimmedColor);
                    }
                    else // refrigerator
                    {
                        var fridge = building.GetComponent<Refrigerator>();

                        if (fridge != null)
                        {
                            SetFilteredStorageColors(fridge.filteredStorage, color, dimmedColor);
                        }
                        else // anything else
                        {
                            var kAnimControllerBase = building.GetComponent<KAnimControllerBase>();

                            if (kAnimControllerBase != null)
                            {
                                kAnimControllerBase.TintColour = color;
                            }
                            else
                            {
                                Debug.LogError($"Can't find KAnimControllerBase component in <{buildingName}> and its not a registered tile.");
                            }
                        }
                    }
                }
            }
        }
        public static void ResetCell(int cellIndex)
        {
            if (ColorHelper.TileColors.Length > cellIndex)
            {
                ColorHelper.TileColors[cellIndex] = null;
            }
        }
        private static void SetFilteredStorageColors(FilteredStorage storage, Color32 color, Color32 dimmedColor)
        {
            storage.filterTint = color;
            storage.noFilterTint = dimmedColor;
            storage.FilterChanged();
        }
    }
}