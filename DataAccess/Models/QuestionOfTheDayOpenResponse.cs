using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class QuestionOfTheDayOpenResponse
    {
        public int QuestionOfTheDayOpenResponsesId { get; set; }
        public int QuestionOfTheDayId { get; set; }
        public string QuestionOfTheDayOpenResponse1 { get; set; }

        public virtual QuestionOfTheDay QuestionOfTheDay { get; set; }
    }
}
