using UnityEngine;

namespace SharpBSP
{
    public class Plane
    {
        public Vector3 normal;
        public float distance;

        public Plane(Vector3 normal, float distance)
        {
            this.normal = normal;
            this.distance = distance;
        }
    }
}
