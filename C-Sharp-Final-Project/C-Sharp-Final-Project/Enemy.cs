using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private const int SPEED = 1; //May subject to change
        private const int SHOOTING_RANGE = 90;

        public Vector position;
        public Vector velocity;
        private Vector direction;

        private SDL_Rect dest;
        private IntPtr objTexture;   

        private int searchPlayerCoolDown;
        private List<Node> path;
        private int currentTargetIndex;
        private bool pathPending;
        private Vector targetPosition;

        public Enemy(Vector position, int width, int height, string texture)
        {
            objTexture = Texture.LoadTexture(texture);

            this.position = position;
            velocity = new Vector(0, 0);

            searchPlayerCoolDown = 0;
            pathPending = false;

            dest.w = width;
            dest.h = height;
        }

        public void Update()
        { 
            //finding new path
            if (Component.CoolDown(ref searchPlayerCoolDown, 20))
            {
                if (targetPosition.X != Game.Player.xpos ||
                    targetPosition.Y != Game.Player.ypos)
                {
                    targetPosition.Y = Game.Player.ypos;
                    targetPosition.X = Game.Player.xpos;
                    if (!pathPending)
                    {
                        if (path != null)
                        {
                            foreach (Node node in path)
                                node.path = false;
                        }// testing 
                        if (path != null)
                        {
                            path[path.Count - 1].endPoint = false;
                            foreach (Node endNode in Game.Grid.PossibleNodeNeighbors(path[path.Count - 1], 2))
                                endNode.endPoint = false;

                            foreach (Node node in path)
                            {
                                node.reserved = false;
                                foreach (Node reserved in Game.Grid.PossibleNodeNeighbors(node, 2))
                                    reserved.reserved = false;
                            }
                        }
                        Game.Pathmanager.RequestPath(position, targetPosition, SHOOTING_RANGE, OnPathFound);
                        pathPending = true;
                    }
                }
            }

            //moving
            if (path != null)
            {
                if (currentTargetIndex < path.Count)
                {
                    position = LocateNextPosition(ref currentTargetIndex);
                } else
                {
                    //shoot bullets
                }
            } 
            
        }

        private void OnPathFound(List<Node> foundPath, bool found)
        {
            if (found && foundPath.Count != 0)
            {
                path = foundPath;

                path[path.Count - 1].endPoint = true;
                foreach (Node endNode in Game.Grid.PossibleNodeNeighbors(path[path.Count - 1], 2))
                    endNode.endPoint = true;

                foreach (Node node in path)
                {
                    node.reserved = true;
                    foreach (Node reserved in Game.Grid.PossibleNodeNeighbors(node, 2))
                        reserved.reserved = true;
                }
                currentTargetIndex = 1;
            }
            else
            {
                path[path.Count - 1].endPoint = true;
                foreach (Node endNode in Game.Grid.PossibleNodeNeighbors(path[path.Count - 1], 2))
                    endNode.endPoint = true;
            }
            pathPending = false;   
        }

        private Vector LocateNextPosition(ref int targetIndex)
        {
            Vector newPosition = position;
            Vector newDirection = direction;
            double incrementSpeed = SPEED;
            double currentDistance;

            while (targetIndex < path.Count)
            {
                currentDistance = Component.DistanceOfPoints(newPosition.X, newPosition.Y,
                                                             path[targetIndex].worldPosition.X,
                                                             path[targetIndex].worldPosition.Y);
                if (incrementSpeed > currentDistance)
                {
                    incrementSpeed = incrementSpeed - currentDistance;
                    newPosition = path[targetIndex].worldPosition;
                    if (targetIndex - 1 >= 0)
                    {
                        path[targetIndex - 1].reserved = false;
                        foreach (Node reserved in Game.Grid.PossibleNodeNeighbors(path[targetIndex - 1], 2))
                            reserved.reserved = false;
                    }
                    if (targetIndex + 1 == path.Count)
                    {
                        break;
                    }
                    else
                    {
                        targetIndex++;
                        currentDistance = Component.DistanceOfPoints(newPosition.X, newPosition.Y,
                                                                 path[targetIndex].worldPosition.X,
                                                                 path[targetIndex].worldPosition.Y);
                    }
                }
                if (incrementSpeed < currentDistance)
                {
                    direction = (path[targetIndex].worldPosition - newPosition) / currentDistance;
                    velocity = direction * incrementSpeed;
                    newPosition += velocity;
                    break;
                }
                if (incrementSpeed == currentDistance)
                {
                    if (targetIndex - 1 >= 0)
                    {
                        path[targetIndex - 1].reserved = false;
                        foreach (Node reserved in Game.Grid.PossibleNodeNeighbors(path[targetIndex - 1], 2))
                            reserved.reserved = false;
                    }
                    newPosition = path[targetIndex].worldPosition;
                    targetIndex++;
                    break;
                }

            }
            return newPosition;
        }

        
        public void Render()
        {
            dest.x = (int)Math.Round(position.X - (dest.w / 2), MidpointRounding.AwayFromZero);
            dest.y = (int)Math.Round(position.Y - (dest.h / 2), MidpointRounding.AwayFromZero);
            SDL_RenderCopy(Game.Renderer, objTexture, IntPtr.Zero, ref dest);
        }
    }
}
