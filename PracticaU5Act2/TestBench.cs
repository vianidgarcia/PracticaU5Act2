namespace PracticaU5Act2;

public sealed record TestResult(string Name, bool Passed, string Detail);

public static class TestBench
{
    public static List<TestResult> RunAll()
    {
        var results = new List<TestResult>
        {
            SortsLike("Arreglo vacío", [[]]),
            SortsLike("Un solo elemento", [[7]]),
            SortsLike("Dos elementos (orden y desorden)", [[1, 2], [2, 1]]),
            SortsLike("Todos los elementos iguales", [Enumerable.Repeat(5, 500).ToArray()]),
            SortsLike("Ya ordenado (1 000)", [Enumerable.Range(0, 1000).ToArray()]),
            SortsLike("Orden inverso (1 000)", [Enumerable.Range(0, 1000).Reverse().ToArray()]),
            SortsLike("Muchos duplicados (5 000 con valores 0-99)", [RandomValues(5000, 100, 11)]),
            SortsLike("Negativos y extremos del tipo int", [[0, -5, int.MaxValue, int.MinValue, 3, -5, 42, int.MinValue]]),
            SortsLike("Tamaños que no son potencia de 2", RandomArrays([3, 7, 100, 1023, 1025])),
            SortsLike("Aleatorio grande (20 000)", [DataGenerator.Random(20_000, 5)]),
            GeneratorConditions(),
            MedianHelper(),
            RunsPolicy(),
            MeasureKeepsInput()
        };
        return results;
    }

    private static int[] RandomValues(int n, int bound, int seed)
    {
        var rng = new Random(seed);
        int[] values = new int[n];
        for (int i = 0; i < n; i++)
            values[i] = rng.Next(bound);
        return values;
    }

    // One random array per requested size; the size is also used as the seed.
    private static int[][] RandomArrays(int[] sizes)
    {
        int[][] arrays = new int[sizes.Length][];
        for (int i = 0; i < sizes.Length; i++)
            arrays[i] = RandomValues(sizes[i], 100000, sizes[i]);
        return arrays;
    }

    private static bool IsAscending(int[] data)
    {
        for (int i = 1; i < data.Length; i++)
        {
            if (data[i - 1] > data[i])
                return false;
        }
        return true;
    }

    private static bool IsDescending(int[] data)
    {
        for (int i = 1; i < data.Length; i++)
        {
            if (data[i - 1] < data[i])
                return false;
        }
        return true;
    }

    private static SortAlgorithm Find(string name)
    {
        foreach (SortAlgorithm algorithm in AlgorithmCatalog.All)
        {
            if (algorithm.Name == name)
                return algorithm;
        }
        throw new InvalidOperationException($"Algorithm not found: {name}");
    }

    // Every algorithm must produce exactly what Array.Sort produces for each input.
    private static TestResult SortsLike(string name, int[][] inputs)
    {
        var failed = new List<string>();
        foreach (SortAlgorithm algorithm in AlgorithmCatalog.All)
        {
            foreach (int[] input in inputs)
            {
                int[] actual = (int[])input.Clone();
                int[] expected = (int[])input.Clone();
                Array.Sort(expected);

                try
                {
                    algorithm.Sort(actual);
                }
                catch (Exception ex)
                {
                    failed.Add($"{algorithm.Name} ({ex.GetType().Name})");
                    break;
                }

                if (!actual.AsSpan().SequenceEqual(expected))
                {
                    failed.Add(algorithm.Name);
                    break;
                }
            }
        }

        return new TestResult(name, failed.Count == 0,
            failed.Count == 0 ? "Los 6 algoritmos coinciden con Array.Sort" : "Fallan: " + string.Join(", ", failed));
    }

    private static TestResult GeneratorConditions()
    {
        int[] random = DataGenerator.Random(1000, 3);
        int[] sorted = DataGenerator.ForCondition(random, DataCondition.Sorted);
        int[] reversed = DataGenerator.ForCondition(random, DataCondition.Reversed);

        int[] reversedSorted = (int[])reversed.Clone();
        Array.Sort(reversedSorted);

        bool ascending = IsAscending(sorted);
        bool descending = IsDescending(reversed);
        bool sameValues = reversedSorted.AsSpan().SequenceEqual(sorted);
        bool deterministic = DataGenerator.Random(1000, 3).AsSpan().SequenceEqual(random);

        bool ok = ascending && descending && sameValues && deterministic;
        return new TestResult("El generador crea las tres condiciones con los mismos valores", ok,
            ok ? "Ordenado, inverso y repetible con la misma semilla" : "Alguna condición no se generó bien");
    }

    private static TestResult MedianHelper()
    {
        bool ok = Statistics.Median([3.0, 1.0, 2.0]) == 2.0 && Statistics.Median([4.0, 1.0, 3.0, 2.0]) == 2.5;
        return new TestResult("Mediana con cantidad impar y par", ok, ok ? "2 y 2.5, como se esperaba" : "La mediana es incorrecta");
    }

    private static TestResult RunsPolicy()
    {
        SortAlgorithm bubble = Find("Burbuja");
        SortAlgorithm quick = Find("Quicksort");

        bool ok = SortBenchmark.RunsFor(bubble, 10_000) == SortBenchmark.NormalRuns
            && SortBenchmark.RunsFor(bubble, 50_000) == 1
            && SortBenchmark.RunsFor(bubble, 100_000) == 1
            && SortBenchmark.RunsFor(quick, 100_000) == SortBenchmark.NormalRuns;
        return new TestResult("Burbuja se mide una sola vez desde 50 000 datos", ok,
            ok ? "Las demás combinaciones usan la mediana de 5 corridas" : "La política de corridas no coincide");
    }

    private static TestResult MeasureKeepsInput()
    {
        int[] data = DataGenerator.Random(3000, 9);
        int[] original = (int[])data.Clone();
        int[] expected = DataGenerator.ForCondition(data, DataCondition.Sorted);

        bool allCorrect = true;
        foreach (SortAlgorithm algorithm in AlgorithmCatalog.All)
        {
            BenchmarkResult result = SortBenchmark.Measure(algorithm, data, expected, DataCondition.Random);
            allCorrect &= result.Correct && result.MedianMs >= 0;
        }

        bool untouched = data.AsSpan().SequenceEqual(original);

        bool ok = untouched && allCorrect;
        return new TestResult("La medición ordena copias y no altera los datos de origen", ok,
            ok ? "Datos originales intactos y resultados verificados" : "Se alteraron los datos o hubo un resultado incorrecto");
    }
}
