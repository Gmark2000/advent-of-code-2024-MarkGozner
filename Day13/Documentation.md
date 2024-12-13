# Advent of Code 2024 - Day 13: Claw Contraption

## Problem Overview
1. Part 1: Calculate the minimum tokens needed to win prizes from claw machines with limited button presses
2. Part 2: Calculate the minimum tokens with offset prize positions requiring larger numbers of button presses

## Problem Details

### Part 1: Basic Prize Collection
- Each claw machine has two buttons (A and B)
- Button A costs 3 tokens, Button B costs 1 token
- Each button moves the claw by specific X and Y coordinates
- Prize is won when claw position exactly matches prize coordinates
- Each button press is estimated to be needed no more than 100 times
- Goal is to find minimum tokens needed to win all possible prizes

### Part 2: Offset Prize Positions
- Same mechanics as Part 1
- All prize positions are offset by 10,000,000,000,000 in both X and Y
- No limit on button presses
- Need to handle very large numbers without overflow
- Different machines may become possible/impossible to win

## Implementation Details

### Core Functions

1. **ParseInput & GetMachines**
    ```csharp
    string[] ParseInput(string inputString)
    List<Machine> GetMachines(string inputString)
    ```
    - Parses input text into machine configurations
    - Handles button movements and prize positions
    - Creates Machine records for each configuration

2. **SolveMachine (Part 1)**
    ```csharp
    (bool possible, int tokens) SolveMachine(Machine machine)
    ```
    - Tries combinations of button presses up to 100 each
    - Calculates final position for each combination
    - Returns if solution is possible and token cost
    - Used for Part 1 with smaller numbers

3. **SolveSystem (Part 2)**
    ```csharp
    (bool found, decimal a, decimal b) SolveSystem(decimal ax, decimal ay, decimal bx, decimal by, decimal px, decimal py)
    ```
    - Solves system of linear equations
    - Handles large numbers using decimal type
    - Returns number of presses needed for each button
    - Checks if solution contains whole, non-negative numbers

### Algorithm Details

1. **Part 1: Brute Force Approach**
   ```
   1. For each machine:
      a. Try all combinations of A and B presses (0-100)
      b. Calculate resulting position (X,Y)
      c. Check if position matches prize
      d. If match found, calculate tokens (A*3 + B*1)
   2. Sum tokens for all successful machines
   ```

2. **Part 2: Mathematical Approach**
   ```
   1. For each machine:
      a. Add offset to prize coordinates
      b. Solve system of linear equations:
         - a*ButtonA.x + b*ButtonB.x = PrizeX
         - a*ButtonA.y + b*ButtonB.y = PrizeY
      c. Check if solution exists with non-negative integers
      d. Calculate tokens needed (a*3 + b*1)
   2. Sum tokens for all solvable machines
   ```

## Key Optimizations
- Uses decimal type to handle large numbers without overflow
- Implements Cramer's rule for efficient equation solving
- Avoids brute force for Part 2 where numbers are too large
- Early exit conditions for impossible solutions

## Data Structures

### Machine Record
```csharp
record Machine(
    (int x, int y) ButtonA,
    (int x, int y) ButtonB,
    (long x, long y) Prize
);
```

### Key Constants
```csharp
const decimal OFFSET = 10000000000000m;  // Part 2 prize offset
```

## Results
- Part 1 (Basic Collection): 33,209 tokens
- Part 2 (Offset Prizes): 83,102,355,665,474 tokens