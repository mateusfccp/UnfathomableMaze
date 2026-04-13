using System.Drawing;
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
    int _selectedOption = 0;

    private static readonly List<String> Options =
    [
        "Laberinto",
        "Tabla",
    ];

    public void Draw(GameEngine.Canvas canvas)
    {
        canvas.Clear();

        canvas.Draw(Title, (canvas.Width - "Unfathomable Maze".Length) / 2, 1);

        int maxWidth = Options.Max(item => item.Length);
        int startX = (canvas.Width - maxWidth) / 2;
        int startY = (canvas.Height - Options.Count) / 2;

        canvas.Draw($"┌{new String('─', maxWidth)}┐", startX - 1, startY - 1);

        for (int i = 0; i < Options.Count; i++)
        {
            int x = startX + maxWidth / 2 - Options[i].Length / 2;
            int y = startY + i;

            canvas.Draw("│", startX - 1, y);
            canvas.Draw("│", startX + maxWidth, y);
            canvas.Draw(Options[i], x, y,
                i == _selectedOption ? new Style(decoration: Decoration.Invert) : new Style());
        }

        canvas.Draw($"└{new String('─', maxWidth)}┘", startX - 1, startY + Options.Count);
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
                // TODO: Implement functionality based on selected option.
                break;
        }
    }
}