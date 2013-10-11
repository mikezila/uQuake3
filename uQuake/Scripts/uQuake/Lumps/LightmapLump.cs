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
        private int lmapCount = 0;

        public LightmapLump()
        {
        }

        public void AddLight(byte[] rgb)
        {
            Texture2D tex = new Texture2D(128, 128, TextureFormat.RGB24, false);
            List<Color> colors = new List<Color>();
            for (int i = 0; i < 49152; i += 3)
            {
                colors.Add(new Color32(rgb[i], rgb[i + 1], rgb[i + 2], (byte)0.5f));
                
            }
            tex.SetPixels(colors.ToArray());
            tex.Apply();
            lightmaps.Add(tex);
            lmapCount++;
            //Debug.Log("LmapCount: " + lmapCount.ToString());
        }
    }
}
