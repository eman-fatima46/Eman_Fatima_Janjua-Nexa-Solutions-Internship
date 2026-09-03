public class Teacher : Person
{
    public string Subject { get; set; }

    public Teacher(
        string name,
        int age,
        string subject)
        : base(name, age)
    {
        Subject = subject;
    }

    public void Teach()
    {
        Console.WriteLine($"{Name} is teaching {Subject}.");
    }

    public override void Introduce()
    {
        Console.WriteLine(
            $"I am {Name}, a teacher who teaches {Subject}."
        );
    }
}