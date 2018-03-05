using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ModLoader
{           
	
    [HarmonyPatch(typeof(CameraController), "OnSpawn", new Type[0] )]
    internal class CameraControllerMod
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === CameraControllerMod INI === ");

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = codes.Count-1; i >= 0; i--)
            {
                CodeInstruction instruction = codes[i];
                if (instruction.opcode == OpCodes.Call)
                {
                    
                    Traverse.Create<CameraController>().Property("maxOrthographicSize").SetValue(300.0);
                    Traverse.Create<CameraController>().Property("maxOrthographicSizeDebug").SetValue(300.0);

                    //Traverse.Create<CameraController>().Method("SetOrthographicsSize").SetValue(Traverse.Create<CameraController>().Property("DEFAULT_MAX_ORTHO_SIZE").GetValue());
                }
                yield return instruction;
            }
            Debug.Log(" === CameraControllerMod END === ");
        }
    }	
}
