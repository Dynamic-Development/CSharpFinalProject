using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Game
    {
        private bool isRunning;
        private IntPtr window;
        private Level currentLevel;
        public static IntPtr Renderer;
        public static Grid Grid;
        public static int Width;
        public static int Height;

        public Game(){}

        public void Init(string title, int xPos, int yPos, int width, int height)
        {
            SDL_WindowFlags flags = 0;
            Width = width;
            Height = height;
            if (SDL_Init(SDL_INIT_EVERYTHING) == 0)
            {
                window = SDL_CreateWindow(title, xPos, yPos, width, height, flags);
                Renderer = SDL_CreateRenderer(window, -1, 0);
                SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
                isRunning = true;
            }  
        }
        public void SetUpNextLevel(string levelFilePath) //Calls in Update
        {
            currentLevel = new Level(levelFilePath);
            Grid = currentLevel.grid;
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
            SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
            SDL_RenderClear(Renderer);

            //Render Objects

            SDL_RenderPresent(Renderer);
        }

        public void Clean()
        {
            SDL_DestroyWindow(window);
            SDL_DestroyRenderer(Renderer);
            SDL_Quit();
        }

        public bool Running()
        {
            return isRunning;
        }
    }
}
