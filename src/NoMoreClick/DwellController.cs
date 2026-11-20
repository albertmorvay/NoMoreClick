namespace NoMoreClick;

internal readonly record struct ClickRequest(MouseButtons Button, bool DoubleClick);

internal sealed class DwellController(Point startingPosition, long startedAt)
{
    private Point pointer = startingPosition;
    private Point clickAnchor = startingPosition;
    private long? lastMovementAt;
    private long blockedUntil = startedAt;
    private bool leftDeadzone;
    private bool doubleClickArmed;
    private long? lastShiftRelease;
    private long? rightButtonPressedAt;
    private bool manualClickInProgress;
    private int? pendingManualDwell;
    private int? completedManualDwell;
    private Point pendingManualPosition;
    private Point lastManualClickPosition;
    private long? lastManualClickAt;
    private AdaptiveDelay adaptiveDelay = new(new AssistanceSettings().ClickDelayMs);

    public bool Enabled { get; private set; } = true;
    public AssistanceSettings Settings { get; private set; } = new();
    public int CurrentDelayMs => Settings.AdaptiveDelay ? adaptiveDelay.CurrentMs : Settings.ClickDelayMs;

    public void Configure(AssistanceSettings settings)
    {
        if (settings.ClickDelayMs != Settings.ClickDelayMs || settings.AdaptiveDelay != Settings.AdaptiveDelay)
        {
            adaptiveDelay = new AdaptiveDelay(settings.ClickDelayMs);
            pendingManualDwell = null;
            completedManualDwell = null;
        }
        Settings = settings;
        RearmFrom(pointer);
    }

    public void SetEnabled(bool enabled, Point position)
    {
        Enabled = enabled;
        doubleClickArmed = false;
        lastShiftRelease = null;
        manualClickInProgress = false;
        pendingManualDwell = null;
        completedManualDwell = null;
        rightButtonPressedAt = null;
        RearmFrom(position);
    }

    public void ObserveKey(GlobalKeyEvent input, long now)
    {
        switch (input.Key)
        {
            case Keys.LShiftKey:
                if (!input.IsDown && !input.IsInjected && Enabled)
                {
                    if (lastShiftRelease is long first && now - first <= Settings.DoubleShiftWindowMs)
                    {
                        doubleClickArmed = true;
                        lastShiftRelease = null;
                    }
                    else
                    {
                        lastShiftRelease = now;
                    }
                }
                break;
            case Keys.ShiftKey or Keys.RShiftKey:
            case Keys.ControlKey or Keys.LControlKey or Keys.RControlKey:
            case Keys.Menu or Keys.LMenu or Keys.RMenu:
                break;
            default:
                if (input.IsDown)
                {
                    lastShiftRelease = null;
                    BlockUntil(now + Settings.TypingCooldownMs);
                }
                break;
        }
    }

    // True when a completed manual left click should trigger a focus check.
    public bool ObserveMouse(GlobalMouseEvent input, long now)
    {
        UpdatePointer(input.Position, now);
        switch (input.Kind)
        {
            case GlobalMouseEventKind.Move:
                if (manualClickInProgress
                    && !WithinRadius(pointer, pendingManualPosition, SystemInformation.DragSize))
                    manualClickInProgress = false;
                break;
            case GlobalMouseEventKind.Wheel:
                manualClickInProgress = false;
                BlockUntil(now + Settings.ScrollCooldownMs);
                break;
            case GlobalMouseEventKind.ButtonDown:
                ObserveButtonDown(input, now);
                break;
            case GlobalMouseEventKind.ButtonUp:
                BlockUntil(now + Settings.PhysicalClickCooldownMs);
                if (input.Button == MouseButtons.Right)
                {
                    rightButtonPressedAt = null;
                    BlockUntil(now + Settings.RightClickCooldownMs);
                }
                if (input.Button == MouseButtons.Left)
                    return FinishManualClick(input.IsInjected, now);
                break;
        }
        return false;
    }

    public bool TakeRightButtonHold(long now, bool rightButtonDown)
    {
        if (!rightButtonDown)
            rightButtonPressedAt = null;
        if (rightButtonPressedAt is not long pressedAt || now - pressedAt < Settings.RightButtonHoldMs)
            return false;

        rightButtonPressedAt = null;
        return true;
    }

    public ClickRequest? Evaluate(Point position, long now, bool mouseButtonHeld, bool blockedByUi,
        bool controlHeld, bool altHeld, bool shiftHeld)
    {
        UpdatePointer(position, now);
        if (completedManualDwell.HasValue && lastManualClickAt is long lastClick
            && now - lastClick > SystemInformation.DoubleClickTime)
            CommitAdaptiveSample();

        if (!Enabled || now < blockedUntil || mouseButtonHeld || blockedByUi)
        {
            RearmFrom(position);
            return null;
        }

        if (!leftDeadzone || lastMovementAt is not long movedAt || now - movedAt < CurrentDelayMs)
            return null;

        bool rightClick = !controlHeld && (Settings.RightClickModifier switch
        {
            RightClickModifier.Alt => altHeld,
            RightClickModifier.Shift => shiftHeld,
            _ => false
        });
        // A held Ctrl modifier belongs to the foreground application, including Ctrl+click in browsers.
        return rightClick
            ? new ClickRequest(MouseButtons.Right, false)
            : new ClickRequest(MouseButtons.Left, doubleClickArmed && !controlHeld && !altHeld && !shiftHeld);
    }

    public void CompleteClick(ClickRequest request, Point position, long now)
    {
        doubleClickArmed = false;
        lastShiftRelease = null;
        RearmFrom(position);
        if (request.Button == MouseButtons.Right)
            BlockUntil(now + Settings.RightClickCooldownMs);
    }

    private void ObserveButtonDown(GlobalMouseEvent input, long now)
    {
        if (input.Button == MouseButtons.Right && !input.IsInjected)
            rightButtonPressedAt = now;

        if (input.Button == MouseButtons.Left)
        {
            manualClickInProgress = !input.IsInjected;
            pendingManualDwell = null;
            pendingManualPosition = pointer;
            bool repeatsLastClick = lastManualClickAt is long previous
                && now - previous <= SystemInformation.DoubleClickTime
                && WithinRadius(pointer, lastManualClickPosition, SystemInformation.DoubleClickSize);
            if (manualClickInProgress)
            {
                // Wait until the system's double-click interval resolves before learning from a click.
                if (repeatsLastClick)
                    completedManualDwell = null;
                else
                    CommitAdaptiveSample();
            }
            if (Enabled && Settings.AdaptiveDelay && manualClickInProgress && !repeatsLastClick
                && lastMovementAt is long movedAt && movedAt > (lastManualClickAt ?? long.MinValue)
                && now >= blockedUntil)
                pendingManualDwell = (int)Math.Min(now - movedAt, int.MaxValue);
        }
        BlockUntil(now + Settings.PhysicalClickCooldownMs);
    }

    private bool FinishManualClick(bool injected, long now)
    {
        bool clicked = manualClickInProgress && !injected
            && WithinRadius(pointer, pendingManualPosition, SystemInformation.DragSize);
        manualClickInProgress = false;
        if (clicked)
        {
            lastManualClickAt = now;
            lastManualClickPosition = pointer;
            completedManualDwell = pendingManualDwell;
        }
        pendingManualDwell = null;
        return clicked;
    }

    private void CommitAdaptiveSample()
    {
        if (completedManualDwell is int dwell)
            adaptiveDelay.Observe(dwell);
        completedManualDwell = null;
    }

    private void BlockUntil(long until)
    {
        blockedUntil = Math.Max(blockedUntil, until);
        RearmFrom(pointer);
    }

    private void UpdatePointer(Point position, long now)
    {
        if (pointer == position)
            return;

        pointer = position;
        lastMovementAt = now;
        int radius = Settings.PostClickDeadzoneRadius;
        if (!WithinRadius(pointer, clickAnchor, new Size(radius * 2, radius * 2)))
            leftDeadzone = true;
    }

    private void RearmFrom(Point position)
    {
        pointer = position;
        clickAnchor = position;
        leftDeadzone = false;
    }

    private static bool WithinRadius(Point point, Point center, Size size) =>
        Math.Abs((long)point.X - center.X) <= size.Width / 2
        && Math.Abs((long)point.Y - center.Y) <= size.Height / 2;
}
