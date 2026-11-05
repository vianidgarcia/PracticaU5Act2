namespace PracticaU5Act2;

public static class DataGenerator
{
    public const int MaxValue = 1_000_000;

    public static int[] Random(int n, int seed)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "The size cannot be negative.");

        var rng = new System.Random(seed);
        int[] data = new int[n];
        for (int i = 0; i < n; i++)
            data[i] = rng.Next(0, MaxValue);
        return data;
    }

    // The three conditions are built from the same random values so the comparison is fair.
    public static int[] ForCondition(int[] randomData, DataCondition condition)
    {
        int[] copy = (int[])randomData.Clone();
        switch (condition)
        {
            case DataCondition.Sorted:
                Array.Sort(copy);
                break;
            case DataCondition.Reversed:
                Array.Sort(copy);
                Array.Reverse(copy);
                break;
        }
        return copy;
    }
}
