# Advent of Code 2023 - Day 5: Print Queue

## Approach

Used graph-based dependency analysis with topological sorting for both parts. This approach was chosen because:
- The problem represents a directed acyclic graph (DAG) of page dependencies
- Efficient for validating ordering constraints
- Optimal for finding correct orderings in Part 2
- Handles partial dependencies well (not all pages need rules)

## Implementation Details

### Functions

1. **ParseInputFile**
    - Parses input file into rules and updates sections
    - Handles the format separation with empty line
    - Time Complexity: O(N), where N is number of lines
    - Space Complexity: O(N), storing rules and updates

2. **BuildDependencyGraph**
    - Constructs directed graph from page ordering rules
    - Each rule X|Y creates edge X→Y in graph
    - Time Complexity: O(R), where R is number of rules
    - Space Complexity: O(R), storing edges in adjacency lists

3. **IsValidOrder**
    - Validates if update sequence respects dependency rules
    - Creates relevant subgraph for pages in update
    - Uses seen set to track visited pages
    - Time Complexity: O(P + D), where P is pages in update, D is dependencies
    - Space Complexity: O(P), for subgraph and seen set

4. **TopologicalSort**
    - Implements Kahn's algorithm for topological sorting
    - Creates subgraph of relevant pages
    - Uses in-degree tracking for dependency resolution
    - Time Complexity: O(V + E), V vertices, E edges
    - Space Complexity: O(V), for queue and result list

5. **ProcessUpdates (Part 1)**
    - Processes all updates to find valid orderings
    - Extracts middle page from valid sequences
    - Time Complexity: O(U * (P + D)), U updates, P pages per update
    - Space Complexity: O(U), storing valid middle pages

6. **ProcessInvalidUpdates (Part 2)**
    - Finds and corrects invalid update sequences
    - Uses topological sort to determine correct order
    - Time Complexity: O(U * (V + E)), U updates, V vertices, E edges
    - Space Complexity: O(V), for sorting data structures

### Algorithm

1. **Part 1 (Valid Order Check)**
    - Build dependency graph from rules
    - For each update sequence:
        1. Create subgraph of relevant dependencies
        2. Check if current order respects dependencies
        3. If valid, add middle page to result
    - Sum middle pages of valid updates

2. **Part 2 (Order Correction)**
    - Build dependency graph from rules
    - For each invalid update sequence:
        1. Identify pages involved
        2. Create subgraph of relevant dependencies
        3. Perform topological sort to find correct order
        4. Extract middle page from corrected sequence
    - Sum middle pages of corrected sequences

## Why This Solution?
- Graph representation naturally models dependencies
- Topological sort guarantees valid ordering
- Subgraph creation reduces processing overhead
- HashSet operations provide optimal lookup performance
- Memory efficient by processing one update at a time