using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class MedicalConditionsDtoValidator : AbstractValidator<MedicalConditionDto>
    {
        public MedicalConditionsDtoValidator(IStringLocalizer<MedicalConditionsDtoValidator> stringLocalizer)
        {
            RuleFor(_ => _.DiabetesType).NotNull().IsInEnum().WithMessage(_ => stringLocalizer["DiabetesType_ValueInvalidMessage"]);

            RuleFor(_ => _.HasArthritis).NotNull().WithMessage(_ => stringLocalizer["HasArthritis_ValueNullMessage"]);

            RuleFor(_ => _.HasCardiacCondition).NotNull().WithMessage(_ => stringLocalizer["HasCardiacCondition_ValueNullMessage"]);

            RuleFor(_ => _.HasDiabetes).NotNull().WithMessage(_ => stringLocalizer["HasDiabetes_ValueNullMessage"]);

            RuleFor(_ => _.HasHypertension).NotNull().WithMessage(_ => stringLocalizer["HasHypertension_ValueNullMessage"]);
        }
    }
}