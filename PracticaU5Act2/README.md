# PracticaU5Act2 – Sorting algorithms test bench

Windows Forms application (C#, .NET 10) for Estructura de Datos, unit 5, activity 5.2.
It implements six sorting algorithms and measures how long each one takes with 10 000, 50 000 and 100 000
integers in three conditions: random, already sorted and reversed.

## Algorithms

| Algorithm | Kind | Notes |
|---|---|---|
| Bubble sort | Internal | Early exit when a pass makes no swaps |
| Quicksort | Internal | Middle pivot, Hoare partition, recursion into the smaller half |
| ShellSort | Internal | Gaps n/2, n/4, ..., 1 |
| Radix sort | Internal | LSD, base 10, stable; accepts negative values |
| Direct merge | External (simulated in memory) | Runs of width 1, 2, 4, ... |
| Natural merge | External (simulated in memory) | Detects the existing ascending runs |

## How the measurements work

- Data are generated with a fixed seed (2026); the sorted and reversed arrays are built from the same random values.
- Each combination runs after a warm-up with 2 000 items. The median of 5 runs is reported.
  Bubble sort with 50 000 and 100 000 items is measured once because it takes seconds per run.
- Every run is checked against `Array.Sort`.
- The time is elapsed wall-clock time from `Stopwatch` (single thread), not the CPU time of the process.
- Run it in **Release** mode; in Debug the times are not representative (the window shows a warning).

## Project files

| File | Purpose |
|---|---|
| `SortingAlgorithms.cs` | The six sorting functions and the `Merge` helper |
| `SortAlgorithm.cs` | Algorithm catalog, data conditions and result record |
| `DataGenerator.cs` | Random data and the three conditions |
| `SortBenchmark.cs` | Timing, number of runs and verification |
| `Statistics.cs` | Median |
| `ChartDrawing.cs` | Log-scale time chart (GDI+) |
| `TestBench.cs` | 14 automatic tests |
| `Theme.cs` | Colors and fonts of the window |
| `Form1.cs`, `Form1.Designer.cs` | User interface |

## Run

1. Open the solution in Visual Studio (.NET 10 SDK, Windows).
2. Select the **Release** configuration and press Ctrl+F5.
3. In **Mediciones** press *Medir todo*. The full run takes a few minutes, mostly because of bubble sort;
   uncheck *Incluir Burbuja* to skip it.
4. Choose the condition above the chart and use *Guardar PNG* to export it.
5. In **Pruebas** press *Ejecutar pruebas* to run the 14 tests.
