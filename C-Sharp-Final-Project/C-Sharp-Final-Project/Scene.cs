using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                case 'L':
                    line = line.Substring(2);
                    tempIndexes = line.Split('x').Select(x => int.Parse(x)).ToArray();
                    Game.Grid = new Grid(Game.Width, Game.Height, tempIndexes[0], tempIndexes[1]);
                   
                    break;
                case 'W':
                    line = line.Substring(2);
                    tempIndexes = line.Split(',').Select(x => int.Parse(x)).ToArray();
                    Game.Walls.Add(new Tile(tempIndexes[0], tempIndexes[1], tempIndexes[2], tempIndexes[3], false));
                    List<Node> unwalkableNodes = Game.Grid.TileNodes(tempIndexes[0], tempIndexes[1], tempIndexes[2], tempIndexes[3], 1);
               
                    foreach (Node node in unwalkableNodes)
                        node.walkable = false;

                    break;
                default:
                    break;
            }
        }
        //Start drawing level: L> widthxheight *MUST INCLUDE IN FILE. Preferably 30x20
        //Drawing a wall: W> topleftx, toplefty, bottomrightx, bottomrighty
        //x, y, width, height properties are in TILES UNITS not actual PIXEL UNITS
    }
}
