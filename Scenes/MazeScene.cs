using System.Drawing;
using UnfathomableMaze.Control;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;

namespace UnfathomableMaze.Scenes
{
    public class MazeScene : IScene
    {
        private MapGenerator mapGenerator = new MapGenerator();
        private nMaze maze;
        private char[,] displayMap;
        private Point player;
        private Point finishFlag;
        private bool hardmode;
        private bool death;
        private int score;

        public MazeScene(bool hardmode, int score = 0)
        {
            maze = new nMaze(mapGenerator);
            displayMap = maze.ConvertMap();
            player = new Point(1, 1);
            finishFlag = new Point(29, 29);
            this.hardmode = hardmode;
            this.score = score;
            
            
        }

        public void Draw(Engine.Canvas canvas)
        {
            canvas.Clear();

            // Score
            canvas.Draw($"Score: {score}", 1, 1);

            // The visual map
            for (int i = 0; i < displayMap.GetLength(0); i++)
            {
                for (int j = 0; j < displayMap.GetLength(1); j++)
                {
                    canvas.Draw(displayMap[i, j].ToString(), canvas.Width/2 - displayMap.GetLength(1)/2 + j, canvas.Height / 2 - displayMap.GetLength(0) / 2 + i);
                }
            }

            // Finish Flag
            canvas.Draw("⚿", canvas.Width / 2 - displayMap.GetLength(1) / 2 + finishFlag.X, canvas.Height / 2 - displayMap.GetLength(0) / 2 + finishFlag.Y, new Models.Style(Color.Gold));

            // Player
            canvas.Draw("@", canvas.Width / 2 - displayMap.GetLength(1) / 2 + player.X, canvas.Height / 2 - displayMap.GetLength(0) / 2 + player.Y);

            // Infinte Mode
            if (player == finishFlag)
            {
                score = score + 1;
                IScene? newScene = null;
                if (hardmode)
                {
                    newScene = new MazeScene(true, score);
                } else newScene = new MazeScene(false, score);
                Engine.Instance?.UpdateScene(newScene);
            }
        }

        public void OnKeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (!maze.ValidateNorth(maze.TileMap, player.Y, player.X)) player.Y--;
                    else if (hardmode) death = true;
                    break;
                case ConsoleKey.RightArrow:
                    if (!maze.ValidateEast(maze.TileMap, player.Y, player.X)) player.X++;
                    else if (hardmode) death = true;
                    break;
                case ConsoleKey.DownArrow:
                    if (!maze.ValidateSouth(maze.TileMap, player.Y, player.X)) player.Y++;
                    else if (hardmode) death = true;
                    break;
                case ConsoleKey.LeftArrow:
                    if (!maze.ValidateWest(maze.TileMap, player.Y, player.X)) player.X--;
                    else if (hardmode) death = true;
                    break;
            }

        }
    }
}
