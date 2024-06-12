using System.Threading;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Services
{
    public class SemaphoreAsyncLockService : ISemaphoreAsyncLockService
    {
        public SemaphoreAsyncLockService()
        {
            SemaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public SemaphoreSlim SemaphoreSlim { get; }
    }
}
