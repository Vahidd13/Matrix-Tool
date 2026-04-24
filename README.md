# Matrix Tool

Matrix Tool is a Windows Forms desktop application for performing common matrix calculations through a graphical interface. It is intended for students who want to quickly check matrix computations and see selected row-reduction steps.

## Features

- Matrix addition
- Matrix multiplication
- Determinant calculation
- Matrix transposition
- Matrix inverse calculation with Gaussian-elimination steps
- Eigenvalue calculation with characteristic polynomial display
- Complex eigenvalue display, for example `2 + 3i` or `-i`
- Reduced row echelon form (RREF) calculation with step-by-step row operations
- Calculation history during the current application session
- Buttons for clearing matrix grids and filling empty cells with zeros

## Requirements

- Windows operating system
- .NET SDK 7.0 or newer compatible SDK
- Visual Studio 2022 is recommended

The application uses Windows Forms, so the graphical application is designed to run on Windows. The project file includes Windows targeting support so package restore and compilation can also work in non-Windows CI environments, but the UI itself still requires Windows to run.

## How to build and run

### Visual Studio

1. Open `matrix.sln` in Visual Studio.
2. Wait until NuGet packages are restored.
3. Select the `matrix` project.
4. Press **Start** or **F5**.

### Visual Studio Code

This is a Windows Forms application, so it must be run on Windows. On macOS, run it inside a Windows virtual machine such as UTM or Parallels.

Open the project folder in VS Code and run:

```bash
dotnet restore
dotnet build
dotnet run
```

### Command line

From the repository root, run:

```bash
dotnet restore
dotnet build
```

To run the application on Windows:

```bash
dotnet run --project matrix.csproj
```

## How to use the application

The application window contains separate tabs for different matrix operations. Each tab contains input cells, operation buttons, and a result area.

### Matrix size

Most tabs contain controls for selecting the number of rows and columns. After changing a size value, press the related size button to recreate the matrix grid. For square-only operations, such as determinant, inverse, and eigenvalues, the application uses one size value for both rows and columns.

### Entering matrix values

Type numbers into the input cells. Empty cells are treated as zero by the parser, but the **Fill Zeros** button can be used to explicitly fill all empty cells with `0` before calculating.

Accepted input is decimal numeric input supported by `double.TryParse`, for example:

- `0`
- `1`
- `-3`
- `2.5`

### Addition

1. Open the **Addition** tab.
2. Choose equal dimensions for both matrices.
3. Enter values into both input matrices.
4. Press **Calculate**.
5. The result matrix appears in the result grid.

### Multiplication

1. Open the **Multiplication** tab.
2. Choose the size of matrix A and matrix B.
3. The number of columns in matrix A must equal the number of rows in matrix B.
4. Enter values into both matrices.
5. Press **Calculate**.
6. The result grid displays the product.

### Determinant

1. Open the **Determinant** tab.
2. Choose the square matrix size.
3. Enter matrix values.
4. Press **Calculate**.
5. The determinant appears in the result field.

### Transpose

1. Open the **Transpose** tab.
2. Choose the matrix size.
3. Enter matrix values.
4. Press **Calculate**.
5. The transposed matrix appears in the result grid.

### Inverse

1. Open the **Inverse** tab.
2. Choose the square matrix size.
3. Enter matrix values.
4. Press **Calculate**.
5. The inverse matrix appears in the result grid.
6. The row-operation steps appear in the steps text area.

If the matrix is singular, the application shows an error message instead of an inverse.

### Eigenvalues

1. Open the **Eigenvalues** tab.
2. Choose the square matrix size.
3. Enter matrix values.
4. Press **Calculate**.
5. The eigenvalues and characteristic polynomial are displayed.

The application supports real and complex eigenvalues.

### RREF

1. Open the **RREF** tab.
2. Choose the matrix size.
3. Enter matrix values.
4. Press **Calculate**.
5. The RREF result appears in the result grid.
6. The row-operation steps appear in the steps text area.

### History

The **History** tab shows calculations made during the current run of the application. The history is stored only in memory and is cleared when the application is closed.

## Project structure

```text
matrix.sln                              Visual Studio solution
matrix.csproj                           C# project file and NuGet dependencies
Program.cs                              Application entry point
MatrixForm.Designer.cs                  Windows Forms control declarations and layout

UI/MatrixForm.cs                        Main form fields and startup initialization
UI/MatrixForm.Events.cs                 Tab and size-control event handlers
UI/MatrixForm.GridSetup.cs              Dynamic input/result grid creation
UI/MatrixForm.Calculations.cs           Calculation dispatch and result display
UI/MatrixForm.GridCommands.cs           Clear and fill-zero commands
UI/MatrixForm.History.cs                Session history formatting
UI/MatrixForm.Theme.cs                  Button, font, and hover styling
UI/TextBoxGridFactory.cs                Reusable TextBox grid creation helper

Model/Matrix.Core.cs                    Matrix storage, indexing, copying, and rounding
Model/Matrix.Arithmetic.cs              Addition, multiplication, determinant, transpose, inverse
Model/Matrix.RowReduction.cs            RREF and inverse-with-steps row operations
Model/Matrix.Formatting.cs              Fraction and aligned matrix text formatting
Model/Matrix.Eigenvalues.cs             Eigenvalues and characteristic polynomial
Model/MatrixStepsResult.cs              Result container for step-by-step algorithms

Services/MatrixOperations.cs            Helpers for parsing and displaying matrix grids
docs/Developer.md                       Developer guide
spec/README.md                          Final project specification
```

## External libraries

- `MathNet.Numerics` is used for eigenvalue calculation.
- `MathNet.Symbolics` is used to build and display the characteristic polynomial.
- `System.Numerics.Vectors` is referenced as a numeric support package.

## Limitations

- The GUI is Windows-only because it uses Windows Forms.
- The determinant and symbolic characteristic polynomial use recursive determinant expansion, which is suitable for small matrices but not optimized for large matrices.
- The application does not save history to disk.
