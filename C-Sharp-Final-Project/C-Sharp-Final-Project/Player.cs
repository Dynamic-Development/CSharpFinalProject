﻿using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Player
    {
        public Vector position;
        private IntPtr texture;
        private SDL_Rect dest;
        private SDL_Rect srcrect;
        private Vector[] boundary = new Vector[4];
        public Vector velocity;
        private SDL_Point point;
        private double direction;
        private int cooldown = 0;
        public double healthBar = 32;
        private SDL_Rect health;
        public Player(Vector position, int width, int height, string texturePath)
        {
            this.position = position;
            velocity = new Vector(0, 0);
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
            if (Component.CoolDown(ref cooldown, 20))
            Console.WriteLine("shoot");
            Game.Bullets.Add(new Bullet(true, position, new Vector(Game.mousePosX, Game.mousePosY), "Textures/PlayerBullet.png"));
        }



        public void Update()
        {   
            if (Game.KeyStates[0]) velocity.Y--;
            if (Game.KeyStates[1]) velocity.X--;
            if (Game.KeyStates[2]) velocity.Y++;
            if (Game.KeyStates[3]) velocity.X++;
            bool collide = false;
            Vector newPosition = position + velocity * 5;
            velocity.X = 0;
            velocity.Y = 0;

            boundary[0] = new Vector(newPosition.X - 15, newPosition.Y - 15);
            boundary[1] = new Vector(newPosition.X + 15, newPosition.Y + 15);
            boundary[2] = new Vector(newPosition.X + 15, newPosition.Y - 15);
            boundary[3] = new Vector(newPosition.X - 15, newPosition.Y + 15);

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
            SDL_RenderCopyEx(Game.Renderer, texture, ref srcrect, ref dest, direction + 90, ref point, SDL_RendererFlip.SDL_FLIP_NONE);

            health.w = (int)healthBar * 6;
            SDL_SetRenderDrawColor(Game.Renderer, 204, 0, 0, 50); //healthbar
            SDL_RenderFillRect(Game.Renderer, ref health);
        }
        
    }
}
