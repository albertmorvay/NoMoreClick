namespace NoMoreClick;

internal enum RightClickModifier
{
    Alt,
    Shift,
    Disabled
}

internal sealed record AssistanceSettings
{
    public const int MinimumDwellDelayMs = 100;
    public const int MaximumDwellDelayMs = 3000;

    public int ClickDelayMs { get; init; } = 200;
    public int RightClickCooldownMs { get; init; } = 600;
    public int PostClickDeadzoneRadius { get; init; } = 20;
    public int RightButtonHoldMs { get; init; } = 1080;
    public int TypingCooldownMs { get; init; } = 1000;
    public int ScrollCooldownMs { get; init; } = 1000;
    public int PhysicalClickCooldownMs { get; init; } = 600;
    public int DoubleShiftWindowMs { get; init; } = 300;
    public bool AdaptiveDelay { get; init; }
    public bool ProtectTextFields { get; init; } = true;
    public RightClickModifier RightClickModifier { get; init; } = RightClickModifier.Alt;
}
