using System.Drawing;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;

namespace UnfathomableMaze.Services
{
    public class MapGenerator : IMapTilesGenerator
    {
        private readonly int _width = 51;    // IMPORTANT: The size of the maze must ALWAYS be made by 2 odd numbers
        private readonly int _height = 51;
        /// <summary>
        /// DETAIL: The number 51 means that the maze is composed by 49 real tiles.
        /// The borders of the matrix will always be walls. This leaves us with 2 tiles less for each axis.
        /// This means that if the player starts at position 1,1 (not 0,0 bc its wall) the finish point will be at index 49, 49.
        ///</summary>
        private readonly Random _random = new();


        public Tile[,] Generate()       // Function that initializates and generates the maze
        {
            Tile[,] map = new Tile[_width, _height];        // Creation of the matrix

            for (int x = 0; x < _width; x++)             // Fully initializated with walls
            {
                for (int y = 0; y < _height; y++)
                {
                    map[x, y] = Tile.Wall;
                }
            }

            Stack<Point> stack = new Stack<Point>(); // Stack of positions the "ant" carved into the maze
            int startX = 1;         // Start position of the player in the maze
            int startY = 1;         // Can be changed but MUST ALWAYS be composed by two odd numbers.

            map[startX, startY] = Tile.Path;       // Start position is turned into a path and pushed into the stack
            stack.Push(new Point(startX, startY));          // This begins the "carving"

            while (stack.Count > 0)
            {
                var current = stack.Peek();     // Buffer for the current tile
                var neighbors = GetUnvisitedNeighbors(current.X, current.Y, map); // Buffer for the current list of neighbors. The var type is list.

                if (neighbors.Count > 0)
                {
                    var next = neighbors[_random.Next(neighbors.Count)]; // If there are neighbors the ant chooses one randomly and goes there.

                    map[next.X, next.Y] = Tile.Path;    // Turns the chosen neighbor and turns it into walkable path.
                    map[current.X + (next.X - current.X) / 2, current.Y + (next.Y - current.Y) / 2] = Tile.Path; // Turns the space between the current tile and the chosen into path.

                    stack.Push(next);   // The stack is updated and advances to the next tile.
                }
                else
                {
                    stack.Pop();    // If no neighbors are left the stack goes backwards and keeps checking for neighbors.
                }
            }

            return map;
        }

        private List<Point> GetUnvisitedNeighbors(int x, int y, Tile[,] map)
        {
            List<Point> neighbors = new List<Point>();    // Generates a list with all the posible options the ant can take in the next step.

            if (x > 2 && map[x - 2, y] == Tile.Wall) neighbors.Add(new Point(x - 2, y)); // Checks for the limit of the matrix at the left and neighbors at the left of X 
            if (x < _width - 2 && map[x + 2, y] == Tile.Wall) neighbors.Add(new Point(x + 2, y)); // Checks for the limit of the matrix at the right and neighbors at the right of X 
            if (y > 2 && map[x, y - 2] == Tile.Wall) neighbors.Add(new Point(x, y - 2)); // Checks for the limit of the matrix at the top and neighbors at the top of Y
            if (y < _height - 2 && map[x, y + 2] == Tile.Wall) neighbors.Add(new Point(x, y + 2)); // Checks for the limit of the matrix at the bot and neighbors at the bot of Y

            return neighbors;
        }
    }
}
