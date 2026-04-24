# Specification of the final project for relevant C# courses

## C# Courses selection

- [x] NPRG035 (Programming in C# language | Programování v jazyce C#)
- [ ] NPRG038 (Advanced C# Programming | Pokročilé programování v jazyce C#)
- [ ] NPRG057 (Advanced .NET Programming II | Pokročilé programování pro .NET II)
- [ ] NPRG064 (Programming user interfaces in .NET | Programování uživatelských rozhraní v .NET)

## Specification

### Matrix Tool with Step-by-Step Linear Algebra Operations

Matrix Tool will be a Windows Forms desktop application for performing common matrix operations through a graphical user interface. The application is intended mainly for students who are learning linear algebra and want a simple tool for checking matrix calculations without using a command-line interface.

#### Motivation

Matrix calculations are common in linear algebra, numerical methods, computer graphics, and other technical subjects. Manual calculations are useful for learning, but they are slow and error-prone. This application will help users quickly verify results and, for row-reduction based operations, also inspect the intermediate row operations.

#### Use case scenarios

A typical user will be a student who wants to:

- add two matrices of the same size,
- multiply two compatible matrices,
- calculate the determinant of a square matrix,
- transpose a matrix,
- calculate the inverse of a square matrix,
- calculate eigenvalues and view the characteristic polynomial,
- reduce a matrix to reduced row echelon form,
- check the row operations used during inverse and RREF calculation.

#### Main features

The application will contain separate tabs for the following operations:

1. **Addition**
   - The user selects matrix dimensions.
   - The user enters two matrices of the same size.
   - The application displays the sum in a result grid.

2. **Multiplication**
   - The user selects dimensions for matrix A and matrix B.
   - The application requires the number of columns of matrix A to match the number of rows of matrix B.
   - The product is shown in a result grid.

3. **Determinant**
   - The user enters a square matrix.
   - The application displays its determinant.

4. **Transpose**
   - The user enters a matrix.
   - The application displays the transposed matrix.

5. **Inverse**
   - The user enters a square matrix.
   - The application calculates the inverse when the matrix is invertible.
   - The application displays row-operation steps used during the calculation.
   - If the matrix is singular, an error message is shown.

6. **Eigenvalues**
   - The user enters a square matrix.
   - The application displays the eigenvalues.
   - Complex eigenvalues are supported and displayed in `a + bi` form.
   - The application also displays the characteristic polynomial.

7. **RREF**
   - The user enters a matrix.
   - The application displays the reduced row echelon form.
   - The application displays the row-operation steps.

8. **History**
   - The application displays a session history of calculations.
   - The history is stored only while the application is running.

The application will also provide helper buttons for clearing matrix inputs and filling empty input cells with zeros.

#### UI/UX

The application will use a Windows Forms graphical interface. The main window will use a tab control, with one tab per operation. Matrix values will be entered into dynamically generated text boxes arranged in grids. Results will be displayed either in read-only grid cells or in read-only text boxes, depending on the operation.

For inverse and RREF operations, a multiline text area will display the calculation steps. These steps will use a monospace font so the matrices remain aligned and readable.

#### Persistence

The application will not use persistent storage. Calculation history will be stored only in memory during the current session and will be cleared when the application is closed.

#### Libraries and technologies

The project will be implemented in C# using Windows Forms on .NET. The application will use:

- `System.Windows.Forms` for the graphical user interface,
- `MathNet.Numerics` for numeric eigenvalue computation,
- `MathNet.Symbolics` for symbolic characteristic polynomial formatting.

#### Testing

The application will be tested manually using known matrix examples. Testing will include:

- addition of matrices with known sums,
- multiplication with compatible and incompatible dimensions,
- determinant values for 1x1, 2x2, and 3x3 matrices,
- transpose of rectangular matrices,
- inverse of invertible matrices and error handling for singular matrices,
- RREF examples with known reduced forms,
- eigenvalue examples with real eigenvalues and complex eigenvalues,
- UI tests for clearing, zero filling, resizing matrix grids, and calculation history.
