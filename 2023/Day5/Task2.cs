namespace AdventOfCode;

public struct MyRange
{
    public MyRange(uint start, uint length)
    {
        Start = start;
        Length = length;
    }

    private MyRange(uint start, uint length, bool modified)
    {
        Start = start;
        Length = length;
        Modified = modified;
    }

    public uint Start { get; }
    public uint Length { get; }
    public uint End => Start + Length - 1;
    public bool Modified { get; private set; } = false;

    public void ClearModified()
    {
        Modified = false;
    }

    public MyRange[] Apply(Transformation transformation)
    {
        if (End < transformation.Start || transformation.End < Start)
        {
            return new[]
            {
                this
            };
        }
        
        if (transformation.Start <= Start && transformation.End >= End)
        {
            return new[]
            {
                new MyRange(transformation.Destination + (Start - transformation.Start), Length, true)
            };
        }

        if (transformation.Start <= Start && transformation.End < End)
        {
            return new[]
            {
                new MyRange(transformation.Destination + (Start - transformation.Start), transformation.End - Start + 1, true),
                new MyRange(transformation.End + 1, End - transformation.End)
            };
        }
        
        if (Start < transformation.Start && End <= transformation.End)
        {
            return new[]
            {
                new MyRange(Start, transformation.Start - Start),
                new MyRange(transformation.Destination, End - transformation.Start + 1, true)
            };
        }
        
        if (transformation.Start > Start && transformation.End < End)
        {
            return new[]
            {
                new MyRange(Start, transformation.Start - Start),
                new MyRange(transformation.Destination, transformation.End - transformation.Start + 1, true),
                new MyRange(transformation.End + 1, End - transformation.End)
            };
        }

        throw new ApplicationException();
    }
}

public struct Transformation(uint destination, uint start, uint length)
{
    public uint Start { get; } = start;
    public uint Length { get; } = length;
    public uint End => Start + Length - 1;
    public uint Destination { get; } = destination;
}

public static class Task2
{
    public static uint Parse(ReadOnlySpan<char> input)
    {
        var linesEnumerator = input.EnumerateLines();
        linesEnumerator.MoveNext();
        var seeds = ParseSeeds(linesEnumerator.Current);

        Span<Range> ranges = stackalloc Range[3];

        foreach (var line in linesEnumerator)
        {
            if (line.Length == 0)
            {
                continue;
            }

            if (!char.IsDigit(line[0]))
            {
                for (var i = 0; i < seeds.Length; i++)
                {
                    seeds[i].ClearModified();
                }
                continue;
            }

            line.Split(ranges, ' ');

            var transformation = new Transformation(
                uint.Parse(line[ranges[0]]),
                uint.Parse(line[ranges[1]]),
                uint.Parse(line[ranges[2]])
            );

            var list = new List<MyRange>();
            for (var i = 0; i < seeds.Length; i++)
            {
                if (seeds[i].Modified)
                {
                    list.Add(seeds[i]);
                    continue;
                }
                
                list.AddRange(seeds[i].Apply(transformation));
            }
            seeds = list.ToArray();
        }
        
        return seeds.Min(x => x.Start);
    }

    private static MyRange[] ParseSeeds(ReadOnlySpan<char> line)
    {
        var valuesPart = line.Slice(7);
        var ranges = new List<MyRange>();

        var start = 0;
        var i = 0;
        uint? first = null;
        while (i < valuesPart.Length)
        {
            if (valuesPart[i] == ' ' && first is null)
            {
                first = uint.Parse(valuesPart[start..i]);
                start = i + 1;
                i++;
                continue;
            }

            if (valuesPart[i] == ' ')
            {
                var second = uint.Parse(valuesPart[start..i]);
                ranges.Add(new MyRange(first!.Value, second));
                first = null;
                start = i + 1;
                i++;
                continue;
            }

            i++;
        }

        if (start != i && first is not null)
        {
            var second = uint.Parse(valuesPart[start..i]);
            ranges.Add(new MyRange(first.Value, second));
        }

        return ranges.ToArray();
    }
}