namespace SensorsMod
{
    using Harmony;

    using UnityEngine;

    using Debug = Debug;

    [HarmonyPatch(typeof(LogicPressureSensorGasConfig), "DoPostConfigureComplete", new[] { typeof(GameObject) })]
    internal static class PressureSensorGasMod
    {
        private static void Postfix(LogicPressureSensorGasConfig __instance, GameObject go)
        {
            Debug.Log(" === PressureSensorGasMod INI === ");
            LogicPressureSensor logicPressureSensor = go.AddOrGet<LogicPressureSensor>();
            AccessTools.Field(typeof(LogicPressureSensor), "rangeMax").SetValue(logicPressureSensor, 25f);

            // logicPressureSensor.rangeMax = 25f;
            Debug.Log(" === PressureSensorGasMod END === ");
        }
    }

    /*
    [HarmonyPatch(typeof(LogicPressureSensorGasConfig), "DoPostConfigureComplete", new Type[] { typeof(GameObject) })]
    internal class PressureSensorGasMod
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === PressureSensorGasMod INI === " + original.Name);

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = codes.Count-1; i >= 0; i--)
            {
                CodeInstruction instruction = codes[i];
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 20)
                    instruction.operand = (float) 25.0;

                yield return instruction;
            }
            Debug.Log(" === PressureSensorGasMod END === ");
        }
    }
    */
}