using System.Drawing; // Assuming you are using this for the Color struct
using System.Collections.Generic;
using UnfathomableMaze.Enums;
using UnfathomableMaze.Models;

namespace UnfathomableMaze.Services;

/// <summary>
/// A collection of ANSI escape sequences.
/// </summary>
public static class Ansi
{
    /// <summary>
    /// Resets all formatting back to the terminal's default.
    /// </summary>
    public const string Reset = "\x1b[0m";

    /// <summary>
    /// Moves the cursor to the specified X and Y coordinates.
    /// Note: ANSI cursor positioning is 1-indexed and formatted as Row;Column.
    /// </summary>
    public static string MoveTo(int x, int y)
    {
        // Add 1 because arrays are 0-indexed but ANSI is 1-indexed.
        // Y is rows, X is columns.
        return $"\x1b[{y + 1};{x + 1}H";
    }

    /// <summary>
    /// Converts a Style struct into a single, chained ANSI escape sequence.
    /// </summary>
    public static string GetStyleSequence(Style style)
    {
        var codes = new List<string>(capacity: 6) { "0" };

        // Text Decorations (Bitwise flag checks)
        if (style.Decoration.HasFlag(Decoration.Bold)) codes.Add("1");
        if (style.Decoration.HasFlag(Decoration.Faint)) codes.Add("2");
        if (style.Decoration.HasFlag(Decoration.Italic)) codes.Add("3");
        if (style.Decoration.HasFlag(Decoration.Underline)) codes.Add("4");
        if (style.Decoration.HasFlag(Decoration.Blink)) codes.Add("5");
        if (style.Decoration.HasFlag(Decoration.Invert)) codes.Add("7");
        if (style.Decoration.HasFlag(Decoration.Conceal)) codes.Add("8");
        if (style.Decoration.HasFlag(Decoration.Strikethrough)) codes.Add("9");

        if (style.ForegroundColor.HasValue)
        {
            Color fg = style.ForegroundColor.Value;
            codes.Add($"38;2;{fg.R};{fg.G};{fg.B}");
        }

        if (style.BackgroundColor.HasValue)
        {
            Color bg = style.BackgroundColor.Value;
            codes.Add($"48;2;{bg.R};{bg.G};{bg.B}");
        }

        return $"\x1b[{string.Join(";", codes)}m";
    }
}