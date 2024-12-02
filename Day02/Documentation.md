# Advent of Code 2024 - Day 2: Red-Nosed Reactor

## Algorithm Explanation

### Part 1: Basic Safety Check

The solution uses three main functions that work together:

1. `CountSafeReports(string filePath)`
    - Reads the input file line by line using streaming
    - Counts how many sequences are safe by calling `IsSafeReport` for each line
    - Keeps track of total safe sequences found

2. `IsSafeReport(string line)`
    - Validates input and parses the line into an array of integers
    - Calls `IsSafeSequence` with the parsed array
    - Handles empty or invalid inputs

3. `IsSafeSequence(int[] levels)`
    - First checks if sequence is at least 2 numbers long
    - Determines direction (increasing/decreasing) from first two numbers
    - Validates each adjacent pair follows these rules:
        * Difference must be 1-3 numbers (inclusive)
        * Must maintain same direction (all increasing or all decreasing)
        * Returns false immediately if any rule is violated
    - Returns true only if all checks pass

### Part 2: Problem Dampener

The solution extends Part 1 with two main functions:

1. `CountSafeReportsWithOneRemoval(string filePath)`
    - Similar to Part 1's counting function
    - Uses `IsSafeWithDampener` instead of `IsSafeReport`
    - Keeps track of sequences that are safe with one removal

2. `IsSafeWithDampener(string line)`
    - First checks if sequence is already safe using original logic
    - If not safe, tries removing each number one at a time:
        * Creates new sequence without current number using Array.Copy
        * Checks if new sequence is safe
        * Returns true as soon as any safe sequence is found
    - Returns false if no removal makes sequence safe

Both parts use early exit strategies to optimize performance, stopping as soon as a definitive result is found rather than checking all possibilities unnecessarily.