using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace Patches
{
    public static class StorageLockerConfigMod
    {
        [HarmonyPatch(typeof(StorageLockerConfig), "CreateBuildingDef")]
        public static class StorageLockerConfig
        {
            public static void Postfix(ref BuildingDef __result)
            {
                __result.BuildLocationRule = BuildLocationRule.Anywhere;
            }
        }
    }
}
