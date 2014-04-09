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
            //The string is read as 64 characters, which includes a bunch of null bytes.  We strip them to avoid oddness when printing and using the texture names.
            name = rawName.Replace("\0", string.Empty);
            this.flags = flags;
            this.contents = contents;

            // Remove some common shader modifiers to get normal
            // textures instead
            name = name.Replace("_hell", string.Empty);
            name = name.Replace("_trans", string.Empty);
            name = name.Replace("flat_400", string.Empty);
            name = name.Replace("_750", string.Empty);
        }
    }
}
