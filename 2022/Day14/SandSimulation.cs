namespace AdventOfCode;

public record Position(int X, int Y);

public class SandSimulation
{
    private HashSet<Position> _blockedPositions;

    private SandSimulation(HashSet<Position> positions)
    {
        _blockedPositions = positions;
    }

    public static SandSimulation Parse(string[] lines)
    {
        var rockPositions = new HashSet<Position>();
        
        foreach (var line in lines)
        {
            var breakingPoints = line.Split(" -> ")
                .Select(point =>
                {
                    var coordinates = point.Split(',');
                    return new Position(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
                })
                .ToArray();
            
            for (var i = 1; i < breakingPoints.Length; i++)
            {
                var from = breakingPoints[i-1];
                var to = breakingPoints[i];

                if (to.X != from.X)
                {
                    var min = Math.Min(from.X, to.X);
                    var max = Math.Max(from.X, to.X);
                    for (var x = min; x <= max; x++)
                    {
                        rockPositions.Add(to with { X = x });
                    }
                }else if (to.Y != from.Y)
                {
                    var min = Math.Min(from.Y, to.Y);
                    var max = Math.Max(from.Y, to.Y);
                    for (var y = min; y <= max; y++)
                    {
                        rockPositions.Add(to with { Y = y });
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            
        }
        
        return new SandSimulation(rockPositions);
    }

    public int GetUnitsOfSandBeforeInfiniteFall()
    {
        var positions = new HashSet<Position>(_blockedPositions);
        var maxY = positions.Select(pos => pos.Y).Max();
        var sands = 0;
        var fallingIntoInfinity = false;

        while (!fallingIntoInfinity)
        {
            var position = new Position(500, 0);

            while (true)
            {
                if (position.Y > maxY + 1)
                {
                    fallingIntoInfinity = true;
                    break;
                }
                
                if (!positions.Contains(position with { Y = position.Y + 1 }))
                {
                    position = position with { Y = position.Y + 1 };
                    continue;
                }

                if (!positions.Contains(new Position(position.X - 1, position.Y + 1)))
                {
                    position = new Position(position.X - 1, position.Y + 1);
                    continue;
                }

                if (!positions.Contains(new Position(position.X + 1, position.Y + 1)))
                {
                    position = new Position(position.X + 1, position.Y + 1);
                    continue;
                }

                sands++;
                positions.Add(position);
                break;
            }
        }

        return sands;
    }
    
    public int GetUnitsOfSandBeforeBlockingInput()
    {
        var positions = new HashSet<Position>(_blockedPositions);
        var maxY = positions.Select(pos => pos.Y).Max() + 2;
        var sands = 0;
        var hasBlockedTheInput = false;

        while (!hasBlockedTheInput)
        {
            var position = new Position(500, 0);

            while (true)
            {
                if (position.Y + 1 < maxY && !positions.Contains(position with { Y = position.Y + 1 }))
                {
                    position = position with { Y = position.Y + 1 };
                    continue;
                }

                if (position.Y + 1 < maxY && !positions.Contains(new Position(position.X - 1, position.Y + 1)))
                {
                    position = new Position(position.X - 1, position.Y + 1);
                    continue;
                }

                if (position.Y + 1 < maxY && !positions.Contains(new Position(position.X + 1, position.Y + 1)))
                {
                    position = new Position(position.X + 1, position.Y + 1);
                    continue;
                }

                sands++;
                if (position is { X: 500, Y: 0 })
                {
                    hasBlockedTheInput = true;
                    break;
                }
                positions.Add(position);
                break;
            }
        }

        return sands;
    }
}