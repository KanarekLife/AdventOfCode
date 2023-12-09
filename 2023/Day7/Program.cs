namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var input = File.ReadAllText("input.txt");
        Console.WriteLine($"Task 1: {SolveTask1(input)}");
        Console.WriteLine($"Task 2: {SolveTask2(input)}");
    }

    public static uint SolveTask1(ReadOnlySpan<char> input)
    {
        var listOfHands = new List<Hand>();
        foreach (var line in input.EnumerateLines())
        {
            if (line.IsEmpty) {continue;}

            listOfHands.Add(Hand.Parse(line[..5], uint.Parse(line[6..])));
        }

        var hands = listOfHands.ToArray();
        
        Array.Sort(hands);

        uint totalWinnings = 0;
        for (uint rank = 0; rank < hands.Length; rank++)
        {
            totalWinnings += (rank + 1) * hands[rank].Bid;
        }

        return totalWinnings;
    }
    
    public static uint SolveTask2(ReadOnlySpan<char> input)
    {
        var listOfHands = new List<HandWithJoker>();
        foreach (var line in input.EnumerateLines())
        {
            if (line.IsEmpty) {continue;}

            listOfHands.Add(HandWithJoker.Parse(line[..5], uint.Parse(line[6..])));
        }

        var hands = listOfHands.ToArray();
        
        Array.Sort(hands);

        uint totalWinnings = 0;
        for (uint rank = 0; rank < hands.Length; rank++)
        {
            totalWinnings += (rank + 1) * hands[rank].Bid;
        }

        return totalWinnings;
    }
}
