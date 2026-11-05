namespace PracticaU5Act2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tabControl = new TabControl();
            tabMeasure = new TabPage();
            tabTests = new TabPage();
            panelDebug = new Panel();
            lblDebug = new Label();
            panelHeader = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            flowActions = new FlowLayoutPanel();
            chkBubble = new CheckBox();
            btnCancel = new Button();
            btnRun = new Button();
            panelProgress = new Panel();
            panelProgressFill = new Panel();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            lblProgress = new ToolStripStatusLabel();
            toolTip = new ToolTip(components);
            splitMeasure = new SplitContainer();
            dgvResults = new DataGridView();
            pbChart = new PictureBox();
            panelChart = new Panel();
            flowCondition = new FlowLayoutPanel();
            rbRandom = new RadioButton();
            rbSorted = new RadioButton();
            rbReversed = new RadioButton();
            flowChartActions = new FlowLayoutPanel();
            btnSavePng = new Button();
            txtTestReport = new TextBox();
            panelTests = new Panel();
            btnRunTests = new Button();
            lblTestSummary = new Label();
            ((System.ComponentModel.ISupportInitialize)splitMeasure).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbChart).BeginInit();
            tabControl.SuspendLayout();
            tabMeasure.SuspendLayout();
            tabTests.SuspendLayout();
            panelDebug.SuspendLayout();
            panelHeader.SuspendLayout();
            flowActions.SuspendLayout();
            panelProgress.SuspendLayout();
            statusStrip.SuspendLayout();
            splitMeasure.Panel1.SuspendLayout();
            splitMeasure.Panel2.SuspendLayout();
            splitMeasure.SuspendLayout();
            panelChart.SuspendLayout();
            flowCondition.SuspendLayout();
            flowChartActions.SuspendLayout();
            panelTests.SuspendLayout();
            SuspendLayout();
            // tabControl
            tabControl.Dock = DockStyle.Fill;
            tabControl.SelectedIndex = 0;
            tabControl.Name = "tabControl";
            tabControl.TabIndex = 1;
            // tabMeasure
            tabMeasure.Text = "Mediciones";
            tabMeasure.Padding = new Padding(16);
            tabMeasure.UseVisualStyleBackColor = true;
            tabMeasure.Name = "tabMeasure";
            tabMeasure.TabIndex = 0;
            // tabTests
            tabTests.Text = "Pruebas";
            tabTests.Padding = new Padding(16);
            tabTests.UseVisualStyleBackColor = true;
            tabTests.Name = "tabTests";
            tabTests.TabIndex = 1;
            // panelDebug
            panelDebug.Dock = DockStyle.Top;
            panelDebug.Size = new Size(1100, 34);
            panelDebug.Padding = new Padding(20, 0, 20, 0);
            panelDebug.Name = "panelDebug";
            panelDebug.TabIndex = 2;
            // lblDebug
            lblDebug.Dock = DockStyle.Fill;
            lblDebug.Text = "Compilado en Debug: los tiempos no son representativos. Ejecútalo en Release para medir.";
            lblDebug.TextAlign = ContentAlignment.MiddleLeft;
            lblDebug.Name = "lblDebug";
            lblDebug.TabIndex = 0;
            // panelHeader
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Size = new Size(1100, 76);
            panelHeader.BackColor = Color.FromArgb(46, 52, 64);
            panelHeader.Name = "panelHeader";
            panelHeader.TabIndex = 0;
            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 12);
            lblTitle.Text = "Banco de pruebas de ordenamiento";
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;
            // lblSubtitle
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(22, 44);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.TabIndex = 1;
            // flowActions
            flowActions.Dock = DockStyle.Right;
            flowActions.Size = new Size(450, 72);
            flowActions.FlowDirection = FlowDirection.LeftToRight;
            flowActions.WrapContents = false;
            flowActions.Padding = new Padding(0, 18, 20, 0);
            flowActions.Name = "flowActions";
            flowActions.TabIndex = 2;
            // chkBubble
            chkBubble.AutoSize = true;
            chkBubble.Checked = true;
            chkBubble.CheckState = CheckState.Checked;
            chkBubble.Margin = new Padding(0, 6, 16, 0);
            chkBubble.Text = "Incluir &Burbuja";
            chkBubble.UseVisualStyleBackColor = true;
            chkBubble.Name = "chkBubble";
            chkBubble.TabIndex = 0;
            // btnCancel
            btnCancel.Size = new Size(110, 36);
            btnCancel.Margin = new Padding(0, 0, 8, 0);
            btnCancel.Text = "&Cancelar";
            btnCancel.Enabled = false;
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Name = "btnCancel";
            btnCancel.TabIndex = 1;
            // btnRun
            btnRun.Size = new Size(150, 36);
            btnRun.Margin = new Padding(0);
            btnRun.Text = "▶  &Medir todo";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Name = "btnRun";
            btnRun.TabIndex = 2;
            // panelProgress
            panelProgress.Dock = DockStyle.Bottom;
            panelProgress.Size = new Size(1100, 4);
            panelProgress.Name = "panelProgress";
            panelProgress.TabIndex = 3;
            // panelProgressFill
            panelProgressFill.Dock = DockStyle.Left;
            panelProgressFill.Size = new Size(0, 4);
            panelProgressFill.Name = "panelProgressFill";
            panelProgressFill.TabIndex = 0;
            // statusStrip
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus, lblProgress });
            statusStrip.Dock = DockStyle.Bottom;
            statusStrip.SizingGrip = false;
            statusStrip.Name = "statusStrip";
            statusStrip.TabIndex = 3;
            // lblStatus
            lblStatus.Spring = true;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.Name = "lblStatus";
            // lblProgress
            lblProgress.TextAlign = ContentAlignment.MiddleRight;
            lblProgress.Name = "lblProgress";
            // splitMeasure
            splitMeasure.Dock = DockStyle.Fill;
            splitMeasure.Orientation = Orientation.Vertical;
            splitMeasure.Size = new Size(1060, 500);
            splitMeasure.Panel1MinSize = 380;
            splitMeasure.Panel2MinSize = 400;
            splitMeasure.SplitterWidth = 16;
            splitMeasure.SplitterDistance = 500;
            splitMeasure.Name = "splitMeasure";
            splitMeasure.TabIndex = 0;
            // dgvResults
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.ReadOnly = true;
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.RowHeadersVisible = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.BackgroundColor = Color.White;
            dgvResults.Name = "dgvResults";
            dgvResults.TabIndex = 0;
            // pbChart
            pbChart.Dock = DockStyle.Fill;
            pbChart.BackColor = Color.White;
            pbChart.BorderStyle = BorderStyle.None;
            pbChart.Name = "pbChart";
            pbChart.TabIndex = 0;
            // panelChart
            panelChart.Dock = DockStyle.Top;
            panelChart.Size = new Size(544, 48);
            panelChart.Padding = new Padding(0, 0, 0, 10);
            panelChart.Name = "panelChart";
            panelChart.TabIndex = 1;
            // flowCondition
            flowCondition.Dock = DockStyle.Left;
            flowCondition.Size = new Size(300, 38);
            flowCondition.FlowDirection = FlowDirection.LeftToRight;
            flowCondition.WrapContents = false;
            flowCondition.Name = "flowCondition";
            flowCondition.TabIndex = 0;
            // rbRandom
            rbRandom.Appearance = Appearance.Button;
            rbRandom.Size = new Size(96, 34);
            rbRandom.Margin = new Padding(0);
            rbRandom.Checked = true;
            rbRandom.TabStop = true;
            rbRandom.Text = "&Aleatorio";
            rbRandom.Name = "rbRandom";
            rbRandom.TabIndex = 0;
            // rbSorted
            rbSorted.Appearance = Appearance.Button;
            rbSorted.Size = new Size(96, 34);
            rbSorted.Margin = new Padding(0);
            rbSorted.Text = "&Ordenado";
            rbSorted.Name = "rbSorted";
            rbSorted.TabIndex = 1;
            // rbReversed
            rbReversed.Appearance = Appearance.Button;
            rbReversed.Size = new Size(96, 34);
            rbReversed.Margin = new Padding(0);
            rbReversed.Text = "&Inverso";
            rbReversed.Name = "rbReversed";
            rbReversed.TabIndex = 2;
            // flowChartActions
            flowChartActions.Dock = DockStyle.Right;
            flowChartActions.Size = new Size(150, 38);
            flowChartActions.FlowDirection = FlowDirection.LeftToRight;
            flowChartActions.WrapContents = false;
            flowChartActions.Name = "flowChartActions";
            flowChartActions.TabIndex = 1;
            // btnSavePng
            btnSavePng.Size = new Size(140, 34);
            btnSavePng.Margin = new Padding(0);
            btnSavePng.Text = "&Guardar PNG";
            btnSavePng.UseVisualStyleBackColor = true;
            btnSavePng.Name = "btnSavePng";
            btnSavePng.TabIndex = 0;
            // txtTestReport
            txtTestReport.Dock = DockStyle.Fill;
            txtTestReport.Multiline = true;
            txtTestReport.ReadOnly = true;
            txtTestReport.ScrollBars = ScrollBars.Both;
            txtTestReport.WordWrap = false;
            txtTestReport.Font = new Font("Consolas", 10F);
            txtTestReport.Name = "txtTestReport";
            txtTestReport.TabIndex = 0;
            // panelTests
            panelTests.Dock = DockStyle.Top;
            panelTests.Size = new Size(1060, 56);
            panelTests.Name = "panelTests";
            panelTests.TabIndex = 1;
            // btnRunTests
            btnRunTests.Location = new Point(0, 0);
            btnRunTests.Size = new Size(150, 36);
            btnRunTests.Text = "&Ejecutar pruebas";
            btnRunTests.UseVisualStyleBackColor = true;
            btnRunTests.Name = "btnRunTests";
            btnRunTests.TabIndex = 0;
            // lblTestSummary
            lblTestSummary.AutoSize = false;
            lblTestSummary.Location = new Point(168, 0);
            lblTestSummary.Size = new Size(500, 36);
            lblTestSummary.TextAlign = ContentAlignment.MiddleLeft;
            lblTestSummary.Name = "lblTestSummary";
            lblTestSummary.TabIndex = 1;
            // containers
            Controls.Add(tabControl);
            Controls.Add(panelDebug);
            Controls.Add(panelHeader);
            Controls.Add(statusStrip);
            tabControl.Controls.Add(tabMeasure);
            tabControl.Controls.Add(tabTests);
            panelDebug.Controls.Add(lblDebug);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(flowActions);
            panelHeader.Controls.Add(panelProgress);
            flowActions.Controls.Add(chkBubble);
            flowActions.Controls.Add(btnCancel);
            flowActions.Controls.Add(btnRun);
            panelProgress.Controls.Add(panelProgressFill);
            tabMeasure.Controls.Add(splitMeasure);
            splitMeasure.Panel1.Controls.Add(dgvResults);
            splitMeasure.Panel2.Controls.Add(pbChart);
            splitMeasure.Panel2.Controls.Add(panelChart);
            panelChart.Controls.Add(flowCondition);
            panelChart.Controls.Add(flowChartActions);
            flowCondition.Controls.Add(rbRandom);
            flowCondition.Controls.Add(rbSorted);
            flowCondition.Controls.Add(rbReversed);
            flowChartActions.Controls.Add(btnSavePng);
            tabTests.Controls.Add(txtTestReport);
            tabTests.Controls.Add(panelTests);
            panelTests.Controls.Add(btnRunTests);
            panelTests.Controls.Add(lblTestSummary);
            // tooltips
            toolTip.SetToolTip(chkBubble, "Burbuja es cuadrática: con 100 000 datos tarda varios segundos por corrida.");
            toolTip.SetToolTip(btnSavePng, "Guarda la gráfica de la condición elegida como imagen PNG.");
            // Form1
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(1100, 680);
            MinimumSize = new Size(980, 600);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Práctica 5.2 – Ordenamiento: banco de pruebas";
            btnRun.Click += btnRun_Click;
            btnCancel.Click += btnCancel_Click;
            pbChart.Paint += pbChart_Paint;
            rbRandom.CheckedChanged += rbCondition_CheckedChanged;
            rbSorted.CheckedChanged += rbCondition_CheckedChanged;
            rbReversed.CheckedChanged += rbCondition_CheckedChanged;
            btnSavePng.Click += btnSavePng_Click;
            btnRunTests.Click += btnRunTests_Click;
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbChart).EndInit();
            panelTests.ResumeLayout(false);
            flowChartActions.ResumeLayout(false);
            flowCondition.ResumeLayout(false);
            panelChart.ResumeLayout(false);
            splitMeasure.Panel1.ResumeLayout(false);
            splitMeasure.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMeasure).EndInit();
            splitMeasure.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            panelProgress.ResumeLayout(false);
            flowActions.ResumeLayout(false);
            flowActions.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelDebug.ResumeLayout(false);
            tabTests.ResumeLayout(false);
            tabMeasure.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabMeasure;
        private TabPage tabTests;
        private Panel panelDebug;
        private Label lblDebug;
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private FlowLayoutPanel flowActions;
        private CheckBox chkBubble;
        private Button btnCancel;
        private Button btnRun;
        private Panel panelProgress;
        private Panel panelProgressFill;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblProgress;
        private ToolTip toolTip;
        private SplitContainer splitMeasure;
        private DataGridView dgvResults;
        private PictureBox pbChart;
        private Panel panelChart;
        private FlowLayoutPanel flowCondition;
        private RadioButton rbRandom;
        private RadioButton rbSorted;
        private RadioButton rbReversed;
        private FlowLayoutPanel flowChartActions;
        private Button btnSavePng;
        private TextBox txtTestReport;
        private Panel panelTests;
        private Button btnRunTests;
        private Label lblTestSummary;
    }
}
