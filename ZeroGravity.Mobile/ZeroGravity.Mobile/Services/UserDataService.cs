using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IApiService _apiService;
        private readonly ISecureStorageService _secureStorageService;
        private readonly ILogger _logger;

        public UserDataService(ILoggerFactory loggerFactory, IApiService apiService, ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<UserDataService>() ?? new NullLogger<UserDataService>();
        }


        public async Task<ApiCallResult<PersonalDataDto>> GetPersonalDataAsync(CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/personaldata/" + accountId;
            var url = baseUrl + api;

            try
            {
                var personalDataDto = await _apiService.GetSingleJsonAsync<PersonalDataDto>(url, cancellationToken);

                return ApiCallResult<PersonalDataDto>.Ok(personalDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetPersonalData.");
                    return ApiCallResult<PersonalDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<PersonalDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<PersonalDataDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<PersonalDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
        
        public async Task<ApiCallResult<PersonalDataDto>> UpdatePersonalDataAsync(PersonalDataDto personalDataDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var personaldataApi = "/personaldata/update";
            var accountApi = "/accounts/" + accountId;
            var personaldataApiUrl = baseUrl + personaldataApi;
            var accountApiUrl = baseUrl + accountApi;

            try
            {
                var personaldataResult = await _apiService.PutJsonAsyncRx<PersonalDataDto, PersonalDataDto>(personaldataApiUrl, personalDataDto, cancellationToken);

                UpdateRequestDto updateRequestDto = new UpdateRequestDto
                {
                    FirstName = personalDataDto.FirstName,
                    LastName = personalDataDto.LastName,
                    DateTimeDisplayType = personalDataDto.DateTimeDisplayType,
                    UnitDisplayType = personalDataDto.UnitDisplayType
                };

                var accountResult = await _apiService.PutJsonAsyncRx<UpdateRequestDto, AccountResponseDto>(accountApiUrl, updateRequestDto, cancellationToken);

                if (accountResult.Id != 0)
                {
                    personaldataResult.FirstName = accountResult.FirstName;
                    personaldataResult.LastName = accountResult.LastName;
                    personaldataResult.DateTimeDisplayType = accountResult.DateTimeDisplayType;
                    personaldataResult.UnitDisplayType = accountResult.UnitDisplayType;
                }

                return ApiCallResult<PersonalDataDto>.Ok(personaldataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdatePersonalData.");
                    return ApiCallResult<PersonalDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<PersonalDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<PersonalDataDto>.Error(ex.CustomErrorMessage, ex.ErrorList);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<PersonalDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<PersonalDataDto>> CreatePersonalDataAsync(PersonalDataDto personalDataDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var personaldataApi = "/personaldata/create";
            var accountApi = "/accounts/" + accountId;
            var personaldataApiUrl = baseUrl + personaldataApi;
            var accountApiUrl = baseUrl + accountApi;

            try
            {
                personalDataDto.AccountId = accountId;

                var personaldataResult = await _apiService.PostJsonAsyncRx<PersonalDataDto, PersonalDataDto>(personaldataApiUrl, personalDataDto, cancellationToken);

                UpdateRequestDto updateRequestDto = new UpdateRequestDto
                {
                    FirstName = personalDataDto.FirstName,
                    LastName = personalDataDto.LastName,
                    DateTimeDisplayType = personalDataDto.DateTimeDisplayType,
                    UnitDisplayType = personalDataDto.UnitDisplayType
                };

                var accountResult = await _apiService.PutJsonAsyncRx<UpdateRequestDto, AccountResponseDto>(accountApiUrl, updateRequestDto, cancellationToken);

                if (accountResult.Id != 0)
                {
       
                }

                return ApiCallResult<PersonalDataDto>.Ok(personaldataResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreatePersonalData.");
                    return ApiCallResult<PersonalDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<PersonalDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<PersonalDataDto>.Error(ex.CustomErrorMessage, ex.ErrorList);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<PersonalDataDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<MedicalConditionDto>> GetMedicalPreConditionAsync(CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/medicalcondition/" + accountId;
            var url = baseUrl + api;

            try
            {
                var medicalConditionDto = await _apiService.GetSingleJsonAsync<MedicalConditionDto>(url, cancellationToken);

                return ApiCallResult<MedicalConditionDto>.Ok(medicalConditionDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetMedicalPreCondition.");
                    return ApiCallResult<MedicalConditionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MedicalConditionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<MedicalConditionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<MedicalConditionDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<MedicalConditionDto>> CreateMedicalPreConditionAsync(MedicalConditionDto medicalConditionDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/medicalcondition/create";
            var url = baseUrl + api;

            try
            {
                medicalConditionDto.AccountId = accountId;

                var medicalConditionResult = await _apiService.PostJsonAsyncRx<MedicalConditionDto, MedicalConditionDto>(url, medicalConditionDto, cancellationToken);

                return ApiCallResult<MedicalConditionDto>.Ok(medicalConditionResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateMedicalPreCondition.");
                    return ApiCallResult<MedicalConditionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MedicalConditionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<MedicalConditionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<MedicalConditionDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<MedicalConditionDto>> UpdateMedicalPreConditionAsync(MedicalConditionDto medicalConditionDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/medicalcondition/update";
            var url = baseUrl + api;

            try
            {
                var medicalConditionResult = await _apiService.PutJsonAsyncRx<MedicalConditionDto, MedicalConditionDto>(url, medicalConditionDto, cancellationToken);

                return ApiCallResult<MedicalConditionDto>.Ok(medicalConditionResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateMedicalPreCondition.");
                    return ApiCallResult<MedicalConditionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MedicalConditionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<MedicalConditionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<MedicalConditionDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<DietPreferencesDto>> GetDietPreferenceAsync(CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/dietpreference/" + accountId;
            var url = baseUrl + api;

            try
            {
                var dietPreferencesDto = await _apiService.GetSingleJsonAsync<DietPreferencesDto>(url, cancellationToken);

                return ApiCallResult<DietPreferencesDto>.Ok(dietPreferencesDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetDietPreference.");
                    return ApiCallResult<DietPreferencesDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<DietPreferencesDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<DietPreferencesDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<DietPreferencesDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<DietPreferencesDto>> CreateDietPreferenceAsync(DietPreferencesDto dietPreferencesDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/dietpreference/create";
            var url = baseUrl + api;

            try
            {
                dietPreferencesDto.AccountId = accountId;

                var dietPreferenceResult =
                    await _apiService.PostJsonAsyncRx<DietPreferencesDto, DietPreferencesDto>(url, dietPreferencesDto, cancellationToken);

                return ApiCallResult<DietPreferencesDto>.Ok(dietPreferenceResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateDietPreference.");
                    return ApiCallResult<DietPreferencesDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<DietPreferencesDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<DietPreferencesDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<DietPreferencesDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<AccountResponseDto>> UpdateAccountDataAsync(AccountResponseDto accountResponseDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var accountApi = "/accounts/" + accountId;
            var accountApiUrl = baseUrl + accountApi;

            try
            {
                UpdateRequestDto updateRequestDto = new UpdateRequestDto
                {
                    FirstName = accountResponseDto.FirstName,
                    LastName = accountResponseDto.LastName
                };

                var accountResponseDtoResult =
                    await _apiService.PutJsonAsyncRx<UpdateRequestDto, AccountResponseDto>(accountApiUrl, updateRequestDto, cancellationToken);

                return ApiCallResult<AccountResponseDto>.Ok(accountResponseDtoResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateAccountData.");
                    return ApiCallResult<AccountResponseDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<AccountResponseDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<AccountResponseDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<AccountResponseDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        
        public async Task<ApiCallResult<bool>> UpdateWizardStateAsync(UpdateWizardRequestDto updateWizardRequestDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            updateWizardRequestDto.AccountId = accountId;

            var baseUrl = Common.ServerUrl;
            var accountApi = "/accounts/updatewizardstate/" + accountId;
            var accountApiUrl = baseUrl + accountApi;

            try
            {
                var accountResponseDtoResult =
                    await _apiService.PutJsonAsyncRx<UpdateWizardRequestDto, bool>(accountApiUrl, updateWizardRequestDto, cancellationToken);

                return ApiCallResult<bool>.Ok(accountResponseDtoResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateWizardState.");
                    return ApiCallResult<bool>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<bool>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<bool>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<bool>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<MealDataDto>> CreateMealDataAsync(MealDataDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
                var baseUrl = Common.ServerUrl;
                var api = "/mealdata/create";
                var url = baseUrl + api;
                
                dto.AccountId = accountId;

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                var result = await _apiService.PostJsonAsyncRx<MealDataDto, MealDataDto>(url, dto, cancellationToken, settings);
                return ApiCallResult<MealDataDto>.Ok(result);

            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateMealData.");
                    return ApiCallResult<MealDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MealDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ApiCallResult<MealDataDto>.Error(e.Message);
            }

        }

        public async Task<ApiCallResult<MealDataDto>> UpdateMealDataAsync(MealDataDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var baseUrl = Common.ServerUrl;
                var api = "/mealdata/update";
                var url = baseUrl + api;


                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                var result = await _apiService.PutJsonAsyncRx<MealDataDto, MealDataDto>(url, dto, cancellationToken, settings);
                return ApiCallResult<MealDataDto>.Ok(result);

            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreateMealData.");
                    return ApiCallResult<MealDataDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<MealDataDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ApiCallResult<MealDataDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<List<MealDataDto>>> GetMealDataAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = $"/mealdata/{accountId}/{DateTimeHelper.ToUniversalControllerDate(dateTime)}";
            var url = baseUrl + api;

            try
            {
                var mealDtos = await _apiService.GetSingleJsonAsync<List<MealDataDto>>(url, cancellationToken);

                return ApiCallResult<List<MealDataDto>>.Ok(mealDtos);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetMealDataAsync.");
                    return ApiCallResult<List<MealDataDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<MealDataDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<MealDataDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<MealDataDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<List<MealDataDto>>> GetMealDataWithFilterAsync(FilterMealDataDto dto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/mealdata/filtered";
            var url = baseUrl + api;

            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
            dto.AccountId = accountId;
            if (dto.DateTime.HasValue)
            {
                dto.DateTime = DateTimeHelper.ConvertLocalBeginOfDayToUtc(dto.DateTime.Value);
            }

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                var meals = await _apiService.PostJsonAsyncRx<FilterMealDataDto, List<MealDataDto>>(url, dto, cancellationToken, settings);
                return ApiCallResult<List<MealDataDto>>.Ok(meals);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateDietPreference.");
                    return ApiCallResult<List<MealDataDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<List<MealDataDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<MealDataDto>>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<MealDataDto>>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsync(CancellationToken cancellationToken)
        {
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;

            var api = "/accounts/" + userId;
            var url = baseUrl + api;

            try
            {
                var accountResponseDto = await _apiService.GetSingleJsonAsync<AccountResponseDto>(url, cancellationToken);
                return ApiCallResult<AccountResponseDto>.Ok(accountResponseDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetAccountData.");
                    return ApiCallResult<AccountResponseDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<AccountResponseDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (HttpException ex)
            {
                _logger.LogInformation($"{ex.Message}", ex);
                return ApiCallResult<AccountResponseDto>.Error(ex.CustomErrorMessage);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<AccountResponseDto>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<DietPreferencesDto>> UpdateDietPreferenceAsync(DietPreferencesDto dietPreferencesDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/dietpreference/update";
            var url = baseUrl + api;

            try
            {
                var dietPreferenceResult =
                    await _apiService.PutJsonAsyncRx<DietPreferencesDto, DietPreferencesDto>(url, dietPreferencesDto, cancellationToken);

                return ApiCallResult<DietPreferencesDto>.Ok(dietPreferenceResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdateDietPreference.");
                    return ApiCallResult<DietPreferencesDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<DietPreferencesDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<DietPreferencesDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<DietPreferencesDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<PersonalGoalDto>> GetPersonalGoalAsync(CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/personalgoal/" + accountId;
            var url = baseUrl + api;

            try
            {
                var personalGoalDto = await _apiService.GetSingleJsonAsync<PersonalGoalDto>(url, cancellationToken);

                return ApiCallResult<PersonalGoalDto>.Ok(personalGoalDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetPersonalGoal.");
                    return ApiCallResult<PersonalGoalDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<PersonalGoalDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<PersonalGoalDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<PersonalGoalDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<PersonalGoalDto>> CreatePersonalGoalAsync(PersonalGoalDto personalGoalDto, CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;
            var api = "/personalgoal/create";
            var url = baseUrl + api;

            try
            {
                personalGoalDto.AccountId = accountId;

                var personalGoalResult =
                    await _apiService.PostJsonAsyncRx<PersonalGoalDto, PersonalGoalDto>(url, personalGoalDto, cancellationToken);

                return ApiCallResult<PersonalGoalDto>.Ok(personalGoalResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to CreatePersonalGoal.");
                    return ApiCallResult<PersonalGoalDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<PersonalGoalDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<PersonalGoalDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<PersonalGoalDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<PersonalGoalDto>> UpdatePersonalGoalAsync(PersonalGoalDto personalGoalDto, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/personalgoal/update";
            var url = baseUrl + api;

            try
            {
                var personalGoalResult =
                    await _apiService.PutJsonAsyncRx<PersonalGoalDto, PersonalGoalDto>(url, personalGoalDto, cancellationToken);

                return ApiCallResult<PersonalGoalDto>.Ok(personalGoalResult);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to UpdatePersonalGoal.");
                    return ApiCallResult<PersonalGoalDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<PersonalGoalDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<PersonalGoalDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<PersonalGoalDto>.Error(AppResources.Common_Error_Unknown);
            }
        }
    }
}