namespace NoMoreClick;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private const int HoldCueDelayMs = 120;

    private readonly GlobalInput input;
    private readonly DwellController dwell = new(Cursor.Position, Environment.TickCount64);
    private readonly TextFocusMonitor textFocus = new();
    private readonly RightButtonGesture rightGesture = new();
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 20 };
    private readonly long startupNoticeAt;
    private readonly Icon icon;
    private readonly NotifyIcon tray;
    private readonly ContextMenuStrip menu = new();
    private readonly ToolStripMenuItem status = new() { Enabled = false };
    private readonly ToolStripMenuItem toggle = new();
    private SettingsDialog? settingsWindow;
    private AssistanceRipple? ripple;
    private bool toggleRequested;
    private bool replayedRightDown;
    private bool restoringSettings;
    private bool inputFailed;
    private bool disposed;

    public TrayApplicationContext()
    {
        input = new GlobalInput();
        input.KeyChanged += inputEvent => dwell.ObserveKey(inputEvent, Environment.TickCount64);
        input.HandleMouseEvent = ObserveMouse;

        icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? (Icon)SystemIcons.Application.Clone();
        toggle.Click += (_, _) => ToggleAssistance();
        menu.Items.AddRange([status, toggle, new ToolStripSeparator(),
            new ToolStripMenuItem("Settings…", null, (_, _) => ShowSettings()),
            new ToolStripMenuItem("Exit", null, (_, _) => ExitThread())]);
        menu.Opening += (_, _) => UpdateTrayStatus();

        tray = new NotifyIcon { Icon = icon, ContextMenuStrip = menu };
        tray.MouseDoubleClick += (_, e) =>
        {
            if (e.Button == MouseButtons.Left)
                ShowSettings();
        };
        UpdateTrayStatus();
        tray.Visible = true;
        startupNoticeAt = Environment.TickCount64 + 500;

        // Give Explorer time to register the tray icon before showing the first notification.
        timer.Tick += AnnounceStartup;
        timer.Tick += Evaluate;
        timer.Start();
    }

    private void AnnounceStartup(object? sender, EventArgs e)
    {
        if (Environment.TickCount64 < startupNoticeAt)
            return;

        timer.Tick -= AnnounceStartup;
        ShowTrayMessage("👇 Running in the system tray. Right-click the icon for controls.", timeoutMs: 10000);
        if (!rightGesture.IsHolding && !inputFailed)
            ShowAssistanceRipple(Cursor.Position);
    }

    private bool ObserveMouse(GlobalMouseEvent mouseEvent)
    {
        long now = Environment.TickCount64;
        if (dwell.ObserveMouse(mouseEvent, now) && dwell.Enabled && dwell.Settings.ProtectTextFields)
            textFocus.ObserveLeftClick(now, mouseEvent.Position);

        switch (rightGesture.Observe(mouseEvent, now, dwell.Settings.RightButtonHoldMs))
        {
            case RightGestureAction.Suppress:
                return true;
            case RightGestureAction.BeginDrag:
                replayedRightDown = MouseClickUtility.TrySetRightButton(down: true);
                return false;
            case RightGestureAction.EndDrag:
                if (!replayedRightDown)
                    return false;
                replayedRightDown = false;
                return MouseClickUtility.TrySetRightButton(down: false);
            case RightGestureAction.Click:
                return MouseClickUtility.TryClick(MouseButtons.Right);
            case RightGestureAction.Toggle:
                toggleRequested = true;
                return true;
            default:
                return false;
        }
    }

    private void Evaluate(object? sender, EventArgs e)
    {
        if (inputFailed)
            return;
        if (input.CallbackError is not null)
        {
            inputFailed = true;
            SetAssistance(false);
            ShowTrayMessage("Input monitoring stopped. Restart NoMoreClick to resume assistance.", ToolTipIcon.Warning);
            return;
        }

        long now = Environment.TickCount64;
        if (toggleRequested)
        {
            toggleRequested = false;
            ToggleAssistance();
        }
        else if (rightGesture.GetHoldProgress(now, dwell.Settings.RightButtonHoldMs) is float progress
            && progress >= (float)HoldCueDelayMs / dwell.Settings.RightButtonHoldMs)
        {
            ripple ??= new AssistanceRipple();
            ripple.ShowHoldAt(rightGesture.PressOrigin, !dwell.Enabled, progress);
        }
        else
        {
            ripple?.ClearHold();
        }

        bool textProtected = dwell.Settings.ProtectTextFields && textFocus.ShouldSuppressDwell(now);
        bool mouseHeld = rightGesture.IsHolding || GlobalInput.IsKeyDown(Keys.RButton) || GlobalInput.IsKeyDown(Keys.LButton)
            || GlobalInput.IsKeyDown(Keys.MButton) || GlobalInput.IsKeyDown(Keys.XButton1)
            || GlobalInput.IsKeyDown(Keys.XButton2);
        bool control = GlobalInput.IsKeyDown(Keys.ControlKey);
        bool alt = GlobalInput.IsKeyDown(Keys.Menu);
        bool windowsShortcut = GlobalInput.IsKeyDown(Keys.LWin) || GlobalInput.IsKeyDown(Keys.RWin)
            || (control && alt);

        Point position = Cursor.Position;
        ClickRequest? next = dwell.Evaluate(position, now, mouseHeld, textProtected || windowsShortcut,
            control, alt, GlobalInput.IsKeyDown(Keys.ShiftKey));
        if (next is not ClickRequest request)
            return;

        // The cursor can change between timer evaluation and input injection.
        if (Cursor.Position != position)
            return;
        if (!MouseClickUtility.TryClick(request.Button, request.DoubleClick))
        {
            SetAssistance(false);
            ShowTrayMessage("Windows rejected a click. Resume assistance from the tray when ready.", ToolTipIcon.Warning);
            return;
        }

        dwell.CompleteClick(request, Cursor.Position, now);
        if (request.Button == MouseButtons.Left && dwell.Settings.ProtectTextFields)
            textFocus.ObserveLeftClick(now, position);
    }

    private void ToggleAssistance()
    {
        if (inputFailed)
            return;

        SetAssistance(!dwell.Enabled);
    }

    private void SetAssistance(bool enabled)
    {
        Point pointer = Cursor.Position;
        dwell.SetEnabled(enabled, pointer);
        textFocus.Reset();
        UpdateTrayStatus();
        ShowAssistanceRipple(pointer);
    }

    private void ShowAssistanceRipple(Point pointer)
    {
        ripple ??= new AssistanceRipple();
        ripple.ShowAt(pointer, dwell.Enabled);
    }

    private void UpdateTrayStatus()
    {
        string state = inputFailed ? "Input unavailable" : dwell.Enabled ? "Active" : "Paused";
        tray.Text = $"NoMoreClick · {state}";
        status.Text = dwell.Enabled ? $"{state} · {dwell.CurrentDelayMs} ms dwell" : state;
        toggle.Text = dwell.Enabled ? "Pause assistance" : "Resume assistance";
        toggle.Enabled = !inputFailed;
    }

    private void ShowTrayMessage(string message, ToolTipIcon tipIcon = ToolTipIcon.Info, int timeoutMs = 3000) =>
        tray.ShowBalloonTip(timeoutMs, "NoMoreClick", message, tipIcon);

    private void ShowSettings()
    {
        if (settingsWindow is null)
        {
            var window = new SettingsDialog(dwell.Settings);
            settingsWindow = window;
            window.FormClosed += (_, _) => settingsWindow = null;
            window.SettingsApplied += settings =>
            {
                dwell.Configure(settings);
                textFocus.Reset();
                UpdateTrayStatus();
            };
            window.Resize += (_, _) =>
            {
                if (restoringSettings || !window.Visible || window.WindowState != FormWindowState.Minimized)
                    return;

                window.Hide();
                ShowTrayMessage("Settings were minimized. NoMoreClick is still in the notification area.");
            };
        }

        restoringSettings = true;
        try
        {
            // WinForms only restores the native window state once the hidden form is shown.
            settingsWindow.Show();
            settingsWindow.WindowState = FormWindowState.Normal;
            settingsWindow.Activate();
        }
        finally
        {
            restoringSettings = false;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !disposed)
        {
            disposed = true;
            timer.Dispose();
            input.Dispose();
            if (replayedRightDown)
                MouseClickUtility.TrySetRightButton(down: false);
            settingsWindow?.Dispose();
            tray.Visible = false;
            tray.Dispose();
            menu.Dispose();
            icon.Dispose();
            ripple?.Dispose();
        }
        base.Dispose(disposing);
    }
}
