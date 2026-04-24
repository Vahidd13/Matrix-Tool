using System;
using System.Windows.Forms;

/// <summary>
/// Contains calculation dispatching and operation-specific UI result display.
/// </summary>
public partial class MatrixForm
{
    private void btnCalculate_Click(object sender, EventArgs e, string operation)
    {
        try
        {
            switch (operation)
            {
                case AdditionOperation:
                    CalculateAddition();
                    break;
                case MultiplicationOperation:
                    CalculateMultiplication();
                    break;
                case DeterminantOperation:
                    CalculateDeterminant();
                    break;
                case TransposeOperation:
                    CalculateTranspose();
                    break;
                case InverseOperation:
                    CalculateInverse();
                    break;
                case RrefOperation:
                    CalculateRref();
                    break;
                case EigenvaluesOperation:
                    CalculateEigenvalues();
                    break;
            }
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is FormatException)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CalculateAddition()
    {
        Matrix matrix1 = MatrixOperations.ParseMatrix(txtMatrix1Addition);
        Matrix matrix2 = MatrixOperations.ParseMatrix(txtMatrix2Addition);
        Matrix result = matrix1 + matrix2;

        MatrixOperations.DisplayResult(result, txtResultAddition);
        AppendHistoryEntry(AdditionOperation, $"{matrix1.ToDisplayString()}{Environment.NewLine}+{Environment.NewLine}{matrix2.ToDisplayString()}{Environment.NewLine}={Environment.NewLine}{result.ToDisplayString()}");
    }

    private void CalculateMultiplication()
    {
        Matrix matrix1 = MatrixOperations.ParseMatrix(txtMatrix1Multiplication);
        Matrix matrix2 = MatrixOperations.ParseMatrix(txtMatrix2Multiplication);
        Matrix result = matrix1 * matrix2;

        MatrixOperations.DisplayResult(result, txtResultMultiplication);
        AppendHistoryEntry(MultiplicationOperation, $"{matrix1.ToDisplayString()}{Environment.NewLine}×{Environment.NewLine}{matrix2.ToDisplayString()}{Environment.NewLine}={Environment.NewLine}{result.ToDisplayString()}");
    }

    private void CalculateDeterminant()
    {
        Matrix matrix = MatrixOperations.ParseMatrix(txtMatrixDeterminant);
        double result = matrix.Determinant();

        txtResultDeterminant.Text = result.ToString();
        AppendHistoryEntry(DeterminantOperation, $"{matrix.ToDisplayString()}{Environment.NewLine}={Environment.NewLine}{result}");
    }

    private void CalculateTranspose()
    {
        Matrix matrix = MatrixOperations.ParseMatrix(txtMatrixTranspose);
        Matrix result = matrix.Transpose();

        MatrixOperations.DisplayResult(result, txtResultTranspose);
        AppendHistoryEntry(TransposeOperation, $"{matrix.ToDisplayString()}{Environment.NewLine}={Environment.NewLine}{result.ToDisplayString()}");
    }

    private void CalculateInverse()
    {
        Matrix matrix = MatrixOperations.ParseMatrix(txtMatrixInverse);
        var inverseResult = matrix.InverseWithSteps();

        MatrixOperations.DisplayResult(inverseResult.Result, txtResultInverse);
        txtStepsInverse.Text = string.Join(Environment.NewLine + Environment.NewLine, inverseResult.Steps);
        AppendHistoryEntry(InverseOperation, $"{matrix.ToDisplayString()}{Environment.NewLine}={Environment.NewLine}{inverseResult.Result.ToDisplayString()}");
    }

    private void CalculateRref()
    {
        Matrix matrix = MatrixOperations.ParseMatrix(txtMatrixRREF);
        var rrefResult = Matrix.RrefWithSteps(matrix);

        MatrixOperations.DisplayResult(rrefResult.Result, txtResultRREF);
        txtStepsRREF.Text = string.Join(Environment.NewLine + Environment.NewLine, rrefResult.Steps);
        AppendHistoryEntry(RrefOperation, $"{matrix.ToDisplayString()}{Environment.NewLine}={Environment.NewLine}{rrefResult.Result.ToDisplayString()}");
    }

    private void CalculateEigenvalues()
    {
        Matrix matrix = MatrixOperations.ParseMatrix(txtMatrixEigen);
        var result = matrix.Eigenvalues();

        txtResultEigenvalues.Text = string.Join(", ", result.Item1);
        txtCharacteristicPolynomial.Text = result.Item2;
        AppendHistoryEntry(EigenvaluesOperation, $"{matrix.ToDisplayString()}{Environment.NewLine}Eigenvalues: {string.Join(", ", result.Item1)}{Environment.NewLine}{result.Item2}");
    }
}
