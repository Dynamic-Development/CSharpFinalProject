using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Game
    {
        private bool isRunning;
        private IntPtr window;
        public static IntPtr renderer;

        public Game(){}

        public void Init(string title, int xpos, int ypos, int width, int height)
        {
            SDL_WindowFlags flags = 0;
            if (SDL_Init(SDL_INIT_EVERYTHING) == 0)
            {
                window = SDL_CreateWindow(title, xpos, ypos, width, height, flags);
                renderer = SDL_CreateRenderer(window, -1, 0);
                SDL_SetRenderDrawColor(renderer, 200, 200, 50, 90);
                isRunning = true;
            }

        }

        public void HandleEvents()
        {
            SDL_Event events;
            SDL_PollEvent(out events);
            switch (events.type)
            {
                case SDL_EventType.SDL_QUIT:
                    isRunning = false;
                    break;
                default:
                    isRunning = true;
                    break;

            }

        }

        public void Update()
        {
            //Update Objects
        }

        public void Render()
        {
            SDL_SetRenderDrawColor(renderer, 200, 200, 50, 90);
            SDL_RenderClear(renderer);

            //Render Objects

            SDL_RenderPresent(renderer);
        }

        public void Clean()
        {
            SDL_DestroyWindow(window);
            SDL_DestroyRenderer(renderer);
            SDL_Quit();
        }

        public bool Running()
        {
            return isRunning;
        }
    }
}
