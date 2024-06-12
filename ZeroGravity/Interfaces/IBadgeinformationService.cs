using System;
using System.Threading.Tasks;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Interfaces
{
    public interface IBadgeinformationService
    {
        Task<ActivityBadgeInformationDto> GetActivityBadgeInformationAsync(int accountId, DateTime dateTime);
        Task<MealsBadgeInformationDto> GetMealsBadgeInformationAsync(int accountId, DateTime dateTime);
        Task<LiquidIntakeBadgeInformationDto> GetLiquidIntakeBadgeInformationAsync(int accountId, DateTime dateTime);
        Task<MyDayBadgeInformationDto> GetMyDayBadgeInformationAsync(int accountId, DateTime dateTime);
        Task<WellbeingType> GetWellbeingBadgeInformationAsync(int accountId, DateTime dateTime);
    }
}