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
    public class LiquidIntakeDtoValidator : AbstractValidator<LiquidIntakeDto>
    {
        public LiquidIntakeDtoValidator(IStringLocalizer<LiquidIntakeDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.AccountId).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["AccountId_ValueNullMessage"]);

            RuleFor(_ => _.Created).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["Created_ValueNullMessage"]);

            RuleFor(_ => _.LiquidType).NotNull().IsInEnum()
                .WithMessage(_ => stringLocalizer["LiquidType_ValueNullMessage"]);

            RuleFor(_ => _.Name).NotNull().NotEmpty()
                .When(_ => _.LiquidType == LiquidType.CalorieDrinkAndAlcohol)
                .WithMessage(_ => stringLocalizer["Name_ValueNullMessage"]);

            RuleFor(_ => _.Amount).NotNull()
                .WithMessage(_ => stringLocalizer["AmounMl_ValueNullMessage"]).InclusiveBetween(
                    LiquidIntakeConstants.MinAmountMl,
                    LiquidIntakeConstants.MaxAmountMl)
                .WithMessage(_ => string.Format(stringLocalizer["AmounMl_ValueNotBetweenMessage"], LiquidIntakeConstants.MinAmountMl, LiquidIntakeConstants.MaxAmountMl));
        }
    }
}
