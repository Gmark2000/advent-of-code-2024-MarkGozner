# Advent of Code 2024 - Day 7: Bridge Repair

## Problem Overview
The problem involves analyzing calibration equations for bridge repair. Engineers need to:
1. Part 1: Find valid equations using addition and multiplication operators
2. Part 2: Extend solutions by adding a concatenation operator

## Approach

The solution uses dynamic programming with:
- HashSet-based state tracking
- Left-to-right operator evaluation
- Mathematical concatenation handling
- Efficient value combination tracking

This approach was chosen because:
- Efficiently tracks possible values using HashSet
- Naturally handles left-to-right evaluation
- Avoids generating all operator combinations
- Optimized for numerical operations

## Implementation Details

### Key Data Structures
1. **Input Format**
    - Lines containing test values and numbers
    - Format: "testValue: num1 num2 num3..."
    - Operators must be inserted between numbers

2. **Value Tracking**
    - HashSet<long> for possible values at each step
    - List<long> for valid test values
    - Long integers for handling large numbers

### Functions

1. **ProcessLines**
    ```csharp
    static long ProcessLines(string[] lines, bool withConcatenation = false)
    ```
    - Processes all input equations
    - Tracks valid test values
    - Time Complexity: O(N * M * K), where:
        - N is number of lines
        - M is average numbers per line
        - K is average possible values per step
    - Space Complexity: O(K), storing possible values

2. **CanMakeTestValueFromNumbers**
    ```csharp
    static bool CanMakeTestValueFromNumbers(long testValue, long[] numbers, bool withConcatenation)
    ```
    - Evaluates if numbers can produce target value
    - Uses dynamic programming approach
    - Time Complexity: O(M * K)
    - Space Complexity: O(K)

3. **Concatenate**
    ```csharp
    static long Concatenate(long a, long b)
    ```
    - Implements concatenation operator
    - Uses pure mathematical approach
    - Time Complexity: O(log b)
    - Space Complexity: O(1)

### Algorithm

1. **Part 1 (Addition and Multiplication)**
   ```
   1. For each line:
      a. Parse test value and numbers
      b. Initialize possible values with first number
      c. For each remaining number:
         - Try adding to each possible value
         - Try multiplying with each possible value
         - Store new possible values
      d. Check if target value is achievable
   2. Sum valid test values
   ```

2. **Part 2 (With Concatenation)**
   ```
   1. Follow Part 1 algorithm
   2. Add concatenation operator:
      a. For each step, also try:
         - Concatenating current number with each possible value
      b. Store all new possible values
   3. Sum valid test values
   ```

### Operator Rules
1. All operators evaluated left-to-right
2. No operator precedence
3. Numbers cannot be rearranged
4. Available operators:
    - Part 1: +, *
    - Part 2: +, *, ||

## Optimization Techniques
1. HashSet for O(1) value lookup
2. Mathematical concatenation without strings
3. Dynamic programming to avoid recalculation
4. Minimal memory allocation

## Why This Solution?
- Efficient handling of large numbers
- Avoids generating all operator combinations
- Memory-efficient value tracking
- Clear separation of concerns
- Easily extensible for new operators
