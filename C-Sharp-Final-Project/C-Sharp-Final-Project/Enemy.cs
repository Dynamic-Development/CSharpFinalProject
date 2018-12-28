using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private const int SPEED = 1; //May subject to change
        private const int SHOOTING_RANGE = 0;

        public Vector position;
        public Vector velocity;
        private Vector direction;

        private SDL_Rect dest;
        private IntPtr objTexture;   

        private int searchPlayerCoolDown;
        private List<Node> path;
        private int currentTargetIndex;
        private bool pathPending;
        private Vector oldPlayerPosition;

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
            if (Component.DistanceOfPoints(Game.Player.xpos, Game.Player.ypos, position.X, position.Y) < SHOOTING_RANGE)
            {
                // && (*castRayToPlayer == true) fire bullets to player 
            } else // continue on
            {
                //finding new path
                if (Component.CoolDown(ref searchPlayerCoolDown, 5))
                {
                    if (oldPlayerPosition.X != Game.Player.xpos ||
                        oldPlayerPosition.Y != Game.Player.ypos)
                    {
                        oldPlayerPosition.Y = Game.Player.ypos;
                        oldPlayerPosition.X = Game.Player.xpos;
                        if (!pathPending)
                        {
                            Game.PathManager.RequestPath(position, oldPlayerPosition, OnPathFound);
                            pathPending = true;
                        }
                    }
                }

                //moving
                if (path != null)
                {
                    if (currentTargetIndex < path.Count - 1)
                    {
                        position = PinPointPosition(ref currentTargetIndex);
                    }
                } 
            }
        }

        private void OnPathFound(List<Node> foundPath, bool found)
        {
            if (found && foundPath.Count != 0)
            {
                path = foundPath;
                currentTargetIndex = 1;
            }
            pathPending = false;   
        }

        private Vector PinPointPosition(ref int targetIndex)
        {
            Vector newPosition = position;
            Vector newDirection = direction;
            double incrementSpeed = SPEED;
            double currentDistance;

            while (targetIndex < path.Count - 1)
            {
                currentDistance = Component.DistanceOfPoints(newPosition.X, newPosition.Y,
                                                             path[targetIndex].worldPosition.X,
                                                             path[targetIndex].worldPosition.Y);
                if (incrementSpeed > currentDistance)
                {
                    incrementSpeed = incrementSpeed - currentDistance;
                    newPosition = path[targetIndex].worldPosition;
                    targetIndex++;
                    currentDistance = Component.DistanceOfPoints(newPosition.X, newPosition.Y,
                                                             path[targetIndex].worldPosition.X,
                                                             path[targetIndex].worldPosition.Y);
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
