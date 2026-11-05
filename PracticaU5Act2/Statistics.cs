namespace PracticaU5Act2;

public static class Statistics
{
    public static double Median(IReadOnlyList<double> values)
    {
        if (values.Count == 0)
            throw new ArgumentException("There are no values.", nameof(values));

        double[] sorted = new double[values.Count];
        for (int i = 0; i < sorted.Length; i++)
            sorted[i] = values[i];
        Array.Sort(sorted);

        int middle = sorted.Length / 2;
        return sorted.Length % 2 == 1 ? sorted[middle] : (sorted[middle - 1] + sorted[middle]) / 2.0;
    }
}
