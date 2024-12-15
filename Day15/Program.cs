using static Utils.Utils;

const char box = 'O';
const char wall = '#';
const char empty = '.';
const char boxLeft = '[';
const char boxRight = ']';

ExecuteAndMeasure("First part", SolvePartOne);
ExecuteAndMeasure("Second part", SolvePartTwo);

return 0;

static long SolvePartOne()
{
    var (grid, movements) = ParseInputFile("input.txt");
    ProcessMovements(grid, movements);
    var total = CalculateBoxGpsCoordinatesSum(grid);
    
    return total;
}

static (char[][] grid, char[] movements) ParseInputFile(string filePath)
{
    var grid = new List<char[]>();
    char[] movements = [];
    var isReadingMovements = false;

    foreach (var line in File.ReadLines(filePath))
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            isReadingMovements = true;
            continue;
        }

        if (!isReadingMovements)
        {
            grid.Add(line.ToCharArray());
        }
        else
        {
            movements = [..movements, ..line.ToCharArray()];
        }
    }

    return (grid.ToArray(), movements);
}

static void ProcessMovements(char[][] grid, char[] movements)
{
    var robotPosition = GetRobotInitialPosition(grid);
    foreach (var movement in movements)
    {
        var newPosition = MoveRobot(grid, robotPosition, movement);
        if (newPosition == robotPosition) continue;
        grid[robotPosition.X][robotPosition.Y] = empty;
        grid[newPosition.X][newPosition.Y] = '@';
        robotPosition = newPosition;
    }
}

static Point MoveRobot(char[][] grid, Point robotPosition, char movement)
{
    var newPos = movement switch
    {
        Movement.Up => robotPosition with { X = robotPosition.X - 1 },
        Movement.Down => robotPosition with { X = robotPosition.X + 1 },
        Movement.Left => robotPosition with { Y = robotPosition.Y - 1 },
        Movement.Right => robotPosition with { Y = robotPosition.Y + 1 },
        _ => robotPosition
    };
    
    if (newPos.X < 0 || newPos.X >= grid.Length || 
        newPos.Y < 0 || newPos.Y >= grid[0].Length)
        return robotPosition;
    
    switch (grid[newPos.X][newPos.Y])
    {
        case wall:
            return robotPosition;
        case empty:
            return newPos;
        case box:
        {
            var boxChain = new List<Point>();
            var checkPos = newPos;
        
            while (true)
            {
                if (checkPos.X < 0 || checkPos.X >= grid.Length || 
                    checkPos.Y < 0 || checkPos.Y >= grid[0].Length)
                    return robotPosition;

                if (grid[checkPos.X][checkPos.Y] == wall)
                    return robotPosition;

                if (grid[checkPos.X][checkPos.Y] == empty)
                    break;

                if (grid[checkPos.X][checkPos.Y] == box)
                    boxChain.Add(checkPos);

                checkPos = movement switch
                {
                    Movement.Up => checkPos with { X = checkPos.X - 1 },
                    Movement.Down => checkPos with { X = checkPos.X + 1 },
                    Movement.Left => checkPos with { Y = checkPos.Y - 1 },
                    Movement.Right => checkPos with { Y = checkPos.Y + 1 },
                    _ => checkPos
                };
            }
        
            for (var i = boxChain.Count - 1; i >= 0; i--)
            {
                var currentBox = boxChain[i];
                var targetPos = movement switch
                {
                    Movement.Up => currentBox with { X = currentBox.X - 1 },
                    Movement.Down => currentBox with { X = currentBox.X + 1 },
                    Movement.Left => currentBox with { Y = currentBox.Y - 1 },
                    Movement.Right => currentBox with { Y = currentBox.Y + 1 },
                    _ => currentBox
                };

                grid[targetPos.X][targetPos.Y] = box;
                grid[currentBox.X][currentBox.Y] = empty;
            }

            return newPos;
        }
        default:
            return robotPosition;
    }
}

static Point GetRobotInitialPosition(char[][] grid)
{
    for (var i = 0; i < grid.Length; i++)
    {
        for (var j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == '@')
            {
                return new Point(i, j);
            } 
        }
    }
    
    return new Point(0, 0);
}

static long CalculateBoxGpsCoordinatesSum(char[][] grid)
{
    long sum = 0;
    
    for (var row = 0; row < grid.Length; row++)
    {
        for (var col = 0; col < grid[row].Length; col++)
        {
            if (grid[row][col] != box) continue;
            
            var distanceFromTop = row;
            var distanceFromLeft = col;
            var gpsCoordinate = (100 * distanceFromTop) + distanceFromLeft;
            sum += gpsCoordinate;
        }
    }
    
    return sum;
}

static long SolvePartTwo()
{
    var (grid, movements) = ParseInputFileForPartTwo("input.txt");
    ProcessMovementsPartTwo(grid, movements);
    var total = CalculateBoxGpsCoordinatesPartTwo(grid);
    return total;
}

static (char[][] grid, char[] movements) ParseInputFileForPartTwo(string filePath)
{
    var (originalGrid, movements) = ParseInputFile(filePath);
    var n = originalGrid.Length;
    var m = originalGrid[0].Length * 2;
    
    var grid = new char[n][];
    for (var i = 0; i < n; i++)
    {
        grid[i] = new char[m];
        var j = 0;
        foreach (var c in originalGrid[i])
        {
            switch (c)
            {
                case wall:
                    grid[i][j++] = '#';
                    grid[i][j++] = '#';
                    break;
                case box:
                    grid[i][j++] = '[';
                    grid[i][j++] = ']';
                    break;
                case empty:
                    grid[i][j++] = '.';
                    grid[i][j++] = '.';
                    break;
                default: // '@'
                    grid[i][j++] = '@';
                    grid[i][j++] = '.';
                    break;
            }
        }
    }
    
    return (grid, movements);
}

static void ProcessMovementsPartTwo(char[][] grid, char[] movements)
{
    var robotPosition = GetRobotInitialPosition(grid);
    int[] dx = [0, -1, 0, 1];
    int[] dy = [-1, 0, 1, 0];

    foreach (var movement in movements)
    {
        var d = movement switch
        {
            Movement.Up => 1,
            Movement.Left => 0,
            Movement.Right => 2,
            Movement.Down => 3,
            _ => throw new ArgumentException("Invalid direction")
        };

        if (!TryPush(grid, robotPosition.X, robotPosition.Y, dx[d], dy[d])) continue;
        
        Push(grid, robotPosition.X, robotPosition.Y, dx[d], dy[d]);
        robotPosition = new Point(X: robotPosition.X + dx[d], Y: robotPosition.Y + dy[d]);
    }
}

static long CalculateBoxGpsCoordinatesPartTwo(char[][] grid)
{
    var sum = 0;
    for (var i = 0; i < grid.Length; i++)
    {
        for (var j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == '[')
            {
                sum += 100 * i + j;
            }
        }
    }
    return sum;
}

static bool TryPush(char[][] a, int sx, int sy, int dx, int dy)
{
    var nx = sx + dx;
    var ny = sy + dy;

    switch (a[nx][ny])
    {
        case '#':
            return false;
        case '.':
            return true;
        default:
        {
            switch (dy)
            {
                case 0 when a[nx][ny] == ']':
                    return TryPush(a, nx, ny, dx, dy) && TryPush(a, nx, ny - 1, dx, dy);
                case 0 when a[nx][ny] == '[':
                    return TryPush(a, nx, ny, dx, dy) && TryPush(a, nx, ny + 1, dx, dy);
                case -1 when a[nx][ny] == ']':
                    return TryPush(a, nx, ny - 1, dx, dy);
                case 1 when a[nx][ny] == '[':
                    return TryPush(a, nx, ny + 1, dx, dy);
            }

            break;
        }
    }

    return false;
}

static void Push(char[][] a, int sx, int sy, int dx, int dy)
{
    var nx = sx + dx;
    var ny = sy + dy;

    switch (a[nx][ny])
    {
        case '#':
            return;
        case '.':
            (a[sx][sy], a[nx][ny]) = (a[nx][ny], a[sx][sy]);
            return;
        default:
        {
            switch (dy)
            {
                case 0 when a[nx][ny] == ']':
                    Push(a, nx, ny, dx, dy);
                    Push(a, nx, ny - 1, dx, dy);
                    (a[sx][sy], a[nx][ny]) = (a[nx][ny], a[sx][sy]);
                    return;
                case 0 when a[nx][ny] == '[':
                    Push(a, nx, ny, dx, dy);
                    Push(a, nx, ny + 1, dx, dy);
                    (a[sx][sy], a[nx][ny]) = (a[nx][ny], a[sx][sy]);
                    return;
                case -1 when a[nx][ny] == ']':
                    Push(a, nx, ny - 1, dx, dy);
                    (a[nx][ny - 1], a[nx][ny], a[sx][sy]) = (a[nx][ny], a[sx][sy], a[nx][ny - 1]);
                    return;
                case 1 when a[nx][ny] == '[':
                    Push(a, nx, ny + 1, dx, dy);
                    (a[nx][ny + 1], a[nx][ny], a[sx][sy]) = (a[nx][ny], a[sx][sy], a[nx][ny + 1]);
                    return;
            }

            break;
        }
    }
}

internal static class Movement
{
    internal const char Up = '^';
    internal const char Down = 'v';
    internal const char Left = '<';
    internal const char Right = '>';
}

internal record struct Point(int X, int Y);