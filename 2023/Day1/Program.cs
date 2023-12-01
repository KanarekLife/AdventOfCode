namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var result1 = File.ReadLines("input.txt")
            .Select(line => ParseLine(line.AsSpan()))
            .Sum();
        
        Console.WriteLine($"Task 1: {result1}");

        var result2 = File.ReadLines("input.txt")
            .Select(line => AdvancedParseLine(line.AsSpan()))
            .Sum();
        
        Console.WriteLine($"Task 2: {result2}");
    }

    private static int ParseLine(ReadOnlySpan<char> data)
    {
        Span<int?> values = stackalloc int?[2];
        foreach (var c in data)
        {
            if (!char.IsDigit(c))
            {
                continue;
            }

            if (values[0] is null)
            {
                values[0] = c - '0';
                continue;
            }

            values[1] = c - '0';
        }

        if (values[1] is null)
        {
            values[1] = values[0];
        }

        if (!values[0].HasValue)
        {
            throw new Exception("No digit was found!");
        }
        
        return 10 * values[0]!.Value + values[1]!.Value;
    }

    private static readonly Dictionary<string, int> MapValues = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };
    private static int AdvancedParseLine(ReadOnlySpan<char> data)
    {
        Span<int?> values = stackalloc int?[2];
        
        for (var i = 0; i < data.Length; i++)
        {
            if (char.IsDigit(data[i]))
            {
                if (!values[0].HasValue)
                {
                    values[0] = data[i] - '0';
                }
                else
                {
                    values[1] = data[i] - '0';
                }
                continue;
            }
            
            foreach (var searchValue in MapValues.Keys)
            {
                if (i + searchValue.Length > data.Length)
                {
                    continue;
                }
                
                if (data.Slice(i, searchValue.Length).SequenceEqual(searchValue))
                {
                    if (!values[0].HasValue)
                    {
                        values[0] = MapValues[searchValue];
                    }
                    else
                    {
                        values[1] = MapValues[searchValue];
                    }
                    // 2 instead of 1 because it needs to support chaining multiple words when last letter is the same (ex. sevenine - 73, eightwo - 82)
                    i += searchValue.Length - 2;
                    break;
                }
            }
        }

        if (values[1] is null)
        {
            values[1] = values[0];
        }

        if (!values[0].HasValue)
        {
            throw new Exception("No digit was found!");
        }
        
        return 10 * values[0]!.Value + values[1]!.Value;
    }
}
