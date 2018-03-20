using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Harmony;

namespace Patches
{
    [HarmonyPatch(typeof(ElectrolyzerConfig), "CreateBuildingDef")]
    public static class ElectrolyzerMod
    {
        public static void Postfix(ref BuildingDef _result)
        {
            _result.EnergyConsumptionWhenActive = 500f;
            _result.SelfHeatKilowattsWhenActive = 0.2f;
        }
    }
    [HarmonyPatch(typeof(ElectrolyzerConfig), "ConfigureBuildingTemplate")]
    public static class ElectrolyzerMod2
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> code = instructions.ToList();
            foreach (CodeInstruction codeInstruction in code)
            {
                if (codeInstruction.opcode == OpCodes.Ldc_R4)
                {
                    codeInstruction.operand = 323.15f;
                }
                yield return codeInstruction;
            }
        }
    }
}