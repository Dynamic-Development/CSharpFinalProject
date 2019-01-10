using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;
using System.Runtime.InteropServices;

namespace C_Sharp_Final_Project
{
    class Game
    {
        private bool isGameRunning;
        public static Grid Grid;
        public static Pathmaster Pathmanager;

        public static bool[] KeyStates = new bool[4];
        public static List<Tile> Walls;
        public static List<Enemy> Enemies;
        public static List<Bullet> Bullets;
        public static Player Player;
        public static int mousePosX;
        public static int mousePosY;
        /*
        private static int _numkeys;
        private static IntPtr _keysBuffer;
        private static byte[] _keysCurr = new byte[(int)SDL_Scancode.SDL_NUM_SCANCODES];
        private static byte[] _keysPrev = new byte[(int)SDL_Scancode.SDL_NUM_SCANCODES];
        */
        public Game(){}
        
        public void Init()
        {   /*
            var tmp = _keysPrev;
            _keysPrev = _keysCurr;
            _keysCurr = tmp;
            // copy new state
            
            if (_keysCurr[(int) SDL_Scancode.SDL_SCANCODE_W] == 1)
                

            Marshal.Copy(_keysBuffer, _keysCurr, 0, _numkeys);
            */
            isGameRunning = true;
            Walls = new List<Tile>();
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();

            Scene.SetUpScene("Scenes/level1.txt");
            Pathmanager = new Pathmaster();
        }

        public void HandleEvents()
        {
            SDL_Event events;
            SDL_PollEvent(out events);
            switch (events.type)
            {
                case SDL_EventType.SDL_QUIT: Screen.IsRunning = false; break;
                case SDL_EventType.SDL_KEYDOWN:
                    switch (events.key.keysym.sym) {
                        case SDL_Keycode.SDLK_ESCAPE: isGameRunning = false; break;
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
                default: Screen.IsRunning = true; isGameRunning = true; break;
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
            
            Pathmanager.TryProcessNext(Component.CoolDown(ref Pathmanager.nextPathCoolDown, 3, true));
        }

        public void Render()
        {
            SDL_SetRenderDrawColor(Screen.Renderer, 200, 200, 50, 90);
            SDL_RenderClear(Screen.Renderer);
            //Render Objects
           // Grid.RenderNodes();
            Player.Render();
            for (int e = 0; e < Enemies.Count; e++)
                Enemies[e].Render();
            for (int i = 0; i < Walls.Count; i++)
                Walls[i].Render();
            for (int b = 0; b < Bullets.Count; b++)
                Bullets[b].Render();

            SDL_RenderPresent(Screen.Renderer);
        }

        public void Clean()
        {
            Grid = null;
            Walls = null;
            Player = null;
            Enemies = null;
            Bullets = null;
        }

        public bool Running()
        {
            return isGameRunning;
        }
    }
}
