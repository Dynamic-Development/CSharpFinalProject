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
        private Enemy enemy;
        private Player player;
        public bool[] playermover = new bool[4];

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
            enemy = new Enemy(100, 100, 32, 32, "Textures/Test2.png");
            player = new Player(200, 100, 56, 36, "Textures/Player.png");
        }

        private void SetUpNextLevel(string levelFilePath) //Calls in Update
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
                case SDL_EventType.SDL_KEYDOWN:
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_w)
                        playermover[0] = true;
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_a)
                        playermover[1] = true;
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_s)
                        playermover[2] = true;
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_d)
                        playermover[3] = true;
                    break;
                case SDL_EventType.SDL_KEYUP:
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_w)
                        playermover[0] = false;
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_a)
                        playermover[1] = false;
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_s)
                        playermover[2] = false;
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_d)
                        playermover[3] = false;
                    break;
                default:
                    isRunning = true;
                    break;

            }
        }

        public void Update()
        {
            if (playermover[0] == true)
            {
                player.yvel--;
            }
            if (playermover[1] == true)
            {
                player.xvel--;
            }
            if (playermover[2] == true)
            {
                player.yvel++;
            }
            if (playermover[3] == true)
            {
                player.xvel++;
            }

            //Update Objects
            enemy.Update();
            player.Update();
            
        }

        public void Render()
        {
            SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
            SDL_RenderClear(Renderer);

            //Render Objects
            enemy.Render();
            player.Render();

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
