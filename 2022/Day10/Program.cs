namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var commands = File.ReadAllLines("input.txt")
            .Select(line =>
            {
                var arr = line.Split(' ');
                return arr[0] switch
                {
                    "noop" => (Command) new NoopCommand(),
                    "addx" => (Command) new AddCommand(int.Parse(arr[1])),
                    _ => throw new InvalidOperationException()
                };
            })
            .ToArray();
        var system = new VideoSystem(commands);

        Console.WriteLine($"Puzzle 1: {system.GetSumOfSignalStrengths()}");
        var image = system.RenderImage();
        Console.WriteLine("Puzzle 2:");
        foreach (var row in image)
        {
            Console.WriteLine(row.Replace('.', ' '));
        }
    }
}
