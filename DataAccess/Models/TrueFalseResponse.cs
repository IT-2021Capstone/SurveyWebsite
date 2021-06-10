using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class TrueFalseResponse
    {
        public int TrueFalseId { get; set; }
        public int QuestionId { get; set; }
        public int TrueFalseUserResponse { get; set; }

        public virtual Question Question { get; set; }
    }
}
