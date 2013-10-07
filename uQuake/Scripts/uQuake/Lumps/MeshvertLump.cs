using System.Collections.Generic;

namespace SharpBSP
{
    public class MeshvertLump
    {
        public List<int> meshVerts = new List<int>();

        public MeshvertLump()
        {
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Mesh Verts =====\r\n";
            int count = 0;
            foreach (int meshvert in meshVerts)
            {
                blob += "MeshVert " + count.ToString() + " VertIndex: " + meshvert.ToString() + "\r\n";
                count++;
            }
            return blob;
        }
    }
}
