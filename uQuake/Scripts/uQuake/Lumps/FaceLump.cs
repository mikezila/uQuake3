using System.Collections.Generic;
using System.Text;

namespace SharpBSP
{
    public class FaceLump
    {
        public Face[] Faces { get; set; }

        public FaceLump(int faceCount)
        {
            Faces = new Face[faceCount];
        }

        public string PrintInfo()
        {
            StringBuilder blob = new StringBuilder();
            int count = 0;
            foreach (Face face in Faces)
            {
                blob.AppendLine("Face " + count.ToString() + "\t Tex: " + face.texture.ToString() + "\tType: " + face.type.ToString() + "\tVertIndex: " + face.vertex.ToString() + "\tNumVerts: " + face.n_vertexes.ToString() + "\tMeshVertIndex: " + face.meshvert.ToString() + "\tMeshVerts: " + face.n_meshverts + "\r\n");
                count++;
            }
            return blob.ToString();
        }
    }
}
