//Fizz Buzz

Console.WriteLine("What is the upper limit of our fizz buzz?");

int upperLimit = 0;
int.TryParse(Console.ReadLine(), out upperLimit);

for (int i = 1; i <= upperLimit ; i++)
{
    if (i % 3 == 0 && i % 5 == 0)
    {
        Console.WriteLine("FizzBuzz");
    }
    else if(i % 3 == 0)
    {
        Console.WriteLine("Fizz");
    }
    else if (i % 5 == 0)
    {
        Console.WriteLine("Buzz");
    }
    else
    {
        Console.WriteLine($"{i}");
    }
}