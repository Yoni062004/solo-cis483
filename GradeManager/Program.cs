// GradeManager - CIS C# crash course assignment
using GradeManager;

// Using await at the top level makes the compiler generate "static async Task Main" for us

// Ignore case, so "alice" and "Alice" count as the same student
var students = new Dictionary<string, Student>(StringComparer.OrdinalIgnoreCase);
const string SaveFile = "students.txt";

await LoadStudentsAsync();

var running = true;
while (running)
{
    Console.WriteLine();
    Console.WriteLine("1) Add student");
    Console.WriteLine("2) Add grade");
    Console.WriteLine("3) Report");
    Console.WriteLine("4) Save & exit");
    Console.Write("Choose: ");

    var input = Console.ReadLine();

    // null means the input stream was closed (Ctrl+Z), so stop instead of looping forever
    if (input is null)
    {
        break;
    }

    // TryParse returns false instead of throwing, so letters or an empty line can't crash the menu
    if (!int.TryParse(input, out var choice))
    {
        Console.WriteLine("Please type a number from 1 to 4.");
        continue;
    }

    switch (choice)
    {
        case 1:
            AddStudent();
            break;
        case 2:
            AddGradeToStudent();
            break;
        case 3:
            ShowReport();
            break;
        case 4:
            running = false; // saving happens after the loop
            break;
        default:
            Console.WriteLine("Please type a number from 1 to 4.");
            break;
    }
}

// Placed after the loop so that both option 4 and a closed input (Ctrl+Z) save the data
await SaveStudentsAsync();
Console.WriteLine("Goodbye!");

void AddStudent()
{
    Console.Write("Student name: ");
    var name = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    // Commas separate the name from the grades in the save file, so a comma in a name would corrupt it
    if (name.Contains(','))
    {
        Console.WriteLine("Name cannot contain a comma.");
        return;
    }

    // students[name] = ... would silently replace an existing student, so check first
    if (students.ContainsKey(name))
    {
        Console.WriteLine($"{name} already exists.");
        return;
    }

    students.Add(name, new Student(name));
    Console.WriteLine($"Added {name}.");
}

void AddGradeToStudent()
{
    Console.Write("Student name: ");
    var name = Console.ReadLine()?.Trim() ?? "";

    // One lookup that both checks the student exists and gets them
    if (!students.TryGetValue(name, out var student))
    {
        Console.WriteLine($"No student named {name}.");
        return;
    }

    Console.Write("Grade (0-100): ");
    if (!int.TryParse(Console.ReadLine(), out var grade))
    {
        Console.WriteLine("That isn't a whole number. Grade not added.");
        return;
    }

    // Student owns the 0-100 rule; here we only turn its exception into a friendly message
    try
    {
        student.AddGrade(grade);
        Console.WriteLine($"Added {grade} to {student.Name}. Average is now {student.Average:F1}.");
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine($"{grade} is out of range. Grades must be between 0 and 100.");
    }
}

void ShowReport()
{
    // A student with no grades has Average 0, which would drag the class average down, so leave them out
    var graded = students.Values.Where(s => s.Grades.Count > 0).ToList();

    // Average() and First() throw on an empty list, so stop early
    if (graded.Count == 0)
    {
        Console.WriteLine("No grades yet. Nothing to report.");
        return;
    }

    // Turn each student into a fixed row, sorted highest average first
    var rows = graded
        .OrderByDescending(s => s.Average)
        .Select(s => new ReportRow(s.Name, s.Average, s.Letter))
        .ToList();

    var classAverage = rows.Average(r => r.Average);
    var topStudent = rows.First(); // already sorted highest first
    // Rows only keep the average, so counting individual 90+ grades still needs the students
    var totalAGrades = graded.Sum(s => s.Grades.Count(g => g >= 90));

    Console.WriteLine();
    Console.WriteLine(ReportRow.Header);
    Console.WriteLine(ReportRow.Divider);

    foreach (var row in rows)
    {
        Console.WriteLine(row.ToTableLine());
    }

    Console.WriteLine(ReportRow.Divider);
    Console.WriteLine($"Class average:  {classAverage:F1}");
    Console.WriteLine($"Top student:    {topStudent.Name} ({topStudent.Average:F1})");
    Console.WriteLine($"A grades (90+): {totalAGrades}");

    var skippedCount = students.Count - graded.Count;
    if (skippedCount > 0)
    {
        Console.WriteLine($"({skippedCount} student(s) with no grades were skipped.)");
    }
}

async Task LoadStudentsAsync()
{
    // The first run has no file yet, which is normal
    if (!File.Exists(SaveFile))
    {
        return;
    }

    string[] lines;
    try
    {
        lines = await File.ReadAllLinesAsync(SaveFile);
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Could not read {SaveFile}: {ex.Message}");
        return;
    }
    catch (UnauthorizedAccessException ex)
    {
        Console.WriteLine($"No permission to read {SaveFile}: {ex.Message}");
        return;
    }

    foreach (var line in lines)
    {
        var parts = line.Split(',');
        var name = parts[0].Trim();

        // Skip blank lines and repeated names in case the file was edited by hand
        if (name.Length == 0 || students.ContainsKey(name))
        {
            continue;
        }

        var student = new Student(name);

        // parts[0] is the name, so the grades start at index 1
        for (var i = 1; i < parts.Length; i++)
        {
            // Skip bad values one at a time, so one damaged grade doesn't lose the whole student
            if (!int.TryParse(parts[i], out var grade))
            {
                continue;
            }

            try
            {
                student.AddGrade(grade);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Skipped invalid grade {grade} for {name}.");
            }
        }

        students.Add(name, student);
    }

    Console.WriteLine($"Loaded {students.Count} student(s) from {SaveFile}.");
}

async Task SaveStudentsAsync()
{
    var lines = new List<string>();

    foreach (var student in students.Values)
    {
        // Leave off the trailing comma when a student has no grades yet
        lines.Add(student.Grades.Count == 0
            ? student.Name
            : $"{student.Name},{string.Join(",", student.Grades)}");
    }

    try
    {
        await File.WriteAllLinesAsync(SaveFile, lines);
        Console.WriteLine($"Saved {lines.Count} student(s) to {SaveFile}.");
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Could not save {SaveFile}: {ex.Message}");
    }
    catch (UnauthorizedAccessException ex)
    {
        Console.WriteLine($"No permission to write {SaveFile}: {ex.Message}");
    }
}
