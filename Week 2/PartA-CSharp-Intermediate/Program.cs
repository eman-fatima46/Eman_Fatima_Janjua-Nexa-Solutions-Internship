using Week2.PartA.Helpers;
using Week2.PartA.Notifications;
using Week2.PartA.AsyncDemo;
using Week2.PartA.RefactorDemo;


Console.WriteLine("=================================");
Console.WriteLine("EXERCISE 1 - GENERIC HELPER METHOD");
Console.WriteLine("=================================");

GenericHelper.DisplayValue<int>(100);
GenericHelper.DisplayValue<string>("Hello C#");
GenericHelper.DisplayValue<double>(85.5);


Console.WriteLine();
Console.WriteLine("=================================");
Console.WriteLine("EXERCISE 2 - INTERFACE");
Console.WriteLine("=================================");

INotificationService emailNotifier = new EmailNotifier();
emailNotifier.SendMessage("Welcome to the internship.");

INotificationService smsNotifier = new SmsNotifier();
smsNotifier.SendMessage("Your registration is complete.");


Console.WriteLine();
Console.WriteLine("=================================");
Console.WriteLine("EXERCISE 3 - ASYNC/AWAIT");
Console.WriteLine("=================================");

ResultService resultService = new ResultService();

string result = await resultService.GetResultAsync();

Console.WriteLine(result);


Console.WriteLine();
Console.WriteLine("=================================");
Console.WriteLine("EXERCISE 4 - CLEAN CODE REFACTOR");
Console.WriteLine("=================================");

Student student = new Student("Ali", 85);

GradeCalculator gradeCalculator = new GradeCalculator();

StudentReportService reportService =
    new StudentReportService(gradeCalculator);

reportService.PrintReport(student);