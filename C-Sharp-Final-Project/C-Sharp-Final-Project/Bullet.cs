using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Bullet
    {
        private SDL_Rect dest;
        private Vector position;
        private Vector velocity;
        private IntPtr texture;
        private Vector[] boundary = new Vector[4];
        private Vector direction;
        
        public Bullet(bool playerBullet, Vector position, Vector targetpos, string texture)
        {
            double distance = Component.DistanceOfPoints(position, targetpos);
            this.position = position;
            direction = (targetpos - position) / distance;
            velocity = direction * 2;
            this.texture = Texture.LoadTexture(texture);
            dest.w = 16;
            dest.h = 16;
        }
        
        public void Update()
        {
            bool collide = false;
            Vector newPosition = position + velocity * 5;
            boundary[0] = new Vector(newPosition.X - 8, newPosition.Y - 8);
            boundary[1] = new Vector(newPosition.X + 8, newPosition.Y + 8);
            boundary[2] = new Vector(newPosition.X + 8, newPosition.Y - 8);
            boundary[3] = new Vector(newPosition.X - 8, newPosition.Y + 8);

            foreach (Tile wall in Game.Walls)
            {
                foreach (Vector point in boundary)
                {
                    if (Component.BoundaryCheck(wall.boundary[0], wall.boundary[1], point))
                    {
                        collide = true;
                        break;
                    }
                }
            }
            if (!collide)
            {
                position = newPosition;
            }
        }

        public void Render()
        {
            dest.x = (int)Math.Round(position.X - (dest.w / 2), MidpointRounding.AwayFromZero);
            dest.y = (int)Math.Round(position.Y - (dest.h / 2), MidpointRounding.AwayFromZero);
            SDL_RenderCopy(Screen.Renderer, texture, IntPtr.Zero, ref dest);
        }
    }
}
