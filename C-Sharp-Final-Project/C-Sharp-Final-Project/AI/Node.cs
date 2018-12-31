using System.Windows;

namespace C_Sharp_Final_Project
{
    public class Node : IHeapItem<Node>
    {
        public Vector worldPosition;
        public Vector gridPosition;
        public bool walkable;
        public int gCost, hCost;
        public Node parent;
        public bool endPoint;

        public bool path = false; //testing

        public Node(Vector worldPosition, Vector gridPosition, bool walkable)
        {
            this.worldPosition = worldPosition;
            this.gridPosition = gridPosition;
            this.walkable = walkable;
            
            gCost = 0;
            hCost = 0;

            parent = null;
            endPoint = false;
        }
        public int fCost {
            get
            {
                return gCost + hCost;
            }
        }
        public int HeapIndex { get; set; }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}
