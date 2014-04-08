using System;

namespace SharpBSP
{
    public class Texture
    {
        public string name;
        public int flags;
        public int contents;

        public Texture(string rawName, int flags, int contents)
        {
            name = rawName.Replace("\0", string.Empty);
            this.flags = flags;
            this.contents = contents;


        }

        public string GetName()
        {
            return name.Trim();
        }
    }
}
