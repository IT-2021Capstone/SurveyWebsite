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


        public void SendQuestion()
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
    }
}