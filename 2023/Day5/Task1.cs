namespace AdventOfCode;

public static class Task1
{
    public static uint Parse(ReadOnlySpan<char> input)
    {
        var linesEnumerator = input.EnumerateLines();
        linesEnumerator.MoveNext();
        var seeds = ParseSeeds(linesEnumerator.Current);
        //var stepsEnumerator = Steps.GetEnumerator();
        
        Span<Range> ranges = stackalloc Range[3];
        Span<uint> values = stackalloc uint[3];
        Span<bool> affected = stackalloc bool[seeds.Length];
        foreach (var line in linesEnumerator)
        {
            if (line.Length == 0)
            {
                continue;
            }

            if (!char.IsDigit(line[0]))
            {
                affected.Clear();
                //Console.WriteLine(line.ToString());
                continue;
            }

            line.Split(ranges, ' ');
            
            for (var i = 0; i < 3; i++)
            {
                values[i] = uint.Parse(line[ranges[i]]);
            }

            for (var i = 0; i < seeds.Length; i++)
            {
                if (affected[i])
                {
                    continue;
                }
                
                var seed = seeds[i];
                
                if (seed >= values[1] && seed < values[1] + values[2])
                {
                    seeds[i] += values[0] - values[1];
                    affected[i] = true;
                }
            }
            
            //Console.WriteLine(string.Join(' ', seeds));
        }

        return seeds.Min();
    }

    private static uint[] ParseSeeds(ReadOnlySpan<char> line)
    {
        var valuesPart = line.Slice(line.IndexOf(':') + 2);
        
        var start = 0;
        var i = 0;
        var list = new List<uint>();
        
        while (i < valuesPart.Length)
        {
            if (valuesPart[i] == ' ')
            {
                list.Add(uint.Parse(valuesPart[start..i]));
                start = i + 1;
            }

            i++;
        }

        if (i != start)
        {
            list.Add(uint.Parse(valuesPart[start..i]));
        }

        return list.ToArray();
    }
}