namespace PracticaU5Act2;

public partial class Form1 : Form
{
    private const int Seed = 2026;

    private readonly List<BenchmarkResult> _results = [];
    private readonly IProgress<string> _status;
    private readonly IProgress<BenchmarkResult> _report;
    private CancellationTokenSource? _cancel;
    private CancellationToken _token;
    private bool _includeBubble;
    private int _plannedMeasurements;

    public Form1()
    {
        InitializeComponent();
        ConfigureGrid();
        ApplyTheme();

        // Created here, on the UI thread, so the progress callbacks run on the UI thread too.
        _status = new Progress<string>(ShowStatus);
        _report = new Progress<BenchmarkResult>(AddResult);

        int algorithms = AlgorithmCatalog.All.Count;
        lblSubtitle.Text = $"{algorithms} algoritmos · tamaños de 10 000, 50 000 y 100 000 datos · " +
            "orden aleatorio, ordenado e inverso";

        panelDebug.Visible = BuildInfo.IsDebug;
        lblStatus.Text = "Listo. Pulsa «Medir todo» para empezar.";
        lblProgress.Text = string.Empty;
    }

    private void ConfigureGrid()
    {
        dgvResults.Columns.Add("columnAlgorithm", "Algoritmo");
        dgvResults.Columns.Add("columnN", "N");
        dgvResults.Columns.Add("columnCondition", "Condición");
        dgvResults.Columns.Add("columnTime", "Tiempo (ms, mediana)");
        dgvResults.Columns.Add("columnRuns", "Corridas");
        dgvResults.Columns.Add("columnCorrect", "Verificado");
    }

    // Only appearance: colors, fonts and button states. The layout itself lives in the Designer.
    private void ApplyTheme()
    {
        Theme.StyleWindow(this);
        Theme.StylePage(tabMeasure);
        Theme.StylePage(tabTests);

        Theme.StyleBanner(panelDebug, lblDebug);
        Theme.StyleHeader(panelHeader, lblTitle, lblSubtitle);
        Theme.StyleProgress(panelProgress, panelProgressFill);
        Theme.StyleCheckOnDark(chkBubble);
        Theme.StyleSecondaryOnDark(btnCancel);
        Theme.StylePrimaryOnDark(btnRun);

        Theme.StyleSegment(rbRandom);
        Theme.StyleSegment(rbSorted);
        Theme.StyleSegment(rbReversed);
        Theme.StyleSecondary(btnSavePng);

        Theme.StyleGrid(dgvResults, 1, 3, 4);

        Theme.StylePrimary(btnRunTests);
        Theme.StyleSummary(lblTestSummary);
        Theme.StyleLog(txtTestReport);

        Theme.StyleStatusStrip(statusStrip);
    }

    private DataCondition SelectedCondition
    {
        get
        {
            if (rbSorted.Checked)
                return DataCondition.Sorted;
            if (rbReversed.Checked)
                return DataCondition.Reversed;
            return DataCondition.Random;
        }
    }

    // =====================================================================
    // Mediciones
    // =====================================================================

    private async void btnRun_Click(object? sender, EventArgs e)
    {
        _results.Clear();
        dgvResults.Rows.Clear();
        pbChart.Invalidate();

        _cancel = new CancellationTokenSource();
        _token = _cancel.Token;
        _includeBubble = chkBubble.Checked;
        _plannedMeasurements = CountPlannedMeasurements();
        UpdateProgress(0, _plannedMeasurements);
        SetRunning(true);

        try
        {
            await Task.Run(RunBenchmark);
            lblStatus.Text = $"Terminado: {_results.Count} mediciones.";
        }
        catch (OperationCanceledException)
        {
            lblStatus.Text = $"Cancelado: se conservan {_results.Count} mediciones.";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error en las mediciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetRunning(false);
            _cancel.Dispose();
            _cancel = null;
        }
    }

    // Runs on a background thread so the window does not freeze.
    private void RunBenchmark() => SortBenchmark.RunAll(_includeBubble, Seed, _status, _report, _token);

    private void ShowStatus(string text) => lblStatus.Text = text;

    private void btnCancel_Click(object? sender, EventArgs e) => _cancel?.Cancel();

    private void SetRunning(bool running)
    {
        btnRun.Enabled = !running;
        btnCancel.Enabled = running;
        chkBubble.Enabled = !running;
    }

    // Sizes x conditions x algorithms that RunAll is going to measure; it is the total of the progress line.
    private int CountPlannedMeasurements()
    {
        int algorithms = 0;
        foreach (SortAlgorithm algorithm in AlgorithmCatalog.All)
        {
            if (_includeBubble || !algorithm.IsQuadratic)
                algorithms++;
        }

        return SortBenchmark.Sizes.Length * Enum.GetValues<DataCondition>().Length * algorithms;
    }

    private void UpdateProgress(int done, int total)
    {
        Theme.SetProgress(panelProgress, panelProgressFill, done, total);
        lblProgress.Text = total > 0 ? $"{done} de {total}" : string.Empty;
    }

    private void AddResult(BenchmarkResult result)
    {
        _results.Add(result);
        int row = dgvResults.Rows.Add(
            result.Algorithm,
            result.N.ToString("N0"),
            AlgorithmCatalog.ConditionName(result.Condition),
            result.MedianMs.ToString("N3"),
            result.Runs,
            result.Correct ? "✓ Sí" : "✗ NO");

        // The state is never shown by color alone: it always carries a symbol and a word.
        DataGridViewCell verified = dgvResults.Rows[row].Cells[5];
        verified.Style.ForeColor = result.Correct ? Theme.Success : Theme.Danger;
        verified.Style.Font = Theme.StrongFont;

        // Keeps the newest row selected, which also scrolls the table to it.
        dgvResults.CurrentCell = dgvResults.Rows[row].Cells[0];

        UpdateProgress(_results.Count, _plannedMeasurements);
        pbChart.Invalidate();
    }

    // =====================================================================
    // Gráfica
    // =====================================================================

    private void pbChart_Paint(object? sender, PaintEventArgs e) =>
        ChartDrawing.Draw(e.Graphics, pbChart.ClientRectangle, _results, SelectedCondition);

    private void rbCondition_CheckedChanged(object? sender, EventArgs e) => pbChart.Invalidate();

    private void btnSavePng_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "Imagen PNG|*.png",
            FileName = $"grafica_{AlgorithmCatalog.ConditionName(SelectedCondition).ToLower()}.png"
        };
        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            using var bitmap = new Bitmap(1100, 650);
            using (Graphics g = Graphics.FromImage(bitmap))
                ChartDrawing.Draw(g, new Rectangle(0, 0, bitmap.Width, bitmap.Height), _results, SelectedCondition);
            bitmap.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "No se pudo guardar la imagen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // =====================================================================
    // Pruebas
    // =====================================================================

    private async void btnRunTests_Click(object? sender, EventArgs e)
    {
        btnRunTests.Enabled = false;
        try
        {
            List<TestResult> tests = await Task.Run(TestBench.RunAll);
            txtTestReport.Clear();

            int passed = 0;
            for (int i = 0; i < tests.Count; i++)
            {
                TestResult test = tests[i];
                txtTestReport.AppendText($"{i + 1,2}. [{(test.Passed ? "APROBADA" : "FALLÓ")}] {test.Name}{Environment.NewLine}");
                txtTestReport.AppendText($"    {test.Detail}{Environment.NewLine}");
                if (test.Passed)
                    passed++;
            }

            bool allPassed = passed == tests.Count;
            lblTestSummary.ForeColor = allPassed ? Theme.Success : Theme.Danger;
            lblTestSummary.Text = $"{(allPassed ? "✓" : "✗")} {passed} de {tests.Count} pruebas aprobadas";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error en las pruebas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnRunTests.Enabled = true;
        }
    }
}

internal static class BuildInfo
{
    public static bool IsDebug
    {
        get
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
