# Advent of Code 2024 - Day 24: Crossed Wires

## Problem Overview
This program simulates a circuit of boolean logic gates installed in a monitoring device in the jungle. The challenge involves:

1. Part 1: Simulate a system of gates and wires to determine the decimal number output on z-wires
2. Part 2: Identify four pairs of gates whose outputs have been swapped, preventing the system from correctly performing binary addition

## Core Functions

### Circuit Input Parsing
```csharp
static (Dictionary<string, bool> initialValues, List<Gate> gates) ReadAndParseInput(string inputFile)
```
- Reads initial wire values and gate connections from input file
- Parses two sections:
    1. Initial wire values (x and y wires)
    2. Gate connections with their types (AND, OR, XOR)
- Returns both initial wire states and gate configurations

### Circuit Simulation (Part 1)
```csharp
static long SolvePartOne(Dictionary<string, bool> initialValues, List<Gate> gates)
```
- Simulates the entire circuit to calculate final wire values
- Key components:
    1. `CalculateWireValues`: Processes gates in order, respecting dependencies
    2. `SimulateGate`: Implements boolean logic for each gate type
    3. `CalculateZWiresValue`: Combines z-wire outputs into final decimal number
- Returns the final decimal value from z-wire states

### Finding Swapped Gates (Part 2)
```csharp
static string SolvePartTwo(List<Gate> gates)
```
- Identifies gates with swapped output wires
- Implementation details:
    1. Checks z-wire gates for incorrect gate types
    2. Examines connections between input (x,y) and internal gates
    3. Verifies correct gate type patterns for binary addition
- Returns comma-separated list of swapped wire names in alphabetical order

## Data Structures

- `Gate` record: Represents a logic gate
    - Type: AND, OR, or XOR
    - Input1, Input2: Input wire names
    - Output: Output wire name


- `Dictionary<string, bool>`: Wire values
    - Key: Wire name
    - Value: Current boolean state, current wire value

## Results
- Part 1 (Circuit Output): 55730288838374
- Part 2 (Swapped Wires): fvw,grf,mdb,nwq,wpq,z18,z22,z36