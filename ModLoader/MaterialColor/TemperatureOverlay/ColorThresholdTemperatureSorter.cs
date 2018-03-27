namespace MaterialColor.TemperatureOverlay
{
    using System.Collections.Generic;

    public class ColorThresholdTemperatureSorter : IComparer<SimDebugView.ColorThreshold>
    {
        public int Compare(SimDebugView.ColorThreshold x, SimDebugView.ColorThreshold y)
        {
            return x.value.CompareTo(y.value);
        }
    }
}