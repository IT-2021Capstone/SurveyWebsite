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


        public int[] GetResponseMutipleAnswers(int qid, int TAnswers)
        {
            int tAnswers = TAnswers;
            var questionID = qid;
            int total = 0;
            int[] answerNum = new int[11];
            for (int i = 0; i <TAnswers; i++)
            { 
                answerNum[i] = _context.MutipleChoiceResponses.Where(s => s.QuestionId == questionID && s.MutipleChoiceUserResponse == i+1).Select(s => s.MutipleChoiceUserResponse).Count();
                total = total + answerNum[i];
            }
            answerNum[TAnswers] = total;
            
            return answerNum;
        }

        public int[] TrueFalseResponses(int qid) 
        {

            var questionID = qid;
            int[] answerNum = new int[3];
            answerNum[0] = _context.TrueFalseResponses.Where(s => s.QuestionId == questionID && s.TrueFalseUserResponse == 1).Select(s => s.TrueFalseUserResponse).Count();
            answerNum[1] = _context.TrueFalseResponses.Where(s => s.QuestionId == questionID && s.TrueFalseUserResponse == 2).Select(s => s.TrueFalseUserResponse).Count();
            answerNum[2] = answerNum[0] + answerNum[1];
            
            return answerNum;
        }

        public string GetText(int qid)
        {
            var questionID = qid;
            return _context.Questions.Where(s => s.QuestionId == questionID).Select(s => s.QuestionText).FirstOrDefault().ToString();
        }

        public string[] GetOpenResponse(int qid)
        {
            return _context.OpenEndedResponses.Where(s => s.QuestionId == qid).Select(s => s.OpenUserResponse).ToArray();
        }

        public string[] GetMutipleAnswerText(int qid)
        {
            return _context.MutipleChoiceTexts.Where(s => s.QuestionId == qid).Select(s => s.AnswerText).ToArray();
        }

        public string QuestionOfTheDayText(int qid) 
        {
            return _context.QuestionOfTheDays.Where(s => s.QuestionOfTheDayId == qid).Select(s => s.QuestionOfDayText).ToString();
        }

        public int[] GetDayResponseMutipleAnswers(int qid, int TAnswers)
        {
            int tAnswers = TAnswers;
            int total = 0;
            int[] answerNum = new int[11];
            for (int i = 0; i < TAnswers; i++)
            {
                answerNum[i] = _context.QuestionOfTheDayResponses.Where(s => s.QuestionOfTheDayId == qid && s.QuestionOfTheDayMutipleResponse == i + 1).Select(s => s.QuestionOfTheDayMutipleResponse).Count();
                total = total + answerNum[i];
            }
            answerNum[TAnswers] = total;
            
            return answerNum;
        }

        public string[] GetDayOpenResponse(int qid)
        {
            return _context.QuestionOfTheDayResponses.Where(s => s.QuestionOfTheDayId == qid).Select(s => s.QuestionOfTheDayOpenResponse).ToArray();
        }

        public string[] GetMutipleAnswerTextDay(int qid)
        {
            return _context.MutipleAnswerQoftheDays.Where(s => s.QuestionOfTheDayId == qid).Select(s => s.DayAnswerText).ToArray();
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

        public void SendMutipleResponse(int Qid,  int userInt)
        {
            MutipleChoiceResponse[] mcr;          
            int userAnswer = userInt;
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@userAnwer", userAnswer);
            SqlParameter param2 = new SqlParameter("@questionID", QuestionID);

            mcr = _context.MutipleChoiceResponses
                .FromSqlRaw("EXECUTE AddMutiplequestionResponse @userAnwer, @questionID", param1, param2)
                .ToArray();
        }

        public void SendTrueFalseResponse(int Qid, int userInt)
        {
            TrueFalseResponse[] mcr;
            int userAnswer = userInt;
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@userAnwer", userAnswer);
            SqlParameter param2 = new SqlParameter("@questionID", QuestionID);

            mcr = _context.TrueFalseResponses
                .FromSqlRaw("EXECUTE AddTrueFalseResponse @userAnwer, @questionID", param1, param2)
                .ToArray();
        }

        public void SendDayMutipleResponse(int Qid, int userInt)
        {

            QuestionOfTheDayResponse[] qotdr;
            int QuestionID = Qid;
            int userAnswer = userInt;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionMutiResponse", userAnswer);
            qotdr = _context.QuestionOfTheDayResponses
                .FromSqlRaw("EXECUTE AddQuestionOfTheDayMutiResponse @questionID, @questionMutiResponse", param1, param2)
                .ToArray();

        }

        public void SendDayOpeneded(int Qid, string userString)
        {
            QuestionOfTheDayResponse[] qotdr;
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionOpenResponse", userAnswer);

            qotdr = _context.QuestionOfTheDayResponses
                .FromSqlRaw("EXECUTE AddQuestionOfTheDayOpenResponse @questionID, @questionOpenResponse", param1, param2)
                .ToArray();

        }
        public void UpdateQuestionText(int Qid, string userString)
        {
            QuestionOfTheDayResponse[] qotdr;
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);

            qotdr = _context.QuestionOfTheDayResponses
                .FromSqlRaw("EXECUTE UpdateQuestionText @questionID, @questionText", param1, param2)
                .ToArray();

        }


    }
}