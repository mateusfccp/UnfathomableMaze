using System.Text;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Models;

namespace UnfathomableMaze.Services;

public class GameEngine
{
    private int _width = Console.WindowWidth;
    private int _height = Console.WindowHeight;
    private Cell[,] _currentBuffer;
    private Cell[,] _nextBuffer;
    private IScene _scene;

    public GameEngine(IScene initialScene)
    {
        _currentBuffer = new Cell[_width, _height];
        _nextBuffer = new Cell[_width, _height];
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
            Canvas canvas = new Canvas(_width, _height);
            ConsoleKey key = ProcessInput();

            _scene.OnKeyPressed(key);
            _scene.Draw(canvas);

            _nextBuffer = canvas.Buffer;

            DrawBuffer();
        }
    }

    private void PollConsoleSize()
    {
        if (Console.WindowWidth != _width || Console.WindowHeight != _height)
        {
            _width = Console.WindowWidth;
            _height = Console.WindowHeight;
            _currentBuffer = new Cell[_width, _height];
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
        StringBuilder framePayload = new StringBuilder();
        Style? activeStyle = null;

        for (int y = 0; y < _currentBuffer.GetLength(1); y++)
        {
            for (int x = 0; x < _currentBuffer.GetLength(0); x++)
            {
                if (_currentBuffer[x, y] != _nextBuffer[x, y])
                {
                    Cell targetCell = _nextBuffer[x, y];
                    _currentBuffer[x, y] = targetCell;

                    framePayload.Append(Ansi.MoveTo(x, y));

                    if (activeStyle == null || !activeStyle.Value.Equals(targetCell.Style))
                    {
                        framePayload.Append(Ansi.GetStyleSequence(targetCell.Style));
                        activeStyle = targetCell.Style;
                    }

                    framePayload.Append(targetCell.Character);
                }
            }
        }

        // Flush the entire frame to the console in one single I/O operation
        if (framePayload.Length > 0)
        {
            framePayload.Append(Ansi.Reset);
            Console.Write(framePayload.ToString());
        }
    }

    /// <summary>
    ///  A canvas that can be used to draw to.
    /// </summary>
    public class Canvas
    {
        /// <summary>
        /// Creates a new canvas with the given width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Canvas(int width, int height)
        {
            Buffer = new Cell[width, height];
        }

        /// <summary>
        /// Clears the canvas.
        /// <br/>
        /// You should probably call this before drawing anything.
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < Buffer.GetLength(0); x++)
            {
                for (int y = 0; y < Buffer.GetLength(1); y++)
                {
                    Buffer[x, y] = Cell.Empty;
                }
            }
        }

        /// <summary>
        /// Draws a string at the given coordinates.
        /// </summary>
        /// <param name="content">The content to be drawn.</param>
        /// <param name="x">The X position of the content.</param>
        /// <param name="y">The Y position of the content.</param>
        /// <param name="style">An optional style to be used in the content.</param>
        public void Draw(string content, int x, int y, Style style = new())
        {
            int currentX = x;
            int currentY = y;

            for (int i = 0; i < content.Length; i++)
            {
                currentX++;
                if (content[i] == Environment.NewLine[0])
                {
                    currentY++;
                    currentX = x;
                    i = i + Environment.NewLine.Length - 1;
                }
                else
                {
                    Buffer[currentX, currentY] = new Cell(content[i], style);
                }
            }
        }

        /// <summary>
        /// The buffer that is being drawn to.
        /// </summary>
        public Cell[,] Buffer { get; }

        /// <summary>
        /// The width of the canvas.
        /// </summary>
        public int Width => Buffer.GetLength(0);

        /// <summary>
        /// The height of the canvas.
        /// </summary>
        public int Height => Buffer.GetLength(1);
    }

    /// <summary>
    ///  Represents a cell in the game.
    /// </summary>
    /// <param name="character">The character in the cell.</param>
    /// <param name="style">The style of the cell.</param>
    public readonly struct Cell(char character, Style style) : IEquatable<Cell>
    {
        /// <summary>
        ///  Represents an empty cell.
        /// </summary>
        public static Cell Empty { get; } = new Cell(' ', new Style());

        public char Character { get; } = character;
        public Style Style { get; } = style;

        /// <summary>
        /// Compares two cells for equality.
        /// </summary>
        /// <param name="other">The other cell being compared to this one.</param>
        /// <returns>Whether the cells are equal.</returns>
        public bool Equals(Cell other)
        {
            return Character == other.Character && Style.Equals(other.Style);
        }

        public override bool Equals(object? obj) => obj is Cell other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Character, Style);

        public static bool operator ==(Cell left, Cell right) => left.Equals(right);
        public static bool operator !=(Cell left, Cell right) => !left.Equals(right);
    }
}