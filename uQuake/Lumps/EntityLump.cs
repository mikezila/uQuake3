
namespace SharpBSP
{
    public class EntityLump
    {
        public string entityString;

        public EntityLump(string lump)
        {
            this.entityString = lump;
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Entities =====\r\n";
            blob += entityString.Replace("\n", "\r\n") + "\r\n";
            return blob;
        }
    }
}
