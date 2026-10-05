namespace GradeManager;

// A record, not an IReportable interface: a report row is a fixed snapshot of data with no
// behaviour of its own, and Student is the only thing we report on, so an interface would
// add a layer without solving a problem. The record gives us a constructor, read-only
// properties, value equality and a readable ToString() in one line.
public record ReportRow(string Name, double Average, string Letter)
{
    // Header and row share the same column widths, so they live together and can't drift apart
    public static string Header => $"{"Name",-15}{"Avg",6}{"Letter",8}";

    // Sized from the header, so changing a column width can never leave the dashes too short or long
    public static string Divider => new string('-', Header.Length);

    public string ToTableLine() => $"{Name,-15}{Average,6:F1}{Letter,8}";
}
