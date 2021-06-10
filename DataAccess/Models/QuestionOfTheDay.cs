using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class QuestionOfTheDay
    {
        public QuestionOfTheDay()
        {
            QuestionOfTheDayResponses = new HashSet<QuestionOfTheDayResponse>();
        }

        public int QuestionOfTheDayId { get; set; }
        public string QuestionOfDayText { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }

        public virtual ICollection<QuestionOfTheDayResponse> QuestionOfTheDayResponses { get; set; }
    }
}
