# Advent of Code 2024 - Day 10: Hiking Trail Analysis

## Problem Overview
1. Part 1: Calculate the sum of trailhead scores based on reachable height-9 positions
2. Part 2: Calculate the sum of trailhead ratings based on distinct possible paths

## Problem Details

### Part 1: Trailhead Scores
- Grid represents heights from 0 (lowest) to 9 (highest)
- Trailheads are positions with height 0
- Valid moves are up, down, left, or right (no diagonals)
- Height must increase by exactly 1 at each step
- Score is number of height-9 positions reachable from each trailhead
- Final answer is sum of all trailhead scores

### Part 2: Trailhead Ratings
- Same grid and movement rules as Part 1
- Rating is number of distinct possible paths from trailhead to height 9
- Each path must continuously increase in height by 1
- Multiple paths can reach the same height-9 position
- Final answer is sum of all trailhead ratings

## Implementation Details

### Core Functions

1. **ParseGrid**
    ```csharp
    static int[][] ParseGrid(string[] input)
    ```
    - Converts input strings into 2D array of integers
    - Subtracts ASCII '0' to convert characters to digits

2. **FindTrailheads**
    ```csharp
    static IEnumerable<(int row, int col)> FindTrailheads(int[][] grid)
    ```
    - Locates all positions with height 0
    - Returns list of coordinate tuples

3. **GetNextPositions**
    ```csharp
    static IEnumerable<(int row, int col)> GetNextPositions(int[][] grid, (int row, int col) current)
    ```
    - Finds valid next moves from current position
    - Checks four directions (up, right, down, left)
    - Validates grid boundaries and height increment

4. **CalculateTrailheadScore**
    ```csharp
    static int CalculateTrailheadScore(int[][] grid, (int row, int col) start)
    ```
    - Implements Part 1 logic using BFS
    - Tracks visited positions and reachable height-9 positions
    - Returns count of unique reachable height-9 positions

5. **CalculatePathsToNine**
    ```csharp
    static long CalculatePathsToNine(int[][] grid, (int row, int col) start)
    ```
    - Implements Part 2 logic using dynamic programming
    - Groups positions by height for bottom-up processing
    - Memoizes number of paths from each position

### Algorithm Details

1. **Part 1: BFS Approach**
   ```
   1. Find all trailhead positions (height 0)
   2. For each trailhead:
      a. Initialize BFS queue with trailhead
      b. Track visited positions and height-9 positions
      c. Explore valid next positions (height+1)
      d. Count unique height-9 positions reached
   3. Sum all trailhead scores
   ```

2. **Part 2: Dynamic Programming Approach**
   ```
   1. Find all trailhead positions
   2. For each trailhead:
      a. Group grid positions by height
      b. Process positions from height 9 down to 0
      c. Calculate paths by summing paths from next positions
      d. Memoize results for each position
   3. Sum all trailhead ratings
   ```

## Key Optimizations
- Uses BFS for Part 1 to efficiently find reachable positions
- Uses bottom-up DP for Part 2 to avoid recursion and stack overflow
- Memoization prevents recalculating paths for same positions

## Results
- Part 1 (sum of scores): 760
- Part 2 (sum of ratings): 1764