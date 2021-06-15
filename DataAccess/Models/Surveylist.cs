using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Surveylist
    {
        public Surveylist()
        {
            Questions = new HashSet<Question>();
            SurveyTakens = new HashSet<SurveyTaken>();
        }

        public int SurveyId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int CurrentOrder { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<SurveyTaken> SurveyTakens { get; set; }
    }
}
