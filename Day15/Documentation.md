# Advent of Code 2024 - Day 15: Warehouse Robot Simulation

## Problem Overview
A malfunctioning robot is pushing boxes around in a warehouse, and you need to track its movements and calculate the final positions of the boxes.

### Challenges
1. **Part 1**: Simulate the robot's movement in a standard-sized warehouse
2. **Part 2**: Simulate the robot's movement in a scaled-up warehouse with wider boxes

## Problem Details

### Warehouse Mechanics
- The warehouse is represented as a 2D grid
- Grid elements include:
    - `#`: Walls (immovable)
    - `O` or `[]`: Boxes (movable)
    - `.`: Empty space
    - `@`: Robot's current position

### Movement Rules
- Robot moves according to a sequence of commands:
    - `^`: Move Up
    - `v`: Move Down
    - `<`: Move Left
    - `>`: Move Right
- If a box is in the way, the robot tries to push it
- If pushing would cause a box or robot to hit a wall, no movement occurs

### GPS Coordinate Calculation
- Box locations are tracked using a GPS coordinate system
- Coordinate = `(100 * distance_from_top) + distance_from_left`
- Goal is to calculate the sum of all box GPS coordinates after robot movement

## Implementation Details

### Core Functions

1. **ParseInputFile**
   ```csharp
   static (char[][] grid, char[] movements) ParseInputFile(string filePath)
   ```
    - Reads warehouse grid and movement sequence from input file
    - Separates grid layout from movement instructions

2. **ProcessMovements**
   ```csharp
   static void ProcessMovements(char[][] grid, char[] movements)
   ```
    - Iterates through movement sequence
    - Updates robot and box positions based on movement rules

3. **MoveRobot**
   ```csharp
   static Point MoveRobot(char[][] grid, Point robotPosition, char movement)
   ```
    - Handles complex robot movement logic
    - Checks for possible box pushes
    - Prevents movement into walls or impossible configurations

4. **CalculateBoxGpsCoordinatesSum**
   ```csharp
   static long CalculateBoxGpsCoordinatesSum(char[][] grid)
   ```
    - Calculates GPS coordinates for all boxes
    - Sums the coordinates to get final result

### Part Two Modifications
- Grid is scaled up to double width
- Boxes represented as `[]` instead of `O`
- Scaling allows pushing multiple boxes simultaneously
- GPS coordinate calculation slightly modified for wider boxes

## Key Challenges and Solutions

### Box Pushing Algorithm
- Recursive approach to handle chain of box movements
- Checks for wall collisions and empty spaces
- Carefully moves boxes without breaking grid rules

### Performance Considerations
- Uses in-place grid modification
- Minimal memory allocation
- Efficient movement tracking

## Data Structures

### Point Record
```csharp
internal record struct Point(int X, int Y)
```
- Represents grid coordinates
- Used for robot and box position tracking

### Movement Constants
```csharp
internal static class Movement
{
    internal const char Up = '^';
    internal const char Down = 'v';
    internal const char Left = '<';
    internal const char Right = '>';
}
```
- Defines movement direction constants

## Execution Flow
1. Parse input file
2. Process robot movements
3. Calculate final box GPS coordinates
4. Return total coordinate sum

## Additional Core Functions

### SolvePartOne
```csharp
static long SolvePartOne()
```
- Top-level function for solving Part 1
- Workflow:
    1. Parse input file
    2. Process robot movements
    3. Calculate GPS coordinates sum
- Returns total GPS coordinate sum

### SolvePartTwo
```csharp
static long SolvePartTwo()
```
- Top-level function for solving Part 2
- Similar to SolvePartOne, but with scaled warehouse
- Handles wider grid and box representation

### ParseInputFileForPartTwo
```csharp
static (char[][] grid, char[] movements) ParseInputFileForPartTwo(string filePath)
```
- Extends original input parsing for Part 2
- Transforms grid to double width
- Mapping of original characters to scaled representation:
    - Wall (`#`) → Double wall (`##`)
    - Box (`O`) → Wide box (`[]`)
    - Empty (`.`) → Double empty space (`..`)
    - Robot (`@`) → Robot with empty space (`@.`)

### ProcessMovementsPartTwo
```csharp
static void ProcessMovementsPartTwo(char[][] grid, char[] movements)
```
- Handles robot movement in scaled warehouse
- Uses directional arrays for movement
- More complex pushing mechanism
- Supports simultaneous pushing of multiple wide boxes

### CalculateBoxGpsCoordinatesPartTwo
```csharp
static long CalculateBoxGpsCoordinatesPartTwo(char[][] grid)
```
- Calculates GPS coordinates for Part 2
- Specifically looks for left edge of wide boxes (`[`)
- Adjusts coordinate calculation for wider grid

### Push Method
```csharp
static void Push(char[][] a, int sx, int sy, int dx, int dy)
```
- Recursive pushing mechanism
- Handles complex box pushing scenarios
- Supports:
    - Pushing single boxes
    - Pushing wide boxes
    - Handling multiple box chains

### TryPush Method
```csharp
static bool TryPush(char[][] a, int sx, int sy, int dx, int dy)
```
- Checks if a push movement is possible
- Validates:
    - No wall collisions
    - Sufficient empty space
    - Compatibility with wide boxes
- Returns boolean indicating push feasibility

## Results
- **Part 1**: 1,446,158 (Sum of box GPS coordinates)
- **Part 2**: 1,446,175 (Sum of box GPS coordinates in scaled warehouse)
