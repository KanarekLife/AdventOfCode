namespace AdventOfCode;

public class FileSystem
{
    private Directory? _currentDirectory = null;
    private readonly Directory _rootDirectory = new("/");
    
    public int GetSumOfSizesOfSmallestDirectories()
    {
        var queue = new Queue<Directory>(new []
        {
            _rootDirectory
        });
        
        var sum = 0;

        while (queue.Count > 0)
        {
            var dir = queue.Dequeue();
            foreach (var directory in dir.Directories)
            {
                queue.Enqueue(directory);
            }

            var size = dir.GetSize();
            if (size <= 100000)
            {
                sum += size;
            }
        }

        return sum;
    }

    public int FindDirectoryToRemove()
    {
        const int totalSize = 70000000;
        const int requiredSize = 30000000;

        var queue = new Queue<Directory>(new [] { _rootDirectory });
        var directorySizes = new List<int>();
        var usedSpace = 0;
        
        while (queue.Count > 0)
        {
            var dir = queue.Dequeue();
            foreach (var directory in dir.Directories)
            {
                queue.Enqueue(directory);
            }

            if (dir.Name == "/")
            {
                usedSpace = dir.GetSize();
                directorySizes.Add(usedSpace);
            }
            else
            {
                directorySizes.Add(dir.GetSize());
            }
        }
        
        var unusedSpace = totalSize - usedSpace;
        var requiredSpace = requiredSize - unusedSpace;

        var fittingDirectory = directorySizes
            .Where(x => x >= requiredSpace)
            .OrderBy(x => x)
            .FirstOrDefault();

        return fittingDirectory;
    }

    public void HandleCommands(string[] lines)
    {
        var commands = new List<string[]>(lines.Length);
        var currentCommand = new List<string> { lines[0] };
        for (var i = 1; i < lines.Length; i++)
        {
            if (lines[i].StartsWith('$'))
            {
                commands.Add(currentCommand.ToArray());
                currentCommand.Clear();
            }

            currentCommand.Add(lines[i]);
        }
        commands.Add(currentCommand.ToArray());
        
        foreach (var command in commands)
        {
            HandleCommand(command);
        }
    }
    
    private void HandleCommand(IReadOnlyList<string> command)
    {
        if (command[0].StartsWith("$ ls"))
        {
            for (var i = 1; i < command.Count; i++)
            {
                var arr = command[i].Split(' ');

                if (int.TryParse(arr[0], out var size))
                {
                    _currentDirectory?.AddFile(arr[1], size);
                }
                else
                {
                    _currentDirectory?.AddDirectory(arr[1]);
                }
            }
        }else if (command[0].StartsWith("$ cd"))
        {
            ChangePath(command[0].Split(' ').Last());
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private void ChangePath(string newPath)
    {
        _currentDirectory = newPath switch
        {
            "/" => _currentDirectory = _rootDirectory,
            ".." => _currentDirectory?.Parent ?? throw new InvalidOperationException(),
            _ => _currentDirectory?.GetDirectory(newPath) ?? throw new InvalidOperationException()
        };
    }
}