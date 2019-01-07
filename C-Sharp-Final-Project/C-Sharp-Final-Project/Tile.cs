using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Tile
    {
        private IntPtr objTexture;
        private SDL_Rect[] objDests;
        public int[] boundary { get; }
        public Vector[][] segments { get; }
        public Vector[] points { get; }

        public Tile(int fromXTile, int fromYTile, int toXTile, int toYTile, bool walkable)
        {
            int widthTiles = toXTile - fromXTile;
            int heightTiles = toYTile - fromYTile;

            boundary = new int[] {Game.Grid.tileWidth * fromXTile,
                                  Game.Grid.tileHeight * fromYTile,
                                  Game.Grid.tileWidth * toXTile,
                                  Game.Grid.tileHeight * toYTile};

            points = new Vector[4];
            points[0] = new Vector(boundary[0], boundary[1]);
            points[1] = new Vector(boundary[2], boundary[1]);
            points[2] = new Vector(boundary[2], boundary[3]);
            points[3] = new Vector(boundary[0], boundary[3]);

            segments = new Vector[4][];
            segments[0] = new Vector[] {points[0], points[1]};
            segments[1] = new Vector[] {points[1], points[2]};
            segments[2] = new Vector[] {points[2], points[3]};
            segments[3] = new Vector[] {points[3], points[0]};

            if (!walkable) objTexture = Texture.LoadTexture("Textures/Test2.png");
            
            objDests = new SDL_Rect[widthTiles * heightTiles];

            int tempIndex = 0;
            for (int x = fromXTile; x < toXTile; x++)
            {
                for (int y = fromYTile; y < toYTile; y++)
                {
                    objDests[tempIndex].w = Game.Grid.tileWidth;
                    objDests[tempIndex].h = Game.Grid.tileHeight;
                    objDests[tempIndex].x = Game.Grid.tileWidth * x;
                    objDests[tempIndex].y = Game.Grid.tileHeight * y;
                    tempIndex++;
                }
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
