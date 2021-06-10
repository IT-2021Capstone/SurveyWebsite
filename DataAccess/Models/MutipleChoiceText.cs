using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class MutipleChoiceText
    {
        public int MutipleChoiceAnswerId { get; set; }
        public int? QuestionId { get; set; }
        public string AnswerText { get; set; }

        public virtual Question Question { get; set; }
    }
}
