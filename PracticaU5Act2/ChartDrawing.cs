using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PracticaU5Act2;

public static class ChartDrawing
{
    private const int LeftMargin = 84;
    private const int RightMargin = 150;   // room for the label at the end of each line
    private const int TopMargin = 76;
    private const int BottomMargin = 64;
    private const float LabelGap = 16F;

    // Line chart of time against N for one condition. The Y axis is logarithmic because the
    // algorithms differ by several orders of magnitude. There is no legend: each line is named
    // at its right end, which is where the eye finishes reading it.
    public static void Draw(Graphics g, Rectangle area, IReadOnlyList<BenchmarkResult> results, DataCondition condition)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        g.Clear(Color.White);

        using var centered = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        using var inkBrush = new SolidBrush(Theme.Ink);
        using var mutedBrush = new SolidBrush(Theme.TextSecondary);
        using var borderPen = new Pen(Theme.Line);

        // A thin border makes the chart read as a card on the page background.
        g.DrawRectangle(borderPen, area.Left, area.Top, area.Width - 1, area.Height - 1);

        var data = new List<BenchmarkResult>();
        foreach (BenchmarkResult row in results)
        {
            if (row.Condition == condition)
                data.Add(row);
        }

        if (data.Count == 0)
        {
            g.DrawString("Ejecuta las mediciones para ver la gráfica.", Theme.BodyFont, mutedBrush, area, centered);
            return;
        }

        string conditionName = AlgorithmCatalog.ConditionName(condition).ToLower();
        g.DrawString($"Tiempo contra número de datos · orden {conditionName}", Theme.HeadingFont, inkBrush,
            area.Left + 20, area.Top + 14);
        g.DrawString("Mediana de las corridas, en milisegundos · escala logarítmica en el eje vertical",
            Theme.CaptionFont, mutedBrush, area.Left + 20, area.Top + 40);

        var plot = new Rectangle(area.Left + LeftMargin, area.Top + TopMargin,
            area.Width - LeftMargin - RightMargin, area.Height - TopMargin - BottomMargin);
        if (plot.Width < 50 || plot.Height < 50)
            return;

        int[] sizes = DistinctSizes(data);

        double minMs = double.MaxValue;
        double maxMs = 0;
        foreach (BenchmarkResult row in data)
        {
            double ms = ClampMs(row.MedianMs);
            if (ms < minMs) minMs = ms;
            if (ms > maxMs) maxMs = ms;
        }

        double minLog = Math.Floor(Math.Log10(minMs));
        double maxLog = Math.Ceiling(Math.Log10(maxMs));
        if (maxLog <= minLog)
            maxLog = minLog + 1;

        float X(int index)
        {
            if (sizes.Length == 1)
                return plot.Left + plot.Width / 2f;

            return plot.Left + index * (plot.Width / (float)(sizes.Length - 1));
        }

        float Y(double ms)
        {
            double fraction = (Math.Log10(ClampMs(ms)) - minLog) / (maxLog - minLog);
            return (float)(plot.Bottom - fraction * plot.Height);
        }

        // Horizontal guides, one per power of ten.
        using var gridPen = new Pen(Theme.Line);
        for (double p = minLog; p <= maxLog; p++)
        {
            float y = Y(Math.Pow(10, p));
            g.DrawLine(gridPen, plot.Left, y, plot.Right, y);

            string label = Math.Pow(10, p) >= 1 ? $"{Math.Pow(10, p):N0} ms" : $"{Math.Pow(10, p):0.###} ms";
            g.DrawString(label, Theme.CaptionFont, mutedBrush,
                plot.Left - 8 - g.MeasureString(label, Theme.CaptionFont).Width, y - 8);
        }

        // The X axis is the only solid line.
        using var axisPen = new Pen(Theme.TextSecondary, 1.5F);
        g.DrawLine(axisPen, plot.Left, plot.Bottom, plot.Right, plot.Bottom);
        for (int i = 0; i < sizes.Length; i++)
        {
            string label = sizes[i].ToString("N0");
            g.DrawLine(axisPen, X(i), plot.Bottom, X(i), plot.Bottom + 4);
            g.DrawString(label, Theme.CaptionFont, inkBrush,
                X(i) - g.MeasureString(label, Theme.CaptionFont).Width / 2, plot.Bottom + 8);
        }

        const string axisTitle = "Número de datos (N)";
        g.DrawString(axisTitle, Theme.CaptionFont, mutedBrush,
            plot.Left + plot.Width / 2f - g.MeasureString(axisTitle, Theme.CaptionFont).Width / 2, plot.Bottom + 32);

        // One line per algorithm. The labels are placed after all the lines so they can be spread out.
        int count = AlgorithmCatalog.All.Count;
        var lastPoints = new PointF[count];
        var seriesWithData = new bool[count];

        for (int a = 0; a < count; a++)
        {
            string name = AlgorithmCatalog.All[a].Name;

            // One point per size, in ascending order of N.
            var points = new List<PointF>();
            for (int s = 0; s < sizes.Length; s++)
            {
                foreach (BenchmarkResult row in data)
                {
                    if (row.Algorithm == name && row.N == sizes[s])
                    {
                        points.Add(new PointF(X(s), Y(row.MedianMs)));
                        break;
                    }
                }
            }

            if (points.Count == 0)
                continue;

            Color color = Theme.SeriesColors[a % Theme.SeriesColors.Length];
            using var pen = new Pen(color, 2.2F);

            // The last series is dashed and every series has its own marker, so the chart
            // can still be read without telling the colors apart.
            if (a == count - 1)
                pen.DashStyle = DashStyle.Dash;

            if (points.Count > 1)
                g.DrawLines(pen, points.ToArray());

            using var solidBrush = new SolidBrush(color);
            foreach (PointF point in points)
                DrawMarker(g, a, point, solidBrush, color);

            lastPoints[a] = points[points.Count - 1];
            seriesWithData[a] = true;
        }

        DrawEndLabels(g, plot, lastPoints, seriesWithData);
    }

    // Names each line at its right end. Lines that finish close together would overlap their names,
    // so the labels are pushed apart by a minimum gap and joined to their line by a thin connector.
    private static void DrawEndLabels(Graphics g, Rectangle plot, PointF[] lastPoints, bool[] hasData)
    {
        int count = lastPoints.Length;
        var wanted = new float[count];
        var indexes = new List<int>();
        for (int a = 0; a < count; a++)
        {
            if (!hasData[a])
                continue;

            wanted[a] = lastPoints[a].Y;
            indexes.Add(a);
        }

        SpreadLabels(wanted, indexes, plot.Bottom);

        foreach (int a in indexes)
        {
            Color line = Theme.SeriesColors[a % Theme.SeriesColors.Length];
            Color text = Theme.SeriesTextColors[a % Theme.SeriesTextColors.Length];
            using var connector = new Pen(line, 1F);
            using var textBrush = new SolidBrush(text);

            float labelX = plot.Right + 14;
            g.DrawLine(connector, lastPoints[a].X + 6, lastPoints[a].Y, plot.Right + 10, wanted[a]);
            g.DrawString(AlgorithmCatalog.All[a].Name, Theme.StrongFont, textBrush, labelX, wanted[a] - 9);
        }
    }

    // Moves the label positions down until neighbors are at least LabelGap apart, then pushes the
    // whole pile back up if it slid below the bottom of the plot.
    private static void SpreadLabels(float[] positions, List<int> indexes, float bottom)
    {
        // Insertion sort of the indexes by vertical position.
        for (int i = 1; i < indexes.Count; i++)
        {
            int current = indexes[i];
            int j = i - 1;
            while (j >= 0 && positions[indexes[j]] > positions[current])
            {
                indexes[j + 1] = indexes[j];
                j--;
            }

            indexes[j + 1] = current;
        }

        for (int i = 1; i < indexes.Count; i++)
        {
            float previous = positions[indexes[i - 1]];
            if (positions[indexes[i]] - previous < LabelGap)
                positions[indexes[i]] = previous + LabelGap;
        }

        if (indexes.Count == 0)
            return;

        int last = indexes[indexes.Count - 1];
        if (positions[last] > bottom)
            positions[last] = bottom;

        for (int i = indexes.Count - 2; i >= 0; i--)
        {
            float next = positions[indexes[i + 1]];
            if (positions[indexes[i]] > next - LabelGap)
                positions[indexes[i]] = next - LabelGap;
        }
    }

    // Six shapes, one per algorithm: circle, square, triangle, diamond, inverted triangle and hollow circle.
    private static void DrawMarker(Graphics g, int shape, PointF center, Brush brush, Color color)
    {
        const float r = 4.5F;
        float x = center.X;
        float y = center.Y;

        switch (shape % 6)
        {
            case 0:
                g.FillEllipse(brush, x - r, y - r, 2 * r, 2 * r);
                break;
            case 1:
                g.FillRectangle(brush, x - r, y - r, 2 * r, 2 * r);
                break;
            case 2:
                g.FillPolygon(brush, new PointF[] { new PointF(x, y - r - 1), new PointF(x + r + 1, y + r), new PointF(x - r - 1, y + r) });
                break;
            case 3:
                g.FillPolygon(brush, new PointF[] { new PointF(x, y - r - 1), new PointF(x + r + 1, y), new PointF(x, y + r + 1), new PointF(x - r - 1, y) });
                break;
            case 4:
                g.FillPolygon(brush, new PointF[] { new PointF(x - r - 1, y - r), new PointF(x + r + 1, y - r), new PointF(x, y + r + 1) });
                break;
            default:
                using (var pen = new Pen(color, 2F))
                {
                    g.FillEllipse(Brushes.White, x - r, y - r, 2 * r, 2 * r);
                    g.DrawEllipse(pen, x - r, y - r, 2 * r, 2 * r);
                }

                break;
        }
    }

    // A logarithmic axis cannot show zero, so very small times are clamped.
    private static double ClampMs(double ms) => Math.Max(ms, 0.001);

    private static int[] DistinctSizes(List<BenchmarkResult> data)
    {
        var sizes = new List<int>();
        foreach (BenchmarkResult row in data)
        {
            if (!sizes.Contains(row.N))
                sizes.Add(row.N);
        }

        sizes.Sort();
        return sizes.ToArray();
    }
}
