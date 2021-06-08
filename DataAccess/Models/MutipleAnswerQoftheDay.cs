using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class MutipleAnswerQoftheDay
    {
        public int MutipleAnswerQoftheDayId { get; set; }
        public int QuestionOfTheDayId { get; set; }
        public string DayAnswerText { get; set; }

        public virtual QuestionOfTheDay QuestionOfTheDay { get; set; }
    }
}
