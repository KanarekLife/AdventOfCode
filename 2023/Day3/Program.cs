using System.Buffers;

namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        Console.WriteLine($"Task 1: {GetSumOfUsedParts(input)}");
        Console.WriteLine($"Task 2: {GetSumOfGearRatios(input)}");
    }

    private static int GetSumOfUsedParts(ReadOnlySpan<char> input)
    {
        var lineWidth = input.IndexOf(Environment.NewLine) + 2;
        var parts = new HashSet<Range>();
        for (var i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]) || char.IsWhiteSpace(input[i]) || input[i] == '.')
            {
                continue;
            }

            var currentColumn = (i % lineWidth);
            var currentRow = i / lineWidth;
            
            foreach (var (dy, dx) in Options)
            {
                var index = GetIndex(dy + currentRow, dx + currentColumn, lineWidth);

                if (index < 0 || index > input.Length)
                {
                    continue;
                }

                var c = input[index];

                if (!char.IsDigit(c))
                {
                    continue;
                }
                var range = new Range(Math.Max(input[..index].LastIndexOfAnyExcept(Digits) + 1, 0), Math.Min(input[index..].IndexOfAnyExcept(Digits) + index, input.Length - 1));
                parts.Add(range);
            }
        }

        var result = 0;
        foreach (var range in parts)
        {
            result += int.Parse(input[range]);
        }
        return result;
    }

    private static int GetSumOfGearRatios(ReadOnlySpan<char> input)
    {
        var lineWidth = input.IndexOf(Environment.NewLine) + 2;
        var result = 0;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] != '*')
            {
                continue;
            }

            var currentColumn = (i % lineWidth);
            var currentRow = i / lineWidth;
            var localHashSet = new HashSet<Range>();
            
            foreach (var (dy, dx) in Options)
            {
                var index = GetIndex(dy + currentRow, dx + currentColumn, lineWidth);

                if (index < 0 || index > input.Length)
                {
                    continue;
                }

                var c = input[index];

                if (!char.IsDigit(c))
                {
                    continue;
                }
                
                var range = new Range(Math.Max(input[..index].LastIndexOfAnyExcept(Digits) + 1, 0), Math.Min(input[index..].IndexOfAnyExcept(Digits) + index, input.Length - 1));
                localHashSet.Add(range);
            }

            if (localHashSet.Count != 2) continue;
            
            var temp = 1;
            foreach (var range in localHashSet)
            {
                temp *= int.Parse(input[range]);
            }
            result += temp;
        }
        return result;
    }
    
    private static readonly (int, int)[] Options = {
        (-1, -1),
        (-1, 0),
        (-1, 1),
        (0, 1),
        (1, 1),
        (1, 0),
        (1, -1),
        (0, -1)
    };

    private static int GetIndex(int dy, int dx, int lineWidth) => (dy) * lineWidth + dx;
    
    private static readonly SearchValues<char> Digits = SearchValues.Create("0123456789");
}
