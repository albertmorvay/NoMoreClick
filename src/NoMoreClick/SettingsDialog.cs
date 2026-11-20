namespace NoMoreClick;

internal sealed class SettingsDialog : Form
{
    private readonly TableLayoutPanel layout = new()
    {
        AutoSize = true,
        Padding = new Padding(12),
        Dock = DockStyle.Fill,
        ColumnCount = 2
    };

    public event Action<AssistanceSettings>? SettingsApplied;

    public SettingsDialog(AssistanceSettings settings)
    {
        Text = "NoMoreClick settings";
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        Controls.Add(layout);

        var delay = AddNumber("Dwell delay (ms)", settings.ClickDelayMs,
            AssistanceSettings.MinimumDwellDelayMs, AssistanceSettings.MaximumDwellDelayMs);
        var rightCooldown = AddNumber("Pause after right-click (ms)", settings.RightClickCooldownMs, 0, 5000);
        var deadzone = AddNumber("Restart distance after a click (px)", settings.PostClickDeadzoneRadius, 1, 150);
        var hold = AddNumber("Hold right button to toggle (ms)", settings.RightButtonHoldMs, 500, 5000);
        var typing = AddNumber("Pause after typing (ms)", settings.TypingCooldownMs, 0, 5000);
        var scrolling = AddNumber("Pause after scrolling (ms)", settings.ScrollCooldownMs, 0, 5000);
        var physical = AddNumber("Pause after a manual click (ms)", settings.PhysicalClickCooldownMs, 0, 5000);
        var shift = AddNumber("Double-Shift interval (ms)", settings.DoubleShiftWindowMs, 100, 1000);

        var modifier = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120 };
        modifier.Items.AddRange(["Alt", "Shift", "Disabled"]);
        modifier.SelectedIndex = (int)settings.RightClickModifier;
        AddRow("Hold for dwell right-click", modifier);

        var adaptive = new CheckBox { Text = "Adapt delay from manual clicks", AutoSize = true, Checked = settings.AdaptiveDelay };
        var protect = new CheckBox { Text = "Pause after clicking into a text field", AutoSize = true, Checked = settings.ProtectTextFields };
        AddFullRow(adaptive);
        AddFullRow(protect);

        var save = new Button { Text = "Apply", AutoSize = true };
        var cancel = new Button { Text = "Cancel", AutoSize = true };
        var buttons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
        buttons.Controls.AddRange([save, cancel]);
        AddFullRow(buttons);
        AcceptButton = save;
        CancelButton = cancel;
        cancel.Click += (_, _) => Close();
        save.Click += (_, _) =>
        {
            SettingsApplied?.Invoke(new AssistanceSettings
            {
                ClickDelayMs = (int)delay.Value,
                RightClickCooldownMs = (int)rightCooldown.Value,
                PostClickDeadzoneRadius = (int)deadzone.Value,
                RightButtonHoldMs = (int)hold.Value,
                TypingCooldownMs = (int)typing.Value,
                ScrollCooldownMs = (int)scrolling.Value,
                PhysicalClickCooldownMs = (int)physical.Value,
                DoubleShiftWindowMs = (int)shift.Value,
                AdaptiveDelay = adaptive.Checked,
                ProtectTextFields = protect.Checked,
                RightClickModifier = (RightClickModifier)modifier.SelectedIndex
            });
            Close();
        };
    }

    private NumericUpDown AddNumber(string label, int value, int minimum, int maximum)
    {
        var number = new NumericUpDown { Minimum = minimum, Maximum = maximum, Value = value, Width = 120 };
        AddRow(label, number);
        return number;
    }

    private void AddRow(string label, Control control)
    {
        int row = layout.RowCount++;
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
        layout.Controls.Add(control, 1, row);
    }

    private void AddFullRow(Control control)
    {
        int row = layout.RowCount++;
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(control, 0, row);
        layout.SetColumnSpan(control, 2);
    }
}
