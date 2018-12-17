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

        private int xvel;
        private int yvel;
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
        public void EventHandler(SDL_EventType e)
        {
        }
        public void Update()
        {
            dest.x = (int)xpos - dest.w / 2;
            dest.y = (int)ypos - dest.h / 2;
        }
        public void Render()
        {
            SDL_RenderCopy(Game.Renderer, texture, IntPtr.Zero, ref dest);
        }
        
    }
}
