using System.Threading.Tasks;
using ZeroGravity.Db.Models.SugarBeatData;

namespace ZeroGravity.Interfaces
{
    public interface ISugarBeatSettingDataService
    {
        Task<SugarBeatSettingData> AddAsync(SugarBeatSettingData glucoseData, bool saveChanges = true);

        Task<SugarBeatSettingData> UpdateAsync(SugarBeatSettingData glucoseData, bool saveChanges = true);

        Task<SugarBeatSettingData> GetByAccountIdAsync(int accountId);
    }
}