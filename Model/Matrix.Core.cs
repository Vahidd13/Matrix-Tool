using System;

/// <summary>
/// Represents a numeric matrix and provides common matrix algorithms used by the application.
/// </summary>
public partial class Matrix
{
    private double[,] data;

    /// <summary>Gets the number of rows in the matrix.</summary>
    public int Rows { get; private set; }
    /// <summary>Gets the number of columns in the matrix.</summary>
    public int Columns { get; private set; }

    /// <summary>
    /// Initializes a matrix with the specified dimensions.
    /// </summary>
    /// <param name="rows">Number of matrix rows.</param>
    /// <param name="columns">Number of matrix columns.</param>
    /// <exception cref="ArgumentException">Thrown when one of the dimensions is not positive.</exception>
    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Matrix dimensions must be positive.");

        Rows = rows;
        Columns = columns;
        data = new double[rows, columns];
    }

    /// <summary>Gets or sets a matrix value by row and column index.</summary>
    /// <param name="row">Zero-based row index.</param>
    /// <param name="col">Zero-based column index.</param>
    /// <returns>The value stored at the selected cell.</returns>
    public double this[int row, int col]
    {
        get { return data[row, col]; }
        set { data[row, col] = value; }
    }

    private Matrix Clone()
    {
        Matrix clone = new Matrix(Rows, Columns);
        Array.Copy(data, clone.data, data.Length);
        return clone;
    }

    /// <summary>
    /// Creates a copy of the matrix with all values rounded to the specified number of decimal places.
    /// </summary>
    /// <param name="decimals">Number of decimal places.</param>
    /// <returns>A rounded copy of the matrix.</returns>
    public Matrix Round(int decimals)
    {
        Matrix rounded = new Matrix(Rows, Columns);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                rounded[i, j] = Math.Round(data[i, j], decimals);
            }
        }
        return rounded;
    }

    /// <summary>
    /// Returns the internal matrix array.
    /// </summary>
    /// <returns>The backing two-dimensional array.</returns>
    public double[,] ToArray()
    {
        return data;
    }
}
