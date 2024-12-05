using static Utils.Utils;

var (rules, updates) = ParseInputFile("input.txt");
var graph = BuildDependencyGraph(rules);

ExecuteAndMeasure("First part", () => ProcessUpdates(updates, graph));
ExecuteAndMeasure("Second part", () => ProcessInvalidUpdates(updates, graph));

return 0;

static (List<string> rules, List<string> updates) ParseInputFile(string filePath)
{
    var rules = new List<string>();
    var updates = new List<string>();
    var isReadingUpdates = false;

    foreach (var line in File.ReadLines(filePath))
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            isReadingUpdates = true;
            continue;
        }

        if (!isReadingUpdates)
        {
            rules.Add(line);
        }
        else
        {
            updates.Add(line);
        }
    }

    return (rules, updates);
}

static Dictionary<int, HashSet<int>> BuildDependencyGraph(List<string> rules)
{
    var graph = new Dictionary<int, HashSet<int>>();
    
    foreach (var rule in rules)
    {
        var parts = rule.Split('|');
        var before = int.Parse(parts[0]);
        var after = int.Parse(parts[1]);
        
        if (!graph.ContainsKey(before))
            graph[before] = [];
        if (!graph.ContainsKey(after))
            graph[after] = [];
            
        graph[before].Add(after);
    }
    
    return graph;
}

static bool IsValidOrder(List<int> update, Dictionary<int, HashSet<int>> graph)
{
    var relevantNodes = new HashSet<int>(update);
    var subgraph = new Dictionary<int, HashSet<int>>();
    
    foreach (var page in update)
    {
        if (!subgraph.ContainsKey(page))
            subgraph[page] = [];

        if (!graph.TryGetValue(page, out var value)) continue;
        
        foreach (var next in value.Where(next => relevantNodes.Contains(next)))
        {
            subgraph[page].Add(next);
        }
    }
    
    var seen = new HashSet<int>();
    
    foreach (var currentPage in update)
    {
        seen.Add(currentPage);

        if (!subgraph.TryGetValue(currentPage, value: out var value)) continue;
        
        if (value.Any(dependency => seen.Contains(dependency)))
        {
            return false;
        }
    }
    
    return true;
}

static int ProcessUpdates(List<string> updates, Dictionary<int, HashSet<int>> graph)
{
    var result = new List<int>();

    foreach (var update in updates)
    {
        var pages = update.Split(',').Select(int.Parse).ToList();

        if (IsValidOrder(pages, graph))
        {
            result.Add(pages[pages.Count / 2]);
        }
    }

    return result.Sum();
}

static List<int> TopologicalSort(HashSet<int> pages, Dictionary<int, HashSet<int>> fullGraph)
{
    var subGraph = new Dictionary<int, HashSet<int>>();
    var inDegree = new Dictionary<int, int>();
    
    foreach (var page in pages)
    {
        subGraph[page] = [];
        inDegree[page] = 0;
    }
    
    foreach (var page in pages)
    {
        if (!fullGraph.TryGetValue(page, out var value)) continue;
        
        foreach (var next in value)
        {
            if (!pages.Contains(next)) continue;
            
            subGraph[page].Add(next);
            inDegree[next] = inDegree[next] + 1;
        }
    }
    
    var queue = new Queue<int>();
    foreach (var pair in inDegree)
    {
        if (pair.Value == 0)
        {
            queue.Enqueue(pair.Key);
        }
    }
    
    var result = new List<int>();
    
    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        result.Add(current);
        
        foreach (var next in subGraph[current])
        {
            inDegree[next]--;
            if (inDegree[next] == 0)
            {
                queue.Enqueue(next);
            }
        }
    }
    
    return result;
}

static int ProcessInvalidUpdates(List<string> updates, Dictionary<int, HashSet<int>> graph)
{
    var sum = 0;
    
    foreach (var update in updates)
    {
        var pages = update.Split(',').Select(int.Parse).ToList();

        if (IsValidOrder(pages, graph)) continue;
        
        var correctedOrder = TopologicalSort([..pages], graph);
        
        var middlePage = correctedOrder[correctedOrder.Count / 2];
        sum += middlePage;
    }
    
    return sum;
}