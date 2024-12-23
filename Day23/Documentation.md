# Advent of Code 2024 - Day 23: LAN Party

## Problem Overview
This program analyzes a network map from Easter Bunny HQ to locate a LAN party by examining computer connections. The challenge involves:

1. Part 1: Find all sets of three interconnected computers containing at least one computer starting with 't'
2. Part 2: Locate the largest fully connected group of computers to find the LAN party password

## Core Functions

### Network Map Parsing
```csharp
static Dictionary<string, HashSet<string>> ReadAndParseInput(string inputFile)
```
- Reads network connection data from input file
- Creates bidirectional graph using Dictionary and HashSet
- Uses TryGetValue for efficient lookups
- Returns complete map of computer connections

### Finding Connected Triples (Part 1)
```csharp
static int SolvePartOne(Dictionary<string, HashSet<string>> computers)
```
- Identifies all unique sets of three fully connected computers
- Uses connection-based traversal for efficiency:
    1. Start from each computer (comp1)
    2. Check its connections (comp2)
    3. Check comp2's connections (comp3)
    4. Verify comp3 connects back to comp1
- Filters for computers starting with 't'
- Uses HashSet to store unique triples
- Returns count of valid triples

### Finding the Largest Group of Connected Computers (Part 2)
```csharp
static string SolvePartTwo(Dictionary<string, HashSet<string>> computers)
```
- Finds largest fully connected group of computers
- Implementation details:
    1. Start from each computer as potential group member
    2. Build computer group by adding only computers connected to all current members
    3. Track potential connections and update after each addition
    4. Keep track of largest group of computers found
- Returns password string (computer names sorted alphabetically, then joined together with commas)

## Data Structures

### Network Representation
- Primary: `Dictionary<string, HashSet<string>>`
    - Key: Computer name
    - Value: HashSet of connected computers
    - Provides O(1) lookup for connections
    - Automatically handles duplicate connections

## Optimization Techniques

1. **Efficient Graph Traversal**
    - Only checks actual connections
    - Avoids checking not connected computers
    - Uses HashSet for O(1) lookups

2. **Memory Optimization**
    - Reuses HashSets where possible
    - Uses collection initializers
    - Minimizes string allocations

3. **Performance Considerations**
    - Early exit conditions
    - Connection-based iteration

## Results
- Part 1 (Triples with 't'): 1000
- Part 2 (LAN Party Password): cf,ct,cv,cz,fi,lq,my,pa,sl,tt,vw,wz,yd