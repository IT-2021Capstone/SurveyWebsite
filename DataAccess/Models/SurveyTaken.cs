using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class SurveyTaken
    {
        public int SurveyTakenId { get; set; }
        public int SurveyId { get; set; }
        public string LoginId { get; set; }

        public virtual AspNetUser Login { get; set; }
        public virtual Surveylist Survey { get; set; }
    }
}
