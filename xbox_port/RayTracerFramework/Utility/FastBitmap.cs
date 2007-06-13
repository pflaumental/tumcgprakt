using System;
using System.Collections.Generic;
using System.Text;
using Color = Microsoft.Xna.Framework.Graphics.Color;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace RayTracerFramework.Utility {
    class FastBitmap {
        private Color[] color;
        private int width, height;

        public FastBitmap(Texture2D sourceTexture)
        {
            color = new Color[sourceTexture.Width * sourceTexture.Height];
            sourceTexture.GetData<Color>(color);
            width = sourceTexture.Width;
            height = sourceTexture.Height;            
        }

        // Bilinear interpolation 
        public RayTracerFramework.Shading.Color GetPixel(float x, float y) {
            int pixelX1 = (int)Math.Floor(x);
            int pixelX2 = (int)Math.Ceiling(x);
            float xLerp = x - (float)Math.Truncate(x);
            int pixelY1 = (int)Math.Floor(y);
            int pixelY2 = (int)Math.Ceiling(y);
            float yLerp = y - (float)Math.Truncate(y);

            RayTracerFramework.Shading.Color c11 = GetPixel(pixelX1, pixelY1);
            RayTracerFramework.Shading.Color c12 = GetPixel(pixelX1, pixelY2);
            RayTracerFramework.Shading.Color c21 = GetPixel(pixelX2, pixelY1);
            RayTracerFramework.Shading.Color c22 = GetPixel(pixelX2, pixelY2);
            return c11 * (1 - xLerp) * (1 - yLerp)
                    + c12 * (1 - xLerp) * yLerp
                    + c21 * xLerp * (1 - yLerp)
                    + c22 * xLerp * yLerp;
        }       
   
        public RayTracerFramework.Shading.Color GetPixel(int x, int y) {
            Color color = this.color[x + y * width];
            return new RayTracerFramework.Shading.Color(color.R, color.G, color.B);
        }

        public int Width {
            get { return width; }
        }
        public int Height {
            get { return height; }
        }
        
    
    }
    
    
}
