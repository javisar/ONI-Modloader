using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace CustomWorldMod
{
    public static class HarmonyPatches
    {
        [HarmonyPatch(typeof(GridSettings), nameof(GridSettings.Reset))]
        public static class GridSettings_Reset
        {
            public static void Prefix(ref int width, ref int height)
            {
                //Todo: add config
              //  if (Config.Enabled && Config.CustomWorldSize)
              //{
              //    width  = Config.width;
              //    height = Config.height;
              //}

                width = 8  * 32;
                height = 12 * 32;
            }
        }
    }
}
