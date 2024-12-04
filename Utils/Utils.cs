using System.Diagnostics;

namespace Utils;

public static class Utils
{
    public static void ExecuteAndMeasure(string description, Func<int> operation)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = operation();
        stopwatch.Stop();
        Console.WriteLine($"{description} result is: {result}");
        Console.WriteLine($"{description} execution time: {stopwatch.ElapsedMilliseconds}ms");
    } 
    
    public static string[] ReadInput(string path) => File.ReadAllLines(path);
}