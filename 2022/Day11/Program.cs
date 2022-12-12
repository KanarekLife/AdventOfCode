namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var group = MonkeyGroup.Parse(File.ReadAllText("input.txt"));

        Console.WriteLine($"Puzzle 1: {group.GetMonkeyBusinessAfterNRounds(20)}");
        Console.WriteLine($"Puzzle 2: {group.GetMonkeyBusinessAfterNRoundsWithoutDivision(10000)}");
    }
}
