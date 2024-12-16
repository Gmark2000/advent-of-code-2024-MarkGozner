using System.Diagnostics;

namespace Utils;

public static class Utils
{
    public static T ExecuteAndMeasure<T>(string description, Func<T> operation, string? executionTimeDescription = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = operation();
        stopwatch.Stop();
        
        Console.WriteLine($"{description} result is: {result}");
        Console.WriteLine($"{executionTimeDescription ?? description} execution time: {stopwatch.ElapsedMilliseconds}ms");
        return result;
    }
    
    public static (T1, T2) ExecuteAndMeasure<T1, T2>(
        string firstDescription,
        string secondDescription,
        Func<(T1, T2)> operation,
        string? executionTimeDescription = null)
    {
        var stopwatch = Stopwatch.StartNew();
        var (firstResult, secondResult) = operation();
        stopwatch.Stop();
    
        Console.WriteLine($"{firstDescription} result is: {firstResult}");
        Console.WriteLine($"{secondDescription} result is: {secondResult}");
        Console.WriteLine($"{executionTimeDescription ?? firstDescription} execution time: {stopwatch.ElapsedMilliseconds}ms");
    
        return (firstResult, secondResult);
    }

    public static string[] ReadInput(string path) => File.ReadAllLines(path)
                                                         .Where(line => !string.IsNullOrEmpty(line))
                                                         .ToArray();
}