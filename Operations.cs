using System;
using System.Linq;
using Expr = MathNet.Symbolics.SymbolicExpression;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

public class Matrix
{
    private double[,] data;

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Matrix dimensions must be positive.");

        Rows = rows;
        Columns = columns;
        data = new double[rows, columns];
    }

    public double this[int row, int col]
    {
        get { return data[row, col]; }
        set { data[row, col] = value; }
    }

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

    private Matrix Clone()
    {
        Matrix clone = new Matrix(Rows, Columns);
        Array.Copy(data, clone.data, data.Length);
        return clone;
    }

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

    public double[,] ToArray()
    {
        return data;
    }

    // Calculates the eigenvalues of a matrix. Complex eigenvalues are supported and displayed as a ± bi.
    public Tuple<string[], string> Eigenvalues()
    {
        if (Rows != Columns)
            throw new ArgumentException("Matrix must be square to calculate eigenvalues.");

        var mat = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.DenseOfArray(data);
        var evd = mat.Evd();
        var eigenvalues = evd.EigenValues.Select(FormatComplexEigenvalue).ToArray();

        // Compute the characteristic polynomial
        var characteristicPolynomial = GetCharacteristicPolynomial();

        return new Tuple<string[], string>(eigenvalues, characteristicPolynomial);
    }

    private static string FormatComplexEigenvalue(Complex value)
    {
        const double tolerance = 1e-10;
        double real = Math.Abs(value.Real) < tolerance ? 0 : value.Real;
        double imaginary = Math.Abs(value.Imaginary) < tolerance ? 0 : value.Imaginary;

        if (imaginary == 0)
        {
            return DoubleToFraction(real);
        }

        string imaginaryPart = FormatImaginaryPart(Math.Abs(imaginary));
        if (real == 0)
        {
            return imaginary > 0 ? imaginaryPart : "-" + imaginaryPart;
        }

        string sign = imaginary > 0 ? " + " : " - ";
        return $"{DoubleToFraction(real)}{sign}{imaginaryPart}";
    }

    private static string FormatImaginaryPart(double magnitude)
    {
        return Math.Abs(magnitude - 1) < 1e-10 ? "i" : $"{DoubleToFraction(magnitude)}i";
    }

    // Gets the characteristic polynomial of a matrix
    private string GetCharacteristicPolynomial()
    {
        var polynomial = CharacteristicPolynomial();
        return polynomial;
    }

    // Calculates the characteristic polynomial of a matrix
    private string CharacteristicPolynomial()
    {
        int n = Rows;
        var symbolicMatrix = BuildSymbolicMatrix(n);
        var detExpression = ComputeDeterminant(symbolicMatrix, n);

        if (n % 2 != 0) {
            detExpression = -1 * detExpression;
        }
        return FormatPolynomial(detExpression, "λ");
    }

    // Builds a symbolic matrix for characteristic polynomial calculation
    private Expr[,] BuildSymbolicMatrix(int size)
    {
        var symbolicMatrix = new Expr[size, size];
        var lambda = Expr.Variable("λ");
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j)
                {
                    symbolicMatrix[i, j] = lambda - data[i, j];
                }
                else
                {
                    symbolicMatrix[i, j] = -data[i, j];
                }
            }
        }
        return symbolicMatrix;
    }

    // Computes the determinant of a symbolic matrix
    private static Expr ComputeDeterminant(Expr[,] matrix, int size)
    {
        if (size == 1)
        {
            return matrix[0, 0];
        }

        Expr det = Expr.Zero;
        for (int j = 0; j < size; j++)
        {
            var minor = Minor(matrix, size, 0, j);
            det += (j % 2 == 0 ? Expr.One : Expr.MinusOne) * matrix[0, j] * ComputeDeterminant(minor, size - 1);
        }

        return det;
    }

    // Calculates the minor of a symbolic matrix
    private static Expr[,] Minor(Expr[,] matrix, int size, int row, int column)
    {
        var minor = new Expr[size - 1, size - 1];
        for (int i = 0, mi = 0; i < size; i++)
        {
            if (i == row) continue;
            for (int j = 0, mj = 0; j < size; j++)
            {
                if (j == column) continue;
                minor[mi, mj] = matrix[i, j];
                mj++;
            }
            mi++;
        }
        return minor;
    }

    // Formats a polynomial expression
    private static string FormatPolynomial(Expr polynomial, string variable)
    {
        var expandedPolynomial = polynomial.Expand().ToString();
        var terms = Regex.Split(expandedPolynomial, @"(?=[+-])")
                         .Where(term => !string.IsNullOrWhiteSpace(term))
                         .Select(term => term.Trim())
                         .ToList();

        var formattedTerms = new List<(int degree, string term, bool isNegative)>();

        foreach (var term in terms)
        {
            string cleanedTerm = term.Trim();
            bool isNegative = cleanedTerm.StartsWith("-");
            cleanedTerm = cleanedTerm.TrimStart('+', '-').Trim();

            string formattedTerm = FormatCoefficient(cleanedTerm);
            int degree = GetDegree(formattedTerm, variable);
            formattedTerms.Add((degree, formattedTerm, isNegative));
        }

        // Sort terms by degree in descending order
        formattedTerms = formattedTerms.OrderByDescending(t => t.degree).ToList();

        var result = new System.Text.StringBuilder();
        result.Append("p(").Append(variable).Append(") = ");
        
        bool isFirstTerm = true;
        foreach (var (degree, formattedTerm, isNegative) in formattedTerms)
        {
            if (isFirstTerm)
            {
                if (isNegative)
                {
                    result.Append("- ").Append(formattedTerm);
                }
                else
                {
                    result.Append(formattedTerm);
                }
            }
            else
            {
                if (isNegative)
                {
                    result.Append(" - ").Append(formattedTerm);
                }
                else
                {
                    result.Append(" + ").Append(formattedTerm);
                }
            }
            isFirstTerm = false;
        }

        return result.ToString().Replace(" - -", " + ").Replace(" + -", " - ");
    }

    // Formats the coefficient in a term of a polynomial
    private static string FormatCoefficient(string term)
    {
        string pattern = @"-?\d+(\.\d+)?";
        var matches = Regex.Matches(term, pattern);

        foreach (Match match in matches)
        {
            if (double.TryParse(match.Value, out double value))
            {
                if (value == (int)value)
                {
                    term = term.Replace(match.Value, ((int)value).ToString());
                }
            }
        }

        return term;
    }

    // Gets the degree of a term in a polynomial
    private static int GetDegree(string term, string variable)
    {
        if (term.Contains($"{variable}^"))
        {
            int index = term.IndexOf($"{variable}^") + $"{variable}^".Length;
            string degreeStr = "";
            while (index < term.Length && char.IsDigit(term[index]))
            {
                degreeStr += term[index];
                index++;
            }
            return int.Parse(degreeStr);
        }
        else if (term.Contains(variable))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}

public class MatrixStepsResult
{
    public MatrixStepsResult(Matrix result, List<string> steps)
    {
        Result = result;
        Steps = steps;
    }

    public Matrix Result { get; }
    public List<string> Steps { get; }
}
