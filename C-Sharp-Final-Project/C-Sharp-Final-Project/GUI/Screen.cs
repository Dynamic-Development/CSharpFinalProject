using SDL2;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static SDL2.SDL;


namespace C_Sharp_Final_Project
{
    class Screen
    {
        public delegate void TaskCallback();

        public const int FPS = 60;
        public int frameDelay = 1000 / FPS;

        public uint frameStart = 0;
        public int frameTime = 0;

        private IntPtr Window;
        public List<Button> MainButtons = new List<Button>();
        public static bool IsRunning;
        public static IntPtr Renderer;
        public static int Width, Height;
        private int mouseX, mouseY;
        private bool endScene;
        private double finalScore;

        SDL_Rect mrect;
        SDL_Rect scrRect;
        int x, y, w, h;
        IntPtr endScore;
        IntPtr endScreen;

        private string[] levels = new string[] {
            "Scenes/level1.txt",
            "Scenes/level2.txt",
            "Scenes/level3.txt",
            "Scenes/level4.txt",
            "Scenes/level5.txt",
        };

        private static Game game;

        public Screen() {  }

        public void Init(string title, int xPos, int yPos, int width, int height)
        {
            SDL_WindowFlags flags = 0;
            Width = width;
            Height = height;
            endScene = false;

            if (SDL_Init(SDL_INIT_EVERYTHING) == 0)
            {
                SDL_ttf.TTF_Init();
                Window = SDL_CreateWindow(title, xPos, yPos, width, height, flags);
                Renderer = SDL_CreateRenderer(Window, -1, 0);
                SDL_SetRenderDrawColor(Renderer, 200, 200, 50, 90);
                IsRunning = true;
            }

            x = 250;
            y = 500;
            w = 170;
            h = 100;
            mrect.x = x;
            mrect.y = y;
            mrect.w = w;
            mrect.h = h;

            endScreen = Texture.LoadTexture("Textures/EndScreen.png");

            scrRect.x = 0;
            scrRect.y = 0;
            scrRect.w = Width;
            scrRect.h = Height;

            Scene.SetUpSceneScreen("Scenes/main menu.txt", this);
        }

        public void HandleEvents()
        {
            SDL_Event events;
            SDL_PollEvent(out events);
            SDL_GetMouseState(out mouseX, out mouseY);

            switch (events.type)
            {
                case SDL_EventType.SDL_QUIT: IsRunning = false; break;
                case SDL_EventType.SDL_KEYDOWN:
                    if (events.key.keysym.sym == SDL_Keycode.SDLK_ESCAPE)
                        endScene = false;
                        MainButtons.Clear();
                        Scene.SetUpSceneScreen("Scenes/main menu.txt", this);
                    break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    foreach(Button button in MainButtons)
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

        public void StartGame()
        {
            game = new Game();
            game.Init("Scenes/level1.txt", levels);

            while (game.Running() && IsRunning && !game.win && !game.isPlayerDeadYet())
            {
                frameStart = SDL_GetTicks();

                game.HandleEvents();
                game.Update();
                game.Render();

                frameTime = Convert.ToInt32(SDL_GetTicks() - frameStart);
                if (frameDelay > frameTime)
                    SDL_Delay(Convert.ToUInt32(frameDelay - frameTime));
            }
            if (game.win)
            {
                endScene = true;
                finalScore = game.score;
                endScore = Texture.DrawText(Math.Round(finalScore, 2).ToString(), "Fonts/Sans.ttf", 255, 255, 255, 100);
            }
            game.Clean();
        }

        public void LoadLevel(string level)
        {
            game = new Game();
            game.Init("Scenes/" +  level, levels);

            while (game.Running() && IsRunning && !game.win)
            {
                frameStart = SDL_GetTicks();

                game.HandleEvents();
                game.Update();
                game.Render();

                frameTime = Convert.ToInt32(SDL_GetTicks() - frameStart);
                if (frameDelay > frameTime)
                    SDL_Delay(Convert.ToUInt32(frameDelay - frameTime));
            }
            if (game.win)
            {
                endScene = true;
                endScore = Texture.DrawText(finalScore.ToString(), "Fonts/Sans.ttf", 255, 255, 255, 100);
                finalScore = game.score;
            }
            game.Clean();
        }

        public void LevelScreen()
        {
            MainButtons.Clear();
            Scene.SetUpSceneScreen("Scenes/choose level.txt", this);
        }

        public void SelectLevel(string level)
        {
            game = new Game();
            game.Init(level, levels);

            while (game.Running() && IsRunning && !game.win && !game.isPlayerDeadYet())
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
            if (!endScene)
            {
                foreach (Button button in MainButtons)
                    button.Render();
            }
            else
            {
                SDL_RenderCopy(Renderer, endScreen, IntPtr.Zero, ref scrRect);
                SDL_RenderCopy(Renderer, endScore, IntPtr.Zero, ref mrect);
            }
            SDL_RenderPresent(Renderer);
        }


        public void Clean()
        {
            SDL_ttf.TTF_Quit();
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
