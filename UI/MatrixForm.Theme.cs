using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Contains UI styling and button hover behavior for <see cref="MatrixForm" />.
/// </summary>
public partial class MatrixForm
{
    private void ApplyUiTheme()
    {
        Font = baseFont;
        BackColor = Color.White;
        tabControl.Font = baseFont;

        foreach (TabPage tab in tabControl.TabPages)
        {
            tab.BackColor = Color.White;
        }

        ApplyFontRecursively(Controls);
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

    /// <summary>
    /// Applies opaque hover effects to a button. Opaque colors keep button text readable on hover.
    /// </summary>
    private void ApplyButtonEffects(Button button)
    {
        button.UseVisualStyleBackColor = false;
        button.MouseEnter += (s, e) => SetButtonColors(button, GetButtonHoverColor(button));
        button.MouseLeave += (s, e) => SetButtonColors(button, GetButtonBaseColor(button));
    }
}
