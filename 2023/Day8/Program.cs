namespace AdventOfCode;

struct Node
{
    private Node(string id, string left, string right)
    {
        Id = id;
        Left = left;
        Right = right;
    }

    public string Id { get; }
    public string Left { get; }
    public string Right { get; }

    private static readonly char[] Delimiters = { ' ', ',', '(', ')', '=' };

    public static Node Parse(ReadOnlySpan<char> input)
    {
        Span<Range> ranges = stackalloc Range[3];
        input.SplitAny(ranges, Delimiters, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return new Node(input[ranges[0]].ToString(), input[ranges[1]].ToString(), input[ranges[2]][..3].ToString());
    }
}

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        Console.WriteLine($"Task 1: {SolveTask1(input)}");
        Console.WriteLine($"Task 2: {SolveTask2(input)}");
    }

    public static int SolveTask1(ReadOnlySpan<char> input)
    {
        var linesEnumerator = input.EnumerateLines();
        linesEnumerator.MoveNext();
        Span<char> instructions = stackalloc char[linesEnumerator.Current.Length];
        linesEnumerator.Current.CopyTo(instructions);
        linesEnumerator.MoveNext();

        var nodes = new Dictionary<string, Node>();
        
        foreach (var line in linesEnumerator)
        {
            if (line.IsEmpty) {continue;}
            
            var node = Node.Parse(line);
            nodes.Add(node.Id, node);
        }

        var currentNode = "AAA";
        var eip = 0;
        var counter = 0;
        while (currentNode != "ZZZ")
        {
            counter++;
            currentNode = instructions[eip] switch
            {
                'L' => nodes[currentNode].Left,
                'R' => nodes[currentNode].Right,
                _ => throw new NotImplementedException()
            };

            eip = (eip + 1) % instructions.Length;
        }

        return counter;
    }
    
    public static ulong SolveTask2(ReadOnlySpan<char> input)
    {
        var linesEnumerator = input.EnumerateLines();
        linesEnumerator.MoveNext();
        Span<char> instructions = stackalloc char[linesEnumerator.Current.Length];
        linesEnumerator.Current.CopyTo(instructions);
        linesEnumerator.MoveNext();

        var nodes = new Dictionary<string, Node>();
        var startingNodes = new List<string>();
        
        foreach (var line in linesEnumerator)
        {
            if (line.IsEmpty) {continue;}
            
            var node = Node.Parse(line);
            nodes.Add(node.Id, node);
            
            if (node.Id.EndsWith('A'))
            {
                startingNodes.Add(node.Id);
            }
        }

        var currentNodes = startingNodes.ToArray();
        Span<int?> smallestIndex = stackalloc int?[currentNodes.Length];
        var smallestIndexesFound = 0;
        var eip = 0;
        var counter = 0;
        while (smallestIndexesFound < currentNodes.Length)
        {
            counter++;
            for (var i = 0; i < currentNodes.Length; i++)
            {
                currentNodes[i] = instructions[eip] switch
                {
                    'L' => nodes[currentNodes[i]].Left,
                    'R' => nodes[currentNodes[i]].Right,
                    _ => throw new NotImplementedException()
                };

                if (currentNodes[i].EndsWith('Z') && !smallestIndex[i].HasValue)
                {
                    smallestIndex[i] = counter;
                    smallestIndexesFound++;
                }
            }
            
            eip = (eip + 1) % instructions.Length;
        }

        var hashSet = new HashSet<ulong>();
        foreach (var t in smallestIndex)
        {
            hashSet.UnionWith(GetPrimeFactors(t!.Value));
        }
        return hashSet.Aggregate((a, b) => a * b);;
    }

    private static HashSet<ulong> GetPrimeFactors(int n)
    {
        var result = new HashSet<ulong>();
        while (n % 2 == 0)
        {
            result.Add(2);
            n /= 2;
        }

        for (var i = 3; i < Math.Sqrt(n); i += 2)
        {
            while (n % i == 0)
            {
                result.Add((ulong)i);
                n /= i;
            }
        }

        result.Add((ulong)n);

        return result;
    }
}
