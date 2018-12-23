using System.Collections;
using System;

namespace C_Sharp_Final_Project
{
    class Node : IHeapItem<Node>
    {
        public double worldX, worldY;
        public int gridX, gridY;
        public bool walkable;
        public int gCost, hCost;
        public Node parent;
        private int heapIndex;

        public Node(double worldX, double worldY, int gridX, int gridY, bool walkable)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.gridX = gridX;
            this.gridY = gridY;
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
        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }
        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
                compare = hCost.CompareTo(nodeToCompare.hCost);
            return -compare;
        }
    }
}
