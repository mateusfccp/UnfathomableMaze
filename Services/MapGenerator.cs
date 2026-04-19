using System;
using System.Collections.Generic;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;

namespace UnfathomableMaze.Services
{
    public class MapGenerator : IMapTilesGenerator
    {
        private readonly int width = 51;    // IMPORTANT: The size of the maze must ALWAYS be made by 2 odd numbers
        private readonly int height = 51;
        // DETAIL: The number 51 means that the maze is composed by 49 real tiles.
        /*
        The borders of the matrix will always be walls. This leaves us with 2 tiles less for each axis.
        This means that if the player starts at position 1,1 (not 0,0 bc its wall) the finish point will be at index 49, 49.
        */
        private readonly Random random = new Random();

        // MATI USA ESTE METODO GENERATE PARA OBTENER EL LABERINTO GENERADO. LA POSICION INICIAL VA A SER 1,1 Y LA FINAL SE SUPONE QUE ES LA ESQUINA OPUESTA.
        public Tile[,] Generate()       // Function that initializates and generates the maze
        {
            Tile[,] map = new Tile[width, height];        // Creation of the matrix

            for (int x = 0; x < width; x++)             // Fully initializated with walls
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = Tile.Wall;
                }
            }

            Stack<(int x, int y)> stack = new Stack<(int x, int y)>(); // Stack of positions the "ant" carved into the maze
            int startX = 1;         // Start position of the player in the maze
            int startY = 1;         // Can be changed but MUST ALWAYS be composed by two odd numbers.

            map[startX, startY] = Tile.Path;       // Start position is turned into a path and pushed into the stack
            stack.Push((startX, startY));          // This begins the "carving"

            while (stack.Count > 0)
            {
                var current = stack.Peek();     // Buffer for the current tile
                var neighbors = GetUnvisitedNeighbors(current.x, current.y, map); // Buffer for the current list of neighbors. The var type is list.

                if (neighbors.Count > 0)
                {
                    var next = neighbors[random.Next(neighbors.Count)]; // If there are neighbors the ant chooses one randomly and goes there.

                    map[next.x, next.y] = Tile.Path;    // Turns the chosen neighbor and turns it into walkable path.
                    map[current.x + (next.x - current.x) / 2, current.y + (next.y - current.y) / 2] = Tile.Path; // Turns the space between the current tile and the chosen into path.

                    stack.Push(next);   // The stack is updated and advances to the next tile.
                }
                else
                {
                    stack.Pop();    // If no neighbors are left the stack goes backwards and keeps checking for neighbors.
                }
            }

            return map;
        }

        private List<(int x, int y)> GetUnvisitedNeighbors(int x, int y, Tile[,] map)
        {
            List<(int x, int y)> neighbors = new List<(int x, int y)>();    // Generates a list with all the posible options the ant can take in the next step.

            if (x > 2 && map[x - 2, y] == Tile.Wall) neighbors.Add((x - 2, y)); // Checks for the limit of the matrix at the left and neighbors at the left of X 
            if (x < width - 2 && map[x + 2, y] == Tile.Wall) neighbors.Add((x + 2, y)); // Checks for the limit of the matrix at the right and neighbors at the right of X 
            if (y > 2 && map[x, y - 2] == Tile.Wall) neighbors.Add((x, y - 2)); // Checks for the limit of the matrix at the top and neighbors at the top of Y
            if (y < height - 2 && map[x, y + 2] == Tile.Wall) neighbors.Add((x, y + 2)); // Checks for the limit of the matrix at the bot and neighbors at the bot of Y

            return neighbors;
        }
    }
}