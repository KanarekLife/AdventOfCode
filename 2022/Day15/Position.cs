namespace AdventOfCode;

public record Position(int X, int Y)
{
    public int GetDistance(Position otherPosition)
        => Math.Abs(X - otherPosition.X) + Math.Abs(Y - otherPosition.Y);
};