using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Interfaces
{
    public interface ILiquidIntakeService
    {
        Task<List<LiquidIntake>> GetByAccoundAndDateAsync(int accountId, DateTime targetDate);
        Task<List<LiquidIntake>> GetByAccountAndDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate);
        Task<List<LiquidIntake>> GetByAccountAndDateAndTypeAsync(int accountId, DateTime targetDate, LiquidType type);
        Task<LiquidIntake> GetByIdAsync(int id);
        Task<LiquidIntake> AddAsync(LiquidIntake liquidIntake, bool saveChanges = true);
        Task<LiquidIntake> UpdateAsync(LiquidIntake liquidIntake, bool saveChanges = true);
    }
}