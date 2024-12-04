# Advent of Code 2024 - Day 4: Ceres Search

## Approach

Used grid traversal with directional search for Part 1 and pattern matching for Part 2. This approach was chosen because:
- Grid dimensions are bounded (NxN grid)
- Efficient for finding overlapping patterns
- Memory efficient as it uses input grid directly

## Implementation Details

### Functions
1. **Part1**
    - Scans grid for 'X' characters
    - For each 'X', checks all 8 directions for "XMAS" pattern
    - Time Complexity: O(N²D), where N is grid dimension, D is number of directions (8)
    - Space Complexity: O(1), only using constant extra space

2. **IsValidXmas**
    - Validates if "XMAS" exists in given direction from start position
    - Time Complexity: O(1), checks fixed length pattern (4 chars)
    - Space Complexity: O(1)

3. **Part2**
    - Scans grid for 'A' characters (centers of X patterns)
    - Checks diagonal neighbors for 'M' and 'S' to form X pattern
    - Time Complexity: O(N²), single pass through grid
    - Space Complexity: O(1)

### Algorithm
1. **Part 1 (XMAS Search)**
    - Find all occurrences of 'X'
    - From each 'X', check all 8 directions for complete "XMAS" pattern
    - Count valid patterns

2. **Part 2 (X-MAS Pattern)**
    - Find all 'A' characters
    - Check diagonal positions for valid 'M' and 'S' combinations
    - Count patterns where two "MAS" strings form an X

## Why This Solution?
- Direct grid traversal avoids need for additional data structures
- Early termination when pattern doesn't match improves efficiency
- Position checking is constant time operation
- Memory efficient as it works directly with input grid