using static Utils.Utils;

var (initialValues, gates) = ReadAndParseInput("input.txt");

ExecuteAndMeasure("First part", () => SolvePartOne(initialValues, gates));
ExecuteAndMeasure("Second part", () => SolvePartTwo(gates));

return 0;

static (Dictionary<string, bool> initialValues, List<Gate> gates) ReadAndParseInput(string inputFile)
{
    var initialValues = new Dictionary<string, bool>();
    var gates = new List<Gate>();
    var lines = File.ReadAllLines(inputFile);
    var parsingGates = false;

    foreach (var line in lines)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            parsingGates = true;
            continue;
        }

        if (!parsingGates)
        {
            var parts = line.Split(':');
            var wire = parts[0].Trim();
            var value = int.Parse(parts[1].Trim());
            initialValues[wire] = value == 1;
        }
        else
        {
            var parts = line.Split("->");
            var outputWire = parts[1].Trim();
            var inputs = parts[0].Trim().Split(' ');

            GateType type;

            if (inputs.Contains("AND"))
            {
                type = GateType.And;
            }
            else if (inputs.Contains("OR"))
            {
                type = GateType.Or;
            }
            else
            {
                type = GateType.Xor;
            }

            gates.Add(new Gate(type, inputs[0], inputs[2], outputWire));
        }
    }

    return (initialValues, gates);
}

static long SolvePartOne(Dictionary<string, bool> initialValues, List<Gate> gates)
{
    var wireValues = CalculateWireValues(initialValues, gates);
    return CalculateZWiresValue(wireValues);
}

static Dictionary<string, bool> CalculateWireValues(Dictionary<string, bool> initialValues, List<Gate> gates)
{
    var wireValues = new Dictionary<string, bool>(initialValues);
    var processedGates = new HashSet<Gate>();
    
    while (processedGates.Count < gates.Count)
    {
        foreach (var gate in gates)
        {
            if (processedGates.Contains(gate)) continue;
            
            if (!wireValues.ContainsKey(gate.Input1) || !wireValues.ContainsKey(gate.Input2))
                continue;

            wireValues[gate.Output] = SimulateGate(gate.Type, wireValues[gate.Input1], wireValues[gate.Input2]);
            processedGates.Add(gate);
        }
    }
    
    return wireValues;
}

static bool SimulateGate(GateType type, bool input1, bool input2) => type switch
{
    GateType.And => input1 && input2,
    GateType.Or => input1 || input2,
    GateType.Xor => input1 ^ input2,
    _ => throw new ArgumentException("Invalid gate type")
};

static long CalculateZWiresValue(Dictionary<string, bool> wireValues)
{
    var zWires = wireValues.Keys
        .Where(k => k.StartsWith('z'))
        .OrderByDescending(k => int.Parse(k[1..]))
        .ToList();

    long result = 0;
    foreach (var wire in zWires)
    {
        result = (result << 1) | (uint)(wireValues[wire] ? 1 : 0);
    }
    
    return result;
}

static string SolvePartTwo(List<Gate> gates)
{
    var faultyGates = new HashSet<Gate>();
    var lastZGate = gates.Where(g => g.Output.StartsWith('z'))
        .OrderByDescending(g => g.Output)
        .First();
    
    foreach (var gate in gates)
    {
        var isFaulty = false;
        
        if (gate.Output.StartsWith('z') && gate.Output != lastZGate.Output)
        {
            isFaulty = gate.Type != GateType.Xor;
        }
        else if (!gate.Output.StartsWith('z') && !IsInputWire(gate.Input1) && !IsInputWire(gate.Input2))
        {
            isFaulty = gate.Type == GateType.Xor;
        }
        else if (IsInputWire(gate.Input1) && IsInputWire(gate.Input2) && !AreInputsFirstBit(gate.Input1, gate.Input2))
        {
            var output = gate.Output;
            var expectedNextType = gate.Type == GateType.Xor ? GateType.Xor : GateType.Or;
            
            var feedsIntoExpectedGate = gates.Any(other => 
                other != gate && 
                (other.Input1 == output || other.Input2 == output) && 
                other.Type == expectedNextType);
                
            isFaulty = !feedsIntoExpectedGate;
        }
        
        if (isFaulty)
        {
            faultyGates.Add(gate);
        }
    }

    return string.Join(",", faultyGates.Select(g => g.Output).OrderBy(w => w));
}

static bool IsInputWire(string wire) => wire.StartsWith('x') || wire.StartsWith('y');

static bool AreInputsFirstBit(string input1, string input2) => 
    input1.EndsWith("00") && input2.EndsWith("00");

internal record Gate(GateType Type, string Input1, string Input2, string Output);

internal enum GateType
{
    And,
    Or,
    Xor
}