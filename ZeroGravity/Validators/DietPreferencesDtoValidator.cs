using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class DietPreferencesDtoValidator : AbstractValidator<DietPreferencesDto>
    {
        public DietPreferencesDtoValidator(IStringLocalizer<DietPreferencesDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.DietType).NotNull().IsInEnum()
                .WithMessage(_ => stringLocalizer["DietType_ValueInvalidMessage"]);

            RuleFor(_ => _.BreakfastTime).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["BreakfastTime_ValueNullMessage"]).Must(IsValidTimespan)
                .WithMessage(_ => stringLocalizer["BreakfastTime_InvalidTimespanMessage"]);

            RuleFor(_ => _.LunchTime).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["LunchTime_ValueNullMessage"]).Must(IsValidTimespan)
                .WithMessage(_ => stringLocalizer["LunchTime_InvalidTimespanMessage"]);

            RuleFor(_ => _.DinnerTime).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["DinnerTime_ValueNullMessage"]).Must(IsValidTimespan)
                .WithMessage(_ => stringLocalizer["DinnerTime_InvalidTimespanMessage"]);
        }

        private bool IsValidTimespan(string value)
        {
            return TimeSpan.TryParse(value, out _);
        }
    }
}