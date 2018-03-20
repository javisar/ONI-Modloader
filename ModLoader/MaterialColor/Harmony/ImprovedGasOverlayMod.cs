namespace MaterialColor
{
    using Harmony;

    using MaterialColor.Extensions;
    using MaterialColor.Helpers;

    using UnityEngine;

    internal static partial class HarmonyPatches
    {
        [HarmonyPatch(typeof(SimDebugView), "GetOxygenMapColour")]
        public static class ImprovedGasOverlayMod
        {
            public static bool Prefix(int cell, ref Color __result)
            {
                float minMass = ONI_Common.State.ConfiguratorState.GasPressureStart;
                float maxMass = ONI_Common.State.ConfiguratorState.GasPressureEnd;

                Element element = Grid.Element[cell];

                if (!element.IsGas)
                {
                    __result = NotGasColor;
                    return false;
                }

                Color gasColor = ColorHelper.GetCellOverlayColor(cell);

                float gasMass = Grid.Mass[cell];

                gasMass -= minMass;

                if (gasMass < 0)
                {
                    gasMass = 0;
                }

                maxMass -= minMass;

                if (maxMass < float.Epsilon)
                {
                    maxMass = float.Epsilon;
                }

                float    intensity;
                ColorHSB gasColorHSB = gasColor;
                float    mass        = Grid.Mass[cell];
                if (element.id == SimHashes.Oxygen || element.id == SimHashes.ContaminatedOxygen)
                {
                    float optimallyBreathable = SimDebugView.optimallyBreathable;
                    intensity = Mathf.Clamp((mass - SimDebugView.minimumBreathable) / optimallyBreathable, 0.05f, 1f);

                    // To red for thin air
                    if (intensity < 1f)
                    {
                        gasColorHSB.B = Mathf.Min(gasColorHSB.B + 1f - intensity, 0.9f);
                    }
                }
                else
                {
                    intensity = GetGasColorIntensity(gasMass, maxMass);
                }

                // Pop ear drum marker
                if (mass > 2.5f)
                {
                    gasColorHSB.H += 0.02f * Mathf.InverseLerp(2.5f, 3.5f, mass);
                    if (gasColorHSB.H > 1f)
                    {
                        gasColorHSB.H -= 1f;
                    }

                    float intens = Mathf.InverseLerp(20f, 3.5f, mass);

                    gasColorHSB.B = Mathf.Max(0.5f, gasColorHSB.B * intens);
                }

                // New code, use the saturation of a color for the pressure
                gasColorHSB.S = intensity * 0.7f;
                __result      = gasColorHSB;

                return false;

                // gasColor *= intensity;
                // gasColor.a = 1;
                // __result = gasColor;
            }

            private static float GetGasColorIntensity(float mass, float maxMass)
            {
                float minIntensity = ONI_Common.State.ConfiguratorState.MinimumGasColorIntensity;

                float intensity = mass / maxMass;

                intensity = Mathf.Sqrt(intensity);

                intensity =  Mathf.Clamp01(intensity);
                intensity *= 1 - minIntensity;
                intensity += minIntensity;

                return intensity;
            }
        }
    }
}