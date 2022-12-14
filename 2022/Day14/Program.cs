namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var simulation = SandSimulation.Parse(File.ReadAllLines("input.txt"));
        Console.WriteLine($"Puzzle 1: {simulation.GetUnitsOfSandBeforeInfiniteFall()}");
        Console.WriteLine($"Puzzle 2: {simulation.GetUnitsOfSandBeforeBlockingInput()}");
    }
}
