using System;
using System.Collections.Generic;
using System.Text;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Enums;

namespace UnfathomableMaze.Control
{
    public class nMaze
    {
        private Tile[,] mapTiles;
        public nMaze(IMapTilesGenerator mapTilesGenerator)
        {
            mapTiles = mapTilesGenerator.Generate();
        }

        public char[,] ConvertMap()
        {
            int width = mapTiles.GetLength(0);
            int height = mapTiles.GetLength(1);
            char[,] charMap = new char[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (mapTiles[i, j] == Tile.Wall)
                    { 
                        bool north = ValidateNorth(mapTiles, i, j);
                        bool south = ValidateSouth(mapTiles, i, j);
                        bool west = ValidateWest(mapTiles, i, j);
                        bool east = ValidateEast(mapTiles, i, j);

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

        public static bool ValidateNorth(Tile[,] map, int posX, int posY)
        {
            if (posX > 0)
            {
                if (map[posX - 1, posY] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }

        public static bool ValidateEast(Tile[,] map, int posX, int posY)
        {
            if (posY < map.GetLength(1) - 1)
            {
                if (map[posX, posY + 1] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }

        public static bool ValidateSouth(Tile[,] map, int posX, int posY)
        {
            if (posX < map.GetLength(0) - 1)
            {
                if (map[posX + 1, posY] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }

        public static bool ValidateWest(Tile[,] map, int posX, int posY)
        {
            if (posY > 0)
            {
                if (map[posX, posY - 1] == Tile.Wall) return true;
                else return false;
            }
            else return false;
        }
    }
}