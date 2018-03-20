namespace SensorsMod
{
    using Harmony;

    using UnityEngine;

    using Debug = Debug;

    [HarmonyPatch(typeof(LogicPressureSensorLiquidConfig), "DoPostConfigureComplete", new[] { typeof(GameObject) })]
    internal static class PressureSensorLiquidMod
    {
        private static void Postfix(LogicPressureSensorLiquidConfig __instance, GameObject go)
        {
            Debug.Log(" === PressureSensorLiquidMod INI === ");
            LogicPressureSensor logicPressureSensor = go.AddOrGet<LogicPressureSensor>();

            AccessTools.Field(typeof(LogicPressureSensor), "rangeMax").SetValue(logicPressureSensor, 10000.0f);

            // logicPressureSensor.rangeMax = 10000.0f;
            Debug.Log(" === PressureSensorLiquidMod END === ");
        }
    }

    /*
    [HarmonyPatch(typeof(LogicPressureSensorLiquidConfig), "DoPostConfigureComplete", new Type[] { typeof(GameObject) })]
    internal class PressureSensorLiquidMod
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === PressureSensorLiquidMod INI === " + original.Name);

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                CodeInstruction instruction = codes[i];
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 2000)
                    instruction.operand = (float)10000.0;

                yield return instruction;
            }
            Debug.Log(" === PressureSensorLiquidMod END === ");
        }
    }
    */
}