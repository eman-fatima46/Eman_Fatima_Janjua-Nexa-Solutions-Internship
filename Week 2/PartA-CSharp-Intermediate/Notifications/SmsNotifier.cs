namespace Week2.PartA.Notifications;

public class SmsNotifier : INotificationService
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}