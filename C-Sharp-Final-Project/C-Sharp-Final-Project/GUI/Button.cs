using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Button
    {
        private Screen.TaskCallback ActivateFunction;
        private SDL_Rect dest;
        private IntPtr objTexture;
        public Vector[] boundary { get; }

        public Button(int width, int height, int x, int y, string texture, Screen.TaskCallback activateFunctionWhenClicked)
        {
            dest.w = width;
            dest.h = height;
            dest.x = x - width / 2;
            dest.y = y - height / 2;

            objTexture = Texture.LoadTexture(texture);
            boundary = new Vector[2] {new Vector(dest.x, dest.y), new Vector(dest.x + width, dest.y + height) };

            ActivateFunction = activateFunctionWhenClicked;
        }

        public void Clicked()
        {
            ActivateFunction();
        }

        public void Render()
        {
            SDL_RenderCopy(Screen.Renderer, objTexture, IntPtr.Zero, ref dest);
        }
    }

}
