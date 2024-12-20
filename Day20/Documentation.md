# Day 20: Race Condition

## Problem Overview
This program solves a pathfinding challenge where a program must navigate through a racetrack from start (S) to end (E), with the ability to cheat by passing through walls for a limited time.

1. Part 1: Find all possible cheats that save at least 100 picoseconds using 2-picosecond wall-passing
2. Part 2: Find all possible cheats that save at least 100 picoseconds using up to 20-picosecond wall-passing

## Core Data Structures

### Point Structure
```csharp
internal readonly record struct Point(int X, int Y)
```
- Immutable structure representing a position on the grid
- Used for tracking positions and movement

### Direction Array
```csharp
(int dx, int dy)[] directions = [(0, 1), (1, 0), (0, -1), (-1, 0)]
```
- Represents the four possible movement directions (right, down, left, up)
- Used for exploring neighbor cells in the grid

## Core Functions

### FindPath
```csharp
static (List<Point> path, Point start, Point end) FindPath(string[] grid)
```
- Locates start ('S') and end ('E') positions in the grid
- Constructs the initial valid path through the track
- Returns the complete path and start/end positions

### GetNextPoint
```csharp
static Point GetNextPoint(Point p, string[] grid, List<Point> visited)
```
- Determines the next valid move from the current position
- Checks all four directions for unvisited valid moves
- Returns the first valid unvisited point found
- Used in initial path construction

### IsValidMove & IsInBounds
```csharp
static bool IsValidMove(Point p, string[] grid)
static bool IsInBounds(Point p, string[] grid)
```
- Utility functions
- IsInBounds: Ensures point is within grid boundaries
- IsValidMove: Checks if point is valid track ('.' or 'E')

## Main Algorithms

### Part One:
```csharp
static int SolvePartOne(Point start, string[] grid, List<Point> path, (int dx, int dy)[] dirs)
```
- Finds all possible cheats using 2 picoseconds wall-passing
- Algorithm:
    1. Creates position-to-index mapping for quick lookups
    2. For each position in path:
        - Explores possible 2-step moves through walls
        - Checks if moves create significant shortcuts (≥100 picoseconds)
    3. Returns count of valid shortcuts
- Uses GetReachablePoints for exploration

### GetReachablePoints (used for Part 1)
```csharp
static IEnumerable<Point> GetReachablePoints(Point start, string[] grid, (int dx, int dy)[] dirs)
```
- Finds all points reachable within 2 steps
- Uses breadth-first search (BFS) for exploration
- Tracks visited points to avoid cycles
- Returns points that are near valid paths

### Part Two:
```csharp
static int SolvePartTwo(Point start, string[] grid, List<Point> path, (int dx, int dy)[] dirs)
```
- Implements extended cheating with up to 20 picoseconds
- Uses parallel processing for performance
- Algorithm similar to Part One but with deeper exploration
- Uses thread-safe collections for parallel execution

### GetReachablePointsPart2
```csharp
static IEnumerable<(Point point, int steps)> GetReachablePointsPart2(Point start, string[] grid, (int dx, int dy)[] dirs)
```
- Enhanced version of GetReachablePoints for Part 2
- Explores up to 20 steps in any direction
- Returns points with their step counts
- Uses BFS with extended depth

## Optimization Techniques

1. **Parallel Processing**
    - Part 2 uses Parallel.For and Thread-safe collections for concurrent exploration

2. **Efficient Data Structures**
    - For example HashSet for O(1) visited point lookups

3. **Early Termination**
    - Stops exploration when maximum steps reached

## Results (cheats saving ≥100 picoseconds)
- Part 1: 1289 
- Part 2: 982425

## Performance Considerations
- Part 1 is relatively fast due to limited exploration depth
- Part 2 is more computationally intensive but optimized with parallel processing
