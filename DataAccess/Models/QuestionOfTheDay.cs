using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class QuestionOfTheDay
    {
        public QuestionOfTheDay()
        {
            MutipleAnswerQoftheDays = new HashSet<MutipleAnswerQoftheDay>();
            QuestionOfTheDayOpenResponses = new HashSet<QuestionOfTheDayOpenResponse>();
            QuestionOfTheDayResponses = new HashSet<QuestionOfTheDayResponse>();
        }

        public int QuestionOfTheDayId { get; set; }
        public string QuestionOfDayText { get; set; }
        public int QuestionOfDayType { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }

        public virtual ICollection<MutipleAnswerQoftheDay> MutipleAnswerQoftheDays { get; set; }
        public virtual ICollection<QuestionOfTheDayOpenResponse> QuestionOfTheDayOpenResponses { get; set; }
        public virtual ICollection<QuestionOfTheDayResponse> QuestionOfTheDayResponses { get; set; }
    }
}
