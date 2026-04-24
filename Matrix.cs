using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public partial class MatrixForm : Form
{
    // Matrix dimensions for various operations
    private int matrixRowsAddition = 3; // Default size for addition
    private int matrixColsAddition = 3; // Default size for addition
    private int matrixRowsMatrix1 = 3; // Default size for matrix 1
    private int matrixColsMatrix1 = 3; // Default size for matrix 1
    private int matrixRowsMatrix2 = 3; // Default size for matrix 2
    private int matrixColsMatrix2 = 3; // Default size for matrix 2
    private int matrixRowsDeterminant = 3; // Default size for determinant
    private int matrixRowsTranspose = 3; // Default size for transpose
    private int matrixColsTranspose = 3; // Default size for transpose
    private int matrixRowsInverse = 3; // Default size for inverse
    private int matrixColsInverse = 3; // Default size for inverse

    private int matrixRowsEigen = 3; // Default size for eigenvalues
    private int matrixColsEigen = 3; // Default size for eigenvalues
    private int matrixRowsRREF = 3; // Default size for RREF
    private int matrixColsRREF = 3; // Default size for RREF
    private string currentOperation; // Current selected operation
    private readonly Font baseFont = new Font("Segoe UI", 9F);
    private readonly Color accentColor = Color.SteelBlue;
    private readonly Color neutralButtonColor = Color.Gainsboro;

    public MatrixForm()
    {
        InitializeComponent();
        ApplyUiTheme();
        // Initialize matrix inputs for various tabs
        InitializeMatrixInputs(matrixRowsAddition, matrixColsAddition, tabAddition, "Addition");
        InitializeMatrixInputs(matrixRowsMatrix1, matrixColsMatrix1, tabMultiplication, "Multiplication");
        InitializeMatrixInputs(matrixRowsDeterminant, matrixRowsDeterminant, tabDeterminant, "Determinant");
        InitializeMatrixInputs(matrixRowsTranspose, matrixColsTranspose, tabTranspose, "Transpose");
        InitializeMatrixInputs(matrixRowsInverse, matrixColsInverse, tabInverse, "Inverse");
        InitializeMatrixInputs(matrixRowsEigen, matrixColsEigen, tabEigenvalues, "Eigenvalues");
        InitializeMatrixInputs(matrixRowsRREF, matrixColsRREF, tabRREF, "RREF");

        // Handle tab selection change
        tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
    }

    private void ApplyUiTheme()
    {
        Font = baseFont;
        BackColor = Color.White;
        tabControl.Font = baseFont;

        foreach (TabPage tab in tabControl.TabPages)
        {
            tab.BackColor = Color.White;
        }

        ApplyFontRecursively(this.Controls);
        txtStepsInverse.Font = new Font("Consolas", 10F);
        txtStepsRREF.Font = new Font("Consolas", 10F);
        StyleButtons();
    }

    private void ApplyFontRecursively(Control.ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            control.Font = baseFont;
            if (control.HasChildren)
            {
                ApplyFontRecursively(control.Controls);
            }
        }
    }

    private void StyleButtons()
    {
        foreach (var button in GetAllControls<Button>(this))
        {
            SetButtonColors(button, GetButtonBaseColor(button));
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.Silver;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = GetButtonHoverColor(button);
            button.FlatAppearance.MouseDownBackColor = BlendColors(GetButtonBaseColor(button), Color.Black, 0.10);
            button.UseVisualStyleBackColor = false;
        }
    }

    private void SetButtonColors(Button button, Color backColor)
    {
        button.BackColor = backColor;
        button.ForeColor = IsPrimaryButton(button) ? Color.White : Color.Black;
    }

    private Color GetButtonBaseColor(Button button)
    {
        return IsPrimaryButton(button) ? accentColor : neutralButtonColor;
    }

    private Color GetButtonHoverColor(Button button)
    {
        return BlendColors(GetButtonBaseColor(button), Color.White, 0.18);
    }

    private bool IsPrimaryButton(Button button)
    {
        return button == btnCalculateAddition
            || button == btnCalculateMultiplication
            || button == btnCalculateDeterminant
            || button == btnCalculateTranspose
            || button == btnCalculateInverse
            || button == btnCalculateEigenvalues
            || button == btnCalculateRREF;
    }

    private static Color BlendColors(Color first, Color second, double amount)
    {
        amount = Math.Max(0, Math.Min(1, amount));
        int r = (int)Math.Round(first.R + (second.R - first.R) * amount);
        int g = (int)Math.Round(first.G + (second.G - first.G) * amount);
        int b = (int)Math.Round(first.B + (second.B - first.B) * amount);
        return Color.FromArgb(r, g, b);
    }

    private static IEnumerable<T> GetAllControls<T>(Control root) where T : Control
    {
        foreach (Control control in root.Controls)
        {
            if (control is T match)
            {
                yield return match;
            }

            foreach (var child in GetAllControls<T>(control))
            {
                yield return child;
            }
        }
    }

    // Event handler for tab selection change
    private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentOperation = tabControl.SelectedTab.Text;
    }

    // Event handler for setting matrix size based on the operation
    private void btnSetSize_Click(object sender, EventArgs e, string operation)
    {
        if (operation == "Addition")
        {
            matrixRowsAddition = (int)numRowsAddition.Value;
            matrixColsAddition = (int)numColsAddition.Value;
            InitializeMatrixInputs(matrixRowsAddition, matrixColsAddition, tabAddition, operation);
        }
        else if (operation == "Determinant")
        {
            matrixRowsDeterminant = (int)numRowsDeterminant.Value;
            InitializeMatrixInputs(matrixRowsDeterminant, matrixRowsDeterminant, tabDeterminant, operation);
        }
        else if (operation == "Transpose")
        {
            matrixRowsTranspose = (int)numRowsTranspose.Value;
            matrixColsTranspose = (int)numColsTranspose.Value;
            InitializeMatrixInputs(matrixRowsTranspose, matrixColsTranspose, tabTranspose, operation);
        }
        else if (operation == "Inverse")
        {
            matrixRowsInverse = (int)numRowsInverse.Value;
            matrixColsInverse = matrixRowsInverse; // Set columns equal to rows
            InitializeMatrixInputs(matrixRowsInverse, matrixColsInverse, tabInverse, operation);
        }
        else if (operation == "Eigenvalues")
        {
            matrixRowsEigen = (int)numRowsEigen.Value;
            matrixColsEigen = matrixRowsEigen; // Ensure the matrix is square by setting columns equal to rows
            InitializeMatrixInputs(matrixRowsEigen, matrixColsEigen, tabEigenvalues, operation);
        }
        else if (operation == "RREF")
        {
            matrixRowsRREF = (int)numRowsRREF.Value;
            matrixColsRREF = (int)numColsRREF.Value;
            InitializeMatrixInputs(matrixRowsRREF, matrixColsRREF, tabRREF, operation);
        }
    }

    // Event handler for setting size of the first matrix in multiplication tab
    private void btnSetSizeMatrix1_Click(object sender, EventArgs e)
    {
        matrixRowsMatrix1 = (int)numRowsMatrix1Multiplication.Value;
        matrixColsMatrix1 = (int)numColsMatrix1Multiplication.Value;

        // Matrix multiplication requires columns(A) == rows(B).
        matrixRowsMatrix2 = matrixColsMatrix1;
        numRowsMatrix2Multiplication.Value = matrixRowsMatrix2;

        InitializeMatrixInputs(matrixRowsMatrix1, matrixColsMatrix1, tabMultiplication, "Multiplication");
    }

    // Event handler for setting size of the second matrix in multiplication tab
    private void btnSetSizeMatrix2_Click(object sender, EventArgs e)
    {
        matrixRowsMatrix2 = (int)numRowsMatrix2Multiplication.Value;
        matrixColsMatrix2 = (int)numColsMatrix2Multiplication.Value;

        // Ensure rows of the second matrix match columns of the first matrix
        if (matrixRowsMatrix2 != matrixColsMatrix1)
        {
            MessageBox.Show("The number of rows in the second matrix must match the number of columns in the first matrix.");
            numRowsMatrix2Multiplication.Value = matrixColsMatrix1;
        }
        else
        {
            InitializeMatrixInputs(matrixRowsMatrix2, matrixColsMatrix2, tabMultiplication, "Multiplication");
        }
    }

    // Method to initialize matrix input fields
    private void InitializeMatrixInputs(int rows, int cols, TabPage tabPage, string operation)
    {
        // Remove existing matrix input controls based on operation
        if (operation == "Addition")
        {
            if (txtMatrix1Addition != null)
            {
                foreach (var textBox in txtMatrix1Addition)
                    tabPage.Controls.Remove(textBox);

                foreach (var textBox in txtMatrix2Addition)
                    tabPage.Controls.Remove(textBox);

                foreach (var textBox in txtResultAddition)
                    tabPage.Controls.Remove(textBox);
            }
        }
        else if (operation == "Multiplication" || operation == "Matrix1" || operation == "Matrix2")
        {
            if (txtMatrix1Multiplication != null)
            {
                foreach (var textBox in txtMatrix1Multiplication)
                    tabPage.Controls.Remove(textBox);
            }

            if (txtMatrix2Multiplication != null)
            {
                foreach (var textBox in txtMatrix2Multiplication)
                    tabPage.Controls.Remove(textBox);
            }

            if (txtResultMultiplication != null)
            {
                foreach (var textBox in txtResultMultiplication)
                    tabPage.Controls.Remove(textBox);
            }
        }
        else if (operation == "Determinant")
        {
            if (txtMatrixDeterminant != null)
            {
                foreach (var textBox in txtMatrixDeterminant)
                    tabPage.Controls.Remove(textBox);

                tabPage.Controls.Remove(txtResultDeterminant);
            }
        }
        else if (operation == "Transpose")
        {
            if (txtMatrixTranspose != null)
            {
                foreach (var textBox in txtMatrixTranspose)
                    tabPage.Controls.Remove(textBox);

                foreach (var textBox in txtResultTranspose)
                    tabPage.Controls.Remove(textBox);
            }
        }
        else if (operation == "Inverse")
        {
            if (txtMatrixInverse != null)
            {
                foreach (var textBox in txtMatrixInverse)
                    tabPage.Controls.Remove(textBox);

                foreach (var textBox in txtResultInverse)
                    tabPage.Controls.Remove(textBox);
            }
        }
        else if (operation == "Eigenvalues")
        {
            if (txtMatrixEigen != null)
            {
                foreach (var textBox in txtMatrixEigen)
                    tabPage.Controls.Remove(textBox);
            }
        }
        else if (operation == "RREF")
        {
            if (txtMatrixRREF != null)
            {
                foreach (var textBox in txtMatrixRREF)
                    tabPage.Controls.Remove(textBox);

                foreach (var textBox in txtResultRREF)
                    tabPage.Controls.Remove(textBox);
            }
        }

        // Initialize new matrix input controls based on operation
        if (operation == "Addition")
        {
            TextBox[,] txtMatrix1 = new TextBox[rows, cols];
            TextBox[,] txtMatrix2 = new TextBox[rows, cols];
            TextBox[,] txtResult = new TextBox[rows, cols];

            int startX1 = 20;
            int startY = 70;
            int spacing = 8;
            int textBoxSize = 32;
            int blockSpacing = 60;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtMatrix1[i, j] = new TextBox
                    {
                        Location = new Point(startX1 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix1[i, j]);
                }
            }

            int startX2 = startX1 + cols * (textBoxSize + spacing) + blockSpacing;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtMatrix2[i, j] = new TextBox
                    {
                        Location = new Point(startX2 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix2[i, j]);
                }
            }

            int startX3 = startX2 + cols * (textBoxSize + spacing) + blockSpacing;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtResult[i, j] = new TextBox
                    {
                        Location = new Point(startX3 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize),
                        ReadOnly = true
                    };
                    tabPage.Controls.Add(txtResult[i, j]);
                }
            }

            txtMatrix1Addition = txtMatrix1;
            txtMatrix2Addition = txtMatrix2;
            txtResultAddition = txtResult;
        }
        else if (operation == "Multiplication" || operation == "Matrix1" || operation == "Matrix2")
        {
            TextBox[,] txtMatrix1 = new TextBox[matrixRowsMatrix1, matrixColsMatrix1];
            TextBox[,] txtMatrix2 = new TextBox[matrixRowsMatrix2, matrixColsMatrix2];
            TextBox[,] txtResult = new TextBox[matrixRowsMatrix1, matrixColsMatrix2];

            int startX1 = 20;
            int startY = 70;
            int spacing = 8;
            int textBoxSize = 32;
            int blockSpacing = 60;

            for (int i = 0; i < matrixRowsMatrix1; i++)
            {
                for (int j = 0; j < matrixColsMatrix1; j++)
                {
                    txtMatrix1[i, j] = new TextBox
                    {
                        Location = new Point(startX1 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix1[i, j]);
                }
            }

            int startX2 = startX1 + matrixColsMatrix1 * (textBoxSize + spacing) + blockSpacing;

            for (int i = 0; i < matrixRowsMatrix2; i++)
            {
                for (int j = 0; j < matrixColsMatrix2; j++)
                {
                    txtMatrix2[i, j] = new TextBox
                    {
                        Location = new Point(startX2 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix2[i, j]);
                }
            }

            int startX3 = startX2 + matrixColsMatrix2 * (textBoxSize + spacing) + blockSpacing;

            for (int i = 0; i < matrixRowsMatrix1; i++)
            {
                for (int j = 0; j < matrixColsMatrix2; j++)
                {
                    txtResult[i, j] = new TextBox
                    {
                        Location = new Point(startX3 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize),
                        ReadOnly = true
                    };
                    tabPage.Controls.Add(txtResult[i, j]);
                }
            }

            txtMatrix1Multiplication = txtMatrix1;
            txtMatrix2Multiplication = txtMatrix2;
            txtResultMultiplication = txtResult;
        }
        else if (operation == "Determinant")
        {
            TextBox[,] txtMatrix = new TextBox[rows, cols];
            int startX = 20;
            int startY = 70;
            int spacing = 8;
            int textBoxSize = 32;
            int blockSpacing = 60;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtMatrix[i, j] = new TextBox
                    {
                        Location = new Point(startX + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix[i, j]);
                }
            }

            txtResultDeterminant = new TextBox
            {
                Location = new Point(startX + cols * (textBoxSize + spacing) + blockSpacing, startY),
                Size = new Size(textBoxSize * 3, textBoxSize),
                ReadOnly = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            tabPage.Controls.Add(txtResultDeterminant);

            txtMatrixDeterminant = txtMatrix;
        }
        else if (operation == "Transpose")
        {
            TextBox[,] txtMatrix = new TextBox[rows, cols];
            TextBox[,] txtResult = new TextBox[cols, rows];

            int startX1 = 20;
            int startY = 70;
            int spacing = 8;
            int textBoxSize = 32;
            int blockSpacing = 60;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtMatrix[i, j] = new TextBox
                    {
                        Location = new Point(startX1 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix[i, j]);
                }
            }

            int startX2 = startX1 + cols * (textBoxSize + spacing) + blockSpacing;

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    txtResult[i, j] = new TextBox
                    {
                        Location = new Point(startX2 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize),
                        ReadOnly = true
                    };
                    tabPage.Controls.Add(txtResult[i, j]);
                }
            }

            txtMatrixTranspose = txtMatrix;
            txtResultTranspose = txtResult;
        }
        else if (operation == "Inverse")
        {
            TextBox[,] txtMatrix = new TextBox[rows, cols];
            TextBox[,] txtResult = new TextBox[rows, cols];

            int startX1 = 20;
            int startY = 70;
            int spacing = 8;
            int textBoxSize = 32;
            int blockSpacing = 60;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtMatrix[i, j] = new TextBox
                    {
                        Location = new Point(startX1 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix[i, j]);
                }
            }

            int startX2 = startX1 + cols * (textBoxSize + spacing) + blockSpacing;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtResult[i, j] = new TextBox
                    {
                        Location = new Point(startX2 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize),
                        ReadOnly = true
                    };
                    tabPage.Controls.Add(txtResult[i, j]);
                }
            }

            txtMatrixInverse = txtMatrix;
            txtResultInverse = txtResult;
        }
        else if (operation == "Eigenvalues")
        {
            TextBox[,] txtMatrix = new TextBox[rows, cols];

            int startX1 = 20;
            int startY = 70;
            int spacing = 8;
            int textBoxSize = 32;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtMatrix[i, j] = new TextBox
                    {
                        Location = new Point(startX1 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix[i, j]);
                }
            }

            txtMatrixEigen = txtMatrix;
        }
        else if (operation == "RREF")
        {
            TextBox[,] txtMatrix = new TextBox[rows, cols];
            TextBox[,] txtResult = new TextBox[rows, cols];

            int startX1 = 20;
            int startY = 70;
            int spacing = 8;
            int textBoxSize = 32;
            int blockSpacing = 60;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtMatrix[i, j] = new TextBox
                    {
                        Location = new Point(startX1 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize)
                    };
                    tabPage.Controls.Add(txtMatrix[i, j]);
                }
            }

            int startX2 = startX1 + cols * (textBoxSize + spacing) + blockSpacing;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    txtResult[i, j] = new TextBox
                    {
                        Location = new Point(startX2 + j * (textBoxSize + spacing), startY + i * (textBoxSize + spacing)),
                        Size = new Size(textBoxSize, textBoxSize),
                        ReadOnly = true
                    };
                    tabPage.Controls.Add(txtResult[i, j]);
                }
            }

            txtMatrixRREF = txtMatrix;
            txtResultRREF = txtResult;
        }
    }

    // Event handler for calculating the result based on the selected operation
    private void btnCalculate_Click(object sender, EventArgs e, string operation)
    {
        Matrix matrix = null;

        if (operation == "Addition")
        {
            matrix = MatrixOperations.ParseMatrix(txtMatrix1Addition);
            Matrix matrix2 = MatrixOperations.ParseMatrix(txtMatrix2Addition);
            Matrix result = matrix + matrix2;
            MatrixOperations.DisplayResult(result, txtResultAddition);
            AppendHistoryEntry("Addition", $"{matrix.ToDisplayString()}\n+\n{matrix2.ToDisplayString()}\n=\n{result.ToDisplayString()}");
        }
        else if (operation == "Multiplication")
        {
            matrix = MatrixOperations.ParseMatrix(txtMatrix1Multiplication);
            Matrix matrix2 = MatrixOperations.ParseMatrix(txtMatrix2Multiplication);
            Matrix result = matrix * matrix2;
            MatrixOperations.DisplayResult(result, txtResultMultiplication);
            AppendHistoryEntry("Multiplication", $"{matrix.ToDisplayString()}\n×\n{matrix2.ToDisplayString()}\n=\n{result.ToDisplayString()}");
        }
        else if (operation == "Determinant")
        {
            matrix = MatrixOperations.ParseMatrix(txtMatrixDeterminant);
            double result = matrix.Determinant();
            txtResultDeterminant.Text = result.ToString();
            AppendHistoryEntry("Determinant", $"{matrix.ToDisplayString()}\n=\n{result}");
        }
        else if (operation == "Transpose")
        {
            matrix = MatrixOperations.ParseMatrix(txtMatrixTranspose);
            Matrix result = matrix.Transpose();
            MatrixOperations.DisplayResult(result, txtResultTranspose);
            AppendHistoryEntry("Transpose", $"{matrix.ToDisplayString()}\n=\n{result.ToDisplayString()}");
        }
        else if (operation == "Inverse")
        {
            matrix = MatrixOperations.ParseMatrix(txtMatrixInverse);
            try
            {
                var inverseResult = matrix.InverseWithSteps();
                Matrix result = inverseResult.Result;
                MatrixOperations.DisplayResult(result, txtResultInverse);
                txtStepsInverse.Text = string.Join(Environment.NewLine + Environment.NewLine, inverseResult.Steps);
                AppendHistoryEntry("Inverse", $"{matrix.ToDisplayString()}\n=\n{result.ToDisplayString()}");
            }
            catch (InvalidOperationException ex)
            {
                txtStepsInverse.Clear();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else if (operation == "RREF")
        {
            matrix = MatrixOperations.ParseMatrix(txtMatrixRREF);
            var rrefResult = Matrix.RrefWithSteps(matrix);
            Matrix result = rrefResult.Result;
            MatrixOperations.DisplayResult(result, txtResultRREF);
            txtStepsRREF.Text = string.Join(Environment.NewLine + Environment.NewLine, rrefResult.Steps);
            AppendHistoryEntry("RREF", $"{matrix.ToDisplayString()}\n=\n{result.ToDisplayString()}");
        }
        else if (operation == "Eigenvalues")
        {
            matrix = MatrixOperations.ParseMatrix(txtMatrixEigen);
            var result = matrix.Eigenvalues();
            txtResultEigenvalues.Text = string.Join(", ", result.Item1);
            txtCharacteristicPolynomial.Text = result.Item2;
            AppendHistoryEntry("Eigenvalues", $"{matrix.ToDisplayString()}\nEigenvalues: {string.Join(", ", result.Item1)}\n{result.Item2}");
        }
    }

    private void AppendHistoryEntry(string operation, string details)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        var builder = new StringBuilder();
        builder.Append('[').Append(timestamp).Append("] ").Append(operation).AppendLine();
        builder.Append(details);
        builder.AppendLine();
        builder.AppendLine(new string('-', 40));

        txtHistory.AppendText(builder.ToString() + Environment.NewLine);
    }

    // Method to clear grids for the specified operation
    private void ClearGrids(string operation)
    {
        if (operation == "Addition")
        {
            MatrixOperations.ClearGrid(txtMatrix1Addition);
            MatrixOperations.ClearGrid(txtMatrix2Addition);
            MatrixOperations.ClearGrid(txtResultAddition);
        }
        else if (operation == "Multiplication")
        {
            MatrixOperations.ClearGrid(txtMatrix1Multiplication);
            MatrixOperations.ClearGrid(txtMatrix2Multiplication);
            MatrixOperations.ClearGrid(txtResultMultiplication);
        }
        else if (operation == "Determinant")
        {
            MatrixOperations.ClearGrid(txtMatrixDeterminant);
            txtResultDeterminant.Text = string.Empty;
        }
        else if (operation == "Transpose")
        {
            MatrixOperations.ClearGrid(txtMatrixTranspose);
            MatrixOperations.ClearGrid(txtResultTranspose);
        }
        else if (operation == "Inverse")
        {
            MatrixOperations.ClearGrid(txtMatrixInverse);
            MatrixOperations.ClearGrid(txtResultInverse);
            txtStepsInverse.Clear();
        }
        else if (operation == "Eigenvalues")
        {
            MatrixOperations.ClearGrid(txtMatrixEigen);
            txtResultEigenvalues.Text = string.Empty;
            txtCharacteristicPolynomial.Text = string.Empty;
        }
        else if (operation == "RREF")
        {
            MatrixOperations.ClearGrid(txtMatrixRREF);
            MatrixOperations.ClearGrid(txtResultRREF);
            txtStepsRREF.Clear();
        }
    }

    // Method to fill empty cells with zeros for the specified operation
    private void FillZeros(string operation)
    {
        if (operation == "Addition")
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix1Addition);
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix2Addition);
        }
        else if (operation == "Multiplication")
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix1Multiplication);
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrix2Multiplication);
        }
        else if (operation == "Determinant")
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixDeterminant);
        }
        else if (operation == "Transpose")
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixTranspose);
        }
        else if (operation == "Inverse")
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixInverse);
        }
        else if (operation == "Eigenvalues")
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixEigen);
        }
        else if (operation == "RREF")
        {
            MatrixOperations.FillEmptyCellsWithZeros(txtMatrixRREF);
        }
    }

    // Apply simple opaque hover effects. Do not fade alpha; transparent button colors can hide the text.
    private void ApplyButtonEffects(Button button)
    {
        button.UseVisualStyleBackColor = false;
        button.MouseEnter += (s, e) => SetButtonColors(button, GetButtonHoverColor(button));
        button.MouseLeave += (s, e) => SetButtonColors(button, GetButtonBaseColor(button));
    }
}
