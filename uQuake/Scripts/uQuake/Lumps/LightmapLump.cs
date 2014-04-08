using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SharpBSP
{
    public class LightmapLump
    {
        public List<Texture2D> lightmaps = new List<Texture2D>();

        public LightmapLump()
        {
        }

        byte CalcLight(byte color)
        {
            int icolor = (int)color;
            //icolor += 200;

            if (icolor > 255)
            {
                icolor = 255;
            }

            return (byte)icolor;
        }

        public void AddLight(byte[] rgb)
        {
            Texture2D tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            List<Color> colors = new List<Color>();
            for (int i = 0; i < 49152; i += 3)
            {
                colors.Add(new Color32(CalcLight(rgb [i]), CalcLight(rgb [i + 1]), CalcLight(rgb [i + 2]), (byte)1f));
            }
            tex.SetPixels(colors.ToArray());
            tex.Apply();
            lightmaps.Add(tex);
        }
    }
}
