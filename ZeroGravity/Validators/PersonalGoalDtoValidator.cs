using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class PersonalGoalDtoValidator : AbstractValidator<PersonalGoalDto>
    {
        public PersonalGoalDtoValidator(IStringLocalizer<PersonalGoalDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.WaterConsumption).NotNull().WithMessage(_ => stringLocalizer["WaterConsumption_ValueNullMessage"])
                .InclusiveBetween(PersonalGoalConstants.MinWaterConsumptionLiters,
                    PersonalGoalConstants.MaxWaterConsumptionLiters)
                .WithMessage(_ => string.Format(stringLocalizer["WaterConsumption_ValueNotBetweenMessage"],PersonalGoalConstants.MinWaterConsumptionLiters,
                    PersonalGoalConstants.MaxWaterConsumptionLiters));

            RuleFor(_ => _.BreakfastAmount).NotNull().IsInEnum()
                .WithMessage(_ => stringLocalizer["BreakfastAmount_ValueInvalidMessage"]);

            RuleFor(_ => _.LunchAmount).NotNull().IsInEnum()
                .WithMessage(_ => stringLocalizer["LunchAmount_ValueInvalidMessage"]);

            RuleFor(_ => _.DinnerAmount).NotNull().IsInEnum()
                .WithMessage(_ => stringLocalizer["DinnerAmount_ValueInvalidMessage"]);
        }
    }
}