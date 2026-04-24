using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Expr = MathNet.Symbolics.SymbolicExpression;

/// <summary>
/// Contains eigenvalue calculation and symbolic characteristic polynomial formatting.
/// </summary>
public partial class Matrix
{
    // Calculates the eigenvalues of a matrix. Complex eigenvalues are supported and displayed as a ± bi.
    /// <summary>
    /// Calculates eigenvalues and the characteristic polynomial for a square matrix.
    /// </summary>
    /// <returns>A tuple containing formatted eigenvalues and the characteristic polynomial.</returns>
    /// <exception cref="ArgumentException">Thrown when the matrix is not square.</exception>
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
