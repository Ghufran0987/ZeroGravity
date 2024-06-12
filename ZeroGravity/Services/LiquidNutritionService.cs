using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class LiquidNutritionService : ILiquidNutritionService
    {
        private readonly ILogger<LiquidNutritionService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public LiquidNutritionService(ILogger<LiquidNutritionService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public Task<LiquidNutrition> GetByLiquidIntakeIdAsync(int liquidIntakeId, bool includeLiquidComponentNutrition = false)
        {
            var liquidNutrition = _repository.Execute(new GetLiquidNutritionByLiquidIntakeId(liquidIntakeId, includeLiquidComponentNutrition));

            return liquidNutrition;
        }
    }
}