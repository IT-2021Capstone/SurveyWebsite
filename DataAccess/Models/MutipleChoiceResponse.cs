using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class MutipleChoiceResponse
    {
        public int MutipleChoiceId { get; set; }
        public int QuestionId { get; set; }
        public int? MutipleChoiceUserResponse { get; set; }

        public virtual Question Question { get; set; }
    }
}
