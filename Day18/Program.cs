using static Utils.Utils;

const int gridSize = 71;
(int, int)[] directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];

var coordinates = File.ReadLines("input.txt")
    .Select(line =>
    {
        var parts = line.Trim().Split(',');
        return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
    })
    .ToArray();

ExecuteAndMeasure("First part", () => SolvePartOne(coordinates, directions));
ExecuteAndMeasure("Second part", () => SolvePartTwo(coordinates));

return 0;

static int SolvePartOne(Point[] coordinates, (int dx, int dy)[] dirs)
{
    var obstacles = coordinates.Take(1024).ToHashSet();
    var distances = new Dictionary<Point, int>();
    var pq = new PriorityQueue<Point, int>();
    
    var start = new Point(0, 0);
    var end = new Point(gridSize - 1, gridSize - 1);
    
    pq.Enqueue(start, 0);
    distances[start] = 0;
    
    while (pq.Count > 0)
    {
        var current = pq.Dequeue();
        var currentDist = distances[current];
        
        if (current == end)
            return currentDist;
        
        foreach (var neighbor in GetNeighbors(current, gridSize, dirs))
        {
            if (obstacles.Contains(neighbor))
                continue;
                
            var newDist = currentDist + 1;

            if (distances.ContainsKey(neighbor) && newDist >= distances[neighbor]) continue;
            
            distances[neighbor] = newDist;
            pq.Enqueue(neighbor, newDist);
        }
    }
    
    return -1;
}

static string SolvePartTwo(Point[] coordinates)
{
    var grid = new bool[gridSize, gridSize];
    var visited = new bool[gridSize, gridSize];
    var queue = new Queue<(int x, int y)>();
    
    var left = 0;
    var right = coordinates.Length - 1;
    
    while (left < right)
    {
        var mid = left + (right - left) / 2;
        Array.Clear(grid, 0, grid.Length);
        
        AddCoordinatesToGrid(coordinates, grid, 0, mid, gridSize);
        
        if (CheckPath(grid, visited, queue, gridSize))
        {
            left = mid + 1;
        }
        else
        {
            right = mid;
        }
    }
        
    return $"{coordinates[left].X},{coordinates[left].Y}";
}

static Point[] GetNeighbors(Point point, int size, (int dx, int dy)[] dirs)
{
    return dirs
        .Select(d => new Point(point.X + d.dx, point.Y + d.dy))
        .Where(p => p.X >= 0 && p.X < size && p.Y >= 0 && p.Y < size)
        .ToArray();
}

static bool IsStartOrEndPoint(int x, int y, int size)
{
    return (x == 0 && y == 0) || (x == size - 1 && y == size - 1);
}

static bool CheckPath(bool[,] grid, bool[,] visited, Queue<(int x, int y)> queue, int size)
{
    Array.Clear(visited, 0, visited.Length);
    queue.Clear();
    
    queue.Enqueue((0, 0));
    visited[0, 0] = true;
    
    while (queue.Count > 0)
    {
        var (x, y) = queue.Dequeue();
        
        if (x == size - 1 && y == size - 1)
            return true;
        
        if (x + 1 < size && !grid[x + 1, y] && !visited[x + 1, y])
        {
            queue.Enqueue((x + 1, y));
            visited[x + 1, y] = true;
        }
        if (y + 1 < size && !grid[x, y + 1] && !visited[x, y + 1])
        {
            queue.Enqueue((x, y + 1));
            visited[x, y + 1] = true;
        }
        if (x - 1 >= 0 && !grid[x - 1, y] && !visited[x - 1, y])
        {
            queue.Enqueue((x - 1, y));
            visited[x - 1, y] = true;
        }
        if (y - 1 >= 0 && !grid[x, y - 1] && !visited[x, y - 1])
        {
            queue.Enqueue((x, y - 1));
            visited[x, y - 1] = true;
        }
    }
    
    return false;
}

static void AddCoordinatesToGrid(Point[] coordinates, bool[,] grid, int start, int end, int size)
{
    for (var i = start; i <= end; i++)
    {
        var coord = coordinates[i];
        if (!IsStartOrEndPoint(coord.X, coord.Y, size))
        {
            grid[coord.X, coord.Y] = true;
        }
    }
}

internal readonly record struct Point(int X, int Y);