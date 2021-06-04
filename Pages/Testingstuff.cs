using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SurveyWebsite.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebsite.Pages
{
    public class Testingstuff
    {
        private readonly ApplicationDbContext _context;
        public Testingstuff(ApplicationDbContext context)
        {
            _context = context;
        }


        public void TestQuestion()
        {
            Question[] q;
            var sureveyID = 3;
            var questionText = "This is from the website";
            var questionType = 1;
            SqlParameter param1 = new SqlParameter("@surveyID", sureveyID);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionType", questionType);
            q = _context.Questions
                .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType", param1, param2, param3)
                .ToArray();

        }

        public void SendQuestion(int surveyid, string questiontext, int questiontype)
        {
            Question[] q;
            int _surveyID = surveyid;
            int _questiontype;
            string _questiontext;
            switch (questiontype)
            {
                //switch case may not be necessary now, but seems like a good practice if we
                //want to add more question types in the future
                case 1:
                    //open ended question type being submitted
                    _questiontype = 1;
                    _questiontext = questiontext;
                    //execute addnonmutiplequestion
                    q = _context.Questions
                        .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                        _surveyID, _questiontext, _questiontype)
                        .ToArray();
                    break;
                case 2:
                    //true false question type being submitted
                    _questiontype = 2;
                    _questiontext = questiontext;
                    //execute addmutiplequestion
                    break;
                case 3:
                    //multiple choice question type being submitted
                    _questiontype = 3;
                    _questiontext = questiontext;
                    //execute addmutiplequestion
                    break;
            }
        }

    }
}