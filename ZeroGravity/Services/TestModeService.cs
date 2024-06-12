using Microsoft.Extensions.Options;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class TestModeService : ITestModeService
    {
        private readonly AppSettings _appSettings;

        public TestModeService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public bool GetTestModeInfo()
        {
            return _appSettings.InTestMode;
        }

        public void SetTestMode(bool isActive)
        {
            _appSettings.InTestMode = isActive;
        }
    }
}