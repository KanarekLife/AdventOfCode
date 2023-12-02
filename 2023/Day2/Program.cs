namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var sum = File.ReadLines("input.txt")
            .Select(line => IsGamePossible(line))
            .Where(gameId => gameId > -1)
            .Sum();
        
        Console.WriteLine($"Task 1: {sum}");
        
        var sumOfPowers = File.ReadLines("input.txt")
            .Select(line => GetMinNumberOfCubesRequiredForGame(line))
            .Select(requiredNumberOfCubes => requiredNumberOfCubes.Red * requiredNumberOfCubes.Blue * requiredNumberOfCubes.Green)
            .Sum();
        
        Console.WriteLine($"Task 2: {sumOfPowers}");
    }

    public static int IsGamePossible(ReadOnlySpan<char> data)
    {
        const int maxNumberOfRedCubes = 12;
        const int maxNumberOfGreenCubes = 13;
        const int maxNumberOfBlueCubs = 14;

        var separatorIndex = data.IndexOf(':');
        var gameId = int.Parse(data[5..separatorIndex]);
        
        var start = 0;
        int? value = null;
        var gameDetails = data.Slice(separatorIndex + 2);
        for (var i = 0; i < gameDetails.Length; i++)
        {
            var c = gameDetails[i];

            if (char.IsWhiteSpace(c) && start == i)
            {
                start++;
                continue;
            }

            if (char.IsWhiteSpace(c) && i > start)
            {
                value = int.Parse(gameDetails[start..i]);
                start = i + 1;
                continue;
            }

            if (c == 'r' && value.HasValue)
            {
                if (value > maxNumberOfRedCubes)
                {
                    return -1;
                }

                start += 4;
                i = start - 1;
                continue;
            }

            if (c == 'b' && value.HasValue)
            {
                if (value > maxNumberOfBlueCubs)
                {
                    return -1;
                }

                start += 5;
                i = start - 1;
                continue;
            }

            if (c == 'g' && value.HasValue)
            {
                if (value > maxNumberOfGreenCubes)
                {
                    return -1;
                }

                start += 6;
                i = start - 1;
                continue;
            }
        }

        return gameId;
    }
    
    public static (int Red, int Green, int Blue) GetMinNumberOfCubesRequiredForGame(ReadOnlySpan<char> data)
    {
        (int Red, int Green, int Blue) requiredCubes = (0, 0, 0);

        var gameDetails = data.Slice(data.IndexOf(':') + 2);
        var start = 0;
        int? value = null;
        for (var i = 0; i < gameDetails.Length; i++)
        {
            var c = gameDetails[i];

            if (char.IsWhiteSpace(c) && i == start)
            {
                start++;
                continue;
            }

            if (char.IsWhiteSpace(c) && i > start)
            {
                value = int.Parse(gameDetails[start..i]);
                start = i + 1;
                continue;
            }

            if (char.IsAsciiLetter(c) && value.HasValue && c == 'r')
            {
                requiredCubes.Red = Math.Max(requiredCubes.Red, value.Value);
                start += 4;
                i = start - 1;
                continue;
            }

            if (char.IsAsciiLetter(c) && value.HasValue && c == 'b')
            {
                requiredCubes.Blue = Math.Max(requiredCubes.Blue, value.Value);
                start += 5;
                i = start - 1;
                continue;
            }

            if (char.IsAsciiLetter(c) && value.HasValue && c == 'g')
            {
                requiredCubes.Green = Math.Max(requiredCubes.Green, value.Value);
                start += 6;
                i = start - 1;
                continue;
            }
        }

        return requiredCubes;
    }
}
