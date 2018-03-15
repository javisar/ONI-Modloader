using UnityEngine;

namespace MaterialColor.Extensions
{
    public static class Color32Extensions
    {
        public static Color32 ToColor32(this int hexVal)
        {
            byte r = (byte)((hexVal >> 16) & 0xFF);
            byte g = (byte)((hexVal >> 8) & 0xFF);
            byte b = (byte)(hexVal & 0xFF);

            return new Color32(r, g, b, 0xFF);
        }

        public static int ToHex(this Color32 color)
        {
            return color.r << 16 | color.g << 8 | color.b;
        }




    }
}
