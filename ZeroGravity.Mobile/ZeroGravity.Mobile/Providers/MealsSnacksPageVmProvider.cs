using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class MealsSnacksPageVmProvider : PageVmProviderBase, IMealsSnacksPageVmProvider
    {
        private readonly IBadgeInformationService _badgeInformationService;

        public MealsSnacksPageVmProvider(ITokenService tokenService, IBadgeInformationService badgeInformationService) :
            base(tokenService)
        {
            _badgeInformationService = badgeInformationService;
        }

        public async Task<ApiCallResult<MealsBadgeInformationProxy>> GetMealsBadgeInformationAsnyc(
            DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var apiCallResult =
                await _badgeInformationService.GetMealsBadgeInformationByDateAsync(targetDateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var mealsBadgeInformationProxy = ProxyConverter.GetMealsBadgeInformationProxy(apiCallResult.Value);

                return ApiCallResult<MealsBadgeInformationProxy>.Ok(mealsBadgeInformationProxy);
            }

            return ApiCallResult<MealsBadgeInformationProxy>.Error(apiCallResult.ErrorMessage,
                apiCallResult.ErrorReason);
        }
    }
}