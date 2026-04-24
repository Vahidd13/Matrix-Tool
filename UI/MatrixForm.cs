using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Main Windows Forms window that contains all matrix operation tabs and UI event handling.
/// </summary>
public partial class MatrixForm : Form
{
    private const string AdditionOperation = "Addition";
    private const string MultiplicationOperation = "Multiplication";
    private const string DeterminantOperation = "Determinant";
    private const string TransposeOperation = "Transpose";
    private const string InverseOperation = "Inverse";
    private const string EigenvaluesOperation = "Eigenvalues";
    private const string RrefOperation = "RREF";

    // Matrix dimensions for each operation tab.
    private int matrixRowsAddition = 3;
    private int matrixColsAddition = 3;

    private int matrixRowsMatrix1 = 3;
    private int matrixColsMatrix1 = 3;
    private int matrixRowsMatrix2 = 3;
    private int matrixColsMatrix2 = 3;

    private int matrixRowsDeterminant = 3;

    private int matrixRowsTranspose = 3;
    private int matrixColsTranspose = 3;

    private int matrixRowsInverse = 3;
    private int matrixColsInverse = 3;

    private int matrixRowsEigen = 3;
    private int matrixColsEigen = 3;

    private int matrixRowsRREF = 3;
    private int matrixColsRREF = 3;

    private string currentOperation;
    private readonly Font baseFont = new Font("Segoe UI", 9F);
    private readonly Color accentColor = Color.SteelBlue;
    private readonly Color neutralButtonColor = Color.Gainsboro;

    /// <summary>
    /// Initializes the form, applies the UI theme, and creates default matrix grids.
    /// </summary>
    public MatrixForm()
    {
        InitializeComponent();
        ApplyUiTheme();
        InitializeDefaultMatrixInputs();

        tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
    }

    private void InitializeDefaultMatrixInputs()
    {
        InitializeMatrixInputs(matrixRowsAddition, matrixColsAddition, tabAddition, AdditionOperation);
        InitializeMatrixInputs(matrixRowsMatrix1, matrixColsMatrix1, tabMultiplication, MultiplicationOperation);
        InitializeMatrixInputs(matrixRowsDeterminant, matrixRowsDeterminant, tabDeterminant, DeterminantOperation);
        InitializeMatrixInputs(matrixRowsTranspose, matrixColsTranspose, tabTranspose, TransposeOperation);
        InitializeMatrixInputs(matrixRowsInverse, matrixColsInverse, tabInverse, InverseOperation);
        InitializeMatrixInputs(matrixRowsEigen, matrixColsEigen, tabEigenvalues, EigenvaluesOperation);
        InitializeMatrixInputs(matrixRowsRREF, matrixColsRREF, tabRREF, RrefOperation);
    }
}
