using System.Collections.Generic;

namespace SharpBSP
{
    public class TextureLump
    {
        public List<Texture> textures = new List<Texture>();

        public TextureLump()
        {
        }

        public string GetTextureCount()
        {
            return textures.Count.ToString();
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Textures =====\r\n";
            int count = 0;
            foreach (Texture tex in textures)
            {
                blob += ("Plane " + count + " Name: " + tex.GetName() + "\tFlags: " + tex.flags.ToString() + "\tContents: " + tex.contents.ToString() + "\r\n");
                count++;
            }
            return blob;
        }
    }
}
