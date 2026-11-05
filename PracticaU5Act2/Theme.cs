namespace PracticaU5Act2;

// Colors, fonts and style helpers shared by the whole window. The palette is a blue-gray (Nord-like) one:
// Ink is the dark base, Cyan is the accent and everything else is a light neutral.
// Every helper only changes appearance; no behavior of the program depends on this class.
internal static class Theme
{
    // ---- Base palette ----
    public static readonly Color Ink = Rgb(0x2E3440);
    public static readonly Color InkRaised = Rgb(0x3B4252);
    public static readonly Color InkSoft = Rgb(0x434C5E);
    public static readonly Color Cyan = Rgb(0x88C0D0);
    public static readonly Color CyanHover = Rgb(0xA3CFDC);
    public static readonly Color CyanPressed = Rgb(0x6FAAC0);
    public static readonly Color Sky = Rgb(0x81A1C1);

    // ---- Light surfaces and text ----
    public static readonly Color Background = Rgb(0xECEFF4);
    public static readonly Color Band = Rgb(0xE5E9F0);
    public static readonly Color Line = Rgb(0xD8DEE9);
    public static readonly Color AltRow = Rgb(0xF7F8FB);
    public static readonly Color Selection = Rgb(0xD3E8EE);
    public static readonly Color ControlBorder = Rgb(0x7B88A1);
    public static readonly Color TextSecondary = Rgb(0x4C566A);
    public static readonly Color OnInk = Rgb(0xECEFF4);
    public static readonly Color OnInkMuted = Rgb(0xD8DEE9);

    // ---- States (never used alone: they always come with a symbol or a word) ----
    public static readonly Color Success = Rgb(0x3A6B35);
    public static readonly Color Danger = Rgb(0xA33F4A);
    public static readonly Color Warning = Rgb(0xEBCB8B);

    // ---- Fonts ----
    public static readonly Font BodyFont = new("Segoe UI", 10F);
    public static readonly Font HeadingFont = new("Segoe UI Semibold", 11F);
    public static readonly Font TitleFont = new("Segoe UI Semibold", 15F);
    public static readonly Font CaptionFont = new("Segoe UI", 9F);
    public static readonly Font StrongFont = new("Segoe UI Semibold", 9.5F);
    public static readonly Font ButtonFont = new("Segoe UI Semibold", 10F);
    public static readonly Font MonoFont = new("Consolas", 10F);

    // One color per algorithm, plus a darker version of it for the text so every label passes 4.5:1 on white.
    public static readonly Color[] SeriesColors =
    [
        Rgb(0xB04A55), Rgb(0x5E81AC), Rgb(0xB06A33), Rgb(0x2E3440), Rgb(0x37859E), Rgb(0x5A8A47)
    ];

    public static readonly Color[] SeriesTextColors =
    [
        Rgb(0xA33F4A), Rgb(0x3F5F8A), Rgb(0x8F5526), Rgb(0x2E3440), Rgb(0x2B6A80), Rgb(0x476E38)
    ];

    private static Color Rgb(int hex) => Color.FromArgb((hex >> 16) & 0xFF, (hex >> 8) & 0xFF, hex & 0xFF);

    // =====================================================================
    // Window, pages and header
    // =====================================================================

    public static void StyleWindow(Form form)
    {
        form.BackColor = Background;
        form.ForeColor = Ink;
        form.Font = BodyFont;
    }

    public static void StylePage(TabPage page)
    {
        page.UseVisualStyleBackColor = false;
        page.BackColor = Background;
        page.Padding = new Padding(16);
    }

    public static void StyleHeader(Panel header, Label title, Label subtitle)
    {
        header.BackColor = Ink;
        title.BackColor = Color.Transparent;
        title.ForeColor = OnInk;
        title.Font = TitleFont;
        subtitle.BackColor = Color.Transparent;
        subtitle.ForeColor = OnInkMuted;
        subtitle.Font = CaptionFont;
    }

    public static void StyleBanner(Panel panel, Label label)
    {
        panel.BackColor = Warning;
        label.BackColor = Color.Transparent;
        label.ForeColor = Ink;
        label.Font = StrongFont;
    }

    public static void StyleStatusStrip(StatusStrip strip)
    {
        strip.Renderer = new ToolStripProfessionalRenderer(new BandColorTable());
        strip.BackColor = Band;
        strip.ForeColor = TextSecondary;
        strip.Font = CaptionFont;
        strip.SizingGrip = false;
        foreach (ToolStripItem item in strip.Items)
            item.ForeColor = TextSecondary;
    }

    // The professional renderer paints the status strip with a gradient; this table makes it a flat band.
    private sealed class BandColorTable : ProfessionalColorTable
    {
        public override Color StatusStripGradientBegin => Band;
        public override Color StatusStripGradientEnd => Band;
    }

    // =====================================================================
    // Progress line (a 4 px track with a fill that grows from the left)
    // =====================================================================

    public static void StyleProgress(Panel track, Panel fill)
    {
        track.BackColor = InkRaised;
        fill.BackColor = Cyan;
        fill.Width = 0;
        track.Resize += OnProgressTrackResize;
    }

    public static void SetProgress(Panel track, Panel fill, int done, int total)
    {
        double fraction = total <= 0 ? 0 : Math.Min(1.0, (double)done / total);
        fill.Tag = fraction;
        fill.Width = (int)(track.Width * fraction);
    }

    private static void OnProgressTrackResize(object? sender, EventArgs e)
    {
        if (sender is Panel track && track.Controls.Count > 0 && track.Controls[0].Tag is double fraction)
            track.Controls[0].Width = (int)(track.Width * fraction);
    }

    // =====================================================================
    // Buttons
    // =====================================================================

    private sealed record ButtonLook(Color Back, Color Border, Color DisabledBack, Color DisabledBorder);

    // Main action on the dark header: cyan with dark text (11:1).
    public static void StylePrimaryOnDark(Button button) =>
        StyleButton(button, Cyan, Ink, Cyan, CyanHover, CyanPressed, InkRaised, InkSoft);

    // Secondary action on the dark header: outlined.
    public static void StyleSecondaryOnDark(Button button) =>
        StyleButton(button, Ink, OnInk, Sky, InkRaised, InkSoft, Ink, InkSoft);

    // Main action on a light surface.
    public static void StylePrimary(Button button) =>
        StyleButton(button, Ink, OnInk, Ink, InkRaised, InkSoft, Band, Line);

    // Secondary action on a light surface.
    public static void StyleSecondary(Button button) =>
        StyleButton(button, Color.White, Ink, ControlBorder, Band, Line, Background, Line);

    private static void StyleButton(Button button, Color back, Color fore, Color border, Color hover, Color pressed,
        Color disabledBack, Color disabledBorder)
    {
        button.UseVisualStyleBackColor = false;
        button.FlatStyle = FlatStyle.Flat;
        button.Font = ButtonFont;
        button.Cursor = Cursors.Hand;
        button.ForeColor = fore;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.MouseOverBackColor = hover;
        button.FlatAppearance.MouseDownBackColor = pressed;

        // WinForms always draws the text of a disabled flat button in gray, so the background and the
        // border are swapped when the button is enabled or disabled.
        button.Tag = new ButtonLook(back, border, disabledBack, disabledBorder);
        button.EnabledChanged += OnButtonEnabledChanged;
        ApplyButtonLook(button);
    }

    private static void OnButtonEnabledChanged(object? sender, EventArgs e)
    {
        if (sender is Button button)
            ApplyButtonLook(button);
    }

    private static void ApplyButtonLook(Button button)
    {
        if (button.Tag is not ButtonLook look)
            return;

        button.BackColor = button.Enabled ? look.Back : look.DisabledBack;
        button.FlatAppearance.BorderColor = button.Enabled ? look.Border : look.DisabledBorder;
    }

    public static void StyleCheckOnDark(CheckBox box)
    {
        box.BackColor = Color.Transparent;
        box.ForeColor = OnInk;
        box.Font = BodyFont;
        box.Cursor = Cursors.Hand;
    }

    // A group of radio buttons drawn as one segmented control. The checked one is dark.
    public static void StyleSegment(RadioButton radio)
    {
        radio.Appearance = Appearance.Button;
        radio.FlatStyle = FlatStyle.Flat;
        radio.TextAlign = ContentAlignment.MiddleCenter;
        radio.UseVisualStyleBackColor = false;
        radio.Font = ButtonFont;
        radio.Cursor = Cursors.Hand;
        radio.FlatAppearance.BorderSize = 1;
        radio.FlatAppearance.BorderColor = ControlBorder;
        radio.FlatAppearance.CheckedBackColor = Ink;
        radio.FlatAppearance.MouseDownBackColor = Line;
        radio.CheckedChanged += OnSegmentCheckedChanged;
        ApplySegmentLook(radio);
    }

    private static void OnSegmentCheckedChanged(object? sender, EventArgs e)
    {
        if (sender is RadioButton radio)
            ApplySegmentLook(radio);
    }

    private static void ApplySegmentLook(RadioButton radio)
    {
        radio.BackColor = radio.Checked ? Ink : Color.White;
        radio.ForeColor = radio.Checked ? OnInk : Ink;
        radio.FlatAppearance.MouseOverBackColor = radio.Checked ? Ink : Band;
    }

    // =====================================================================
    // Tables, logs and summaries
    // =====================================================================

    // Flat, horizontal rules only, alternating rows and right-aligned numbers.
    public static void StyleGrid(DataGridView grid, params int[] rightAlignedColumns)
    {
        grid.BorderStyle = BorderStyle.None;
        grid.BackgroundColor = Color.White;
        grid.GridColor = Line;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.RowHeadersVisible = false;
        grid.AllowUserToResizeRows = false;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = 44;
        grid.RowTemplate.Height = 32;

        DataGridViewCellStyle header = grid.ColumnHeadersDefaultCellStyle;
        header.BackColor = Band;
        header.ForeColor = Ink;
        header.Font = StrongFont;
        header.SelectionBackColor = Band;
        header.SelectionForeColor = Ink;
        header.WrapMode = DataGridViewTriState.True;
        header.Alignment = DataGridViewContentAlignment.MiddleLeft;
        header.Padding = new Padding(8, 0, 8, 0);

        DataGridViewCellStyle cells = grid.DefaultCellStyle;
        cells.BackColor = Color.White;
        cells.ForeColor = Ink;
        cells.Font = BodyFont;
        cells.SelectionBackColor = Selection;
        cells.SelectionForeColor = Ink;
        cells.Padding = new Padding(8, 0, 8, 0);

        DataGridViewCellStyle alternate = grid.AlternatingRowsDefaultCellStyle;
        alternate.BackColor = AltRow;
        alternate.SelectionBackColor = Selection;
        alternate.SelectionForeColor = Ink;

        foreach (int index in rightAlignedColumns)
        {
            if (index < 0 || index >= grid.Columns.Count)
                continue;

            grid.Columns[index].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns[index].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }

    public static void StyleLog(TextBox box)
    {
        box.BackColor = Color.White;
        box.ForeColor = Ink;
        box.Font = MonoFont;
        box.BorderStyle = BorderStyle.FixedSingle;
    }

    public static void StyleSummary(Label label)
    {
        label.BackColor = Color.Transparent;
        label.ForeColor = TextSecondary;
        label.Font = HeadingFont;
    }
}
