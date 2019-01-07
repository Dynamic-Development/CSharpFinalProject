using System;
using SDL2;

namespace C_Sharp_Final_Project
{
    class Texture
    {
        public static IntPtr LoadTexture(string texturePath)
        {
            IntPtr surface = SDL_image.IMG_Load(texturePath);
            IntPtr texture = SDL.SDL_CreateTextureFromSurface(Game.Renderer, surface);
            SDL.SDL_FreeSurface(surface);
            return texture;
        }
    }
}
