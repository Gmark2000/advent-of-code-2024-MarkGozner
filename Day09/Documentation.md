# Advent of Code 2023 - Day 9: Disk Fragmenter

## Problem Overview
1. Part 1: Moving individual file blocks from right to left to create contiguous space
2. Part 2: Moving whole files to optimize disk space while minimizing fragmentation

## Problem Details

### Part 1: Block-by-Block Movement
- Files are represented by consecutive blocks with the same ID
- Blocks must be moved one at a time from right to left
- Each block should be moved to the leftmost available free space
- Movement continues until no gaps remain between file blocks
- File IDs start at 0 and increase with each file in the input

### Part 2: Whole File Movement
- Files must be moved as complete units
- Files are processed in order of decreasing file ID (highest to lowest)
- Each file can only be moved once
- Files only move if there's enough contiguous space to their left
- Files that can't find suitable space remain in place

## Implementation Details

### Core Functions

1. **MapToBlocks**
    ```csharp
    static List<int> MapToBlocks(string input)
    ```
    - Converts the input string into a list of block IDs
    - -1 represents free space, non-negative integers represent file blocks
    - Time Complexity: O(N), where N is total blocks
    - Space Complexity: O(N)

2. **CompactBlocks**
    ```csharp
    static void CompactBlocks(List<int> blocks)
    ```
    - Implements Part 1 logic
    - Moves blocks one at a time from right to left
    - Continues until no more moves are possible
    - Time Complexity: O(N²), where N is total blocks

3. **GetFileInfos**
    ```csharp
    static List<FileInfo> GetFileInfos(List<int> blocks)
    ```
    - Analyzes blocks to extract file information
    - Groups consecutive blocks with same ID
    - Returns list of FileInfo objects with Id, Size, and StartPosition
    - Time Complexity: O(N)
    - Space Complexity: O(F), where F is number of files

4. **CompactWholeFiles**
    ```csharp
    static void CompactWholeFiles(List<int> blocks)
    ```
    - Implements Part 2 logic
    - Processes files in decreasing ID order
    - Finds leftmost suitable space for each file
    - Moves entire files at once
    - Time Complexity: O(N * F), where F is number of files

5. **CalculateChecksum**
    ```csharp
    static long CalculateChecksum(List<int> blocks)
    ```
    - Calculates final filesystem checksum
    - Multiplies each block's position by its file ID
    - Sums all products for non-empty blocks
    - Time Complexity: O(N)

### Helper Classes

1. **FileInfo**
    ```csharp
    public class FileInfo
    {
        public int Id { get; init; }
        public int Size { get; init; }
        public int StartPosition { get; init; }
    }
    ```
    - Stores information about a single file
    - Used in Part 2 for whole-file operations

### Algorithm Details

1. **Part 1: Block Movement Process**
   ```
   1. Convert input string to blocks
   2. While moves are possible:
      a. Find rightmost file block
      b. Find leftmost empty space before it
      c. Move block if space found
   3. Calculate checksum
   ```

2. **Part 2: File Movement Process**
   ```
   1. Convert input string to blocks
   2. Extract file information
   3. Sort files by ID (descending)
   4. For each file:
      a. Find leftmost span of sufficient free space
      b. Move entire file if space found
   5. Calculate checksum
   ```
## Results
- Part 1 (filesystem checksum): 6398252054886
- Part 2 (filesystem checksum): 6415666220005