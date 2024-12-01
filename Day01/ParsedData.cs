namespace Day01;

public record ParsedData(
    Dictionary<int, int> FirstColumn,
    Dictionary<int, int> SecondColumn,
    int MinValue,
    int MaxValue,
    int TotalPairs
);