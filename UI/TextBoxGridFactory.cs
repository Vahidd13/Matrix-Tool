using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Creates and removes rectangular grids of text boxes used as matrix cells.
/// </summary>
internal static class TextBoxGridFactory
{
    public const int CellSize = 32;
    public const int CellSpacing = 8;
    public const int BlockSpacing = 60;

    /// <summary>
    /// Creates a matrix-shaped grid of text boxes on the selected tab page.
    /// </summary>
    public static TextBox[,] CreateGrid(TabPage tabPage, int rows, int columns, int startX, int startY, bool readOnly = false)
    {
        var grid = new TextBox[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                grid[row, column] = new TextBox
                {
                    Location = new Point(
                        startX + column * (CellSize + CellSpacing),
                        startY + row * (CellSize + CellSpacing)),
                    Size = new Size(CellSize, CellSize),
                    ReadOnly = readOnly
                };

                tabPage.Controls.Add(grid[row, column]);
            }
        }

        return grid;
    }

    /// <summary>
    /// Removes all text boxes from a previously created grid.
    /// </summary>
    public static void RemoveGrid(TabPage tabPage, TextBox[,] grid)
    {
        if (grid == null)
        {
            return;
        }

        foreach (var textBox in grid)
        {
            tabPage.Controls.Remove(textBox);
            textBox.Dispose();
        }
    }

    /// <summary>
    /// Calculates the horizontal space used by a matrix grid.
    /// </summary>
    public static int GridWidth(int columns)
    {
        return columns * (CellSize + CellSpacing);
    }
}
