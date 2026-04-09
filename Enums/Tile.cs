namespace UnfathomableMaze.Models;

/// <summary>
///  A tile in the maze.
///
/// Can be either a wall or a path.
/// </summary>
public enum Tile
{
    /// <summary>
    /// A wall tile.
    ///
    /// It is a solid barrier that cannot be walked through.
    /// </summary>
    Wall,

    /// <summary>
    ///  A path tile.
    ///
    /// It is a free space that can be walked through.
    /// </summary>
    Path
}