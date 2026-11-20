namespace NoMoreClick;

internal enum RightGestureAction
{
    Forward,
    Suppress,
    BeginDrag,
    EndDrag,
    Click,
    Toggle
}

internal sealed class RightButtonGesture
{
    private enum PressState { None, Pending, Dragging, Cancelled }

    private PressState state;
    private MouseButtons otherButtons;
    private Size dragSize;
    private long pressedAt;

    public bool IsHolding => state != PressState.None;
    public Point PressOrigin { get; private set; }

    public float? GetHoldProgress(long now, int holdMs) =>
        state == PressState.Pending ? Math.Clamp((now - pressedAt) / (float)holdMs, 0, 1) : null;

    public RightGestureAction Observe(GlobalMouseEvent input, long now, int holdMs)
    {
        if (input.Button != MouseButtons.Right)
        {
            if (input.Kind == GlobalMouseEventKind.ButtonDown)
                otherButtons |= input.Button;
            else if (input.Kind == GlobalMouseEventKind.ButtonUp)
                otherButtons &= ~input.Button;

            if (state == PressState.Pending && (otherButtons != MouseButtons.None
                || input.Kind == GlobalMouseEventKind.Wheel && !input.IsInjected))
                state = PressState.Cancelled;
        }

        if (input.IsInjected)
            return RightGestureAction.Forward;

        if (input.Button == MouseButtons.Right && input.Kind == GlobalMouseEventKind.ButtonDown)
        {
            if (state == PressState.None)
            {
                state = otherButtons == MouseButtons.None ? PressState.Pending : PressState.Cancelled;
                pressedAt = now;
                PressOrigin = input.Position;
                dragSize = SystemInformation.DragSize;
            }
            return RightGestureAction.Suppress;
        }

        if (input.Button == MouseButtons.Right && input.Kind == GlobalMouseEventKind.ButtonUp)
        {
            PressState previous = state;
            state = PressState.None;
            return previous switch
            {
                PressState.None => RightGestureAction.Forward,
                PressState.Dragging => RightGestureAction.EndDrag,
                PressState.Pending when IsWithinDragThreshold(input.Position) =>
                    now - pressedAt >= holdMs ? RightGestureAction.Toggle : RightGestureAction.Click,
                _ => RightGestureAction.Suppress
            };
        }

        if (state == PressState.Pending && input.Kind == GlobalMouseEventKind.Move
            && !IsWithinDragThreshold(input.Position))
        {
            state = PressState.Dragging;
            return RightGestureAction.BeginDrag;
        }

        return RightGestureAction.Forward;
    }

    private bool IsWithinDragThreshold(Point position) =>
        Math.Abs((long)position.X - PressOrigin.X) <= dragSize.Width / 2
        && Math.Abs((long)position.Y - PressOrigin.Y) <= dragSize.Height / 2;
}
