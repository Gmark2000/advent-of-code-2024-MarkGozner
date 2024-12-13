using static Utils.Utils;

const decimal OFFSET = 10000000000000m;

var input = File.ReadAllText("input.txt");
var machines = GetMachines(input);

ExecuteAndMeasure("First part", () => SolvePartOne(machines));
ExecuteAndMeasure("Second part", () => SolvePartTwo(machines));

return 0;

string[] ParseInput(string inputString) => 
    inputString.Split("\n");

Machine ParseMachine(string[] lines, int index)
{
    var aLine = lines[index].Split("X+")[1].Split(", Y+");
    var bLine = lines[index + 1].Split("X+")[1].Split(", Y+");
    var prizeLine = lines[index + 2].Split("X=")[1].Split(", Y=");
    
    return new Machine(
        ButtonA: (int.Parse(aLine[0]), int.Parse(aLine[1])),
        ButtonB: (int.Parse(bLine[0]), int.Parse(bLine[1])),
        Prize: (int.Parse(prizeLine[0]), int.Parse(prizeLine[1]))
    );
}

List<Machine> GetMachines(string inputString)
{
    var lines = ParseInput(inputString);
    var machines = new List<Machine>();
    
    for (var i = 0; i < lines.Length; i += 4)
    {
        machines.Add(ParseMachine(lines, i));
    }
    
    return machines;
}

(bool possible, int tokens) SolveMachine(Machine machine)
{
    for (var a = 0; a <= 100; a++)
    {
        for (var b = 0; b <= 100; b++)
        {
            var x = a * machine.ButtonA.x + b * machine.ButtonB.x;
            var y = a * machine.ButtonA.y + b * machine.ButtonB.y;
            
            if (x == machine.Prize.x && y == machine.Prize.y)
            {
                return (true, a * 3 + b); // A costs 3 tokens, B costs 1
            }
        }
    }
    
    return (false, 0);
}

int SolvePartOne(List<Machine> machines)
{
    var totalTokens = 0;
    
    foreach (var machine in machines)
    {
        var (possible, tokens) = SolveMachine(machine);
        if (possible)
        {
            totalTokens += tokens;
        }
    }
    
    return totalTokens;
}

(bool found, decimal a, decimal b) SolveSystem(decimal ax, decimal ay, decimal bx, decimal by, decimal px, decimal py)
{
    var determinant = ax * by - ay * bx;
    
    if (determinant == 0)
        return (false, 0, 0);
        
    var a = (px * by - py * bx) / determinant;
    var b = (ax * py - ay * px) / determinant;
    
    if (a != Math.Floor(a) || b != Math.Floor(b))
        return (false, 0, 0);
    
    if (a < 0 || b < 0)
        return (false, 0, 0);
        
    return (true, a, b);
}

(bool possible, decimal tokens)? SolveMachinePartTwo(Machine m)
{
    var targetX = m.Prize.x + OFFSET;
    var targetY = m.Prize.y + OFFSET;
    
    var (found, pressA, pressB) = SolveSystem(
        m.ButtonA.x, m.ButtonA.y,
        m.ButtonB.x, m.ButtonB.y,
        targetX, targetY
    );
    
    if (!found)
        return null;
        
    return (true, pressA * 3 + pressB);
}

decimal SolvePartTwo(List<Machine> machines)
{
    decimal totalTokens = 0;
    
    foreach (var machine in machines)
    {
        var result = SolveMachinePartTwo(machine);
        if (result?.possible == true)
        {
            totalTokens += result.Value.tokens;
        }
    }
    
    return totalTokens;
}
