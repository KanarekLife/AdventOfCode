using System.Diagnostics;

namespace AdventOfCode;

[DebuggerDisplay("[{Start}..{End}]")]
public struct Range
{
    public int Start;
    public int End;
    public bool Empty;

    public Range()
    {
        Empty = true;
    }
    
    public Range(int start, int end)
    {
        Start = start;
        End = end;
        Empty = end - start == 0;
    }

    public bool CanMerge(Range otherRange)
    {
        return End >= otherRange.Start
               || Start >= otherRange.End
               || otherRange.Start - End == 1
               || Start - otherRange.End == 1;
    }

    public Range Merge(Range otherRange)
    {
        return new Range(Math.Min(otherRange.Start, Start), Math.Max(otherRange.End, End));
    }

    public int GetLength() => End - Start;
}