﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static SDL2.SDL;


namespace C_Sharp_Final_Project
{
    class Screen
    {
        public delegate void TaskCallback();

        public const int FPS = 60;
        public static int frameDelay = 1000 / FPS;

        public static ulong frameStart = 0;
        public static int frameTime = 0;

        private IntPtr Window;

        public static List<Button> Buttons = new List<Button>();
        public static bool IsRunning;
        public static IntPtr Renderer;
        public static int Width, Height;
        private int mouseX, mouseY;

        private static Game game;

        public Screen() { }

        public void Init(string title, int xPos, int yPos, int width, int height)
        {
            SDL_WindowFlags flags = 0;
            Width = width;
            Height = height;

            if (SDL_Init(SDL_INIT_EVERYTHING) == 0)
            {
                Window = SDL_CreateWindow(title, xPos, yPos, width, height, flags);
                Renderer = SDL_CreateRenderer(Window, -1, 0);
                SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
                IsRunning = true;
            }

            Scene.SetUpScene("Scenes/main menu.txt");
        }

        public void HandleEvents()
        {
            SDL_Event events;
            SDL_PollEvent(out events);
            SDL_GetMouseState(out mouseX, out mouseY);

            switch (events.type)
            {
                case SDL_EventType.SDL_QUIT: IsRunning = false; break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    foreach(Button button in Buttons)
                    {
                        if (Component.BoundaryCheck(button.boundary[0], button.boundary[1], mouseX, mouseY))
                        {
                            button.Clicked();
                            break;
                        }
                    }
                    break;
                default: IsRunning = true; break;
            }
        }

        public static void StartGame()
        {
            game = new Game();
            game.Init();

            while (game.Running() && IsRunning)
            {
                frameStart = SDL_GetTicks();

                game.HandleEvents();
                game.Update();
                game.Render();

                frameTime = Convert.ToInt32(SDL_GetTicks() - frameStart);
                if (frameDelay > frameTime)
                    SDL_Delay(Convert.ToUInt32(frameDelay - frameTime));
            }

            game.Clean();
        }

        public void Update()
        {
            if (!IsRunning)
                Clean();
        }

        public void Render()
        {
            SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
            SDL_RenderClear(Renderer);

            foreach (Button button in Buttons)
                button.Render();
          
            SDL_RenderPresent(Renderer);
        }


        public void Clean()
        {
            SDL_RenderClear(Renderer);
            SDL_DestroyWindow(Window);
            SDL_DestroyRenderer(Renderer);
            SDL_Quit();
        }

        public bool Running()
        {
            return IsRunning;
        }
    }
}
