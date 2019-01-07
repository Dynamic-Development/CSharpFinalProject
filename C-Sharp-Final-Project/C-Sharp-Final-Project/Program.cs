using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            const int FPS = 60;
            const int frameDelay = 1000 / FPS;

            uint frameStart = 0;
            int frameTime = 0;

            game.Init("2D Game Engine", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 900, 600);

            while (game.Running())
            {
                frameStart = SDL_GetTicks();

                game.HandleEvents();
                game.Update();
                game.Render();

                frameTime = System.Convert.ToInt32(SDL_GetTicks() - frameStart);
                if (frameDelay > frameTime)
                    SDL_Delay(System.Convert.ToUInt32(frameDelay - frameTime));
            }

            game.Clean();
        }
    }
}
