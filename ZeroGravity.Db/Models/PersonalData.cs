using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    [Index(nameof(AccountId))]
    public class PersonalData : ModelBase
    {
        [Required]
        public int AccountId { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public double WaistDiameter { get; set; }

        public double HipDiameter { get; set; }

        public double NeckDiameter { get; set; }

        public GenderBiologicalType BiologicalGender { get; set; }
        public GenderIdentifyType IdentifyGender { get; set; }

        public DeviceType DeviceType { get; set; }

        public string Ethnicity { get; set; }

        public string TimeZone { get; set; }

        public ICollection<QuestionAndAnswerData> QuestionAndAnswers { get; set; }

        public SalutationType Salutation { get; set; }
    }
}