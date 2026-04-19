using System;
using System.Collections.Generic;
using System.Text;
using UnfathomableMaze.Control;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;

namespace UnfathomableMaze.Scenes
{
    public class MazeScene : IScene
    {
        private MathyMap mapGenerator = new MathyMap();
        private nMaze maze;

        public MazeScene()
        {
            maze = new nMaze(mapGenerator);
        }

        public void Draw(GameEngine.Canvas canvas)
        {
            canvas.Clear();

            // The visual map
            char[,] displayMap = maze.ConvertMap();

            for (int i = 0; i < displayMap.GetLength(0); i++)
            {
                for (int j = 0; j < displayMap.GetLength(1); j++)
                {
                    canvas.Draw(displayMap[i, j].ToString(), j, i);
                }
            }
        }

        public void OnKeyPressed(ConsoleKey key)
        {

        }
    }
}
