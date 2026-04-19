using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;

namespace UnfathomableMaze.Control
{
    public class MathyMap : IMapTilesGenerator
    {
        public Tile[,] Generate()

        /*
         * 1, 0, 1, 1, 1, 1, 1
         * 1, 0, 0, 0, 0, 0, 1
         * 1, 0, 1, 0, 1, 1, 1
         * 1, 1, 1, 0, 0, 0, 1
         * 1, 0, 0, 0, 1, 0, 1
         * 1, 0, 1, 0, 1, 0, 0
         * 1, 1, 1, 1, 1, 1, 1
        */

        {
            return new Tile[7, 7] {
                { Tile.Wall, Tile.Path, Tile.Wall, Tile.Wall, Tile.Wall, Tile.Wall, Tile.Wall },
                { Tile.Wall, Tile.Path, Tile.Path, Tile.Path, Tile.Path, Tile.Path, Tile.Wall },
                { Tile.Wall, Tile.Path, Tile.Wall, Tile.Path, Tile.Wall, Tile.Wall, Tile.Wall },
                { Tile.Wall, Tile.Wall, Tile.Wall, Tile.Path, Tile.Path, Tile.Path, Tile.Wall },
                { Tile.Wall, Tile.Path, Tile.Path, Tile.Path, Tile.Wall, Tile.Path, Tile.Wall },
                { Tile.Wall, Tile.Path, Tile.Wall, Tile.Path, Tile.Wall, Tile.Path, Tile.Path },
                { Tile.Wall, Tile.Wall, Tile.Wall, Tile.Wall, Tile.Wall, Tile.Wall, Tile.Wall }
            };
        }
    }
}