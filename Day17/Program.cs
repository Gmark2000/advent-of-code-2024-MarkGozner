using static Utils.Utils;

var lines = File.ReadLines("input.txt").ToArray();
var registers = (
    A: int.Parse(lines[0].Split(':')[1]),
    B: int.Parse(lines[1].Split(':')[1]),
    C: int.Parse(lines[2].Split(':')[1])        
);
var program = lines[4].Split(':')[1].Split(',').Select(int.Parse).ToArray();

ExecuteAndMeasure("First part", () => SolvePartOne(registers, program));
ExecuteAndMeasure("Second part", () => SolvePartTwo(program));

return 0;

static string SolvePartOne((int A, int B, int C) registers, int[] program) =>
    string.Join(",", RunProgram(program, new State(registers.A, registers.B, registers.C)).Output);

static long SolvePartTwo(int[] program)
{
    var loopProgram = program.Take(program.Length - 2).ToArray();
    return ReverseEngineerRecursive(loopProgram, program.Reverse().ToList(), 0);
}

static long ReverseEngineerRecursive(int[] loop, List<int> target, long aSoFar)
{
    if (target.Count == 0)
        return aSoFar;

    for (var next3Bits = 0; next3Bits < 8; next3Bits++)
    {
        var candidateA = (aSoFar * 8) + next3Bits;
        var result = RunProgram(loop, new State(candidateA, 0, 0));

        if (result.Output.Count <= 0 || result.Output.Last() != target.First()) continue;
        try
        {
            return ReverseEngineerRecursive(loop, target.Skip(1).ToList(), candidateA);
        }
        catch
        {
            // continue the reverse engineering process
        }
    }
    
    throw new InvalidOperationException("No solution found");
}

static (List<int> Output, State FinalState) RunProgram(int[] program, State initialState)
{
    var output = new List<int>();
    var ip = 0;
    var state = initialState;

    while (ip < program.Length - 1)
    {
        var (opcode, operand) = (program[ip], program[ip + 1]);
        var combo = GetComboValue(operand, state);
        
        state = ExecuteInstruction(opcode, operand, combo, state);
        
        if (opcode == 5)
            output.Add(combo % 8);
            
        ip = opcode == 3 && state.A != 0 ? operand : ip + 2;
    }

    return (output, state);
}

static int GetComboValue(int operand, State state) => operand switch
{
    <= 3 => operand,
    4 => (int)(state.A % 8),
    5 => (int)(state.B % 8),
    6 => (int)(state.C % 8),
    _ => throw new InvalidOperationException($"Invalid operand: {operand}")
};

static State ExecuteInstruction(int opcode, int operand, int combo, State state) => opcode switch
{
    0 => state with { A = state.A / (1L << combo) },
    1 => state with { B = state.B ^ operand },
    2 => state with { B = combo % 8 },
    3 => state,
    4 => state with { B = state.B ^ state.C },
    5 => state,
    6 => state with { B = state.A / (1L << combo) },
    7 => state with { C = state.A / (1L << combo) },
    _ => throw new InvalidOperationException($"Invalid opcode: {opcode}")
};

internal record struct State(long A, long B, long C);