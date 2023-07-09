namespace NoMoreClick
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelAttributionIconPart1 = new System.Windows.Forms.Label();
            this.labelAttributionIconPart2 = new System.Windows.Forms.Label();
            this.linkLabelAttributionIconPart1 = new System.Windows.Forms.LinkLabel();
            this.linkLabelAttributionIconPart2 = new System.Windows.Forms.LinkLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numericUpDownPostClickDeadzoneRadius = new System.Windows.Forms.NumericUpDown();
            this.labelPostClickDeadzoneRadius = new System.Windows.Forms.Label();
            this.labelClickDelayMs = new System.Windows.Forms.Label();
            this.numericUpDownClickDelayMs = new System.Windows.Forms.NumericUpDown();
            this.labelClickDelayAfterRightClickMs = new System.Windows.Forms.Label();
            this.numericUpDownDelayAfterRightClickMs = new System.Windows.Forms.NumericUpDown();
            this.labelToggleClickAssistanceMs = new System.Windows.Forms.Label();
            this.numericUpDownToggleClickAssistanceMs = new System.Windows.Forms.NumericUpDown();
            this.labelNoClickAfterTypingMs = new System.Windows.Forms.Label();
            this.numericUpDownNoClickAfterTypingMs = new System.Windows.Forms.NumericUpDown();
            this.labelNoClickAfterScrollingMs = new System.Windows.Forms.Label();
            this.numericUpDownNoClickAfterScrollingMs = new System.Windows.Forms.NumericUpDown();
            this.labelNoClickAfterPhysicalMouseClickMs = new System.Windows.Forms.Label();
            this.numericUpDownNoClickAfterPhysicalMouseClickMs = new System.Windows.Forms.NumericUpDown();
            this.labelDoubleClickThresholdToPressShiftKeyMs = new System.Windows.Forms.Label();
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPostClickDeadzoneRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClickDelayMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelayAfterRightClickMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToggleClickAssistanceMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoClickAfterTypingMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoClickAfterScrollingMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoClickAfterPhysicalMouseClickMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDoubleClickThresholdToPressShiftKeyMs)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAttributionIconPart1
            // 
            this.labelAttributionIconPart1.AutoSize = true;
            this.labelAttributionIconPart1.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.labelAttributionIconPart1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAttributionIconPart1.Location = new System.Drawing.Point(96, 533);
            this.labelAttributionIconPart1.Name = "labelAttributionIconPart1";
            this.labelAttributionIconPart1.Size = new System.Drawing.Size(91, 13);
            this.labelAttributionIconPart1.TabIndex = 0;
            this.labelAttributionIconPart1.Text = "Icons made by ";
            this.labelAttributionIconPart1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAttributionIconPart2
            // 
            this.labelAttributionIconPart2.AutoSize = true;
            this.labelAttributionIconPart2.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.labelAttributionIconPart2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAttributionIconPart2.Location = new System.Drawing.Point(152, 546);
            this.labelAttributionIconPart2.Name = "labelAttributionIconPart2";
            this.labelAttributionIconPart2.Size = new System.Drawing.Size(31, 13);
            this.labelAttributionIconPart2.TabIndex = 1;
            this.labelAttributionIconPart2.Text = "from";
            this.labelAttributionIconPart2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // linkLabelAttributionIconPart1
            // 
            this.linkLabelAttributionIconPart1.AutoSize = true;
            this.linkLabelAttributionIconPart1.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.linkLabelAttributionIconPart1.Location = new System.Drawing.Point(189, 533);
            this.linkLabelAttributionIconPart1.Name = "linkLabelAttributionIconPart1";
            this.linkLabelAttributionIconPart1.Size = new System.Drawing.Size(85, 13);
            this.linkLabelAttributionIconPart1.TabIndex = 2;
            this.linkLabelAttributionIconPart1.TabStop = true;
            this.linkLabelAttributionIconPart1.Text = "Pixel perfect";
            this.linkLabelAttributionIconPart1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAttributionIconPart1_LinkClicked);
            // 
            // linkLabelAttributionIconPart2
            // 
            this.linkLabelAttributionIconPart2.AutoSize = true;
            this.linkLabelAttributionIconPart2.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.linkLabelAttributionIconPart2.Location = new System.Drawing.Point(189, 546);
            this.linkLabelAttributionIconPart2.Name = "linkLabelAttributionIconPart2";
            this.linkLabelAttributionIconPart2.Size = new System.Drawing.Size(103, 13);
            this.linkLabelAttributionIconPart2.TabIndex = 3;
            this.linkLabelAttributionIconPart2.TabStop = true;
            this.linkLabelAttributionIconPart2.Text = "www.flaticon.com";
            this.linkLabelAttributionIconPart2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAttributionIconPart2_LinkClicked);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Is still running minimized in taskbar notification area. Near the clock below. 👇" +
    "";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "NoMoreClick";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 48);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.restoreToolStripMenuItem.Text = "Open";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // numericUpDownPostClickDeadzoneRadius
            // 
            this.numericUpDownPostClickDeadzoneRadius.Location = new System.Drawing.Point(12, 36);
            this.numericUpDownPostClickDeadzoneRadius.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownPostClickDeadzoneRadius.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPostClickDeadzoneRadius.Name = "numericUpDownPostClickDeadzoneRadius";
            this.numericUpDownPostClickDeadzoneRadius.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPostClickDeadzoneRadius.TabIndex = 4;
            this.numericUpDownPostClickDeadzoneRadius.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownPostClickDeadzoneRadius.ValueChanged += new System.EventHandler(this.numericUpDownPostClickDeadzoneRadius_ValueChanged);
            // 
            // labelPostClickDeadzoneRadius
            // 
            this.labelPostClickDeadzoneRadius.AutoSize = true;
            this.labelPostClickDeadzoneRadius.Location = new System.Drawing.Point(12, 17);
            this.labelPostClickDeadzoneRadius.Name = "labelPostClickDeadzoneRadius";
            this.labelPostClickDeadzoneRadius.Size = new System.Drawing.Size(133, 13);
            this.labelPostClickDeadzoneRadius.TabIndex = 5;
            this.labelPostClickDeadzoneRadius.Text = "PostClickDeadzoneRadius";
            // 
            // labelClickDelayMs
            // 
            this.labelClickDelayMs.AutoSize = true;
            this.labelClickDelayMs.Location = new System.Drawing.Point(12, 78);
            this.labelClickDelayMs.Name = "labelClickDelayMs";
            this.labelClickDelayMs.Size = new System.Drawing.Size(74, 13);
            this.labelClickDelayMs.TabIndex = 7;
            this.labelClickDelayMs.Text = "ClickDelayMs ";
            // 
            // numericUpDownClickDelayMs
            // 
            this.numericUpDownClickDelayMs.Location = new System.Drawing.Point(12, 97);
            this.numericUpDownClickDelayMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownClickDelayMs.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownClickDelayMs.Name = "numericUpDownClickDelayMs";
            this.numericUpDownClickDelayMs.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownClickDelayMs.TabIndex = 6;
            this.numericUpDownClickDelayMs.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownClickDelayMs.ValueChanged += new System.EventHandler(this.numericUpDownClickDelayMs_ValueChanged);
            // 
            // labelClickDelayAfterRightClickMs
            // 
            this.labelClickDelayAfterRightClickMs.AutoSize = true;
            this.labelClickDelayAfterRightClickMs.Location = new System.Drawing.Point(12, 143);
            this.labelClickDelayAfterRightClickMs.Name = "labelClickDelayAfterRightClickMs";
            this.labelClickDelayAfterRightClickMs.Size = new System.Drawing.Size(144, 13);
            this.labelClickDelayAfterRightClickMs.TabIndex = 9;
            this.labelClickDelayAfterRightClickMs.Text = "ClickDelayAfterRightClickMs ";
            // 
            // numericUpDownDelayAfterRightClickMs
            // 
            this.numericUpDownDelayAfterRightClickMs.Location = new System.Drawing.Point(12, 162);
            this.numericUpDownDelayAfterRightClickMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownDelayAfterRightClickMs.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownDelayAfterRightClickMs.Name = "numericUpDownDelayAfterRightClickMs";
            this.numericUpDownDelayAfterRightClickMs.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDelayAfterRightClickMs.TabIndex = 8;
            this.numericUpDownDelayAfterRightClickMs.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownDelayAfterRightClickMs.ValueChanged += new System.EventHandler(this.numericUpDownDelayAfterRightClickMs_ValueChanged);
            // 
            // labelToggleClickAssistanceMs
            // 
            this.labelToggleClickAssistanceMs.AutoSize = true;
            this.labelToggleClickAssistanceMs.Location = new System.Drawing.Point(12, 209);
            this.labelToggleClickAssistanceMs.Name = "labelToggleClickAssistanceMs";
            this.labelToggleClickAssistanceMs.Size = new System.Drawing.Size(131, 13);
            this.labelToggleClickAssistanceMs.TabIndex = 11;
            this.labelToggleClickAssistanceMs.Text = "ToggleClickAssistanceMs ";
            // 
            // numericUpDownToggleClickAssistanceMs
            // 
            this.numericUpDownToggleClickAssistanceMs.Location = new System.Drawing.Point(12, 228);
            this.numericUpDownToggleClickAssistanceMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownToggleClickAssistanceMs.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownToggleClickAssistanceMs.Name = "numericUpDownToggleClickAssistanceMs";
            this.numericUpDownToggleClickAssistanceMs.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownToggleClickAssistanceMs.TabIndex = 10;
            this.numericUpDownToggleClickAssistanceMs.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownToggleClickAssistanceMs.ValueChanged += new System.EventHandler(this.numericUpDownToggleClickAssistanceMs_ValueChanged);
            // 
            // labelNoClickAfterTypingMs
            // 
            this.labelNoClickAfterTypingMs.AutoSize = true;
            this.labelNoClickAfterTypingMs.Location = new System.Drawing.Point(12, 283);
            this.labelNoClickAfterTypingMs.Name = "labelNoClickAfterTypingMs";
            this.labelNoClickAfterTypingMs.Size = new System.Drawing.Size(115, 13);
            this.labelNoClickAfterTypingMs.TabIndex = 13;
            this.labelNoClickAfterTypingMs.Text = "NoClickAfterTypingMs ";
            // 
            // numericUpDownNoClickAfterTypingMs
            // 
            this.numericUpDownNoClickAfterTypingMs.Location = new System.Drawing.Point(12, 302);
            this.numericUpDownNoClickAfterTypingMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterTypingMs.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterTypingMs.Name = "numericUpDownNoClickAfterTypingMs";
            this.numericUpDownNoClickAfterTypingMs.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownNoClickAfterTypingMs.TabIndex = 12;
            this.numericUpDownNoClickAfterTypingMs.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterTypingMs.ValueChanged += new System.EventHandler(this.numericUpDownNoClickAfterTypingMs_ValueChanged);
            // 
            // labelNoClickAfterScrollingMs
            // 
            this.labelNoClickAfterScrollingMs.AutoSize = true;
            this.labelNoClickAfterScrollingMs.Location = new System.Drawing.Point(12, 360);
            this.labelNoClickAfterScrollingMs.Name = "labelNoClickAfterScrollingMs";
            this.labelNoClickAfterScrollingMs.Size = new System.Drawing.Size(120, 13);
            this.labelNoClickAfterScrollingMs.TabIndex = 15;
            this.labelNoClickAfterScrollingMs.Text = "NoClickAfterScrollingMs";
            // 
            // numericUpDownNoClickAfterScrollingMs
            // 
            this.numericUpDownNoClickAfterScrollingMs.Location = new System.Drawing.Point(12, 379);
            this.numericUpDownNoClickAfterScrollingMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterScrollingMs.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterScrollingMs.Name = "numericUpDownNoClickAfterScrollingMs";
            this.numericUpDownNoClickAfterScrollingMs.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownNoClickAfterScrollingMs.TabIndex = 14;
            this.numericUpDownNoClickAfterScrollingMs.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterScrollingMs.ValueChanged += new System.EventHandler(this.numericUpDownNoClickAfterScrollingMs_ValueChanged);
            // 
            // labelNoClickAfterPhysicalMouseClickMs
            // 
            this.labelNoClickAfterPhysicalMouseClickMs.AutoSize = true;
            this.labelNoClickAfterPhysicalMouseClickMs.Location = new System.Drawing.Point(12, 417);
            this.labelNoClickAfterPhysicalMouseClickMs.Name = "labelNoClickAfterPhysicalMouseClickMs";
            this.labelNoClickAfterPhysicalMouseClickMs.Size = new System.Drawing.Size(174, 13);
            this.labelNoClickAfterPhysicalMouseClickMs.TabIndex = 17;
            this.labelNoClickAfterPhysicalMouseClickMs.Text = "NoClickAfterPhysicalMouseClickMs";
            // 
            // numericUpDownNoClickAfterPhysicalMouseClickMs
            // 
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.Location = new System.Drawing.Point(12, 436);
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.Name = "numericUpDownNoClickAfterPhysicalMouseClickMs";
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.TabIndex = 16;
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownNoClickAfterPhysicalMouseClickMs.ValueChanged += new System.EventHandler(this.numericUpDownNoClickAfterPhysicalMouseClickMs_ValueChanged);
            // 
            // labelDoubleClickThresholdToPressShiftKeyMs
            // 
            this.labelDoubleClickThresholdToPressShiftKeyMs.AutoSize = true;
            this.labelDoubleClickThresholdToPressShiftKeyMs.Location = new System.Drawing.Point(12, 471);
            this.labelDoubleClickThresholdToPressShiftKeyMs.Name = "labelDoubleClickThresholdToPressShiftKeyMs";
            this.labelDoubleClickThresholdToPressShiftKeyMs.Size = new System.Drawing.Size(201, 13);
            this.labelDoubleClickThresholdToPressShiftKeyMs.TabIndex = 19;
            this.labelDoubleClickThresholdToPressShiftKeyMs.Text = "doubleClickThresholdToPressShiftKeyMs";
            // 
            // numericUpDownDoubleClickThresholdToPressShiftKeyMs
            // 
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.Location = new System.Drawing.Point(12, 490);
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.Name = "numericUpDownDoubleClickThresholdToPressShiftKeyMs";
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.TabIndex = 18;
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownDoubleClickThresholdToPressShiftKeyMs.ValueChanged += new System.EventHandler(this.numericUpDownDoubleClickThresholdToPressShiftKeyMs_ValueChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 568);
            this.Controls.Add(this.labelDoubleClickThresholdToPressShiftKeyMs);
            this.Controls.Add(this.numericUpDownDoubleClickThresholdToPressShiftKeyMs);
            this.Controls.Add(this.labelNoClickAfterPhysicalMouseClickMs);
            this.Controls.Add(this.numericUpDownNoClickAfterPhysicalMouseClickMs);
            this.Controls.Add(this.labelNoClickAfterScrollingMs);
            this.Controls.Add(this.numericUpDownNoClickAfterScrollingMs);
            this.Controls.Add(this.labelNoClickAfterTypingMs);
            this.Controls.Add(this.numericUpDownNoClickAfterTypingMs);
            this.Controls.Add(this.labelToggleClickAssistanceMs);
            this.Controls.Add(this.numericUpDownToggleClickAssistanceMs);
            this.Controls.Add(this.labelClickDelayAfterRightClickMs);
            this.Controls.Add(this.numericUpDownDelayAfterRightClickMs);
            this.Controls.Add(this.labelClickDelayMs);
            this.Controls.Add(this.numericUpDownClickDelayMs);
            this.Controls.Add(this.labelPostClickDeadzoneRadius);
            this.Controls.Add(this.numericUpDownPostClickDeadzoneRadius);
            this.Controls.Add(this.linkLabelAttributionIconPart2);
            this.Controls.Add(this.linkLabelAttributionIconPart1);
            this.Controls.Add(this.labelAttributionIconPart2);
            this.Controls.Add(this.labelAttributionIconPart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "NoMoreClick";
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPostClickDeadzoneRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClickDelayMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelayAfterRightClickMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToggleClickAssistanceMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoClickAfterTypingMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoClickAfterScrollingMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNoClickAfterPhysicalMouseClickMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDoubleClickThresholdToPressShiftKeyMs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAttributionIconPart1;
        private System.Windows.Forms.Label labelAttributionIconPart2;
        private System.Windows.Forms.LinkLabel linkLabelAttributionIconPart1;
        private System.Windows.Forms.LinkLabel linkLabelAttributionIconPart2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown numericUpDownPostClickDeadzoneRadius;
        private System.Windows.Forms.Label labelPostClickDeadzoneRadius;
        private System.Windows.Forms.Label labelClickDelayMs;
        private System.Windows.Forms.NumericUpDown numericUpDownClickDelayMs;
        private System.Windows.Forms.Label labelClickDelayAfterRightClickMs;
        private System.Windows.Forms.NumericUpDown numericUpDownDelayAfterRightClickMs;
        private System.Windows.Forms.Label labelToggleClickAssistanceMs;
        private System.Windows.Forms.NumericUpDown numericUpDownToggleClickAssistanceMs;
        private System.Windows.Forms.Label labelNoClickAfterTypingMs;
        private System.Windows.Forms.NumericUpDown numericUpDownNoClickAfterTypingMs;
        private System.Windows.Forms.Label labelNoClickAfterScrollingMs;
        private System.Windows.Forms.NumericUpDown numericUpDownNoClickAfterScrollingMs;
        private System.Windows.Forms.Label labelNoClickAfterPhysicalMouseClickMs;
        private System.Windows.Forms.NumericUpDown numericUpDownNoClickAfterPhysicalMouseClickMs;
        private System.Windows.Forms.Label labelDoubleClickThresholdToPressShiftKeyMs;
        private System.Windows.Forms.NumericUpDown numericUpDownDoubleClickThresholdToPressShiftKeyMs;
    }
}

