using System.Collections.Generic;
using UnityEngine;
using Ionic.Zip;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpBSP
{
    public class TextureLump
    {
		public Texture[] Textures{ get; set; }
        private Dictionary<string,Texture2D> readyTextures = new Dictionary<string, Texture2D>();

        public TextureLump(int textureCount)
        {
			Textures = new Texture[textureCount];
        }

        public int TextureCount
        { 
            get
            {
                return Textures.Length; 
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
            foreach (Texture tex in Textures)
            {
                // The size of the new Texture2D object doesn't matter. It will be replaced (including its size) with the data from the .jpg texture that's getting pulled from the pk3 file.
                if (pk3.ContainsEntry(tex.Name + ".jpg"))
                {
                    Texture2D readyTex = new Texture2D(4, 4);
                    var entry = pk3 [tex.Name + ".jpg"];
                    using (var stream = entry.OpenReader())
                    {
                        var ms = new MemoryStream();
                        entry.Extract(ms);
                        readyTex.LoadImage(ms.GetBuffer());
                    }
                    
                    readyTex.name = tex.Name;
                    readyTex.filterMode = FilterMode.Trilinear;
                    readyTex.Compress(true);
                    
                    if (readyTextures.ContainsKey(tex.Name))
                    {
                        Debug.Log("Updating texture with name " + tex.Name);
                        readyTextures [tex.Name] = readyTex;
                    } else
                        readyTextures.Add(tex.Name, readyTex);
                }
            }
        }

        private void LoadTGATextures(ZipFile pk3)
        {
            foreach (Texture tex in Textures)
            {
                // The size of the new Texture2D object doesn't matter. It will be replaced (including its size) with the data from the texture that's getting pulled from the pk3 file.
                if (pk3.ContainsEntry(tex.Name + ".tga"))
                {
                    Texture2D readyTex = new Texture2D(4, 4);
                    var entry = pk3 [tex.Name + ".tga"];
                    using (var stream = entry.OpenReader())
                    {
                        var ms = new MemoryStream();
                        entry.Extract(ms);
                        readyTex = TGALoader.LoadTGA(ms);
                    }

                    readyTex.name = tex.Name;
                    readyTex.filterMode = FilterMode.Trilinear;
                    readyTex.Compress(true);

                    if (readyTextures.ContainsKey(tex.Name))
                    {
                        Debug.Log("Updating texture with name " + tex.Name + ".tga");
                        readyTextures [tex.Name] = readyTex;
                    } else
                        readyTextures.Add(tex.Name, readyTex);
                }
            }
        }


        public string PrintInfo()
        {
			StringBuilder blob = new StringBuilder ();
            int count = 0;
            foreach (Texture tex in Textures)
            {
                blob.Append("Texture " + count++ + " Name: " + tex.Name.Trim() + "\tFlags: " + tex.Flags.ToString() + "\tContents: " + tex.Contents.ToString() + "\r\n");
            }
            return blob.ToString();
        }
    }
}
