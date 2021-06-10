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

        public int? LoginId { get; set; }
        public int SurveyId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int CurrentOrder { get; set; }

        public virtual Login Login { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<SurveyTaken> SurveyTakens { get; set; }
    }
}
