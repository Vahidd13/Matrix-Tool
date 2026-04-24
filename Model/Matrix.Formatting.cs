using System;
using System.Text;

/// <summary>
/// Contains matrix, fraction, and step-display formatting helpers.
/// </summary>
public partial class Matrix
{
    /// <summary>
    /// Formats a numeric value as a fraction when a compact fractional representation can be found.
    /// </summary>
    /// <param name="value">Value to format.</param>
    /// <returns>A string containing either an integer, a fraction, or zero.</returns>
    public static string DoubleToFraction(double value)
    {
        if (Math.Abs(value) < 1e-10) // Adjust zero threshold as needed
            return "0";

        (long num, long denom) = ApproximateFraction(value, 1e-10); // Use a fixed precision, adjust as needed

        if (denom == 1)
            return num.ToString();
        else
            return $"{num}/{denom}";
    }

    /// <summary>
    /// Formats the matrix as aligned text for step displays and history.
    /// </summary>
    /// <returns>A multiline string representation of the matrix.</returns>
    public string ToDisplayString()
    {
        var values = new string[Rows, Columns];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                values[i, j] = DoubleToFraction(data[i, j]);
            }
        }

        return FormatMatrixGrid(values);
    }

    private static string FormatAugmentedMatrix(double[,] left, double[,] right)
    {
        int rows = left.GetLength(0);
        int colsLeft = left.GetLength(1);
        int colsRight = right.GetLength(1);

        var leftValues = new string[rows, colsLeft];
        var rightValues = new string[rows, colsRight];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < colsLeft; j++)
            {
                leftValues[i, j] = DoubleToFraction(left[i, j]);
            }
            for (int j = 0; j < colsRight; j++)
            {
                rightValues[i, j] = DoubleToFraction(right[i, j]);
            }
        }

        int[] leftWidths = GetColumnWidths(leftValues);
        int[] rightWidths = GetColumnWidths(rightValues);
        var builder = new StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            AppendFormattedRow(builder, leftValues, leftWidths, i);
            builder.Append("  |  ");
            AppendFormattedRow(builder, rightValues, rightWidths, i);
            if (i < rows - 1)
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }

    private static string FormatMatrixGrid(string[,] values)
    {
        int rows = values.GetLength(0);
        int[] widths = GetColumnWidths(values);
        var builder = new StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            AppendFormattedRow(builder, values, widths, i);
            if (i < rows - 1)
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }

    private static int[] GetColumnWidths(string[,] values)
    {
        int rows = values.GetLength(0);
        int cols = values.GetLength(1);
        int[] widths = new int[cols];

        for (int j = 0; j < cols; j++)
        {
            int width = 0;
            for (int i = 0; i < rows; i++)
            {
                width = Math.Max(width, values[i, j]?.Length ?? 0);
            }
            widths[j] = width;
        }

        return widths;
    }

    private static void AppendFormattedRow(StringBuilder builder, string[,] values, int[] widths, int row)
    {
        int cols = values.GetLength(1);
        for (int j = 0; j < cols; j++)
        {
            builder.Append((values[row, j] ?? string.Empty).PadLeft(widths[j]));
            if (j < cols - 1)
            {
                builder.Append("   ");
            }
        }
    }

    private static void SwapRows(double[,] left, double[,] right, int row1, int row2)
    {
        int colsLeft = left.GetLength(1);
        int colsRight = right.GetLength(1);
        for (int col = 0; col < colsLeft; col++)
        {
            var temp = left[row1, col];
            left[row1, col] = left[row2, col];
            left[row2, col] = temp;
        }
        for (int col = 0; col < colsRight; col++)
        {
            var temp = right[row1, col];
            right[row1, col] = right[row2, col];
            right[row2, col] = temp;
        }
    }

    private static void DivideRow(double[,] left, double[,] right, int row, double divisor)
    {
        int colsLeft = left.GetLength(1);
        int colsRight = right.GetLength(1);
        for (int col = 0; col < colsLeft; col++)
        {
            left[row, col] /= divisor;
        }
        for (int col = 0; col < colsRight; col++)
        {
            right[row, col] /= divisor;
        }
    }

    private static void SubtractRow(double[,] left, double[,] right, int row, int subtractRow, double factor)
    {
        int colsLeft = left.GetLength(1);
        int colsRight = right.GetLength(1);
        for (int col = 0; col < colsLeft; col++)
        {
            left[row, col] -= factor * left[subtractRow, col];
        }
        for (int col = 0; col < colsRight; col++)
        {
            right[row, col] -= factor * right[subtractRow, col];
        }
    }

    private static string FormatValue(double value)
    {
        return DoubleToFraction(value);
    }

    private static (long, long) ApproximateFraction(double value, double accuracy)
    {
        int sign = Math.Sign(value);
        value = Math.Abs(value);

        long lower_n = 0;
        long lower_d = 1;
        long upper_n = 1;
        long upper_d = 0;
        long middle_n = 0;
        long middle_d = 1;

        while (true)
        {
            middle_n = lower_n + upper_n;
            middle_d = lower_d + upper_d;
            if (middle_d * (value + accuracy) < middle_n)
            {
                upper_n = middle_n;
                upper_d = middle_d;
            }
            else if (middle_n < (value - accuracy) * middle_d)
            {
                lower_n = middle_n;
                lower_d = middle_d;
            }
            else
            {
                return (middle_n * sign, middle_d);
            }
        }
    }
}
