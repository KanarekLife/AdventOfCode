namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var fileSystem = new FileSystem();
        var lines = System.IO.File.ReadAllLines("input.txt");
        fileSystem.HandleCommands(lines);
        Console.WriteLine($"Puzzle 1: {fileSystem.GetSumOfSizesOfSmallestDirectories()}");
        Console.WriteLine($"Puzzle 2: {fileSystem.FindDirectoryToRemove()}");
    }
}
