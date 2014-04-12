using System.Text;

namespace SharpBSP
{
    public class VertexLump
    {
		public Vertex[] Verts{ get; set; }
		public int[] MeshVerts{ get; set; }

        public VertexLump(int VertexCount)
        {
			Verts = new Vertex[VertexCount];
        }

        public override string ToString()
        {
			StringBuilder blob = new StringBuilder ();
            int count = 0;
            foreach (Vertex vert in Verts)
            {
                blob.Append("Vertex " + count.ToString() + " Pos: " + vert.position.ToString() + " Normal: " + vert.normal.ToString() + "\r\n");
                count++;
            }
            return blob.ToString();
        }
    }
}
