namespace Week2.PartA.RefactorDemo;

public class GradeCalculator
{
    public string GetGrade(int marks)
    {
        if (marks >= 80)
            return "A";

        if (marks >= 70)
            return "B";

        if (marks >= 60)
            return "C";

        if (marks >= 50)
            return "D";

        return "F";
    }
}