using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Enums;

namespace UnfathomableMaze.Control
{
    public class nMaze
    {
        private Tile[,] TileMap { get; } // We use this getter in the MazeScene as the map to validate walls and player interation
        public nMaze(IMapTilesGenerator mapTilesGenerator)
        {
            TileMap = mapTilesGenerator.Generate();
        }

        // Tile Matrix -> Char Matrix
        public char[,] ConvertMap()
        {
            int width = TileMap.GetLength(0);
            int height = TileMap.GetLength(1);
            char[,] charMap = new char[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (TileMap[i, j] == Tile.Wall)
                    {
                        bool north = ValidateNorth(TileMap, i, j);
                        bool south = ValidateSouth(TileMap, i, j);
                        bool west = ValidateWest(TileMap, i, j);
                        bool east = ValidateEast(TileMap, i, j);

                        // Corners
                        if (south && east && !north && !west) charMap[i, j] = '┌';
                        else if (south && west && !north && !east) charMap[i, j] = '┐';
                        else if (north && east && !south && !west) charMap[i, j] = '└';
                        else if (north && west && !south && !east) charMap[i, j] = '┘';

                        // 3 Way Intersections
                        else if (west && east && south && !north) charMap[i, j] = '┬';
                        else if (west && east && north && !south) charMap[i, j] = '┴';
                        else if (north && south && east && !west) charMap[i, j] = '├';
                        else if (north && south && west && !east) charMap[i, j] = '┤';

                        // Cross
                        else if (north && south && west && east) charMap[i, j] = '┼';

                        // Straights
                        else if (north || south) charMap[i, j] = '│';
                        else charMap[i, j] = '─';
                    }
                    else
                    {
                        charMap[i, j] = ' ';
                    }
                }
            }
            return charMap;
        }

        // Wall validation
        public bool ValidateNorth(Tile[,] map, int y, int x)
        {
            if (y > 0)
            {
                if (map[y - 1, x] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }

        // Wall validation
        public bool ValidateEast(Tile[,] map, int y, int x)
        {
            if (x < map.GetLength(1) - 1)
            {
                if (map[y, x + 1] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }

        // Wall validation
        public bool ValidateSouth(Tile[,] map, int y, int x)
        {
            if (y < map.GetLength(0) - 1)
            {
                if (map[y + 1, x] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }

        // Wall validation
        public bool ValidateWest(Tile[,] map, int y, int x)
        {
            if (x > 0)
            {
                if (map[y, x - 1] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }
    }
}
