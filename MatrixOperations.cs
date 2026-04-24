using System;
using System.Windows.Forms;

public static class MatrixOperations
{
    // Parses the text from TextBoxes to create a Matrix object
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

    // Displays the result matrix in the TextBoxes
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

    // Clears the grid of TextBoxes
    public static void ClearGrid(TextBox[,] textBoxes)
    {
        foreach (var textBox in textBoxes)
        {
            textBox.Text = string.Empty;
        }
    }

    // Fills empty cells in the grid with zeros
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
