using Harmony;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace ModLoader
{       

	[HarmonyPatch(typeof(LogicPressureSensorLiquidConfig), "DoPostConfigureComplete", new Type[] { typeof(GameObject) })]
    internal class TestLogicPressureSensorLiquidConfig
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === TestLogicPressureSensorLiquidConfig INI === " + original.Name);
            
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                CodeInstruction instruction = codes[i];
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 2000)
                    instruction.operand = (float)10000.0;

                yield return instruction;       
            }
            Debug.Log(" === TestLogicPressureSensorLiquidConfig END === ");
        }
    }

    /*
    [HarmonyPatch(typeof(LogicPressureSensorGasConfig), "DoPostConfigureComplete", new Type[] { typeof(GameObject) })]
    internal class TestLogicPressureSensorGasConfig
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === TestLogicPressureSensorGasConfig INI === " + original.Name);
            foreach (CodeInstruction instruction in Helper.Reverse(instructions))
            {
                if (instruction.opcode == OpCodes.Ldc_R4
                    && (float)instruction.operand == 2)
                {
                    instruction.operand = 25.0;
                }
            }
            Debug.Log(" === TestLogicPressureSensorGasConfig END === ");
            return instructions;
        }
    }


    [HarmonyPatch(typeof(LogicTemperatureSensorConfig), "DoPostConfigureComplete", new Type[] { typeof(GameObject) })]
    internal class TestLogicTemperatureSensorConfig
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === TestLogicTemperatureSensorConfig INI === " + original.Name);
            foreach (CodeInstruction instruction in Helper.Reverse(instructions))
            {
                if (instruction.opcode == OpCodes.Ldc_R4)
                {
                    instruction.operand = 1273.15;
                }
            }
            Debug.Log(" === TestLogicTemperatureSensorConfig END === ");
            return instructions;
        }
    }
    */
	/*
    [HarmonyPatch(typeof(CameraController), "OnSpawn", new Type[0] )]
    internal class TestCameraController
    {
        private static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            Debug.Log(" === TestCameraController INI === " + original.Name);

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

            for (int i = codes.Count-1; i >= 0; i--)
            {
                CodeInstruction instruction = codes[i];
                if (instruction.opcode == OpCodes.Call)
                {
                    
                    Traverse.Create<CameraController>().Property("maxOrthographicSize").SetValue(300.0);
                    Traverse.Create<CameraController>().Property("maxOrthographicSizeDebug").SetValue(300.0);

                    Traverse.Create<CameraController>().Method("SetOrthographicsSize").SetValue(Traverse.Create<CameraController>().Property("DEFAULT_MAX_ORTHO_SIZE").GetValue());

                }

                yield return instruction;
            }
            Debug.Log(" === TestCameraController END === ");
        }
    }
	*/

    /*
    [HarmonyPatch(typeof(FallingWater))]
    [HarmonyPatch("gravityScale", PropertyMethod.Getter)]
    internal class TestGravityWater
    {
        private static readonly FieldInfo gravityScale = AccessTools.Field(typeof(FallingWater), "gravityScale");

        [HarmonyPostfix]
        public static void NormalGravity(FallingWater __instance, ref float __result)
        {            
            Debug.Log(" === TestGravityWater INI === ");

            __result = 1f;

            Debug.Log(" === TestGravityWater END === ");
        }
     
    }
    */
    /*
    [HarmonyPatch(typeof(FallingWater), "onSpawn", new Type[0])]
    internal class TestGravityWater
    {

        private static void Postfix(FallingWater __instance)
        {
            Debug.Log(" === TestGravityWater INI === "+ __instance.GetType().ToString());

            PropertyInfo gravityScale = AccessTools.Property(typeof(FallingWater), "gravityScale");
            gravityScale.SetValue(__instance, 1.0f,null);

            Debug.Log(" === TestGravityWater END === ");
        }
    }
    */
	/*
    [HarmonyPatch(typeof(Global), "Start", new Type[0])]
    internal class TestStart
    {

        private static void Postfix(Global __instance)
        {
            Debug.Log(" === TestStart INI === ");
            Debug.Log(" === TestStart === "+STRINGS.BUILDINGS.PREFABS.ALGAEHABITAT.NAME);
            //Database.Techs.TECH_GROUPING["FarmingTech"].Add("AlgaeHabitat2");
            Debug.Log(" === TestStart END === ");
        }
    }
    */

    /*
    [HarmonyPatch(typeof(STRINGS.BUILDINGS.PREFABS), "Sim200ms", new Type[] { typeof(float) })]
    internal class TestNewElement
    {
        private static bool Prefix(OxygenBreather __instance, float dt)
        {
            return false;
        }
    }
    /*


    /*
    [HarmonyPatch(typeof(OxygenBreather), "Sim200ms", new Type[] { typeof(float) })]
    internal class TestNewElement
    {
        private static bool Prefix(OxygenBreather __instance, float dt)
        {
            Debug.Log(" === OxygenBreather.Sim200ms loaded === ");

            PropertyInfo base_ = AccessTools.Property(typeof(OxygenBreather), "base");
            PropertyInfo airConsumptionRate = AccessTools.Property(typeof(OxygenBreather), "airConsumptionRate");
            PropertyInfo gasProvider = AccessTools.Property(typeof(OxygenBreather), "gasProvider");
            PropertyInfo co2Accumulator = AccessTools.Property(typeof(OxygenBreather), "co2Accumulator");
            PropertyInfo accumulatedCO2 = AccessTools.Property(typeof(OxygenBreather), "accumulatedCO2");
            PropertyInfo minCO2ToEmit = AccessTools.Property(typeof(OxygenBreather), "minCO2ToEmit");
            PropertyInfo facing = AccessTools.Property(typeof(OxygenBreather), "facing");
            PropertyInfo temperature = AccessTools.Property(typeof(OxygenBreather), "temperature");
            PropertyInfo hasAirTimer = AccessTools.Property(typeof(OxygenBreather), "hasAirTimer");
            PropertyInfo hasAir = AccessTools.Property(typeof(OxygenBreather), "hasAir");

            //airConsumptionRate.GetValue(__instance, null)
            MethodInfo airConsumptionRate_GetTotalValue = AccessTools.Method(typeof(AttributeInstance), "GetTotalValue");
            MethodInfo gasProvider_ConsumeGas = AccessTools.Method(typeof(AttributeInstance), "ConsumeGas");
            MethodInfo gasProvider_ShouldEmitCO2 = AccessTools.Method(typeof(AttributeInstance), "ShouldEmitCO2");
            MethodInfo hasAirTimer_Start = AccessTools.Method(typeof(Timer), "Start");
            MethodInfo hasAirTimer_TryStop = AccessTools.Method(typeof(Timer), "TryStop");
            MethodInfo hasAirTimer_Stop = AccessTools.Method(typeof(Timer), "Stop");

            Debug.Log(" === OxygenBreather.Sim200ms loaded === "+ ((KMonoBehaviour)base_.GetValue(__instance, null)).gameObject.GetProperName());

            if (!((KMonoBehaviour)base_.GetValue(__instance,null)).gameObject.HasTag(GameTags.Dead))
            {
                AttributeInstance airConsumptionRateO = (AttributeInstance) airConsumptionRate.GetValue(__instance, null);
                float num = ((float)airConsumptionRate_GetTotalValue.Invoke(airConsumptionRateO, null)) * dt;

                OxygenBreather.IGasProvider gasProviderO = (OxygenBreather.IGasProvider)gasProvider.GetValue(__instance, null);
                bool flag = (bool) gasProvider_ConsumeGas.Invoke(gasProviderO,new object[] { __instance, num});

                if (flag && (bool)gasProvider_ShouldEmitCO2.Invoke(gasProviderO,null))
                {
                    float num2 = num * __instance.O2toCO2conversion;
                    Game.Instance.accumulators.Accumulate((HandleVector<int>.Handle)co2Accumulator.GetValue(__instance,null), num2);

                    accumulatedCO2.SetValue(__instance, ((float)accumulatedCO2.GetValue(__instance, null)) + num2, null);

                    if ((float)accumulatedCO2.GetValue(__instance,null) >= (float)minCO2ToEmit.GetValue(__instance,null))
                    {
                        accumulatedCO2.SetValue(__instance, ((float)accumulatedCO2.GetValue(__instance, null)) - (float) minCO2ToEmit.GetValue(__instance,null), null);
                        Vector3 position = ((KMonoBehaviour)base_.GetValue(__instance, null)).transform.GetPosition();
                        position.x += ((!((Facing)facing.GetValue(__instance,null)).GetFacing()) ? __instance.mouthOffset.x : (0f - __instance.mouthOffset.x));
                        position.y += __instance.mouthOffset.y;
                        position.z -= 0.5f;
                        CO2Manager.instance.SpawnBreath(position, (float)minCO2ToEmit.GetValue(__instance,null), ((AmountInstance)temperature.GetValue(__instance,null)).value);
                    }
                }
                if (flag != (bool)hasAir.GetValue(__instance,null))
                {
                    hasAirTimer_Start.Invoke(hasAirTimer.GetValue(__instance, null),null);                    
                    if ((bool)hasAirTimer_TryStop.Invoke(hasAirTimer.GetValue(__instance, null), new object[] { 2f }))
                    {
                        hasAir.SetValue(__instance, flag, null);
                    }
                }
                else
                {
                    hasAirTimer_Stop.Invoke(hasAirTimer.GetValue(__instance, null), null);
                }
            }
            return false;
        }
    }
    */

    /*
    [HarmonyPatch(typeof(Database.Techs))]
    internal class TestNewElement
    {
        private static void Postfix()
        {
            Debug.Log(" === Database.Techs loaded === ");
            Database.Techs.TECH_GROUPING["FarmingTech"].Add("AlgaeHabitat2");
        }
    }

    [HarmonyPatch(typeof(Global), "Start", new Type[0])]
    internal class TestStart
    {

        private static void Postfix(Global __instance)
        {
            Debug.Log(" === TestStart === ");
            Database.Techs.TECH_GROUPING["FarmingTech"].Add("AlgaeHabitat2");
        }
    }
    */
    /*
    [HarmonyPatch(typeof(SpeedControlScreen), "OnChanged", new Type[0])]
    internal class TestSpeedButton
    {

        private static bool Prefix(SpeedControlScreen __instance)
        {
            Debug.Log(" === This game is MODDED. Do not report any issues to Hinterland! === ");
            if (__instance.IsPaused)
            {
                Time.timeScale = 0f;
            }
            else if (__instance.GetSpeed() == 0)
            {
                Time.timeScale = __instance.normalSpeed;
            }
            else if (__instance.GetSpeed() == 1)
            {
                Time.timeScale = __instance.fastSpeed;
            }
            else if (__instance.GetSpeed() == 2)
            {
                Time.timeScale = __instance.ultraSpeed;
            }
            if (__instance.OnGameSpeedChanged != null)
            {
                __instance.OnGameSpeedChanged();
            }

            return false;
        }
    }
    */
    /*
    [HarmonyPatch(typeof(GameManager), "ReadVersionFile", new Type[0])]
    internal class AddModdedToVersionString {

        private static void Postfix() {
            GameManager.m_GameVersionString = "Modded " + GameManager.m_GameVersionString;
            Debug.Log(" === This game is MODDED. Do not report any issues to Hinterland! === ");
        }
    }

    [HarmonyPatch(typeof(Panel_MainMenu), "SetVersionLabel", new Type[0])]
    internal class AddFailedModsToVersionLabel {

        private static void Postfix(Panel_MainMenu __instance) {
            if (!ModLoader.HasFailed)
                return;

            GameObject gameObject = __instance.m_VersionLabel;
            UILabel versionLabel = gameObject.GetComponent<UILabel>();
            versionLabel.color = Color.red;
            versionLabel.multiLine = true;
            versionLabel.text += "\n\n" + ModLoader.failureMessage;
            versionLabel.depth = int.MaxValue;
        }
    }

    [HarmonyPatch(typeof(BasicMenu), "InternalClickAction", new Type[] { typeof(int), typeof(bool) })]
    internal class DisableStartGameButtons {

        private static bool Prefix(int index) {
            // Skip if mod loading failed and button clicked was Story, Sandbox or Challenge
            return (!ModLoader.HasFailed) || (index > 2);
        }
    }
    */
}
