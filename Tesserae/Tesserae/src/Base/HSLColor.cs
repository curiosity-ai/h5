using System;

namespace Tesserae
{
    public class HSLColor
    {
        // Private data members below are on scale 0-1, they are scaled for use externally based on scale
        private double _hue = 1.0;
        private double _saturation = 1.0;
        private double _luminosity = 1.0;
        private const double _scale = 240.0;

        public double Hue
        {
            get { return _hue * _scale; }
            set { _hue = CheckRange(value / _scale); }
        }
        public double Saturation
        {
            get { return _saturation * _scale; }
            set { _saturation = CheckRange(value / _scale); }
        }
        public double Luminosity
        {
            get { return _luminosity * _scale; }
            set { _luminosity = CheckRange(value / _scale); }
        }

        private static double CheckRange(double value)
        {
            if (value < 0.0) value = 0.0;
            else if (value > 1.0) value = 1.0;
            return value;
        }

        public override string ToString()
        {
            return string.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
        }

        public string ToRGB()
        {
            return ((Color)this).ToRGB();
        }
        public string ToRGBA(float opacity)
        {
            return ((Color)this).ToRGBA(opacity);
        }

        public string ToHex()
        {
            var c = (Color)this;
            return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
        }

        private static Random _rng = new Random();

        public static HSLColor Random()
        {
            return new HSLColor(_rng.NextDouble() * _scale, _rng.NextDouble() * _scale, 0.5 * _scale);
        }

        public static implicit operator Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor._luminosity != 0)
            {
                if (hslColor._saturation == 0)
                    r = g = b = hslColor._luminosity;
                else
                {
                    double temp2 = GetTemp2(hslColor);
                    double temp1 = 2.0 * hslColor._luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor._hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, hslColor._hue);
                    b = GetColorComponent(temp1, temp2, hslColor._hue - 1.0 / 3.0);
                }
            }
            return Color.FromArgb((byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0 / 6.0) return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5) return temp2;
            else if (temp3 < 2.0 / 3.0) return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else return temp1;
        }
        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0) temp3 += 1.0;
            else if (temp3 > 1.0) temp3 -= 1.0;
            return temp3;
        }
        private static double GetTemp2(HSLColor hslColor)
        {
            double temp2;
            if (hslColor._luminosity < 0.5)  temp2 = hslColor._luminosity * (1.0 + hslColor._saturation);
            else temp2 = hslColor._luminosity + hslColor._saturation - (hslColor._luminosity * hslColor._saturation);
            return temp2;
        }

        public static implicit operator HSLColor(Color color)
        {
            HSLColor hslColor = new HSLColor();
            hslColor._hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360
            hslColor._luminosity = color.GetBrightness();
            hslColor._saturation = color.GetSaturation();
            return hslColor;
        }

        public void SetRGB(byte red, byte green, byte blue)
        {
            HSLColor hslColor = (HSLColor)Color.FromArgb(red, green, blue);
            _hue = hslColor._hue;
            _saturation = hslColor._saturation;
            _luminosity = hslColor._luminosity;
        }

        public HSLColor() { }
        public HSLColor(Color color)
        {
            SetRGB(color.R, color.G, color.B);
        }
        public HSLColor(byte red, byte green, byte blue)
        {
            SetRGB(red, green, blue);
        }
        public HSLColor(double hue, double saturation, double luminosity)
        {
            Hue = hue;
            Saturation = saturation;
            Luminosity = luminosity;
        }
    }
}
