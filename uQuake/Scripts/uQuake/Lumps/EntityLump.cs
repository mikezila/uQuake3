using System;
using System.Text;

namespace SharpBSP
{
    public class EntityLump
    {
        public string EntityString
        {
            get;
            private set;
        }


        public EntityLump(string lump)
        {
            EntityString = lump;
        }

        public override string ToString()
        {
            return EntityString;
        }
    }
}
