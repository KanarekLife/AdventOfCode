namespace AdventOfCode;

class Program
{
    private enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    private record Command(Direction Dir, int Steps);

    private record Position(int X, int Y);

    private static double GetDistance(Position pos1, Position pos2)
    {
        return Math.Sqrt(Math.Pow(pos2.X - pos1.X, 2) + Math.Pow(pos2.Y - pos1.Y, 2));
    }

    private static int GetNumberOfVisitedPositions(IEnumerable<Command> moves, int n)
    {
        var visited = new HashSet<Position>(new[] { new Position(0, 0) });
        // [head, tail, tail, tail, ... ]
        var knots = Enumerable.Repeat(new Position(0, 0), n).ToArray();
        
        foreach (var move in moves)
        {
            for (var i = 0; i < move.Steps; i++)
            {
                knots[0] = move.Dir switch
                {
                    Direction.Right => knots[0] with { X = knots[0].X + 1 },
                    Direction.Left => knots[0] with { X = knots[0].X - 1 },
                    Direction.Up => knots[0] with { Y = knots[0].Y + 1 },
                    Direction.Down => knots[0] with { Y = knots[0].Y - 1 },
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                for (var j = 1; j < n; j++)
                {
                    if (!(GetDistance(knots[j - 1], knots[j]) >= 2.0)) break;

                    if (knots[j - 1].Y != knots[j].Y)
                    {
                        knots[j] = knots[j] with { Y = knots[j - 1].Y > knots[j].Y ? knots[j].Y + 1 : knots[j].Y - 1 };
                    }

                    if (knots[j - 1].X != knots[j].X)
                    {
                        knots[j] = knots[j] with { X = knots[j - 1].X > knots[j].X ? knots[j].X + 1 : knots[j].X - 1 };
                    }
                }

                visited.Add(knots[^1]);
            }
        }
        return visited.Count;
    }


    static void Main(string[] args)
    {
        var moves = File.ReadAllLines("input.txt")
            .Select(line =>
            {
                var arr = line.Split(' ');
                return new Command(arr[0][0] switch
                {
                    'R' => Direction.Right,
                    'L' => Direction.Left,
                    'U' => Direction.Up,
                    'D' => Direction.Down,
                    _ => throw new ArgumentOutOfRangeException()
                }, int.Parse(arr[1]));
            })
            .ToArray();

        Console.WriteLine($"Puzzle 1: {GetNumberOfVisitedPositions(moves, 2)}");
        Console.WriteLine($"Puzzle 2: {GetNumberOfVisitedPositions(moves, 10)}");
    }
}