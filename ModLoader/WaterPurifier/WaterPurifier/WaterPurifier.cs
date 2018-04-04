using Harmony;
using UnityEngine;

namespace WaterPurifier
{
    [HarmonyPatch(typeof(WaterPurifierConfig), "ConfigureBuildingTemplate")]
    public static class WaterPurifier
    {
        public static void Postfix(WaterPurifierConfig __instance, ref GameObject go)
        {
            Debug.Log("=============== Water Purifier Changed =======================");         

            ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
            //ElementConverter.OutputElement[] oldOutputElements = elementConverter.outputElements;
            ElementConverter.OutputElement[] newOutputElements;

            newOutputElements = new ElementConverter.OutputElement[]
            {
                new ElementConverter.OutputElement(5f, SimHashes.Water, 0f, true, 0f, 0.5f, true, 0.75f, 255, 0),
                new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 0f, true, 0f, 0.5f, true, 0.25f, 255, 0)
            };

            elementConverter.outputElements = newOutputElements;

            //elementConverter = go.AddOrGet<ElementConverter>();
            //checkNewElements = elementConverter.outputElements;
            //Debug.Log("Total elements: " + checkNewElements.Count().ToString());
            //foreach (var item in checkNewElements)
            //Debug.Log("outputElements: " + item.element.id.ToString());

            Debug.Log("=================== END ======================");
        }
    }
}
