using static Utils.Utils;

const int east = 0, south = 1, west = 2, north = 3;
const int moveCost = 1;
const int rotateCost = 1000;

const char wallChar = '#';
const char startChar = 'S';
const char endChar = 'E';

var grid = File.ReadAllLines("input.txt");
var (start, end) = FindStartAndEnd(grid);
ExecuteAndMeasure(
    "First part",
    "Second part",
    () => SolveMaze(grid, start, end),
    "First and second part");

return 0;

static (Point start, Point end) FindStartAndEnd(string[] grid)
{
    Point start = default, end = default;
    for (var y = 0; y < grid.Length; y++)
    {
        for (var x = 0; x < grid[y].Length; x++)
        {
            if (grid[y][x] == startChar) start = new Point(x, y);
            if (grid[y][x] == endChar) end = new Point(x, y);
        }
    }
    return (start, end);
}

static (int minCost, int optimalTileCount) SolveMaze(string[] grid, Point start, Point end)
{
    var width = grid[0].Length;
    var height = grid.Length;

    var searchState = InitializeSearchState(start);
    var result = ExploreAllPaths(searchState, grid, start, end, width, height);
    ExploreOptimalPaths(result.optimalTiles, result.pathsToExplore);

    return (result.minCost, result.optimalTiles.Count);
}

static SearchState InitializeSearchState(Point start)
{
    var state = new SearchState
    {
        Queue = new PriorityQueue<SearchNode, int>(),
        CostToState = [],
        OptimalTiles = [],
        PathsToExplore = [],
        MinCost = int.MaxValue
    };

    var initialNode = new SearchNode(new State(start, east), [start], 0);
    state.Queue.Enqueue(initialNode, 0);
    return state;
}

static (int minCost, HashSet<Point> optimalTiles, List<HashSet<Point>> pathsToExplore)
ExploreAllPaths(SearchState state, string[] grid, Point start, Point end, int width, int height)
{
    while (state.Queue.Count > 0)
    {
        var current = state.Queue.Dequeue();

        if (!ShouldContinueExploration(state, current))
            continue;

        state.CostToState[current.CurrentState] = current.Cost;

        if (HasReachedEnd(state, current, end))
            continue;

        ExploreNextStates(state, current, grid, width, height);
    }

    return (state.MinCost, state.OptimalTiles, state.PathsToExplore);
}

static bool ShouldContinueExploration(SearchState state, SearchNode current)
{
    if (current.Cost > state.MinCost)
        return false;

    if (!state.CostToState.TryGetValue(current.CurrentState, out var knownCost)) return true;
    
    if (current.Cost > knownCost)
        return false;

    if (current.Cost != knownCost) return true;
    
    foreach (var tile in current.Path)
        state.OptimalTiles.Add(tile);

    return true;
}

static bool HasReachedEnd(SearchState state, SearchNode current, Point end)
{
    if (!current.CurrentState.Pos.Equals(end))
        return false;

    if (current.Cost < state.MinCost)
    {
        state.MinCost = current.Cost;
        state.OptimalTiles.Clear();
        state.PathsToExplore.Clear();
    }

    if (current.Cost != state.MinCost) return true;
    
    state.PathsToExplore.Add(current.Path);
    foreach (var tile in current.Path)
        state.OptimalTiles.Add(tile);

    return true;
}

static void ExploreNextStates(SearchState state, SearchNode current, string[] grid, int width, int height)
{
    foreach (var next in GetNextStates(current.CurrentState, grid, width, height))
    {
        var newCost = current.Cost + (next.isRotation ? rotateCost : moveCost);

        if (newCost > state.MinCost) continue;
        
        var newPath = new HashSet<Point>(current.Path) { next.state.Pos };
        var nextNode = new SearchNode(next.state, newPath, newCost);
        state.Queue.Enqueue(nextNode, newCost);
    }
}

static IEnumerable<(State state, bool isRotation)> GetNextStates(
    State current, string[] grid, int width, int height)
{
    List<(State state, bool isRotation)> nextStates = [
        (current with { Dir = (current.Dir + 3) % 4 }, true),
        (current with { Dir = (current.Dir + 1) % 4 }, true)
    ];

    var (dx, dy) = GetDirectionOffset(current.Dir);
    var newPos = new Point(current.Pos.X + dx, current.Pos.Y + dy);

    if (IsValidMove(newPos, grid, width, height))
    {
        nextStates.Add((current with { Pos = newPos }, false));
    }

    return nextStates;
}

static (int dx, int dy) GetDirectionOffset(int direction) => direction switch
{
    east => (1, 0),
    south => (0, 1),
    west => (-1, 0),
    north => (0, -1),
    _ => throw new NotImplementedException()
};

static bool IsValidMove(Point p, string[] grid, int width, int height) =>
    p.X >= 0 && p.X < width &&
    p.Y >= 0 && p.Y < height &&
    grid[p.Y][p.X] != wallChar;

static void ExploreOptimalPaths(HashSet<Point> optimalTiles, List<HashSet<Point>> pathsToExplore)
{
    foreach (var path in pathsToExplore)
    {
        foreach (var tile in path)
            optimalTiles.Add(tile);
    }
}

internal record struct Point(int X, int Y);
internal record struct State(Point Pos, int Dir);
internal record struct SearchNode(State CurrentState, HashSet<Point> Path, int Cost);

internal class SearchState
{
    public required PriorityQueue<SearchNode, int> Queue { get; init; }
    public required Dictionary<State, int> CostToState { get; init; }
    public required HashSet<Point> OptimalTiles { get; init; }
    public required List<HashSet<Point>> PathsToExplore { get; init; }
    public int MinCost { get; set; }
}