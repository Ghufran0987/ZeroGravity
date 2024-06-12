using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMedicalPreConditionsPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<MedicalPreconditionsProxy>> GetMedicalPreConditionsAsnyc(CancellationToken cancellationToken);

        Task<ApiCallResult<MedicalPreconditionsProxy>> CreateMedicalConditionsAsnyc(MedicalPreconditionsProxy medicalPreconditionsProxy, CancellationToken cancellationToken);

        Task<ApiCallResult<MedicalPreconditionsProxy>> UpdateMedicalConditionsAsnyc(MedicalPreconditionsProxy medicalPreconditionsProxy, CancellationToken cancellationToken);
    }
}