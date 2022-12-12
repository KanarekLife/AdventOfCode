using System.Numerics;

namespace AdventOfCode;

public class Monkey : ICloneable
{
    public int Id { get; }
    
    private Queue<BigInteger> _items;
    public IReadOnlyCollection<BigInteger> Items => _items;

    private int _add;
    private int _multiply;
    private int _pow;
    public int DividableBy { get; private set; }
    private int _whenTrueThrowTo;
    private int _whenFalseThrowTo;

    public long TimesItemInspected { get; private set; } = 0;

    private Monkey(
        int id,
        IEnumerable<BigInteger> items,
        int dividableBy,
        int add,
        int multiply,
        int pow,
        int whenTrueThrowTo,
        int whenFalseThrowTo
        )
    {
        Id = id;
        _items = new Queue<BigInteger>(items);
        DividableBy = dividableBy;
        _add = add;
        _multiply = multiply;
        _pow = pow;
        _whenTrueThrowTo = whenTrueThrowTo;
        _whenFalseThrowTo = whenFalseThrowTo;
    }

    public void Turn(MonkeyGroup group)
    {
        while (_items.Count > 0)
        {
            var item = _items.Dequeue();
            var worryLevel = (Pow(item, _pow) * _multiply + _add) / 3;
            group.GiveTo(
                worryLevel % DividableBy == 0 ? _whenTrueThrowTo : _whenFalseThrowTo,
                worryLevel
            );
            TimesItemInspected++;
        }
    }

    private static BigInteger Pow(BigInteger item, int n)
    {
        var r = item;
        for (var i = 2; i <= n; i++)
        {
            r *= item;
        }
        return r;
    }

    public void TurnWithoutDivision(MonkeyGroup group)
    {
        while (_items.Count > 0)
        {
            var item = _items.Dequeue();
            var worryLevel = (Pow(item, _pow) * _multiply + _add) % group.GetMultiplicationOfDivisionValues();
            group.GiveTo(
                worryLevel % DividableBy == 0 ? _whenTrueThrowTo : _whenFalseThrowTo,
                worryLevel
            );
            TimesItemInspected++;
        }
    }

    public void AddItem(BigInteger item)
    {
        _items.Enqueue(item);
    }

    public static Monkey Parse(string input)
    {
        var data = input.Split(Environment.NewLine);
        var id = int.Parse(data[0].Trim().Replace("Monkey ", "").Replace(":", ""));
        var items = data[1].Trim().Replace("Starting items: ", "").Split(',').Select(BigInteger.Parse);

        var operationData = data[2].Trim().Replace("Operation: ", "").Split(' ');
        var add = 0;
        var multiply = 1;
        var pow = 1;

        switch (operationData[3])
        {
            case "+":
                add = int.Parse(operationData[4]);
                break;
            case "*" when int.TryParse(operationData[4], out var value):
                multiply = value;
                break;
            case "*" when operationData[4] == "old":
                pow = 2;
                break;
            default:
                throw new InvalidOperationException();
        }
        
        var dividableBy = int.Parse(data[3].Trim().Split(' ').Last());
        var whenTrueThrowTo = int.Parse(data[4].Trim().Split(' ').Last());
        var whenFalseThrowTo = int.Parse(data[5].Trim().Split(' ').Last());

        return new Monkey(id, items, dividableBy, add, multiply, pow, whenTrueThrowTo, whenFalseThrowTo);
    }

    public object Clone()
    {
        Monkey m = new Monkey(Id, Items, DividableBy, _add, _multiply, _pow, _whenTrueThrowTo, _whenFalseThrowTo);
        return m;
    }
}