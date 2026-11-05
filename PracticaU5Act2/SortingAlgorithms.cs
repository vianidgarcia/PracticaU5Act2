namespace PracticaU5Act2;

public static class SortingAlgorithms
{
    // Bubble sort with early exit: a pass without swaps means the array is already sorted.
    public static void Bubble(int[] data)
    {
        int n = data.Length;
        for (int pass = 0; pass < n - 1; pass++)
        {
            bool swapped = false;
            for (int i = 0; i < n - 1 - pass; i++)
            {
                if (data[i] > data[i + 1])
                {
                    (data[i], data[i + 1]) = (data[i + 1], data[i]);
                    swapped = true;
                }
            }

            if (!swapped)
                break;
        }
    }

    // Quicksort with the pivot taken from the middle (Hoare partition). Recursing only into the
    // smaller half keeps the call stack at O(log n) even if a partition is unbalanced.
    public static void Quick(int[] data) => Quick(data, 0, data.Length - 1);

    private static void Quick(int[] data, int low, int high)
    {
        while (low < high)
        {
            int split = Partition(data, low, high);
            if (split - low < high - split)
            {
                Quick(data, low, split);
                low = split + 1;
            }
            else
            {
                Quick(data, split + 1, high);
                high = split;
            }
        }
    }

    private static int Partition(int[] data, int low, int high)
    {
        int pivot = data[low + (high - low) / 2];
        int i = low - 1;
        int j = high + 1;

        while (true)
        {
            do { i++; } while (data[i] < pivot);
            do { j--; } while (data[j] > pivot);

            if (i >= j)
                return j;

            (data[i], data[j]) = (data[j], data[i]);
        }
    }

    // ShellSort: insertion sort over gaps n/2, n/4, ... 1.
    public static void Shell(int[] data)
    {
        int n = data.Length;
        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i++)
            {
                int value = data[i];
                int j = i;
                while (j >= gap && data[j - gap] > value)
                {
                    data[j] = data[j - gap];
                    j -= gap;
                }
                data[j] = value;
            }
        }
    }

    // LSD radix sort in base 10. Values are shifted by the minimum so negative numbers also work.
    public static void Radix(int[] data)
    {
        int n = data.Length;
        if (n < 2)
            return;

        int min = data[0];
        int max = data[0];
        for (int i = 1; i < n; i++)
        {
            if (data[i] < min) min = data[i];
            if (data[i] > max) max = data[i];
        }

        long range = (long)max - min;
        int[] source = data;
        int[] target = new int[n];
        int[] count = new int[10];

        for (long exp = 1; range / exp > 0; exp *= 10)
        {
            Array.Clear(count);
            for (int i = 0; i < n; i++)
                count[(int)(((long)source[i] - min) / exp % 10)]++;

            for (int d = 1; d < 10; d++)
                count[d] += count[d - 1];

            // Walking backwards keeps equal digits in their original order (stable).
            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (int)(((long)source[i] - min) / exp % 10);
                target[--count[digit]] = source[i];
            }

            (source, target) = (target, source);
        }

        if (!ReferenceEquals(source, data))
            Array.Copy(source, data, n);
    }

    // Direct merge: merges runs of width 1, 2, 4, ... regardless of the existing order.
    public static void DirectMerge(int[] data)
    {
        int n = data.Length;
        int[] source = data;
        int[] target = new int[n];

        for (int width = 1; width < n; width *= 2)
        {
            for (int left = 0; left < n; left += 2 * width)
            {
                int middle = Math.Min(left + width, n);
                int right = Math.Min(left + 2 * width, n);
                Merge(source, target, left, middle, right);
            }

            (source, target) = (target, source);
        }

        if (!ReferenceEquals(source, data))
            Array.Copy(source, data, n);
    }

    // Natural merge: detects the ascending runs that already exist and merges them in pairs.
    public static void NaturalMerge(int[] data)
    {
        int n = data.Length;
        if (n < 2)
            return;

        int[] source = data;
        int[] target = new int[n];

        while (true)
        {
            int groups = 0;
            int start = 0;

            while (start < n)
            {
                int middle = RunEnd(source, start);
                if (middle >= n)
                {
                    Array.Copy(source, start, target, start, n - start);
                    groups++;
                    break;
                }

                int right = RunEnd(source, middle);
                Merge(source, target, start, middle, right);
                groups++;
                start = right;
            }

            (source, target) = (target, source);
            if (groups <= 1)
                break;
        }

        if (!ReferenceEquals(source, data))
            Array.Copy(source, data, n);
    }

    // Index right after the ascending run that begins at "start".
    private static int RunEnd(int[] data, int start)
    {
        int end = start + 1;
        while (end < data.Length && data[end - 1] <= data[end])
            end++;
        return end;
    }

    // Merges the two consecutive sorted runs that start at left and at middle (ending at right) into target.
    private static void Merge(int[] source, int[] target, int left, int middle, int right)
    {
        int i = left;
        int j = middle;
        int k = left;

        while (i < middle && j < right)
            target[k++] = source[i] <= source[j] ? source[i++] : source[j++];

        while (i < middle)
            target[k++] = source[i++];

        while (j < right)
            target[k++] = source[j++];
    }
}
