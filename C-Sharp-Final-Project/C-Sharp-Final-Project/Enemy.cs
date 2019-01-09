using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private const int SPEED = 1; //May subject to change
        private const int SHOOTING_RANGE = 145;

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

		private const int RADIUS_IN_NODES = 2;
        private int radius;

        //if player goes near enemy, player die

        public Enemy(Vector position, int width, int height, string texture)
        {
            objTexture = Texture.LoadTexture(texture);

            this.position = position;
            velocity = new Vector(0, 0);

            searchPlayerCoolDown = 0;
            pathPending = false;
			
			Game.Grid.SetLevelGroupNodes(Game.Grid.NodeFromWorld(position), 2, RADIUS_IN_NODES);
            radius = width / 2;
            dest.w = width;
            dest.h = height;
        }

        public void Update()
        { 
            //finding new path
            if (Component.CoolDown(ref searchPlayerCoolDown, 50) &&
               (targetPosition != Game.Player.position))
            {
                targetPosition = Game.Player.position;
                if (!pathPending && 
                    (!Component.DistanceOfPointsLessThan(targetPosition, position, SHOOTING_RANGE) ||
                    Raycaster.AreWallsBlockView(targetPosition, position, Game.Walls)))
                {
                    if (path != null)
                    {
                        foreach (Node node in path)
                            node.path = false;
                    }// testing 
                    if (path != null)
                    {
                        for (int i = currentTargetIndex - 1; i < path.Count; i++)
                        {
                            if (i >= 0)
                                Game.Grid.SetLevelGroupNodes(path[i], 0, RADIUS_IN_NODES);
                        }
                        Game.Grid.SetLevelGroupNodes(Game.Grid.NodeFromWorld(position), 3, 2);
                    }
                    Game.Pathmanager.RequestPath(position, targetPosition, SHOOTING_RANGE, OnPathFound);
                    pathPending = true;
                }
                
            } else if (path != null)
            {
                /*
                if (Component.DistanceOfPointsLessThan(targetPosition, position, SHOOTING_RANGE) &&
                    !Raycaster.AreWallsBlockView(targetPosition, position, Game.Walls))
                {
                    for (int i = currentTargetIndex - 1; i < path.Count; i++)
                    {
                        if (i >= 0)
                            Game.Grid.SetLevelGroupNodes(path[i], 0, RADIUS_IN_NODES);
                    }
                    Game.Grid.SetLevelGroupNodes(Game.Grid.NodeFromWorld(position), 3, 2);
                }
                else
                {
                */
                if (currentTargetIndex < path.Count)
                {
                    bool nearAnotherEnemy = false;
                    foreach (Enemy e in Game.Enemies)
                    {
                        if (e != this)
                            nearAnotherEnemy = Component.DistanceOfPointsLessThan(position, e.position, radius * 2);
                        if (nearAnotherEnemy)
                            break;
                    }
                    if (!nearAnotherEnemy)
                        position = LocateNextPosition();
                }
                else
                {
                    if (currentTargetIndex - 1 >= 0)
                    {
                        Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 0, RADIUS_IN_NODES, 1);
                    }
                    //shoot bullets
                }
                
            } 
            
        }

        private void OnPathFound(List<Node> foundPath, bool found)
        {
            if (found && foundPath.Count != 0)
            {
                path = foundPath;

                for (int i = 0; i < path.Count; i++)
                {
                    if (i == path.Count - 1)
                        Game.Grid.SetLevelGroupNodes(path[i], 3, RADIUS_IN_NODES);
                    else
                        Game.Grid.SetLevelGroupNodes(path[i], 1, RADIUS_IN_NODES);
                }
                currentTargetIndex = 0;
            } else
            {
                Game.Grid.SetLevelGroupNodes(Game.Grid.NodeFromWorld(position), 3, RADIUS_IN_NODES);
            }
           
            pathPending = false;   
        }

        private Vector LocateNextPosition()
        {
            Vector newPosition = position;
            Vector newDirection = direction;
            double incrementSpeed = SPEED;
            double currentDistance;

            while (currentTargetIndex < path.Count)
            {
                currentDistance = Component.DistanceOfPoints(newPosition, path[currentTargetIndex].worldPosition);
                if (incrementSpeed > currentDistance)
                {
                    incrementSpeed = incrementSpeed - currentDistance;
                    newPosition = path[currentTargetIndex].worldPosition;
                    if (currentTargetIndex - 1 >= 0)
                    {
                        Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 0, RADIUS_IN_NODES, 1);
                    }
                    if (currentTargetIndex + 1 == path.Count)
                    {
                        break;
                    }
                    else
                    {
                        currentTargetIndex++;
                        currentDistance = Component.DistanceOfPoints(newPosition, path[currentTargetIndex].worldPosition);
                    }
                }
                if (incrementSpeed < currentDistance)
                {
                    direction = (path[currentTargetIndex].worldPosition - newPosition) / currentDistance;
                    velocity = direction * incrementSpeed;
                    newPosition += velocity;
                    break;
                }
                if (incrementSpeed == currentDistance)
                {
                    if (currentTargetIndex - 1 >= 0)
                    {
                        Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 0, RADIUS_IN_NODES, 1);
                    }
                    newPosition = path[currentTargetIndex].worldPosition;
                    currentTargetIndex++;
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
