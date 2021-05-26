using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class SurveyTaken
    {
        public int SurveyTakenId { get; set; }
        public int SurveyId { get; set; }
        public int LoginId { get; set; }

        public virtual Login Login { get; set; }
        public virtual Surveylist Survey { get; set; }
    }
}
