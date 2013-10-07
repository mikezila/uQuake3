using System.Collections.Generic;

namespace SharpBSP
{
    public class FaceLump
    {
        public List<Face> faces = new List<Face>();

        public FaceLump()
        {
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Faces =====\r\n";
            int count = 0;
            foreach (Face face in faces)
            {
                blob += ("Face " + count.ToString() + "\t Tex: " + face.texture.ToString() + "\tType: " + face.type.ToString() + "\tVertIndex: " + face.vertex.ToString() + "\tNumVerts: " + face.n_vertexes.ToString() + "\tMeshVertIndex: " + face.meshvert.ToString() + "\tMeshVerts: " + face.n_meshverts + "\r\n");
                count++;
            }
            return blob;
        }
    }
}
