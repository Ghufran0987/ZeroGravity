using System.Threading;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ISemaphoreAsyncLockService
    {
        SemaphoreSlim SemaphoreSlim { get; }
    }
}
