public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public double Marks { get; set; }

    public Student(int id, string name, string department, double marks)
    {
        Id = id;
        Name = name;
        Department = department;
        Marks = marks;
    }
}