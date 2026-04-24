using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Creates, removes, and assigns the dynamic matrix input/result grids.
/// </summary>
public partial class MatrixForm
{
    private const int GridStartX = 20;
    private const int GridStartY = 70;

    private void InitializeMatrixInputs(int rows, int cols, TabPage tabPage, string operation)
    {
        switch (operation)
        {
            case AdditionOperation:
                InitializeAdditionGrid(rows, cols, tabPage);
                break;
            case MultiplicationOperation:
                InitializeMultiplicationGrid(tabPage);
                break;
            case DeterminantOperation:
                InitializeDeterminantGrid(rows, cols, tabPage);
                break;
            case TransposeOperation:
                InitializeTransposeGrid(rows, cols, tabPage);
                break;
            case InverseOperation:
                InitializeInverseGrid(rows, cols, tabPage);
                break;
            case EigenvaluesOperation:
                InitializeEigenvaluesGrid(rows, cols, tabPage);
                break;
            case RrefOperation:
                InitializeRrefGrid(rows, cols, tabPage);
                break;
        }
    }

    private void InitializeAdditionGrid(int rows, int cols, TabPage tabPage)
    {
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrix1Addition);
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrix2Addition);
        TextBoxGridFactory.RemoveGrid(tabPage, txtResultAddition);

        int matrix2X = GridStartX + TextBoxGridFactory.GridWidth(cols) + TextBoxGridFactory.BlockSpacing;
        int resultX = matrix2X + TextBoxGridFactory.GridWidth(cols) + TextBoxGridFactory.BlockSpacing;

        txtMatrix1Addition = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, GridStartX, GridStartY);
        txtMatrix2Addition = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, matrix2X, GridStartY);
        txtResultAddition = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, resultX, GridStartY, readOnly: true);
    }

    private void InitializeMultiplicationGrid(TabPage tabPage)
    {
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrix1Multiplication);
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrix2Multiplication);
        TextBoxGridFactory.RemoveGrid(tabPage, txtResultMultiplication);

        int matrix2X = GridStartX + TextBoxGridFactory.GridWidth(matrixColsMatrix1) + TextBoxGridFactory.BlockSpacing;
        int resultX = matrix2X + TextBoxGridFactory.GridWidth(matrixColsMatrix2) + TextBoxGridFactory.BlockSpacing;

        txtMatrix1Multiplication = TextBoxGridFactory.CreateGrid(
            tabPage,
            matrixRowsMatrix1,
            matrixColsMatrix1,
            GridStartX,
            GridStartY);

        txtMatrix2Multiplication = TextBoxGridFactory.CreateGrid(
            tabPage,
            matrixRowsMatrix2,
            matrixColsMatrix2,
            matrix2X,
            GridStartY);

        txtResultMultiplication = TextBoxGridFactory.CreateGrid(
            tabPage,
            matrixRowsMatrix1,
            matrixColsMatrix2,
            resultX,
            GridStartY,
            readOnly: true);
    }

    private void InitializeDeterminantGrid(int rows, int cols, TabPage tabPage)
    {
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrixDeterminant);

        if (txtResultDeterminant != null)
        {
            tabPage.Controls.Remove(txtResultDeterminant);
            txtResultDeterminant.Dispose();
        }

        txtMatrixDeterminant = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, GridStartX, GridStartY);

        txtResultDeterminant = new TextBox
        {
            Location = new Point(
                GridStartX + TextBoxGridFactory.GridWidth(cols) + TextBoxGridFactory.BlockSpacing,
                GridStartY),
            Size = new Size(TextBoxGridFactory.CellSize * 3, TextBoxGridFactory.CellSize),
            ReadOnly = true,
            Font = new Font("Arial", 12, FontStyle.Bold),
            ForeColor = Color.DarkBlue
        };

        tabPage.Controls.Add(txtResultDeterminant);
    }

    private void InitializeTransposeGrid(int rows, int cols, TabPage tabPage)
    {
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrixTranspose);
        TextBoxGridFactory.RemoveGrid(tabPage, txtResultTranspose);

        int resultX = GridStartX + TextBoxGridFactory.GridWidth(cols) + TextBoxGridFactory.BlockSpacing;

        txtMatrixTranspose = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, GridStartX, GridStartY);
        txtResultTranspose = TextBoxGridFactory.CreateGrid(tabPage, cols, rows, resultX, GridStartY, readOnly: true);
    }

    private void InitializeInverseGrid(int rows, int cols, TabPage tabPage)
    {
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrixInverse);
        TextBoxGridFactory.RemoveGrid(tabPage, txtResultInverse);

        int resultX = GridStartX + TextBoxGridFactory.GridWidth(cols) + TextBoxGridFactory.BlockSpacing;

        txtMatrixInverse = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, GridStartX, GridStartY);
        txtResultInverse = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, resultX, GridStartY, readOnly: true);
    }

    private void InitializeEigenvaluesGrid(int rows, int cols, TabPage tabPage)
    {
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrixEigen);
        txtMatrixEigen = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, GridStartX, GridStartY);
    }

    private void InitializeRrefGrid(int rows, int cols, TabPage tabPage)
    {
        TextBoxGridFactory.RemoveGrid(tabPage, txtMatrixRREF);
        TextBoxGridFactory.RemoveGrid(tabPage, txtResultRREF);

        int resultX = GridStartX + TextBoxGridFactory.GridWidth(cols) + TextBoxGridFactory.BlockSpacing;

        txtMatrixRREF = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, GridStartX, GridStartY);
        txtResultRREF = TextBoxGridFactory.CreateGrid(tabPage, rows, cols, resultX, GridStartY, readOnly: true);
    }
}
