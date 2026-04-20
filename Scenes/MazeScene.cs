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
        private long steps;

        public MazeScene(bool hardmode, int score = 0, long steps = 0)
        {
            maze = new nMaze(mapGenerator);
            displayMap = maze.ConvertMap();
            player = new Point(1, 1);
            finishFlag = new Point(29, 29);
            this.hardmode = hardmode;
            this.score = score;
            this.steps = steps;
            
            
        }

        public void Draw(Engine.Canvas canvas)
        {
            canvas.Clear();
            ValidateGameState();

            // Score
            canvas.Draw($"Score: {score}", 1, 1);
            // Steps
            canvas.Draw($"Steps: {steps}", 1, 2);
            // Return to menu
            canvas.Draw("Press", 1, 3);
            canvas.Draw("Esc", 7, 3, new Models.Style(Color.Salmon));
            canvas.Draw("to return to Menu", 11, 3);

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
        }

        public void OnKeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    steps++;
                    if (!maze.ValidateNorth(maze.TileMap, player.Y, player.X)) player.Y--;
                    else if (hardmode) death = true;
                    break;
                case ConsoleKey.RightArrow:
                    steps++;
                    if (!maze.ValidateEast(maze.TileMap, player.Y, player.X)) player.X++;
                    else if (hardmode) death = true;
                    break;
                case ConsoleKey.DownArrow:
                    steps++;
                    if (!maze.ValidateSouth(maze.TileMap, player.Y, player.X)) player.Y++;
                    else if (hardmode) death = true;
                    break;
                case ConsoleKey.LeftArrow:
                    steps++;
                    if (!maze.ValidateWest(maze.TileMap, player.Y, player.X)) player.X--;
                    else if (hardmode) death = true;
                    break;
                case ConsoleKey.Escape:
                    Engine.Instance?.UpdateScene(new MenuScene());
                    break;

            }

        }

        private void ValidateGameState()
        {
            if (player == finishFlag)
            {
                score = score + 1;
                IScene? newScene = null;
                if (hardmode)
                {
                    newScene = new MazeScene(true, score, steps);
                }
                else newScene = new MazeScene(false, score, steps);
                Engine.Instance?.UpdateScene(newScene);
            }

            if (death)
            {
                IScene? newScene = null;
                newScene = new DeathScene();    
                Engine.Instance?.UpdateScene(newScene);
            }
        }
    }
}
