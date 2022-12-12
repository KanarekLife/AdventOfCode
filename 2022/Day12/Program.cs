namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        Position? startPos = null;
        Position? endPos = null;
        var grid = File.ReadAllLines("input.txt")
            .Select((line, y) => line
                .Select((c, x) =>
                {
                    switch (c)
                    {
                        case 'S':
                            startPos = new Position(x, y);
                            return 'a';
                        case 'E':
                            endPos = new Position(x, y);
                            return 'z';
                        default:
                            return c;
                    }
                })
                .Select(c => char.IsAsciiLetterLower(c) ? c - 'a' : c)
                .ToArray()
            ).ToArray();
        
        Console.WriteLine($"Puzzle 1: {FindShortestPathLength(startPos, endPos, grid)}");

        var startPositions = GetStartPositions(grid);

        Console.WriteLine($"Puzzle 2: {startPositions.Select(s => FindShortestPathLength(s, endPos, grid)).Min()}");
    }

    private static IEnumerable<Position> GetStartPositions(int[][] grid)
    {
        var startPositions = new List<Position>();
        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == 0)
                {
                    startPositions.Add(new Position(x, y));
                }
            }
        }

        return startPositions;
    }

    private static int FindShortestPathLength(Position? startPos, Position? endPos, int[][] grid)
    {
        if (startPos is null || endPos is null)
        {
            throw new Exception("StartPos or EndPos not found!");
        }

        var mem = new int[grid.Length][];
        for (var i = 0; i < grid.Length; i++)
        {
            mem[i] = Enumerable.Repeat(-1, grid[0].Length).ToArray();
        }

        var posToCheck = new Queue<PositionWithHistory>(new[] { new PositionWithHistory(startPos.X, startPos.Y, 0) });

        while (posToCheck.Count > 0)
        {
            var pos = posToCheck.Dequeue();

            if (pos.X == endPos.X && pos.Y == endPos.Y)
            {
                return pos.NumberOfSteps;
            }

            if (mem[pos.Y][pos.X] != -1)
            {
                continue;
            }

            mem[pos.Y][pos.X] = pos.NumberOfSteps;

            if (pos.X != grid[0].Length - 1 && grid[pos.Y][pos.X + 1] - grid[pos.Y][pos.X] <= 1)
            {
                posToCheck.Enqueue(new PositionWithHistory(pos.X + 1, pos.Y, pos.NumberOfSteps + 1));
            }

            if (pos.X != 0 && grid[pos.Y][pos.X - 1] - grid[pos.Y][pos.X] <= 1)
            {
                posToCheck.Enqueue(new PositionWithHistory(pos.X - 1, pos.Y, pos.NumberOfSteps + 1));
            }

            if (pos.Y != grid.Length - 1 && grid[pos.Y + 1][pos.X] - grid[pos.Y][pos.X] <= 1)
            {
                posToCheck.Enqueue(new PositionWithHistory(pos.X, pos.Y + 1, pos.NumberOfSteps + 1));
            }

            if (pos.Y != 0 && grid[pos.Y - 1][pos.X] - grid[pos.Y][pos.X] <= 1)
            {
                posToCheck.Enqueue(new PositionWithHistory(pos.X, pos.Y - 1, pos.NumberOfSteps + 1));
            }
        }

        return int.MaxValue;
    }
}
