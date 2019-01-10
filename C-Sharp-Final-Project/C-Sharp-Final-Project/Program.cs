using System.Runtime.InteropServices;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Program
    {
        static void Main(string[] args)
        {

            Screen main = new Screen();

            main.Init("2D Top Down Shooter", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 900, 600);

            while (main.Running())
            {
                Screen.frameStart = SDL_GetTicks();

                main.HandleEvents();
                main.Update();
                main.Render();

                try
                {
                    Screen.frameTime = System.Convert.ToInt32(SDL_GetTicks() - Screen.frameStart);
                } catch (System.OverflowException)
                {
                    main.Clean();
                }

                if (Screen.frameDelay > Screen.frameTime)
                    SDL_Delay(System.Convert.ToUInt32(Screen.frameDelay - Screen.frameTime));
            }
            main.Clean();
        }
    }
}



