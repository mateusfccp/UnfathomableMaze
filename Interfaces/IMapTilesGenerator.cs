using UnfathomableMaze.Models;

namespace UnfathomableMaze.Interfaces;

/// <summary>
///  A generator for a map of tiles.
/// </summary>
public interface IMapTilesGenerator
{
    /// <summary>
    /// Generates a map of tiles.
    /// </summary>
    /// <returns>
    /// A matrix of tiles representing the tiles in the map.
    /// <br />
    /// This method should be idempotent in relation to the instance of the generator.  
    /// <br/>
    /// The returned matrix can have each tile accessed by its x and y coordinates, e.g. <c>map[x, y]</c>.
    /// </returns>
    Tile[,] Generate();
}