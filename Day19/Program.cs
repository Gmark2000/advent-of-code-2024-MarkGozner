using static Utils.Utils;

var input = File.ReadAllText("input.txt");

ExecuteAndMeasure("Part One", () => SolveTowelPatternsPartOne(input));
ExecuteAndMeasure("Part Two", () => SolveTowelPatternsPartTwo(input));

return 0;

static int SolveTowelPatternsPartOne(string input)
{
    var (patterns, designs) = ParseInput(input);
    return CountPossibleDesigns(patterns, designs);
}

static long SolveTowelPatternsPartTwo(string input)
{
    var (patterns, designs) = ParseInput(input);
    return CountAllPossibleWays(patterns, designs);
}

static (HashSet<string> patterns, string[] designs) ParseInput(string input)
{
    var parts = input.Split("\n\n");

    var patterns = parts[0].Split(", ")
                          .Select(p => p.Trim())
                          .ToHashSet();

    var designs = parts[1].Split("\n")
                         .Select(d => d.Trim())
                         .ToArray();

    return (patterns, designs);
}

static int CountPossibleDesigns(HashSet<string> patterns, string[] designs)
{
    Dictionary<string, bool> memo = [];
    var possibleDesignsCount = designs.Count(design => CanCreatePattern(design, patterns, memo));
    return possibleDesignsCount;
}

static bool CanCreatePattern(string design, HashSet<string> patterns, Dictionary<string, bool> memo)
{
    if (string.IsNullOrEmpty(design))
        return true;
    
    if (memo.TryGetValue(design, out var createPattern))
        return createPattern;
    
    foreach (var pattern in patterns)
    {
        if (!design.StartsWith(pattern)) continue;
        
        var remaining = design[pattern.Length..];
        
        if (!CanCreatePattern(remaining, patterns, memo)) continue;
        
        memo[design] = true;
        return true;
    }

    memo[design] = false;
    return false;
}

static long CountAllPossibleWays(HashSet<string> patterns, string[] designs)
{
    Dictionary<string, long> memo = [];
    var possibleWaysCount = designs.Sum(design => CountWaysToCreate(design, patterns, memo));
    return possibleWaysCount;
}

static long CountWaysToCreate(string design, HashSet<string> patterns, Dictionary<string, long> memo)
{
    if (string.IsNullOrEmpty(design))
        return 1;
    
    if (memo.TryGetValue(design, out var create))
        return create;

    long totalWays = 0;
    
    foreach (var pattern in patterns)
    {
        if (!design.StartsWith(pattern)) continue;
        
        var remaining = design[pattern.Length..];
        totalWays += CountWaysToCreate(remaining, patterns, memo);
    }

    memo[design] = totalWays;
    return totalWays;
}