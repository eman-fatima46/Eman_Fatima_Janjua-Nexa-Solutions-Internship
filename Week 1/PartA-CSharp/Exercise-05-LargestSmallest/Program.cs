int[] numbers =
{
    25, 10, 45, 3, 78,
    12, 56, 9, 31, 67
};

int largest = numbers[0];
int smallest = numbers[0];

foreach (int number in numbers)
{
    if (number > largest)
    {
        largest = number;
    }

    if (number < smallest)
    {
        smallest = number;
    }
}

Console.WriteLine("Largest number: " + largest);
Console.WriteLine("Smallest number: " + smallest);