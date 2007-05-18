using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracerFramework.Shading {
    
    class Color {
        private byte red, green, blue;      // integer color channel values in range [0, 255] 
        private float redF, greenF, blueF; // float color channel values in range [0,1]

        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Blue = new Color(0, 0, 255);
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(255, 255, 255);

        public Color() : this(1f, 1f, 1f) { }

        public Color(byte red, byte green, byte blue) {
            redF = red / 255f;
            greenF = green / 255f;
            blueF = blue / 255f;
        }

        public Color(float red, float green, float blue) {
            this.redF = red;
            this.greenF = green;
            this.blueF = blue;
        }

        // Clamps the float color channels to [0, 1]
        public void Saturate() {
            redF = redF < 0f ? 0 : (redF > 1f ? 1 : redF);
            greenF = greenF < 0f ? 0 : (greenF > 1f ? 1 : greenF);
            blueF = blueF < 0f ? 0 : (blueF > 1f ? 1 : blueF); 
        }

        public static Color operator *(Color c1, Color c2) {
            return new Color(c1.redF * c2.redF, c1.greenF * c2.greenF, c1.blueF * c2.blueF);
        }

        public static Color operator *(Color c, float factor) {
            return new Color(c.redF * factor, c.greenF * factor, c.blueF * factor);
        }

        public static Color operator *(float factor, Color c) {
            return new Color(c.redF * factor, c.greenF * factor, c.blueF * factor);
        }

        public byte RedInt {
            get { return (byte)(redF * 255); }
            set { redF = value / 255f; }
        }

        public byte GreenInt {
            get { return (byte)(greenF * 255); }
            set { greenF = value / 255f; }
        }

        public byte BlueInt {
            get { return (byte)(blueF * 255); }
            set { blueF = value / 255f; }
        }

        public float RedFloat {
            get { return redF; }
            set { redF = value; }
        }

        public float GreenFloat {
            get { return greenF; }
            set { greenF = value; }
        }

        public float BlueFloat {
            get { return blueF; }
            set { blueF = value; }
        }
    }


}
