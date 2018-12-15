using System;
using System.Collections.Generic;

namespace C_Sharp_Final_Project
{
    class Grid
    {
        private double nodeWidth, nodeHeight;
        private double halfNodeWidth, halfNodeHeight;
        private int numNodeWidth, numNodeHeight;
        public Node[,] worldNodes;
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
            for (int x = 0; x < numNodeWidth; x++)
                for (int y = 0; y < numNodeHeight; y++)
                {
                    nodex = halfNodeWidth + (nodeWidth * x);
                    nodey = halfNodeHeight + (nodeHeight * x);
                    worldNodes[x, y] = new Node(nodex, nodey, x, y, true); 
                }
        }
        public List<Node> NodeNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            int gridX, gridY;
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    gridX = node.gridX + x;
                    gridY = node.gridY + y;

                    if ((gridX >= 0) && (gridX < numNodeWidth) &&
                        (gridY >= 0) && (gridX < numNodeHeight))
                        neighbors.Add(worldNodes[x, y]);
                }

            return neighbors;
        }
        public Node NodeFromWorld(int worldX, int worldY)
        {
            int xi = (int) Math.Round((worldX - halfNodeWidth) / (nodeWidth), MidpointRounding.AwayFromZero);
            int yi = (int) Math.Round((worldX - halfNodeHeight) / (nodeHeight), MidpointRounding.AwayFromZero);

            return worldNodes[xi, yi];
        }
        public Node[] GroupNodesInArea(int tileUnitsX, int tileUnitsY, int tileUnitsWidth, int tileUnitsHeight)
        {
            Node[] nodeInUnitsArea = new Node[tileUnitsHeight * tileUnitsWidth];
            int tempIndex = 0;
            for (int x = tileUnitsX; x < tileUnitsWidth; x++)
                for (int y = tileUnitsY; y < tileUnitsHeight; y++)
                    nodeInUnitsArea[tempIndex++] = worldNodes[x, y];

            return nodeInUnitsArea;
        }
    }
}
