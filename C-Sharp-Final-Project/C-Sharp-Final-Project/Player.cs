using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Player
    {
        private const int SPEED = 2;

        public Vector position;
        private IntPtr texture;
        private SDL_Rect dest;
        private SDL_Rect srcrect;
        private Vector[] boundary = new Vector[4];
        public Vector velocity;
        private SDL_Point point;
        private double direction;
        private int shootCoolDown = 0;
        public double healthBar = 32;
        private SDL_Rect health;
        private Vector checkNewPosition;

        public Player(Vector position, int width, int height, string texturePath)
        {
            this.position = position;
            velocity = new Vector();
            texture = Texture.LoadTexture(texturePath);

            srcrect.w = width;
            srcrect.h = height;
            dest.w = width;
            dest.h = height;
            health.h = 15;
            health.x = 5;
            health.y = 5;
        }
        
        public void Shoot()
        {
            if (shootCoolDown == 0)
            {
                Game.Bullets.Add(new Bullet(true, position, new Vector(Game.mousePosX, Game.mousePosY), "Textures/PlayerBullet.png"));
                shootCoolDown = 20;
            }
        }

        public void Update()
        {
            Component.CoolDown(ref shootCoolDown, 20, false);

            if (Game.KeyStates[0]) velocity.Y--;
            if (Game.KeyStates[1]) velocity.X--;
            if (Game.KeyStates[2]) velocity.Y++;
            if (Game.KeyStates[3]) velocity.X++;
            bool collide = false;

            if (velocity.X != 0 && velocity.Y != 0)
                velocity *= .7071;

            checkNewPosition = position + velocity * SPEED;
            velocity.X = 0;
            velocity.Y = 0;

            boundary[0] = new Vector(checkNewPosition.X - 15, checkNewPosition.Y - 15);
            boundary[1] = new Vector(checkNewPosition.X + 15, checkNewPosition.Y + 15);
            boundary[2] = new Vector(checkNewPosition.X + 15, checkNewPosition.Y - 15);
            boundary[3] = new Vector(checkNewPosition.X - 15, checkNewPosition.Y + 15);

            foreach (Tile wall in Game.Walls)
            {
                foreach (Vector point in boundary)
                {
                    if (wall.level == 3)
                    {
                        if (Component.ScreenBoundaryCheck(wall.boundary[0], wall.boundary[1], point))
                        {
                            if (point.X > wall.boundary[0].X || point.X < wall.boundary[1].X)
                            {
                                checkNewPosition.X = position.X;
                                Console.WriteLine("test x");
                            }
                            if (point.Y < wall.boundary[1].Y || point.Y > wall.boundary[0].Y)
                            {
                                checkNewPosition.Y = position.Y;
                                Console.WriteLine("test Y");
                            }
                            break;
                        }
                    }
                    else if (Component.BoundaryCheck(wall.boundary[0], wall.boundary[1], point))
                    {
                        if (point.X > wall.boundary[0].X || point.X < wall.boundary[1].X)
                        {
                            checkNewPosition.X = position.X;
                        }
                        if (point.Y < wall.boundary[1].Y || point.Y > wall.boundary[0].Y)
                        {
                            checkNewPosition.Y = position.Y;
                        }
                        break;
                    }
                
                }
            }

            position = checkNewPosition; 
            direction = (Math.Atan2(Game.mousePosY - position.Y, Game.mousePosX - position.X)*180)/Math.PI;
        }
        public void Render()
        {
            srcrect.x = 0;
            srcrect.y = 0;
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
