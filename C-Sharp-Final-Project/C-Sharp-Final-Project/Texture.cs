using System;
using SDL2;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Texture
    {
        public static IntPtr LoadTexture(string texturePath)
        {
            IntPtr surface = SDL_image.IMG_Load(texturePath);
            IntPtr texture = SDL_CreateTextureFromSurface(Screen.Renderer, surface);
            SDL_FreeSurface(surface);
            return texture;
        }

        public static IntPtr DrawText(string text, string fontFileTTF, byte r, byte g, byte b, byte a)
        {

            IntPtr font = SDL_ttf.TTF_OpenFont(fontFileTTF, 40); 

            SDL_Color color;  
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = a;

            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, text, color);

            IntPtr message = SDL_CreateTextureFromSurface(Screen.Renderer, surfaceMessage); 

            SDL_FreeSurface(surfaceMessage);
            return message;
        }

    }
}
