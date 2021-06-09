using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SurveyWebsite.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace SurveyWebsite.Pages
{
    public class Testingstuff
    {

        
        private readonly ApplicationDbContext _context;
        public Testingstuff(ApplicationDbContext context)
        {
            _context = context;
        }


        //get the response for questions with more then 2 responses also has total resposnse for this question
        public int[] GetUserResponseMutipleAnswers(int qid, int TAnswers)
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
        //same as above but for question of the day
        public int[] GetUserResponseMutipleAnswersQotD(int qid, int TAnswers)
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
        //gets the responses for true or false questions and the total number of responses
        public int[] GetUserTrueFalseResponses(int qid) 
        {
            var questionID = qid;
            int[] answerNum = new int[3];
            answerNum[0] = _context.TrueFalseResponses.Where(s => s.QuestionId == questionID && s.TrueFalseUserResponse == 1).Select(s => s.TrueFalseUserResponse).Count();
            answerNum[1] = _context.TrueFalseResponses.Where(s => s.QuestionId == questionID && s.TrueFalseUserResponse == 2).Select(s => s.TrueFalseUserResponse).Count();
            answerNum[2] = answerNum[0] + answerNum[1];
            
            return answerNum;
        }
        //gets the responses for an open ended question will retrun as a string array
        public string[] GetUserResponseOpen(int qid)
        {
            return _context.OpenEndedResponses.Where(s => s.QuestionId == qid).Select(s => s.OpenUserResponse).ToArray();
        }
        public string[] GetUserOpenResponseQotD(int qid)
        {
            return _context.QuestionOfTheDayResponses.Where(s => s.QuestionOfTheDayId == qid).Select(s => s.QuestionOfTheDayOpenResponse).ToArray();
        }
        //get the words for a question depending on the question ID
        public string GetQuestionText(int qid)
        {
            var questionID = qid;
            return _context.Questions.Where(s => s.QuestionId == questionID).Select(s => s.QuestionText).FirstOrDefault().ToString();
        }
        //same as above but for question of the day
        public string GetQuestionTextQotD(int qid)
        {
            return _context.QuestionOfTheDays.Where(s => s.QuestionOfTheDayId == qid).Select(s => s.QuestionOfDayText).First().ToString();
        }
        //gets the answers a user can select for a question that has more then one answer
        public string[] GetAnswerText(int qid)
        {
            return _context.MutipleChoiceTexts.Where(s => s.QuestionId == qid).Select(s => s.AnswerText).ToArray();
        }
        //same as above but for question of the day
        public string[] GetAnswerTextQotD(int qid)
        {
            return _context.MutipleAnswerQoftheDays.Where(s => s.QuestionOfTheDayId == qid).Select(s => s.DayAnswerText).ToArray();
        }

        //adds a question to a survey in the database 
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
        //sends user answer to a true of false question
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
        // sends a user response to a question with more then one answer
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
        //same as above but for question of the day
        public void SendMutipleResponseQotD(int Qid, int userInt)
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
        //sends a user response to a open ended question
        public void SendOpenededResponse(int Qid, string userString)
        {
            QuestionOfTheDayResponse[] qotdr;
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@userAnwer", userAnswer);
            SqlParameter param2 = new SqlParameter("@questionID", QuestionID);
            qotdr = _context.QuestionOfTheDayResponses
                .FromSqlRaw("EXECUTE AddOpenEndedResponse @userAnwer, @questionID ", param1, param2)
                .ToArray();

        }
        //same as above but for question of the day
        public void SendOpenededResponseQotD(int Qid, string userString)
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
        //will update a the text of a question
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