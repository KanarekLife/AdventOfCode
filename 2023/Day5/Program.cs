namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        Console.WriteLine($"Task 1: {Task1.Parse(input)}");
        Console.WriteLine($"Task 2: {Task2.Parse(input)}");
    }
}
