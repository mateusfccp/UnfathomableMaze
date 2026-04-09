using System.Drawing;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;

var engine = new GameEngine(new TestScene());
engine.Start();

class TestScene() : IScene
{
    private Point _playerPosition = new(0, 0);

    public void Draw(char[,] buffer)
    {
        for (int x = 0; x < buffer.GetLength(0); x++)
        {
            for (int y = 0; y < buffer.GetLength(1); y++)
            {
                if (x == _playerPosition.X && y == _playerPosition.Y)
                {
                    buffer[x, y] = '☺';
                }
                else
                {
                    buffer[x, y] = ' ';
                }
            }
        }
    }

    public void OnKeyPressed(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                _playerPosition.Y = _playerPosition.Y - 1;
                break;
            case ConsoleKey.DownArrow:
                _playerPosition.Y = _playerPosition.Y + 1;
                break;
            case ConsoleKey.LeftArrow:
                _playerPosition.X = _playerPosition.X - 1;
                break;
            case ConsoleKey.RightArrow:
                _playerPosition.X = _playerPosition.X + 1;
                break;
        }
    }
}