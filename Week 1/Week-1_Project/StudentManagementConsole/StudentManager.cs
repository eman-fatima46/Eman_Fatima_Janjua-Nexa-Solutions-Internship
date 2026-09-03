public class StudentManager
{
    private List<Student> students = new List<Student>();


    public void AddStudent()
    {
        try
        {
            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            bool exists = students.Any(student => student.Id == id);

            if (exists)
            {
                Console.WriteLine("Student with this ID already exists.");
                return;
            }

            Console.Write("Enter Student Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Enter Department: ");
            string department = Console.ReadLine() ?? "";

            Console.Write("Enter Marks: ");
            double marks = Convert.ToDouble(Console.ReadLine());

            Student student = new Student(
                id,
                name,
                department,
                marks
            );

            students.Add(student);

            Console.WriteLine("Student added successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid numeric input.");
        }
    }


    public void ViewStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No students available.");
            return;
        }

        Console.WriteLine("\n=== STUDENTS ===");

        var sortedStudents = students
            .OrderBy(student => student.Name);

        foreach (Student student in sortedStudents)
        {
            Console.WriteLine(
                $"ID: {student.Id} | Name: {student.Name} | Department: {student.Department} | Marks: {student.Marks}"
            );
        }
    }


    public void SearchStudent()
    {
        try
        {
            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Student? student = students
                .FirstOrDefault(student => student.Id == id);

            if (student != null)
            {
                Console.WriteLine("\nStudent Found:");

                Console.WriteLine(
                    $"ID: {student.Id} | Name: {student.Name} | Department: {student.Department} | Marks: {student.Marks}"
                );
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Please enter a valid Student ID.");
        }
    }


    public void UpdateStudent()
    {
        try
        {
            Console.Write("Enter Student ID to update: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Student? student = students
                .FirstOrDefault(student => student.Id == id);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.Write("Enter new name: ");
            student.Name = Console.ReadLine() ?? "";

            Console.Write("Enter new department: ");
            student.Department = Console.ReadLine() ?? "";

            Console.Write("Enter new marks: ");
            student.Marks = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Student updated successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input.");
        }
    }


    public void DeleteStudent()
    {
        try
        {
            Console.Write("Enter Student ID to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Student? student = students
                .FirstOrDefault(student => student.Id == id);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            students.Remove(student);

            Console.WriteLine("Student deleted successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Please enter a valid Student ID.");
        }
    }
}