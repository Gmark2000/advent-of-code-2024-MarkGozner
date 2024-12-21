# Keypad Conundrum - Technical Documentation

## Problem Overview
This program solves a chain-of-robots challenge where each robot controls another robot through a series of directional keypads, ultimately typing codes on a numeric keypad. The challenge involves:

1. Part 1: Calculate total complexity for 3 layers of robot control
2. Part 2: Calculate total complexity for 26 layers of robot control (you + 25 robots)

## Core Data Structures

### Movement Record
```csharp
readonly record struct Movement((int x, int y) Delta, bool CrossesEmpty)
```
- Immutable structure representing a movement on keypads
- `Delta`: Represents direction and distance of movement (x,y coordinates)
- `CrossesEmpty`: Indicates if movement path crosses empty space (requires reverse path)

### Keypad Dictionary
```csharp
Dictionary<char, (int x, int y)> CreateKeypad(string layout)
```
- Maps keypad buttons to their 2D coordinates
- Supports both numeric (789/456/123/0A) and directional (^,A,<,v,>) keypads
- Enables efficient position lookups

## Core Functions

### CreateKeypad
```csharp
static Dictionary<char, (int x, int y)> CreateKeypad(string layout)
```
- Creates a coordinate mapping for keypad buttons
- Uses modulo arithmetic for 3x4 or 2x3 grid layout
- Returns dictionary mapping characters to (x,y) positions

### GetMovementsForSequence
```csharp
static Dictionary<Movement, long> GetMovementsForSequence(Dictionary<char, (int x, int y)> keypad, string sequence)
```
- Analyzes a sequence of target buttons to reach
- Tracks moves between consecutive positions
- Detects movements that cross empty spaces
- Returns dictionary of movements with their frequencies

### TransformMovements
```csharp
static Dictionary<Movement, long> TransformMovements(Dictionary<Movement, long> movements, Dictionary<char, (int x, int y)> directionalKeypad)
```
- Converts movements through one robot layer
- For each movement:
    1. Generates directional sequence (e.g., "^>A")
    2. Calculates new movements for that sequence
    3. Accumulates frequencies of resulting movements
- Handles multiplication of movement counts through layers

### GetDirectionalSequence
```csharp
static string GetDirectionalSequence(Movement movement)
```
- Converts a movement into directional button presses
- Handles all the coordinate changes
- Reverses sequence if movement crosses empty space (so we find the correct path without crossing the empty space)
  - e.g., going from A to 7 with 2 robot layers: 
    - Without reversal: "<<<^^^A" would try to go left first
    - With reversal:    "^^^<<<A" goes up first, avoiding the empty space
- Always appends 'A' for button activation

## Main Algorithm

### CalculateTotalComplexity
```csharp
static long CalculateTotalComplexity(string[] codes, int robotLayers)
```
1. Creates keypads:
    - Numeric keypad for final input
    - Directional keypad for robot control
2. For each code:
    - Gets initial movements from numeric sequence
    - Transforms movements through robot layers
    - Calculates sequence length * numeric value
3. Returns sum of all complexities

### Key Insights
1. Movement Transformation:
    - Each layer converts movements into new sequences
    - Frequencies multiply through transformations
    - Empty space crossings require path reversal

2. Complexity Management:
    - Uses long integers for large numbers
    - Accumulates frequencies instead of explicit paths
    - Handles movement combinations efficiently

## Optimization Techniques

1. **Movement Aggregation**
    - Combines identical movements
    - Tracks frequencies instead of individual paths

2. **Efficient Data Structures**
    - Dictionary-based position lookups

## Results
- Part 1 (3 layers): 125742
- Part 2 (25 robot layers): 157055032722640

## Implementation Notes
- Uses 26 layers in Part 2, because:
    - 1 layer for my input ("One directional keypad that you are using.")
    - 25 robot keypads
- Empty space handling is crucial for correct path generation