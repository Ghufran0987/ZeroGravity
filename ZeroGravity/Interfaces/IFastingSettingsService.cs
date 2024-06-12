using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IFastingSettingsService
    {
        Task<FastingSetting> GetByAccounIdAsync(int accountId);
        Task<FastingSetting> AddAsync(FastingSetting fastingSetting, bool saveChanges = true);
        Task<FastingSetting> UpdateAsync(FastingSetting fastingSetting, bool saveChanges = true);
    }
}