using System;

/// <summary>
/// Contains arithmetic operations such as addition, multiplication, determinant, transpose, and inverse.
/// </summary>
public partial class Matrix
{
    /// <summary>
    /// Adds two matrices of the same dimensions.
    /// </summary>
    /// <param name="a">First matrix.</param>
    /// <param name="b">Second matrix.</param>
    /// <returns>The element-wise sum of the two matrices.</returns>
    /// <exception cref="ArgumentException">Thrown when the matrix dimensions do not match.</exception>
    public static Matrix operator +(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
            throw new ArgumentException("Matrices must have the same dimensions for addition.");

        Matrix result = new Matrix(a.Rows, a.Columns);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++)
            {
                result[i, j] = a[i, j] + b[i, j];
            }
        }
        return result;
    }

    /// <summary>
    /// Multiplies two compatible matrices.
    /// </summary>
    /// <param name="a">Left matrix.</param>
    /// <param name="b">Right matrix.</param>
    /// <returns>The matrix product.</returns>
    /// <exception cref="ArgumentException">Thrown when the number of columns in <paramref name="a"/> does not equal the number of rows in <paramref name="b"/>.</exception>
    public static Matrix operator *(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
            throw new ArgumentException("Number of columns in the first matrix must equal the number of rows in the second matrix.");

        Matrix result = new Matrix(a.Rows, b.Columns);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < b.Columns; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < a.Columns; k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Calculates the determinant of a square matrix.
    /// </summary>
    /// <returns>The determinant value.</returns>
    /// <exception cref="ArgumentException">Thrown when the matrix is not square.</exception>
    public double Determinant()
    {
        if (Rows != Columns)
            throw new ArgumentException("Matrix must be square to calculate the determinant.");

        return Determinant(data);
    }

    private double Determinant(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        if (n == 1)
            return matrix[0, 0];

        double det = 0;
        for (int j = 0; j < n; j++)
        {
            double value = matrix[0, j] * Determinant(CalculateMinor(matrix, 0, j));
            det += (j % 2 == 0) ? value : -value;
        }
        return det;
    }

    private double[,] CalculateMinor(double[,] matrix, int row, int column)
    {
        int n = matrix.GetLength(0);
        double[,] minor = new double[n - 1, n - 1];
        int rowIndex = 0, colIndex;

        for (int i = 0; i < n; i++)
        {
            if (i == row) continue;

            colIndex = 0;
            for (int j = 0; j < n; j++)
            {
                if (j == column) continue;
                minor[rowIndex, colIndex] = matrix[i, j];
                colIndex++;
            }
            rowIndex++;
        }
        return minor;
    }

    /// <summary>
    /// Creates a transposed copy of this matrix.
    /// </summary>
    /// <returns>A new matrix where rows and columns are swapped.</returns>
    public Matrix Transpose()
    {
        Matrix transposed = new Matrix(Columns, Rows);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                transposed[j, i] = data[i, j];
            }
        }
        return transposed;
    }

    /// <summary>
    /// Calculates the inverse of this square matrix.
    /// </summary>
    /// <returns>The inverse matrix.</returns>
    /// <exception cref="ArgumentException">Thrown when the matrix is not square.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the matrix is singular.</exception>
    public Matrix Inverse()
    {
        if (Rows != Columns)
            throw new ArgumentException("Matrix must be square to calculate the inverse.");

        double det = Determinant();
        if (det == 0)
            throw new InvalidOperationException("Matrix is not invertible.");

        Matrix result = new Matrix(Rows, Columns);
        double[,] adjoint = Adjoint(data);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                result[i, j] = adjoint[i, j] / det;
            }
        }
        return result;
    }

    private double[,] Adjoint(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        double[,] adjoint = new double[n, n];

        if (n == 1)
        {
            adjoint[0, 0] = 1;
            return adjoint;
        }

        int sign = 1;
        double[,] temp = new double[n - 1, n - 1];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                GetCofactor(matrix, temp, i, j, n);
                sign = ((i + j) % 2 == 0) ? 1 : -1;
                adjoint[j, i] = sign * Determinant(temp);
            }
        }
        return adjoint;
    }

    private void GetCofactor(double[,] matrix, double[,] temp, int p, int q, int n)
    {
        int i = 0, j = 0;
        for (int row = 0; row < n; row++)
        {
            for (int col = 0; col < n; col++)
            {
                if (row != p && col != q)
                {
                    temp[i, j++] = matrix[row, col];
                    if (j == n - 1)
                    {
                        j = 0;
                        i++;
                    }
                }
            }
        }
    }
}
