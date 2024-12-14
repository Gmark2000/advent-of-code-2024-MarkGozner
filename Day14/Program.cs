using static Utils.Utils;
using System.Text;
using SkiaSharp;

const int width = 101;
const int height = 103;

const string imagesFolder = "grid_images";

const int startTime = 0;
const int endTime = 10000;

const int gridScale = 4;
const int imageWidth = width * gridScale;
const int imageHeight = height * gridScale;


var lines = ReadInput("input.txt");
var robots = ParseInput(lines);

ExecuteAndMeasure("First part", () => SolvePartOne(robots));
ExecuteAndMeasure("Second part", 
    () => SolvePartTwo(robots), 
    "Second part (with saving all the grids as an image)");

return 0;

static List<Robot> ParseInput(string[] lines)
{
    List<Robot> robots = [];

    foreach (var line in lines)
    {
        var splitLine = line.Split("p=")[1].Split(" v=");
        var initialPositionSplit = splitLine[0].Split(",");
        var velocitySplit = splitLine[1].Split(",");
        
        robots.Add(new Robot()
        {
            InitialPosition = new Point(int.Parse(initialPositionSplit[0]), int.Parse(initialPositionSplit[1])),
            Velocity = new Point(int.Parse(velocitySplit[0]), int.Parse(velocitySplit[1]))
        });
    }
    
    return robots;
}

static int SolvePartOne(List<Robot> robots)
{
    int[] quadrants = [0, 0, 0, 0];
    
    foreach (var robot in robots)
    {
        var futurePosition = CalculateFuturePosition(robot.InitialPosition, robot.Velocity, 100);
        var quadrant = CheckQuadrant(futurePosition);
        
        if (quadrant == -1) continue;
        
        quadrants[quadrant] += 1;
    }
    
    var safetyFactor = quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3];
    return safetyFactor;
}

static Point CalculateFuturePosition(Point initial, Point velocity, int seconds)
{
    var totalX = initial.X + (long)(seconds * velocity.X);
    var totalY = initial.Y + (long)(seconds * velocity.Y);
    
    var wrappedX = (int)((totalX % width + width) % width);
    var wrappedY = (int)((totalY % height + height) % height);
    
    return new Point(wrappedX, wrappedY);
}

static int CheckQuadrant(Point position)
{
    const int midX = width / 2;
    const int midY = height / 2;

    if (position.X == midX || position.Y == midY)
        return -1;

    return position.X switch
    {
        < midX when position.Y < midY => 0,
        > midX when position.Y < midY => 1,
        < midX when position.Y > midY => 2,
        > midX when position.Y > midY => 3,
        _ => -1
    };
}

static int SolvePartTwo(List<Robot> robots)
{
    var gridImages = Path.Combine(Directory.GetCurrentDirectory(), imagesFolder);
    Directory.CreateDirectory(gridImages);
    
    var minFileSize = long.MaxValue;
    var minFileSizeTime = -1;  
    bool[,] smallestGrid = null;  
    
    for (var seconds = startTime; seconds < endTime; seconds++)
    {
        var positions = GetAllPositionsAtTime(robots, seconds);
        var grid = CreateGrid(positions);
        var fileSize = SaveGridAsImage(grid, seconds);
        
        if (fileSize > 0 && fileSize < minFileSize)
        {
            minFileSize = fileSize;
            minFileSizeTime = seconds;
            smallestGrid = grid;
        }
        
        if (seconds % 1000 == 0)
        {
            Console.WriteLine($"Processed time: {seconds} seconds");
        }
    }
    
    Console.WriteLine($"\nChristmas tree found at time: {minFileSizeTime}");
    Console.WriteLine($"Pattern file size: {minFileSize} bytes");
    Console.WriteLine("Pattern grid:");
    PrintGrid(smallestGrid!);
    
    return minFileSizeTime;
}

static void PrintGrid(bool[,] grid)
{
    var sb = new StringBuilder();
    sb.AppendLine();
    
    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            sb.Append(grid[y, x] ? '#' : '.');
        }
        sb.AppendLine();
    }
    
    Console.WriteLine(sb.ToString());
}

static long SaveGridAsImage(bool[,] grid, int seconds)
{
    using var surface = SKSurface.Create(new SKImageInfo(imageWidth, imageHeight));
    using var canvas = surface.Canvas;
    
    canvas.Clear(SKColors.Black);
    
    using var paint = new SKPaint();
    paint.Color = SKColors.White;
    paint.Style = SKPaintStyle.Fill;

    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            if (grid[y, x])
            {
                canvas.DrawRect(x * gridScale, y * gridScale, gridScale, gridScale, paint);
            }
        }
    }
    
    using var image = surface.Snapshot();
    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
    var filename = $"{seconds}.png";
    
    var fullPath = Path.Combine(
        Directory.GetCurrentDirectory(),
        imagesFolder,
        filename
    );
    
    try
    {
        using var stream = File.OpenWrite(fullPath);
        data.SaveTo(stream);
        return data.Size;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving image at time {seconds}: {ex.Message}");
        return 0;
    }
}

static bool[,] CreateGrid(List<Point> positions)
{
    var grid = new bool[height, width];
    
    foreach (var pos in positions)
    {
        grid[pos.Y, pos.X] = true;
    }
    
    return grid;
}

static List<Point> GetAllPositionsAtTime(List<Robot> robots, int seconds)
{
    return robots.Select(robot => CalculateFuturePosition(robot.InitialPosition, robot.Velocity, seconds)).ToList();
}

internal class Robot
{
    public Point InitialPosition { get; init; }
    public Point Velocity { get; init; }
}

internal record struct Point(int X, int Y);