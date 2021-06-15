using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebsite.DataAccessLibrary.Models
{
    public class Question
    {
        public enum Qtypes
        {
            Multiple_Choice,
            Long_Text,
            True_Or_False
        }
        public int ID { get; set; }
        public string QuestionText { get; set; }
        public Qtypes Type { get; set; }
        
        
    }
}
