using AdventOfCode.Framework;

[Solution(2)]
//[SolutionInput("Input2_test.txt")]
[SolutionInput("Input2.txt")]
public class Solution2 : Solution
{
    // Opponent:
    // A = Rock, B = Paper, C = Scissors
    //
    // Me:
    // X = Rock, Y = Paper, C = Scissors
    //
    // Points for picking:
    // 1 = Rock, 2 = Paper, 3 = Scissors
    //
    // Points for Result:
    // 0 = Lost, 3 = Draw, 6 = Won

    private int Problem1Points = 0;
    private int Problem2Points = 0;

    public Solution2(Input input) : base(input)
    {
        foreach (string line in input.Lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                char[] moves = line.ToCharArray();
                Problem1Points += PointAllocator.GetProb1Points(moves[0], moves[2]);
                Problem2Points += PointAllocator.GetProb2Points(moves[0], moves[2]);
            }
        }
    }

    protected override string Problem1()
    {
        return Problem1Points.ToString();
    }
    
    protected override string? Problem2()
    {
        return Problem2Points.ToString();
    }
}

public static class PointAllocator
{
    private static readonly Dictionary<char, RPSMove> CharToRPSMove = new()
    {
        {'A', RPSMove.Rock },
        {'B', RPSMove.Paper },
        {'C', RPSMove.Scissors },
        {'X', RPSMove.Rock },
        {'Y', RPSMove.Paper },
        {'Z', RPSMove.Scissors },
    };
    
    private static readonly Dictionary<char, RPSResult> CharToRPSResult = new()
    {
        {'X', RPSResult.Loss },
        {'Y', RPSResult.Draw },
        {'Z', RPSResult.Win },
    };

    private static readonly Dictionary<RPSMove, int> PointsFromMove = new()
    {
        {RPSMove.Rock, 1 },
        {RPSMove.Paper, 2 },
        {RPSMove.Scissors, 3 },
    };

    private static readonly Dictionary<RPSResult, int> PointsFromResult = new()
    {
        {RPSResult.Win, 6 },
        {RPSResult.Loss, 0 },
        {RPSResult.Draw, 3 },
    };

    public static int GetProb1Points(char opponentMove, char myMove)
    {
        RPSMove Attack = CharToRPSMove[opponentMove];
        RPSMove CounterAttack = CharToRPSMove[myMove];
        return  PointsFromMove[CounterAttack] + 
                PointsFromResult[GetRPSResult(Attack, CounterAttack)];
    }
    
    public static int GetProb2Points(char opponentMove, char desiredResult)
    {
        RPSMove attack = CharToRPSMove[opponentMove];
        RPSResult result = CharToRPSResult[desiredResult];
        RPSMove myMove = GetRequiredMove(attack, result);
        return PointsFromMove[myMove] + PointsFromResult[result];
    }

    private static RPSMove GetRequiredMove(RPSMove attack, RPSResult result)
    {
        switch (attack)
        {
            case RPSMove.Rock:
                switch (result)
                {
                    case RPSResult.Win:
                        return RPSMove.Paper;
                    case RPSResult.Loss:
                        return RPSMove.Scissors;
                    case RPSResult.Draw:
                        return RPSMove.Rock;
                    default:
                        return RPSMove.Unknown;
                }
            case RPSMove.Paper:
                switch (result)
                {
                    case RPSResult.Win:
                        return RPSMove.Scissors;
                    case RPSResult.Loss:
                        return RPSMove.Rock;
                    case RPSResult.Draw:
                        return RPSMove.Paper;
                    default:
                        return RPSMove.Unknown;
                }
            case RPSMove.Scissors:
                switch (result)
                {
                    case RPSResult.Win:
                        return RPSMove.Rock;
                    case RPSResult.Loss:
                        return RPSMove.Paper;
                    case RPSResult.Draw:
                        return RPSMove.Scissors;
                    default:
                        return RPSMove.Unknown;
                }
            default:
                return RPSMove.Unknown;
        }
    }

    private static RPSResult GetRPSResult(RPSMove attack, RPSMove counterAttack)
    {
        switch (attack)
        {
            case RPSMove.Rock:
                switch (counterAttack)
                {
                    case RPSMove.Rock:
                        return RPSResult.Draw;
                    case RPSMove.Paper:
                        return RPSResult.Win;
                    case RPSMove.Scissors:
                        return RPSResult.Loss;
                    default:
                        return RPSResult.Unknown;
                }
            case RPSMove.Paper:
                switch (counterAttack)
                {
                    case RPSMove.Rock:
                        return RPSResult.Loss;
                    case RPSMove.Paper:
                        return RPSResult.Draw;
                    case RPSMove.Scissors:
                        return RPSResult.Win;
                    default:
                        return RPSResult.Unknown;
                }
            case RPSMove.Scissors:
                switch (counterAttack)
                {
                    case RPSMove.Rock:
                        return RPSResult.Win;
                    case RPSMove.Paper:
                        return RPSResult.Loss;
                    case RPSMove.Scissors:
                        return RPSResult.Draw;
                    default:
                        return RPSResult.Unknown;
                }
            default:
                return RPSResult.Unknown;
        }
    }
}

public enum RPSMove
{
    Rock,
    Paper,
    Scissors,
    Unknown
}
public enum RPSResult
{
    Win,
    Loss,
    Draw,
    Unknown
}
