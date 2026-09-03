Console.Write("Enter first number: ");
double number1 = Convert.ToDouble(Console.ReadLine());

Console.Write("Enter second number: ");
double number2 = Convert.ToDouble(Console.ReadLine());

Console.WriteLine("\n--- Results ---");

Console.WriteLine("Sum: " + (number1 + number2));
Console.WriteLine("Difference: " + (number1 - number2));
Console.WriteLine("Product: " + (number1 * number2));

if (number2 != 0)
{
    Console.WriteLine("Quotient: " + (number1 / number2));
}
else
{
    Console.WriteLine("Quotient: Cannot divide by zero.");
}