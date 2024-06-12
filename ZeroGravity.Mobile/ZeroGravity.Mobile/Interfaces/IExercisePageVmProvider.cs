using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IExercisePageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<ExerciseActivityProxy>> GetExerciseActivityAsnyc(int activityId, CancellationToken cancellationToken);
        Task<ApiCallResult<ExerciseActivityProxy>> GetExerciseActivityByDateAsnyc(DateTime dateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<ExerciseActivityProxy>> CreateExerciseActivityAsnyc(ExerciseActivityProxy exerciseActivityProxy, CancellationToken cancellationToken);
        Task<ApiCallResult<ExerciseActivityProxy>> UpdateExerciseActivitysnyc(ExerciseActivityProxy exerciseActivityProxy, CancellationToken cancellationToken);
        ObservableCollection<ComboBoxItem> GetExerciseItems();
    }
}