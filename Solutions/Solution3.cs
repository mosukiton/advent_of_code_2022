using AdventOfCode.Framework;

[Solution(3)]
//[SolutionInput("Input3_test.txt")]
[SolutionInput("Input3.txt")]
public class Solution3 : Solution
{
    List<Rucksack> Rucksacks;
    List<ElfGroup> ElfGroups;
    public Solution3(Input input) : base(input)
    {
        Rucksacks = new();
        ElfGroups = new();
        foreach (var line in input.Lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                Rucksacks.Add(new Rucksack(line));
            }
        }
        foreach (var elf in Rucksacks)
        {
            if (Rucksacks.IndexOf(elf) % 3 == 0)
            {
                ElfGroups.Add(
                        new ElfGroup(
                            elf,
                            Rucksacks[(Rucksacks.IndexOf(elf) + 1)],
                            Rucksacks[Rucksacks.IndexOf(elf) + 2]
                            )
                        );
            }
        }
    }

    protected override string? Problem1()
    {
        int priorityCount = 0; 
        foreach (var rucksack in Rucksacks)
        {
            //Console.WriteLine(rucksack.Duplicate);
            priorityCount += rucksack.DuplicatePriority;
        }
        return priorityCount.ToString();
    }
    
    protected override string? Problem2()
    {
        int prioCount = 0;
        foreach (var group in ElfGroups)
        {
            prioCount += group.CommonItemPriority;
        }
        return prioCount.ToString();
    }
}

public class Rucksack
{
    public char[] Items { get; set; }
    public char[] CompartmentOne { get; set; }
    public char[] CompartmentTwo { get; set; }
    public char Duplicate { get; set; }
    public int DuplicatePriority 
    {
        get => CharStuff.GetPriority(Duplicate);
    }

    public Rucksack(string items)
    {
        Duplicate = default;
        Items = items.ToCharArray();
        int totalItemCount = items.Length;
        CompartmentOne = items.Substring(0, totalItemCount/2).ToCharArray();
        CompartmentTwo = items.Substring(totalItemCount/2, totalItemCount/2)
            .ToCharArray();
        FindDuplicate();
    }

    public void FindDuplicate()
    {
        foreach (char item in CompartmentOne)
        {
            if (CompartmentTwo.Contains(item))
            {
                Duplicate = item;
            }
        }
    }

}

public class ElfGroup
{
    public List<Rucksack> Elves { get; set; }
    public char CommonItem { get; set; }
    public int CommonItemPriority
    {
        get => CharStuff.GetPriority(CommonItem);
    }
    public ElfGroup(Rucksack first, Rucksack second, Rucksack third)
    {
        Elves = new();
        Elves.Add(first);
        Elves.Add(second);
        Elves.Add(third);
        CommonItem = default;
        FindCommonItem();
    }

    public void FindCommonItem()
    {
        var firstElf = Elves[0];
        foreach (var item in firstElf.Items)
        {
            if (Elves[1].Items.Contains(item) &&
                    Elves[2].Items.Contains(item))
            {
                CommonItem = item;
            }
        }
    }
}

public static class CharStuff 
{
    public static int GetPriority(char x)
    {
        if (char.IsUpper(x))
        {
            return (((int)x) - 38);
        }
        else 
        {
            return (((int)x) - 96);
        }
    }
}
