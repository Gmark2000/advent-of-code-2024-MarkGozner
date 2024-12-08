# Advent of Code 2023 - Day 8: Resonant Collinearity

## Problem Overview
1. Part 1: Finding antinodes where pairs of same-frequency antennas create resonant points with a specific 1:2 distance ratio
2. Part 2: Identifying all points that are collinear with any two antennas of the same frequency, incorporating resonant harmonics

## Problem Details

### Part 1: Distance-Based Antinodes
- Antinodes occur when a point is perfectly in line with two same-frequency antennas
- One antenna must be twice as far from the antinode as the other
- For any pair of antennas, there are two potential antinodes (one on either side)
- Antinodes can overlap with antenna positions
- Different frequencies (e.g., 'A' and 'a') don't interact

### Part 2: Resonant Harmonics
- Any point collinear with two same-frequency antennas becomes an antinode
- Distance ratios are no longer relevant
- Antenna positions themselves become antinodes if they align with other same-frequency antennas
- Still requires same-frequency antennas for interaction

## Implementation Details

### Core Functions

1. **MapAntennas**
    ```csharp
    static Dictionary<char, List<Point>> MapAntennas(string[] grid)
    ```
    - Groups antenna positions by their frequency
    - Time Complexity: O(R * C)
    - Space Complexity: O(A), where A is antenna count

2. **FindAntinodesWithRatio**
    ```csharp
    static HashSet<Point> FindAntinodesWithRatio(...)
    ```
    - Implements Part 1 logic
    - Checks both collinearity and distance ratio
    - Handles antenna position overlaps

3. **FindAntinodesCollinear**
    ```csharp
    static HashSet<Point> FindAntinodesCollinear(...)
    ```
    - Implements Part 2 logic
    - Checks only collinearity
    - Includes antenna positions as potential antinodes

### Algorithm Details

1. **Part 1: Distance-Based Detection**
   ```
   1. Group antennas by frequency
   2. For each frequency group with 2+ antennas:
      a. Scan each grid point
      b. For each antenna pair:
         - Check if point is collinear
         - Calculate distances from point to both antennas
         - Verify 1:2 distance ratio
         - Add valid points to result set
   ```

2. **Part 2: Collinearity Detection**
   ```
   1. Group antennas by frequency
   2. For each frequency group with 2+ antennas:
      a. Add all antennas as antinodes
      b. For each grid point:
         - Check collinearity with any antenna pair
         - Add collinear points to result set
   ```

### Helper functions
1. **Collinearity Check**
    ```csharp
    static bool AreCollinear(Point p1, Point p2, Point p3)
    {
        var area = (p2.X - p1.X) * (p3.Y - p1.Y) - 
                  (p3.X - p1.X) * (p2.Y - p1.Y);
        return area == 0;
    }
    ```

2. **Distance Calculation**
    ```csharp
    static double Distance(Point p1, Point p2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    ```

## Optimization Techniques
1. HashSet for efficient unique point tracking
2. Dictionary for frequency-based grouping
3. Early exit conditions
4. Precomputed antenna pairs
5. Grid-based point scanning

## Results
- Part 1: 351 unique antinode locations
- Part 2: 1259 unique antinode locations