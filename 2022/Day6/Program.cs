using System.Diagnostics;

namespace AdventOfCode;

class Program
{
    private static int GetIndexOfFirstStartOfPacket(string input, int n)
    {
        for (var i = n; i < input.Length; i++)
        {
            var range = input[(i - n)..i];
            var allUnique = range.Distinct().Count() == range.Length;

            if (allUnique)
            {
                return i;
            }
        }

        throw new InvalidOperationException("Out of bounds");
    }

    static void Main(string[] args)
    {
        /*
        Console.WriteLine(GetIndexOfFirstStartOfPacket(File.ReadAllText("example_input.txt"), 4) == 7);
        Console.WriteLine(GetIndexOfFirstStartOfPacket("bvwbjplbgvbhsrlpgdmjqwftvncz", 4) == 5);
        Console.WriteLine(GetIndexOfFirstStartOfPacket("nppdvjthqldpwncqszvftbrmjlhg", 4) == 6);
        Console.WriteLine(GetIndexOfFirstStartOfPacket("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4) == 10);
        Console.WriteLine(GetIndexOfFirstStartOfPacket("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4) == 11);
        Console.WriteLine(GetIndexOfFirstStartOfPacket("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14) == 19);
        */
        
        Console.WriteLine(GetIndexOfFirstStartOfPacket(File.ReadAllText("input.txt"), 4));
        Console.WriteLine(GetIndexOfFirstStartOfPacket(File.ReadAllText("input.txt"), 14));
    }
}
