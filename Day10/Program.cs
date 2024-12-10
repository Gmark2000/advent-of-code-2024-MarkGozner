using static Utils.Utils;

var lines = ReadInput("input.txt");
var grid = ParseGrid(lines);

ExecuteAndMeasure("First part", () => SolvePartOne(grid));
ExecuteAndMeasure("Second part", () => SolvePartTwo(grid));

return 0;

static int[][] ParseGrid(string[] input)
{
    return input.Select(line => line.Select(c => c - '0').ToArray())
                .ToArray();
}

static IEnumerable<(int row, int col)> FindTrailheads(int[][] grid)
{
    List<(int row, int col)> trailheads = [];
    
    for (var row = 0; row < grid.Length; row++)
    {
        for (var col = 0; col < grid[row].Length; col++)
        {
            if (grid[row][col] == 0)
            {
                trailheads.Add((row, col));
            }
        }
    }

    return trailheads;
}

static IEnumerable<(int row, int col)> GetNextPositions(int[][] grid, (int row, int col) current)
{
    var directions = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
    var currentHeight = grid[current.row][current.col];

    List<(int row, int col)> positions = [];
    
    foreach (var (directionRow, directionCol) in directions)
    {
        var newRow = current.row + directionRow;
        var newCol = current.col + directionCol;
        
        if (newRow >= 0 && newRow < grid.Length &&
            newCol >= 0 && newCol < grid[0].Length &&
            grid[newRow][newCol] == currentHeight + 1)
        {
            positions.Add((newRow, newCol));
        }
    }

    return positions;
}

static int CalculateTrailheadScore(int[][] grid, (int row, int col) start)
{
    var visited = new HashSet<(int row, int col)>();
    var reachableNines = new HashSet<(int row, int col)>();
    var queue = new Queue<(int row, int col)>();
    
    queue.Enqueue(start);
    visited.Add(start);
    
    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        
        if (grid[current.row][current.col] == 9)
        {
            reachableNines.Add(current);
            continue;
        }
        
        foreach (var next in GetNextPositions(grid, current))
        {
            if (!visited.Add(next)) continue;
            queue.Enqueue(next);
        }
    }
    
    return reachableNines.Count;
}

static int SolvePartOne(int[][] grid)
{
    return FindTrailheads(grid)
        .Select(trailhead => CalculateTrailheadScore(grid, trailhead))
        .Sum();
}

static long CalculatePathsToNine(int[][] grid, (int row, int col) start)
{
    var memo = new Dictionary<(int row, int col), long>();
    var positionsByHeight = new List<(int row, int col)>[10];
    
    for (var i = 0; i < 10; i++)
        positionsByHeight[i] = [];
    
    for (var row = 0; row < grid.Length; row++)
    {
        for (var col = 0; col < grid[0].Length; col++)
        {
            positionsByHeight[grid[row][col]].Add((row, col));
        }
    }
    
    for (var height = 9; height >= grid[start.row][start.col]; height--)
    {
        foreach (var pos in positionsByHeight[height])
        {
            if (height == 9)
            {
                memo[pos] = 1;
                continue;
            }
            
            long totalPaths = 0;
            foreach (var next in GetNextPositions(grid, pos))
            {
                if (memo.TryGetValue(next, out var value))
                {
                    totalPaths += value;
                }
            }
            memo[pos] = totalPaths;
        }
    }

    return memo.GetValueOrDefault(start, 0);
}

static long CalculateTrailheadRating(int[][] grid, (int row, int col) start)
{
    return CalculatePathsToNine(grid, start);
}

static long SolvePartTwo(int[][] grid)
{
    return FindTrailheads(grid)
        .Select(trailhead => CalculateTrailheadRating(grid, trailhead))
        .Sum();
}