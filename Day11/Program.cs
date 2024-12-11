using Day11;
using static Utils.Utils;

var input = File.ReadAllText("input.txt");

ExecuteAndMeasure("First part", () => SolvePartOne(input, 25));
ExecuteAndMeasure("Second part", () => SolvePartTwo(input, 75));

return 0;

static int SolvePartOne(string input, int blinks)
{
    var stones = ParseInput(input);
    
    for (var i = 0; i < blinks; i++)
    {
        stones = TransformStones(stones);
    }
    
    return stones.Length;
}

static long SolvePartTwo(string input, int blinks)
{
    var stones = ParseInputPart2(input);
    
    for (var i = 0; i < blinks; i++)
    {
        stones = TransformStonesForPartTwo(stones);
    }
    
    return stones.Sum(stone => stone.Count);
}

static int CountDigits(long n)
{
    if (n == 0) return 1;
    var digits = 0;
    while (n != 0)
    {
        n /= 10;
        digits++;
    }
    return digits;
}
static long PowerOf10(int power)
{
    long result = 1;
    while (power > 0)
    {
        result *= 10;
        power--;
    }
    return result;
}

static (long left, long right) SplitNumber(long number, int digitCount)
{
    var halfDigits = digitCount / 2;
    var divisor = PowerOf10(halfDigits);
    return (number / divisor, number % divisor);
}

static long[] TransformStones(long[] stones)
{
    var result = new List<long>();
    
    foreach (var stone in stones)
    {
        if (stone == 0)
        {
            result.Add(1);
            continue;
        }

        var digitCount = CountDigits(stone);
        if (digitCount % 2 == 0)
        {
            var (left, right) = SplitNumber(stone, digitCount);
            result.Add(left);
            result.Add(right);
        }
        else
        {
            result.Add(stone * 2024);
        }
    }
    
    return result.ToArray();
}

static List<Stone> TransformStonesForPartTwo(List<Stone> stones)
{
    Dictionary<string, long> result = [];

    foreach (var stone in stones)
    {
        if (stone.Value == "0")
        {
            AddToDict(result, "1", stone.Count);
        }
        else
        {
            var trimmed = stone.Value.TrimStart('0');
            if (trimmed.Length % 2 == 0)
            {
                var mid = trimmed.Length / 2;
                
                var left = trimmed[..mid].TrimStart('0');
                if (string.IsNullOrEmpty(left)) left = "0";
                
                var right = trimmed[mid..].TrimStart('0');
                if (string.IsNullOrEmpty(right)) right = "0";
                
                AddToDict(result, left, stone.Count);
                AddToDict(result, right, stone.Count);
            }
            else
            {
                var product = MultiplyString(trimmed, "2024");
                AddToDict(result, product, stone.Count);
            }
        }
    }

    return result.Select(kvp => new Stone { Value = kvp.Key, Count = kvp.Value }).ToList();
}

static string MultiplyString(string number, string multiplier)
{
    var result = new int[number.Length + multiplier.Length];

    for (var i = number.Length - 1; i >= 0; i--)
    {
        for (var j = multiplier.Length - 1; j >= 0; j--)
        {
            var n1 = number[i] - '0';
            var n2 = multiplier[j] - '0';
            var prod = n1 * n2;
            
            var sum = prod + result[i + j + 1];
            result[i + j + 1] = sum % 10;
            result[i + j] += sum / 10;
        }
    }

    var sb = new System.Text.StringBuilder();
    var skipLeadingZeros = true;
    foreach (var digit in result)
    {
        if (digit != 0) skipLeadingZeros = false;
        if (!skipLeadingZeros) sb.Append(digit);
    }
    
    return sb.Length == 0 ? "0" : sb.ToString();
}

static void AddToDict(Dictionary<string, long> dict, string key, long value)
{
    if (!dict.TryAdd(key, value))
        dict[key] += value;
}

static long[] ParseInput(string input)
{
    return input.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();
}

static List<Stone> ParseInputPart2(string input)
{
    return input.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new Stone { Value = s, Count = 1 })
                .ToList();
}
