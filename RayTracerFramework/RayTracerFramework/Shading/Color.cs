using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RayTracerFramework.Shading {
    
    public class Color {
        private float red, green, blue;  // float color channel values in range [0,1]

        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Blue = new Color(0, 0, 255);
        public static readonly Color LightSlateGray = new Color(119, 136, 153);

        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(255, 255, 255);

        public Color() : this(0f, 0f, 0f) { }

        public Color(byte red, byte green, byte blue) {
            this.red = red / 255f;
            this.green = green / 255f;
            this.blue = blue / 255f;
        }

        public Color(float red, float green, float blue) {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        // Clamps the float color channels to [0, 1]
        public Color Saturate() {
            red = red < 0f ? 0 : (red > 1f ? 1 : red);
            green = green < 0f ? 0 : (green > 1f ? 1 : green);
            blue = blue < 0f ? 0 : (blue > 1f ? 1 : blue);
            return this;
        }

        public static Color operator *(Color c1, Color c2) {
            return new Color(c1.red * c2.red, c1.green * c2.green, c1.blue * c2.blue);
        }

        public static Color operator *(Color c, float factor) {
            return new Color(c.red * factor, c.green * factor, c.blue * factor);
        }

        public static Color operator *(float factor, Color c) {
            return new Color(c.red * factor, c.green * factor, c.blue * factor);
        }

        public static Color operator +(Color c1, Color c2) {
            return new Color(c1.red + c2.red, c1.green + c2.green, c1.blue + c2.blue);    
        }

        public static Color operator -(Color c1, Color c2) {
            return new Color(c1.red - c2.red, c1.green - c2.green, c1.blue - c2.blue);
        }

        [XmlIgnore()]
        public byte RedInt {
            get { return (byte)(red * 255); }
            set { red = value / 255f; }
        }

        [XmlIgnore()]
        public byte GreenInt {
            get { return (byte)(green * 255); }
            set { green = value / 255f; }
        }

        [XmlIgnore()]
        public byte BlueInt {
            get { return (byte)(blue * 255); }
            set { blue = value / 255f; }
        }

        [XmlElement("Red")]
        public float RedFloat {
            get { return red; }
            set { red = value; }
        }

        [XmlElement("Green")]        
        public float GreenFloat {
            get { return green; }
            set { green = value; }
        }

        [XmlElement("Blue")]
        public float BlueFloat {
            get { return blue; }
            set { blue = value; }
        }

        public override string ToString() {
            return "(" + RedInt + " / " + GreenInt + " / " + BlueInt + ")";
        }
    }


}
