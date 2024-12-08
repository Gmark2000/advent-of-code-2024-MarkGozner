using static Utils.Utils;

var grid = ReadInput("input.txt");

ExecuteAndMeasure("First part", () => SolvePartOne(grid));
ExecuteAndMeasure("Second part", () => SolvePartTwo(grid));

return 0;

static Dictionary<char, List<Point>> MapAntennas(string[] grid)
{
    var antennasByFrequency = new Dictionary<char, List<Point>>();
    
    for (var y = 0; y < grid.Length; y++)
    {
        for (var x = 0; x < grid[0].Length; x++)
        {
            var c = grid[y][x];
            if (c == '.') continue;
            
            if (!antennasByFrequency.TryGetValue(c, out var points))
            {
                points = [];
                antennasByFrequency[c] = points;
            }
            
            points.Add(new Point(x, y));
        }
    }
    
    return antennasByFrequency;
}

static int SolvePartOne(string[] grid)
{
    var antennasByFrequency = MapAntennas(grid);
    var antinodes = FindAntinodesWithRatio(antennasByFrequency, grid.Length, grid[0].Length);
    return antinodes.Count;
}

static int SolvePartTwo(string[] grid)
{
    var antennasByFrequency = MapAntennas(grid);
    var antinodes = FindAntinodesCollinear(antennasByFrequency, grid.Length, grid[0].Length);
    return antinodes.Count;
}

static HashSet<Point> FindAntinodesWithRatio(Dictionary<char, List<Point>> antennasByFrequency, int rows, int cols)
{
    var antinodes = new HashSet<Point>();
    
    foreach (var antennas in antennasByFrequency.Values.Where(a => a.Count >= 2))
    {
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                var point = new Point(x, y);
                
                for (var i = 0; i < antennas.Count - 1; i++)
                {
                    var a1 = antennas[i];
                    for (var j = i + 1; j < antennas.Count; j++)
                    {
                        var a2 = antennas[j];
                        
                        if (!AreCollinear(point, a1, a2)) continue;
                        if (point.Equals(a1) || point.Equals(a2)) continue;
                        
                        var d1 = Distance(point, a1);
                        var d2 = Distance(point, a2);
                        
                        if ((Math.Abs(d1 - 2 * d2) == 0) || (Math.Abs(d2 - 2 * d1) == 0))
                        {
                            antinodes.Add(point);
                        }
                    }
                }
            }
        }
    }
    
    return antinodes;
}

static HashSet<Point> FindAntinodesCollinear(Dictionary<char, List<Point>> antennasByFrequency, int rows, int cols)
{
    var antinodes = new HashSet<Point>();
    
    foreach (var antennas in antennasByFrequency.Values.Where(a => a.Count >= 2))
    {
        antinodes.UnionWith(antennas);
        
        var antennaPairs = new List<(Point, Point)>();
        for (var i = 0; i < antennas.Count - 1; i++)
        {
            for (var j = i + 1; j < antennas.Count; j++)
            {
                antennaPairs.Add((antennas[i], antennas[j]));
            }
        }
        
        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                var point = new Point(x, y);
                if (IsCollinearWithAnyPair(point, antennaPairs))
                {
                    antinodes.Add(point);
                }
            }
        }
    }
    
    return antinodes;
}

static bool IsCollinearWithAnyPair(Point point, List<(Point, Point)> antennaPairs)
{
    return antennaPairs.Any(pair => AreCollinear(point, pair.Item1, pair.Item2));
}

static bool AreCollinear(Point p1, Point p2, Point p3)
{
    var area = (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
    return area == 0;
}

static double Distance(Point p1, Point p2)
{
    var dx = p1.X - p2.X;
    var dy = p1.Y - p2.Y;
    return Math.Sqrt(dx * dx + dy * dy);
}