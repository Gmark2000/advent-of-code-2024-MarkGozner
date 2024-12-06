# Advent of Code 2023 - Day 6: Guard Patrol

## Problem Overview
The problem involves predicting the path of a guard in a 2D grid-based map. The guard follows specific movement rules and we need to:
1. Part 1: Count distinct positions visited before the guard leaves the map
2. Part 2: Find positions where placing an obstacle would create a patrol loop

## Approach

The solution uses grid-based pathfinding with:
- Position tracking for Part 1
- Loop detection for Part 2
- Direction-based movement simulation
- Obstacle detection and boundary checking

This approach was chosen because:
- Efficiently tracks visited positions using HashSet
- Naturally handles the guard's movement rules
- Effectively detects cycles in patrol routes
- Optimized for grid-based operations

## Implementation Details

### Key Data Structures
1. **Grid Representation**
    - 2D string array storing the map
    - '.' for empty spaces
    - '#' for obstacles
    - '^' for guard's starting position (facing up)

2. **Position Tracking**
    - Tuple (r, c) for row and column coordinates
    - HashSet for tracking visited positions
    - Direction vectors for movement (up, right, down, left)

### Functions

1. **PartOne**
    - Simulates guard's movement until leaving map
    - Tracks unique visited positions
    - Time Complexity: O(R*C), where R is rows, C is columns
    - Space Complexity: O(P), where P is visited positions

2. **PartTwo**
    - Tests each empty position for loop creation
    - Uses cycle detection in patrol routes
    - Time Complexity: O(R*C*P), where P is average path length
    - Space Complexity: O(P), storing visited states

3. **CheckForLoop**
    - Detects cycles in guard's patrol route
    - Uses position+direction state tracking
    - Time Complexity: O(P), where P is path length
    - Space Complexity: O(P), storing visited states

4. **FindStartPosition**
    - Locates guard's initial position
    - Scans grid for '^' character
    - Time Complexity: O(R*C)
    - Space Complexity: O(1)

5. **IsInBounds**
    - Validates grid coordinates
    - Prevents out-of-bounds access
    - Time Complexity: O(1)
    - Space Complexity: O(1)

### Algorithm

1. **Part 1 (Path Tracking)**
   ```
   1. Find guard's starting position
   2. Initialize direction (0 = up)
   3. While still in bounds:
      a. Add current position to visited set
      b. Look ahead in current direction
      c. If obstacle/boundary ahead:
         - Turn right (direction = (direction + 1) % 4)
      d. Else:
         - Move forward
   4. Return count of visited positions
   ```

2. **Part 2 (Loop Detection)**
   ```
   1. For each empty position in grid:
      a. Skip if occupied or start position
      b. Simulate guard movement with test obstacle
      c. Track position+direction states
      d. If state repeats:
         - Found a loop, increment counter
      e. If leaves map:
         - Not a valid position, continue
   2. Return count of loop-creating positions
   ```

### Movement Rules
1. Guard always starts facing up
2. If obstacle ahead: turn right 90 degrees
3. If clear ahead: move forward one step
4. Repeat until leaving map or entering loop

## Optimization Techniques
1. HashSet for O(1) position lookup
2. Direction vectors for efficient movement
3. Early exit on map boundary
4. State-based loop detection
5. Minimal array copying
6. Efficient obstacle checking

## Why This Solution?
- Natural representation of grid-based movement
- Efficient position tracking with HashSet
- Clear separation of path finding and loop detection
- Memory efficient state tracking