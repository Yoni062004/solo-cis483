namespace GradeManager;

public class Student
{
    // Private and readonly: only this class can change the list, and it can never be swapped out
    private readonly List<int> _grades = [];

    public Student(string name)
    {
        Name = name;
    }

    // No setter, so a student's name can't change once they're created
    public string Name { get; }

    // Read-only view so the report and the save code can read grades without changing them
    public IReadOnlyList<int> Grades => _grades;

    public void AddGrade(int grade)
    {
        // Check here so no other code can ever store a bad grade
        if (grade < 0 || grade > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(grade), grade, "Grade must be between 0 and 100.");
        }

        _grades.Add(grade);
    }

    // LINQ Average() throws on an empty sequence, so a student with no grades gets 0 instead
    public double Average => _grades.Count == 0 ? 0 : _grades.Average();

    public string Letter => Average switch
    {
        >= 90 => "A",
        >= 80 => "B",
        >= 70 => "C",
        >= 60 => "D",
        _ => "F"
    };
}
