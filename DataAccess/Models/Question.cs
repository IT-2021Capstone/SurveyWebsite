using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Question
    {
        public Question()
        {
            MutipleChoiceResponses = new HashSet<MutipleChoiceResponse>();
            MutipleChoiceTexts = new HashSet<MutipleChoiceText>();
            OpenEndedResponses = new HashSet<OpenEndedResponse>();
            TrueFalseResponses = new HashSet<TrueFalseResponse>();
        }

        public int QuestionId { get; set; }
        public int? SurveyId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public bool IsRequired { get; set; }

        public virtual QuestionType QuestionTypeNavigation { get; set; }
        public virtual Surveylist Survey { get; set; }
        public virtual ICollection<MutipleChoiceResponse> MutipleChoiceResponses { get; set; }
        public virtual ICollection<MutipleChoiceText> MutipleChoiceTexts { get; set; }
        public virtual ICollection<OpenEndedResponse> OpenEndedResponses { get; set; }
        public virtual ICollection<TrueFalseResponse> TrueFalseResponses { get; set; }
    }
}
