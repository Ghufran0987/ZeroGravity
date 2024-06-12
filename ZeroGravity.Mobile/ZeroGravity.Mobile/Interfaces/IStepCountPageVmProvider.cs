using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IStepCountPageVmProvider : IPageVmProvider
    {
        int GetCurrentSteps();

        void InitSensorService();

        void StopSensorService();

        bool IsSensorAvailable();

        Task<ApiCallResult<StepCountDataProxy>> GetStepCountDataAsync(int stepCountId,
            CancellationToken cancellationToken);

        Task<ApiCallResult<StepCountDataProxy>> GetStepCountDataByDateAsync(DateTime dateTime,
            CancellationToken cancellationToken);

        Task<ApiCallResult<StepCountDataProxy>> CreateStepCountDataAsync(StepCountDataProxy stepCountDataProxy,
            CancellationToken cancellationToken);

        Task<ApiCallResult<StepCountDataProxy>> UpdateStepCountDataAsync(StepCountDataProxy stepCountDataProxy,
            CancellationToken cancellationToken);
    }
}