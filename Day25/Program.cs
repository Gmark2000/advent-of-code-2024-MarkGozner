using static Utils.Utils;

const int schematicHeight = 7;
const int schematicWidth = 5;

var (locks, keys) = ReadAndParseInput("input.txt");

ExecuteAndMeasure("First part", () => SolvePartOne(locks, keys));

// there is no second part 🎅🎅🎅

return 0;

static (Dictionary<int, int[]> locks, Dictionary<int, int[]> keys) ReadAndParseInput(string inputFile)
{
    var schematics = File.ReadAllText(inputFile).Split("\n\n");
    Dictionary<int, int[]> locks = [];
    Dictionary<int, int[]> keys = [];
    var lockCount = 0;
    var keyCount = 0;

    foreach (var schematic in schematics)
    {
        var rows = schematic.Split("\n");

        var isLock = rows[0][0] == '#';
        
        var heights = new int[schematicWidth];
        
        for (var col = 0; col < schematicWidth; col++)
        {
            if (isLock)
            {
                var height = 0;
                for (var row = rows.Length - 1; row >= 0; row--)
                {
                    if (rows[row][col] != '#') continue;
                    
                    height = row + 1;
                    break;
                }
                heights[col] = height;
            }
            else
            {
                var height = 0;
                for (var row = 0; row < rows.Length; row++)
                {
                    if (rows[row][col] != '#') continue;
                    
                    height = rows.Length - row;
                    break;
                }
                heights[col] = height;
            }
        }
        
        if (isLock)
            locks[lockCount++] = heights;
        else
            keys[keyCount++] = heights;
    }
    
    return (locks, keys);
}

static int SolvePartOne(Dictionary<int, int[]> locks, Dictionary<int, int[]> keys)
{
    var compatibleLockKeyPairsCount = 0;

    foreach (var lockEntry in locks)
    {
        foreach (var KeyEntry in keys)
        {
            if (DoesNotOverlapInAnyColumn(lockEntry.Value, KeyEntry.Value)) 
                compatibleLockKeyPairsCount++;
        }
    }
    
    return compatibleLockKeyPairsCount;
}

static bool DoesNotOverlapInAnyColumn(int[] lockHeights, int[] keyHeights)
{
    var i = 0;
    for (; i < schematicWidth; i++)
    {
        if (lockHeights[i] + keyHeights[i] > schematicHeight) return false;
    }

    return true;
}