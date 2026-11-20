namespace NoMoreClick;

internal sealed class AdaptiveDelay
{
    private const int MinimumSamples = 7;
    private const int WindowSize = 15;
    private const int MaximumStepMs = 10;
    private readonly Queue<int> samples = new(WindowSize);

    private readonly int baselineMs;

    public AdaptiveDelay(int baselineMs)
    {
        this.baselineMs = baselineMs;
        CurrentMs = baselineMs;
    }

    public int CurrentMs { get; private set; }

    public void Observe(long millisecondsAfterMovement)
    {
        // Ignore clicks made while moving or after lingering over the same spot.
        if (millisecondsAfterMovement is < 40 or > 800)
            return;

        if (samples.Count == WindowSize)
            samples.Dequeue();
        samples.Enqueue((int)millisecondsAfterMovement);
        if (samples.Count < MinimumSamples)
            return;

        int[] sorted = samples.ToArray();
        Array.Sort(sorted);
        int median = (sorted[(sorted.Length - 1) / 2] + sorted[sorted.Length / 2]) / 2;
        int lower = Math.Max(AssistanceSettings.MinimumDwellDelayMs, baselineMs * 3 / 4);
        int upper = Math.Min(AssistanceSettings.MaximumDwellDelayMs, baselineMs * 5 / 4);
        int target = Math.Clamp(median + 25, lower, upper);
        CurrentMs += Math.Clamp(target - CurrentMs, -MaximumStepMs, MaximumStepMs);
    }
}
