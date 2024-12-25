# Advent of Code 2024 - Day 25: Code Chronicle

## Problem Overview
This program analyzes virtual five-pin tumbler lock schematics to find compatible lock and key pairs. The challenge involves:

1. Part 1: Count how many unique lock/key pairs can fit together without their pins overlapping in any column
2. Part 2: N/A (No second part for Day 25) 🎅

## Core Functions

### Schematic Input Parsing
```csharp
static (Dictionary<int, int[]> locks, Dictionary<int, int[]> keys) ReadAndParseInput(string inputFile)
```
- Reads lock and key schematics from input file
- Parses each schematic to extract pin heights:
    1. Locks: Counts empty spaces from bottom to top
    2. Keys: Counts empty spaces from top to bottom
- Returns dictionaries containing pin heights for each lock and key

### Lock and Key Compatibility Check (Part 1)
```csharp
static int SolvePartOne(Dictionary<int, int[]> locks, Dictionary<int, int[]> keys)
```
- Checks every possible lock and key combination
- Uses `DoesNotOverlapInAnyColumn` to verify pin compatibility
- Returns total count of compatible lock/key pairs

### Pin Height Validation
```csharp
static bool DoesNotOverlapInAnyColumn(int[] lockHeights, int[] keyHeights)
```
- Verifies if a lock and key can fit together
- Checks each column to ensure pins don't overlap
- Returns true if lock and key are compatible

## Data Structures

- `Dictionary<int, int[]>`: Lock and key collections
    - Key: Index of lock/key
    - Value: Array of pin heights for each column

## Results
- Part 1 (Compatible Pairs): 3466
- Part 2: No second part for final day 🎄