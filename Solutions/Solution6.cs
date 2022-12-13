using AdventOfCode.Framework;

[Solution(6)]
#if DEBUG
[SolutionInput("Input6_test.txt")]
#else
[SolutionInput("Input6.txt")]
#endif
public class Solution6 : Solution
{
    Subroutine subroutine;
    public Solution6(Input input) : base(input)
    {
        subroutine = new(input.Lines[0]);
    }

    protected override string? Problem1()
    {
        return (subroutine.FindFirstNewMessageIndex(4) + 1).ToString();
    }
    
    protected override string? Problem2()
    {
        return (subroutine.FindFirstNewMessageIndex(14) + 1).ToString();
    }
}

public class Subroutine
{
    public char[] RawInput { get; set; }
    public List<int> NewMessageIndex { get; set; }

    public Subroutine(string rawInput)
    {
        RawInput = rawInput.ToCharArray();
        NewMessageIndex = new();
    }

    public int FindFirstNewMessageIndex(int uniqueLength)
    {
        for (int i = 0; i < RawInput.Length - uniqueLength - 1; i++)
        {
            HashSet<char> charHashSet = new();
            for (int j = 0; j < uniqueLength; j++)
            {
                charHashSet.Add(RawInput[i + j]);
            }
            if (charHashSet.Count == uniqueLength)
            {
                return (i + uniqueLength - 1);
            }
        }
        return -2;
    }
}
