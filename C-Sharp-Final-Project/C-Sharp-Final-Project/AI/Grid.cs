using System;
using System.Collections.Generic;
using System.Windows;
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

            Vector nodePosition = new Vector(0, 0);
            for (int x = 0; x < numNodeWidth; x++) { 
                nodePosition.X = halfNodeWidth + (nodeWidth * x);
                for (int y = 0; y < numNodeHeight; y++)
                {
                    nodePosition.Y = halfNodeHeight + (nodeHeight * y);
                    worldNodes[x, y] = new Node(nodePosition, new Vector(x, y), true);
                }
            }

            rect.w = (int) nodeWidth;
            rect.h = (int)nodeHeight;

        }
        public List<Node> PossibleNodeNeighbors(Node node, int depth)
        {
            List<Node> neighbors = new List<Node>();

            Vector grid = new Vector(0, 0);
            for (int x = -depth; x <= depth; x++)
            {
                for (int y = -depth; y <= depth; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    grid.X = node.gridPosition.X + x;
                    grid.Y = node.gridPosition.Y + y;

                    if ((grid.X >= 0) && (grid.X < numNodeWidth) &&
                        (grid.Y >= 0) && (grid.Y < numNodeHeight) &&
                        worldNodes[(int)grid.X, (int)grid.Y].walkable)
                    {
                        neighbors.Add(worldNodes[(int)grid.X, (int)grid.Y]);
                    }
                }
            }
            return neighbors;
        }

        public Node NodeFromWorld(Vector worldPosition)
        {
            int xi = (int) Math.Round((worldPosition.X - halfNodeWidth) / nodeWidth, MidpointRounding.AwayFromZero);
            int yi = (int) Math.Round((worldPosition.Y - halfNodeHeight) / nodeHeight, MidpointRounding.AwayFromZero);

            return worldNodes[xi, yi];
        }

        public List<Node> TileNodes(int fromXTile, int fromYTile, int toXTile, int toYTile)
        {
            int widthTiles = toXTile - fromXTile;
            int heightTiles = toYTile - fromYTile;
            List<Node> nodeInTileVolume = new List<Node>();
            int fromXNode = fromXTile * NODES_PER_TILE;
            int fromYNode = fromYTile * NODES_PER_TILE;
            
            for (int x = fromXNode; x < ((widthTiles + 1) * NODES_PER_TILE + fromXNode); x++)
            {
                for (int y = fromYNode; y < ((heightTiles + 1) * NODES_PER_TILE + fromYNode); y++)
                {
                    nodeInTileVolume.Add(worldNodes[x, y]);
                }
            }
            
            return nodeInTileVolume;
        }
		
		public void SetLevelGroupNodes(Node currentNode, int occupationLevel, int neighborDepth)
		{
			foreach (Node neighbor in PossibleNodeNeighbors(currentNode, neighborDepth)){
				if (neighbor.rLevel == currentNode.rLevel) {
					neighbor.rLevel = occupationLevel;
				}
			}				
			currentNode.rLevel = occupationLevel;
		}
		
        public void RenderNodes()
        {
            Node playerNode = NodeFromWorld(new Vector(Game.Player.xpos, Game.Player.ypos));


            foreach(Node node in worldNodes)
            {
                rect.x = (int)(node.worldPosition.X - (nodeWidth / 2));
                rect.y = (int)(node.worldPosition.Y - (nodeHeight / 2));
                if (node == playerNode) {
                    SDL_SetRenderDrawColor(Game.Renderer, 30, 25, 0, 0);
                }
                else if (node.rLevel == 1)
                {
                    SDL_SetRenderDrawColor(Game.Renderer, 30, 25, 0, 0);
                } else if (node.rLevel == 3)
                {
                    SDL_SetRenderDrawColor(Game.Renderer, 90, 25, 90, 0);
                }
                else if (node.path) {
                    SDL_SetRenderDrawColor(Game.Renderer, 255, 255, 0, 0);
                }
                else if (node.walkable)
                {
                    SDL_SetRenderDrawColor(Game.Renderer, 0, 0, 0, 0);
                } else
                {
                    SDL_SetRenderDrawColor(Game.Renderer, 255, 255, 255, 255);
                }
                SDL_RenderFillRect(Game.Renderer, ref rect); //testing
            }
        }
    }
}
