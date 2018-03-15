using JetBrains.Annotations;
using MaterialColor.Helpers;
using ONI_Common.Data;
using UnityEngine;

namespace MaterialColor.Extensions
{
    public static class SimHashesExtensions
    {
        public static Color GetMaterialColorForType(this SimHashes material, string objectTypeName)
        {
            if (!ColorHelper.TryGetTypeStandardColor(objectTypeName, out Color32 typeStandardColor))
            {
                if (State.ConfiguratorState.ShowMissingTypeColorOffsets)
                {
                    UnityEngine.Debug.LogError($"Can't find <{objectTypeName}> type color");
                    return typeStandardColor;
                }
            }

            Color32 colorOffsetForWhite = typeStandardColor.TintToWhite();

            if (State.ConfiguratorState.ShowBuildingsAsWhite)
            {
                return colorOffsetForWhite;
            }

            ElementColorInfo elementColorInfo = material.GetMaterialColorInfo();

          //  UnityEngine.Debug.Log("About to multiply - "+ objectTypeName+"-"  + material + "-" + elementColorInfo.ColorMultiplier.Red + "-"+ elementColorInfo.Brightness);
            Color32 multiply = colorOffsetForWhite.Multiply(elementColorInfo.ColorMultiplier);
            Color32 materialColor = multiply.SetBrightness(elementColorInfo.Brightness);

            return materialColor;
        }

        [NotNull]
        public static ElementColorInfo GetMaterialColorInfo(this SimHashes materialHash)
        {
            if (State.ElementColorInfos.TryGetValue(materialHash, out ElementColorInfo elementColorInfo))
            {
                return elementColorInfo;
            }

            if (!State.ConfiguratorState.ShowMissingElementColorInfos)
            {
                return new ElementColorInfo(Color32Multiplier.One);
            }

            UnityEngine.Debug.LogError($"Can't find <{materialHash}> color info");
            return new ElementColorInfo(new Color32Multiplier(1, 0, 1));
        }

        public static Color ToCellMaterialColor(this SimHashes material)
        {
            ElementColorInfo colorInfo = material.GetMaterialColorInfo();

            Color result = new Color(
                colorInfo.ColorMultiplier.Red,
                colorInfo.ColorMultiplier.Green,
                colorInfo.ColorMultiplier.Blue
                ) * colorInfo.Brightness;

            result.a = byte.MaxValue;

            return result;
        }

        public static Color32 ToDebugColor(this SimHashes material)
        {
            Element element = ElementLoader.FindElementByHash(material);

            if (element?.substance != null)
            {
                Color32 debugColor = element.substance.debugColour;

                debugColor.a = byte.MaxValue;

                return debugColor;
            }

            return Color.white;
        }
    }
}
