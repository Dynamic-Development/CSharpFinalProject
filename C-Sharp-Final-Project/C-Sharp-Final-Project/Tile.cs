using System;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Tile
    {
        private IntPtr objTexture;
        private SDL_Rect[] objDests;
        public Tile(int fromXTile, int fromYTile, int widthTile, int heightTile, bool walkable)
        {
            if (!walkable) objTexture = Textures.LoadTexture("Textures/Test2.png");

            objDests = new SDL_Rect[widthTile * heightTile];
            int tempIndex = 0;
            for (int x = fromXTile; x < widthTile + fromXTile; x++)
                for (int y = fromYTile; y < heightTile + fromYTile; y++)
                {
                    objDests[tempIndex].w = Game.Grid.tileWidth;
                    objDests[tempIndex].h = Game.Grid.tileHeight;
                    objDests[tempIndex].x = Game.Grid.tileWidth * x;
                    objDests[tempIndex].y = Game.Grid.tileHeight * y;
                    tempIndex++;
                }
        }
        public void Render()
        {
            for (int i = 0; i < objDests.Length; i++)
            {
                SDL_RenderCopy(Game.Renderer, objTexture, IntPtr.Zero, ref objDests[i]);
            }
        }
    }
}
