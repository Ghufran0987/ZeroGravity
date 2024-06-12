using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface ILiquidNutritionService
    {
        Task<LiquidNutrition> GetByLiquidIntakeIdAsync(int liquidIntakeId, bool includeLiquidComponentNutrition);
    }
}
