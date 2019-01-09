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
        public static Pathmaster Pathmanager;

        public static bool[] KeyStates = new bool[4];
        public static List<Tile> Walls = new List<Tile>();
        public static List<Enemy> Enemies = new List<Enemy>();
        public static List<Bullet> Bullets = new List<Bullet>();
        public static Player Player;
        public static int mousePosX;
        public static int mousePosY;

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
            Scene.SetUpScene("Scenes/level1.txt");
            Pathmanager = new Pathmaster();

            Player = new Player(new Vector(200, 300), 56, 36, "Textures/Player.png");

            Enemies = new List<Enemy>
            {
                new Enemy(new Vector(200, 100), 32, 32, "Textures/Test2.png"),
                new Enemy(new Vector(200, 500), 32, 32, "Textures/Test2.png")
            };
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
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    Player.Shoot();
                    break;
                default: isRunning = true; break;
            }
            SDL_GetMouseState(out mousePosX, out mousePosY);
        }

        public void Update()
        {
            //Update Objects
            Player.Update();
            for (int e = 0; e < Enemies.Count; e++)
                Enemies[e].Update();
            for (int b = 0; b < Bullets.Count; b++)
                Bullets[b].Update();
            
            Pathmanager.TryProcessNext(Component.CoolDown(ref Pathmanager.nextPathCoolDown, 1));
        }

        public void Render()
        {

            SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
            SDL_RenderClear(Renderer);

            //Render Objects
           // Grid.RenderNodes();
            Player.Render();
            for (int e = 0; e < Enemies.Count; e++)
                Enemies[e].Render();
            for (int i = 0; i < Walls.Count; i++)
                Walls[i].Render();
            for (int b = 0; b < Bullets.Count; b++)
                Bullets[b].Render();
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
