# Advent of Code 2024 - Day 14: Restroom Redoubt

## Problem Overview
1. Part 1: Calculate the safety factor by tracking robot positions in quadrants after 100 seconds
2. Part 2: Find the exact moment when robots form a Christmas tree pattern

## Problem Details

### Part 1: Robot Movement and Safety Factor
- Robots move in straight lines on a grid
- Grid size is 101x103 tiles
- Each robot has:
    - Initial position (x,y from top-left corner)
    - Velocity (x,y in tiles per second)
- Robots wrap around edges (teleport to opposite side)
- Safety factor is calculated by:
    - Counting robots in each quadrant after 100 seconds
    - Multiplying the counts together
- Robots on middle lines don't count for any quadrant

### Part 2: Christmas Tree Pattern Detection
- Robots occasionally form a Christmas tree pattern
- Need to find the earliest occurrence of the pattern
- Initial approach:
    - Save all grid states as PNG images
    - Manually inspect the images to find the Christmas tree
    - While reviewing images, notice a pattern in file sizes
- Discovery:
    - Christmas tree pattern creates the most organized, compact structure
    - This results in the smallest PNG file size due to better compression
    - Pattern shows a clear Christmas tree inside a square border
    - Other patterns are more scattered and result in larger file sizes
- Final solution:
    - Track and find the smallest PNG file
    - The timestamp of this file indicates when the Christmas tree appears
    - No need for complex pattern recognition algorithms

## Implementation Details

### Core Functions

1. **ParseInput**
    ```csharp
    static List<Robot> ParseInput(string[] lines)
    ```
    - Parses input text into robot configurations
    - Extracts initial positions and velocities
    - Creates Robot objects for each configuration

2. **CalculateFuturePosition**
    ```csharp
    static Point CalculateFuturePosition(Point initial, Point velocity, int seconds)
    ```
    - Calculates robot position after given seconds
    - Handles wrapping around grid edges
    - Uses modulo arithmetic for position wrapping

3. **CheckQuadrant**
    ```csharp
    static int CheckQuadrant(Point position)
    ```
    - Determines which quadrant a robot is in
    - Returns -1 for robots on middle lines
    - Used for safety factor calculation

4. **SaveGridAsImage**
    ```csharp
    static long SaveGridAsImage(bool[,] grid, int seconds)
    ```
    - Creates PNG image of robot positions
    - Returns file size for pattern detection
    - Uses SkiaSharp for cross-platform image creation

### Algorithm Details

1. **Part 1: Safety Factor Calculation**
   ```
   1. For each robot:
      a. Calculate position at t=100
      b. Determine quadrant
      c. Increment quadrant counter
   2. Multiply all quadrant counts together
   3. Return result as safety factor
   ```

2. **Part 2: Pattern Detection**
   ```
   1. For each time step (0 to 10000):
      a. Calculate all robot positions
      b. Create grid representation
      c. Save as PNG image
      d. Track smallest file size
   2. Return time when smallest file found
   ```

## Key Optimizations
- Uses boolean grid for efficient position tracking
- Implements modulo arithmetic for position wrapping

## Data Structures

### Robot Class
```csharp
internal class Robot
{
    public Point InitialPosition { get; init; }
    public Point Velocity { get; init; }
}
```

### Point Record
```csharp
internal record struct Point(int X, int Y)
```

### Key Constants
```csharp
const int WIDTH = 101;
const int HEIGHT = 103;
const int GRID_SCALE = 4;  // For image visualization
```

## Results
- Part 1 (Safety Factor): 232,589,280
- Part 2 (Christmas Tree Time): 7,569 seconds