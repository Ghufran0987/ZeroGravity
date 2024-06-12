using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class MealDataDtoValidator : AbstractValidator<MealDataDto>
    {
        public MealDataDtoValidator(IStringLocalizer<MealDataDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.Name).NotNull().NotEmpty().WithMessage(_ => stringLocalizer["Name_InvalidMessage"]);
        }
    }
}