using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Player
    {
        public Vector position;
        private IntPtr texture;
        private SDL_Rect dest;
        private int[] boundary;
        public int xvel;
        public int yvel;
        public Player(Vector position, int width, int height, string texturePath)
        {
            this.position = position;
            xvel = 0;
            yvel = 0;
            texture = Texture.LoadTexture(texturePath);

            dest.w = width;
            dest.h = height;
        }
        
        public void Update()
        {   
            if (Game.KeyStates[0]) yvel--;
            if (Game.KeyStates[1]) xvel--;
            if (Game.KeyStates[2]) yvel++;
            if (Game.KeyStates[3]) xvel++;
            bool collide = false;
            Vector newPosition = new Vector(position.X + (xvel * 5), position.Y + (yvel * 5));

            foreach (Tile wall in Game.Walls)
            {
                Console.WriteLine(wall.boundary[0] + " " + wall.boundary[1] + " " + wall.boundary[2] + " " + wall.boundary[3] + " " + newPosition);
                if (newPosition.X > wall.boundary[0] && newPosition.X < wall.boundary[2] && newPosition.Y > wall.boundary[1] && newPosition.Y < wall.boundary[3])
                {
                    
                    collide = true;
                    break;
                }
            }
            if (collide == false)
            {
                position = newPosition; 
            }
            


            
            xvel = 0;
            yvel = 0;

            dest.x = (int) position.X - dest.w / 2;
            dest.y = (int) position.Y - dest.h / 2;
        }
        public void Render()
        {
            SDL_RenderCopy(Game.Renderer, texture, IntPtr.Zero, ref dest);
        }
        
    }
}
