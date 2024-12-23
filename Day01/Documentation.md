# Advent of Code 2024 - Day 1: Historian Hysteria

## Approach

Used dictionaries to count occurrences of values in both lists, effectively implementing a counting sort approach. This was chosen because:
- Input values are bounded (< 100,000)
- Allows linear time processing without explicit sorting
- Memory efficient for the given constraints

## Implementation Details

### Data Structure
- Two `Dictionary<int, int>` for frequency counting of each column
- Tracks min/max values to optimize iteration range

### Algorithm
1. **Part 1 (Distance Calculation)**
    - Process numbers from min to max value
    - Match identical values first (0 distance)
    - For remaining values, match with closest available numbers
    - Complexity: O(n) for parsing + O(k) for processing, where k is range of values, so the complexity remains O(n)

2. **Part 2 (Similarity Score)**
    - Simply iterate through value range once
    - For each value, multiply its frequencies from both lists
    - Complexity: O(k) where k is range of values

## Why This Solution?
- Sorting would require O(n log n) time
- Since input values < 100,000, counting approach is more efficient
- Dictionary-based solution runs in linear time for parsing
- Avoids unnecessary sorting while maintaining good space complexity