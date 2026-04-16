using System.Diagnostics.Contracts;
using System.Drawing;
using Microsoft.VisualBasic;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;

namespace UnfathomableMaze.Scenes;
/// <summary>
/// The Scene what shows a table.
/// </summary>
public class TableScene : IScene
{
    private string[,] _dataTable;
    private readonly int[] _columnMaxLengths;
    private readonly int _padding = 1;
    private readonly Size _tableDimention;

    public TableScene()
    {
        _dataTable = LoadDataTable();
        _columnMaxLengths = findColumnWidths(_dataTable);
        _tableDimention = findTableDimentions(_dataTable);
    }

    public void Draw(GameEngine.Canvas canvas)
{
    canvas.Clear();

    var startX = (canvas.Width - _tableDimention.Width) / 2;
    var startY = (canvas.Height - _tableDimention.Height) / 2;

    // 1. Dibujar Borde Superior del Encabezado
    canvas.Draw("┌", startX - 1, startY - 1);
    int currentX = startX;
    for (int i = 0; i < _dataTable.GetLength(1); i++)
    {
        int colWidth = _columnMaxLengths[i] + (_padding * 2);
        canvas.Draw(new string('─', colWidth), currentX, startY - 1);
        currentX += colWidth;
        if (i < _dataTable.GetLength(1) - 1)
            canvas.Draw("┬", currentX, startY - 1);
        currentX++; 
    }
    canvas.Draw("┐", currentX - 1, startY - 1);

    // 2. Dibujar Contenido de las Filas
    for (int f = 0; f < _dataTable.GetLength(0); f++)
    {
        int y = startY + f;
        // Si no es la primera fila (títulos), movemos el contenido hacia abajo 
        // para dejar espacio a la línea divisora del encabezado
        if (f > 0) y++; 

        currentX = startX - 1;
        for (int c = 0; c < _dataTable.GetLength(1); c++)
        {
            // Dibujar pared lateral izquierda de la celda
            canvas.Draw("│", currentX, y);
            
            // Dibujar el texto centrado o con padding
            canvas.Draw(_dataTable[f, c], currentX + 1 + _padding, y);

            currentX += _columnMaxLengths[c] + (_padding * 2) + 1;
        }
        // Dibujar pared lateral final derecha
        canvas.Draw("│", currentX, y);

        // 3. Dibujar Línea Divisora (Solo debajo de los títulos)
        if (f == 0)
        {
            int sepY = startY + 1;
            canvas.Draw("├", startX - 1, sepY);
            int lineX = startX;
            for (int i = 0; i < _dataTable.GetLength(1); i++)
            {
                int colWidth = _columnMaxLengths[i] + (_padding * 2);
                canvas.Draw(new string('─', colWidth), lineX, sepY);
                lineX += colWidth;
                if (i < _dataTable.GetLength(1) - 1)
                    canvas.Draw("┼", lineX, sepY);
                lineX++;
            }
            canvas.Draw("┤", lineX - 1, sepY);
        }
    }

    // 4. Borde Inferior Final (Cierra la tabla abajo del todo)
    int lastY = startY + _dataTable.GetLength(0) + 1;
    canvas.Draw("└", startX - 1, lastY);
    int footerX = startX;
    for (int i = 0; i < _dataTable.GetLength(1); i++)
    {
        int colWidth = _columnMaxLengths[i] + (_padding * 2);
        canvas.Draw(new string('─', colWidth), footerX, lastY);
        footerX += colWidth;
        if (i < _dataTable.GetLength(1) - 1)
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

    private int[] findColumnWidths(string[,] dataTable)
    {
        int[] columnWidths= new int[dataTable.GetLength(1)];
        try
        {
                for(int i=0; i<dataTable.GetLength(1);i++)
                {
                int tempLength = 0;
                for (int j = 0; j < dataTable.GetLength(0); j++)
                    {
                        if(dataTable[j,i].Length > tempLength)
                        {
                            tempLength = dataTable[j,i].Length;
                        }
                    }
                    columnWidths[i] = tempLength;
                }            
        }
        catch(IndexOutOfRangeException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("\nError al calcular los lengths más largos de la matriz");
        }
        return columnWidths;
    }

    private Size findTableDimentions(string[,] dataMatrix)
    {
        var heightTable = dataMatrix.GetLength(0)*2 +1;
        var linesToSeparateCells = dataMatrix.GetLength(1) + 1;
        var widthTable = linesToSeparateCells; 

        for(int i = 0; i < _columnMaxLengths.Length; i++)
        {
            widthTable = widthTable + _columnMaxLengths[i] + (_padding*2);            
        }   

        return new Size(widthTable,heightTable);
    }
   private string[,] LoadDataTable()
{
    string[,] dataTable = new string[12, 6]
    {
        { "ProductID", "Name",       "Category",     "Price",   "Stock", "Supplier" },
        { "P001",      "Laptop",     "Electronics",  "$1200",   "35",    "TechCorp" },
        { "P002",      "Smartphone", "Electronics",  "$800",    "50",    "MobileMax" },
        { "P003",      "Desk",       "Furniture",    "$150",    "20",    "FurniCo" },
        { "P004",      "Chair",      "Furniture",    "$85",     "100",   "FurniCo" },
        { "P005",      "Headphones", "Electronics",  "$60",     "200",   "SoundWave" },
        { "P006",      "Backpack",   "Accessories",  "$45",     "75",    "CarryAll" },
        { "P007",      "Shoes",      "Clothing",     "$90",     "120",   "FashionHub" },
        { "P008",      "Watch",      "Accessories",  "$250",    "40",    "TimeKeepers" },
        { "P009",      "Tablet",     "Electronics",  "$500",    "60",    "TechCorp" },
        { "P010",      "Lamp",       "Home Decor",   "$35",     "150",   "BrightHome" },
        { "P011",      "Book",       "Stationery",   "$20",     "300",   "EduWorld" }
    };
    return dataTable;
}



    private void testFindWidthFunction(GameEngine.Canvas canvas)
    {
        //var startX = 0;
        var startY = 0;

        for(int i =0; i < _dataTable.GetLength(1); i++)
        {
            var y = startY + i;
            var x = 0;
            canvas.Draw($"Column {i+1} : {_columnMaxLengths[i]}",x,y);
        }
    }
}