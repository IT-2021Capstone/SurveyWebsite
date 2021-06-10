using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SurveyWebsite.DataAccessLibrary.Models;

namespace SurveyWebsite.DataAccessLibrary.Models
{
    public class QuestionOptionListBase : ComponentBase
    {
        public static IEnumerable<QuestionOption> QuestionOptions { get; set;}
        protected override Task OnInitializedAsync()
        {
            
            return base.OnInitializedAsync();
        }
    }
}
