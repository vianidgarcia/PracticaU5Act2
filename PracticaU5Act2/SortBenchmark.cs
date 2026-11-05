using System.Diagnostics;

namespace PracticaU5Act2;

public static class SortBenchmark
{
    public const int NormalRuns = 5;
    public const int SlowSizeThreshold = 50_000;
    public const int WarmUpSize = 2_000;
    public static readonly int[] Sizes = [10_000, 50_000, 100_000];

    // Quadratic algorithms with large inputs take seconds per run, so they are measured once.
    public static int RunsFor(SortAlgorithm algorithm, int n) =>
        algorithm.IsQuadratic && n >= SlowSizeThreshold ? 1 : NormalRuns;

    public static BenchmarkResult Measure(SortAlgorithm algorithm, int[] data, int[] expected, DataCondition condition)
    {
        // A small warm-up so the JIT compilation is not counted in the measured runs.
        algorithm.Sort(DataGenerator.Random(WarmUpSize, 1));
        GC.Collect();

        int runs = RunsFor(algorithm, data.Length);
        double[] times = new double[runs];
        bool correct = true;

        for (int r = 0; r < runs; r++)
        {
            int[] copy = (int[])data.Clone();

            long start = Stopwatch.GetTimestamp();
            algorithm.Sort(copy);
            times[r] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;

            correct &= copy.AsSpan().SequenceEqual(expected);
        }

        return new BenchmarkResult(algorithm.Name, data.Length, condition, Statistics.Median(times), runs, correct);
    }

    // Runs every algorithm for every size and condition and reports each result as soon as it exists.
    public static void RunAll(bool includeBubble, int seed, IProgress<string> status, IProgress<BenchmarkResult> results,
        CancellationToken cancel)
    {
        foreach (int n in Sizes)
        {
            int[] randomData = DataGenerator.Random(n, seed);
            int[] expected = DataGenerator.ForCondition(randomData, DataCondition.Sorted);

            foreach (DataCondition condition in Enum.GetValues<DataCondition>())
            {
                int[] data = DataGenerator.ForCondition(randomData, condition);

                foreach (SortAlgorithm algorithm in AlgorithmCatalog.All)
                {
                    if (!includeBubble && algorithm.IsQuadratic)
                        continue;

                    cancel.ThrowIfCancellationRequested();
                    status.Report($"Midiendo {algorithm.Name} con {n:N0} datos ({AlgorithmCatalog.ConditionName(condition).ToLower()})...");
                    results.Report(Measure(algorithm, data, expected, condition));
                }
            }
        }
    }
}
