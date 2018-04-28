using Harmony;

namespace OxygenBreatherMod
{
    [HarmonyPatch(typeof(OxygenBreather), "Sim200ms", new[] { typeof(float) })]
    internal static class NoOxygenConsumptionMod
    {
        private static bool Prefix(OxygenBreather __instance, ref float dt)
        {
            dt = 0;
            return true;
        }
    }

    /*
    [HarmonyPatch(typeof(OxygenBreather), "OnSpawn", new Type[0] )]
    internal class NoOxygenConsumptionMod
    {
        private static readonly FieldInfo eggProgressField = AccessTools.Field(typeof(OxygenBreather), "airConsumptionRate");

        private static bool Prefix(OxygenBreather __instance)
        {
            // Disable all code in OxygenBreather.Sim200ms
            //return false;
            AttributeInstance att = (AttributeInstance)eggProgressField.GetValue(__instance);
            Klei.AI.Attribute nat =new Klei.AI.Attribute("AirConsumptionRate", false, Klei.AI.Attribute.Display.Normal, false, -100f);

            eggProgressField.SetValue(__instance, nat);

            //eggProgressField.SetValue(__instance, 0);
            //att.ClearModifiers();
            //att.Attribute.BaseValue = 0f;
            return true;
        }
    }
    */
    /*
    [HarmonyPatch(typeof(OxygenBreather), "Sim200ms", new Type[] { typeof(float) })]
    internal class NoOxygenConsumptionMod
    {
        private static readonly FieldInfo eggProgressField = AccessTools.Field(typeof(OxygenBreather), "airConsumptionRate");

        private static bool Prefix(OxygenBreather __instance, float dt)
        {
            // Disable all code in OxygenBreather.Sim200ms
            //return false;

            AttributeInstance att =  (AttributeInstance)eggProgressField.GetValue(__instance);
            Debug.Log(" === OxygenBreather.Sim200ms === " + att.GetTotalValue());
            Debug.Log(" === OxygenBreather.Sim200ms === " + att.GetBaseValue());
            //eggProgressField.SetValue(__instance, 0);
            att.ClearModifiers();
            att.Attribute.BaseValue = 0f;
            return true;
        }
    }
    */
    /*
    [HarmonyPatch(typeof(OxygenBreather), "Sim200ms", new Type[] { typeof(float) })]
    internal class NoOxygenConsumptionMod
    {
        private static bool Prefix(OxygenBreather __instance, float dt)
        {
            // Disable all code in OxygenBreather.Sim200ms
            return false;
        }
    }

    [HarmonyPatch(typeof(BreathMonitor.Instance), "GetBreath", new Type[0])]
    internal class BreathMonitorMod
    {
        private static bool Prefix(BreathMonitor.Instance __instance, ref float __result)
        {
            // Disable all code in OxygenBreather.Sim200ms
            __result = 1f;
            return false;
        }
    }

    [HarmonyPatch(typeof(BreathMonitor.Instance), "IsFullBreath", new Type[0] )]
    internal class IsFullBreathMod
    {
        private static bool Prefix(BreathMonitor.Instance __instance, ref bool __result)
        {
            // Disable all code in OxygenBreather.Sim200ms
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(BreathMonitor.Instance), "IsInBreathableArea", new Type[0])]
    internal class IsInBreathableAreaMod
    {
        private static bool Prefix(BreathMonitor.Instance __instance, ref bool __result)
        {
            // Disable all code in OxygenBreather.Sim200ms
            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(BreathMonitor.Instance), "IsLowBreath", new Type[0])]
    internal class IsLowBreathMod
    {
        private static bool Prefix(BreathMonitor.Instance __instance, ref bool __result)
        {
            // Disable all code in OxygenBreather.Sim200ms
            __result = false;
            return false;
        }
    }
    */

    /*
    [HarmonyPatch(typeof(OxygenBreather))]
    [HarmonyPatch("IsSuffocating", PropertyMethod.Getter)]
    public static class NoSuffocatingMod
    {
        [HarmonyPostfix]
        public static void IsSuffocating(OxygenBreather __instance, ref bool __result)
        {
            __result = false;
        }
    }
    */
    /*
    [HarmonyPatch(typeof(OxygenBreather), "IsSuffocating", new Type[] { typeof(bool) })]
    public static class NoSuffocatingMod
    {
        private static void Postfix(OxygenBreather __instance, ref bool __result)
        {
            __result = false;
        }
    }
    */
    /*
    [HarmonyPatch(typeof(OxygenBreather), "Sim200ms", new Type[] { typeof(float) })]
    internal class NoOxygenBreatherMod
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
}