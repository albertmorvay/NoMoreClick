using Config.Net;

namespace NoMoreClick
{
    public interface IMySettings
    {
        int ClickDelayMs { get; }
        int ClickDelayAfterRightClickMs { get; }
        int PostClickDeadzoneRadius { get; }
        int ToggleClickAssistanceMs { get; }
        int NoClickAfterTypingMs { get; }
        int NoClickAfterScrollingMs { get; }
        int NoClickAfterPhysicalMouseClickMs { get; }
        string WavFileLocationMouseAssistOn { get; }
        string WavFileLocationMouseAssistOff { get; }
    }
}
