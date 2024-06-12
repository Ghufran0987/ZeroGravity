using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IUserDataService
    {
        Task<ApiCallResult<PersonalDataDto>> CreatePersonalDataAsync(PersonalDataDto personalDataDto, CancellationToken cancellationToken);
        Task<ApiCallResult<PersonalDataDto>> GetPersonalDataAsync(CancellationToken cancellationToken);
        Task<ApiCallResult<PersonalDataDto>> UpdatePersonalDataAsync(PersonalDataDto personalDataDto, CancellationToken cancellationToken);

        Task<ApiCallResult<MedicalConditionDto>> CreateMedicalPreConditionAsync(MedicalConditionDto medicalConditionDto, CancellationToken cancellationToken);
        Task<ApiCallResult<MedicalConditionDto>> GetMedicalPreConditionAsync(CancellationToken cancellationToken);
        Task<ApiCallResult<MedicalConditionDto>> UpdateMedicalPreConditionAsync(MedicalConditionDto medicalConditionDto, CancellationToken cancellationToken);

        Task<ApiCallResult<DietPreferencesDto>> CreateDietPreferenceAsync(DietPreferencesDto dietPreferencesDto, CancellationToken cancellationToken);
        Task<ApiCallResult<DietPreferencesDto>> GetDietPreferenceAsync(CancellationToken cancellationToken);
        Task<ApiCallResult<DietPreferencesDto>> UpdateDietPreferenceAsync(DietPreferencesDto dietPreferencesDto, CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalDto>> CreatePersonalGoalAsync(PersonalGoalDto personalGoalDto, CancellationToken cancellationToken);
        Task<ApiCallResult<PersonalGoalDto>> GetPersonalGoalAsync(CancellationToken cancellationToken);
        Task<ApiCallResult<PersonalGoalDto>> UpdatePersonalGoalAsync(PersonalGoalDto personalGoalDto, CancellationToken cancellationToken);

        Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsync(CancellationToken token);
        Task<ApiCallResult<AccountResponseDto>> UpdateAccountDataAsync(AccountResponseDto accountResponseDto, CancellationToken cancellationToken);
        Task<ApiCallResult<bool>> UpdateWizardStateAsync(UpdateWizardRequestDto updateWizardRequestDto, CancellationToken cancellationToken);

        Task<ApiCallResult<MealDataDto>> CreateMealDataAsync(MealDataDto dto, CancellationToken cancellationToken);
        Task<ApiCallResult<MealDataDto>> UpdateMealDataAsync(MealDataDto dto, CancellationToken cancellationToken);

        Task<ApiCallResult<List<MealDataDto>>> GetMealDataAsync(DateTime dateTime, CancellationToken cancellationToken);
        Task<ApiCallResult<List<MealDataDto>>> GetMealDataWithFilterAsync(FilterMealDataDto dto, CancellationToken cancellationToken);
    }
}