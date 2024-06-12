using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class FastingDataDtoValidator : AbstractValidator<FastingDataDto>
    {
        public FastingDataDtoValidator(IStringLocalizer<FastingDataDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.AccountId).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["AccountId_ValueNullMessage"]);

            RuleFor(_ => _.End).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["TargetDate_ValueNullMessage"]);

            RuleFor(_ => _.Start).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["TargetDate_ValueNullMessage"]);

            RuleFor(_ => _.Created).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["TargetDate_ValueNullMessage"]);

            RuleFor(_ => _.Duration).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["TargetDate_ValueNullMessage"]);

            //RuleFor(_ => _.Duration).NotNull()
            //    .WithMessage(_ => stringLocalizer["Duration_ValueNullMessage"]).InclusiveBetween(
            //        ActivityConstants.MinDayToDayDuration,
            //        ActivityConstants.MaxDayToDayDuration)
            //    .WithMessage(_ => string.Format(stringLocalizer["Duration_ValueNotBetweenMessage"], ActivityConstants.MinDayToDayDuration,
            //        ActivityConstants.MaxDayToDayDuration));

            //RuleFor(_ => _.Start).NotNull().NotEmpty()
            //    .WithMessage(_ => stringLocalizer["StartTime_ValueNullMessage"]).Must(IsValidTimespan)
            //    .WithMessage(_ => stringLocalizer["StartTime_InvalidTimespanMessage"]);
        }

        private bool IsValidTimespan(string value)
        {
            return TimeSpan.TryParse(value, out _);
        }
    }
}
