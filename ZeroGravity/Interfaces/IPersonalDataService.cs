using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IPersonalDataService
    {
        Task<PersonalData> GetByAccounIdAsync(int accountId);
        Task<PersonalData> AddAsync(PersonalData personalData, bool saveChanges = true);
        Task<PersonalData> UpdateAsync(PersonalData personalData, bool saveChanges = true);
    }
}
