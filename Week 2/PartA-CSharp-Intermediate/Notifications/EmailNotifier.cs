namespace Week2.PartA.Notifications;

public class EmailNotifier : INotificationService
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}