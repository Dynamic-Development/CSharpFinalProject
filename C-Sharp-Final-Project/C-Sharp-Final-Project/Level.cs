using System.IO;
using System.Linq;

namespace C_Sharp_Final_Project
{
    class Level
    {
        private string[] levelDecode;
        public Grid grid;
        public Level(string levelFilePath)
        {
            levelDecode = File.ReadAllLines(levelFilePath);
            foreach (string line in levelDecode)
                DecodeLineToGame(line);
        }
        private void DecodeLineToGame(string line)
        {
            line = line.Replace(" ", string.Empty);
            int[] tempIndexes;
            switch (line[0])
            {
                case 'D':
                    line = line.Substring(2);
                    tempIndexes = line.Split('x').Select(x => int.Parse(x)).ToArray();
                    grid = new Grid(Game.Width, Game.Height, tempIndexes[0], tempIndexes[1]);
                    break;
                case 'W':
                    line = line.Substring(2);
                    tempIndexes = line.Split(',').Select(x => int.Parse(x)).ToArray();
                    //TODO: Create wall objects
                    Node[] unwalkableNodes = grid.GroupNodesInArea(tempIndexes[0], tempIndexes[1], tempIndexes[2], tempIndexes[3]);
                    foreach (Node node in unwalkableNodes)
                        node.walkable = false;
                    break;
                default:
                    break;
            }
        }
        //Start drawing: D> widthxheight *MUST INCLUDE IN FILE
        //Drawing a wall: W> topleftx, toplefty, width, height
        //x, y, width, height properties are in TILES UNITS not actual PIXEL UNITS
    }
}
