using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IMedicalConditionService
    {
        Task<MedicalCondition> GetByAccounId(int accountId);
        Task<MedicalCondition> AddAsync(MedicalCondition medicalCondition, bool saveChanges = true);
        Task<MedicalCondition> UpdateAsync(MedicalCondition medicalCondition, bool saveChanges = true);
    }
}