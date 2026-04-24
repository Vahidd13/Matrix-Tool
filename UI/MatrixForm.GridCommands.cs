/// <summary>
/// Contains commands that modify current matrix grids without performing calculations.
/// </summary>
public partial class MatrixForm
{
    private void ClearGrids(string operation)
    {
        if (operation == AdditionOperation)
        {
            MatrixOperations.ClearGrid(txtMatrix1Addition);
            MatrixOperations.ClearGrid(txtMatrix2Addition);
            MatrixOperations.ClearGrid(txtResultAddition);
        }
        else if (operation == MultiplicationOperation)
        {
            MatrixOperations.ClearGrid(txtMatrix1Multiplication);
            MatrixOperations.ClearGrid(txtMatrix2Multiplication);
            MatrixOperations.ClearGrid(txtResultMultiplication);
        }
        else if (operation == DeterminantOperation)
        {
            MatrixOperations.ClearGrid(txtMatrixDeterminant);
            txtResultDeterminant.Text = string.Empty;
        }
        else if (operation == TransposeOperation)
        {
            MatrixOperations.ClearGrid(txtMatrixTranspose);
            MatrixOperations.ClearGrid(txtResultTranspose);
        }
        else if (operation == InverseOperation)
        {
            MatrixOperations.ClearGrid(txtMatrixInverse);
            MatrixOperations.ClearGrid(txtResultInverse);
            txtStepsInverse.Clear();
        }
        else if (operation == EigenvaluesOperation)
        {
            MatrixOperations.ClearGrid(txtMatrixEigen);
            txtResultEigenvalues.Text = string.Empty;
            txtCharacteristicPolynomial.Text = string.Empty;
        }
        else if (operation == RrefOperation)
        {
            MatrixOperations.ClearGrid(txtMatrixRREF);
            MatrixOperations.ClearGrid(txtResultRREF);
            txtStepsRREF.Clear();
        }
    }

    private void FillZeros(string operation)
    {
        if (operation == AdditionOperation)
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix1Addition);
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix2Addition);
        }
        else if (operation == MultiplicationOperation)
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix1Multiplication);
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix2Multiplication);
        }
        else if (operation == DeterminantOperation)
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixDeterminant);
        }
        else if (operation == TransposeOperation)
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixTranspose);
        }
        else if (operation == InverseOperation)
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixInverse);
        }
        else if (operation == EigenvaluesOperation)
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixEigen);
        }
        else if (operation == RrefOperation)
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixRREF);
        }
    }
}
