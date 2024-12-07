using static Utils.Utils;

const string input = "input.txt";
var lines = File.ReadAllLines(input);
ExecuteAndMeasure("First part", () => ProcessLines(lines));
ExecuteAndMeasure("Second part", () => ProcessLines(lines, withConcatenation: true));
return 0;

static long ProcessLines(string[] lines, bool withConcatenation = false)
{
    List<long> validTestValues = [];
    var index = 0;
    for (; index < lines.Length; index++)
    {
        var line = lines[index];
        var parts = line.Split(":");
        var testValue = long.Parse(parts[0]);
        var numbers = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        if (CanMakeTestValueFromNumbers(testValue, numbers, withConcatenation))
        {
            validTestValues.Add(testValue);
        }
    }

    return validTestValues.Sum();
}

static bool CanMakeTestValueFromNumbers(long testValue, long[] numbers, bool withConcatenation = false)
{
    var possibleValues = new HashSet<long>{ numbers[0] };

    int i;
    for (i = 1; i < numbers.Length; i++)
    {
        var actualNumber = numbers[i];

        HashSet<long> newValuesAfterEvaluation = [];
        foreach (var possibleValue in possibleValues)
        {
            newValuesAfterEvaluation.Add(possibleValue + actualNumber);
            newValuesAfterEvaluation.Add(possibleValue * actualNumber);

            if (withConcatenation)
            {
                newValuesAfterEvaluation.Add(Concatenate(possibleValue, actualNumber));
            }
        }
        possibleValues = newValuesAfterEvaluation;
    }
    
    return possibleValues.Contains(testValue);
}

static long Concatenate(long a, long b)
{
    if (b == 0) return a * 10;
    
    var temp = b;
    long multiplier = 10;
    while (temp >= 10)
    {
        temp /= 10;
        multiplier *= 10;
    }
    
    return a * multiplier + b;
}
