using static Utils.Utils;

const string input = "input.txt";
var inputString = File.ReadAllText(input);

ExecuteAndMeasure("First part", () => ProcessPart1(inputString));
ExecuteAndMeasure("Second part", () => ProcessPart2(inputString));
return;

static int ProcessPart1(string input)
{
    return ProcessInstructions(input, false);
}

static int ProcessPart2(string input)
{
    return ProcessInstructions(input, true);
}

static int ProcessInstructions(string input, bool enableControlInstructions)
{
    var totalSum = 0;
    var mulEnabled = true;
    var mulState = new StateProcessor();
    var doState = new StateProcessor();
    var dontState = new StateProcessor();

    char[][] controlInstructions = [
        ['d', 'o', '(', ')'],
        ['d', 'o', 'n', '\'', 't', '(', ')']
     ];

    foreach (var c in input)
    {
        ProcessMulInstruction(c, mulState, ref totalSum, mulEnabled);

        if (!enableControlInstructions) continue;
        ProcessControlInstruction(c, doState, ref mulEnabled, true, controlInstructions);
        ProcessControlInstruction(c, dontState, ref mulEnabled, false, controlInstructions);
    }

    return totalSum;
}

static void ProcessMulInstruction(char c, StateProcessor state, ref int totalSum, bool mulEnabled)
{
    switch (state.CurrentState)
    {
        case 0:
            if (c == 'm') state.CurrentState = 1;
            break;
        case 1:
            if (c == 'u') state.CurrentState = 2;
            else state.Reset();
            break;
        case 2:
            if (c == 'l') state.CurrentState = 3;
            else state.Reset();
            break;
        case 3:
            if (c == '(')
            {
                state.CurrentState = 4;
                state.FirstNumber = 0;
            }
            else state.Reset();
            break;
        case 4:
            if (char.IsDigit(c))
            {
                state.FirstNumber = state.FirstNumber * 10 + (c - '0');
            }
            else if (c == ',')
            {
                state.CurrentState = 5;
                state.SecondNumber = 0;
            }
            else state.Reset();
            break;
        case 5:
            if (char.IsDigit(c))
            {
                state.SecondNumber = state.SecondNumber * 10 + (c - '0');
            }
            else if (c == ')' && mulEnabled)
            {
                totalSum += state.FirstNumber * state.SecondNumber;
                state.Reset();
            }
            else state.Reset();
            break;
    }
}

static void ProcessControlInstruction(char c, StateProcessor state, ref bool mulEnabled, bool enableValue, char[][] controlInstructions)
{
    var pattern = enableValue ? controlInstructions[0] : controlInstructions[1];

    if (state.CurrentState >= pattern.Length) return;
    if (c == pattern[state.CurrentState])
    {
        state.CurrentState++;
        if (state.CurrentState != pattern.Length) return;
        mulEnabled = enableValue;
        state.Reset();
    }
    else
    {
        state.Reset();
        if (c == pattern[0]) state.CurrentState = 1;
    }
}

internal class StateProcessor
{
    public int CurrentState { get; set; }
    public int FirstNumber { get; set; }
    public int SecondNumber { get; set; }

    public void Reset()
    {
        CurrentState = 0;
        FirstNumber = 0;
        SecondNumber = 0;
    }
}