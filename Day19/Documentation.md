# Advent of Code 2024 - Day 19: Linen Layout

## Problem Overview
1. Part 1: Count the number of possible designs that can be created using available towel patterns
2. Part 2: Count all possible unique ways to create each valid design

## Problem Details

### Part 1: Pattern Possibility Check
- Given a set of towel patterns and desired designs
- Each pattern represents a sequence of colored stripes (w, u, b, r, g)
- Must determine if a design can be created using available patterns
- Patterns cannot be reversed/flipped
- Can use multiple instances of the same pattern

### Part 2: Pattern Combination Counting
- For each valid design, count all unique ways it can be created
- Different combinations of patterns that create the same design count separately

## Implementation Details

### Core Data Structures
```csharp
HashSet<string> patterns // Store available towel patterns
Dictionary<string, bool> memo // Memoization for part 1
Dictionary<string, long> memo // Memoization for part 2
```

### Key Functions

1. **SolveTowelPatternsPartOne**
```csharp
static int SolveTowelPatternsPartOne(string input)
```
- Parses input and counts possible designs
- Uses recursive dynamic programming with memoization
- Returns count of achievable designs

2. **SolveTowelPatternsPartTwo**
```csharp
static long SolveTowelPatternsPartTwo(string input)
```
- Counts all possible ways to create each design
- Uses recursive approach for optimal performance
- Returns sum of all possible combinations

3. **Helper Functions**
```csharp
static (HashSet<string> patterns, string[] designs) ParseInput(string input)
static bool CanCreatePattern(string design, HashSet<string> patterns, Dictionary<string, bool> memo)
static long CountWaysToCreate(string design, HashSet<string> patterns, Dictionary<string, long> memo)
```

### Algorithm Details

#### Part 1: Recursive Pattern Matching
```
1. Initialize:
   - Parse patterns and designs
   - Create memoization dictionary
   - For each design:
     * Try to create using available patterns
     * Use recursion with memoization
     * Stop on first valid solution

2. Main Recursion:
   - Base case: empty string is valid
   - Check memo for existing result
   - For each pattern:
     * If design starts with pattern
     * Recursively check remaining string
     * Cache and return result

3. Optimizations:
   - Memoization prevents redundant calculations
   - Early termination on first valid solution
```

#### Part 2: Recursive Combination Counting
```
1. Initialize:
   - Similar setup to Part 1
   - Use long for large numbers
   - Process each design independently

2. Main Recursion:
   - Base case: empty string has one way
   - Check memo for existing count
   - For each matching pattern:
     * Add ways to create remaining string
     * Accumulate total possibilities

3. Key Differences from Part 1:
   - Must explore all possibilities
   - Adds up combinations instead of boolean check
```

## Results
- Part 1: 317 (possible designs)
- Part 2: 883,443,544,805,484 (total possible combinations)
