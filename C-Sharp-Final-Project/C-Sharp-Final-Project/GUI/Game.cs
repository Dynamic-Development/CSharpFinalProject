using System;
using System.Windows;
using System.Collections.Generic;
using static SDL2.SDL;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace C_Sharp_Final_Project
{
    class Game
    {
        private bool isGameRunning;
        public static Grid Grid;
        public static Pathmaster Pathmanager;
        
        public bool win;
        public static bool nextLevelPend;
        public static List<Tile> Walls;
        public static List<Enemy> Enemies;
        public static List<Bullet> Bullets;
        public static Player Player;
        public static int mousePosX;
        public static int mousePosY;
        
        private readonly int numkeys;
        private readonly IntPtr keysBuffer;
        private readonly byte[] keysCurr = new byte[(int)SDL_Scancode.SDL_NUM_SCANCODES];
        private readonly byte[] keysPrev = new byte[(int)SDL_Scancode.SDL_NUM_SCANCODES];

        public static bool playerDeath;
        private Stopwatch sw;
        public double score;
        private int currentLevelIndex;
        private string[] levels;

        public Game()
        {
            isGameRunning = true;
            keysBuffer = SDL_GetKeyboardState(out numkeys);
        }
        
        public void Init(string startLevel, string[] levelScenes)
        {  
            Walls = new List<Tile>();
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();

            Grid = new Grid(Screen.Width, Screen.Height, 30, 20);
            Walls.Add(new Tile(0, 0, Grid.numTileWidth, Grid.numTileHeight, 3));

            levels = levelScenes;
            nextLevelPend = false;
            win = false;
            currentLevelIndex = Array.IndexOf(levelScenes, startLevel);
            Scene.SetUpSceneLevel(startLevel);
            playerDeath = false;

            sw = new Stopwatch();
            sw.Start();

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
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_ESCAPE)
                        isGameRunning = false; 
                    break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    Player.Shoot();
                    break;
                default: Screen.IsRunning = true; isGameRunning = true; break;
            }

            if (keysCurr[(int)SDL_Scancode.SDL_SCANCODE_W] == 1)
                Player.velocity.Y--;
            if (keysCurr[(int)SDL_Scancode.SDL_SCANCODE_A] == 1)
                Player.velocity.X--;
            if (keysCurr[(int)SDL_Scancode.SDL_SCANCODE_S] == 1)
                Player.velocity.Y++;
            if (keysCurr[(int)SDL_Scancode.SDL_SCANCODE_D] == 1)
                Player.velocity.X++;

            Marshal.Copy(keysBuffer, keysCurr, 0, numkeys);

            SDL_GetMouseState(out mousePosX, out mousePosY);
        }

        public void Win()
        {
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            score = 1000 - ts.TotalMinutes * 100;
            win = true;
        }

        public bool NextLevel()
        {
            currentLevelIndex++;
            if (currentLevelIndex < levels.Length)
            {
                Walls = new List<Tile>();
                Enemies = new List<Enemy>();
                Bullets = new List<Bullet>();
                Grid = new Grid(Screen.Width, Screen.Height, 30, 20);
                Walls.Add(new Tile(0, 0, Grid.numTileWidth, Grid.numTileHeight, 3));
                Scene.SetUpSceneLevel(levels[currentLevelIndex]);
                return false;
            }
            Win();
            return true;
        }

        public void Update()
        {
            //Update Objects
            if (nextLevelPend)
            {
                nextLevelPend = false;
                if (NextLevel())
                    return;
            }

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

            Grid.RenderNodes();

            for (int b = 0; b < Bullets.Count; b++)
                Bullets[b].Render();
            for (int i = 0; i < Walls.Count; i++)
                Walls[i].Render();
            for (int e = 0; e < Enemies.Count; e++)
                Enemies[e].Render();
                                    
            Player.Render();

            SDL_RenderPresent(Screen.Renderer);
        }

        public bool isPlayerDeadYet()
        {
            return playerDeath;
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
