using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class FastingSettingDtoValidator : AbstractValidator<FastingSettingDto>
    {
        public FastingSettingDtoValidator(IStringLocalizer<FastingSettingDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.AccountId).NotNull().NotEmpty()
                .WithMessage(_ => stringLocalizer["AccountId_ValueNullMessage"]);

            RuleFor(_ => _.SkipBreakfast).NotNull().WithMessage(_ => stringLocalizer["SkipBreakfast_ValueNullMessage"]);
            RuleFor(_ => _.SkipLunch).NotNull().WithMessage(_ => stringLocalizer["SkipLunch_ValueNullMessage"]);
            RuleFor(_ => _.SkipDinner).NotNull().WithMessage(_ => stringLocalizer["SkipDinner_ValueNullMessage"]);

            RuleFor(_ => _.IncludeMondays).NotNull()
                .WithMessage(_ => stringLocalizer["IncludeMondays_ValueNullMessage"]);
            RuleFor(_ => _.IncludeTuesdays).NotNull()
                .WithMessage(_ => stringLocalizer["IncludeTuesdays_ValueNullMessage"]);
            RuleFor(_ => _.IncludeWednesdays).NotNull()
                .WithMessage(_ => stringLocalizer["IncludeWednesdays_ValueNullMessage"]);
            RuleFor(_ => _.IncludeThursdays).NotNull()
                .WithMessage(_ => stringLocalizer["IncludeThursdays_ValueNullMessage"]);
            RuleFor(_ => _.IncludeFridays).NotNull()
                .WithMessage(_ => stringLocalizer["IncludeFridays_ValueNullMessage"]);
            RuleFor(_ => _.IncludeSaturdays).NotNull()
                .WithMessage(_ => stringLocalizer["IncludeSaturdays_ValueNullMessage"]);
            RuleFor(_ => _.IncludeSundays).NotNull()
                .WithMessage(_ => stringLocalizer["IncludeSundays_ValueNullMessage"]);
        }
    }
}