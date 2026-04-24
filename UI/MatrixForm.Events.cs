using System;
using System.Windows.Forms;

/// <summary>
/// Contains event handlers for tab changes and matrix size controls.
/// </summary>
public partial class MatrixForm
{
    private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentOperation = tabControl.SelectedTab.Text;
    }

    private void btnSetSize_Click(object sender, EventArgs e, string operation)
    {
        if (operation == AdditionOperation)
        {
            matrixRowsAddition = (int)numRowsAddition.Value;
            matrixColsAddition = (int)numColsAddition.Value;
            InitializeMatrixInputs(matrixRowsAddition, matrixColsAddition, tabAddition, operation);
        }
        else if (operation == DeterminantOperation)
        {
            matrixRowsDeterminant = (int)numRowsDeterminant.Value;
            InitializeMatrixInputs(matrixRowsDeterminant, matrixRowsDeterminant, tabDeterminant, operation);
        }
        else if (operation == TransposeOperation)
        {
            matrixRowsTranspose = (int)numRowsTranspose.Value;
            matrixColsTranspose = (int)numColsTranspose.Value;
            InitializeMatrixInputs(matrixRowsTranspose, matrixColsTranspose, tabTranspose, operation);
        }
        else if (operation == InverseOperation)
        {
            matrixRowsInverse = (int)numRowsInverse.Value;
            matrixColsInverse = matrixRowsInverse;
            InitializeMatrixInputs(matrixRowsInverse, matrixColsInverse, tabInverse, operation);
        }
        else if (operation == EigenvaluesOperation)
        {
            matrixRowsEigen = (int)numRowsEigen.Value;
            matrixColsEigen = matrixRowsEigen;
            InitializeMatrixInputs(matrixRowsEigen, matrixColsEigen, tabEigenvalues, operation);
        }
        else if (operation == RrefOperation)
        {
            matrixRowsRREF = (int)numRowsRREF.Value;
            matrixColsRREF = (int)numColsRREF.Value;
            InitializeMatrixInputs(matrixRowsRREF, matrixColsRREF, tabRREF, operation);
        }
    }

    private void btnSetSizeMatrix1_Click(object sender, EventArgs e)
    {
        matrixRowsMatrix1 = (int)numRowsMatrix1Multiplication.Value;
        matrixColsMatrix1 = (int)numColsMatrix1Multiplication.Value;

        // Matrix multiplication requires columns(A) == rows(B).
        matrixRowsMatrix2 = matrixColsMatrix1;
        numRowsMatrix2Multiplication.Value = matrixRowsMatrix2;

        InitializeMatrixInputs(matrixRowsMatrix1, matrixColsMatrix1, tabMultiplication, MultiplicationOperation);
    }

    private void btnSetSizeMatrix2_Click(object sender, EventArgs e)
    {
        matrixRowsMatrix2 = (int)numRowsMatrix2Multiplication.Value;
        matrixColsMatrix2 = (int)numColsMatrix2Multiplication.Value;

        if (matrixRowsMatrix2 != matrixColsMatrix1)
        {
            MessageBox.Show("The number of rows in the second matrix must match the number of columns in the first matrix.");
            numRowsMatrix2Multiplication.Value = matrixColsMatrix1;
            matrixRowsMatrix2 = matrixColsMatrix1;
        }

        InitializeMatrixInputs(matrixRowsMatrix2, matrixColsMatrix2, tabMultiplication, MultiplicationOperation);
    }
}
