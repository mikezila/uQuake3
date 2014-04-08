using System.Collections.Generic;

namespace SharpBSP
{
    public class TextureLump
    {
        public List<Texture> textures = new List<Texture>();

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

        public string PrintInfo()
        {
            string blob = "\r\n=== Textures =====\r\n";
            int count = 0;
            foreach (Texture tex in textures)
            {
                blob += ("Texture " + count++ + " Name: " + tex.GetName() + "\tFlags: " + tex.flags.ToString() + "\tContents: " + tex.contents.ToString() + "\r\n");
            }
            return blob;
        }
    }
}
