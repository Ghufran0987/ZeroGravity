using System.ComponentModel.DataAnnotations;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    public class MedicalCondition : ModelBase
    {
        [Required]
        public int AccountId { get; set; }

        public bool HasDiabetes { get; set; }

        public DiabetesType DiabetesType { get; set; }

        public bool HasHypertension { get; set; }

        public bool HasArthritis { get; set; }

        public bool HasCardiacCondition { get; set; }

        public bool DontWantToSayNow { get; set; }
        public bool Others { get; set; }
    }
}