namespace AdventOfCode;

public record Position(int X, int Y);

public record PositionWithHistory(int X, int Y, int NumberOfSteps) : Position(X, Y);