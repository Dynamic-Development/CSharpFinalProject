using System;

namespace C_Sharp_Final_Project
{
    class Grid
    {
        private double nodeWidth, nodeHeight;
        private double halfNodeWidth, halfNodeHeight;
        private int numNodeWidth, numNodeHeight;
        public Node[,] worldNodes { get; }
        public Grid(int worldWidth, int worldHeight, int numNodeWidth, int numNodeHeight)
        {
            nodeWidth = worldWidth / numNodeWidth;
            nodeHeight = worldHeight / numNodeHeight;
            halfNodeHeight = numNodeHeight / 2;
            halfNodeWidth = numNodeWidth / 2;
            worldNodes = new Node[numNodeWidth, numNodeHeight];

            this.numNodeWidth = numNodeWidth;
            this.numNodeHeight = numNodeHeight;

            double nodex, nodey;
            for (int x = 0; x < numNodeHeight; x++)
                for (int y = 0; y < numNodeWidth; y++)
                {
                    nodex = halfNodeWidth + (nodeWidth * x);
                    nodey = halfNodeHeight + (nodeHeight * x);
                    worldNodes[x, y] = new Node(nodex, nodey, x, y, true); 
                }
        }
        public Node[] GetNeighbors(Node node)
        {
            Node[] neighbors = new Node[8];

            int gridX, gridY;
            int numNeighbors = 0;
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    gridX = node.gridX + x;
                    gridY = node.gridY + y;

                    if ((gridX >= 0) && (gridX < numNodeWidth) &&
                        (gridY >= 0) && (gridX < numNodeHeight))
                        neighbors[numNeighbors++] = worldNodes[x, y];
                }

            return neighbors;
        }
        public Node NodeFromWorld(int worldX, int worldY)
        {
            int xi = (int) Math.Round((worldX - halfNodeWidth) / (nodeWidth), MidpointRounding.AwayFromZero);
            int yi = (int) Math.Round((worldX - halfNodeHeight) / (nodeHeight), MidpointRounding.AwayFromZero);

            return worldNodes[xi, yi];
        }
    }
}
