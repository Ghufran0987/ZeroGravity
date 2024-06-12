using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMeditationService
    {
        Task<ApiCallResult<MeditationDataDto>> SaveMeditationDataAsync(MeditationDataDto meditationDataDto, CancellationToken cancellationToken);
        Task<ApiCallResult<List<MeditationDataDto>>> GetAllMeditationDataForDateAsync(DateTime date, CancellationToken cancellationToken);
    }
}
