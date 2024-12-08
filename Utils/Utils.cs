using System.Diagnostics;

namespace Utils;

public static class Utils
{
    public static T ExecuteAndMeasure<T>(string description, Func<T> operation)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = operation();
        stopwatch.Stop();
        Console.WriteLine($"{description} result is: {result}");
        Console.WriteLine($"{description} execution time: {stopwatch.ElapsedMilliseconds}ms");
        return result;
    }

    public static string[] ReadInput(string path) => File.ReadAllLines(path)
                                                         .Where(line => !string.IsNullOrEmpty(line))
                                                         .ToArray();
}