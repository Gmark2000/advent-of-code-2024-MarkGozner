# Advent of Code 2024 - Day 12: Garden Groups

## Problem Overview
1. Part 1: Calculate the total fencing cost based on area and perimeter of garden regions
2. Part 2: Calculate the total fencing cost using the number of distinct sides instead of perimeter

## Problem Details

### Part 1: Area and Perimeter Based Calculation
- Garden plots are arranged in a grid with letters indicating plant types
- Connected plots (horizontally/vertically) of the same plant type form regions
- Each region's price is calculated as: area × perimeter
- Area is the number of plots in the region
- Perimeter is the number of plot sides not connected to the same plant type
- Final answer is the sum of all region prices

### Part 2: Distinct Sides Based Calculation
- Same grid arrangement as Part 1
- Price calculation changes to: area × number of distinct sides
- A side is counted as one unit regardless of its length
- Internal boundaries contribute separate sides for each region
- Diagonal connections don't create shared boundaries

## Implementation Details

### Core Functions

1. **ParseInput**
    ```csharp
    static char[][] ParseInput(string input)
    ```
    - Converts input string into 2D character array
    - Handles line breaks and creates grid structure
    - Used by both parts of the solution

2. **CalculateRegionStats**
    ```csharp
    static (int area, int perimeter) CalculateRegionStats(char[][] grid, bool[,] visited, int startI, int startJ)
    ```
    - Calculates area and perimeter for Part 1
    - Identifies connected regions
    - Tracks visited cells to avoid double counting
    - Returns tuple of (area, perimeter)

3. **FindRegionWithSides**
    ```csharp
    static (int area, int sides) FindRegionWithSides(char[][] grid, bool[,] visited, int startI, int startJ)
    ```
    - Identifies region and calculates area for Part 2
    - Collects all cells in the region
    - Returns tuple of (area, number of sides)

4. **CountSides**
    ```csharp
    static int CountSides(char[][] grid, HashSet<(int i, int j)> region)
    ```
    - Implements the key logic for Part 2
    - Identifies distinct sides of a region
    - Handles special cases (single cells, internal boundaries)
    - Uses direction-based side counting

### Algorithm Details

1. **Part 1: Perimeter-Based Approach**
   ```
   1. Parse input into character grid
   2. For each unvisited cell:
      a. Identify connected region
      b. Calculate area (number of cells)
      c. Calculate perimeter (boundary edges)
      d. Multiply area × perimeter for price
   3. Sum all region prices
   ```

2. **Part 2: Distinct Sides Approach**
   ```
   1. Parse input into character grid
   2. For each unvisited cell:
      a. Collect all cells in the region
      b. Calculate area
      c. Identify distinct sides:
         - Track horizontal and vertical boundaries
         - Handle internal holes
         - Count direction changes
      d. Multiply area × sides for price
   3. Sum all region prices
   ```

## Key Optimizations
- Uses HashSet for efficient region cell tracking
- Avoids revisiting cells with visited array
- Optimizes side counting with directional approach
- Uses Queue for efficient flood fill implementation

## Data Structures

### Core Collections
```csharp
// Region tracking
HashSet<(int i, int j)> region

// Visited cell tracking
bool[,] visited

// Boundary processing
HashSet<string> sides

// Region processing
Queue<(int i, int j)> queue
```

## Results
- Part 1 (Perimeter-based): 1,488,414
- Part 2 (Sides-based): 911,750