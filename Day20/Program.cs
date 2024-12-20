using static Utils.Utils;

const char startChar = 'S';
const char endChar = 'E';
const char trackChar = '.';

(int dx, int dy)[] directions = [(dx: 0, dy: 1), (dx: 1, dy: 0), (dx: 0, dy: -1), (dx: -1, dy: 0)];
var grid = ReadInput("input.txt");
var (path, start, end) = FindPath(grid);

ExecuteAndMeasure("First part", () => SolvePartOne(start, grid, path, directions));
ExecuteAndMeasure("Second part", () => SolvePartTwo(start, grid, path, directions));
return;

static (List<Point> path, Point start, Point end) FindPath(string[] grid)
{
    Point start = default, end = default;
    for (var i = 0; i < grid.Length; i++)
        for (var j = 0; j < grid[0].Length; j++)
        {
            if (grid[i][j] == startChar) start = new Point(i, j);
            if (grid[i][j] == endChar) end = new Point(i, j);
        }

    var path = new List<Point> { start };
    while (path.Last() != end)
    {
        var curr = path.Last();
        path.Add(GetNextPoint(curr, grid, path));
    }
    return (path, start, end);
}

static Point GetNextPoint(Point p, string[] grid, List<Point> visited)
{
    foreach (var (dx, dy) in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) })
    {
        var next = new Point(p.X + dx, p.Y + dy);
        if (IsValidMove(next, grid) && !visited.Contains(next))
            return next;
    }
    return p;
}

static int SolvePartOne(Point start, string[] grid, List<Point> path, (int dx, int dy)[] dirs)
{
    var shortcuts = new HashSet<(int from, int to)>();
    var positions = path.Select((p, i) => (p, i)).ToDictionary(x => x.p, x => x.i);
    var startIndex = positions[start];

    for (var i = startIndex; i < path.Count - 102; i++)
    {
        foreach (var end in GetReachablePoints(path[i], grid, dirs))
        {
            if (!positions.TryGetValue(end, out var j)) continue;
            var saved = j - i - 2;
            if (saved >= 100)
                shortcuts.Add((i, j));
        }
    }
    return shortcuts.Count;
}

static IEnumerable<Point> GetReachablePoints(Point start, string[] grid, (int dx, int dy)[] dirs)
{
    var visited = new HashSet<Point> { start };
    var points = new List<Point>();
    var queue = new Queue<(Point p, int steps)>();
    queue.Enqueue((start, 0));
    
    while (queue.Count > 0)
    {
        var (curr, steps) = queue.Dequeue();
        switch (steps)
        {
            case 2 when HasPathNearby(curr, grid, dirs):
                points.Add(curr);
                continue;
            case < 2:
            {
                foreach (var (dx, dy) in dirs)
                {
                    var next = new Point(curr.X + dx, curr.Y + dy);
                    if (IsInBounds(next, grid) && visited.Add(next))
                    {
                        queue.Enqueue((next, steps + 1));
                    }
                }
                break;
            }
        }
    }
    
    return points;
}

static int SolvePartTwo(Point start, string[] grid, List<Point> path, (int dx, int dy)[] dirs)
{
    var shortcuts = new HashSet<(int from, int to)>();
    var positions = path.Select((p, i) => (p, i)).ToDictionary(x => x.p, x => x.i);
    var startIndex = positions[start];
    
    object lockObj = new();
    Parallel.For(startIndex, path.Count - 102, i =>
    {
        List<(int, int)> localShortcuts = [];
        var reachablePoints = GetReachablePointsPart2(path[i], grid, dirs);
        
        foreach (var (end, stepsUsed) in reachablePoints)
        {
            if (!positions.TryGetValue(end, out var j)) continue;
            var saved = j - i - stepsUsed;
            if (saved >= 100)
                localShortcuts.Add((i, j));
        }
        
        if (localShortcuts.Count <= 0) return;
        lock (lockObj)
        {
            foreach (var shortcut in localShortcuts)
                shortcuts.Add(shortcut);
        }
    });

    return shortcuts.Count;
}

static IEnumerable<(Point point, int steps)> GetReachablePointsPart2(Point start, string[] grid, (int dx, int dy)[] dirs)
{
    var visited = new HashSet<Point> { start };
    var queue = new Queue<(Point p, int steps)>();
    var points = new List<(Point point, int steps)>();
    queue.Enqueue((start, 0));
    
    while (queue.Count > 0)
    {
        var (curr, steps) = queue.Dequeue();
        
        if (steps is > 0 and <= 20 && HasPathNearby(curr, grid, dirs))
        {
            points.Add((curr, steps));
        }

        if (steps >= 20) continue;
        
        foreach (var (dx, dy) in dirs)
        {
            var next = new Point(curr.X + dx, curr.Y + dy);
            if (IsInBounds(next, grid) && visited.Add(next))
            {
                queue.Enqueue((next, steps + 1));
            }
        }
    }

    return points;
}

static bool HasPathNearby(Point p, string[] grid, (int dx, int dy)[] dirs)
{
    return dirs.Any(d =>
    {
        var next = new Point(p.X + d.dx, p.Y + d.dy);
        return IsValidMove(next, grid);
    });
}

static bool IsInBounds(Point p, string[] grid) => 
    p.X >= 0 && p.X < grid.Length && p.Y >= 0 && p.Y < grid[0].Length;

static bool IsValidMove(Point p, string[] grid) => 
    IsInBounds(p, grid) && (grid[p.X][p.Y] == trackChar || grid[p.X][p.Y] == endChar);

internal readonly record struct Point(int X, int Y);
