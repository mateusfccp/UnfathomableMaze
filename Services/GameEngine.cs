using UnfathomableMaze.Interfaces;

namespace UnfathomableMaze.Services;

public class GameEngine
{
    private int _width = Console.WindowWidth;
    private int _height = Console.WindowHeight;
    private Char[,] _currentBuffer;
    private Char[,] _nextBuffer;
    private IScene _scene;

    public GameEngine(IScene initialScene)
    {
        _currentBuffer = new Char[_width, _height];
        _nextBuffer = new Char[_width, _height];
        _scene = initialScene;
    }

    /// <summary>
    /// Starts the game engine.
    /// </summary>
    public void Start()
    {
        Console.CursorVisible = false;
        Loop();
        Console.CursorVisible = true;
    }

    private void Loop()
    {
        while (true)
        {
            PollConsoleSize();
            ConsoleKey key = ProcessInput();
            if (key == ConsoleKey.Escape)
            {
                break;
            }

            _scene.OnKeyPressed(key);
            _scene.Draw(_nextBuffer);
            DrawBuffer();
        }
    }

    private void PollConsoleSize()
    {
        if (Console.WindowWidth != _width || Console.WindowHeight != _height)
        {
            _width = Console.WindowWidth;
            _height = Console.WindowHeight;
            _currentBuffer = new Char[_width, _height];
        }
    }

    private ConsoleKey ProcessInput()
    {
        if (Console.KeyAvailable)
        {
            return Console.ReadKey(intercept: true).Key;
        }
        else
        {
            return ConsoleKey.None;
        }
    }

    private void DrawBuffer()
    {
        for (int x = 0; x < _currentBuffer.GetLength(0); x++)
        {
            for (int y = 0; y < _currentBuffer.GetLength(1); y++)
            {
                if (_currentBuffer[x, y] != _nextBuffer[x, y])
                {
                    _currentBuffer[x, y] = _nextBuffer[x, y];
                    Console.SetCursorPosition(x, y);
                    Console.Write(_currentBuffer[x, y]);
                }
            }
        }
    }
}