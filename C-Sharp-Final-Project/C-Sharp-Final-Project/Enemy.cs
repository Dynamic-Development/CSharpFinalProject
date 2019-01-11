using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private const double SPEED = 1.5; //May subject to change
        private const int SHOOTING_RANGE = 200;
        private const int ACTIVATE_RANGE = 450;

        public Vector position;
        public Vector velocity;
        private Vector direction;
        private double degrees;
        private SDL_Rect dest;
        private IntPtr objTexture;
        private SDL_Point point;
        private SDL_Rect srcrect;

        private int searchPlayerCoolDown;
        private List<Node> path;
        private int currentTargetIndex;
        private bool pathPending;
        private Vector targetPosition;

		private const int RADIUS_IN_NODES = 2;
        public int radius;
        private int shootCoolDown;
        private int healthBar;

        //if player goes near enemy, player die

        public Enemy(Vector position, int width, int height, string texture)
        {
            objTexture = Texture.LoadTexture(texture);

            this.position = position;
            velocity = new Vector();
            healthBar = 5;

            srcrect.x = 0;
            srcrect.y = 0;
            srcrect.w = width;
            srcrect.h = height;

            searchPlayerCoolDown = 0;
            shootCoolDown = 30;
            pathPending = false;

            Game.Grid.SetLevelGroupNodes(Game.Grid.NodeFromWorld(position), 2, RADIUS_IN_NODES);
            radius = width / 2;
            dest.w = width;
            dest.h = height;
        }

        public void Update()
        {
            //finding new path
            if (healthBar <= 0)
            {
                Death();
            }
            else
            {
                if (Component.CoolDown(ref searchPlayerCoolDown, 50, false) && targetPosition != Game.Player.position)
                {
                    targetPosition = Game.Player.position;

                    if (Component.DistanceOfPointsLessThan(targetPosition, position, ACTIVATE_RANGE) || path != null)
                    {
                        if (!pathPending &&
                            (!Component.DistanceOfPointsLessThan(targetPosition, position, SHOOTING_RANGE) ||
                            Raycaster.AreWallsBlockView(targetPosition, position, Game.Walls)))
                        {
                            searchPlayerCoolDown = 50;
                            
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
                    }
                }

                if (path != null)
                {
                    if (currentTargetIndex < path.Count)
                    {
                        position = LocateNextPosition();
                        degrees = Math.Atan2(direction.Y, direction.X) * 180 / Math.PI;

                    }
                    else
                    {
                        if (currentTargetIndex - 1 >= 0)
                        {
                            Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 0, RADIUS_IN_NODES, 1);

                        }
                        degrees = (Math.Atan2(Game.Player.position.Y - position.Y, Game.Player.position.X - position.X) * 180) / Math.PI;
                        if (!Raycaster.AreWallsBlockView(targetPosition, position, Game.Walls))
                        {
                            if (Component.CoolDown(ref shootCoolDown, 30, true))
                            {
                                Game.Bullets.Add(new Bullet(false, position, Game.Player.position, "Textures/EnemyBullet.png"));
                            }
                        }
                    }
                }
            }
        }
        
        public void Hit()
        {
            healthBar--;
        }

        public void Death()
        {
            if (path != null)
            {
                for (int i = currentTargetIndex - 1; i < path.Count; i++)
                {
                    if (i >= 0)
                        Game.Grid.SetLevelGroupNodes(path[i], 0, RADIUS_IN_NODES);
                }
            }
            Game.Enemies.Remove(this);
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
                        currentTargetIndex++;
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

            point.x = dest.w / 2;
            point.y = dest.h / 2;
            SDL_RenderCopyEx(Screen.Renderer, objTexture, ref srcrect, ref dest, degrees + 90, ref point, SDL_RendererFlip.SDL_FLIP_NONE);
           
        }
    }
}
