using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Game
    {
        private bool isRunning;
        private IntPtr window;

        public static IntPtr Renderer;
        public static Grid Grid;
        public static int Width;
        public static int Height;
        public static PathMaster PathManager;

        public static bool[] KeyStates = new bool[4];
        public static List<Tile> Walls = new List<Tile>();
        public static List<Enemy> Enemy;
        public static Player Player;

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
            PathManager = new PathMaster();
            Player = new Player(400, 210, 56, 36, "Textures/Player.png");

            Enemy = new List<Enemy>
            {
                new Enemy(new Vector(560, 434), 32, 32, "Textures/Test2.png"),
                new Enemy(new Vector(200, 200), 32, 32, "Textures/Test2.png")
            };
            
            Scene.SetUpScene("Scenes/level1.txt");
        }

        public void HandleEvents()
        {
            SDL_Event events;
            SDL_PollEvent(out events);
            switch (events.type)
            {
                case SDL_EventType.SDL_QUIT: isRunning = false; break;
                case SDL_EventType.SDL_KEYDOWN:
                    switch (events.key.keysym.sym) {
                        case SDL_Keycode.SDLK_w: KeyStates[0] = true; break;
                        case SDL_Keycode.SDLK_a: KeyStates[1] = true; break;
                        case SDL_Keycode.SDLK_s: KeyStates[2] = true; break;
                        case SDL_Keycode.SDLK_d: KeyStates[3] = true; break;
                    }
                    break;
                case SDL_EventType.SDL_KEYUP:
                    switch (events.key.keysym.sym)
                    {
                        case SDL_Keycode.SDLK_w: KeyStates[0] = false; break;
                        case SDL_Keycode.SDLK_a: KeyStates[1] = false; break;
                        case SDL_Keycode.SDLK_s: KeyStates[2] = false; break;
                        case SDL_Keycode.SDLK_d: KeyStates[3] = false; break;
                    }
                    break;
                default: isRunning = true; break;
            }
        }

        public void Update()
        {
            //Update Objects
            Player.Update();
            for (int e = 0; e < Enemy.Count; e++)
                Enemy[e].Update();
            
            PathManager.TryProcessNext(Component.CoolDown(ref PathManager.nextPathCoolDown, 1));
        }

        public void Render()
        {

            SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
            SDL_RenderClear(Renderer);
            
            //Render Objects
           // Grid.RenderNodes();
            Player.Render();
            for (int e = 0; e < Enemy.Count; e++)
                Enemy[e].Render();
            for (int i = 0; i < Walls.Count; i++)
                Walls[i].Render();

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
