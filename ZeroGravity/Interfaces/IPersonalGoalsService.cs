using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IPersonalGoalsService
    {
        Task<PersonalGoal> GetByAccounIdAsync(int accountId);
        Task<PersonalGoal> AddAsync(PersonalGoal personalGoal, bool saveChanges = true);
        Task<PersonalGoal> UpdateAsync(PersonalGoal personalGoal, bool saveChanges = true);
    }
}
