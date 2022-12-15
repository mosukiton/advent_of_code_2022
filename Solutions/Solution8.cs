using AdventOfCode.Framework;

[Solution(8)]
#if DEBUG
[SolutionInput("Input8_test.txt")]
#else
[SolutionInput("Input8.txt")]
#endif
public class Solution8 : Solution
{
    Grid grid;
    public Solution8(Input input) : base(input)
    {
        grid = new();
        foreach (string line in input.Lines)
        {
            List<Tree> newRow = new();
            foreach (char c in line)
            {
                newRow.Add(new Tree(int.Parse(c.ToString()),
                                    grid.Trees.Count,
                                    newRow.Count));
            }
            if (newRow.Count != 0)
            {
                grid.Trees.Add(newRow);
            }
        }
        grid.DetermineVisibilityOfTrees();
    }

    protected override string? Problem1()
    {
        int totalVisible = 0;
        foreach (var row in grid.Trees)
        {
            foreach (var tree in row)
            {
                int isVisible = tree.Visibility.Count > 0 ? 1 : 0;
                totalVisible += isVisible;
            }
        }
#if DEBUG
        foreach (var row in grid.Trees)
        {
            foreach (var tree in row)
            {
                Console.Write(tree.Height);
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        foreach (var row in grid.Trees)
        {
            foreach (var tree in row)
            {
                if (tree.IsVisible)
                {
                    Console.Write(tree.Height);
                }
                else 
                {
                    Console.Write("x");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        foreach (var row in grid.Trees)
        {
            foreach (var tree in row)
            {
                Console.Write(tree.ScenicScore);
            }
            Console.WriteLine();
        }
        Console.WriteLine();

#endif
        return totalVisible.ToString();
    }
    
    protected override string? Problem2()
    {
        return grid.Trees.SelectMany(x => x).Max(x => x.ScenicScore).ToString();
    }
}

public class Grid
{
    public List<List<Tree>> Trees { get; set; }

    public Grid()
    {
        Trees = new();
    }

    public void DetermineVisibilityOfTrees()
    {
        foreach (List<Tree> treeRow in Trees)
        {
            foreach (Tree tree in treeRow)
            {
                LookFromNorth(tree);
                LookFromEast(tree);
                LookFromSouth(tree);
                LookFromWest(tree);
            }
        }
    }

    private void LookFromNorth(Tree tree)
    {
        var northBlockingList =
            Trees.Where(x => Trees.IndexOf(x) < tree.RowIndex)
                 .SelectMany(x => x.Where(y => x.IndexOf(y) == tree.ColIndex))
                 .OrderByDescending(x => x.RowIndex);
        int northScoreCount = 0;
        if (!northBlockingList.Any())
        {
            tree.Visibility.Add(VisibleFrom.North);
            tree.NorthScore = northScoreCount;
        }
        else
        {
            foreach (Tree n in northBlockingList)
            {
                northScoreCount += 1;
                if (n.Height >= tree.Height)
                {
                    break;
                }
            }
            tree.NorthScore = northScoreCount;
        }
        if (!northBlockingList.Where(x => x.Height >= tree.Height).Any())
        {
            tree.Visibility.Add(VisibleFrom.North);
        }
    }

    private void LookFromEast(Tree tree)
    {
        List<Tree> treeRow = Trees[tree.RowIndex];
        var eastBlockingList = treeRow.Where(x => treeRow.IndexOf(x) > tree.ColIndex)
            .OrderBy(x => x.ColIndex);
        int eastScoreCount = 0;
        if (!eastBlockingList.Any())
        {
            tree.Visibility.Add(VisibleFrom.East);
            tree.EastScore = eastScoreCount;
        }
        else
        {
            foreach (Tree e in eastBlockingList)
            {
                eastScoreCount += 1;
                if (e.Height >= tree.Height)
                {
                    break;
                }
            }
            tree.EastScore = eastScoreCount;
        }
        if (!eastBlockingList.Where(x => x.Height >= tree.Height).Any())
        {
            tree.Visibility.Add(VisibleFrom.East);
        }
    }

    private void LookFromSouth(Tree tree)
    {
        var southBlockingList =
            Trees.Where(x => Trees.IndexOf(x) > tree.RowIndex)
                 .SelectMany(x => x.Where(y => x.IndexOf(y) == tree.ColIndex))
                 .OrderBy(x => x.RowIndex);
        int southScoreCount = 0;
        if (!southBlockingList.Any())
        {
            tree.Visibility.Add(VisibleFrom.South);
            tree.SouthScore = southScoreCount;
        }
        else
        {
            foreach (Tree s in southBlockingList)
            {
                southScoreCount += 1;
                if (s.Height >= tree.Height)
                {
                    break;
                }
            }
            tree.SouthScore = southScoreCount;
        }
        if (!southBlockingList.Where(x => x.Height >= tree.Height).Any())
        {
            tree.Visibility.Add(VisibleFrom.South);
        }
    }
    private void LookFromWest(Tree tree)
    {
        List<Tree> treeRow = Trees[tree.RowIndex];
        var westBlockingList = treeRow.Where(x => treeRow.IndexOf(x) < tree.ColIndex)
            .OrderByDescending(x => x.ColIndex);
        int westScoreCount = 0;
        if (!westBlockingList.Any())
        {
            tree.Visibility.Add(VisibleFrom.West);
            tree.EastScore = westScoreCount;
        }
        else
        {
            foreach (Tree w in westBlockingList)
            {
                westScoreCount += 1;
                if (w.Height >= tree.Height)
                {
                    break;
                }
            }
            tree.WestScore = westScoreCount;
        }
        if (!westBlockingList.Where(x => x.Height >= tree.Height).Any())
        {
            tree.Visibility.Add(VisibleFrom.West);
        }
    }
}

public class Tree
{
    public int Height { get; init;}
    public HashSet<VisibleFrom> Visibility { get; private set; }
    public bool IsVisible
    {
        get => Visibility.Count > 0 ? true : false;
    }
    
    public int RowIndex { get; set; }
    public int ColIndex { get; set; }

    public long ScenicScore
    {
        get => (long)(NorthScore * EastScore * SouthScore * WestScore);
    }
    public int NorthScore { get; set;}
    public int EastScore { get; set;}
    public int SouthScore { get; set;}
    public int WestScore { get; set;}
    
    public Tree(int height, int row, int col)
    {
        Height = height;
        Visibility = new();
        RowIndex = row;
        ColIndex = col;
        NorthScore = default;
        EastScore = default;
        SouthScore = default;
        WestScore = default;
    }

    public void ConfirmVisibility(VisibleFrom location) => Visibility.Add(location);
}

public enum VisibleFrom
{
    North,
    East,
    South,
    West
}
