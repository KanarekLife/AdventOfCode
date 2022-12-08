namespace AdventOfCode;

class Program
{
    private static int FindVisible(int[][] trees)
    {
        // n starts with the number of outer trees
        var n = (trees.Length * 2) + (trees[0].Length - 2) * 2;
        
        for (var i = 1; i < trees.Length-1; i++)
        {
            for (var j = 1; j < trees[i].Length-1; j++)
            {
                if (!IsHidden(trees, i, j))
                    n++;
            }
        }

        return n;
    }

    private static bool IsHidden(int[][] trees, int x, int y)
    {
        var htop = false;
        var hbottom = false;
        var hright = false;
        var hleft = false;
        
        // check top
        for (var i = y-1; i >= 0; i--)
        {
            if (trees[i][x] < trees[y][x]) continue;
            htop = true;
            break;
        }
        
        // check bottom
        for (var i = y+1; i < trees[x].Length; i++)
        {
            if (trees[i][x] < trees[y][x]) continue;
            hbottom = true;
            break;
        }
        
        // check right
        for (var i = x+1; i < trees.Length; i++)
        {
            if (trees[y][i] < trees[y][x]) continue;
            hright = true;
            break;
        }
        
        // check left
        for (var i = x-1; i >= 0; i--)
        {
            if (trees[y][i] < trees[y][x]) continue;
            hleft = true;
            break;
        }

        return htop && hbottom && hright && hleft;
    }

    private static int GetScore(int[][] trees, int x, int y)
    {
        var score = 1;
        var visibleTrees = 0;
        
        // check top
        for (var i = y-1; i >= 0; i--)
        {
            visibleTrees++;
            
            if (trees[i][x] >= trees[y][x])
            {
                break;
            }
        }

        score *= visibleTrees;
        visibleTrees = 0;
        
        // check bottom
        for (var i = y+1; i < trees[x].Length; i++)
        {
            visibleTrees++;
            
            if (trees[i][x] >= trees[y][x])
            {
                break;
            }
        }
        
        score *= visibleTrees;
        visibleTrees = 0;
        
        // check right
        for (var i = x+1; i < trees.Length; i++)
        {
            visibleTrees++;
            
            if (trees[y][i] >= trees[y][x])
            {
                break;
            }
        }
        
        score *= visibleTrees;
        visibleTrees = 0;
        
        // check left
        for (var i = x-1; i >= 0; i--)
        {
            visibleTrees++;

            if (trees[y][i] >= trees[y][x])
            {
                break;
            }
        }
        
        score *= visibleTrees;
        return score;
    }

    private static int FindBestTree(int[][] trees)
    {
        var bestScore = 0;
        for (var x = 0; x < trees.Length; x++)
        {
            for (var y = 0; y < trees[x].Length; y++)
            {
                bestScore = Math.Max(bestScore, GetScore(trees, x, y));
            }
        }
        return bestScore;
    }
    
    static void Main(string[] args)
    {
        // trees[y][x]
        var trees = File.ReadAllLines("input.txt")
            .Select(line => line.ToCharArray())
            .Select(ca => ca.Select(c => c - '0').ToArray())
            .ToArray();
        
        Console.WriteLine($"Puzzle 1: {FindVisible(trees)}");
        Console.WriteLine($"Puzzle 2: {FindBestTree(trees)}");
    }
}
