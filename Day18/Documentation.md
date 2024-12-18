# Advent of Code 2024 - Day 18: RAM Run

## Problem Overview
1. Part 1: Find shortest path through memory grid with obstacles (first 1024 bytes)
2. Part 2: Find first byte that makes path to exit impossible

## Problem Details

### Part 1: Shortest Path Finding
- Grid size: 71x71 (coordinates 0 to 70)
- Start position: (0,0)
- End position: (70,70)
- Consider first 1024 bytes as obstacles
- Movement: Four directions (up, down, left, right)
- Must avoid obstacles and stay within grid bounds

### Part 2: Critical Path Analysis
- Find first byte that blocks all possible paths
- Binary search through coordinates
- Use BFS to check path existence
- Return coordinates of blocking byte

## Implementation Details

### Core Data Structures

```csharp
internal readonly record struct Point(int X, int Y)
const int gridSize = 71;
(int, int)[] directions = [(0, 1), (1, 0), (0, -1), (-1, 0)];
```
- Point record for coordinate representation
- Fixed grid size constant
- Direction array for movement options

### Key Functions

1. **SolvePartOne**
    ```csharp
    static int SolvePartOne(Point[] coordinates, (int dx, int dy)[] dirs)
    ```
    - Implements Dijkstra's algorithm
    - Uses HashSet for obstacle lookups
    - PriorityQueue for path finding
    - Returns minimum steps to exit

2. **SolvePartTwo**
    ```csharp
    static string SolvePartTwo(Point[] coordinates)
    ```
    - Uses binary search for better performance
    - Uses BFS for path checking
    - Returns coordinates of blocking byte

3. **Helper Functions**
    ```csharp
    static Point[] GetNeighbors(Point point, int size, (int dx, int dy)[] dirs)
    static bool CheckPath(bool[,] grid, bool[,] visited, Queue<(int x, int y)> queue, int size)
    static void AddCoordinatesToGrid(Point[] coordinates, bool[,] grid, int start, int end, int size)
    ```

### Algorithm Details

#### Part 1: Dijkstra's Algorithm Implementation
```
1. Initialize:
   - HashSet of first 1024 obstacles
   - Dictionary for distances
   - PriorityQueue for processing

2. Main Loop:
   - Dequeue point with shortest distance
   - If at end point, return distance
   - For each valid neighbor:
     * Skip if obstacle
     * Update if shorter path found
     * Add to queue with new distance
```

#### Part 2: Binary Search with BFS
```
1. Initialize:
   - Boolean grid for obstacles
   - Visited array and queue for BFS
   - Binary search bounds

2. Binary Search:
   - Calculate midpoint
   - Clear and rebuild grid
   - Check path existence with BFS
   - Adjust search bounds based on result

3. BFS Path Checking:
   - Prioritize right and down movements
   - Reuse data structures
   - Early termination on success
```

## Results
- Part 1: 308 (minimum steps to exit)
- Part 2: 46,28 (coordinates of blocking byte)
