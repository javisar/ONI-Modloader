using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace SensorsMod
{

    [HarmonyPatch(typeof(LogicTemperatureSensorConfig), "DoPostConfigureComplete", new Type[] { typeof(GameObject) })]
    internal class TemperatureSensorMod
    {
        private static void Postfix(LogicTemperatureSensorConfig __instance, GameObject go)
        {
            Debug.Log(" === TemperatureSensorMod INI === ");
            LogicTemperatureSensor logicTemperatureSensor = go.AddOrGet<LogicTemperatureSensor>();
            logicTemperatureSensor.maxTemp = 1573.15f;
            Debug.Log(" === TemperatureSensorMod END === ");
        }
    }

    /*
    [HarmonyPatch(typeof(LogicTemperatureSensorConfig), "DoPostConfigureComplete", new Type[] { typeof(GameObject) })]
    internal class TemperatureSensorMod
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === TemperatureSensorMod INI === " + original.Name);

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = codes.Count - 1; i >= 0; i--)
            {
                CodeInstruction instruction = codes[i];
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 573.15)
                    instruction.operand = (float)1273.15;

                yield return instruction;
            }
            Debug.Log(" === TemperatureSensorMod END === ");
        }
    }
    */
}
