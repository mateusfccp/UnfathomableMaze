using System.Diagnostics;
using System.Drawing;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;

namespace UnfathomableMaze.Services;

/// <summary>
/// A generator for a map of tiles.
///
/// This class uses a depth-first search that uses a stack to keep track of the current path. The "ant" starts at a
/// random position and carves a path through the maze until it reaches a dead end. When it reaches a dead end, it
/// backtracks until it finds a new path to carve. This process continues until all paths have been carved and the maze
/// is complete.
///
/// This class generates a maze with a fixed size of 51x51 tiles.
/// </summary>
public class MapGenerator : IMapTilesGenerator
{
    private int Width = 41; // IMPORTANT: The size of the maze must ALWAYS be made by 2 odd numbers
    private int Height = 41;

    /// <summary>
    /// DETAIL: The number 31 means that the maze is composed by 29 real tiles.
    /// The borders of the matrix will always be walls. This leaves us with 2 tiles less for each axis.
    /// This means that if the player starts at position 1,1 (not 0,0 bc its wall) the finish point will be at index 49, 49.
    ///</summary>
    private readonly Random _random = new();

    public MapGenerator() {
        int rand1 = _random.Next(20, 50);
        int rand2 = _random.Next(20, 50);

        if (rand1 % 2 == 0) rand1++;
        if (rand2 % 2 == 0) rand2++;

        Width = rand1;
        Height = rand2;
    }

    public Tile[,] Generate() // Function that initializes and generates the maze
    {
        Debug.Assert(Width % 2 == 1 && Height % 2 == 1, "The size of the maze must be made by 2 odd numbers.");

        var map = new Tile[Width, Height]; // Creation of the matrix

        for (var x = 0; x < Width; x++) // Fully initialized with walls
        {
            for (var y = 0; y < Height; y++)
            {
                map[x, y] = Tile.Wall;
            }
        }

        var stack = new Stack<Point>(); // Stack of positions the "ant" carved into the maze
        const int startX = 21; // Start position of the player in the maze
        const int startY = 21; // Can be changed but MUST ALWAYS be composed by two odd numbers.

        map[startX, startY] = Tile.Path; // Start position is turned into a path and pushed into the stack

        stack.Push(new Point(startX, startY)); // This begins the "carving"

        while (stack.Count > 0)
        {
            var current = stack.Peek(); // Buffer for the current tile

            // Buffer for the current list of neighbors. The var type is list.
            var neighbors = GetUnvisitedNeighbors(current.X, current.Y, map);

            if (neighbors.Count > 0)
            {
                // If there are neighbors, the ant chooses one randomly and goes there.
                var next = neighbors[_random.Next(neighbors.Count)];

                map[next.X, next.Y] = Tile.Path; // Turns the chosen neighbor and turns it into walkable path.

                // Turns the space between the current tile and the chosen into path.
                map[current.X + (next.X - current.X) / 2, current.Y + (next.Y - current.Y) / 2] = Tile.Path;

                stack.Push(next); // The stack is updated and advances to the next tile.
            }
            else
            {
                stack.Pop(); // If no neighbors are left, the stack goes backwards and keeps checking for neighbors.
            }
        }
        AddLoops(map, 0.02); // Turns the simple maze into a braid maze (adds loops)

        return map;
    }

    private List<Point> GetUnvisitedNeighbors(int x, int y, Tile[,] map)
    {
        // Generates a list with all the possible options the ant can take in the next step.
        List<Point> neighbors = [];

        if (x > 2 && map[x - 2, y] == Tile.Wall)
            // Checks for the limit of the matrix at the left and neighbors at the left of X
            neighbors.Add(new Point(x - 2, y));
        if (x < Width - 2 && map[x + 2, y] == Tile.Wall)
            // Checks for the limit of the matrix at the right and neighbors at the right of X
            neighbors.Add(new Point(x + 2, y));
        if (y > 2 && map[x, y - 2] == Tile.Wall)
            // Checks for the limit of the matrix at the top and neighbors at the top of Y
            neighbors.Add(new Point(x, y - 2));
        if (y < Height - 2 && map[x, y + 2] == Tile.Wall)
            // Checks for the limit of the matrix at the bot and neighbors at the bot of Y
            neighbors.Add(new Point(x, y + 2));

        return neighbors;
    }
    private void AddLoops(Tile[,] map, double probability)
    {
        for (int x = 1; x < Width - 1; x++)
        {
            for (int y = 1; y < Height - 1; y++)
            {
                if (map[x, y] == Tile.Wall)
                {
                    bool verticalColisions = (map[x, y - 1] == Tile.Path && map[x, y + 1] == Tile.Path);
                    bool horizontalColisions = (map[x - 1, y] == Tile.Path && map[x + 1, y] == Tile.Path);
                    if ((verticalColisions || horizontalColisions) && _random.NextDouble() < probability)
                    {
                        map[x, y] = Tile.Path;
                    }
                }
            }
        }
    }
}
