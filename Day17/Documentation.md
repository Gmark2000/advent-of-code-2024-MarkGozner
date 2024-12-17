# Advent of Code 2024 - Day 17: Chronospatial Computer

## Problem Overview
1. Part 1: Execute a 3-bit computer program and collect its output
2. Part 2: Reverse engineer an initial register A value that makes the program output itself

## Problem Details

### Part 1: Program Execution
- Program consists of 3-bit numbers (0-7)
- Three registers: A, B, and C (can hold any integer)
- Instructions have two parts:
    - Opcode (3-bit number)
    - Operand (3-bit number)
- Instruction pointer advances by 2 after each instruction (except jumps)
- Two types of operands:
    - Literal operands: direct value
    - Combo operands: value derived from registers or small literals

### Part 2: Self-Reproducing Program
- Find initial value for register A
- Program must output its own code
- Must find lowest positive value that works
- Uses reverse engineering from output to input

## Implementation Details

### Core Data Structures

```csharp
internal record struct State(long A, long B, long C)
```
- State record holding register values
- Uses long to handle large numbers in calculations

### Core Functions

1. **RunProgram**
    ```csharp
    static (List<int> Output, State FinalState) RunProgram(int[] program, State initialState)
    ```
    - Executes the program with given initial state
    - Returns both output and final state
    - Handles instruction execution and state transitions
    - Manages instruction pointer movement

2. **GetComboValue**
    ```csharp
    static int GetComboValue(int operand, State state)
    ```
    - Resolves combo operand values
    - Maps operands 0-3 to literals
    - Maps operands 4-6 to register values
    - Takes last 3 bits of register values

3. **ExecuteInstruction**
    ```csharp
    static State ExecuteInstruction(int opcode, int operand, int combo, State state)
    ```
    - Implements the eight instruction types
    - Returns new state after instruction execution

4. **ReverseEngineerRecursive**
    ```csharp
    static long ReverseEngineerRecursive(int[] loop, List<int> target, long aSoFar)
    ```
    - Core algorithm for finding initial A value
    - Builds solution from least to most significant bits
    - Uses recursive approach to construct the value
    - Tests candidate values against target output

### Reverse Engineering Algorithm

```
1. Start with target program sequence reversed (we build A from right to left)
2. For each 3-bit group:
   a. Try values 0-7 as new least significant bits
   b. Run program with constructed value
   c. Check if output matches target digit
   d. If match found, recurse with remaining target
3. Build solution from right to left
```

Key aspects:
- Processes 3 bits at a time (matching program's 3-bit nature)
- Uses program's own output mechanism to verify correctness

## Results
- Part 1 (output): 7,3,5,7,5,7,4,3,0
- Part 2 (register A): 105734774294938
