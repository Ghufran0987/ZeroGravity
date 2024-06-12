using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IMeditationDataService
    {
        Task<MeditationData> AddAsync(MeditationData activityData, bool saveChanges = true);
        Task<MeditationData> GetByIdAsync(int id);
        Task<List<MeditationData>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime);
    }
}
