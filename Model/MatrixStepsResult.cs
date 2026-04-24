using System.Collections.Generic;

/// <summary>
/// Stores a matrix result together with the textual steps that produced it.
/// </summary>
public class MatrixStepsResult
{
    /// <summary>
    /// Initializes a result object with a final matrix and step descriptions.
    /// </summary>
    /// <param name="result">Final matrix result.</param>
    /// <param name="steps">Formatted calculation steps.</param>
    public MatrixStepsResult(Matrix result, List<string> steps)
    {
        Result = result;
        Steps = steps;
    }

    /// <summary>Gets the final matrix result.</summary>
    public Matrix Result { get; }
    /// <summary>Gets the formatted calculation steps.</summary>
    public List<string> Steps { get; }
}
