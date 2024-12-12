using static Utils.Utils;

var input = File.ReadAllText("input.txt");
var grid = ParseInput(input);

ExecuteAndMeasure("First part", () => SolvePartOne(grid));
ExecuteAndMeasure("Second part", () => SolvePartTwo(grid));

return 0;

static int SolvePartOne(char[][] grid)
{
    var visited = new bool[grid.Length, grid[0].Length];
    var totalPrice = 0;

    for (var i = 0; i < grid.Length; i++)
    {
        for (var j = 0; j < grid[0].Length; j++)
        {
            if (visited[i, j]) continue;
            
            var (area, perimeter) = CalculateRegionStats(grid, visited, i, j);
            totalPrice += area * perimeter;
        }
    }

    return totalPrice;
}

static int SolvePartTwo(char[][] grid)
{
    var visited = new bool[grid.Length, grid[0].Length];
    var totalPrice = 0;

    for (var i = 0; i < grid.Length; i++)
    {
        for (var j = 0; j < grid[0].Length; j++)
        {
            if (visited[i, j]) continue;
            
            var (area, sides) = FindRegionWithSides(grid, visited, i, j);
            totalPrice += area * sides;
        }
    }

    return totalPrice;
}

static char[][] ParseInput(string input)
{
    return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.ToCharArray())
                .ToArray();
}

static (int area, int perimeter) CalculateRegionStats(char[][] grid, bool[,] visited, int startI, int startJ)
{
    var area = 0;
    var perimeter = 0;
    var queue = new Queue<(int i, int j)>();
    queue.Enqueue((startI, startJ));
    visited[startI, startJ] = true;

    while (queue.Count > 0)
    {
        var (i, j) = queue.Dequeue();
        area++;

        var directions = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        foreach (var (di, dj) in directions)
        {
            var newI = i + di;
            var newJ = j + dj;

            if (IsValidPosition(grid, newI, newJ))
            {
                if (grid[newI][newJ] == grid[startI][startJ])
                {
                    if (visited[newI, newJ]) continue;
                    
                    queue.Enqueue((newI, newJ));
                    visited[newI, newJ] = true;
                }
                else
                {
                    perimeter++;
                }
            }
            else
            {
                perimeter++;
            }
        }
    }

    return (area, perimeter);
}

static (int area, int sides) FindRegionWithSides(char[][] grid, bool[,] visited, int startI, int startJ)
{
    var area = 0;
    var plant = grid[startI][startJ];
    var cells = new HashSet<(int i, int j)>();
    var queue = new Queue<(int i, int j)>();
    
    queue.Enqueue((startI, startJ));
    visited[startI, startJ] = true;
    cells.Add((startI, startJ));

    while (queue.Count > 0)
    {
        var (i, j) = queue.Dequeue();
        area++;

        var directions = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        foreach (var (di, dj) in directions)
        {
            var newI = i + di;
            var newJ = j + dj;

            if (!IsValidPosition(grid, newI, newJ) || grid[newI][newJ] != plant || visited[newI, newJ]) continue;
            
            queue.Enqueue((newI, newJ));
            visited[newI, newJ] = true;
            cells.Add((newI, newJ));
        }
    }

    return (area, CountSides(grid, cells));
}

static int CountSides(char[][] grid, HashSet<(int i, int j)> region)
{
    if (region.Count == 1) return 4;

    var sides = new HashSet<string>();
    
    foreach (var (i, j) in region)
    {
        if (!region.Contains((i - 1, j)))
        {
            var start = j;
            while (start > 0 && region.Contains((i, start - 1)) && !region.Contains((i - 1, start - 1)))
                start--;
                
            var end = j;
            while (end < grid[0].Length - 1 && region.Contains((i, end + 1)) && !region.Contains((i - 1, end + 1)))
                end++;
                
            sides.Add($"U{i},{start},{end}");
        }
        
        if (!region.Contains((i + 1, j)))
        {
            var start = j;
            while (start > 0 && region.Contains((i, start - 1)) && !region.Contains((i + 1, start - 1)))
                start--;
                
            var end = j;
            while (end < grid[0].Length - 1 && region.Contains((i, end + 1)) && !region.Contains((i + 1, end + 1)))
                end++;
                
            sides.Add($"D{i},{start},{end}");
        }
        
        if (!region.Contains((i, j - 1)))
        {
            var start = i;
            while (start > 0 && region.Contains((start - 1, j)) && !region.Contains((start - 1, j - 1)))
                start--;
                
            var end = i;
            while (end < grid.Length - 1 && region.Contains((end + 1, j)) && !region.Contains((end + 1, j - 1)))
                end++;
                
            sides.Add($"L{j},{start},{end}");
        }
        
        if (!region.Contains((i, j + 1)))
        {
            var start = i;
            while (start > 0 && region.Contains((start - 1, j)) && !region.Contains((start - 1, j + 1)))
                start--;
                
            var end = i;
            while (end < grid.Length - 1 && region.Contains((end + 1, j)) && !region.Contains((end + 1, j + 1)))
                end++;
                
            sides.Add($"R{j},{start},{end}");
        }
    }
    
    return sides.Count;
}

static bool IsValidPosition(char[][] grid, int i, int j)
{
    return i >= 0 && i < grid.Length && j >= 0 && j < grid[0].Length;
}
