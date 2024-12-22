# Monkey Market - Technical Documentation

## Problem Overview
This program solves a trading challenge in the Monkey Exchange Market where buyers use pseudorandom sequences to determine prices. The challenge involves:

1. Part 1: Calculate sum of 2000th secret numbers for all buyers
2. Part 2: Find optimal sequence of price changes to maximize banana collection

## Core Functions

### Secret Number Generation

```csharp
static long GenerateNextSecret(long secret)
```
- Transforms a secret number into the next in sequence through three steps:
    1. Multiply by 64, mix and prune
    2. Divide by 32, mix and prune
    3. Multiply by 2048, mix and prune
- Uses bitwise XOR for mixing
- Uses modulo 16777216 for pruning

### Mix and Prune Operation
```csharp
static void MixAndPrune(ref long secret, long value)
```
- Performs the fundamental operations on secret numbers:
    - Mix: XOR operation between secret and value
    - Prune: Modulo operation to keep number within bounds

### Price Generation
```csharp
static List<int> GeneratePrices(long initialSecret, int count)
```
- Generates sequence of prices from secret numbers
- Takes ones digit of each secret number
- Returns list of prices

### Change Calculation
```csharp
static List<int> CalculateChanges(List<int> prices)
```
- Calculates price differences between consecutive prices
- Returns list of changes for sequence analysis
- Uses simple loop for performance

### Sequence Finding
```csharp
static Dictionary<(int, int, int, int), int> FindSequences(List<int> prices, List<int> changes)
```
- Identifies all unique 4-change sequences
- Maps sequences to their resulting prices
- Only stores first occurrence of each sequence

## Main Algorithm

### Part One (SolvePartOne)
```csharp
static long SolvePartOne(long[] initialSecrets)
```
1. For each initial secret:
    - Generate 2000 new secret numbers
    - Take the 2000th number
2. Sum all 2000th numbers
3. Return total

### Part Two (SolvePartTwo)
```csharp
static long SolvePartTwo(long[] initialSecrets)
```
1. Process all buyers in parallel
2. For each buyer:
    - Generate price sequence
    - Calculate price changes
    - Find all unique 4-change sequences
3. Track sequence occurrences across all buyers
4. Return maximum total bananas possible

## Optimization Techniques

1. **Parallel Processing**
    - Uses `Parallel.ForEach` for buyer processing
    - Uses `ConcurrentDictionary` for thread-safe operations

## Results
- Part 1 (Sum of 2000th secrets): 13764677935
- Part 2 (Maximum bananas): 1619