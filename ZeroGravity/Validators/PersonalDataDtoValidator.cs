using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Validators
{
    public class PersonalDataDtoValidator : AbstractValidator<PersonalDataDto>
    {
        public PersonalDataDtoValidator(IStringLocalizer<PersonalDataDtoValidator> stringLocalizer)
        {
            //RuleFor(_ => _.Weight)
            //    .NotNull().WithMessage(_ => stringLocalizer["Weight_ValueNullMessage"]).InclusiveBetween(PersonalDataConstants.MinWeightKg,
            //        PersonalDataConstants.MaxWeightKg).WithMessage(_ => string.Format(stringLocalizer["Weight_ValueNotBetweenMessage"], PersonalDataConstants.MinWeightKg,
            //        PersonalDataConstants.MaxWeightKg));

            //RuleFor(_ => _.Height)
            //    .NotNull().WithMessage(_ => stringLocalizer["Height_ValueNullMessage"]).InclusiveBetween(PersonalDataConstants.MinHeightCm,
            //        PersonalDataConstants.MaxHeightCm).WithMessage(_ => string.Format(stringLocalizer["Height_ValueNotBetweenMessage"], PersonalDataConstants.MinHeightCm,
            //        PersonalDataConstants.MaxHeightCm));

            //RuleFor(_ => _.YearOfBirth)
            //    .NotNull().WithMessage(_ => stringLocalizer["YearOfBirth_ValueNullMessage"]).InclusiveBetween(PersonalDataConstants.MinYear,
            //        DateTime.Now.Year).WithMessage(_ => string.Format(stringLocalizer["YearOfBirth_ValueNotBetweenMessage"], PersonalDataConstants.MinYear,
            //        DateTime.Now.Year));

            //RuleFor(_ => _.Country)
            //    .NotNull().NotEmpty().WithMessage(_ => stringLocalizer["Country_ValueNullMessage"]);

            //RuleFor(_ => _.DeviceType)
            //    .NotNull().IsInEnum().WithMessage(_ => stringLocalizer["DeviceType_ValueInvalidMessage"]);

            //RuleFor(_ => _.GenderType)
            //    .NotNull().IsInEnum().WithMessage(_ => stringLocalizer["GenderType_ValueInvalidMessage"]);

            //RuleFor(_ => _.WaistDiameter)
            //    .NotNull().WithMessage(_ => stringLocalizer["WaistDiameter_ValueNullMessage"]).InclusiveBetween(PersonalDataConstants.MinWaistDiametertCm,
            //        PersonalDataConstants.MaxWaistDiametertCm).WithMessage(_ => stringLocalizer["WaistDiameter_ValueNotBetweenMessage"]);

            //RuleFor(_ => _.HipDiameter)
            //    .NotNull().WithMessage(_ => stringLocalizer["HipDiameter_ValueNullMessage"]).InclusiveBetween(PersonalDataConstants.MinHipDiametertCm,
            //        PersonalDataConstants.MaxHipDiametertCm).WithMessage(_ => string.Format(stringLocalizer["HipDiameter_ValueNotBetweenMessage"], PersonalDataConstants.MinHipDiametertCm,
            //    PersonalDataConstants.MaxHipDiametertCm));

            //RuleFor(_ => _.NeckDiameter)
            //    .NotNull().WithMessage(_ => stringLocalizer["NeckDiameter_ValueNullMessage"]).InclusiveBetween(PersonalDataConstants.MinNeckDiametertCm,
            //        PersonalDataConstants.MaxNeckDiametertCm).WithMessage(_ => string.Format(stringLocalizer["NeckDiameter_ValueNotBetweenMessage"], PersonalDataConstants.MinNeckDiametertCm,
            //    PersonalDataConstants.MaxNeckDiametertCm));
        }
    }
}