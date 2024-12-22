using System.Collections.Concurrent;
using static Utils.Utils;

var initialSecrets = ReadInput("input.txt").Select(long.Parse).ToArray();

ExecuteAndMeasure("First part", () => SolvePartOne(initialSecrets));
ExecuteAndMeasure("Second part", () => SolvePartTwo(initialSecrets));

return 0;

static long SolvePartOne(long[] initialSecrets)
{
    return initialSecrets.Select(secret => GenerateNthSecret(secret, 2000)).Sum();
}

static long GenerateNthSecret(long initialSecret, int n)
{
    var currentSecret = initialSecret;
    
    for (var i = 0; i < n; i++)
    {
        currentSecret = GenerateNextSecret(currentSecret);
    }
    
    return currentSecret;
}

static long SolvePartTwo(long[] initialSecrets)
{
    var sequenceCounts = new ConcurrentDictionary<(int, int, int, int), int>();
    
    Parallel.ForEach(initialSecrets, secret =>
    {
        var prices = GeneratePrices(secret, 2000);
        var changes = CalculateChanges(prices);
        var sequences = FindSequences(prices, changes);
        
        foreach (var (sequence, price) in sequences)
        {
            sequenceCounts.AddOrUpdate(
                sequence,
                price,
                (_, currentCount) => currentCount + price
            );
        }
    });
    
    return sequenceCounts.Values.Max();
}

static List<int> GeneratePrices(long initialSecret, int count)
{
    List<int> prices = [];
    var currentSecret = initialSecret;
    
    for (var i = 0; i < count; i++)
    {
        currentSecret = GenerateNextSecret(currentSecret);
        prices.Add((int)(currentSecret % 10));
    }
    
    return prices;
}

static List<int> CalculateChanges(List<int> prices)
{
    List<int> changes = new(prices.Count - 1);
    for (var i = 1; i < prices.Count; i++)
    {
        changes.Add(prices[i] - prices[i - 1]);
    }
    return changes;
}

static Dictionary<(int, int, int, int), int> FindSequences(List<int> prices, List<int> changes)
{
    var sequences = new Dictionary<(int, int, int, int), int>();
    
    for (var i = 0; i <= changes.Count - 4; i++)
    {
        var sequence = (
            changes[i],
            changes[i + 1],
            changes[i + 2],
            changes[i + 3]
        );
        
        if (!sequences.ContainsKey(sequence))
        {
            sequences[sequence] = prices[i + 4];
        }
    }
    
    return sequences;
}

static long GenerateNextSecret(long secret)
{
    MixAndPrune(ref secret, secret * 64);
    MixAndPrune(ref secret, secret / 32);
    MixAndPrune(ref secret, secret * 2048);
    
    return secret;
}

static void MixAndPrune(ref long secret, long value)
{
    secret ^= value;
    secret %= 16777216;
}