using System.Diagnostics;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Models;
using UnfathomableMaze.Services;

namespace UnfathomableMaze.Scenes;

/// <summary>
/// The menu scene to select the options.
/// </summary>
public class MenuScene : IScene
{
    private static readonly string Title = $"Unfathomable Maze{Environment.NewLine}  and the Table";
    private int _selectedOption;

    private static readonly List<string> Options =
    [
        "Laberinto (fácil)",
        "Laberinto (duro)",
        "Tabla",
        "Salir"
    ];

    public MenuScene(int initialOption = 0)
    {
        _selectedOption = initialOption;
    }

    public void Draw(Engine.Canvas canvas)
    {
        canvas.Clear();

        canvas.Draw(Title, (canvas.Width - "Unfathomable Maze".Length) / 2, 1);

        var maxWidth = Options.Max(item => item.Length);
        var startX = (canvas.Width - maxWidth) / 2;
        var startY = (canvas.Height - Options.Count) / 2;

        canvas.Draw($"┌{new string('─', maxWidth)}┐", startX - 1, startY - 1);

        for (var i = 0; i < Options.Count; i++)
        {
            var x = startX + maxWidth / 2 - Options[i].Length / 2;
            var y = startY + i;

            canvas.Draw("│", startX - 1, y);
            canvas.Draw("│", startX + maxWidth, y);
            canvas.Draw(Options[i], x, y,
                i == _selectedOption ? new Style(decoration: Decoration.Invert) : new Style());
        }

        canvas.Draw($"└{new string('─', maxWidth)}┘", startX - 1, startY + Options.Count);
    }

    public void OnKeyPressed(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                _selectedOption = (_selectedOption - 1 + Options.Count) % Options.Count;
                break;
            case ConsoleKey.DownArrow:
                _selectedOption = (_selectedOption + 1) % Options.Count;
                break;
            case ConsoleKey.Enter:
                IScene? newScene = null;

                switch (_selectedOption)
                {
                    case 0:
                        newScene = new MazeScene(false);
                        break;
                    case 1:
                        newScene = new MazeScene(true);
                        break;
                    case 2:
                        newScene = new TableScene();
                        break;
                    case 3:
                        Console.Clear();
                        Engine.Instance?.Stop();
                        return;
                }

                Debug.Assert(newScene != null, "Invalid option selected.");

                Engine.Instance?.UpdateScene(newScene);
                break;
        }
    }
}
