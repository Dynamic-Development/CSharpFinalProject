using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Grid
    {
        public int tileWidth;
        public int tileHeight;
        public Node[,] worldNodes;
        public int numNodeWidth, numNodeHeight;

        private const int NODES_PER_TILE = 3;

        private double nodeWidth, nodeHeight;
        private double halfNodeWidth, halfNodeHeight;

        SDL_Rect rect;

        public Grid(int worldWidth, int worldHeight, int numTileWidth, int numTileHeight)
        {
            numNodeWidth = numTileWidth * NODES_PER_TILE;
            numNodeHeight = numTileHeight * NODES_PER_TILE;
            nodeWidth = worldWidth / numNodeWidth;
            nodeHeight = worldHeight / numNodeHeight;
            halfNodeHeight = nodeHeight / 2;
            halfNodeWidth = nodeWidth / 2;
            worldNodes = new Node[numNodeWidth, numNodeHeight];
            tileHeight = worldHeight / numTileHeight;
            tileWidth = worldWidth / numTileWidth;

            double nodex, nodey;
            for (int x = 0; x < numNodeWidth; x++) { 
                nodex = halfNodeWidth + (nodeWidth * x);
                for (int y = 0; y < numNodeHeight; y++)
                {
                    nodey = halfNodeHeight + (nodeHeight * y);
                    worldNodes[x, y] = new Node(nodex, nodey, x, y, true);
                }
            }

            rect.w = (int) nodeWidth;
            rect.h = (int)nodeHeight;

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
                        (gridY >= 0) && (gridY < numNodeHeight))
                    {
                        neighbors.Add(worldNodes[gridX, gridY]);
                    }
                }

            return neighbors;
        }

        public Node NodeFromWorld(int worldX, int worldY)
        {
            int xi = (int) Math.Round((worldX - halfNodeWidth) / nodeWidth, MidpointRounding.AwayFromZero);
            int yi = (int) Math.Round((worldY - halfNodeHeight) / nodeHeight, MidpointRounding.AwayFromZero);

            return worldNodes[xi, yi];
        }

        public Node[] GroupNodesTileArea(int fromXTile, int fromYTile, int toXTile, int toYTile)
        {
            int widthTiles = toXTile - fromXTile;
            int heightTiles = toYTile - fromXTile;
            Node[] nodeInTileArea = new Node[(heightTiles) * (widthTiles) * NODES_PER_TILE * NODES_PER_TILE];
            int fromXNode = fromXTile * NODES_PER_TILE;
            int fromYNode = fromYTile * NODES_PER_TILE;
            int tempIndex = 0;
            for (int x = fromXNode; x < ((widthTiles) * NODES_PER_TILE + fromXNode); x++)
                for (int y = fromYNode; y < ((heightTiles) * NODES_PER_TILE + fromYNode); y++) {
                    nodeInTileArea[tempIndex++] = worldNodes[x, y];
                }
            return nodeInTileArea;
        }

        public void RenderNodes()
        {
            foreach(Node n in worldNodes)
            {
                rect.x = (int)(n.worldX - (nodeWidth / 2));
                rect.y = (int)(n.worldY - (nodeHeight / 2));

                if (n.walkable)
                {
                    SDL_SetRenderDrawColor(Game.Renderer, 0, 0, 0, 0);

                } else
                {
                    SDL_SetRenderDrawColor(Game.Renderer, 255, 255, 255, 255);
                }
                SDL_RenderFillRect(Game.Renderer, ref rect);
            }

        }
    }
}
