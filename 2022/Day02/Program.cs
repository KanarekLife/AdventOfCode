namespace AdventOfCode;

public class Program
{
    private enum Move
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private enum Outcome
    {
        Lose = 0,
        Draw = 3,
        Win = 6
    }


    private static Outcome GetOutcome(Move opponentsMove, Move myMove)
    {
        if (opponentsMove == myMove)
        {
            return Outcome.Draw;
        }

        return opponentsMove switch
        {
            Move.Scissors when myMove == Move.Rock => Outcome.Win,
            Move.Rock when myMove == Move.Scissors => Outcome.Lose,
            _ => opponentsMove > myMove ? Outcome.Lose : Outcome.Win
        };
    }

    private static Move GetCorrectMove(Outcome outcome, Move opponentsMove)
    {
        return outcome switch

        {
            Outcome.Draw => opponentsMove,
            Outcome.Win => opponentsMove switch
            {
                Move.Rock => Move.Paper,
                Move.Paper => Move.Scissors,
                Move.Scissors => Move.Rock,
                _ => throw new ArgumentOutOfRangeException(nameof(opponentsMove), opponentsMove, null)
            },
            Outcome.Lose => opponentsMove switch
            {
                Move.Paper => Move.Rock,
                Move.Rock => Move.Scissors,
                Move.Scissors => Move.Paper,
                _ => throw new ArgumentOutOfRangeException(nameof(opponentsMove), opponentsMove, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
        };
    }

    private static int GetScore(IEnumerable<string> input)
    {
        return input
            .Select(round =>
            {
                var opponentsMove = (Move)(round[0] - 'A' + 1);
                var myMove = (Move)(round[2] - 'X' + 1);
                return (int)myMove + (int)GetOutcome(opponentsMove, myMove);
            }).Sum();
    }

    static int GetNewScore(IEnumerable<string> input)
    {
        return input
            .Select(round =>
            {
                var opponentsMove = (Move)(round[0] - 'A' + 1);
                var outcome = (Outcome)((round[2] - 'X') * 3);
                return (int)GetCorrectMove(outcome, opponentsMove) + (int)outcome;
            }).Sum();
    }
    
    public static void Main()
    {
        var data = File.ReadAllLines("input.txt");
        Console.WriteLine($"Puzzle 1: {GetScore(data)}");
        Console.WriteLine($"Puzzle 2: {GetNewScore(data)}");
    }
}