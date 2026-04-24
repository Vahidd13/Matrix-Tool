using System;
using System.Text;

/// <summary>
/// Contains history formatting for calculations made during the current session.
/// </summary>
public partial class MatrixForm
{
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
}
