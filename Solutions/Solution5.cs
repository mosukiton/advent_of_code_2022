using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Framework;

[Solution(5)]
#if DEBUG
[SolutionInput("Input5_test.txt")]
#else
[SolutionInput("Input5.txt")]
#endif
public class Solution5 : Solution
{
    SupplyStacks supplyStacks;
    public Solution5(Input input) : base(input)
    {
        int lineIndexOfStackBase = 0;
        char[] stackIdLine = {};
        List<string> Lines = new(input.Lines);
        // find number of stacks
        foreach (string line in Lines)
        {
            if (!line.Contains('['))
            {
                lineIndexOfStackBase = Lines.IndexOf(line);
                stackIdLine = line.ToCharArray();
                var totalStacks = line.Split(' ',
                            StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x))
                    .Max();
                supplyStacks = new(totalStacks);
                break;
            }
        }

        // populate stacks
        var stackIdLineList = new List<char>(stackIdLine);
        foreach (char c in stackIdLineList)
        {
            string cstring = c.ToString();
            if (int.TryParse(cstring, out int stackId))
            {
                int columnIndex = stackIdLineList.IndexOf(c);
                for(int i = lineIndexOfStackBase - 1; i >= 0; i--)
                {
                    var item = input.Lines[i].ToCharArray()[columnIndex];
                    if (!string.IsNullOrEmpty(item.ToString()) &&
                            !char.IsWhiteSpace(item))
                    {
                        supplyStacks!.Stacks[stackId - 1].Push(item);
                    }
                }
            }
        }

        //test
        //foreach (Stack<char> stack in supplyStacks!.Stacks)
        //{
            //Console.WriteLine(supplyStacks.Stacks.IndexOf(stack) + 1);
            ////foreach (char x in stack)
            ////{
                ////Console.WriteLine(x);
            ////}
            //while (stack.Count > 0)
            //{
                //Console.WriteLine(stack.Pop());
            //}
        //}
    }

    protected override string? Problem1()
    {
        return null;
    }
    
    protected override string? Problem2()
    {
        return null;
    }
}

public class SupplyStacks
{
    public List<Stack<char>> Stacks { get; set; }

    public SupplyStacks(int numberOfStacks)
    {
        Stacks = new();
        for (int i = 1; i <= numberOfStacks; i++)
        {
            Stacks.Add(new Stack<char>());
        }
    }
}
