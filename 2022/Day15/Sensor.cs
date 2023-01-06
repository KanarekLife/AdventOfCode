namespace AdventOfCode;

public struct Sensor
{
    public Position Position;
    public Position NearestBeacon;
    public int Radius;
    public int Circumference;

    public Sensor(Position position, Position nearestBeacon)
    {
        Position = position;
        NearestBeacon = nearestBeacon;
        Radius = Position.GetDistance(NearestBeacon);
        Circumference = 8 * Radius;
    }

    public Range GetValuesAtY(int y)
    {
        var distance = Math.Abs(y - Position.Y);

        if (distance > Radius)
        {
            return new Range();
        }

        var difference = Radius - distance;

        return new Range(Position.X - difference, Position.X + difference);
    }
}