namespace Week2.PartA.RefactorDemo;

public class StudentReportService
{
    private readonly GradeCalculator _gradeCalculator;

    public StudentReportService(GradeCalculator gradeCalculator)
    {
        _gradeCalculator = gradeCalculator;
    }

    public void PrintReport(Student student)
    {
        string grade = _gradeCalculator.GetGrade(student.Marks);

        Console.WriteLine($"Student: {student.Name}");
        Console.WriteLine($"Marks: {student.Marks}");
        Console.WriteLine($"Grade: {grade}");
    }
}