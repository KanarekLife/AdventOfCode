namespace AdventOfCode;

class Program
{
    private static bool AreSectionsFullyOverlapping(Range elf1, Range elf2)
    {
        if (elf1.Start.Value >= elf2.Start.Value && elf1.End.Value <= elf2.End.Value)
        {
            return true;
        }
        
        if (elf2.Start.Value >= elf1.Start.Value && elf2.End.Value <= elf1.End.Value)
        {
            return true;
        }

        return false;
    }
    
    private static bool AreSectionsOverlapping(Range elf1, Range elf2)
    {
        if (elf2.Start.Value >= elf1.Start.Value && elf1.End.Value >= elf2.Start.Value)
        {
            return true;
        }

        if (elf1.Start.Value >= elf2.Start.Value && elf2.End.Value >= elf1.Start.Value)
        {
            return true;
        }

        return false;
    }

    private static (Range elf1, Range elf2) ParseInput(string line)
    {
        var ranges = line
            .Split(',')
            .Select(range =>
            {
                var values = range.Split('-');
                return new Range(new Index(int.Parse(values[0])), new Index(int.Parse(values[1])));
            })
            .ToArray();
        return (ranges[0], ranges[1]);
    }
    
    public static void Main()
    {
        var input = File.ReadAllLines("input.txt")
            .Select(ParseInput)
            .ToArray();
        
        var numberOfFullyOverlappingSections = input
            .Select(x => AreSectionsFullyOverlapping(x.elf1, x.elf2))
            .Count(x=>x);
        Console.WriteLine($"Puzzle 1: {numberOfFullyOverlappingSections}");

        var numberOfOverlappingSections = input
            .Select(x => AreSectionsOverlapping(x.elf1, x.elf2))
            .Count(x => x);
        Console.WriteLine($"Puzzle 2: {numberOfOverlappingSections}");
    }
}