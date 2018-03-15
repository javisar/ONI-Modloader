using UnityEngine;

namespace ONI_Common.Data
{
    public struct SerializeableVector2
    {
        public SerializeableVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float x;
        public float y;

        public Vector2 ToVector2()
        {
            return new Vector2(this.x, this.y);
        }
    }
}
