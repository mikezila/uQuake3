// This was made by aaro4130 on the Unity forums.  Thanks boss!
// It's been optimized and slimmed down for the purpose of loading Quake 3 TGA textures from memory streams.

using System;
using System.IO;
using UnityEngine;

public static class TGALoader
{

    public static Texture2D LoadTGA(string fileName)
    {
        using (var imageFile = File.OpenRead(fileName))
        {
            return LoadTGA(imageFile);
        }
    }

    public static Texture2D LoadTGA(Stream TGAStream)
    {
   
        using (BinaryReader r = new BinaryReader(TGAStream, System.Text.Encoding.Unicode))
        {
            // Skip some header info we don't care about.
            // Even if we did care, we have to move the stream seek point to the beginning,
            // as the previous method in the workflow left it at the end.
            r.BaseStream.Seek(12, SeekOrigin.Begin);

            Int16 width = r.ReadInt16();
            Int16 height = r.ReadInt16();

            // Skip a couple bytes we don't care about, more header information.
            r.BaseStream.Seek(2, SeekOrigin.Current);

            Texture2D tex = new Texture2D(width, height);
            Color32[] pulledColors = new Color32[width * height];

            for (int i = 0; i < width * height; i++)
            {

                float red = Convert.ToSingle(r.ReadByte());  
                float green = Convert.ToSingle(r.ReadByte());    
                float blue = Convert.ToSingle(r.ReadByte());
                float alpha = Convert.ToSingle(r.ReadByte());

                alpha /= 255;
                green /= 255;
                blue /= 255;
                red /= 255;
   
                pulledColors [i] = new Color(blue, green, red, alpha);
            }

            tex.SetPixels32(pulledColors);
            tex.Apply();
            return tex;

        }
    }
}