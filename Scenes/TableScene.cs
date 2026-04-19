using System.Drawing;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Models;
using UnfathomableMaze.Services;

namespace UnfathomableMaze.Scenes;

/// <summary>
/// The Scene what shows a table.
/// </summary>
public class TableScene : IScene
{
    private static readonly string[,] DataTable = new[,]
    {
        { "Language", "Year", "Creator", "Paradigm", "Typing", "Use Case" },
        { "C", "1972", "Dennis Ritchie", "Procedural", "Static", "Systems Programming" },
        { "Java", "1995", "James Gosling", "Object-Oriented", "Static", "Enterprise Apps" },
        { "Python", "1991", "Guido van Rossum", "Multi-Paradigm", "Dynamic", "Data Science, Web" },
        { "JavaScript", "1995", "Brendan Eich", "Event-Driven", "Dynamic", "Web Development" },
        { "C#", "2000", "Microsoft", "Object-Oriented", "Static", "Desktop & Web Apps" },
        { "Ruby", "1995", "Yukihiro Matsumoto", "Object-Oriented", "Dynamic", "Web Development" },
        { "Go", "2009", "Google", "Concurrent", "Static", "Cloud & Networking" },
        { "Rust", "2010", "Mozilla", "Systems + Safe", "Static", "Safe Systems Programming" }
    };

    private readonly int[] _columnMaxLengths;
    private readonly Size _tableDimention;

    private readonly Style _titlesStyle = new Style(Color.Cyan, null, Enums.Decoration.Bold);

    public TableScene()
    {
        _columnMaxLengths = FindColumnWidths(DataTable);
        _tableDimention = FindTableDimentions(DataTable);
    }

    public void Draw(GameEngine.Canvas canvas)
    {
        canvas.Clear();
        //Validation
        if (canvas.Width < _tableDimention.Width || canvas.Height < _tableDimention.Height)
        {
            Console.WriteLine(
                $"Tamaño de pantalla insuficiente, intente nuevamente \n({_tableDimention.Width}x{_tableDimention.Height} de espacios requeridos)");
            Console.WriteLine("Presione una tecla para volver....");
            Console.ReadKey(true);
            return;
        }

        var startX = (canvas.Width - _tableDimention.Width) / 2;
        var startY = (canvas.Height - _tableDimention.Height) / 2;

        //Top line
        canvas.Draw("┌", startX - 1, startY - 1);
        int currentX = startX;
        for (int i = 0; i < DataTable.GetLength(1); i++)
        {
            int colWidth = _columnMaxLengths[i] + 2;
            canvas.Draw(new string('─', colWidth), currentX, startY - 1);
            currentX += colWidth;
            if (i < DataTable.GetLength(1) - 1)
                canvas.Draw("┬", currentX, startY - 1);
            currentX++;
        }

        canvas.Draw("┐", currentX - 1, startY - 1);

        // 2. Dibujar Contenido de las Filas
        for (int f = 0; f < DataTable.GetLength(0); f++)
        {
            int y = startY + f;
            // Si no es la primera fila (títulos), movemos el contenido hacia abajo 
            // para dejar espacio a la línea divisora del encabezado
            if (f > 0) y++;

            currentX = startX - 1;
            for (int c = 0; c < DataTable.GetLength(1); c++)
            {
                // Dibujar pared lateral izquierda de la celda
                canvas.Draw("│", currentX, y);

                // Dibujar el texto centrado o con padding
                if (f > 0)
                {
                    canvas.Draw(DataTable[f, c], currentX + 2, y);
                }
                else
                {
                    canvas.Draw(DataTable[f, c], currentX + 2, y, _titlesStyle);
                }


                currentX += _columnMaxLengths[c] + 3;
            }

            // Dibujar pared lateral final derecha
            canvas.Draw("│", currentX, y);

            // 3. Dibujar Línea Divisora (Solo debajo de los títulos)
            if (f == 0)
            {
                int sepY = startY + 1;
                canvas.Draw("├", startX - 1, sepY);
                int lineX = startX;
                for (int i = 0; i < DataTable.GetLength(1); i++)
                {
                    int colWidth = _columnMaxLengths[i] + 2;
                    canvas.Draw(new string('─', colWidth), lineX, sepY);
                    lineX += colWidth;
                    if (i < DataTable.GetLength(1) - 1)
                        canvas.Draw("┼", lineX, sepY);
                    lineX++;
                }

                canvas.Draw("┤", lineX - 1, sepY);
            }
        }

        // 4. Borde Inferior Final (Cierra la tabla abajo del todo)
        int lastY = startY + DataTable.GetLength(0) + 1;
        canvas.Draw("└", startX - 1, lastY);
        int footerX = startX;
        for (int i = 0; i < DataTable.GetLength(1); i++)
        {
            int colWidth = _columnMaxLengths[i] + 2;
            canvas.Draw(new string('─', colWidth), footerX, lastY);
            footerX += colWidth;
            if (i < DataTable.GetLength(1) - 1)
                canvas.Draw("┴", footerX, lastY);
            footerX++;
        }

        canvas.Draw("┘", footerX - 1, lastY);
    }

    void IScene.OnKeyPressed(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Backspace:
                return;
        }
    }

    private int[] FindColumnWidths(string[,] dataTable)
    {
        int[] columnWidths = new int[dataTable.GetLength(1)];
        for (int i = 0; i < dataTable.GetLength(1); i++)
        {
            int tempLength = 0;
            for (int j = 0; j < dataTable.GetLength(0); j++)
            {
                if (dataTable[j, i].Length > tempLength)
                {
                    tempLength = dataTable[j, i].Length;
                }
            }

            columnWidths[i] = tempLength;
        }

        return columnWidths;
    }

    private Size FindTableDimentions(string[,] dataMatrix)
    {
        var heightTable = dataMatrix.GetLength(0) * 2 + 1;
        var linesToSeparateCells = dataMatrix.GetLength(1) + 1;
        var widthTable = linesToSeparateCells;

        for (int i = 0; i < _columnMaxLengths.Length; i++)
        {
            widthTable = widthTable + _columnMaxLengths[i] + 2;
        }

        return new Size(widthTable, heightTable);
    }
}
