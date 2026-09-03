StudentManager manager = new StudentManager();

bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("================================");
    Console.WriteLine("   STUDENT MANAGEMENT SYSTEM");
    Console.WriteLine("================================");

    Console.WriteLine("1. Add Student");
    Console.WriteLine("2. View Students");
    Console.WriteLine("3. Update Student");
    Console.WriteLine("4. Delete Student");
    Console.WriteLine("5. Search Student");
    Console.WriteLine("6. Exit");

    Console.Write("Enter your choice: ");

    string choice = Console.ReadLine() ?? "";

    switch (choice)
    {
        case "1":
            manager.AddStudent();
            break;

        case "2":
            manager.ViewStudents();
            break;

        case "3":
            manager.UpdateStudent();
            break;

        case "4":
            manager.DeleteStudent();
            break;

        case "5":
            manager.SearchStudent();
            break;

        case "6":
            running = false;
            Console.WriteLine("Program closed.");
            break;

        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}