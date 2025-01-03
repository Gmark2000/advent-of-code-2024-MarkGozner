# Advent of Code 2024 - Day 3: Mull It Over

## Algorithm Explanation

The solution uses a state machine approach to process corrupted memory instructions, particularly focusing on multiplication operations and control instructions.

### Core Components

#### StateProcessor Class
```csharp
class StateProcessor
{
public int CurrentState { get; set; }    // Tracks current state in the state machine
public int FirstNumber { get; set; }     // Stores first number in multiplication
public int SecondNumber { get; set; }    // Stores second number in multiplication
public void Reset()                      // Resets all state values to 0
}
```

### Main Functions

1. `ProcessPart1(string input)`
- Handles basic multiplication instructions without control statements (without do()/don't() statements)
- Processes input string character by character
- Returns sum of all valid multiplications
- Passes `false` to ProcessInstructions to disable control instructions

2. `ProcessPart2(string input)`
- Extends Part 1 by adding support for control instructions (do/don't)
- Passes `true` to ProcessInstructions to enable control instructions
- Returns sum of only enabled multiplications

3. `ProcessInstructions(string input, bool enableControlInstructions)`
- Core processing function that handles both parts
- Maintains three state processors:
* mulState: Tracks multiplication instruction state
* doState: Tracks 'do()' instruction state
* dontState: Tracks 'don't()' instruction state
- Uses control instructions array:
```csharp
char[][] controlInstructions = [
['d', 'o', '(', ')'],              // do instruction characters
['d', 'o', 'n', '\'', 't', '(', ')'] // don't instruction characters
];
```

4. `ProcessMulInstruction(char c, StateProcessor state, ref int totalSum, bool mulEnabled)`
- State machine for processing multiplication instructions
- States:
* 0: Looking for 'm'
* 1: Found 'm', looking for 'u'
* 2: Found 'u', looking for 'l'
* 3: Found 'l', looking for '('
* 4: Reading first number
* 5: Reading second number and looking for ')'
- Only adds to totalSum if mulEnabled is true

5. `ProcessControlInstruction(char c, StateProcessor state, ref bool mulEnabled, bool enableValue, char[][] controlInstructions)`
- Handles do() and don't() instructions
- Uses pattern matching to identify valid control instructions
- Updates mulEnabled flag based on the instruction type

### State Machine Logic

#### Multiplication Pattern Recognition
The state machine recognizes valid multiplication patterns:
- Must match exactly: mul(X,Y) where X and Y are 1-3 digit numbers
- No spaces or other characters allowed within the pattern
- Only processes if multiplication is currently enabled

#### Control Instructions
- do(): Enables multiplication operations
- don't(): Disables multiplication operations
- Only the most recent control instruction applies
- Multiplications start enabled by default

### Example Processing
```
Input: xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
Part 1 Output: 161 (processes all valid multiplications)
Part 2 Output: 48 (only processes enabled multiplications)
```