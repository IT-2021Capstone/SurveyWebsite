using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class OpenEndedResponse
    {
        public int OpenEndedId { get; set; }
        public int QuestionId { get; set; }
        public string OpenUserResponse { get; set; }

        public virtual Question Question { get; set; }
    }
}
