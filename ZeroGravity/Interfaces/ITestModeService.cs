namespace ZeroGravity.Interfaces
{
    public interface ITestModeService
    {
        bool GetTestModeInfo();
        void SetTestMode(bool isActive);
    }
}