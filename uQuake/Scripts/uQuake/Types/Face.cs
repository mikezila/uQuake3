using UnityEngine;
using System.Collections.Generic;

namespace SharpBSP
{
    public class Face
    {
        // The fields in this class are kind of obtuse.  I recommend looking up the Q3 .bsp map spec for full understanding.

        public int texture;
        public int effect;
        public int type;
        public int vertex;
        public int n_vertexes;
        public int meshvert;
        public int n_meshverts;
        public int lm_index;
        public int[] lm_start;
        public int[] lm_size;
        public Vector3 lm_origin;
        public Vector3[] lm_vecs;
        public Vector3 normal;
        public int[] size;

        public Face(int texture, int effect, int type, int vertex, int n_vertexes, int meshvert, int n_meshverts, int lm_index, int[] lm_start, int[] lm_size, Vector3 lm_origin, Vector3[] lm_vecs, Vector3 normal, int[] size)
        {
            this.texture = texture;
            this.effect = effect;
            this.type = type;
            this.vertex = vertex;
            this.n_vertexes = n_vertexes;
            this.meshvert = meshvert;
            this.n_meshverts = n_meshverts;
            this.lm_index = lm_index;
            this.lm_start = lm_start;
            this.lm_size = lm_size;
            this.lm_origin = lm_origin;
            this.lm_vecs = lm_vecs;
            this.normal = normal;
            this.size = size;
        } 
    }
}
