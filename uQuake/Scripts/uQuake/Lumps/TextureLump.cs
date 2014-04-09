using System.Collections.Generic;
using UnityEngine;
using Ionic.Zip;
using System.IO;
using System.Linq;

namespace SharpBSP
{
    public class TextureLump
    {
        public List<Texture> textures = new List<Texture>();
        private Dictionary<string,Texture2D> readyTextures = new Dictionary<string, Texture2D>();

        public TextureLump()
        {
        }

        public int TextureCount
        { 
            get
            {
                return textures.Count; 
            } 
        }

        public bool ContainsTexture(string textureName)
        {
            foreach (KeyValuePair<string,Texture2D> tex in readyTextures)
            {
                if (tex.Key == textureName)
                    return true;
            }
            return false;
        }

        public Texture2D GetTexture(string textureName)
        {
            return readyTextures [textureName];
        }

        public void PullInTextures(string pakName)
        {
            using (ZipFile pak = ZipFile.Read("Assets/baseq3/"+pakName))
            {
                LoadJPGTextures(pak);
                LoadTGATextures(pak);
            }
        }

        private void LoadJPGTextures(ZipFile pk3)
        {
            foreach (Texture tex in textures)
            {
                // The size of the new Texture2D object doesn't matter. It will be replaced (including its size) with the data from the .jpg texture that's getting pulled from the pk3 file.
                if (pk3.ContainsEntry(tex.name + ".jpg"))
                {
                    Texture2D readyTex = new Texture2D(4, 4);
                    var entry = pk3 [tex.name + ".jpg"];
                    using (var stream = entry.OpenReader())
                    {
                        var ms = new MemoryStream();
                        entry.Extract(ms);
                        readyTex.LoadImage(ms.GetBuffer());
                    }
                    
                    readyTex.name = tex.name;
                    readyTex.filterMode = FilterMode.Trilinear;
                    readyTex.Compress(true);
                    
                    if (readyTextures.ContainsKey(tex.name))
                    {
                        Debug.Log("Updating texture with name " + tex.name);
                        readyTextures [tex.name] = readyTex;
                    } else
                        readyTextures.Add(tex.name, readyTex);
                }
            }
        }

        private void LoadTGATextures(ZipFile pk3)
        {
            foreach (Texture tex in textures)
            {
                // The size of the new Texture2D object doesn't matter. It will be replaced (including its size) with the data from the .jpg texture that's getting pulled from the pk3 file.
                if (pk3.ContainsEntry(tex.name + ".tga"))
                {
                    Texture2D readyTex = new Texture2D(4, 4);
                    var entry = pk3 [tex.name + ".tga"];
                    using (var stream = entry.OpenReader())
                    {
                        var ms = new MemoryStream();
                        entry.Extract(ms);
                        readyTex = TGALoader.LoadTGA(ms);
                    }

                    readyTex.name = tex.name;
                    readyTex.filterMode = FilterMode.Trilinear;
                    readyTex.Compress(true);

                    if (readyTextures.ContainsKey(tex.name))
                        Debug.Log("Skipping texture with name " + tex.name);
                    else
                        readyTextures.Add(tex.name, readyTex);
                }
            }
        }


        public string PrintInfo()
        {
            string blob = "\r\n=== Textures =====\r\n";
            int count = 0;
            foreach (Texture tex in textures)
            {
                blob += ("Texture " + count++ + " Name: " + tex.name.Trim() + "\tFlags: " + tex.flags.ToString() + "\tContents: " + tex.contents.ToString() + "\r\n");
            }
            return blob;
        }
    }
}
