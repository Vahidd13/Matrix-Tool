using System;
using System.Windows.Forms;

/// <summary>
/// Provides helper methods for converting between WinForms text grids and matrix objects.
/// </summary>
public static class MatrixOperations
{
    /// <summary>
    /// Parses a two-dimensional grid of text boxes into a numeric matrix.
    /// Empty or invalid cells are interpreted as zero.
    /// </summary>
    /// <param name="textBoxes">Grid of input text boxes.</param>
    /// <returns>A matrix containing the parsed values.</returns>
    public static Matrix ParseMatrix(TextBox[,] textBoxes)
    {
        int rows = textBoxes.GetLength(0);
        int cols = textBoxes.GetLength(1);
        Matrix matrix = new Matrix(rows, cols);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                double.TryParse(textBoxes[i, j].Text, out double value);
                matrix[i, j] = value;
            }
        }

        return matrix;
    }

    /// <summary>
    /// Writes a matrix result into a grid of text boxes.
    /// </summary>
    /// <param name="result">Matrix whose values should be displayed.</param>
    /// <param name="textBoxes">Target text box grid.</param>
    public static void DisplayResult(Matrix result, TextBox[,] textBoxes)
    {
        int rows = result.Rows;
        int cols = result.Columns;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                textBoxes[i, j].Text = Matrix.DoubleToFraction(result[i, j]);
            }
        }
    }

    /// <summary>
    /// Clears all cells in a text box grid.
    /// </summary>
    /// <param name="textBoxes">Grid to clear.</param>
    public static void ClearGrid(TextBox[,] textBoxes)
    {
        foreach (var textBox in textBoxes)
        {
            textBox.Text = string.Empty;
        }
    }

    /// <summary>
    /// Fills empty cells in a text box grid with zeroes.
    /// </summary>
    /// <param name="textBoxes">Grid whose empty cells should be filled.</param>
    public static void FillEmptyCellsWithZeros(TextBox[,] textBoxes)
    {
        foreach (var textBox in textBoxes)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "0";
            }
        }
    }
}
