# Advent of Code 2024 - Day 11: Plutonian Pebbles

## Problem Overview
1. Part 1: Calculate the number of stones after 25 blinks using numeric transformations
2. Part 2: Calculate the number of stones after 75 blinks using string-based transformations for large numbers

## Problem Details

### Part 1: Stone Transformations (25 blinks)
- Stones are arranged in a straight line with numbers engraved
- Each blink transforms stones simultaneously based on rules:
    1. If stone is 0, becomes 1
    2. If stone has even digits, splits into two stones (left and right halves)
    3. Otherwise, multiply by 2024
- Order is preserved after transformations
- Final answer is the total count of stones after 25 blinks

### Part 2: Extended Transformations (75 blinks)
- Same rules as Part 1
- Requires handling much larger numbers
- Uses string-based operations to handle number overflow
- Final answer is the total count of stones after 75 blinks

## Implementation Details

### Core Functions

1. **CountDigits**
    ```csharp
    static int CountDigits(long n)
    ```
    - Counts number of digits in a number
    - Handles zero as special case (1 digit)
    - Used for determining even/odd digit count

2. **PowerOf10**
    ```csharp
    static long PowerOf10(int power)
    ```
    - Calculates powers of 10 for number splitting
    - Used in dividing numbers for even-digit rule

3. **SplitNumber**
    ```csharp
    static (long left, long right) SplitNumber(long number, int digitCount)
    ```
    - Splits a number with even digits into two parts
    - Returns tuple of left and right halves

4. **TransformStones**
    ```csharp
    static long[] TransformStones(long[] stones)
    ```
    - Implements Part 1 transformation rules
    - Handles numeric operations for 25 blinks

5. **TransformStonesForPartTwo**
    ```csharp
    static List<Stone> TransformStonesForPartTwo(List<Stone> stones)
    ```
    - Implements Part 2 transformation rules
    - Uses string operations for large numbers
    - Groups identical stones to save memory

### Algorithm Details

1. **Part 1: Numeric Approach**
   ```
   1. Parse input numbers into long array
   2. For each blink (25 times):
      a. Process each stone according to rules
      b. Handle 0 → 1 transformation
      c. Split even-digit numbers
      d. Multiply odd-digit numbers by 2024
   3. Return final stone count
   ```

2. **Part 2: String-Based Approach**
   ```
   1. Parse input into Stone objects with string values
   2. For each blink (75 times):
      a. Process stones using string operations
      b. Group identical stones using dictionary
      c. Handle string multiplication for large numbers
      d. Track counts efficiently
   3. Sum all stone counts
   ```

## Key Optimizations
- Uses dictionary to group identical stones in Part 2
- Handles string operations for large number arithmetic

## Data Structures

### Stone Class
```csharp
public class Stone
{
    public required string Value { get; init; }
    public long Count { get; init; }
}
```
- Represents a stone with its value and count
- Used in Part 2 for efficient grouping

## Results
- Part 1 (25 blinks): 186,203 stones
- Part 2 (75 blinks): 221,291,560,078,593 stones