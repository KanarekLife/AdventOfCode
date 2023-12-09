namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        
        Console.WriteLine($"Task 1: {SolveTask1(input)}");
        Console.WriteLine($"Task 2: {SolveTask2(input)}");
    }

    static ulong GetSpeed(uint time) => time;
    static ulong GetDistance(uint holdingTime, uint totalTime) => GetSpeed(holdingTime) * (totalTime - holdingTime);
    
    static uint SolveTask2(ReadOnlySpan<char> input)
    {
        var enumerator = input.EnumerateLines();

        Span<char> buffer = stackalloc char[20];
        var current = 0;
        
        enumerator.MoveNext();
        for (var i = 0; i < enumerator.Current.Length; i++)
        {
            if (char.IsDigit(enumerator.Current[i]))
            {
                buffer[current++] = enumerator.Current[i];
            }
        }
        var allowedTime = uint.Parse(buffer);
        
        enumerator.MoveNext();
        buffer.Clear();
        current = 0;
        for (var i = 0; i < enumerator.Current.Length; i++)
        {
            if (char.IsDigit(enumerator.Current[i]))
            {
                buffer[current++] = enumerator.Current[i];
            }
        }
        var recordDistance = ulong.Parse(buffer);

        uint counter = 0;
        for (uint holdingTime = 0; holdingTime <= allowedTime; holdingTime++)
        {
            var hasWon = GetDistance(holdingTime, allowedTime) > recordDistance;
                
            if (!hasWon && counter > 0)
            {
                break;
            }
        
            if (hasWon)
            {
                counter++;
            }
        }

        return counter;
    }

    static uint SolveTask1(ReadOnlySpan<char> input)
    {
        const int numberOfRaces = 4;
        Span<Range> ranges = stackalloc Range[numberOfRaces+1];
        Span<uint> timesOfRaces = stackalloc uint[numberOfRaces];
        Span<uint> recordDistancesOfRaces = stackalloc uint[numberOfRaces];

        var enumerator = input.EnumerateLines();
        
        enumerator.MoveNext();
        enumerator.Current.Split(ranges, ' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        for (var i = 1; i < numberOfRaces+1; i++)
        {
            if (ranges[i].End.Value == ranges[i].Start.Value)
            {
                timesOfRaces = timesOfRaces.Slice(0, i - 1);
                break;
            }
            
            timesOfRaces[i - 1] = uint.Parse(enumerator.Current[ranges[i]]);
        }
        
        enumerator.MoveNext();
        enumerator.Current.Split(ranges, ' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        for (var i = 1; i < numberOfRaces+1; i++)
        {
            if (ranges[i].End.Value == ranges[i].Start.Value)
            {
                recordDistancesOfRaces = recordDistancesOfRaces.Slice(0, i - 1);
                break;
            }
            
            recordDistancesOfRaces[i - 1] = uint.Parse(enumerator.Current[ranges[i]]);
        }

        uint marginOfError = 1;
        for (var i = 0; i < timesOfRaces.Length; i++)
        {
            var allowedTime = timesOfRaces[i];
            var recordDistance = recordDistancesOfRaces[i];

            uint counter = 0;
            for (uint holdingTime = 0; holdingTime <= allowedTime; holdingTime++)
            {
                var hasWon = GetDistance(holdingTime, allowedTime) > recordDistance;
                
                if (!hasWon && counter > 0)
                {
                    break;
                }

                if (hasWon)
                {
                    counter++;
                }
            }

            marginOfError *= counter;
        }

        return marginOfError;
    }
}
