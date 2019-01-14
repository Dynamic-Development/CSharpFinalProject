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
        private Vector newPosition;
        private bool playerBullet;
        private int damage;

        public Bullet(bool playerBullet, Vector position, Vector targetpos, string texture, int damage)
        {
            this.playerBullet = playerBullet;
            this.damage = damage;
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
            newPosition = position + velocity * 5;

            if (playerBullet) {
                for (int i = 0; i < Game.Enemies.Count; i++)
                {
                    if (Component.DistanceOfPointsLessThan(Game.Enemies[i].position, position, Game.Enemies[i].radius + 8))
                    {
                        Game.Enemies[i].Hit();
                        Game.Bullets.Remove(this);
                    }
                }
            } else
            {
                if (Component.DistanceOfPointsLessThan(Game.Player.position, position, Game.Player.radius + 8))
                {
                    Game.Player.Hit(damage);
                    Game.Bullets.Remove(this);
                }
            }

            for(int i = 0; i < Game.Walls.Count; i++)
            {
                if (Game.Walls[i].level == 3)
                {
                    if (Component.ScreenCollision(position))
                    {
                        newPosition = position;
                        Game.Bullets.Remove(this);
                        break;
                    }
                }
                else if (Component.BulletWallCollision(Game.Walls[i], position))
                {
                    newPosition = position;
                    Game.Bullets.Remove(this);
                    break;
                }
            }
            position = newPosition;
           
        }

        public void Render()
        {
            dest.x = (int)Math.Round(position.X - (dest.w / 2), MidpointRounding.AwayFromZero);
            dest.y = (int)Math.Round(position.Y - (dest.h / 2), MidpointRounding.AwayFromZero);
            SDL_RenderCopy(Screen.Renderer, texture, IntPtr.Zero, ref dest);
        }
    }
}
