using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class SurveyOrder
    {
        public int? SurveyId { get; set; }
        public int? QuestionOfTheDayId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CurrentOrder { get; set; }
        public string SurveyName { get; set; }

        public virtual QuestionOfTheDay QuestionOfTheDay { get; set; }
        public virtual Surveylist Survey { get; set; }
    }
}
