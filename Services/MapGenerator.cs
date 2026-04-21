using System.Diagnostics;
using System.Drawing;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;

namespace UnfathomableMaze.Services;

/// <summary>
/// A generator for a map of tiles.
/// <br />
/// This class uses a depth-first search that uses a stack to keep track of the current path. The "ant" starts at a
/// random position and carves a path through the maze until it reaches a dead end. When it reaches a dead end, it
/// backtracks until it finds a new path to carve. This process continues until all paths have been carved and the maze
/// is complete.
/// <br />
/// It adds loops to the maze by randomly turning a tile into a path with a certain probabilit, which makes the maze
/// more interesting.
/// </summary>
public class MapGenerator : IMapTilesGenerator
{
    private readonly Size _size;
    private readonly Random _random = new();

    /// <summary>
    /// Creates a new map generator with the given size.
    /// </summary>
    /// <param name="size">
    /// The size of the maze.
    /// <br />
    /// The size must be composed of two odd numbers, both greater than 1.
    /// </param>
    public MapGenerator(Size size)
    {
        Debug.Assert(size.Width % 2 == 1 && size.Height % 2 == 1,
            "The size of the maze must be made by 2 odd numbers.");
        Debug.Assert(size is { Width: > 1, Height: > 1 }, "The size of the maze must be greater than 1×1.");
        _size = size;
    }

    public Tile[,] Generate()
    {
        var map = new Tile[_size.Width, _size.Height]; // Creation of the matrix

        for (var x = 0; x < _size.Width; x++) // Fully initialized with walls
        {
            for (var y = 0; y < _size.Height; y++)
            {
                map[x, y] = Tile.Wall;
            }
        }

        var stack = new Stack<Point>(); // Stack of positions the "ant" carved into the maze
        int startX = (_size.Width / 2); // Start position of the ant in the maze
        int startY = (_size.Height / 2); // Can be changed but MUST ALWAYS be composed by two odd numbers.
        if (startX % 2 == 0) startX++;
        if (startY % 2 == 0) startY++;

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
        if (x < _size.Width - 2 && map[x + 2, y] == Tile.Wall)
            // Checks for the limit of the matrix at the right and neighbors at the right of X
            neighbors.Add(new Point(x + 2, y));
        if (y > 2 && map[x, y - 2] == Tile.Wall)
            // Checks for the limit of the matrix at the top and neighbors at the top of Y
            neighbors.Add(new Point(x, y - 2));
        if (y < _size.Height - 2 && map[x, y + 2] == Tile.Wall)
            // Checks for the limit of the matrix at the bot and neighbors at the bot of Y
            neighbors.Add(new Point(x, y + 2));

        return neighbors;
    }

    private void AddLoops(Tile[,] map, double probability)
    {
        for (int x = 1; x < _size.Width - 1; x++)
        {
            for (int y = 1; y < _size.Height - 1; y++)
            {
                if (map[x, y] == Tile.Wall)
                {
                    bool verticalCollisions = (map[x, y - 1] == Tile.Path && map[x, y + 1] == Tile.Path);
                    bool horizontalCollisions = (map[x - 1, y] == Tile.Path && map[x + 1, y] == Tile.Path);
                    if ((verticalCollisions || horizontalCollisions) && _random.NextDouble() < probability)
                    {
                        map[x, y] = Tile.Path;
                    }
                }
            }
        }
    }
}
