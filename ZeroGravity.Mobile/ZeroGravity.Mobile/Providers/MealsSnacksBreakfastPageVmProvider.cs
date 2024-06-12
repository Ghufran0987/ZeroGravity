using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class MealsSnacksBreakfastPageVmProvider : PageVmProviderBase, IMealsSnacksBreakfastPageVmProvider
    {
        private readonly IUserDataService _userDataService;
        private readonly IFastingDataService _fastingDataService;

        public MealsSnacksBreakfastPageVmProvider(ITokenService tokenService, IUserDataService userDataService, IFastingDataService fastingDataService) : base(
            tokenService)
        {
            _userDataService = userDataService;
            _fastingDataService = fastingDataService;
        }

        public async Task<ApiCallResult<MealDataProxy>> AddBreakfastAsync(MealDataProxy proxy,
            CancellationToken cancellationToken)
        {
            var dto = ProxyConverter.GetMealDataDto(proxy);
            var result = await _userDataService.CreateMealDataAsync(dto, cancellationToken);
            return result.Success
                ? ApiCallResult<MealDataProxy>.Ok(ProxyConverter.GetMealDataProxy(result.Value))
                : ApiCallResult<MealDataProxy>.Error(result.ErrorMessage, result.ErrorReason);
           
        }

        public async Task<ApiCallResult<MealDataProxy>> UpdateBreakfastAsync(MealDataProxy proxy, CancellationToken cancellationToken)
        {
            var dto = ProxyConverter.GetMealDataDto(proxy);
            var result = await _userDataService.UpdateMealDataAsync(dto, cancellationToken);
            return result.Success
                ? ApiCallResult<MealDataProxy>.Ok(ProxyConverter.GetMealDataProxy(result.Value))
                : ApiCallResult<MealDataProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> GetPersonalGoalAsync(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetPersonalGoalAsync(cancellationToken);
            return apiCallResult.Success
                ? ApiCallResult<PersonalGoalProxy>.Ok(ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value))
                : ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);

        }

        public async Task<ApiCallResult<IEnumerable<FastingDataProxy>>> GetActiveFastingDataAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _fastingDataService.GetActiveFastingDataByDateAsync(dateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var proxies = new List<FastingDataProxy>();
                foreach (var fastingDataDto in apiCallResult.Value)
                {
                    var fastingDataProxy = ProxyConverter.GetFastingDataProxy(fastingDataDto);
                    proxies.Add(fastingDataProxy);
                }

                return ApiCallResult<IEnumerable<FastingDataProxy>>.Ok(proxies);
            }

            return ApiCallResult<IEnumerable<FastingDataProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<List<MealDataProxy>>> GetMealDataWithFilterAsync(FilterMealDataDto dto, CancellationToken cancellationToken)
        {
            var result = await _userDataService.GetMealDataWithFilterAsync(dto, cancellationToken);
            if (!result.Success)
            {
                return ApiCallResult<List<MealDataProxy>>.Error(result.ErrorMessage, result.ErrorReason);
            }

            var proxies = result.Value.Select(ProxyConverter.GetMealDataProxy);
            return ApiCallResult<List<MealDataProxy>>.Ok(proxies.ToList());
        }
    }
}