﻿using DataAccess.Models;
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


        public Question[] getResponse3Users(int surveyId, int questionId)
        {
            Question[] eachvalue;
            var surveyID = surveyId;
            var questionID = questionId;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);
            SqlParameter param2 = new SqlParameter("@questionID", questionID);
            eachvalue = _context.Questions.FromSqlRaw("Execute ViewMutipleChoice3ansers @surveyID, @questionID", param1, param2).ToArray();
            var a = 2;
            return eachvalue;
        }

        public void SendQuestion(int id, string text, int qtype )
        {
            Question[] q;
            var surveyID = id;
            var questionText = text;
            var questionType = qtype;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionType", questionType);
            q = _context.Questions
                .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType", param1, param2, param3)
                .ToArray();

        }
        
    }
}