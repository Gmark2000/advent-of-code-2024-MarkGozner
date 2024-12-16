# Advent of Code 2024 - Day 16: Reindeer Maze

## Problem Overview
1. Part 1: Find the lowest possible score for a reindeer navigating through a maze
2. Part 2: Identify all tiles that are part of any optimal path through the maze

## Problem Details

### Part 1: Optimal Path Finding
- Reindeer starts at 'S' tile facing East
- Must reach 'E' tile with minimum score
- Movement costs:
    - Moving forward: 1 point
    - Rotating 90° (clockwise/counterclockwise): 1000 points
- Cannot move through walls (#)
- Score is calculated by summing all movement and rotation costs

### Part 2: Optimal Path Tiles
- Identify all tiles that are part of any optimal path
- Includes:
    - Start tile (S)
    - End tile (E)
    - Any empty tile (.) that's part of a path with minimum score
- Multiple optimal paths may exist with the same minimum score

## Implementation Details

### Core Data Structures

1. **Point Record**
```csharp
internal record struct Point(int X, int Y)
```
- Represents a position in the maze
- Used for tracking current position and path history

2. **State Record**
```csharp
internal record struct State(Point Pos, int Dir)
```
- Combines position and direction
- Used for tracking unique states in the search

3. **SearchNode Record**
```csharp
internal record struct SearchNode(State CurrentState, HashSet<Point> Path, int Cost)
```
- Represents a node in the search tree
- Tracks current state, path history, and accumulated cost

4. **SearchState Class**
```csharp
internal class SearchState
{
    public required PriorityQueue<SearchNode, int> Queue { get; init; }
    public required Dictionary<State, int> CostToState { get; init; }
    public required HashSet<Point> OptimalTiles { get; init; }
    public required List<HashSet<Point>> PathsToExplore { get; init; }
    public int MinCost { get; set; }
}
```
- Contains all search-related data structures
- Manages the search queue and optimal path tracking

### Core Functions

1. **FindStartAndEnd**
```csharp
static (Point start, Point end) FindStartAndEnd(string[] grid)
```
- Scans grid to locate 'S' and 'E' tiles
- Returns start and end positions as Points

2. **SolveMaze**
```csharp
static (int minCost, int optimalTileCount) SolveMaze(string[] grid, Point start, Point end)
```
- Main solving function that returns both part 1 and part 2 solutions
- Returns minimum cost and count of tiles in optimal paths

3. **InitializeSearchState**
```csharp
static SearchState InitializeSearchState(Point start)
```
- Creates initial search state with start position
- Sets up priority queue with initial node facing east

4. **ExploreAllPaths**
```csharp
static (int minCost, HashSet<Point> optimalTiles, List<HashSet<Point>> pathsToExplore) 
ExploreAllPaths(SearchState state, string[] grid, Point start, Point end, int width, int height)
```
- Implements main search algorithm
- Uses Dijkstra's algorithm with state-space search
- Tracks all optimal paths and their tiles

5. **GetNextStates**
```csharp
static IEnumerable<(State state, bool isRotation)> GetNextStates(
    State current, string[] grid, int width, int height)
```
- Generates possible next moves from current state
- Includes rotations and forward movement
- Validates moves against grid boundaries and walls

### Helper Functions

1. **ShouldContinueExploration**
```csharp
static bool ShouldContinueExploration(SearchState state, SearchNode current)
```
- Determines if current path should be explored further
- Checks against known costs and minimum cost found so far

2. **HasReachedEnd**
```csharp
static bool HasReachedEnd(SearchState state, SearchNode current, Point end)
```
- Checks if current node has reached the end
- Updates minimum cost and optimal paths if necessary

3. **ExploreNextStates**
```csharp
static void ExploreNextStates(SearchState state, SearchNode current, string[] grid, int width, int height)
```
- Processes all possible next moves from current state
- Adds valid moves to the search queue

4. **GetDirectionOffset**
```csharp
static (int dx, int dy) GetDirectionOffset(int direction)
```
- Converts direction (0-3) to coordinate offsets
- Used for calculating next position when moving forward

## Algorithm Details

1. **Part 1: Finding Minimum Cost Path**
```
1. Initialize search with start position facing east
2. For each state in priority queue:
   a. Generate possible moves (forward and rotations)
   b. Calculate cost for each move
   c. Add valid moves to queue with priority = total cost
   d. Track minimum cost to reach end
3. Return minimum cost found
```

2. **Part 2: Finding All Optimal Path Tiles**
```
1. During the search process:
   a. Track all paths that reach end with minimum cost
   b. Store all tiles visited in these paths
2. After search completes:
   a. Combine all tiles from optimal paths
   b. Return count of unique tiles
```

## Results

- Part 1 (Minimum Cost): 95,476
- Part 2 (Optimal Tiles Count): 511