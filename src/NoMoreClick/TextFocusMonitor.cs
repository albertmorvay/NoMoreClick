using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace NoMoreClick;

internal sealed class TextFocusMonitor
{
    private const int FocusSettleMilliseconds = 45;
    private const int DetectionGraceMilliseconds = 180;
    private const int TextFieldPauseMilliseconds = 1200;

    private sealed record Click(long Time, Point Position);

    private Click? pendingClick;
    private Click? probingClick;
    private Task<bool>? probe;
    private long suppressUntil;

    public void ObserveLeftClick(long now, Point? position = null)
    {
        pendingClick = new Click(now, position ?? Cursor.Position);
        UpdateProbe(now);
    }

    public bool ShouldSuppressDwell(long now)
    {
        UpdateProbe(now);

        Click? recentClick = pendingClick ?? probingClick;
        return now < suppressUntil
            || (recentClick is not null && now - recentClick.Time < DetectionGraceMilliseconds);
    }

    public void Reset()
    {
        pendingClick = null;
        probingClick = null;
        suppressUntil = 0;
    }

    private void UpdateProbe(long now)
    {
        // A provider can hang. Keep at most one probe running, with only the latest click queued.
        if (probe is { IsCompleted: true } completed)
        {
            if (completed.IsCompletedSuccessfully)
            {
                if (completed.Result && probingClick is { } checkedClick && pendingClick is null)
                {
                    suppressUntil = checkedClick.Time + TextFieldPauseMilliseconds;
                }
            }
            else if (completed.Exception is { } error)
            {
                Trace.TraceWarning("Text focus probe failed ({0}).", error.GetBaseException().GetType().Name);
            }

            probe = null;
            probingClick = null;
        }

        if (probe is not null || pendingClick is not { } click)
        {
            return;
        }

        pendingClick = null;
        if (now - click.Time < DetectionGraceMilliseconds)
        {
            probingClick = click;
            probe = Task.Run(() => ProbeAsync(click));
        }
    }

    private static async Task<bool> ProbeAsync(Click click)
    {
        int remaining = (int)Math.Clamp(
            click.Time + FocusSettleMilliseconds - Environment.TickCount64, 0, FocusSettleMilliseconds);
        await Task.Delay(remaining).ConfigureAwait(false);

        if (Environment.TickCount64 - click.Time >= DetectionGraceMilliseconds)
        {
            return false;
        }

        try
        {
            return WasClickedTextFieldFocused(click.Position);
        }
        catch (Exception exception) when (exception is ElementNotAvailableException
            or COMException or UnauthorizedAccessException or InvalidOperationException or NotSupportedException)
        {
            // The foreground window can change or its accessibility provider can become unavailable.
            return false;
        }
    }

    private static bool WasClickedTextFieldFocused(Point position)
    {
        var cache = new CacheRequest();
        cache.Add(AutomationElement.ControlTypeProperty);
        cache.Add(AutomationElement.IsEnabledProperty);
        cache.Add(AutomationElement.HasKeyboardFocusProperty);
        cache.Add(AutomationElement.BoundingRectangleProperty);
        cache.Add(ValuePattern.IsReadOnlyProperty);

        AutomationElement? focused;
        using (cache.Activate())
        {
            focused = AutomationElement.FocusedElement;
        }

        if (focused is null)
        {
            return false;
        }

        var properties = focused.Cached;
        if ((properties.ControlType != ControlType.Edit && properties.ControlType != ControlType.Document)
            || !properties.IsEnabled
            || !properties.HasKeyboardFocus
            || !properties.BoundingRectangle.Contains(position.X, position.Y))
        {
            return false;
        }

        // Check writability without fetching the text or the Value property, including for passwords.
        object readOnly = focused.GetCachedPropertyValue(ValuePattern.IsReadOnlyProperty, ignoreDefaultValue: true);
        if (readOnly is bool isReadOnly)
        {
            return !isReadOnly;
        }

        return focused.TryGetCurrentPattern(TextPattern.Pattern, out object pattern)
            && pattern is TextPattern textPattern
            && textPattern.DocumentRange.GetAttributeValue(TextPattern.IsReadOnlyAttribute) is false;
    }
}
