using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnfathomableMaze.Control;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;
using static UnfathomableMaze.Services.GameEngine;

namespace UnfathomableMaze.Scenes
{
    public class MazeScene : IScene
    {
        private MapGenerator mapGenerator = new MapGenerator();
        private nMaze maze;
        private char[,] displayMap;
        private Point player;

        public MazeScene()
        {
            maze = new nMaze(mapGenerator);
            displayMap = maze.ConvertMap();
            player = new Point(1, 1);
        }

        public void Draw(GameEngine.Canvas canvas)
        {
            canvas.Clear();

            // The visual map
            for (int i = 0; i < displayMap.GetLength(0); i++)
            {
                for (int j = 0; j < displayMap.GetLength(1); j++)
                {
                    canvas.Draw(displayMap[i, j].ToString(), canvas.Width/2 - displayMap.GetLength(1)/2 + j, canvas.Height / 2 - displayMap.GetLength(0) / 2 + i);
                }
            }

            // Player
            canvas.Draw("@", canvas.Width / 2 - displayMap.GetLength(1) / 2 + player.X, canvas.Height / 2 - displayMap.GetLength(0) / 2 + player.Y);
        }

        public void OnKeyPressed(ConsoleKey key)
        {

        }
    }
}
