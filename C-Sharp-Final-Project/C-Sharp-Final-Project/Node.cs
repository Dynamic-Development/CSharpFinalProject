namespace C_Sharp_Final_Project
{
    class Node
    {
        public double worldX, worldY;
        public int gridX, gridY;
        public bool walkable;
        public int gCost, hCost;

        public Node(double worldX, double worldY, int gridX, int gridY, bool walkable)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.gridX = gridX;
            this.gridY = gridY;
            this.walkable = walkable;

            gCost = 0;
            hCost = 0;
        }
        public int fCost()
        {
            return gCost + hCost;
        }
    }
}
