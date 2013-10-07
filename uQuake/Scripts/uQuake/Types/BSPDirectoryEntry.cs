
namespace SharpBSP
{
    public class BSPDirectoryEntry
    {
        public int offset;
        public int length;
        public string name;

        public BSPDirectoryEntry(int offset, int length)
        {
            this.offset = offset;
            this.length = length;
        }

        public string GetName()
        {
            return name;
        }

        public bool Validate()
        {
            if (length % 4 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
