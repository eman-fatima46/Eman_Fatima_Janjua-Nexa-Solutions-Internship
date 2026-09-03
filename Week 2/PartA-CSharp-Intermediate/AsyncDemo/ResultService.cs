namespace Week2.PartA.AsyncDemo;

public class ResultService
{
    public async Task<string> GetResultAsync()
    {
        Console.WriteLine("Processing result...");

        await Task.Delay(2000);

        return "Result received successfully.";
    }
}