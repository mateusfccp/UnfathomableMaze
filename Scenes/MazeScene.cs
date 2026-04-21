using System.Drawing;
using System.Numerics;
using UnfathomableMaze.Controllers;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnfathomableMaze.Scenes;

public class MazeScene : IScene
{
    private readonly IMapTilesGenerator _mapGenerator;
    private readonly MazeController _mazeController;
    private readonly char[,] _displayMap;
    private Point _player;
    private Point _finishFlag;
    private readonly bool _hardmode;
    private bool _death;
    private int _score;
    private long _steps;

    public MazeScene(IMapTilesGenerator mapGenerator, bool hardMode, int score = 0, long steps = 0)
    {
        _mapGenerator = mapGenerator;
        _mazeController = new MazeController(mapGenerator);
        _displayMap = ConvertMap(_mazeController.TileMap);
        _player = new Point(1, 1);
        _hardMode = hardMode;
        _score = score;
        _steps = steps;
    }

    public void Draw(Engine.Canvas canvas)
    {
        canvas.Clear();

        // Score
        canvas.Draw($"Score: {_score}", 1, 1);
        // Steps
        canvas.Draw($"Steps: {_steps}", 1, 2);
        // Return to menu
        canvas.Draw("Press", 1, 3);
        canvas.Draw("Esc", 7, 3, new Models.Style(Color.Salmon));
        canvas.Draw("to return to Menu", 11, 3);

        // The visual map
        for (int i = 0; i < _displayMap.GetLength(0); i++)
        {
            for (int j = 0; j < _displayMap.GetLength(1); j++)
            {
                canvas.Draw(_displayMap[i, j].ToString(), canvas.Width / 2 - _displayMap.GetLength(1) / 2 + j,
                    canvas.Height / 2 - _displayMap.GetLength(0) / 2 + i);
            }
        }

        // Finish Flag
        _finishFlag = new Point(_displayMap.GetLength(1) - 2, _displayMap.GetLength(0) - 2);
        canvas.Draw("⚿", canvas.Width / 2 - _displayMap.GetLength(1) / 2 + _finishFlag.X,
            canvas.Height / 2 - _displayMap.GetLength(0) / 2 + _finishFlag.Y, new Models.Style(Color.Gold));

        // Player
        canvas.Draw("@", canvas.Width / 2 - _displayMap.GetLength(1) / 2 + _player.X,
            canvas.Height / 2 - _displayMap.GetLength(0) / 2 + _player.Y);
    }

    public void OnKeyPressed(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (!_mazeController.ValidateNorth(_mazeController.TileMap, _player.Y, _player.X))
                {
                    _player.Y--;
                    _steps++;
                }
                else if (_hardmode) _death = true;
                break;
            case ConsoleKey.RightArrow:
                if (!_mazeController.ValidateEast(_mazeController.TileMap, _player.Y, _player.X))
                {
                    _player.X++;
                    _steps++;
                }
                else if (_hardmode) _death = true;
                break;
            case ConsoleKey.DownArrow:
                if (!_mazeController.ValidateSouth(_mazeController.TileMap, _player.Y, _player.X))
                {
                    _player.Y++;
                    _steps++;
                }
                else if (_hardmode) _death = true;
                break;
            case ConsoleKey.LeftArrow:
                if (!_mazeController.ValidateWest(_mazeController.TileMap, _player.Y, _player.X))
                {
                    _player.X--;
                    _steps++;
                }
                else if (_hardmode) _death = true;
                break;
            case ConsoleKey.Escape:
                Engine.Instance?.UpdateScene(new MenuScene());
                break;
        }
    }

    private void ValidateGameState()
    {
        if (_player == _finishFlag)
        {
            _score = _score + 1;
            var newScene = new MazeScene(_mapGenerator, _hardmode, _score, _steps);
            Engine.Instance?.UpdateScene(newScene);
        }
        else if (_death)
        {
            var newScene = new DeathScene();
            Engine.Instance?.UpdateScene(newScene);
        }
    }

    private char[,] ConvertMap(Tile[,] tileMap)
    {
        int width = tileMap.GetLength(0);
        int height = tileMap.GetLength(1);
        char[,] charMap = new char[width, height];

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                if (tileMap[y, x] == Tile.Wall)
                {
                    bool north = _mazeController.ValidateNorth(tileMap, x, y);
                    bool south = _mazeController.ValidateSouth(tileMap, x, y);
                    bool west = _mazeController.ValidateWest(tileMap, x, y);
                    bool east = _mazeController.ValidateEast(tileMap, x, y);

                    // Corners
                    if (south && east && !north && !west) charMap[y, x] = '┌';
                    else if (south && west && !north && !east) charMap[y, x] = '┐';
                    else if (north && east && !south && !west) charMap[y, x] = '└';
                    else if (north && west && !south && !east) charMap[y, x] = '┘';

                    // 3 Way Intersections
                    else if (west && east && south && !north) charMap[y, x] = '┬';
                    else if (west && east && north && !south) charMap[y, x] = '┴';
                    else if (north && south && east && !west) charMap[y, x] = '├';
                    else if (north && south && west && !east) charMap[y, x] = '┤';

                    // Cross
                    else if (north && south && west && east) charMap[y, x] = '┼';

                    // Straights
                    else if (north || south) charMap[y, x] = '│';
                    else charMap[y, x] = '─';
                }
                else
                {
                    charMap[y, x] = ' ';
                }
            }
        }

        return charMap;
    }
}
