using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class QuestionAndAnswerData : ModelBase
    {
        [Required] public int AccountId { get; set; }
        [Required] public int PersonalDataId { get; set; }
        public virtual PersonalData PersonalData { get; set; }
        [Required] public int QuestionId { get; set; }
        public QuestionData Question { get; set; }
        [Required] public int AnswerId { get; set; }
        public AnswerOptionData Answer { get; set; }
    }
}