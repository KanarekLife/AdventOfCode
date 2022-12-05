namespace AdventOfCode;

class Program
{
    private static string BasicProblem(IReadOnlyList<string> input)
    {
        var context = CratesContext.FromInput(input[0]);

        foreach (var instruction in input[1]
                     .Split(Environment.NewLine)
                     .Where(s => !string.IsNullOrWhiteSpace(s))
                )
        {
            context.Move(instruction);
        }

        return context.GetMessageFromTopCrates();
    }

    private static string AdvancedProblem(IReadOnlyList<string> input)
    {
        var context = CratesContext.FromInput(input[0]);
        
        foreach (var instruction in input[1]
                     .Split(Environment.NewLine)
                     .Where(s => !string.IsNullOrWhiteSpace(s))
                )
        {
            context.MoveInOrder(instruction);
        }

        return context.GetMessageFromTopCrates();
    }
    
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine);

        Console.WriteLine($"Puzzle 1: {BasicProblem(input)}");
        Console.WriteLine($"Puzzle 2: {AdvancedProblem(input)}");
    }
}
