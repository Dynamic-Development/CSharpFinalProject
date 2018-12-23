using System;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Player
    {
        public int xpos;
        public int ypos;
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

            xpos = xpos + (xvel * 5);
            ypos = ypos + (yvel * 5);
            xvel = 0;
            yvel = 0;

            dest.x = xpos - dest.w / 2;
            dest.y = ypos - dest.h / 2;
        }
        public void Render()
        {
            SDL_RenderCopy(Game.Renderer, texture, IntPtr.Zero, ref dest);
        }
        
    }
}
