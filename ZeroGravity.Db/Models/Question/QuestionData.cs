using System;
using System.Collections.Generic;
using System.Text;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models
{
    public class QuestionData : ModelBase
    {
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public ICollection<AnswerOptionData> Answers { get; set; }
        public AnswerDataType AnswerDataType { get; set; }
        public DataFieldType DataFieldType { get; set; }
    }
}