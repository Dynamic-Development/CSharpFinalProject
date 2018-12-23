using System.Collections;
using System;

namespace C_Sharp_Final_Project
{
    class Node
    {
        public double worldX, worldY;
        public int gridX, gridY;
        public bool walkable;
        public int gCost, hCost;
        public Node parent;
        public bool path = false;

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
    }
}
