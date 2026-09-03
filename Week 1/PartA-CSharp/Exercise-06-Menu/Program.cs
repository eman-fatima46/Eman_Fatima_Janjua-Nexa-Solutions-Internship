bool running = true;

while (running)
{
    Console.WriteLine();
    Console.WriteLine("===== MENU =====");
    Console.WriteLine("1. Add");
    Console.WriteLine("2. View");
    Console.WriteLine("3. Search");
    Console.WriteLine("4. Delete");
    Console.WriteLine("5. Exit");
    Console.Write("Enter your choice: ");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("Add selected.");
            break;

        case "2":
            Console.WriteLine("View selected.");
            break;

        case "3":
            Console.WriteLine("Search selected.");
            break;

        case "4":
            Console.WriteLine("Delete selected.");
            break;

        case "5":
            running = false;
            Console.WriteLine("Exiting program...");
            break;

        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}