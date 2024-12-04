using static Utils.Utils;

var input = ReadInput("input.txt");
ExecuteAndMeasure("First part", () => Part1(input));
ExecuteAndMeasure("Second part", () => Part2(input));
return;

static int Part1(string[] grid)
{
    var count = 0;
    var rows = grid.Length;
    var cols = grid[0].Length;
    var directions = new (int dr, int dc)[] 
    { 
        (0,1), (1,0), (1,1), (-1,1), (0,-1), (-1,0), (-1,-1), (1,-1) 
    };

    for (var i = 0; i < rows; i++)
    {
        for (var j = 0; j < cols; j++)
        {
            if (grid[i][j] != 'X') continue;

            foreach (var (dr, dc) in directions)
            {
                if (IsValidXmas(grid, i, j, dr, dc, rows, cols)) count++;
            }
        }
    }
    return count;
}

static bool IsValidXmas(string[] grid, int row, int col, int dr, int dc, int rows, int cols)
{
    const string target = "XMAS";
    for (var i = 0; i < target.Length; i++)
    {
        var newRow = row + (i * dr);
        var newCol = col + (i * dc);
        
        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols 
            || grid[newRow][newCol] != target[i]) return false;
    }
    return true;
}

static int Part2(string[] grid)
{
    var count = 0;
    var rows = grid.Length;
    var cols = grid[0].Length;

    for (var row = 1; row < rows - 1; row++)
    {
        for (var col = 1; col < cols - 1; col++)
        {
            if (grid[row][col] != 'A') continue;

            var upLeftM = grid[row - 1][col - 1] == 'M';
            var upLeftS = grid[row - 1][col - 1] == 'S';
            var downRightM = grid[row + 1][col + 1] == 'M';
            var downRightS = grid[row + 1][col + 1] == 'S';
            var upRightM = grid[row - 1][col + 1] == 'M';
            var upRightS = grid[row - 1][col + 1] == 'S';
            var downLeftM = grid[row + 1][col - 1] == 'M';
            var downLeftS = grid[row + 1][col - 1] == 'S';

            if (((upLeftM && downRightS) || (upLeftS && downRightM)) 
                && ((upRightM && downLeftS) || (upRightS && downLeftM)))
            {
                count++;
            }
        }
    }
    return count;
}
