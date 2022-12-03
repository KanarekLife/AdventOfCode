namespace AdventOfCode;

public class Program
{
    private static int GetPriority(char item)
    {
        return item switch
        {
            >= 'a' and <= 'z' => item - 'a' + 1,
            >= 'A' and <= 'Z' => item - 'A' + 27,
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, null)
        };
    }

    private static IEnumerable<char> GetPackingMistakes(string rucksack)
    {
        var compartment1 = rucksack[..(rucksack.Length / 2)].ToHashSet();
        var compartment2 = rucksack[(rucksack.Length / 2)..];
        compartment1.IntersectWith(compartment2);
        return compartment1;
    }

    private static int GetSumOfPrioritiesFromMistakes(IEnumerable<string> rucksacks)
    {
        return rucksacks
            .SelectMany(GetPackingMistakes)
            .Select(GetPriority)
            .Sum();
    }

    private static int GetSumOfPrioritiesFromBadges(IEnumerable<string> rucksacks)
    {
        return rucksacks
            .Chunk(3)
            .Select(chunk => 
                chunk
                    .SelectMany(s => s.Distinct())
                    .GroupBy(c => c)
                    .Select(g => new Tuple<char, int>(g.Key, g.Count()))
                    .Single(t => t.Item2 == 3).Item1
                )
            .Select(GetPriority)
            .Sum();
    }

    public static void Main()
    {
        var file = File.ReadAllLines("input.txt");
        Console.WriteLine(GetSumOfPrioritiesFromMistakes(file));
        Console.WriteLine(GetSumOfPrioritiesFromBadges(file));
    }
}