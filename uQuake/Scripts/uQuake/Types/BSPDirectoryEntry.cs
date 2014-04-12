
namespace SharpBSP
{
    public class BSPDirectoryEntry
    {
        public int Offset
        {
            get;
            private set;
        }

        public int Length
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            set;
        }


        public BSPDirectoryEntry(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        public bool Validate()
        {
            if (Length % 4 == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
