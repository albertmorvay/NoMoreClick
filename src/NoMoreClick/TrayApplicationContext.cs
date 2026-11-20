using System.Media;
using System.Reflection;

namespace NoMoreClick;

internal sealed class TrayApplicationContext : ApplicationContext
{
    private readonly GlobalInput input;
    private readonly DwellController dwell = new(Cursor.Position, Environment.TickCount64);
    private readonly TextFocusMonitor textFocus = new();
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 20 };
    private readonly Icon icon;
    private readonly NotifyIcon tray;
    private readonly ContextMenuStrip menu = new();
    private readonly ToolStripMenuItem status = new() { Enabled = false };
    private readonly ToolStripMenuItem toggle = new();
    private readonly SoundPlayer enabledSound = CreateSound("NoMoreClick.AssistOn");
    private readonly SoundPlayer pausedSound = CreateSound("NoMoreClick.AssistOff");
    private SettingsDialog? settingsWindow;
    private bool restoringSettings;
    private bool inputFailed;
    private bool disposed;

    public TrayApplicationContext()
    {
        input = new GlobalInput();
        input.KeyChanged += inputEvent => dwell.ObserveKey(inputEvent, Environment.TickCount64);
        input.MouseChanged += ObserveMouse;

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

        // Give Explorer time to register the tray icon before showing the first notification.
        timer.Tick += AnnounceStartup;
        timer.Tick += Evaluate;
        timer.Start();
    }

    private void AnnounceStartup(object? sender, EventArgs e)
    {
        timer.Tick -= AnnounceStartup;
        ShowTrayMessage("NoMoreClick is running in the notification area. Open its tray menu to pause or change settings.");
    }

    private void ObserveMouse(GlobalMouseEvent mouseEvent)
    {
        long now = Environment.TickCount64;
        if (dwell.ObserveMouse(mouseEvent, now) && dwell.Enabled && dwell.Settings.ProtectTextFields)
            textFocus.ObserveLeftClick(now, mouseEvent.Position);
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
        bool rightDown = GlobalInput.IsKeyDown(Keys.RButton);
        if (dwell.TakeRightButtonHold(now, rightDown))
            ToggleAssistance();

        bool textProtected = dwell.Settings.ProtectTextFields && textFocus.ShouldSuppressDwell(now);
        bool mouseHeld = rightDown || GlobalInput.IsKeyDown(Keys.LButton)
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
        try
        {
            (dwell.Enabled ? enabledSound : pausedSound).Play();
        }
        catch (InvalidOperationException error)
        {
            System.Diagnostics.Trace.TraceWarning("Could not play the assistance sound: {0}", error.Message);
        }
    }

    private void SetAssistance(bool enabled)
    {
        dwell.SetEnabled(enabled, Cursor.Position);
        textFocus.Reset();
        UpdateTrayStatus();
    }

    private void UpdateTrayStatus()
    {
        string state = inputFailed ? "Input unavailable" : dwell.Enabled ? "Active" : "Paused";
        tray.Text = $"NoMoreClick · {state}";
        status.Text = dwell.Enabled ? $"{state} · {dwell.CurrentDelayMs} ms dwell" : state;
        toggle.Text = dwell.Enabled ? "Pause assistance" : "Resume assistance";
        toggle.Enabled = !inputFailed;
    }

    private void ShowTrayMessage(string message, ToolTipIcon tipIcon = ToolTipIcon.Info) =>
        tray.ShowBalloonTip(3000, "NoMoreClick", message, tipIcon);

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
            settingsWindow?.Dispose();
            tray.Visible = false;
            tray.Dispose();
            menu.Dispose();
            icon.Dispose();
            enabledSound.Stream?.Dispose();
            enabledSound.Dispose();
            pausedSound.Stream?.Dispose();
            pausedSound.Dispose();
        }
        base.Dispose(disposing);
    }

    private static SoundPlayer CreateSound(string name) =>
        new(Assembly.GetExecutingAssembly().GetManifestResourceStream(name));
}
