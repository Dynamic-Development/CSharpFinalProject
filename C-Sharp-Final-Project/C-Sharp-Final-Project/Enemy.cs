using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private const int SPEED = 1; //May subject to change
        private const int SHOOTING_RANGE = 200;

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

		private int radiusInNodes = 2;
		
        public Enemy(Vector position, int width, int height, string texture)
        {
            objTexture = Texture.LoadTexture(texture);

            this.position = position;
            velocity = new Vector(0, 0);

            searchPlayerCoolDown = 0;
            pathPending = false;
			
			Game.Grid.SetLevelGroupNodes(Game.Grid.NodeFromWorld(position), 2, radiusInNodes);
			
            dest.w = width;
            dest.h = height;
        }

        public void Update()
        { 
            //finding new path
            if (Component.CoolDown(ref searchPlayerCoolDown, 50))
            {
                if (targetPosition.X != Game.Player.xpos ||
                    targetPosition.Y != Game.Player.ypos)
                {
                    targetPosition.Y = Game.Player.ypos;
                    targetPosition.X = Game.Player.xpos;
                    if (!pathPending)
                    {
                        if ((Component.DistanceOfPoints(targetPosition, position) > SHOOTING_RANGE ||
                            Raycaster.WallsBlockView(targetPosition, position, Game.Walls)
                            ))
                        {
                            if (path != null)
                            {
                                foreach (Node node in path)
                                    node.path = false;
                            }// testing 
                            if (path != null)
                            {
                                foreach (Node node in path)
                                {
                                    Game.Grid.SetLevelGroupNodes(node, 0, radiusInNodes);
                                }
                            }
                            Game.Pathmanager.RequestPath(position, targetPosition, SHOOTING_RANGE, OnPathFound);
                            pathPending = true;
                        }
                    }
                }
            }

            //moving
            if (path != null)
            {
                if (currentTargetIndex < path.Count)
                {
                    position = LocateNextPosition();
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

                for(int i = 0; i < path.Count; i++)
                {
					if (i == path.Count - 1)
						Game.Grid.SetLevelGroupNodes(path[i], 3, radiusInNodes);
					else
						Game.Grid.SetLevelGroupNodes(path[i], 1, radiusInNodes);
				}
                currentTargetIndex = 0;
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
                    if (currentTargetIndex - 1 >= 0 && path[currentTargetIndex - 1].rLevel != 3)
                    {
                        Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 0, radiusInNodes);
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
                    if (currentTargetIndex - 1 >= 0 && path[currentTargetIndex - 1].rLevel != 3)
                    {
                        Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 0, radiusInNodes);
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
