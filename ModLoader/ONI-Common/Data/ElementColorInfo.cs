namespace ONI_Common.Data
{
    public class ElementColorInfo
    {
        public ElementColorInfo(Color32Multiplier multiplier, float brightness = 1)
        {
            this.ColorMultiplier = multiplier;
            this.Brightness = brightness;
        }

        public Color32Multiplier ColorMultiplier { get; set; }
        public float Brightness { get; set; }
    }
}
