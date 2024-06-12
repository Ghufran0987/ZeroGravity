using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IDietPreferenceService
    {
        Task<DietPreference> GetByAccounId(int accountId);
        Task<DietPreference> AddAsync(DietPreference medicalCondition, bool saveChanges = true);
        Task<DietPreference> UpdateAsync(DietPreference dietPreference, bool saveChanges = true);
    }
}