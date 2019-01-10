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
        public int numNodeWidth, numNodeHeight, numTileWidth, numTileHeight;

        private const int NODES_PER_TILE = 3;

        private double nodeWidth, nodeHeight;
        private double halfNodeWidth, halfNodeHeight;

        SDL_Rect rect;

        public Grid(int worldWidth, int worldHeight, int numTileWidth, int numTileHeight)
        {
            this.numTileHeight = numTileHeight;
            this.numTileWidth = numTileWidth;

            numNodeWidth = numTileWidth * NODES_PER_TILE;
            numNodeHeight = numTileHeight * NODES_PER_TILE;

            nodeWidth = worldWidth / numNodeWidth;
            nodeHeight = worldHeight / numNodeHeight;

            halfNodeHeight = nodeHeight / 2;
            halfNodeWidth = nodeWidth / 2;

            worldNodes = new Node[numNodeWidth, numNodeHeight];

            tileHeight = worldHeight / numTileHeight;
            tileWidth = worldWidth / numTileWidth;

            Vector nodePosition = new Vector();
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

            Vector grid = new Vector();
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
            try
            {
                int xi = (int)Math.Round((worldPosition.X - halfNodeWidth) / nodeWidth, MidpointRounding.AwayFromZero);
                int yi = (int)Math.Round((worldPosition.Y - halfNodeHeight) / nodeHeight, MidpointRounding.AwayFromZero);

                return worldNodes[xi, yi];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public List<Node> TileNodes(int fromXTile, int fromYTile, int toXTile, int toYTile, int overlay)
        {
            int widthTiles = toXTile - fromXTile;
            int heightTiles = toYTile - fromYTile;
            List<Node> nodeInTileVolume = new List<Node>();
            int fromXNode = fromXTile * NODES_PER_TILE;
            int fromYNode = fromYTile * NODES_PER_TILE;
            
            for (int x = fromXNode - overlay; x < ((widthTiles + 1) * NODES_PER_TILE + fromXNode + overlay); x++)
            {
                for (int y = fromYNode - overlay; y < ((heightTiles + 1) * NODES_PER_TILE + fromYNode + overlay); y++)
                {
                    if ((x >= 0) && (x < numNodeWidth) &&
                        (y >= 0) && (y < numNodeHeight))
                    {
                        nodeInTileVolume.Add(worldNodes[x, y]);
                    }
                }
            }
            
            return nodeInTileVolume;
        }
		
		public void SetLevelGroupNodes(Node currentNode, int occupationLevel, int neighborDepth)
		{
			foreach (Node neighbor in PossibleNodeNeighbors(currentNode, neighborDepth))
				neighbor.rLevel = occupationLevel;	
			currentNode.rLevel = occupationLevel;
		}

        public void SetLevelGroupNodes(Node currentNode, int occupationLevel, int neighborDepth, int specifiedLevelChange)
        {
            foreach (Node neighbor in PossibleNodeNeighbors(currentNode, neighborDepth))
            {
                if (neighbor.rLevel == specifiedLevelChange)
                {
                    neighbor.rLevel = occupationLevel;
                }
            }
            currentNode.rLevel = occupationLevel;
        }

        public Vector? ConvertTileUnitsIntoPixels(int tileUnitX, int tileUnitY)
        {
            if (tileUnitX >= 0 && tileUnitX < numTileWidth &&
                tileUnitY >= 0 && tileUnitY < numTileHeight)
                return new Vector(tileWidth * (tileUnitX + 0.5), tileHeight * (tileUnitY + 0.5));
            return null;
        }

        public void RenderNodes()
        {
            Node playerNode = NodeFromWorld(Game.Player.position);


            foreach(Node node in worldNodes)
            {
                rect.x = (int)(node.worldPosition.X - (nodeWidth / 2));
                rect.y = (int)(node.worldPosition.Y - (nodeHeight / 2));
                if (node == playerNode) {
                    SDL_SetRenderDrawColor(Screen.Renderer, 30, 25, 0, 0);
                }
                else if (node.path)
                {
                    SDL_SetRenderDrawColor(Screen.Renderer, 30, 25, 0, 0);
                } else if (node.rLevel == 3)
                {
                    SDL_SetRenderDrawColor(Screen.Renderer, 90, 25, 90, 0);
                }
                else if (node.rLevel == 1) {
                    SDL_SetRenderDrawColor(Screen.Renderer, 255, 255, 0, 0);
                }
                else if (node.walkable)
                {
                    SDL_SetRenderDrawColor(Screen.Renderer, 0, 0, 0, 0);
                } else
                {
                    SDL_SetRenderDrawColor(Screen.Renderer, 255, 255, 255, 255);
                }
                SDL_RenderFillRect(Screen.Renderer, ref rect); //testing
            }
        }
    }
}
