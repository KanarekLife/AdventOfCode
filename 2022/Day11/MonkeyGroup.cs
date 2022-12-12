using System.Numerics;

namespace AdventOfCode;

public class MonkeyGroup
{
    private Monkey[] _monkeys;
    private int _getMultiplicationOfDivisionValues;
    private readonly Monkey[] _originalMonkeys;

    private MonkeyGroup(Monkey[] monkeys)
    {
        _monkeys = monkeys;
        _getMultiplicationOfDivisionValues = _monkeys.Select(x => x.DividableBy).Aggregate((a, b) => a * b);
        _originalMonkeys = monkeys.Select(a => (Monkey)a.Clone()).ToArray();
    }

    public static MonkeyGroup Parse(string input)
    {
        var monkeys = input
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(Monkey.Parse)
            .OrderBy(x => x.Id)
            .ToArray();
        
        return new MonkeyGroup(monkeys);
    }

    public long GetMonkeyBusinessAfterNRounds(int n)
    {
        for (var i = 0; i < n; i++)
        {
            foreach (var monkey in _monkeys)
            {
                monkey.Turn(this);
            }
        }
        var monkeyBusiness = _monkeys
            .Select(x => x.TimesItemInspected)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((s1, s2) => s1 * s2);
        Reset();
        return monkeyBusiness;
    }
    
    public long GetMonkeyBusinessAfterNRoundsWithoutDivision(int n)
    {
        for (var i = 0; i < n; i++)
        {
            foreach (var monkey in _monkeys)
            {
                monkey.TurnWithoutDivision(this);
            }
        }
        var monkeyBusiness = _monkeys
            .Select(x => x.TimesItemInspected)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((s1, s2) => s1 * s2);
        Reset();
        return monkeyBusiness;
    }

    public void GiveTo(int monkeyId, BigInteger item)
    {
        _monkeys[monkeyId].AddItem(item);
    }

    public int GetMultiplicationOfDivisionValues() => _getMultiplicationOfDivisionValues;

    private void Reset()
    {
        _monkeys = _originalMonkeys;
        _getMultiplicationOfDivisionValues = _monkeys.Select(x => x.DividableBy).Aggregate((a, b) => a * b);
    }
}