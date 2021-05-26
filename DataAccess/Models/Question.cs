using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Question
    {
        public Question()
        {
            MutipleChoiceResponses = new HashSet<MultipleChoiceResponse>();
            MutipleChoiceTexts = new HashSet<MultipleChoiceText>();
            OpenEndedResponses = new HashSet<OpenEndedResponse>();
            TrueFalseResponses = new HashSet<TrueFalseResponse>();
        }

        public int QuestionId { get; set; }
        public int? SurveyId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }

        public virtual QuestionType QuestionTypeNavigation { get; set; }
        public virtual Surveylist Survey { get; set; }
        public virtual ICollection<MultipleChoiceResponse> MutipleChoiceResponses { get; set; }
        public virtual ICollection<MultipleChoiceText> MutipleChoiceTexts { get; set; }
        public virtual ICollection<OpenEndedResponse> OpenEndedResponses { get; set; }
        public virtual ICollection<TrueFalseResponse> TrueFalseResponses { get; set; }
    }
}
