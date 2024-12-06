using static Utils.Utils;

var grid = File.ReadAllLines("input.txt");

ExecuteAndMeasure("First part", () => PartOne(grid));
ExecuteAndMeasure("Second part", () => PartTwo(grid));

return 0;

static int PartOne(string[] grid)
{
    var height = grid.Length;
    var width = grid[0].Length;
    
    var currentPos = FindStartPosition(grid);
    var direction = 0; 
    
    var visited = new HashSet<(int r, int c)>();
    int[] rowDirections = [-1, 0, 1, 0];
    int[] columnDirections = [0, 1, 0, -1];
    
    var willLeaveMap = false;
    
    while (!willLeaveMap)
    {
        visited.Add(currentPos);
        
        var hasObstacle = false;
        var tempPos = currentPos;
        
        while (true)
        {
            (int r, int c) nextPos = (
                tempPos.r + rowDirections[direction],
                tempPos.c + columnDirections[direction]
            );
            
            if (!IsInBounds(nextPos.r, nextPos.c, height, width))
            {
                if (!hasObstacle)
                {
                    willLeaveMap = true;
                }
                break;
            }
            
            if (grid[nextPos.r][nextPos.c] == '#')
            {
                hasObstacle = true;
                break;
            }
            
            tempPos = nextPos;
            visited.Add(tempPos);
        }
        
        if (willLeaveMap)
            break;
        
        currentPos = tempPos;
        direction = (direction + 1) % 4;
    }
    
    return visited.Count;
}

static int PartTwo(string[] grid)
{
    var height = grid.Length;
    var width = grid[0].Length;
    var startPos = FindStartPosition(grid);
    var possibleLoops = 0;

    for (var r = 0; r < height; r++)
    {
        for (var c = 0; c < width; c++)
        {
            if (grid[r][c] != '.' || (r, c) == startPos)
                continue;

            if (CheckForLoop(grid, startPos, (r, c)))
                possibleLoops++;
        }
    }

    return possibleLoops;
}

static bool CheckForLoop(string[] grid, (int r, int c) startPos, (int r, int c) obstaclePos)
{
    var height = grid.Length;
    var width = grid[0].Length;
    var currentPos = startPos;
    var direction = 0;

    var visited = new HashSet<(int r, int c, int dir)>();
    int[] dr = [-1, 0, 1, 0];
    int[] dc = [0, 1, 0, -1];

    while (true)
    {
        if (!visited.Add((currentPos.r, currentPos.c, direction)))
            return true;

        var tempPos = currentPos;
        while (true)
        {
            var nextPos = (
                r: tempPos.r + dr[direction],
                c: tempPos.c + dc[direction]
            );
            
            if (!IsInBounds(nextPos.r, nextPos.c, height, width))
            {
                return false;
            }
            
            if (grid[nextPos.r][nextPos.c] == '#' || nextPos == obstaclePos)
            {
                break;
            }

            tempPos = nextPos;
        }

        currentPos = tempPos;
        direction = (direction + 1) % 4;
    }
}


static (int r, int c) FindStartPosition(string[] grid)
{
    for (var r = 0; r < grid.Length; r++)
    for (var c = 0; c < grid[r].Length; c++)
        if (grid[r][c] == '^')
            return (r, c); 
                
    throw new Exception("No starting position found");
}

static bool IsInBounds(int row, int col, int height, int width)
{
    return row >= 0 && row < height && col >= 0 && col < width;
}