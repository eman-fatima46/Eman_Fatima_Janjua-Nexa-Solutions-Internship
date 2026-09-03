List<Student> students = new List<Student>();

students.Add(new Student(1001, "Ali", "Computer Science", 85));
students.Add(new Student(1002, "Sara", "Software Engineering", 92));
students.Add(new Student(1003, "Ahmed", "Computer Science", 75));
students.Add(new Student(1004, "Fatima", "Artificial Intelligence", 90));
students.Add(new Student(1005, "Usman", "Software Engineering", 68));


Console.WriteLine("=== ALL STUDENTS ===");

foreach (Student student in students)
{
    Console.WriteLine(
        $"ID: {student.Id}, Name: {student.Name}, Department: {student.Department}, Marks: {student.Marks}"
    );
}


Console.WriteLine("\n=== COMPUTER SCIENCE STUDENTS ===");

var computerScienceStudents = students
    .Where(student => student.Department == "Computer Science");

foreach (Student student in computerScienceStudents)
{
    Console.WriteLine(
        $"{student.Name} - {student.Department} - {student.Marks}"
    );
}


Console.WriteLine("\n=== STUDENT NAMES ===");

var studentNames = students
    .Select(student => student.Name);

foreach (string name in studentNames)
{
    Console.WriteLine(name);
}


Console.WriteLine("\n=== STUDENTS SORTED BY MARKS ===");

var sortedStudents = students
    .OrderBy(student => student.Marks);

foreach (Student student in sortedStudents)
{
    Console.WriteLine(
        $"{student.Name} - {student.Marks}"
    );
}


Console.Write("\nEnter student ID to search: ");

try
{
    int searchId = Convert.ToInt32(Console.ReadLine());

    Student? foundStudent = students
        .FirstOrDefault(student => student.Id == searchId);

    if (foundStudent != null)
    {
        Console.WriteLine(
            $"Student found: {foundStudent.Name}, {foundStudent.Department}, Marks: {foundStudent.Marks}"
        );
    }
    else
    {
        Console.WriteLine("Student not found.");
    }
}
catch (FormatException)
{
    Console.WriteLine("Invalid input. Please enter a valid number.");
}


bool hasAIStudents = students
    .Any(student => student.Department == "Artificial Intelligence");

Console.WriteLine(
    $"\nAre there any Artificial Intelligence students? {hasAIStudents}"
);


int studentCount = students.Count();

Console.WriteLine(
    $"Total number of students: {studentCount}"
);