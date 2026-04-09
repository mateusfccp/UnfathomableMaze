using UnfathomableMaze.Interfaces;

namespace UnfathomableMaze.Models;

public class Map
{
    private readonly Tile[,] _tiles;

    /// <summary>
    /// The width of the map.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The height of the map.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// The tile at the given coordinates.
    /// </summary>
    /// <param name="x">The x coordinate of the tile in the map.</param>
    /// <param name="y">The y coordinate of the tile in the map.</param>
    public Tile this[int x, int y] => _tiles[x, y];

    /// <summary>
    /// Creates a new map.
    /// </summary>
    /// <param name="width">The width of the map.</param>
    /// <param name="height">The height of the map.</param>
    /// <param name="generator">The generator to be used when building the map tiles.</param>
    public Map(int width, int height, IMapTilesGenerator generator)
    {
        Width = width;
        Height = height;
        _tiles = generator.Generate();
    }
}