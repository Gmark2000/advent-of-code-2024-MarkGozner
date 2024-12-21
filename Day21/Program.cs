using static Utils.Utils;

var codes = ReadInput("input.txt");

ExecuteAndMeasure("First part", () => SolvePartOne(codes));
ExecuteAndMeasure("Second part", () => SolvePartTwo(codes));

return 0;

static long SolvePartOne(string[] codes) => CalculateTotalComplexity(codes, robotLayers: 3);
static long SolvePartTwo(string[] codes) => CalculateTotalComplexity(codes, robotLayers: 26);

static long CalculateTotalComplexity(string[] codes, int robotLayers)
{
    var numericKeypad = CreateKeypad("789456123 0A");
    var directionalKeypad = CreateKeypad(" ^A<v>");
    long totalComplexity = 0;
    
    foreach (var code in codes)
    {
        var movements = GetMovementsForSequence(numericKeypad, code);
        
        for (var i = 0; i < robotLayers; i++)
            movements = TransformMovements(movements, directionalKeypad);

        var sequenceLength = movements.Values.Sum(x => (long)x);
        totalComplexity += sequenceLength * long.Parse(code[..^1]);
    }
    
    return totalComplexity;
}

static Dictionary<char, (int x, int y)> CreateKeypad(string layout) =>
    layout.Select((c, i) => (c, pos: (x: i % 3, y: i / 3)))
          .ToDictionary(x => x.c, x => x.pos);

static Dictionary<Movement, long> GetMovementsForSequence(Dictionary<char, (int x, int y)> keypad, string sequence)
{
    Dictionary<Movement, long> result = [];
    var position = keypad['A'];
    var empty = keypad[' '];

    foreach (var target in sequence)
    {
        var nextPosition = keypad[target];
        var crossesEmpty = (nextPosition.x == empty.x && position.y == empty.y) || 
                          (nextPosition.y == empty.y && position.x == empty.x);
        
        var movement = new Movement(
            (nextPosition.x - position.x, nextPosition.y - position.y),
            crossesEmpty
        );
        
        result[movement] = result.GetValueOrDefault(movement) + 1;
        position = nextPosition;
    }

    return result;
}

static Dictionary<Movement, long> TransformMovements(
    Dictionary<Movement, long> movements,
    Dictionary<char, (int x, int y)> directionalKeypad)
{
    Dictionary<Movement, long> result = [];
    
    foreach (var (movement, count) in movements)
    {
        var sequence = GetDirectionalSequence(movement);
        var transformed = GetMovementsForSequence(directionalKeypad, sequence);
        
        foreach (var (newMove, newCount) in transformed)
            result[newMove] = result.GetValueOrDefault(newMove) + newCount * count;
    }

    return result;
}

static string GetDirectionalSequence(Movement movement)
{
    var ((dx, dy), reverse) = movement;
    var moves = string.Concat(
        new string('<', -Math.Min(dx, 0)),
        new string('v', Math.Max(dy, 0)),
        new string('^', -Math.Min(dy, 0)),
        new string('>', Math.Max(dx, 0))
    );
    
    return (reverse ? string.Concat(moves.Reverse()) : moves) + 'A';
}

internal readonly record struct Movement((int x, int y) Delta, bool CrossesEmpty);