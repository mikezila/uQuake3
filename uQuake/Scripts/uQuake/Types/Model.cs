using UnityEngine;

namespace SharpBSP
{
    public class Model
    {
        public Vector3 mins;
        public Vector3 maxes;
        public int face;
        public int n_faces;
        public int brush;
        public int n_brushes;

        public Model(Vector3 mins, Vector3 maxes, int face, int n_faces, int brush, int n_brushes)
        {
            this.mins = mins;
            this.maxes = maxes;
            this.face = face;
            this.n_faces = n_faces;
            this.brush = brush;
            this.n_brushes = n_brushes;
        }
    }
}
