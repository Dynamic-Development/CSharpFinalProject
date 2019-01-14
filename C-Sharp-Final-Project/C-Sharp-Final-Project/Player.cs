using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Player
    {
        private const int SPEED = 3;

        public Vector position;
        private IntPtr texture;
        private SDL_Rect dest;
        private SDL_Rect srcrect;
        private Vector[] boundary = new Vector[4];
        public Vector velocity;
        private SDL_Point point;
        private double direction;
        private int shootCoolDown = 0;
        public double healthBar = 30;
        private SDL_Rect health;
        private Vector checkNewPosition;
        public int radius;

        public Player(Vector position, int width, int height, string texturePath)
        {
            this.position = position;
            velocity = new Vector();
            texture = Texture.LoadTexture(texturePath);

            srcrect.x = 0;
            srcrect.y = 0;
            srcrect.w = width;
            srcrect.h = height;

            dest.w = width;
            dest.h = height;

            radius = 30;

            health.h = 15;
            health.x = 5;
            health.y = 5;
        }
        
        public void Shoot()
        {
            if (shootCoolDown == 0)
            {
                Game.Bullets.Add(new Bullet(true, position, new Vector(Game.mousePosX, Game.mousePosY), "Textures/PlayerBullet.png", 1));
                shootCoolDown = 8;
            }
        }

        public void Update()
        {
            Component.CoolDown(ref shootCoolDown, 10, false);

            if (velocity.X != 0 && velocity.Y != 0)
                velocity *= .5;

            checkNewPosition = position + velocity * SPEED;
            velocity.X = 0;
            velocity.Y = 0;

            foreach (Tile wall in Game.Walls)
            {
                if (wall.level == 5)
                {
                    if(Component.WallCollision(wall, checkNewPosition, 16))
                    {
                        Game.nextLevelPend = true;
                        checkNewPosition = position;
                        break;
                    }
                }
                else if (wall.level == 3)
                {
                    if (Component.ScreenCollision(checkNewPosition, 16))
                    {
                        if (Component.ScreenCollision(new Vector(checkNewPosition.X, position.Y), 16))
                        {
                            checkNewPosition.X = position.X;
                        }
                        if (Component.ScreenCollision(new Vector(position.X, checkNewPosition.Y), 16))
                        {
                            checkNewPosition.Y = position.Y;
                        }
                    }
                } 
                else if (Component.WallCollision(wall, checkNewPosition, 16))
                {
                    if (Component.WallCollision(wall, new Vector(checkNewPosition.X, position.Y), 16))
                        checkNewPosition.X = position.X;
                    if (Component.WallCollision(wall, new Vector(position.X, checkNewPosition.Y), 16))
                        checkNewPosition.Y = position.Y;
                    break;
                }
            }

            for(int i = 0; i < Game.Enemies.Count; i++)
            {
                if (Game.Enemies[i].type != 3 && Component.DistanceOfPointsLessThan(Game.Enemies[i].position, position, radius + Game.Enemies[i].radius))
                {
                    Death();
                    break;
                }
            }
            Console.WriteLine(healthBar);
            if(healthBar <= 0)
            {
                Death();
            }

            position = checkNewPosition; 
            direction = (Math.Atan2(Game.mousePosY - position.Y, Game.mousePosX - position.X)*180)/Math.PI;
        }

        public void Hit(int damage)
        {
            healthBar -= damage;
        }

        public void Death()
        {
            Game.playerDeath = true;
        }

        public void Render()
        {
            dest.x = (int) position.X - dest.w / 2;
            dest.y = (int) position.Y - dest.h / 2;
            point.x = dest.w/2;
            point.y = dest.h/2;
            SDL_RenderCopyEx(Screen.Renderer, texture, ref srcrect, ref dest, direction + 90, ref point, SDL_RendererFlip.SDL_FLIP_NONE);

            health.w = (int)healthBar * 6;
            SDL_SetRenderDrawColor(Screen.Renderer, 204, 0, 0, 50); //healthbar
            SDL_RenderFillRect(Screen.Renderer, ref health);
        }
        
    }
}
