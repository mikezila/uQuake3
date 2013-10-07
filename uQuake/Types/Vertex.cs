using UnityEngine;

namespace SharpBSP
{
    public class Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public byte[] color;

        // These are texture coords, or UVs
        public Vector2 texcoord = new Vector2();
        public Vector2 lmcoord = new Vector2();

        public Vertex(Vector3 position, float texX, float texY, float lmX, float lmY, Vector3 normal, byte[] color)
        {
            this.position = position;
            this.normal = normal;

            // Color data doesn't get used
            this.color = color;

            texcoord.x = texX;
            texcoord.y = texY;
            lmcoord.x = lmX;
            lmcoord.y = lmY;

            // Do that swizzlin'.
            // Dunno why it's called swizzle.  An article I was reading called it that.
            // Swizzle, swizzle, swizzle.
            Swizzle();
        }

        // This converts the verts from the format Q3 uses to the one Unity3D uses
        // Look up the Q3 map/rendering specs if you want the details.
        // Quake3 also uses an odd scale where 0.03 units is about 1 meter, so scall it way down
        // while you're at it.
        private void Swizzle()
        {
            float tempz = position.z;
            float tempy = position.y;
            position.y = tempz;
            position.z = -tempy;

            tempz = normal.z;
            tempy = normal.y;

            normal.y = tempz;
            normal.z = -tempy;

            position.Scale(new Vector3(0.03f, 0.03f, 0.03f));
            normal.Scale(new Vector3(0.03f, 0.03f, 0.03f));
        }
    }
}
