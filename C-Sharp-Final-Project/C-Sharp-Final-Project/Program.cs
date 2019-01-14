using System.Runtime.InteropServices;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Program
    {
        static void Main(string[] args)
        {

            Screen main = new Screen();

            main.Init("Space Mafia - Attack of the Mafia", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 900, 600);

            while (main.Running())
            {
                main.frameStart = SDL_GetTicks();

                main.HandleEvents();
                main.Update();
                main.Render();

                try
                {
                    main.frameTime = System.Convert.ToInt32(SDL_GetTicks() - main.frameStart);
                } catch (System.OverflowException)
                {
                    main.Clean();
                }

                if (main.frameDelay > main.frameTime)
                    SDL_Delay(System.Convert.ToUInt32(main.frameDelay - main.frameTime));
            }
            main.Clean();
        }
    }
}



