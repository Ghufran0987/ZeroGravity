namespace ZeroGravity.Mobile.Interfaces
{
    public interface IStepCounterService
    {
        int GetSteps();

        bool IsAvailable();

        void InitSensorService();

        void StopSensorService();
    }
}