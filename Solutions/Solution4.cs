using AdventOfCode.Framework;

[Solution(4)]
#if DEBUG
[SolutionInput("Input4_test.txt")]
#else
[SolutionInput("Input4.txt")]
#endif
public class Solution4 : Solution
{
    List<ElfPair> ElfPairs;
    public Solution4(Input input) : base(input)
    {
        ElfPairs = new();
        foreach (var line in input.Lines)
        {
            if(!string.IsNullOrEmpty(line))
            {
                ElfPairs.Add(new ElfPair(line.Split(',')));
            }
        }
    }

    protected override string? Problem1()
    {
        int subsetCount = 0;
        foreach (var elfpair in ElfPairs)
        {
            if (elfpair.DoesContainSubset)
            {
                subsetCount++;
            }
        }
        return subsetCount.ToString();
    }
    
    protected override string? Problem2()
    {
        int overlapCount = 0;
        foreach (var elfpair in ElfPairs)
        {
            if (elfpair.DoesContainOverlap)
            {
                overlapCount++;
            }
        }
        return overlapCount.ToString();
    }
}

public class ElfPair
{
    public SectionRange FirstSet { get; set; }
    public SectionRange SecondSet { get; set; }
    public bool DoesContainSubset
    {
        get => IsOneSetASubsetOfTheOther();
    }
    public bool DoesContainOverlap
    {
        get => AnyOverlap();
    }

    public ElfPair(string[] pairOfIdRanges)
    {
        FirstSet = new SectionRange(pairOfIdRanges[0]);
        SecondSet = new SectionRange(pairOfIdRanges[1]);
    }
    
    private bool IsOneSetASubsetOfTheOther()
    {
        return ((FirstSet.Start >= SecondSet.Start && FirstSet.End <= SecondSet.End)
                ||
                (FirstSet.Start <= SecondSet.Start && FirstSet.End >= SecondSet.End));
    }

    private bool AnyOverlap()
    {
        foreach (var id in FirstSet.AllIds)
        {
            if (SecondSet.AllIds.Contains(id))
            {
                return true;
            }
        }
        return false;
    }
}

public class SectionRange
{
    public int Start { get; set; }
    public int End { get; set; }
    public int Range { get; set;}
    public List<int> AllIds { get; set; }
    
    public SectionRange(string range)
    {
        var strArr = range.Split('-');
        Start = int.Parse(strArr[0]);
        End = int.Parse(strArr[1]);
        
        AllIds = new();
        for (int i = Start; i <= End; i++)
        {
            AllIds.Add(i);
        }
        Range = AllIds.Count;
    }
}
