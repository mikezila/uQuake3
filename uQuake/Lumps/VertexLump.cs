using System.Collections.Generic;

namespace SharpBSP
{
    public class VertexLump
    {
        public List<Vertex> verts = new List<Vertex>();

        public VertexLump()
        {
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Vertexes =====\r\n";
            int count = 0;
            foreach (Vertex vert in verts)
            {
                blob += "Vertex " + count.ToString() + " Pos: " + vert.position.ToString() + " Normal: " + vert.normal.ToString() + "\r\n";
                count++;
            }
            return blob;
        }
    }
}
