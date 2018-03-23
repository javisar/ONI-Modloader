using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using JetBrains.Annotations;

namespace Patches
{
    public static class StorageLockerConfigMod
    {
        [HarmonyPatch(typeof(StorageLockerConfig), nameof(StorageLockerConfig.CreateBuildingDef))]
        public static class StorageLockerConfigModPatch
        {
            public static void Postfix([NotNull] ref BuildingDef __result)
            {
                __result.BuildLocationRule = BuildLocationRule.Anywhere;
            }
        }
    }
}
