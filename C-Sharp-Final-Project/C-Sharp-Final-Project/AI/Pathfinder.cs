using System;
using System.Collections.Generic;
using System.Windows;

namespace C_Sharp_Final_Project
{
    class Pathfinder
    {
        List<Node> nodePath;

        public void FindPath(Vector start, Vector target)
        {
            bool success = false;

            Node startNode = Game.Grid.NodeFromWorld(start);
            Node targetNode = Game.Grid.NodeFromWorld(target);
            
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
            Game.PathManager.FinishedrocessingPath(nodePath, success);
        }

        private void RetracePath(Node startNode, Node targetNode)
        {
            foreach (Node node in Game.Grid.worldNodes)
                node.path = false;

            nodePath = new List<Node>();
            Node currentNode = targetNode;
            while(currentNode != startNode)
            {
                nodePath.Add(currentNode);
                currentNode = currentNode.parent;
            }

            nodePath.Reverse();

            foreach (Node node in nodePath)
                node.path = true;
        }

        private int NodeDistance(Node nodea, Node nodeb)
        {
            Vector dist = new Vector(Math.Abs(nodea.gridPosition.X - nodeb.gridPosition.X),
                                     Math.Abs(nodea.gridPosition.Y - nodeb.gridPosition.Y));

            return dist.X > dist.Y ? (int) (14 * dist.Y + 10 * (dist.X - dist.Y)) : (int) (14 * dist.X + 10 * (dist.Y - dist.X));
        }
    }
}
