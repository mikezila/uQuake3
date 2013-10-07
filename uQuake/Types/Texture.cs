
namespace SharpBSP
{
    public class Texture
    {
        public string name;
        public int flags;
        public int contents;

        public Texture(string name, int flags, int contents)
        {
            this.name = name;
            this.flags = flags;
            this.contents = contents;
        }

        public string GetName()
        {
            return name.Trim();
        }
    }
}
