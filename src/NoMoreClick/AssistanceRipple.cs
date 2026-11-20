namespace NoMoreClick;

internal sealed class AssistanceRipple : Form
{
    private const int DurationMs = 520;
    private const int CanvasSize = 28;
    private const int TopmostWindow = 0x00000008;
    private const int TransparentWindow = 0x00000020;
    private const int ToolWindow = 0x00000080;
    private const int NoActivate = 0x08000000;

    private readonly System.Windows.Forms.Timer animation = new() { Interval = 16 };
    private long startedAt;
    private bool assistanceEnabled;
    private float? holdProgress;

    public AssistanceRipple()
    {
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        ShowInTaskbar = false;
        DoubleBuffered = true;
        BackColor = Color.Magenta;
        TransparencyKey = BackColor;
        ClientSize = new Size(CanvasSize, CanvasSize);
        animation.Tick += (_, _) =>
        {
            if (Environment.TickCount64 - startedAt >= DurationMs)
            {
                animation.Stop();
                Hide();
            }
            else if (!assistanceEnabled)
            {
                Invalidate();
            }
        };
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            var parameters = base.CreateParams;
            // WinForms focuses topmost forms on first show when its managed TopMost property is set.
            parameters.ExStyle |= TopmostWindow | TransparentWindow | ToolWindow | NoActivate;
            return parameters;
        }
    }

    public void ShowAt(Point pointer, bool enabled)
    {
        holdProgress = null;
        assistanceEnabled = enabled;
        startedAt = Environment.TickCount64;
        ShowAtPointer(pointer);
        animation.Start();
    }

    public void ShowHoldAt(Point pointer, bool enabling, float progress)
    {
        if (holdProgress == progress && assistanceEnabled == enabling)
            return;

        animation.Stop();
        holdProgress = progress;
        assistanceEnabled = enabling;
        ShowAtPointer(pointer);
    }

    public void ClearHold()
    {
        if (holdProgress is null)
            return;

        holdProgress = null;
        Hide();
    }

    private void ShowAtPointer(Point pointer)
    {
        Point location = new(pointer.X - ClientSize.Width / 2, pointer.Y - ClientSize.Height / 2);
        if (Location != location)
            Location = location;
        if (!Visible)
            Show();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        float progress = Math.Clamp((Environment.TickCount64 - startedAt) / (float)DurationMs, 0, 1);
        const float readyDiameter = 10;
        Color neon = Color.FromArgb(57, 255, 20);
        Color orange = Color.FromArgb(255, 104, 31);
        float pulse = assistanceEnabled ? 0 : 1 - Math.Abs(2 * progress - 1);
        float diameter = assistanceEnabled ? readyDiameter
            : progress <= 0.5f ? readyDiameter * (1 + pulse)
            : 2 * readyDiameter * pulse;
        Color fill = Color.FromArgb(
            (int)(neon.R + (orange.R - neon.R) * pulse),
            (int)(neon.G + (orange.G - neon.G) * pulse),
            (int)(neon.B + (orange.B - neon.B) * pulse));
        if (holdProgress is float held)
        {
            bool ready = held >= 1;
            diameter = ready ? readyDiameter : 4 + 4 * held;
            fill = assistanceEnabled
                ? ready ? neon : Color.FromArgb(34, 160, 16)
                : ready ? orange : Color.FromArgb(170, 66, 20);
        }
        if (diameter > 0)
        {
            float x = (ClientSize.Width - diameter) / 2f;
            float y = (ClientSize.Height - diameter) / 2f;
            using var brush = new SolidBrush(fill);
            e.Graphics.FillEllipse(brush, x, y, diameter, diameter);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            animation.Dispose();
        base.Dispose(disposing);
    }
}
