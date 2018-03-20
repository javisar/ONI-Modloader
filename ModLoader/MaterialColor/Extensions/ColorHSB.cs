/*
 * Created by C.J. Kimberlin (http://cjkimberlin.com)
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2015
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 *
 *
 * ============= Description =============
 *
 * An ColorHSB struct for interpreting a color in hue/saturation/value instead of red/green/blue.
 * NOTE! hue will be a value from 0 to 1 instead of 0 to 360.
 *
 * ColorHSB hsvRed = new ColorHSB(1, 1, 1, 1); // RED
 * ColorHSB hsvGreen = new ColorHSB(0.333f, 1, 1, 1); // GREEN
 *
 *
 * Also supports implicit conversion between Color and Color32.
 *
 * ColorHSB hsvBlue = Color.blue; // HSVA(0.667f, 1, 1, 1)
 * Color blue = hsvBlue; // RGBA(0, 0, 1, 1)
 * Color32 blue32 = hsvBlue; // RGBA(0, 0, 255, 255)
 *
 *
 * If functions are desired instead of implicit conversion then use the following.
 *
 * Color yellowBefore = Color.yellow; // RBGA(1, .922f, 0.016f, 1)
 * ColorHSB hsvYellow = Color.yellowBefore.ToHSV(); // HSVA(0.153f, 0.984f, 1, 1)
 * Color yellowAfter = hsvYellow.ToRGB(); // RBGA(1, .922f, 0.016f, 1)
 * */

// ReSharper disable All
namespace MaterialColor.Extensions
{
    using UnityEngine;

    public struct ColorHSB
    {
        public float H;

        public float S;

        public float B;

        public float A;

        public ColorHSB(float h, float s, float b, float a)
        {
            this.H = h;
            this.S = s;
            this.B = b;
            this.A = a;
        }

        public override string ToString()
        {
            return string.Format("HSVA: ({0:F3}, {1:F3}, {2:F3}, {3:F3})", this.H, this.S, this.B, this.A);
        }

        public static bool operator ==(ColorHSB lhs, ColorHSB rhs)
        {
            if (lhs.A != rhs.A)
            {
                return false;
            }

            if (lhs.B == 0 && rhs.B == 0)
            {
                return true;
            }

            if (lhs.S == 0 && rhs.S == 0)
            {
                return lhs.B == rhs.B;
            }

            return lhs.H == rhs.H && lhs.S == rhs.S && lhs.B == rhs.B;
        }

        public static implicit operator ColorHSB(Color c)
        {
            return c.ToHSV();
        }

        public static implicit operator Color(ColorHSB hsb)
        {
            return hsb.ToRgb();
        }

        public static implicit operator ColorHSB(Color32 c32)
        {
            return ((Color)c32).ToHSV();
        }

        public static implicit operator Color32(ColorHSB hsb)
        {
            return hsb.ToRgb();
        }

        public static bool operator !=(ColorHSB lhs, ColorHSB rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            if (other is ColorHSB || other is Color || other is Color32)
            {
                return this == (ColorHSB)other;
            }

            return false;
        }

        public override int GetHashCode()
        {
            // This is maybe not a good implementation :)
            return ((Color)this).GetHashCode();
        }

        public Color ToRgb()
        {
            Vector3 rgb = HuEtoRgb(this.H);
            Vector3 vc  = ((rgb - Vector3.one) * this.S + Vector3.one) * this.B;

            return new Color(vc.x, vc.y, vc.z, this.A);
        }

        private static Vector3 HuEtoRgb(float h)
        {
            float r = Mathf.Abs(h * 6 - 3) - 1;
            float g = 2                    - Mathf.Abs(h * 6 - 2);
            float b = 2                    - Mathf.Abs(h * 6 - 4);

            return new Vector3(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b));
        }
    }

    public static class ColorExtension
    {
        private const float Epsilon = 1e-10f;

        public static ColorHSB ToHSV(this Color rgb)
        {
            Vector3 hcv = RgBtoHcv(rgb);
            float   s   = hcv.y / (hcv.z + Epsilon);

            return new ColorHSB(hcv.x, s, hcv.z, rgb.a);
        }

        private static Vector3 RgBtoHcv(Color rgb)
        {
            Vector4 p = rgb.g < rgb.b
                        ? new Vector4(rgb.b, rgb.g, -1, 2f  / 3f)
                        : new Vector4(rgb.g, rgb.b, 0,  -1f / 3f);
            Vector4 q = rgb.r < p.x ? new Vector4(p.x, p.y, p.w, rgb.r) : new Vector4(rgb.r, p.y, p.z, p.x);
            float   c = q.x - Mathf.Min(q.w, q.y);
            float   h = Mathf.Abs((q.w - q.y) / (6 * c + Epsilon) + q.z);

            return new Vector3(h, c, q.x);
        }
    }
}