using System.Text;

namespace AdventOfCode;

public class CratesContext
{
    private readonly Stack<char>[] _stacks;

    private CratesContext(Stack<char>[] stacks)
    {
        _stacks = stacks;
    }

    public static CratesContext FromInput(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var numberOfStacks = lines[^1].TrimEnd()[^1] - '0';
        var stacks = new Stack<char>[numberOfStacks];
        var stackValues = lines
            .Take(lines.Length - 1)
            .ToArray();
        var i = 1;
        var j = 1;
        
        while (i <= numberOfStacks)
        {
            stacks[i-1] = new Stack<char>(numberOfStacks);
            for (var k = stackValues.Length-1; k >= 0; k--)
            {
                var value = stackValues[k][j];
                
                if (value == ' ')
                {
                    break;
                }
                
                stacks[i-1].Push(value);
            }
            i++;
            j += 4;
        }

        return new CratesContext(stacks);
    }

    public void Move(string instruction)
    {
        var arr = instruction
            .Split(' ')
            .ToArray();
        Move(int.Parse(arr[1]), int.Parse(arr[3]), int.Parse(arr[5]));
    }

    private void Move(int numberOfCrates, int from, int to)
    {
        for (var i = 0; i < numberOfCrates; i++)
        {
            _stacks[to-1].Push(_stacks[from-1].Pop());
        }
    }

    public string GetMessageFromTopCrates()
    {
        var builder = new StringBuilder();
        
        foreach (var stack in _stacks)
        {
            builder.Append(stack.Peek());
        }

        return builder.ToString();
    }

    public void MoveInOrder(string instruction)
    {
        var arr = instruction
            .Split(' ')
            .ToArray();
        MoveInOrder(int.Parse(arr[1]), int.Parse(arr[3]), int.Parse(arr[5]));
    }

    private void MoveInOrder(int numberOfCrates, int from, int to)
    {
        var temp = new Stack<char>(numberOfCrates);
        
        for (var i = 0; i < numberOfCrates; i++)
        {
            temp.Push(_stacks[from-1].Pop());
        }
        
        for (var i = 0; i < numberOfCrates; i++)
        {
            _stacks[to-1].Push(temp.Pop());
        }
    }
}