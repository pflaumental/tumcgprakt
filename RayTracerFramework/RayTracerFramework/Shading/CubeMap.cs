using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Color = RayTracerFramework.Shading.Color;
using RayTracerFramework.Geometry;
using RayTracerFramework.Utility;

namespace RayTracerFramework.Shading {
    class CubeMap {
        public float xMin, xMax, yMin, yMax, zMin, zMax;

        private FastBitmap xMinTexture, xMaxTexture;
        private FastBitmap yMinTexture, yMaxTexture;
        private FastBitmap zMinTexture, zMaxTexture;

        public CubeMap(float width, float height, float depth, string texturesBaseName) {
            this.xMin = -(width * 0.5f);
            this.xMax = width * 0.5f;
            this.yMin = -(height * 0.5f);
            this.yMax = height * 0.5f;
            this.zMin = -(depth * 0.5f);
            this.zMax = depth * 0.5f;

            xMinTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/" + texturesBaseName + "NX.png")));
            xMaxTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/" + texturesBaseName + "PX.png")));

            yMinTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/" + texturesBaseName + "NY.png")));
            yMaxTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/" + texturesBaseName + "PY.png")));

            zMinTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/" + texturesBaseName + "NZ.png")));
            zMaxTexture = new FastBitmap(new Bitmap(Image.FromFile("../../Textures/" + texturesBaseName + "PZ.png")));
        }

  
        public Color getColor(Ray ray) {
            float t;
 
            Vec3 posOS = ray.position;
            Vec3 dirOS = ray.direction;

            // Test if ray intersects right plane
            if (dirOS.x > 0) {
                t = (xMax - posOS.x) / dirOS.x;
                Vec3 p = posOS + dirOS * t;
                if (p.y <= yMax && p.y >= yMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (-p.z + zMax) / (zMax - zMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (xMaxTexture.Width - 1));
                    float pixelY = (yTex * (xMaxTexture.Height - 1));

                    //int pixelX = (int)(xTex * (xMaxTexture.Width-1));
                    //int pixelY = (int)(yTex * (xMaxTexture.Height-1));
                    return xMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            // Left Plane DONE
            else if (dirOS.x < 0) {
                t = (xMin - posOS.x) / dirOS.x;
                Vec3 p = posOS + dirOS * t;
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

            // Upper plane todo
            if (dirOS.y > 0) {
                t = (yMax - posOS.y) / dirOS.y;
                Vec3 p = posOS + dirOS * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (p.z + zMax) / (zMax - zMin);

                    float pixelX = (xTex * (yMaxTexture.Width - 1));
                    float pixelY = (yTex * (yMaxTexture.Height - 1));

                    //int pixelX = (int)(xTex * (yMaxTexture.Width-1));
                    //int pixelY = (int)(yTex * (yMaxTexture.Height - 1));

                    return yMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            // lower plane todo
            else if (dirOS.y < 0) {
                t = (yMin - posOS.y) / dirOS.y;
                Vec3 p = posOS + dirOS * t;
                if (p.x <= xMax && p.x >= xMin && p.z >= zMin && p.z <= zMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.z + zMax) / (zMax - zMin);

                    float pixelX = (xTex * (yMinTexture.Width - 1));
                    float pixelY = (yTex * (yMinTexture.Height - 1));

                    //int pixelX = (int)(xTex * (yMinTexture.Width - 1));
                    //int pixelY = (int)(yTex * (yMinTexture.Height - 1));

                    return yMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            // Back plane DONE
            if (dirOS.z > 0) {
                t = (zMax - posOS.z) / dirOS.z;
                Vec3 p = posOS + dirOS * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    float xTex = (p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = xTex * (zMaxTexture.Width - 1);
                    float pixelY = yTex * (zMaxTexture.Height - 1);                    

                    //int pixelX = (int) xTex * (zMaxTexture.Width - 1);
                    //int pixelY = (int) yTex * (zMaxTexture.Height - 1);

                    return zMaxTexture.GetPixel(pixelX, pixelY);
                }
            } 
            // Front Plane DONE
            else if (dirOS.z < 0) {
                t = (zMin - posOS.z) / dirOS.z;
                Vec3 p = posOS + dirOS * t;
                if (p.x <= xMax && p.x >= xMin && p.y >= yMin && p.y <= yMax) {
                    float xTex = (-p.x + xMax) / (xMax - xMin);
                    float yTex = (-p.y + yMax) / (yMax - yMin);

                    float pixelX = (xTex * (zMinTexture.Width - 1));
                    float pixelY = (yTex * (zMinTexture.Height - 1));                    

                    //int pixelX = (int)(xTex * (zMinTexture.Width-1));
                    //int pixelY = (int)(yTex * (zMinTexture.Height-1));

                    return zMinTexture.GetPixel(pixelX, pixelY);
                }
            }

            throw new Exception("No valid direction for the ray specified.");
           
        }


    }
}
