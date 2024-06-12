using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class ActivityDataDtoValidator : AbstractValidator<ActivityDataDto>
    {
        public ActivityDataDtoValidator(IStringLocalizer<ActivityDataDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.AccountId).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["AccountId_ValueNullMessage"]);

            RuleFor(_ => _.ActivityType).NotNull().IsInEnum()
                .WithMessage(_ => stringLocalizer["ExerciseType_ValueInvalidMessage"]);

            RuleFor(_ => _.Duration).NotNull()
                .WithMessage(_ => stringLocalizer["Duration_ValueNullMessage"]).InclusiveBetween(
                    ActivityConstants.MinDayToDayDuration,
                    ActivityConstants.MaxDayToDayDuration)
                .When(_ => _.ActivityType == ActivityType.DayToDay)
                .WithMessage(_ => string.Format(stringLocalizer["Duration_ValueNotBetweenMessage"],
                    ActivityConstants.MinDayToDayDuration,
                    ActivityConstants.MaxDayToDayDuration));

            RuleFor(_ => _.Duration).NotNull()
                .WithMessage(_ => stringLocalizer["Duration_ValueNullMessage"]).InclusiveBetween(
                    ActivityConstants.MinExerciseDuration,
                    ActivityConstants.MaxExerciseDuration)
                .When(_ => _.ActivityType == ActivityType.Exercise)
                .WithMessage(_ => string.Format(stringLocalizer["Duration_ValueNotBetweenMessage"],
                    ActivityConstants.MinExerciseDuration,
                    ActivityConstants.MaxExerciseDuration));

            RuleFor(_ => _.Created).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["Created_ValueNullMessage"]);

            RuleFor(_ => _.Name).NotEmpty()
                .WithMessage(_ => stringLocalizer["Name_ValueNullMessage"]);
        }
    }
}