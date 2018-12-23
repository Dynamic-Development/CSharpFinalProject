using System;
using System.Collections.Generic;

namespace C_Sharp_Final_Project
{
    class Pathfinder
    {
        int[] wayPointsX;
        int[] wayPointsY;

        public void FindPath(int xPos, int yPos, int targetXPos, int targetYPos)
        {
            bool success = false;

            Node startNode = Game.Grid.NodeFromWorld(xPos, yPos);
            Node targetNode = Game.Grid.NodeFromWorld(targetXPos, targetYPos);
            
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            { 
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                    {
                        if (openSet[i].hCost < currentNode.hCost)
                            currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    success = true;
                    break;
                }

                foreach(Node neighbor in Game.Grid.NodeNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;
                    int moveCost = currentNode.gCost + NodeDistance(currentNode, neighbor);
                    if (moveCost < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = moveCost;
                        neighbor.hCost = NodeDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);                     
                    }
                }
            }
            if (success)
                RetracePath(startNode, targetNode);
            Game.PathManager.FinishedrocessingPath(wayPointsX, wayPointsY, success);
        }

        private void RetracePath(Node startNode, Node targetNode)
        { 
            List<Node> nodePath = new List<Node>();
            Node currentNode = targetNode;
            while(currentNode != startNode)
            {
                currentNode.path = true;
                nodePath.Add(currentNode);
                currentNode = currentNode.parent;
            }

            nodePath.Reverse();

            List<int> wayPointsXList = new List<int>();
            List<int> wayPointsYList = new List<int>();
            for (int i = 1; i < nodePath.Count; i++)
            {
                wayPointsXList.Add((int)nodePath[i].worldX);
                wayPointsYList.Add((int)nodePath[i].worldY);
            }
            wayPointsX = wayPointsXList.ToArray();
            wayPointsY = wayPointsYList.ToArray();

        }

        private int NodeDistance(Node nodea, Node nodeb)
        {
            int distX = Math.Abs(nodea.gridX - nodeb.gridX);
            int distY = Math.Abs(nodea.gridY - nodeb.gridY);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
        } 
    }
}
