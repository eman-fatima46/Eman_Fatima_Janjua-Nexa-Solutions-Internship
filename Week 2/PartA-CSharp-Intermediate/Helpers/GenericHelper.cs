namespace Week2.PartA.Helpers;

public static class GenericHelper
{
    public static void DisplayValue<T>(T value)
    {
        Console.WriteLine($"Value: {value}");
    }
}