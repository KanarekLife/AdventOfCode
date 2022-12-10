using System.Collections;

namespace AdventOfCode;

public abstract record Command;

public record NoopCommand : Command;

public record AddCommand(int Value) : Command;

public class VideoSystem
{
    private readonly Command[] _commands;
    public VideoSystem(Command[] commands)
    {
        _commands = commands;
    }
    
    private IEnumerable<int> GetRegisterValues(IReadOnlyList<int> extractAtCycles)
    {
        var queue = new Queue<Command>(_commands);
        Command? currentCommand = null;
        var cyclesLeft = 0;
        var x = 1;
        var n = extractAtCycles[^1];
        var r = new List<int>();
        
        for (var i = 1; i <= n; i++)
        {
            switch (currentCommand)
            {
                case AddCommand addCommand when cyclesLeft == 0:
                    x += addCommand.Value;
                    break;
            }
            
            if ((currentCommand is null || cyclesLeft == 0) && queue.Count > 0)
            {
                currentCommand = queue.Dequeue();

                cyclesLeft = currentCommand switch
                {
                    AddCommand => 2,
                    NoopCommand => 1,
                    _ => throw new InvalidOperationException()
                };
            }

            if (cyclesLeft > 0)
            {
                cyclesLeft--;
            }

            if (extractAtCycles.Contains(i))
            {
                r.Add(x * i);
            }
        }

        return r;
    }


    public int GetSumOfSignalStrengths()
    {
        var extractAt = new List<int>();
        for (var i = 20; i <= 220; i += 40)
        {
            extractAt.Add(i);
        }
        return GetRegisterValues(extractAt.ToArray())
            .Sum();
    }

    public IEnumerable<string> RenderImage()
    {
        var queue = new Queue<Command>(_commands);
        Command? current = null;
        var cyclesLeft = 0;
        var x = 1;
        var pos = 0;
        var render = new List<string>();
        var output = new char[40];

        while (queue.Count > 0 || cyclesLeft > 0)
        {
            switch (current)
            {
                case AddCommand addCommand when cyclesLeft == 0:
                    x += addCommand.Value;
                    break;
            }

            if ((current is null || cyclesLeft == 0) && queue.Count > 0)
            {
                current = queue.Dequeue();

                cyclesLeft = current switch
                {
                    AddCommand => 2,
                    NoopCommand => 1,
                    _ => throw new InvalidOperationException()
                };
            }

            if (cyclesLeft > 0)
            {
                cyclesLeft--;
            }

            output[pos] = pos >= x - 1 && pos <= x + 1 ? '#' : '.';

            if ((pos + 1) % 40 == 0)
            {
                render.Add(new string(output));
            }
            
            pos = (pos + 1) % 40;
        }

        return render;
    }
}