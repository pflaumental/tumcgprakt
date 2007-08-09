using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Color = RayTracerFramework.Shading.Color;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Shading {
    public class CubeMap {
        // Related assignement: 4.2.b

        public float xMin, xMax, yMin, yMax, zMin, zMax;

        public FastBitmap xMinTexture, xMaxTexture;
        public FastBitmap yMinTexture, yMaxTexture;
        public FastBitmap zMinTexture, zMaxTexture;

        public string cubeMapFilename;

        public CubeMap(float width, float height, float depth, string texturesBaseName) {
            cubeMapFilename = texturesBaseName;

            this.xMin = -(width * 0.5f);
            this.xMax = width * 0.5f;
            this.yMin = -(height * 0.5f);
            this.yMax = height * 0.5f;
            this.zMin = -(depth * 0.5f);
            this.zMax = depth * 0.5f;

            xMinTexture = new FastBitmap(new Bitmap(Image.FromFile(Settings.Setup.Loading.DefaultStandardTextureDirectory + Settings.Setup.Loading.DefaultCubeMapPrefix + texturesBaseName + "NX.jpg")));
            xMaxTexture = new FastBitmap(new Bitmap(Image.FromFile(Settings.Setup.Loading.DefaultStandardTextureDirectory + Settings.Setup.Loading.DefaultCubeMapPrefix + texturesBaseName + "PX.jpg")));

            yMinTexture = new FastBitmap(new Bitmap(Image.FromFile(Settings.Setup.Loading.DefaultStandardTextureDirectory + Settings.Setup.Loading.DefaultCubeMapPrefix + texturesBaseName + "NY.jpg")));
            yMaxTexture = new FastBitmap(new Bitmap(Image.FromFile(Settings.Setup.Loading.DefaultStandardTextureDirectory + Settings.Setup.Loading.DefaultCubeMapPrefix + texturesBaseName + "PY.jpg")));

            zMinTexture = new FastBitmap(new Bitmap(Image.FromFile(Settings.Setup.Loading.DefaultStandardTextureDirectory + Settings.Setup.Loading.DefaultCubeMapPrefix + texturesBaseName + "NZ.jpg")));
            zMaxTexture = new FastBitmap(new Bitmap(Image.FromFile(Settings.Setup.Loading.DefaultStandardTextureDirectory + Settings.Setup.Loading.DefaultCubeMapPrefix + texturesBaseName + "PZ.jpg")));
        }

        public float Width() { return xMax - xMin; }

        public float Height() { return yMax - yMin; }

        public float Depth() { return zMax - zMin; }

  
        public Color getColor(Ray ray) {
            float t;
 
            Vec3 posWS = ray.position;  // Position where the ray starts in world space
            Vec3 dirWS = ray.direction; // Direction of the ray in world space

            // Test if ray intersects right plane
            if (dirWS.x > 0) {
                t = (xMax - posWS.x) / dirWS.x;
                Vec3 p = posWS + dirWS * t;
                if (p.y <= yMax && p.y >= yMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (-p.z + zMax) / (zMax - zMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (xMaxTexture.Width - 1));
                    float pixelY = (yTex * (xMaxTexture.Height - 1));

                    return xMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            else if (dirWS.x < 0) {
                t = (xMin - posWS.x) / dirWS.x;
                Vec3 p = posWS + dirWS * t;
                if (p.y <= yMax && p.y >= yMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.z + zMax) / (zMax - zMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (xMinTexture.Width - 1));
                    float pixelY = (yTex * (xMinTexture.Height - 1));

                    //int pixelX = (int)(xTex * (xMinTexture.Width-1));
                    //int pixelY = (int)(yTex * (xMinTexture.Height-1));
                    return xMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            if (dirWS.y > 0) {
                t = (yMax - posWS.y) / dirWS.y;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (p.z + zMax) / (zMax - zMin);

                    float pixelX = (xTex * (yMaxTexture.Width - 1));
                    float pixelY = (yTex * (yMaxTexture.Height - 1));

                    return yMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 

            else if (dirWS.y < 0) {
                t = (yMin - posWS.y) / dirWS.y;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.z + zMax) / (zMax - zMin);

                    float pixelX = (xTex * (yMinTexture.Width - 1));
                    float pixelY = (yTex * (yMinTexture.Height - 1));

                    return yMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            if (dirWS.z > 0) {
                t = (zMax - posWS.z) / dirWS.z;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = xTex * (zMaxTexture.Width - 1);
                    float pixelY = yTex * (zMaxTexture.Height - 1);                    

                    return zMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            else if (dirWS.z < 0) {
                t = (zMin - posWS.z) / dirWS.z;
                Vec3 p = posWS + dirWS * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    float xTex = (-p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (zMinTexture.Width - 1));
                    float pixelY = (yTex * (zMinTexture.Height - 1));                    

                    return zMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            throw new Exception("No valid direction for the ray specified.");
           
        }


    }
}
