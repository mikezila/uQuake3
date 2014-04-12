using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SharpBSP
{
    public class LightmapLump
    {
        public Texture2D[] Lightmaps { get; set; }

        public LightmapLump(int lightmapCount)
        {
            Lightmaps = new Texture2D[lightmapCount];
        }

        private static byte CalcLight(byte color)
        {
            int icolor = (int)color;
            //icolor += 200;

            if (icolor > 255)
            {
                icolor = 255;
            }

            return (byte)icolor;
        }

        public static Texture2D CreateLightmap(byte[] rgb)
        {
            Texture2D tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            Color32[] colors = new Color32[128 * 128];
            int j = 0;
            for (int i = 0; i < 128 * 128; i++)
            {
                colors [i] = new Color32(CalcLight(rgb [j++]), CalcLight(rgb [j++]), CalcLight(rgb [j++]), (byte)1f);
            }
            tex.SetPixels32(colors);
            tex.Apply();
            return tex;
        }
    }
}
