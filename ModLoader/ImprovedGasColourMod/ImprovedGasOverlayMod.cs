using Harmony;
using MaterialColor.Extensions;
using UnityEngine;

namespace ImprovedGasColourMod
{
    public static class HarmonyPatches
    {
        private static readonly Color NotGasColor = new Color(0.6f, 0.6f, 0.6f);

        [HarmonyPatch(typeof(SimDebugView), "GetOxygenMapColour")]
        public static class ImprovedGasOverlayMod
        {
            public const float EarPopFloat = 2.5f;

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

                Color gasColor = GetCellOverlayColor(cell);

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

                float intensity;
                ColorHSV gasColorHSV = gasColor;
                float mass = Grid.Mass[cell];
                if (element.id == SimHashes.Oxygen || element.id == SimHashes.ContaminatedOxygen)
                {
                    float optimallyBreathable = SimDebugView.optimallyBreathable;
                    float minimumBreathable = SimDebugView.minimumBreathable;
                    intensity = Mathf.Max(0.05f, Mathf.InverseLerp(minimumBreathable, optimallyBreathable, mass));

                    // // To red for thin air
                    // if (intensity < 1f)
                    // {
                    //     gasColorHSV.V = Mathf.Min(gasColorHSV.V + 1f - intensity, 0.9f);
                    // }
                }
                else
                {
                    intensity = GetGasColorIntensity(gasMass, maxMass);
                    intensity = Mathf.Max(intensity, 0.15f);

                }

                // Pop ear drum marker
                if (mass > EarPopFloat)
                {
                    gasColorHSV.H += 0.02f * Mathf.InverseLerp(EarPopFloat, 3.5f, mass);
                    if (gasColorHSV.H > 1f)
                    {
                        gasColorHSV.H -= 1f;
                    }

                    float intens = Mathf.InverseLerp(EarPopFloat, 20f, mass);

                    float modifier = 1f - intens / 2;

                    gasColorHSV.V *= modifier;

                }

                // New code, use the saturation of a color for the pressure
                gasColorHSV.S *= intensity;
                __result = gasColorHSV;

                return false;

                // gasColor *= intensity;
                // gasColor.a = 1;
                // __result = gasColor;
            }

            public static Color GetCellOverlayColor(int cellIndex)
            {
                Element element = Grid.Element[cellIndex];
                Substance substance = element.substance;

                Color32 overlayColor = substance.overlayColour;

                overlayColor.a = byte.MaxValue;

                return overlayColor;
            }

            private static float GetGasColorIntensity(float mass, float maxMass)
            {
                float minIntensity = ONI_Common.State.ConfiguratorState.MinimumGasColorIntensity;

                float intensity = mass / maxMass;

                intensity = Mathf.Sqrt(intensity);

                intensity = Mathf.Clamp01(intensity);
                intensity *= 1 - minIntensity;
                intensity += minIntensity;

                return intensity;
            }
        }
    }
}