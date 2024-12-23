using static Utils.Utils;

var computers = ReadAndParseInput("input.txt");
ExecuteAndMeasure("First part", () => SolvePartOne(computers));
ExecuteAndMeasure("Second part", () => SolvePartTwo(computers));

return 0;

static Dictionary<string, HashSet<string>> ReadAndParseInput(string inputFile)
{
    Dictionary<string, HashSet<string>> computers = [];
    var lines = ReadInput(inputFile);

    foreach (var line in lines)
    {
        var items = line.Split("-");
        var comp1 = items[0];
        var comp2 = items[1];
        
        if (!computers.TryGetValue(comp1, out var hashSet1))
        {
            hashSet1 = [];
            computers[comp1] = hashSet1;
        }
        
        if (!computers.TryGetValue(comp2, out var hashSet2))
        {
            hashSet2 = [];
            computers[comp2] = hashSet2;
        }

        hashSet1.Add(comp2);
        hashSet2.Add(comp1);
    }
    
    return computers;
}

static int SolvePartOne(Dictionary<string, HashSet<string>> computers)
{
    var uniqueTriples = new HashSet<string>();
    var computerList = computers.Keys.ToList();
    
    foreach (var comp1 in computerList)
    {
        foreach (var comp2 in computers[comp1])
        {
            foreach (var comp3 in computers[comp2])
            {
                if (!computers[comp1].Contains(comp3)) continue;
                if (!comp1.StartsWith('t') && !comp2.StartsWith('t') && !comp3.StartsWith('t')) continue;

                var triple = string.Join(",", new[] { comp1, comp2, comp3 }.OrderBy(x => x));
                uniqueTriples.Add(triple);
            }
        }
    }
    
    return uniqueTriples.Count;
}

static string SolvePartTwo(Dictionary<string, HashSet<string>> computers)
{
    HashSet<string> maxConnectedComputers = [];
    
    foreach (var startComputer in computers.Keys)
    {
        HashSet<string> connectedComputers = [startComputer];
        HashSet<string> potentialConnections = [..computers[startComputer]];

        while (potentialConnections.Count > 0)
        {
            var nextComputer = potentialConnections.First();
            potentialConnections.Remove(nextComputer);
            
            var isConnectedToAll = true;
            foreach (var computer in connectedComputers)
            {
                if (computers[nextComputer].Contains(computer)) continue;
                
                isConnectedToAll = false;
                break;
            }

            if (!isConnectedToAll) continue;
            
            connectedComputers.Add(nextComputer);
            potentialConnections.IntersectWith(computers[nextComputer]);
        }

        if (connectedComputers.Count > maxConnectedComputers.Count)
        {
            maxConnectedComputers = connectedComputers;
        }
    }
    
    return string.Join(",", maxConnectedComputers.OrderBy(x => x));
}
