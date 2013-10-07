using System.Collections.Generic;

namespace SharpBSP
{
    public class PlaneLump
    {
        public List<Plane> planes = new List<Plane>();

        public PlaneLump()
        {
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Planes =====\r\n";
            int count = 0;
            foreach (Plane plane in planes)
            {
                blob += "Plane " + count + " X: " + plane.normal.x.ToString() + " Y: " + plane.normal.y.ToString() + " Z: " + plane.normal.z.ToString() + " Distance: " + plane.distance.ToString() + "\r\n";
                count++;
            }
            return blob;
        }
    }
}
