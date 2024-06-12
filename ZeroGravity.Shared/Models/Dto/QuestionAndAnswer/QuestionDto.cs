using System.Collections.Generic;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Shared.Models.Dto
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public ICollection<AnswerOptionDto> Answers { get; set; }
        public AnswerDataType AnswerDataType { get; set; }
        public DataFieldType DataFieldType { get; set; }
    }
}