using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Db.Models
{
    public class AnswerOptionData : ModelBase
    {
        public int QuestionId { get; set; }
        public QuestionData Question { get; set; }

        public string AnswerText { get; set; }
    }
}