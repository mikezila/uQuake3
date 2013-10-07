
namespace SharpBSP
{
    public class Node
    {
        public int plane;
        public int[] children;
        public int[] mins;
        public int[] maxes;

        public Node(int plane, int[] children, int[] mins, int[] maxes)
        {
            this.plane = plane;
            this.children = children;
            this.mins = mins;
            this.maxes = maxes;
        }
    }
}
