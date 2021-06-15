using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SurveyWebsite.DataAccessLibrary.Models;

namespace SurveyWebsite.DataAccessLibrary.Models
    {
        public class QuestionListBase : ComponentBase
        {
            public static IEnumerable<Question> Questions { get; set; }

            protected override Task OnInitializedAsync()
            {
                LoadQuestions();
                return base.OnInitializedAsync();
            }

            public void LoadQuestions()
            {
                Question q1 = new Question
                {
                    ID = 1,
                    QuestionText = ""                    
                };                

                Questions = new List<Question> { q1 };

            }
        }
    }

