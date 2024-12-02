// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

ExecuteAndMeasure("First part", () => CountSafeReports("input.txt"));
ExecuteAndMeasure("Second part", () => CountSafeReportsWithOneRemoval("input.txt"));
return;

static void ExecuteAndMeasure(string description, Func<int> operation)
{
    var stopwatch = Stopwatch.StartNew();
    var result = operation();
    stopwatch.Stop();
    Console.WriteLine($"{description} result is: {result}");
    Console.WriteLine($"{description} execution time: {stopwatch.ElapsedMilliseconds}ms");
}

static int CountSafeReports(string filePath)
{
    var safeCount = 0;
        
    foreach (var line in File.ReadLines(filePath))
    {
        if (IsSafeReport(line))
        {
            safeCount++;
        }
    }
        
    return safeCount;
}

static bool IsSafeReport(string line)
{
    if (string.IsNullOrWhiteSpace(line))
        return false;
            
    var levels = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();
            
    return levels.Length < 2 || IsSafeSequence(levels);
}

static bool IsSafeSequence(int[] levels)
{
    if (levels.Length < 2)
        return true;
            
    int diff = levels[1] - levels[0];
    bool isIncreasing = diff > 0;
        
    for (int i = 1; i < levels.Length; i++)
    {
        int currentDiff = levels[i] - levels[i - 1];
            
        if (Math.Abs(currentDiff) < 1 || Math.Abs(currentDiff) > 3)
            return false;
                
        if ((isIncreasing && currentDiff <= 0) || (!isIncreasing && currentDiff >= 0))
            return false;
    }
        
    return true;
}

static bool IsSafeWithDampener(string line)
{
    var levels = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();
    
    if (IsSafeSequence(levels))
        return true;
    
    for (var i = 0; i < levels.Length; i++)
    {
        var newSequence = new int[levels.Length - 1];
        Array.Copy(levels, 0, newSequence, 0, i);
        Array.Copy(levels, i + 1, newSequence, i, levels.Length - i - 1);
            
        if (IsSafeSequence(newSequence))
            return true;
    }
        
    return false;
}

static int CountSafeReportsWithOneRemoval(string filePath)
{
    var safeCount = 0;
        
    foreach (var line in File.ReadLines(filePath))
    {
        if (IsSafeWithDampener(line))
        {
            safeCount++;
        }
    }
        
    return safeCount;
}








 