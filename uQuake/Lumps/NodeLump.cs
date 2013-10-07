using System.Collections.Generic;

namespace SharpBSP
{
    public class NodeLump
    {
        public List<Node> nodes = new List<Node>();

        public NodeLump()
        {
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Nodes =====\r\n";
            int count = 0;
            foreach (Node node in nodes)
            {
                blob += "Node " + count.ToString() + " Plane: " + node.plane.ToString() + " Children: " + node.children[0].ToString() + ", " + node.children[1].ToString() + "\r\n";
                count++;
            }
            return blob;
        }
    }
}
