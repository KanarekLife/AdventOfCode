namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var group = MonkeyGroup.Parse(File.ReadAllText("example_input.txt"));

        Console.WriteLine($"Puzzle 1: {group.GetMonkeyBusinessAfterNRounds(20)}");
    }
}
