﻿using System;
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
        private bool win;

        public static bool[] KeyStates = new bool[4];
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

        private Stopwatch sw;
        IntPtr message;
        SDL_Rect mrect;
        IntPtr font;
        IntPtr loadMessage;
        SDL_Color textColor = new SDL_Color();
        IntPtr winScreen;
        SDL_Rect scrRect;
        
        public Game()
        {
            isGameRunning = true;
            keysBuffer = SDL_GetKeyboardState(out numkeys);

            SDL2.SDL_ttf.TTF_Init();
            textColor.a = 50;
            textColor.g = 255;
            textColor.b = 255;
            textColor.r = 255;
        }
        
        public void Init(string startLevel)
        {  
            Walls = new List<Tile>();
            Enemies = new List<Enemy>();
            Bullets = new List<Bullet>();

            Scene.SetUpSceneLevel(startLevel);

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
            Clean();
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            font = SDL2.SDL_ttf.TTF_OpenFont("Sans.ttf", 28);
            message = SDL2.SDL_ttf.TTF_RenderText_Solid(font, ts.Minutes.ToString(), textColor);
            loadMessage = SDL_CreateTextureFromSurface(Screen.Renderer, message);
            mrect.x = 0;  
            mrect.y = 0; 
            mrect.w = 100;
            mrect.h = 100;
            scrRect.x = 0;
            scrRect.y = 0;
            scrRect.w = Screen.Width;
            scrRect.h = Screen.Height;

            win = true;
            SDL_FreeSurface(message);
            winScreen = Texture.LoadTexture("Textures/winScreen.png");
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
            if (win)
            {
                for (int b = 0; b < Bullets.Count; b++)
                    Bullets[b].Render();
                for (int i = 0; i < Walls.Count; i++)
                    Walls[i].Render();
                for (int e = 0; e < Enemies.Count; e++)
                    Enemies[e].Render();
                Player.Render();
            } else
            {
                SDL_RenderCopy(Screen.Renderer, loadMessage, IntPtr.Zero, ref mrect);
                SDL_RenderCopy(Screen.Renderer, winScreen, IntPtr.Zero, ref scrRect);
            }
            SDL_RenderPresent(Screen.Renderer);
        }

        public void Clean()
        {
            Grid = null;
            Walls = null;
            Player = null;
            Enemies = null;
            Bullets = null;

            SDL2.SDL_ttf.TTF_CloseFont(font);
            SDL2.SDL_ttf.TTF_Quit();
        }

        public bool Running()
        {
            return isGameRunning;
        }
    }
}
