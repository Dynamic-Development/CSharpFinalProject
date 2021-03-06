﻿using System;
using System.Windows;
using static SDL2.SDL;

namespace C_Sharp_Final_Project
{
    class Tile
    {
        private IntPtr[] objTexture;
        private SDL_Rect[] objDests;
        public Vector[] boundary { get; }
        public Vector[][] segments { get; }
        public Vector[] points { get; }
        public int level; // 1 is wall; 2 is floor; 3 is screen; 5 is goal
        public Vector corner;
        public int width, height;

        public Tile(int fromXTile, int fromYTile, int toXTile, int toYTile, int level)
        {
            int widthTiles = toXTile - fromXTile;
            int heightTiles = toYTile - fromYTile;

            width = (widthTiles + 1) * Game.Grid.tileWidth;
            height = (heightTiles + 1) * Game.Grid.tileHeight;

            
            this.level = level;

            if (level == 5)
            {
                objTexture = new IntPtr[1];
                objTexture[0] = Texture.LoadTexture("Textures/WinPenent.png");
            }

            boundary = new Vector[] {new Vector(Game.Grid.tileWidth * fromXTile, Game.Grid.tileHeight * fromYTile),
                                    new Vector(Game.Grid.tileWidth * (1 + toXTile), Game.Grid.tileHeight * (1 + toYTile))};

            corner = boundary[0];

            points = new Vector[4];
            points[0] = boundary[0];
            points[1] = new Vector(boundary[1].X, boundary[0].Y);
            points[2] = boundary[1];
            points[3] = new Vector(boundary[0].X, boundary[1].Y);

            segments = new Vector[4][];
            segments[0] = new Vector[] { points[0], points[1] };
            segments[1] = new Vector[] { points[1], points[2] };
            segments[2] = new Vector[] { points[2], points[3] };
            segments[3] = new Vector[] { points[3], points[0] };

            objDests = new SDL_Rect[(widthTiles + 1) * (heightTiles + 1)];

            if (level == 5)
            {
                int tempIndex = 0;
                for (int x = fromXTile; x <= toXTile; x++)
                {
                    for (int y = fromYTile; y <= toYTile; y++)
                    {
                        objDests[tempIndex].w = Game.Grid.tileWidth;
                        objDests[tempIndex].h = Game.Grid.tileHeight;
                        objDests[tempIndex].x = Game.Grid.tileWidth * x;
                        objDests[tempIndex].y = Game.Grid.tileHeight * y;
                        tempIndex++;
                    }
                }
            } else if (level == 1)
            {
                objTexture = new IntPtr[(widthTiles + 1) * (heightTiles + 1)];
                int tempIndex = 0;
                for (int x = fromXTile; x <= toXTile; x++)
                {
                    for (int y = fromYTile; y <= toYTile; y++)
                    {
                        objTexture[tempIndex] = Texture.LoadTexture(Component.ChooseRandomWallType());
                        objDests[tempIndex].w = Game.Grid.tileWidth;
                        objDests[tempIndex].h = Game.Grid.tileHeight;
                        objDests[tempIndex].x = Game.Grid.tileWidth * x;
                        objDests[tempIndex].y = Game.Grid.tileHeight * y;
                        tempIndex++;
                    }
                }
            }
        }

        public void Render()
        {
            if (level == 1 || 
                level == 2 || 
                level == 5)
            {
                for (int i = 0; i < objDests.Length; i++)
                {
                    SDL_RenderCopy(Screen.Renderer, objTexture[i], IntPtr.Zero, ref objDests[i]);
                }
            }
            
        }
    }
}
