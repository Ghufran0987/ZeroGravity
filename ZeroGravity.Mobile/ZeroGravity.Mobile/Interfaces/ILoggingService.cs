namespace ZeroGravity.Mobile.Interfaces
{
    /// <summary>
    /// Interface to log platform-specific to a text file (and save on device).
    /// </summary>
    public interface ILoggingService
    {
        void Log(string text);
    }
}
