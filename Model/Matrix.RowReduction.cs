using System;
using System.Collections.Generic;

/// <summary>
/// Contains row-reduction algorithms and row-operation helpers.
/// </summary>
public partial class Matrix
{
    /// <summary>
    /// Calculates the reduced row echelon form of a matrix.
    /// </summary>
    /// <param name="matrix">Input matrix.</param>
    /// <returns>The matrix converted to reduced row echelon form.</returns>
    public static Matrix RREF(Matrix matrix)
    {
        var rref = matrix.Clone();
        int lead = 0;
        for (int r = 0; r < rref.Rows; r++)
        {
            if (lead >= rref.Columns)
                break;
            int i = r;
            while (rref[i, lead] == 0)
            {
                i++;
                if (i == rref.Rows)
                {
                    i = r;
                    lead++;
                    if (lead == rref.Columns)
                        return rref.Round(10); // Round the result to 10 decimal places
                }
            }
            rref.SwapRows(r, i);
            var div = rref[r, lead];
            rref.DivideRow(r, div);

            for (int j = 0; j < rref.Rows; j++)
            {
                if (j != r)
                {
                    var sub = rref[j, lead];
                    rref.SubtractRow(j, r, sub);
                }
            }
            lead++;
        }
        return rref.Round(10); // Round the result to 10 decimal places
    }

    /// <summary>
    /// Calculates the reduced row echelon form and records each row operation.
    /// </summary>
    /// <param name="matrix">Input matrix.</param>
    /// <returns>The final matrix and a list of formatted calculation steps.</returns>
    public static MatrixStepsResult RrefWithSteps(Matrix matrix)
    {
        var steps = new List<string>();
        var rref = matrix.Clone();
        steps.Add("Start:" + Environment.NewLine + rref.ToDisplayString());

        int lead = 0;
        for (int r = 0; r < rref.Rows; r++)
        {
            if (lead >= rref.Columns)
                break;
            int i = r;
            while (Math.Abs(rref[i, lead]) < 1e-10)
            {
                i++;
                if (i == rref.Rows)
                {
                    i = r;
                    lead++;
                    if (lead == rref.Columns)
                        return new MatrixStepsResult(rref.Round(10), steps);
                }
            }
            if (i != r)
            {
                rref.SwapRows(r, i);
                steps.Add($"Swap R{r + 1} ↔ R{i + 1}{Environment.NewLine}{rref.ToDisplayString()}");
            }
            var div = rref[r, lead];
            if (Math.Abs(div - 1) > 1e-10)
            {
                rref.DivideRow(r, div);
                steps.Add($"R{r + 1} = R{r + 1} / {FormatValue(div)}{Environment.NewLine}{rref.ToDisplayString()}");
            }

            for (int j = 0; j < rref.Rows; j++)
            {
                if (j != r)
                {
                    var sub = rref[j, lead];
                    if (Math.Abs(sub) > 1e-10)
                    {
                        rref.SubtractRow(j, r, sub);
                        steps.Add($"R{j + 1} = R{j + 1} - {FormatValue(sub)} × R{r + 1}{Environment.NewLine}{rref.ToDisplayString()}");
                    }
                }
            }
            lead++;
        }
        return new MatrixStepsResult(rref.Round(10), steps);
    }

    private void SwapRows(int row1, int row2)
    {
        for (int col = 0; col < Columns; col++)
        {
            var temp = data[row1, col];
            data[row1, col] = data[row2, col];
            data[row2, col] = temp;
        }
    }

    private void DivideRow(int row, double divisor)
    {
        for (int col = 0; col < Columns; col++)
        {
            data[row, col] /= divisor;
        }
    }

    private void SubtractRow(int row, int subtractRow, double factor)
    {
        for (int col = 0; col < Columns; col++)
        {
            data[row, col] -= factor * data[subtractRow, col];
        }
    }

    /// <summary>
    /// Calculates the inverse of this matrix using row operations and records the intermediate steps.
    /// </summary>
    /// <returns>The inverse matrix and formatted row-operation steps.</returns>
    /// <exception cref="ArgumentException">Thrown when the matrix is not square.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the matrix is singular.</exception>
    public MatrixStepsResult InverseWithSteps()
    {
        if (Rows != Columns)
            throw new ArgumentException("Matrix must be square to calculate the inverse.");

        int n = Rows;
        double[,] left = (double[,])data.Clone();
        double[,] right = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            right[i, i] = 1;
        }

        var steps = new List<string>
        {
            "Start:" + Environment.NewLine + FormatAugmentedMatrix(left, right)
        };

        for (int col = 0; col < n; col++)
        {
            int pivotRow = col;
            while (pivotRow < n && Math.Abs(left[pivotRow, col]) < 1e-10)
            {
                pivotRow++;
            }

            if (pivotRow == n)
                throw new InvalidOperationException("Matrix is not invertible.");

            if (pivotRow != col)
            {
                SwapRows(left, right, pivotRow, col);
                steps.Add($"Swap R{col + 1} ↔ R{pivotRow + 1}{Environment.NewLine}{FormatAugmentedMatrix(left, right)}");
            }

            double pivot = left[col, col];
            if (Math.Abs(pivot - 1) > 1e-10)
            {
                DivideRow(left, right, col, pivot);
                steps.Add($"R{col + 1} = R{col + 1} / {FormatValue(pivot)}{Environment.NewLine}{FormatAugmentedMatrix(left, right)}");
            }

            for (int row = 0; row < n; row++)
            {
                if (row == col)
                    continue;

                double factor = left[row, col];
                if (Math.Abs(factor) > 1e-10)
                {
                    SubtractRow(left, right, row, col, factor);
                    steps.Add($"R{row + 1} = R{row + 1} - {FormatValue(factor)} × R{col + 1}{Environment.NewLine}{FormatAugmentedMatrix(left, right)}");
                }
            }
        }

        Matrix inverse = new Matrix(n, n);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                inverse[i, j] = right[i, j];
            }
        }

        return new MatrixStepsResult(inverse, steps);
    }

}