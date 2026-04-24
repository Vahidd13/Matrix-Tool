using System.Drawing;
using System.Windows.Forms;

public partial class MatrixForm : Form
{
    private TabControl tabControl;
    private TabPage tabAddition;
    private TabPage tabMultiplication;
    private TabPage tabDeterminant;
    private TabPage tabTranspose;
    private TabPage tabInverse;
    private TabPage tabEigenvalues;
    private TabPage tabRREF; // New Tab for RREF
    private TabPage tabHistory;
    private TextBox[,] txtMatrix1Addition;
    private TextBox[,] txtMatrix2Addition;
    private TextBox[,] txtResultAddition;
    private TextBox[,] txtMatrix1Multiplication;
    private TextBox[,] txtMatrix2Multiplication;
    private TextBox[,] txtResultMultiplication;
    private TextBox[,] txtMatrixDeterminant;
    private TextBox txtResultDeterminant;
    private TextBox[,] txtMatrixTranspose;
    private TextBox[,] txtResultTranspose;
    private TextBox[,] txtMatrixInverse;
    private TextBox[,] txtResultInverse;
    private TextBox[,] txtMatrixEigen;
    private TextBox[,] txtMatrixRREF; // New TextBox array for RREF
    private TextBox[,] txtResultRREF;
    private TextBox txtResultEigenvalues;
    private TextBox txtCharacteristicPolynomial;
    private TextBox txtStepsInverse;
    private TextBox txtStepsRREF;
    private TextBox txtHistory;
    private Label lblStepsInverse;
    private Label lblStepsRREF;
    private Button btnCalculateAddition;
    private Button btnCalculateMultiplication;
    private Button btnCalculateDeterminant;
    private Button btnCalculateTranspose;
    private Button btnCalculateInverse;
    private Button btnCalculateEigenvalues;
    private Button btnCalculateRREF;
    private NumericUpDown numRowsAddition;
    private NumericUpDown numColsAddition;
    private NumericUpDown numRowsMatrix1Multiplication;
    private NumericUpDown numColsMatrix1Multiplication;
    private NumericUpDown numRowsMatrix2Multiplication;
    private NumericUpDown numColsMatrix2Multiplication;
    private NumericUpDown numRowsDeterminant;
    private NumericUpDown numRowsTranspose;
    private NumericUpDown numColsTranspose;
    private NumericUpDown numRowsInverse;
    private NumericUpDown numRowsEigen;
    private NumericUpDown numRowsRREF; // New NumericUpDown for RREF rows
    private NumericUpDown numColsRREF;
    private Button btnSetSizeAddition;
    private Button btnSetSizeMatrix1Multiplication;
    private Button btnSetSizeMatrix2Multiplication;
    private Button btnSetSizeDeterminant;
    private Button btnSetSizeTranspose;
    private Button btnSetSizeInverse;
    private Button btnSetSizeEigenvalues;
    private Button btnSetSizeRREF;
    private Button btnClearAddition;
    private Button btnFillZeroAddition;
    private Button btnClearMultiplication;
    private Button btnFillZeroMultiplication;
    private Button btnClearDeterminant;
    private Button btnFillZeroDeterminant;
    private Button btnClearTranspose;
    private Button btnFillZeroTranspose;
    private Button btnClearInverse;
    private Button btnFillZeroInverse;
    private Button btnClearEigenvalues; // New Button for Eigenvalues clear
    private Button btnFillZeroEigenvalues;
    private Button btnClearRREF; // New Button for clearing RREF
    private Button btnFillZeroRREF; // New Button for filling RREF with zeros
    private Button btnClearHistory;

    private void InitializeComponent()
    {
        // Set the form title and size
        this.Text = "Matrix Tool";
        this.Size = new Size(1200, 700);
        this.BackColor = Color.White;

        // Initialize TabControl and TabPages
        tabControl = new TabControl
        {
            Location = new Point(10, 10),
            Size = new Size(1160, 650),
            Font = new Font("Arial", 10, FontStyle.Regular)
        };
        tabAddition = new TabPage("Addition") { BackColor = Color.White };
        tabMultiplication = new TabPage("Multiplication") { BackColor = Color.White };
        tabDeterminant = new TabPage("Determinant") { BackColor = Color.White };
        tabTranspose = new TabPage("Transpose") { BackColor = Color.White };
        tabInverse = new TabPage("Inverse") { BackColor = Color.White };
        tabEigenvalues = new TabPage("Eigenvalues") { BackColor = Color.White };
        tabRREF = new TabPage("RREF") { BackColor = Color.White }; // New Tab for RREF
        tabHistory = new TabPage("History") { BackColor = Color.White };

        // Add TabPages to TabControl
        tabControl.TabPages.Add(tabAddition);
        tabControl.TabPages.Add(tabMultiplication);
        tabControl.TabPages.Add(tabDeterminant);
        tabControl.TabPages.Add(tabTranspose);
        tabControl.TabPages.Add(tabInverse);
        tabControl.TabPages.Add(tabEigenvalues);
        tabControl.TabPages.Add(tabRREF); // Add RREF tab to TabControl
        tabControl.TabPages.Add(tabHistory);

        // Initialize controls for Addition tab
        numRowsAddition = new NumericUpDown { Location = new Point(10, 10), Minimum = 1, Maximum = 10, Value = 3 };
        numColsAddition = new NumericUpDown { Location = new Point(130, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeAddition = new Button
        {
            Location = new Point(260, 10),
            Size = new Size(100, 30),
            Text = "Set Size",
            Font = new Font("Arial", 9)
        };
        btnSetSizeAddition.Click += (sender, e) => btnSetSize_Click(sender, e, "Addition");

        btnCalculateAddition = new Button
        {
            Location = new Point(10, 500),
            Size = new Size(100, 30),
            Text = "Calculate",
            BackColor = Color.LightBlue
        };
        btnCalculateAddition.Click += (sender, e) => btnCalculate_Click(sender, e, "Addition");
        ApplyButtonEffects(btnCalculateAddition);

        btnClearAddition = new Button
        {
            Location = new Point(120, 500),
            Size = new Size(100, 30),
            Text = "Clear",
            BackColor = Color.LightCoral
        };
        btnClearAddition.Click += (sender, e) => ClearGrids("Addition");
        ApplyButtonEffects(btnClearAddition);

        btnFillZeroAddition = new Button
        {
            Location = new Point(230, 500),
            Size = new Size(100, 30),
            Text = "Fill Zeros",
            BackColor = Color.LightGreen
        };
        btnFillZeroAddition.Click += (sender, e) => FillZeros("Addition");
        ApplyButtonEffects(btnFillZeroAddition);

        tabAddition.Controls.Add(numRowsAddition);
        tabAddition.Controls.Add(numColsAddition);
        tabAddition.Controls.Add(btnSetSizeAddition);
        tabAddition.Controls.Add(btnCalculateAddition);
        tabAddition.Controls.Add(btnClearAddition);
        tabAddition.Controls.Add(btnFillZeroAddition);

        // Initialize controls for Multiplication tab
        numRowsMatrix1Multiplication = new NumericUpDown { Location = new Point(10, 10), Minimum = 1, Maximum = 10, Value = 3 };
        numColsMatrix1Multiplication = new NumericUpDown { Location = new Point(130, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeMatrix1Multiplication = new Button
        {
            Location = new Point(260, 10),
            Size = new Size(150, 30),
            Text = "Set Size Matrix 1",
        };
        btnSetSizeMatrix1Multiplication.Click += btnSetSizeMatrix1_Click;

        numRowsMatrix2Multiplication = new NumericUpDown { Location = new Point(420, 10), Minimum = 1, Maximum = 10, Value = 3 };
        numColsMatrix2Multiplication = new NumericUpDown { Location = new Point(540, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeMatrix2Multiplication = new Button
        {
            Location = new Point(670, 10),
            Size = new Size(150, 30),
            Text = "Set Size Matrix 2",
            Font = new Font("Arial", 9)
        };
        btnSetSizeMatrix2Multiplication.Click += btnSetSizeMatrix2_Click;

        btnCalculateMultiplication = new Button
        {
            Location = new Point(10, 500),
            Size = new Size(100, 30),
            Text = "Calculate",
            BackColor = Color.LightBlue
        };
        btnCalculateMultiplication.Click += (sender, e) => btnCalculate_Click(sender, e, "Multiplication");
        ApplyButtonEffects(btnCalculateMultiplication);

        btnClearMultiplication = new Button
        {
            Location = new Point(120, 500),
            Size = new Size(100, 30),
            Text = "Clear",
            BackColor = Color.LightCoral
        };
        btnClearMultiplication.Click += (sender, e) => ClearGrids("Multiplication");
        ApplyButtonEffects(btnClearMultiplication);

        btnFillZeroMultiplication = new Button
        {
            Location = new Point(230, 500),
            Size = new Size(100, 30),
            Text = "Fill Zeros",
            BackColor = Color.LightGreen
        };
        btnFillZeroMultiplication.Click += (sender, e) => FillZeros("Multiplication");
        ApplyButtonEffects(btnFillZeroMultiplication);

        tabMultiplication.Controls.Add(numRowsMatrix1Multiplication);
        tabMultiplication.Controls.Add(numColsMatrix1Multiplication);
        tabMultiplication.Controls.Add(btnSetSizeMatrix1Multiplication);
        tabMultiplication.Controls.Add(numRowsMatrix2Multiplication);
        tabMultiplication.Controls.Add(numColsMatrix2Multiplication);
        tabMultiplication.Controls.Add(btnSetSizeMatrix2Multiplication);
        tabMultiplication.Controls.Add(btnCalculateMultiplication);
        tabMultiplication.Controls.Add(btnClearMultiplication);
        tabMultiplication.Controls.Add(btnFillZeroMultiplication);

        // Initialize controls for Determinant tab
        numRowsDeterminant = new NumericUpDown { Location = new Point(10, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeDeterminant = new Button
        {
            Location = new Point(130, 10),
            Size = new Size(100, 30),
            Text = "Set Size",
        };
        btnSetSizeDeterminant.Click += (sender, e) => btnSetSize_Click(sender, e, "Determinant");

        btnCalculateDeterminant = new Button
        {
            Location = new Point(10, 500),
            Size = new Size(100, 30),
            Text = "Calculate",
            BackColor = Color.LightBlue
        };
        btnCalculateDeterminant.Click += (sender, e) => btnCalculate_Click(sender, e, "Determinant");
        ApplyButtonEffects(btnCalculateDeterminant);

        btnClearDeterminant = new Button
        {
            Location = new Point(120, 500),
            Size = new Size(100, 30),
            Text = "Clear",
            BackColor = Color.LightCoral
        };
        btnClearDeterminant.Click += (sender, e) => ClearGrids("Determinant");
        ApplyButtonEffects(btnClearDeterminant);

        btnFillZeroDeterminant = new Button
        {
            Location = new Point(230, 500),
            Size = new Size(100, 30),
            Text = "Fill Zeros",
            BackColor = Color.LightGreen
        };
        btnFillZeroDeterminant.Click += (sender, e) => FillZeros("Determinant");
        ApplyButtonEffects(btnFillZeroDeterminant);

        tabDeterminant.Controls.Add(numRowsDeterminant);
        tabDeterminant.Controls.Add(btnSetSizeDeterminant);
        tabDeterminant.Controls.Add(btnCalculateDeterminant);
        tabDeterminant.Controls.Add(btnClearDeterminant);
        tabDeterminant.Controls.Add(btnFillZeroDeterminant);

        // Initialize controls for Transpose tab
        numRowsTranspose = new NumericUpDown { Location = new Point(10, 10), Minimum = 1, Maximum = 10, Value = 3 };
        numColsTranspose = new NumericUpDown { Location = new Point(130, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeTranspose = new Button
        {
            Location = new Point(260, 10),
            Size = new Size(100, 30),
            Text = "Set Size",
        };
        btnSetSizeTranspose.Click += (sender, e) => btnSetSize_Click(sender, e, "Transpose");

        btnCalculateTranspose = new Button
        {
            Location = new Point(10, 500),
            Size = new Size(100, 30),
            Text = "Calculate",
            BackColor = Color.LightBlue
        };
        btnCalculateTranspose.Click += (sender, e) => btnCalculate_Click(sender, e, "Transpose");
        ApplyButtonEffects(btnCalculateTranspose);

        btnClearTranspose = new Button
        {
            Location = new Point(120, 500),
            Size = new Size(100, 30),
            Text = "Clear",
            BackColor = Color.LightCoral
        };
        btnClearTranspose.Click += (sender, e) => ClearGrids("Transpose");
        ApplyButtonEffects(btnClearTranspose);

        btnFillZeroTranspose = new Button
        {
            Location = new Point(230, 500),
            Size = new Size(100, 30),
            Text = "Fill Zeros",
            BackColor = Color.LightGreen
        };
        btnFillZeroTranspose.Click += (sender, e) => FillZeros("Transpose");
        ApplyButtonEffects(btnFillZeroTranspose);

        tabTranspose.Controls.Add(numRowsTranspose);
        tabTranspose.Controls.Add(numColsTranspose);
        tabTranspose.Controls.Add(btnSetSizeTranspose);
        tabTranspose.Controls.Add(btnCalculateTranspose);
        tabTranspose.Controls.Add(btnClearTranspose);
        tabTranspose.Controls.Add(btnFillZeroTranspose);

        // Initialize controls for Inverse tab
        numRowsInverse = new NumericUpDown { Location = new Point(10, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeInverse = new Button
        {
            Location = new Point(150, 10),
            Size = new Size(100, 30),
            Text = "Set Size",
        };
        btnSetSizeInverse.Click += (sender, e) => btnSetSize_Click(sender, e, "Inverse");

        btnCalculateInverse = new Button
        {
            Location = new Point(10, 500),
            Size = new Size(100, 30),
            Text = "Calculate",
            BackColor = Color.LightBlue
        };
        btnCalculateInverse.Click += (sender, e) => btnCalculate_Click(sender, e, "Inverse");
        ApplyButtonEffects(btnCalculateInverse);

        btnClearInverse = new Button
        {
            Location = new Point(120, 500),
            Size = new Size(100, 30),
            Text = "Clear",
            BackColor = Color.LightCoral
        };
        btnClearInverse.Click += (sender, e) => ClearGrids("Inverse");
        ApplyButtonEffects(btnClearInverse);

        btnFillZeroInverse = new Button
        {
            Location = new Point(230, 500),
            Size = new Size(100, 30),
            Text = "Fill Zeros",
            BackColor = Color.LightGreen
        };
        btnFillZeroInverse.Click += (sender, e) => FillZeros("Inverse");
        ApplyButtonEffects(btnFillZeroInverse);

        tabInverse.Controls.Add(numRowsInverse);
        tabInverse.Controls.Add(btnSetSizeInverse);
        tabInverse.Controls.Add(btnCalculateInverse);
        tabInverse.Controls.Add(btnClearInverse);
        tabInverse.Controls.Add(btnFillZeroInverse);

        // Initialize controls for Eigenvalues tab
        numRowsEigen = new NumericUpDown { Location = new Point(10, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeEigenvalues = new Button
        {
            Location = new Point(150, 10),
            Size = new Size(100, 30),
            Text = "Set Size",
        };
        btnSetSizeEigenvalues.Click += (sender, e) => btnSetSize_Click(sender, e, "Eigenvalues");

        btnCalculateEigenvalues = new Button
        {
            Location = new Point(10, 500),
            Size = new Size(100, 30),
            Text = "Calculate",
            BackColor = Color.LightBlue
        };
        btnCalculateEigenvalues.Click += (sender, e) => btnCalculate_Click(sender, e, "Eigenvalues");
        ApplyButtonEffects(btnCalculateEigenvalues);

        btnClearEigenvalues = new Button
        {
            Location = new Point(120, 500),
            Size = new Size(100, 30),
            Text = "Clear",
            BackColor = Color.LightCoral
        };
        btnClearEigenvalues.Click += (sender, e) => ClearGrids("Eigenvalues");
        ApplyButtonEffects(btnClearEigenvalues);

        btnFillZeroEigenvalues = new Button
        {
            Location = new Point(230, 500),
            Size = new Size(100, 30),
            Text = "Fill Zeros",
            BackColor = Color.LightGreen
        };
        btnFillZeroEigenvalues.Click += (sender, e) => FillZeros("Eigenvalues");
        ApplyButtonEffects(btnFillZeroEigenvalues);

        txtResultEigenvalues = new TextBox
        {
            Location = new Point(10, 550),
            Size = new Size(500, 30),
            ReadOnly = true,
            ForeColor = Color.Black
        };

        txtCharacteristicPolynomial = new TextBox
        {
            Location = new Point(10, 590),
            Size = new Size(500, 30),
            ReadOnly = true,
            ForeColor = Color.Black
        };

        tabEigenvalues.Controls.Add(numRowsEigen);
        tabEigenvalues.Controls.Add(btnSetSizeEigenvalues);
        tabEigenvalues.Controls.Add(btnCalculateEigenvalues);
        tabEigenvalues.Controls.Add(btnClearEigenvalues);
        tabEigenvalues.Controls.Add(btnFillZeroEigenvalues);
        tabEigenvalues.Controls.Add(txtResultEigenvalues);
        tabEigenvalues.Controls.Add(txtCharacteristicPolynomial);

        // Initialize controls for RREF tab
        numRowsRREF = new NumericUpDown { Location = new Point(10, 10), Minimum = 1, Maximum = 10, Value = 3 };
        numColsRREF = new NumericUpDown { Location = new Point(130, 10), Minimum = 1, Maximum = 10, Value = 3 };
        btnSetSizeRREF = new Button
        {
            Location = new Point(260, 10),
            Size = new Size(100, 30),
            Text = "Set Size",
        };
        btnSetSizeRREF.Click += (sender, e) => btnSetSize_Click(sender, e, "RREF");

        btnCalculateRREF = new Button
        {
            Location = new Point(10, 500),
            Size = new Size(100, 30),
            Text = "Calculate",
            BackColor = Color.LightBlue
        };
        btnCalculateRREF.Click += (sender, e) => btnCalculate_Click(sender, e, "RREF");
        ApplyButtonEffects(btnCalculateRREF);

        btnClearRREF = new Button
        {
            Location = new Point(120, 500),
            Size = new Size(100, 30),
            Text = "Clear",
            BackColor = Color.LightCoral
        };
        btnClearRREF.Click += (sender, e) => ClearGrids("RREF");
        ApplyButtonEffects(btnClearRREF);

        btnFillZeroRREF = new Button
        {
            Location = new Point(230, 500),
            Size = new Size(100, 30),
            Text = "Fill Zeros",
            BackColor = Color.LightGreen
        };
        btnFillZeroRREF.Click += (sender, e) => FillZeros("RREF");
        ApplyButtonEffects(btnFillZeroRREF);

        tabRREF.Controls.Add(numRowsRREF);
        tabRREF.Controls.Add(numColsRREF);
        tabRREF.Controls.Add(btnSetSizeRREF);
        tabRREF.Controls.Add(btnCalculateRREF);
        tabRREF.Controls.Add(btnClearRREF);
        tabRREF.Controls.Add(btnFillZeroRREF);

        lblStepsInverse = new Label
        {
            Location = new Point(700, 20),
            Size = new Size(200, 20),
            Text = "Steps"
        };

        txtStepsInverse = new TextBox
        {
            Location = new Point(700, 45),
            Size = new Size(420, 430),
            ReadOnly = true,
            Multiline = true,
            WordWrap = false,
            ScrollBars = ScrollBars.Both,
            Font = new Font("Consolas", 10F)
        };

        tabInverse.Controls.Add(lblStepsInverse);
        tabInverse.Controls.Add(txtStepsInverse);

        lblStepsRREF = new Label
        {
            Location = new Point(700, 20),
            Size = new Size(200, 20),
            Text = "Steps"
        };

        txtStepsRREF = new TextBox
        {
            Location = new Point(700, 45),
            Size = new Size(420, 430),
            ReadOnly = true,
            Multiline = true,
            WordWrap = false,
            ScrollBars = ScrollBars.Both,
            Font = new Font("Consolas", 10F)
        };

        tabRREF.Controls.Add(lblStepsRREF);
        tabRREF.Controls.Add(txtStepsRREF);

        txtHistory = new TextBox
        {
            Location = new Point(20, 20),
            Size = new Size(1100, 520),
            ReadOnly = true,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };

        btnClearHistory = new Button
        {
            Location = new Point(20, 560),
            Size = new Size(110, 30),
            Text = "Clear History"
        };
        btnClearHistory.Click += (sender, e) => txtHistory.Clear();

        tabHistory.Controls.Add(txtHistory);
        tabHistory.Controls.Add(btnClearHistory);

        // Add TabControl to the form
        Controls.Add(tabControl);
    }
}
