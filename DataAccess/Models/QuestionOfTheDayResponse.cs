using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class QuestionOfTheDayResponse
    {
        public int QuestionOfTheDayResoponseId { get; set; }
        public int QuestionOfTheDayId { get; set; }
        public int? QuestionOfTheDayMutipleResponse { get; set; }
        public string QuestionOfTheDayOpenResponse { get; set; }

        public virtual QuestionOfTheDay QuestionOfTheDay { get; set; }
    }
}
