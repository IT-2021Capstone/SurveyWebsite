using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Services
{
    public class ResultsServices
    {
        protected readonly SurveySiteContext _dbcontext;

        public ResultsServices(SurveySiteContext _db)
        {
            _dbcontext = _db;
        }

        public Question[] GetQuestionDetails()
        {
            Question[] q;
            q = _dbcontext.Questions.FromSqlRaw("exec viewQuestionText 11").ToArray();
            return q;

        }
    }
}
