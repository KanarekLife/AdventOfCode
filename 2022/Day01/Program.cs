var caloriesByElf = File.ReadAllText("input.txt")
    .Split(Environment.NewLine + Environment.NewLine)
    .Select(inventory => inventory
        .Split(Environment.NewLine)
        .Where(value => !string.IsNullOrWhiteSpace(value))
        .Select(int.Parse)
        .Sum()
    )
    .ToArray();

Console.WriteLine($"Puzzle 1: {caloriesByElf.Max()}");

var sumOfTop3CaloriesCarriedByElf = caloriesByElf
    .OrderBy(x => x)
    .TakeLast(3)
    .Sum();

Console.WriteLine($"Puzzle 2: {sumOfTop3CaloriesCarriedByElf}");