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
    private readonly int _padding = 2;
    private readonly Size _tableDimention;

    public TableScene()
    {
        _dataTable = LoadDataTable();
        _columnMaxLengths = findColumnWidths(_dataTable);
        _tableDimention = findTableDimentions(_dataTable);
    }

    void IScene.Draw(GameEngine.Canvas canvas)
    {
        canvas.Clear();

        var startX = (canvas.Width - _tableDimention.Width) / 2;
        var startY = (canvas.Height - _tableDimention.Height) / 2;

        canvas.Draw($"┌{new string('─', _tableDimention.Width-2)}┐", startX - 1, startY - 1);
        canvas.Draw("│",startX-1,startY);
        canvas.Draw("TO-DO",startX+(_tableDimention.Width/2-"TO-DO".Length),startY);
        canvas.Draw("│",startX+_tableDimention.Width-2,startY);
        canvas.Draw($"└{new string('─', _tableDimention.Width-2)}┘", startX - 1, startY+1);
        
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
            widthTable = widthTable + _columnMaxLengths[i] + _padding;            
        }   

        return new Size(widthTable,heightTable);
    }
private string[,] LoadDataTable()
    {
        string[,] dataTable = new string[3, 4]
        {
            { "Name",   "Country",   "Age", "Occupation" },
            { "Alice",  "USA",       "29", "Engineer"   },
            { "Bruno",  "Argentina", "35", "Teacher"    }
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