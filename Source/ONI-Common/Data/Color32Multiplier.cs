namespace ONI_Common.Data
{
    public class Color32Multiplier
    {
        public static readonly Color32Multiplier One = new Color32Multiplier(1);

        [Newtonsoft.Json.JsonConstructor]
        public Color32Multiplier(float red, float green, float blue)
        {
            this.Red   = red;
            this.Green = green;
            this.Blue  = blue;
        }

        public Color32Multiplier(float all)
        {
            this.Red = this.Green = this.Blue = all;
        }

        public float Blue { get; set; }

        public float Green { get; set; }

        public float Red { get; set; }
    }
}