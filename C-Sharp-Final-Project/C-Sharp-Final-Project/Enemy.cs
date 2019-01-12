using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Enemy
    {
        private double SPEED;
        private int SHOOTING_RANGE;
        private const int ACTIVATE_RANGE = 500;

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
        private int maxReloadBuffer;
        private int shootCoolDown;
        private int healthBar;
        private int damage;
        public byte type;
        //if player goes near enemy, player die

        public Enemy(Vector position, int width, int height, byte type)
        {
            if (type == 1) objTexture = Texture.LoadTexture("Textures/Enemy_Single.png");
            else if (type == 2) objTexture = Texture.LoadTexture("Textures/Enemy_Double.png");
            else if (type == 3) objTexture = Texture.LoadTexture("Textures/Enemy_Bomber.png");

            this.type = type;

            BehaviorSet();

            this.position = position;
            velocity = new Vector();

            srcrect.x = 0;
            srcrect.y = 0;
            srcrect.w = width;
            srcrect.h = height;

            searchPlayerCoolDown = 0;
            pathPending = false;

            degrees = (Math.Atan2(Game.Player.position.Y - position.Y, Game.Player.position.X - position.X) * 180) / Math.PI;
            shootCoolDown = maxReloadBuffer;

            Game.Grid.SetLevelGroupNodes(Game.Grid.NodeFromWorld(position), 2, RADIUS_IN_NODES);
            radius = 30;
            dest.w = width;
            dest.h = height;
        }

        private void BehaviorSet()
        {
            if (type == 1)
            {
                SHOOTING_RANGE = 500;
                healthBar = 15;
                SPEED = 1.3;
                damage = 2;
                maxReloadBuffer = 15;
            } else if (type == 2)
            {
                SHOOTING_RANGE = 410;
                healthBar = 20;
                SPEED = 1.5;
                damage = 1;
                maxReloadBuffer = 10;
            } else if (type == 3)
            {
                SHOOTING_RANGE = 0;
                healthBar = 35;
                SPEED = 2.5;
                damage = 12;
                maxReloadBuffer = 0;
            }

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

                        if (type == 3 && Component.DistanceOfPointsLessThan(position, Game.Player.position, Game.Player.radius + radius))
                        {
                            Game.Player.healthBar -= damage;
                            Death();
                        }
                    }
                    else
                    {
                        if (currentTargetIndex >= 0)
                        {
                            //Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 0, RADIUS_IN_NODES, 1);
                            Game.Grid.SetLevelGroupNodes(path[currentTargetIndex - 1], 3, RADIUS_IN_NODES, 1);
                        }
                        degrees = (Math.Atan2(Game.Player.position.Y - position.Y, Game.Player.position.X - position.X) * 180) / Math.PI;
                        DamageBehavior();
                    }
                }
            }
        }

        public void DamageBehavior()
        {
            if (type == 1)
            {
                if (!Raycaster.AreWallsBlockView(targetPosition, position, Game.Walls))
                {
                    if (Component.CoolDown(ref shootCoolDown, maxReloadBuffer, true))
                    {
                        Game.Bullets.Add(new Bullet(false, position, Game.Player.position, "Textures/EnemyBullet.png", damage));
                    }
                }
            } else if (type == 3)
            {
                if(Component.DistanceOfPointsLessThan(position, Game.Player.position, Game.Player.radius + radius))
                {
                    Game.Player.Hit(damage);
                    Death();
                }
            } else if (type == 2)
            {
                if (!Raycaster.AreWallsBlockView(targetPosition, position, Game.Walls))
                {
                    if (Component.CoolDown(ref shootCoolDown, maxReloadBuffer, true))
                    {
                        double r = (degrees * Math.PI) / 180;
                        double pX = Math.Sin(r);
                        double pY = Math.Cos(r);
                        Game.Bullets.Add(new Bullet(false, position + new Vector(pX * 27, pY * 27), Game.Player.position, "Textures/EnemyBullet.png", damage));
                        Game.Bullets.Add(new Bullet(false, position + new Vector(pX * -27, pY * -27), Game.Player.position, "Textures/EnemyBullet.png", damage));
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
