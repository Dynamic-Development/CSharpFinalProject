using System;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Player
    {
        private int xpos;
        private int ypos;
        private IntPtr texture;
        private SDL_Rect dest;
        
        public int xvel;
        public int yvel;
        public Player(int posx , int posy, int width, int height, string texturePath)
        {
            xpos = posx;
            ypos = posy;

            xvel = 0;
            yvel = 0;
            texture = Textures.LoadTexture(texturePath);
            dest.w = width;
            dest.h = height;

        }
        
        public void Update()
        {
            xpos = xpos + (xvel * 5);
            ypos = ypos + (yvel * 5);

            dest.x = (int)xpos - dest.w / 2;
            dest.y = (int)ypos - dest.h / 2;
        }
        public void Render()
        {
            SDL_RenderCopy(Game.Renderer, texture, IntPtr.Zero, ref dest);
        }
        
    }
}
