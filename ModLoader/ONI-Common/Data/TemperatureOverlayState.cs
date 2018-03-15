using System.Collections.Generic;

namespace ONI_Common.Data
{
    public class TemperatureOverlayState
    {
        public bool CustomRangesEnabled { get; set; } = true;

        public List<float> Temperatures => new List<float>
        {
        this.Aqua,
        this.Turquoise,
        this.Blue,
        this.Green,
        this.Lime,
        this.Orange,
        this.RedOrange,
        this.Red
        };

        public float Red { get; set; } = 1800;
        public float RedOrange { get; set; } = 0.1f;
        public float Orange { get; set; } = 323;
        public float Lime { get; set; } = 293;
        public float Green { get; set; } = 273;
        public float Blue { get; set; } = 0.2f;
        public float Turquoise { get; set; } = 0.1f;
        public float Aqua { get; set; } = 0;

        public bool LogThresholds { get; set; } = false;
    }
}
