namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var sumOfIndices = File.ReadAllText("input.txt")
            .Trim()
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(pair => pair
                .Split(Environment.NewLine)
                .Select(Packet.Parse)
                .ToArray()
            )
            .Select((pair, i) => (i: i+1, pair))
            .Where(t => t.pair[0].CompareTo(t.pair[1]) == -1)
            .Sum(t => t.i);

        Console.WriteLine($"Puzzle 1: {sumOfIndices}");

        var decoderKey = File.ReadAllLines("input.txt")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Prepend("[[6]]")
            .Prepend("[[2]]")
            .Select(Packet.Parse)
            .Order()
            .Select((packet, i) => (packet, i: i + 1))
            .Where(t => t.packet.IsDividerKey())
            .Select(t => t.i)
            .Aggregate((a,b) => a * b);
        
        Console.WriteLine($"Puzzle 2: {decoderKey}");
    }
}
