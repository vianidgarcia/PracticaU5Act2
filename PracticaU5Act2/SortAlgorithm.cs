namespace PracticaU5Act2;

public enum DataCondition
{
    Random,
    Sorted,
    Reversed
}

public sealed record SortAlgorithm(string Name, Action<int[]> Sort, bool IsQuadratic);

public sealed record BenchmarkResult(string Algorithm, int N, DataCondition Condition, double MedianMs, int Runs, bool Correct);

public static class AlgorithmCatalog
{
    public static readonly IReadOnlyList<SortAlgorithm> All =
    [
        new SortAlgorithm("Burbuja", SortingAlgorithms.Bubble, true),
        new SortAlgorithm("Quicksort", SortingAlgorithms.Quick, false),
        new SortAlgorithm("ShellSort", SortingAlgorithms.Shell, false),
        new SortAlgorithm("Radix", SortingAlgorithms.Radix, false),
        new SortAlgorithm("Mezcla directa", SortingAlgorithms.DirectMerge, false),
        new SortAlgorithm("Mezcla natural", SortingAlgorithms.NaturalMerge, false)
    ];

    public static string ConditionName(DataCondition condition) => condition switch
    {
        DataCondition.Random => "Aleatorio",
        DataCondition.Sorted => "Ordenado",
        DataCondition.Reversed => "Inverso",
        _ => condition.ToString()
    };
}
