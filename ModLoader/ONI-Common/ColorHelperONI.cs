using UnityEngine;

namespace MaterialColor.Helpers
{
    public static class ColorHelperONI
    {



        public static readonly Color32 MissingDebugColor =
            new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

        public static readonly Color32 NoOffset =
            new Color32(0, 0, 0, byte.MaxValue);


        public static bool TryGetTypeStandardColor(string typeName, out Color32 standardColor)
        {
            Color32 typeStandardColor;
            if (State.TypeColorOffsets.TryGetValue(typeName, out typeStandardColor))
            {
                standardColor = typeStandardColor;
                return true;
            }

            standardColor = State.ConfiguratorState.ShowMissingTypeColorOffsets
                ? MissingDebugColor
                : NoOffset;

            return false;
        }

        public static Color DefaultCellColor
            => new Color(1, 1, 1);

        public static Color InvalidCellColor
            => new Color(1, 0, 0);
    }
}
