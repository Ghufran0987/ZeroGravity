using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class MedicalConditionDto
    {
        public int Id { get; set; }

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