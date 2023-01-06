using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

partial class Program
{
    private const int MapSize = 4000000;
    
    static void Main(string[] args)
    {
        var sensors = File.ReadAllLines("input.txt")
            .Select(line => InputRegex().Match(line).Groups)
            .Select(groups => new Sensor(
                new Position(int.Parse(groups[1].ValueSpan), int.Parse(groups[2].ValueSpan)), 
                new Position(int.Parse(groups[3].ValueSpan), int.Parse(groups[4].ValueSpan))
                )
            )
            .ToArray();
        
        Console.WriteLine("=== Part 1 ===");
        Console.WriteLine($"Result: {FindCovered(2000000, sensors)}");

        Console.WriteLine("=== Part 2 ===");
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        Parallel.For(0, MapSize + 1, (y, state) =>
        {
            var pos = FindNotCovered(y, sensors);

            if (pos is not null)
            {
                stopwatch.Stop();
                state.Break();
                Console.WriteLine($"Found: ({pos.X}, {pos.Y})");
                var result = (long)pos.X * 4000000 + pos.Y;
                Console.WriteLine($"Result: {result}");
                Console.WriteLine($"Took: {stopwatch.ElapsedMilliseconds} ms");
            }
        });
    }

    private static int FindCovered(int y, IEnumerable<Sensor> sensors)
    {
        var ranges = sensors
            .Select(s => s.GetValuesAtY(y))
            .Where(r => !r.Empty)
            .OrderBy(r => r.Start)
            .ToArray();
        
        var queue = new Queue<Range>(ranges);
        var current = queue.Dequeue();

        var disconnectedRanges = new List<Range>();
            
        while (queue.Count > 0)
        {
            var range = queue.Dequeue();
            if (!current.CanMerge(range))
            {
                disconnectedRanges.Add(current);
                current = queue.Dequeue();
                continue;
            }
            current = current.Merge(range);
        }
        disconnectedRanges.Add(current);

        return disconnectedRanges.Sum(r => r.GetLength());
    }

    private static Position? FindNotCovered(int y, IEnumerable<Sensor> sensors)
    {
        var ranges = sensors
            .Select(s => s.GetValuesAtY(y))
            .Where(r => !r.Empty)
            .OrderBy(r => r.Start)
            .ToArray();
        var queue = new Queue<Range>(ranges);
        var current = queue.Dequeue();
            
        while (queue.Count > 0)
        {
            var range = queue.Dequeue();
            if (!current.CanMerge(range))
            {
                return new Position(current.End + 1, y);
            }
            current = current.Merge(range);
        }

        if (current is { Start: <= 0, End: >= MapSize })
        {
            return null;
        }

        if (current.Start > 0)
        {
            return new Position(0, y);
        }

        if (current.End < MapSize)
        {
            return new Position(MapSize, y);
        }

        throw new NotImplementedException();
    }

    [GeneratedRegex(".*x=(-?\\d*), y=(-?\\d*).*x=(-?\\d*), y=(-?\\d*)")]
    private static partial Regex InputRegex();
}
