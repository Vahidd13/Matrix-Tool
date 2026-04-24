# Developer Guide

## Overview

Matrix Tool is a single-project C# Windows Forms application for performing common matrix operations through a graphical user interface. The application is designed mainly for students who want to calculate matrices and inspect step-by-step solutions for selected operations.

The project is intentionally divided into three main parts:

- **UI layer**: Windows Forms controls, tab handling, dynamic matrix grids, button behavior, and history display.
- **Model layer**: the `Matrix` data type and matrix algorithms.
- **Service/helper layer**: conversion between UI text boxes and `Matrix` objects.

The project does not use a database or persistent storage. Matrix inputs, results, and history exist only during the current application session.

## Project structure

The repository contains one Visual Studio solution and one C# project:

```text
matrix.sln
matrix.csproj
Program.cs
MatrixForm.Designer.cs

UI/
  MatrixForm.cs
  MatrixForm.Events.cs
  MatrixForm.GridSetup.cs
  MatrixForm.Calculations.cs
  MatrixForm.GridCommands.cs
  MatrixForm.History.cs
  MatrixForm.Theme.cs
  TextBoxGridFactory.cs

Model/
  Matrix.Core.cs
  Matrix.Arithmetic.cs
  Matrix.RowReduction.cs
  Matrix.Formatting.cs
  Matrix.Eigenvalues.cs
  MatrixStepsResult.cs

Services/
  MatrixOperations.cs

README.md
spec/README.md
docs/Developer.md
```

The application targets `net7.0-windows` and uses Windows Forms, so the graphical application runs on Windows.

## High-level architecture

The application is built around one main form, `MatrixForm`. The form contains a tab control where each tab represents one matrix operation.

The general data flow is:

```text
User enters values into TextBox grid
        ↓
MatrixOperations.ParseMatrix
        ↓
Matrix object
        ↓
Matrix algorithm in Model/
        ↓
Matrix result or scalar/text result
        ↓
MatrixOperations.DisplayResult or text output
        ↓
Result grid, label, step box, or history list
```

This design keeps most mathematical logic independent from Windows Forms controls.

## Main components

### `Program.cs`

`Program.cs` is the application entry point. It enables visual styles, configures text rendering compatibility, and starts `MatrixForm`.

### `MatrixForm.Designer.cs`

This file contains Windows Forms designer-generated control declarations and layout code. It defines the main tabs, buttons, labels, numeric controls, result fields, history box, and step text boxes.

This file should normally be edited through the Visual Studio Windows Forms Designer. Manual edits are possible but should be done carefully because the designer may rewrite parts of the file.

### `UI/MatrixForm.cs`

This file contains the main `MatrixForm` partial class declaration, operation-name constants, dimension fields, styling fields, the constructor, and startup grid initialization.

It keeps the form startup logic separate from the larger event-handling and grid-building logic.

### `UI/MatrixForm.Events.cs`

This file contains event handlers for:

- changing the selected tab,
- setting dimensions for normal operations,
- setting matrix A dimensions for multiplication,
- setting matrix B dimensions for multiplication.

Multiplication receives special handling because the number of columns in matrix A must match the number of rows in matrix B.

### `UI/MatrixForm.GridSetup.cs`

This file creates and assigns the dynamic matrix input/result grids.

It contains one setup method per operation:

- `InitializeAdditionGrid`
- `InitializeMultiplicationGrid`
- `InitializeDeterminantGrid`
- `InitializeTransposeGrid`
- `InitializeInverseGrid`
- `InitializeEigenvaluesGrid`
- `InitializeRrefGrid`

The public-facing dispatcher for these setup methods is `InitializeMatrixInputs`.

This decomposition prevents one very large method from handling all operations at once.

### `UI/TextBoxGridFactory.cs`

This helper creates reusable rectangular `TextBox[,]` grids. It centralizes cell size, spacing, grid creation, grid removal, and grid-width calculation.

This keeps the layout code shorter inside `MatrixForm.GridSetup.cs`.

### `UI/MatrixForm.Calculations.cs`

This file contains the calculation dispatcher and operation-specific calculation methods.

The main dispatcher is `btnCalculate_Click`. It chooses the correct calculation method based on the selected operation.

The operation-specific methods are:

- `CalculateAddition`
- `CalculateMultiplication`
- `CalculateDeterminant`
- `CalculateTranspose`
- `CalculateInverse`
- `CalculateRref`
- `CalculateEigenvalues`

Each method is responsible for parsing input, calling the model layer, displaying the result, and adding a history entry.

### `UI/MatrixForm.GridCommands.cs`

This file contains commands that modify grids without calculating results:

- `ClearGrids`
- `FillZeros`

Keeping these commands separate from calculations makes the UI logic easier to navigate.

### `UI/MatrixForm.History.cs`

This file formats calculation history entries and appends them to the history text box.

History is not saved to disk. It is cleared when the application closes.

### `UI/MatrixForm.Theme.cs`

This file contains visual styling:

- form and tab fonts,
- button colors,
- hover colors,
- primary button detection,
- recursive font application,
- helper methods for finding controls.

The hover effect uses opaque colors so button text remains visible.

### `Services/MatrixOperations.cs`

`MatrixOperations` connects WinForms text boxes to the model layer. It contains methods for:

- parsing `TextBox[,]` grids into `Matrix` objects,
- displaying `Matrix` values in result grids,
- clearing text box grids,
- filling empty cells with zeros.

This class is deliberately small and UI-focused.

### `Model/Matrix.Core.cs`

This file contains the core matrix state and basic object behavior:

- private `double[,]` storage,
- `Rows` and `Columns` properties,
- indexer access,
- constructor validation,
- clone/copy helper,
- rounding,
- array conversion.

### `Model/Matrix.Arithmetic.cs`

This file contains basic matrix operations:

- addition,
- multiplication,
- determinant,
- transpose,
- inverse using determinant and adjoint.

### `Model/Matrix.RowReduction.cs`

This file contains algorithms based on row operations:

- RREF,
- RREF with steps,
- inverse with steps using the augmented matrix `[A | I]`,
- row swapping,
- row division,
- row subtraction.

### `Model/Matrix.Formatting.cs`

This file contains formatting logic used by result display, history, and step boxes:

- decimal-to-fraction formatting,
- aligned matrix text formatting,
- augmented matrix formatting,
- column width calculation.

The step displays use aligned text, so this file is important for readability.

### `Model/Matrix.Eigenvalues.cs`

This file contains:

- eigenvalue calculation using MathNet Numerics,
- complex eigenvalue formatting,
- characteristic polynomial construction using MathNet Symbolics,
- symbolic determinant expansion for the polynomial.

Complex values are displayed in forms such as:

```text
a + bi
a - bi
bi
a
```

### `Model/MatrixStepsResult.cs`

This is a small result container for algorithms that return both a final matrix and textual steps.

It is used by inverse and RREF step-by-step operations.

## User interface design

The user interface is organized by operation tabs. This avoids showing all controls at once and makes each operation easier to use.

Each operation tab contains only the controls needed for that operation. For example, determinant and eigenvalues need one input matrix, while addition and multiplication need two input matrices.

Matrix inputs are created dynamically as `TextBox[,]` grids. This allows the application to support different matrix sizes without manually designing every possible grid in the Windows Forms Designer.

Result output depends on the operation:

- matrix operations display a matrix result grid,
- determinant displays a single numeric value,
- eigenvalues display a list of values and a characteristic polynomial,
- inverse and RREF also display step-by-step row operations.

The inverse and RREF step boxes use a monospace font and scrollbars because matrix alignment is important for readability.

## Matrix size handling

The user can change matrix dimensions through numeric controls.

When a size changes, the application removes the old text box grid and creates a new one. The grid always matches the selected dimensions.

For multiplication:

```text
A = m × n
B = n × p
Result = m × p
```

The application ensures that `columns(A) == rows(B)`.

## Calculation flow

A normal calculation follows this sequence:

1. The user selects a tab.
2. The user enters matrix values.
3. The user presses **Calculate**.
4. `btnCalculate_Click` dispatches to the correct operation method.
5. The operation method uses `MatrixOperations.ParseMatrix`.
6. The `Matrix` model performs the mathematical calculation.
7. The result is displayed in a grid or text box.
8. A history entry is added.

## Algorithms

### Addition

Matrix addition requires both matrices to have the same dimensions. Each result cell is computed by adding matching cells.

```text
C[i, j] = A[i, j] + B[i, j]
```

### Multiplication

Matrix multiplication requires the number of columns in the first matrix to equal the number of rows in the second matrix.

```text
A = m × n
B = n × p
C = m × p
```

Each result cell is calculated with a dot product.

### Determinant

The determinant is calculated recursively using expansion by minors. This implementation is simple and readable, but it is not optimized for very large matrices.

### Transpose

The transpose operation swaps rows and columns.

```text
T[j, i] = A[i, j]
```

### Inverse

The basic inverse method checks that the matrix is square and non-singular. It then uses determinant and adjoint logic.

The step-by-step inverse method uses Gaussian elimination on an augmented matrix:

```text
[A | I]
```

When the left side becomes the identity matrix, the right side becomes the inverse:

```text
[I | A^-1]
```

### RREF

The RREF algorithm processes columns from left to right:

1. Find a pivot.
2. Swap rows if needed.
3. Normalize the pivot row.
4. Eliminate all other entries in the pivot column.
5. Continue to the next pivot column.

The step-by-step method records each important row operation.

### Eigenvalues

Eigenvalues are computed with MathNet Numerics. The result can contain real or complex values.

The characteristic polynomial is generated with MathNet Symbolics by building a symbolic matrix with `λ` on the diagonal and calculating its determinant.

## Error handling

The application handles invalid mathematical operations, including:

- adding matrices with different dimensions,
- multiplying incompatible matrices,
- calculating determinant for a non-square matrix,
- calculating inverse for a non-square matrix,
- calculating inverse for a singular matrix,
- calculating eigenvalues for a non-square matrix.

The UI catches expected exceptions and displays readable error messages with message boxes.

## Dependencies

The project uses these NuGet packages:

```text
MathNet.Numerics
MathNet.Symbolics
System.Numerics.Vectors
```

Dependencies are restored from `matrix.csproj`.

## Build notes

The project targets:

```text
net7.0-windows
```

The project file contains:

```xml
<EnableWindowsTargeting>true</EnableWindowsTargeting>
```

This allows restore/build steps in some non-Windows CI environments, but running the Windows Forms GUI still requires Windows.

On macOS, run and test the application inside a Windows virtual machine such as UTM or Parallels.

## Code style and conventions

The project uses standard C# naming conventions:

- classes use `PascalCase`,
- methods use `PascalCase`,
- local variables use `camelCase`,
- private fields use descriptive names.

Recommended placement of future code:

- UI behavior belongs in the `UI/MatrixForm.*.cs` partial file that matches the behavior.
- Mathematical algorithms belong in `Model/Matrix.*.cs`.
- TextBox-to-matrix conversion belongs in `Services/MatrixOperations.cs`.
- Reusable grid creation belongs in `UI/TextBoxGridFactory.cs`.

## How to add a new matrix operation

To add a new operation:

1. Add a new tab and controls in `MatrixForm.Designer.cs`.
2. Add fields for the operation's input and result grids if needed.
3. Add the operation name constant in `UI/MatrixForm.cs`.
4. Add grid setup code in `UI/MatrixForm.GridSetup.cs`.
5. Add a calculation method in `UI/MatrixForm.Calculations.cs`.
6. Implement the mathematical operation in the appropriate `Model/Matrix.*.cs` file.
7. Add clear/fill-zero support in `UI/MatrixForm.GridCommands.cs` if needed.
8. Update `README.md` and this developer guide.

## Testing approach

Recommended manual tests include:

### Addition

- add two matrices with the same dimensions,
- try matrices with different dimensions.

### Multiplication

- multiply compatible matrices,
- try incompatible dimensions,
- verify the result grid size.

### Determinant

- test 1×1, 2×2, and 3×3 matrices with known determinants,
- try a non-square matrix.

### Inverse

- test an invertible matrix,
- verify that multiplying by the inverse gives the identity matrix,
- test a singular matrix.

### RREF

- test matrices with simple row reductions,
- test matrices requiring row swaps,
- test rectangular matrices.

### Eigenvalues

- test a matrix with real eigenvalues,
- test a matrix with complex eigenvalues,
- test a non-square matrix.

### UI behavior

- open every tab and confirm grids appear,
- resize matrices and confirm grids update,
- confirm multiplication initializes both input matrices and result grid,
- check that buttons remain readable on hover,
- check that inverse and RREF steps are aligned and readable.

## Known limitations

- The determinant and symbolic polynomial implementations use recursive expansion, so they are not suitable for very large matrices.
- The Windows Forms GUI does not run natively on macOS or Linux.
- Calculation history is not saved after the application closes.
- The project is intended as an educational matrix calculator, not as a high-performance numerical computing tool.

## Future improvements

Possible improvements include:

- saving and loading matrices from files,
- exporting calculation history,
- adding operations such as rank, LU decomposition, or QR decomposition,
- adding automated unit tests.
