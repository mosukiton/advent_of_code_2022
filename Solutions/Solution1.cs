using AdventOfCode.Framework;

[Solution(1)]
[SolutionInput("Input1.txt")]
public class Solution1 : Solution
{
    List<Elf> ElfList;

    public Solution1(Input input) : base(input)
    {
        ElfList = new();
        int calorieCount = 0;
        foreach (string line in Input.Lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                calorieCount += int.Parse(line);
            }
            else
            {
                Elf newElf = new(ElfList.Count + 1);
                newElf.AddCalories(calorieCount);
                ElfList.Add(newElf);
                calorieCount = 0;
            }
        }
    }

    protected override string? Problem1()
    {
        return ElfList.OrderByDescending(e => e.Calories).First().Calories.ToString();
    }
    
    protected override string? Problem2()
    {
        return ElfList.OrderByDescending(e => e.Calories).Take(3)
            .Select(x => x.Calories).Sum().ToString();
    }
}

public class Elf
{
    public int Id { get; private set; } 
    public int Calories { get; private set; }

    public Elf(int id)
    {
        Id = id;
        Calories = default;
    }

    public void AddCalories(int x)
    {
        Calories = x;
    }
}
