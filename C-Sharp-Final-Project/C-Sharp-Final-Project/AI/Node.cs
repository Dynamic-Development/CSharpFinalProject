using System.Windows;

namespace C_Sharp_Final_Project
{
    class Node
    {
        public Vector worldPosition;
        public Vector gridPosition;
        public bool walkable;
        public int gCost, hCost;
        public Node parent;

        public bool path = false;
        public Node(Vector worldPosition, Vector gridPosition, bool walkable)
        {
            this.worldPosition = worldPosition;
            this.gridPosition = gridPosition;
            this.walkable = walkable;

            gCost = 0;
            hCost = 0;

            parent = null;
        }
        public int fCost {
            get
            {
                return gCost + hCost;
            }
        }
    }
}
