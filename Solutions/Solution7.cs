using AdventOfCode.Framework;

[Solution(7)]
#if DEBUG
[SolutionInput("Input7_test.txt")]
#else
[SolutionInput("Input7.txt")]
#endif
public class Solution7 : Solution
{
    FileSystem fileSystem; 
    private const int TotalSpace = 70_000_000;
    private const int RequireSpace = 30_000_000;
    public Solution7(Input input) : base(input)
    {
        fileSystem = new();
        foreach (string line in input.Lines)
        {
            fileSystem.ParseTerminal(line);
        }
    }

    protected override string? Problem1()
    {
        fileSystem.ResetCurrentDirectory();
        List<int> allDirectorySizes = new();
        allDirectorySizes.Add(fileSystem.Root.Size);
        return RecursiveSizeSearch(fileSystem.CurrentDirectory, allDirectorySizes)
            .Where(x => x < 100000)
            .Sum()
            .ToString();
    }

    private List<int> RecursiveSizeSearch(Directory rootDir, List<int> allDirectorySizes)
    {
        foreach (Directory dir in rootDir.ChildrenDirectories)
        {
            allDirectorySizes.Add(dir.Size);
            RecursiveSizeSearch(dir, allDirectorySizes);
        }
        return allDirectorySizes;
    }
    
    protected override string? Problem2()
    {
        fileSystem.ResetCurrentDirectory();
        List<int> allDirectorySizes = new();
        allDirectorySizes.Add(fileSystem.Root.Size);
        int freeSpace = TotalSpace - allDirectorySizes[0];
        int spaceToFree = RequireSpace - freeSpace;
        return RecursiveSizeSearch(fileSystem.CurrentDirectory, allDirectorySizes)
            .Where(x => x > spaceToFree)
            .OrderBy(x => x)
            .First()
            .ToString();
    }
}

public class FileSystem 
{
    public Directory Root {get; init;}
    public Directory CurrentDirectory { get; set; }

    public FileSystem()
    {
        Root = new(null, "/");
        CurrentDirectory = Root;
    }

    public void ResetCurrentDirectory()
    {
        ChangeDirectory("/");
    }

    public void ParseTerminal(string line)
    {
        var lineSplit = line.Split(' ');
        if (line.Contains('$'))
        {
            if (lineSplit[1] == "cd")
            {
                ChangeDirectory(lineSplit[2]);
            }
        }
        if (lineSplit[0] == "dir")
        {
            CurrentDirectory.ChildrenDirectories.Add(new Directory(CurrentDirectory, lineSplit[1]));
        }
        if (int.TryParse(lineSplit[0], out int fileSize))
        {
            CurrentDirectory.Files.Add(new File(CurrentDirectory, fileSize, lineSplit[1]));
        }
    }

    private void ChangeDirectory(string name)
    {
        if (name == "/")
        {
            CurrentDirectory = Root;
        }
        else if (name == "..")
        {
            CurrentDirectory = CurrentDirectory!.Parent!;
        }
        else
        {
            CurrentDirectory = CurrentDirectory.ChildrenDirectories.First(x => x.Name == name);
        }
    }
}

public class Directory
{
    public Directory? Parent { get; init; }
    public List<Directory> ChildrenDirectories { get; init; }
    public List<File> Files { get; init; }
    public string Name { get; init; }
    public int Size 
    {
        get => CalculateRecursiveSize(this);
    }

    public Directory(Directory? parent, string name)
    {
        Parent = parent;
        ChildrenDirectories = new();
        Files = new();
        Name = name;
    }

    private int CalculateRecursiveSize(Directory sourceDirectory)
    {
        int size = 0;
        size += sourceDirectory.Files.Select(x => x.Size).Sum();
        foreach(Directory dir in sourceDirectory.ChildrenDirectories)
        {
            size += CalculateRecursiveSize(dir);
        }
        return size;
    }
}

public class File
{
    public Directory Parent { get; init;} 
    public string Name { get; init; }
    //public string Extension { get; init; }
    public int Size { get; init; }

    public File(Directory parent, int fileSize, string name)
    {
        Parent = parent;
        Size = fileSize;
        Name = name;
        //Extension = splitTerminalOutput[1].Split('.').Last();
    }
}
