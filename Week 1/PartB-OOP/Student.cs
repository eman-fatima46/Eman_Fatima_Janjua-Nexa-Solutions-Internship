public class Student : Person, IPrintable
{
    public int StudentId { get; set; }
    public string Department { get; set; }

    public Student(
        string name,
        int age,
        int studentId,
        string department)
        : base(name, age)
    {
        StudentId = studentId;
        Department = department;
    }

    public void Study()
    {
        Console.WriteLine($"{Name} is studying {Department}.");
    }

    public override void Introduce()
    {
        Console.WriteLine(
            $"I am {Name}, a student studying {Department}."
        );
    }
    public void Print()
    {
        Console.WriteLine(
            $"Student ID: {StudentId} | Name: {Name} | Department: {Department}"
        );
    }
}