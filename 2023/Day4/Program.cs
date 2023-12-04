namespace AdventOfCode;

class Program
{
    private static readonly char[] Delimiters = new[] { ':', '|' };
    
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");

        var task1Result = GetSumOfPointsInGame(input);
        
        Console.WriteLine($"Task 1: {task1Result}");

        var task2Result = GetNumberOfAllScratchCards(input);
        
        Console.WriteLine($"Task 2: {task2Result}");
    }

    private static int GetSumOfPointsInGame(ReadOnlySpan<char> input)
    {
        var result = 0;
        foreach (var line in input.EnumerateLines())
        {
            if (line.Length > 0)
            {
                result += GetNumberOfPointsInGame(line);
            }
        }
        return result;
    }

    private static int GetNumberOfPointsInGame(ReadOnlySpan<char> gameInput)
    {
        var result = GetNumberOfWinningNumbers(gameInput);
        return result > 0
            ? (int) Math.Pow(2, result - 1)
            : 0;
    }
    
    private static int GetNumberOfAllScratchCards(ReadOnlySpan<char> input)
    {
        var results = new Dictionary<int, int>();
        var i = 0;
        foreach (var line in input.EnumerateLines())
        {
            if (line.Length > 0)
            {
                results.Add(i++, GetNumberOfWinningNumbers(line));
            }
        }

        var numberOfGames = new Dictionary<int, int>(i + 1);
        foreach (var pair in results)
        {
            numberOfGames.Add(pair.Key, 1);
        }
        numberOfGames.Add(i, 0);
        
        for (var j = 0; j < i; j++)
        {
            foreach (var gameId in Enumerable.Range(j + 1, results[j]))
            {
                numberOfGames[gameId] += numberOfGames[j];
            }
        }

        return numberOfGames.Values.Sum();
    }
    
    private static int GetNumberOfWinningNumbers(ReadOnlySpan<char> gameInput)
    {
        Span<Range> ranges = stackalloc Range[3];
        gameInput.SplitAny(ranges, Delimiters);

        var winningNumbersPart = gameInput[ranges[1]].Slice(1);
        var winningNumbersCount = winningNumbersPart.Count(' ');
        Span<int> winningNumbers = stackalloc int[winningNumbersCount];
        Span<Range> winningNumbersRanges = stackalloc Range[winningNumbersCount];
        winningNumbersPart.Split(winningNumbersRanges, ' ');
        var emptyRanges = 0;
        for (var i = 0; i < winningNumbersCount; i++)
        {
            if (winningNumbersPart[winningNumbersRanges[i]].Length == 0)
            {
                emptyRanges++;
                continue;
            }
            
            winningNumbers[i-emptyRanges] = int.Parse(winningNumbersPart[winningNumbersRanges[i]]);
        }
        winningNumbers = winningNumbers.Slice(0, winningNumbers.Length - emptyRanges);

        var result = 0;

        var ownedNumbersPart = gameInput[ranges[2]].Slice(1);
        for (var i = 0; i < ownedNumbersPart.Length; i++)
        {
            if (i < ownedNumbersPart.Length - 1 && char.IsDigit(ownedNumbersPart[i]) && char.IsDigit(ownedNumbersPart[i+1]))
            {
                if (winningNumbers.Contains(int.Parse(ownedNumbersPart.Slice(i, 2))))
                {
                    result++;
                }
                i += 1;
                continue;
            }

            if (char.IsDigit(ownedNumbersPart[i]))
            {
                if (winningNumbers.Contains(ownedNumbersPart[i] - '0'))
                {
                    result++;
                }
            }
        }

        return result;
    }
}
