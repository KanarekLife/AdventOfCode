namespace AdventOfCode;

public class File
{
    public File(string name, int size, Directory parent)
    {
        Name = name;
        Size = size;
        Parent = parent;
    }

    public string Name { get; }
    public int Size { get; }
    public Directory Parent { get; }
}