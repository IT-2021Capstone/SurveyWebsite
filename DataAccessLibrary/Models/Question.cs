using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebsite.DataAccessLibrary.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Questions { get; set; }
        public string Type { get; set; }
        public IEnumerable<QuestionOption> QOpts { get; set; }
    }
}
