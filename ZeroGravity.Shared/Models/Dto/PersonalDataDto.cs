using System;
using System.Collections.Generic;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class PersonalDataDto
    {
        public PersonalDataDto()
        {
            Height = PersonalDataConstants.MinHeightCm;

            Weight = PersonalDataConstants.MinWeightKg;

            WaistDiameter = PersonalDataConstants.MinWaistDiametertCm;

            HipDiameter = PersonalDataConstants.MinHipDiametertCm;

            NeckDiameter = PersonalDataConstants.MinNeckDiametertCm;

            DateOfBirth = new DateTime(1970, 1, 1);
        }

        public int AccountId { get; set; }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth{ get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public double WaistDiameter { get; set; }

        public double HipDiameter { get; set; }

        public double NeckDiameter { get; set; }

        public GenderBiologicalType BiologicalGender { get; set; }

        public GenderIdentifyType IdentifyGender { get; set; }

        public DeviceType DeviceType { get; set; }

        public DateTimeDisplayType DateTimeDisplayType { get; set; }

        public UnitDisplayType UnitDisplayType { get; set; }

        public string Ethnicity { get; set; }

        public string TimezoneId { get; set; }

        public ICollection<QuestionAndAnswerDto> QuestionAndAnswers { get; set; }

        public SalutationType Salutation { get; set; }
    }
}