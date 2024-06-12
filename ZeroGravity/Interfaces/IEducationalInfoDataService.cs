using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IEducationalInfoDataService
    {
        Task<EducationalInfoData> AddAsync(EducationalInfoData educationalInfoData, bool saveChanges = true);

        Task<EducationalInfoData> GetByIdAsync(int id);

        Task<EducationalInfoData> GetRandomByCategoryAsync(string category);

        Task<EducationalInfoData> UpdateAsync(EducationalInfoData educationalInfoData, bool saveChanges = true);
    }
}