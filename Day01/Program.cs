using System.Diagnostics;
using Day01;

var lines = File.ReadAllLines("input.txt");

ExecuteAndMeasure("First part", () => CalculateDistances(lines));
ExecuteAndMeasure("Second part", () => CalculateSimilarityScore(lines));
return;

static void ExecuteAndMeasure(string description, Func<int> operation)
{
    var stopwatch = Stopwatch.StartNew();
    var result = operation();
    stopwatch.Stop();
    Console.WriteLine($"{description} result is: {result}");
    Console.WriteLine($"{description} execution time: {stopwatch.ElapsedMilliseconds}ms");
}

static ParsedData ReadAndParseInput(string[] lines)
{
    var firstColumn = new Dictionary<int, int>();
    var secondColumn = new Dictionary<int, int>();
    var minVal = int.MaxValue;
    var maxVal = int.MinValue;

    foreach (var line in lines)
    {
        var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
            
        firstColumn[values[0]] = firstColumn.GetValueOrDefault(values[0], 0) + 1;
        secondColumn[values[1]] = secondColumn.GetValueOrDefault(values[1], 0) + 1;
        minVal = Math.Min(minVal, Math.Min(values[0], values[1]));
        maxVal = Math.Max(maxVal, Math.Max(values[0], values[1]));
    }

    return new ParsedData(firstColumn, secondColumn, minVal, maxVal, lines.Length);
}

static int CalculateDistances(string[] lines)
{ 
    var parsedData = ReadAndParseInput(lines);
    
    var totalDistance = 0;
    var remainingPairs = parsedData.TotalPairs;
    var currentNum = parsedData.MinValue;

    for (var current = parsedData.MinValue; current <= parsedData.MaxValue && remainingPairs > 0; current++)
    {
        var matchedPairs = MatchSameValuePairs(parsedData.FirstColumn, parsedData.SecondColumn, current);
        remainingPairs -= matchedPairs;
        
        remainingPairs -= ProcessRemainingPairs(parsedData.FirstColumn, parsedData.SecondColumn, current, parsedData.MaxValue, ref totalDistance);
        remainingPairs -= ProcessRemainingPairs(parsedData.SecondColumn, parsedData.FirstColumn, current, parsedData.MaxValue, ref totalDistance);
    }
    
    return totalDistance;
}

static int MatchSameValuePairs(Dictionary<int, int> first, Dictionary<int, int> second, int value)
{
    var firstCount = first.GetValueOrDefault(value);
    var secondCount = second.GetValueOrDefault(value);
    var pairs = Math.Min(firstCount, secondCount);

    if (pairs <= 0) return pairs;
    
    first[value] -= pairs;
    second[value] -= pairs;

    return pairs;
}

static int ProcessRemainingPairs(
    Dictionary<int, int> sourceColumn,
    Dictionary<int, int> targetColumn,
    int currentValue,
    int maxValue,
    ref int totalDistance)
{
    var processedPairs = 0;
    var sourceCount = sourceColumn.GetValueOrDefault(currentValue);
        
    if (sourceCount <= 0) return 0;

    for (var targetValue = currentValue + 1; targetValue <= maxValue; targetValue++)
    {
        var targetCount = targetColumn.GetValueOrDefault(targetValue);
        if (targetCount <= 0) continue;

        var pairs = Math.Min(sourceCount, targetCount);
        totalDistance += Math.Abs(currentValue - targetValue) * pairs;
            
        sourceColumn[currentValue] -= pairs;
        targetColumn[targetValue] -= pairs;
        processedPairs += pairs;
        sourceCount -= pairs;

        if (sourceCount <= 0) break;
    }

    return processedPairs;
}

static int CalculateSimilarityScore(string[] lines)
{ 
    var parsedData = ReadAndParseInput(lines);
    
    var similarityScore = 0;
    var currentNum = parsedData.MinValue;

    while (currentNum <= parsedData.MaxValue)
    {
        var firstColumnValueCount = parsedData.FirstColumn.GetValueOrDefault(currentNum, 0);
        var secondColumnValueCount = parsedData.SecondColumn.GetValueOrDefault(currentNum, 0);

        if (firstColumnValueCount > 0 && secondColumnValueCount > 0)
        {
            similarityScore += secondColumnValueCount * firstColumnValueCount * currentNum;
        }
        
        currentNum++;
    }
    
    return similarityScore;
}