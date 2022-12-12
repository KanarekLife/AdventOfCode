namespace AdventOfCode;

public class Directory
{
    public Directory(string name, Directory? parent = null)
    {
        Name = name;
        Parent = parent;
    }

    public string Name { get; }
    private readonly List<File> _files = new();
    private readonly List<Directory> _directories = new();
    public IEnumerable<Directory> Directories => _directories;
    public Directory? Parent { get; } = null;

    public void AddFile(string name, int size)
    {
        _files.Add(new File(name, size, this));
    }

    public void AddDirectory(string name)
    {
        _directories.Add(new Directory(name, this));
    }

    public int GetSize()
    {
        return GetSizeOfFiles() + Directories.Select(dir => dir.GetSize()).Sum();
    }

    public Directory? GetDirectory(string name)
    {
        return _directories.FirstOrDefault(x => x.Name == name);
    }

    private int GetSizeOfFiles()
    {
        return _files.Select(x => x.Size).Sum();
    }
}