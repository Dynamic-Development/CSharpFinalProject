using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace C_Sharp_Final_Project
{
    class Scene
    {
        public static void SetUpScene(string sceneFilePath)
        {
            string[] sceneDecode = File.ReadAllLines(sceneFilePath);
            foreach (string line in sceneDecode)
                DecodeLineToGame(line);
        }
        private static void DecodeLineToGame(string line)
        {
            line = line.Replace(" ", string.Empty);
            int[] tempIndexes;

            switch (line[0])
            {
                case 'L':// Draw Level
                    line = line.Substring(2);
                    tempIndexes = line.Split('x').Select(x => int.Parse(x)).ToArray();
                    Game.Grid = new Grid(Game.Width, Game.Height, tempIndexes[0], tempIndexes[1]);
                    break;
                case 'W': //Draw Walls
                    line = line.Substring(2);
                    tempIndexes = line.Split(',').Select(x => int.Parse(x)).ToArray();
                    Game.Walls.Add(new Tile(tempIndexes[0], tempIndexes[1], tempIndexes[2], tempIndexes[3], false));
                    List<Node> unwalkableNodes = Game.Grid.TileNodes(tempIndexes[0], tempIndexes[1], tempIndexes[2], tempIndexes[3], 1);
                    foreach (Node node in unwalkableNodes)
                        node.walkable = false;
                    break;
                case 'E': //Draw Enemy
                    line = line.Substring(2);
                    tempIndexes = line.Split(',').Select(x => int.Parse(x)).ToArray();
                    if (Game.Grid.ConvertTileUnitsIntoPixels(tempIndexes[0], tempIndexes[1]) != null)
                    {
                        Game.Enemy.Add(new Enemy(Game.Grid.ConvertTileUnitsIntoPixels(tempIndexes[0], tempIndexes[1]).GetValueOrDefault(), 
                            32, 32, "Textures/Test2.png"));
                    } else
                    {
                        Console.WriteLine("Enemy out of bound: " + tempIndexes[0] + "," + tempIndexes[1] + ";" + 
                            Game.Grid.numTileWidth + "," + Game.Grid.numTileHeight);
                    }

                    break;
                case 'P': //Draw Player
                    line = line.Substring(2);
                    tempIndexes = line.Split(',').Select(x => int.Parse(x)).ToArray();
                    if (Game.Grid.ConvertTileUnitsIntoPixels(tempIndexes[0], tempIndexes[1]) != null)
                    {
                        Game.Player = new Player(Game.Grid.ConvertTileUnitsIntoPixels(tempIndexes[0], tempIndexes[1]).GetValueOrDefault(),
                            56, 36, "Textures/Player.png");
                    }
                    else
                    {
                        Console.WriteLine("Player out of bound: " + tempIndexes[0] + "," + tempIndexes[1] + ";" +
                            Game.Grid.numTileWidth + "," + Game.Grid.numTileHeight);
                    }

                    break;
                default:
                    Console.WriteLine("Unrecognize symbol: " + line[0]);
                    break;
            }
        }
        /*  IMPORTANT:
            ALL PROPERTIES are in TILES UNITS not actual PIXEL UNITS
            
            Start drawing level: L> widthxheight *MUST INCLUDE IN FILE. Preferably 30x20. THIS IS TILE UNITS.
            Drawing a wall: W> topleftx, toplefty, bottomrightx, bottomrighty
            Drawing an enemy: E> x, y
            Drawing a player: P> x, y
        */
    }
}
