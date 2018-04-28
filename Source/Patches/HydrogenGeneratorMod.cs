using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;

namespace Patches
{
    /// <summary>
    /// Requires additional work 
    /// </summary>
    public static class HydrogenGeneratorMod3
    {
        [HarmonyPatch(typeof(HydrogenGeneratorConfig), "CreateBuildingDef")]
        public static class HydrogenGeneratorConfig
        {
            public static void Postfix(ref BuildingDef __result)
            {
                __result.OutputConduitType = ConduitType.Solid;
                __result.UtilityOutputOffset = new CellOffset(1, 0);
            }
        }

        [HarmonyPatch(typeof(HydrogenGeneratorConfig), "DoPostConfigureComplete")]
        public static class HydrogenGeneratorConfig2
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> code = instructions.ToList();
                foreach (CodeInstruction codeInstruction in code)
                {
                    if (codeInstruction.opcode == OpCodes.Ldc_I4 && codeInstruction.operand == AccessTools.Field(typeof(SimHashes), SimHashes.Void.ToString()))
                    {
                        codeInstruction.operand = SimHashes.Water;
                    }
                    if (codeInstruction.opcode == OpCodes.Ldc_R4 && codeInstruction.operand == AccessTools.Field(typeof(float), "0.0"))
                    {
                        codeInstruction.operand = 1.0f;
                    }
                    yield return codeInstruction;
                }
            }
        }
    }
}