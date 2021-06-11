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

        //see current list of all surveys on the website that exits
        public string[] ViewOrder()
        {
            int[] listOrder = new int[_context.SurveyOrders.Select(d => d.CurrentOrder).Count()];
            listOrder = _context.SurveyOrders.Select(e => e.CurrentOrder).ToArray();
            string[] name = new string[listOrder.Length];
            foreach (int i in listOrder)
            {
                name[i - 1] = _context.SurveyOrders.Where(f => f.CurrentOrder == i).Select(f => f.SurveyName).First().ToString();
            }
            return name;
        }
        //gets list of all surveys created only works if person has created surveys
        public string[] ViewCreated(string userID)
        {
            int[] sids = _context.Surveylists.Where(g => g.UserId == userID).Select(g => g.SurveyId).ToArray();
            string[] names = new string[_context.Surveylists.Where(g => g.UserId == userID).Select(g => g.SurveyId).Count()];
            foreach (int s in sids)
            {
                names[s] = _context.SurveyOrders.Where(a => a.SurveyId == s).Select(a => a.SurveyName).First().ToString();
            }
            return names;
        }
        //gets all surveys taken by current user, only works if logged in
        public string[] ViewTaken(string userID)
        {
            int[] sids = _context.SurveyTakens.Where(g => g.LoginId == userID).Select(g => g.SurveyId).ToArray();
            string[] names = new string[_context.SurveyTakens.Where(g => g.LoginId == userID).Select(g => g.SurveyId).Count()];
            foreach (int s in sids)
            {
                names[s] = _context.SurveyOrders.Where(a => a.SurveyId == s).Select(a => a.SurveyName).First().ToString();
            }
            return names;
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
        //same as above but for question of the day
        public string[] GetUserOpenResponseQotD(int qid)
        {
            return _context.QuestionOfTheDayOpenResponses.Where(s => s.QuestionOfTheDayId == qid).Select(s => s.QuestionOfTheDayOpenResponse1).ToArray();
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
        //checks if the question is required to be answered for use in surevys
        public bool IsRequredAnswer(int qid) 
        {
            var required = _context.Questions.Where(q => q.QuestionId == qid).Select(r => r.IsRequired).First();
            
            return (bool)required;
        }

        // will get user role once I have it set up used to see who can create surveys
        //public int GetUserRole(string role)
        //{
        //    int roletype = (Int32)_context.AspNetUserRoles.Where(u => u.RoleId == role).Select(r => r.RoleType).FirstOrDefault();
        //    return roletype;
        //}

        //gets the most recent question added
        private int LastQuestionAddedId()
        {
            int qid = _context.Questions.OrderByDescending(q =>q.QuestionId).FirstOrDefault().QuestionId;
            return qid;
        }
        //gets the int of the last item in current order to add to create survey or to find the last survey in the list
        private int GetLastSurvey()
        {
            int sid = _context.SurveyOrders.OrderByDescending(q => q.CurrentOrder).FirstOrDefault().CurrentOrder;
            return sid;
        }
        //used to get the most recently added survey
        private int GetCurrentSurvey()
        {
            int sid = _context.Surveylists.OrderByDescending(q => q.SurveyId).FirstOrDefault().SurveyId;
            return sid;
        }
        //used to add a survey to the database for creating surveys.
        public int AddSurvey(string uId)
        {
            string UserId = uId;
            int currentO = GetLastSurvey() + 1;
            DateTime now = DateTime.Now;

            SqlParameter param1 = new SqlParameter("@userID", UserId);
            SqlParameter param2 = new SqlParameter("@dateCreated", now);
            SqlParameter param3 = new SqlParameter("@currentOrder", currentO);
            var qotdr = _context.Surveylists
                 .FromSqlRaw("EXECUTE AddSurvey @userID, @dateCreated, @currentOrder", param1, param2, param3)
                 .ToList();
            return GetCurrentSurvey();
        }

        //adds a question to a survey in the database 
        public int SendQuestion(int id, string text, int qtype )
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
            return LastQuestionAddedId();
        }
        //adds the option to send a quesiton where an answer is not required
        public int SendQuestion(int id, string text, int qtype, bool required)
        {
            Question[] q;
            var surveyID = id;
            var questionText = text;
            var questionType = qtype;
            var questionReqired = required;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionType", questionType);
            SqlParameter param4 = new SqlParameter("@reqired ", questionReqired);
            q = _context.Questions
                .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType, @reqired ", param1, param2, param3, param4)
                .ToArray();
            return LastQuestionAddedId();
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
            int QuestionID = Qid;
            int userAnswer = userInt;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionMutiResponse", userAnswer);
             var send =  _context.QuestionOfTheDayResponses
                .FromSqlRaw("EXECUTE AddQuestionOfTheDayMutiResponse @questionID, @questionMutiResponse", param1, param2)
                .ToArray();
        }
        //sends a user response to a open ended question
        public void SendOpenededResponse(int Qid, string userString)
        {
            
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@userAnwer", userAnswer);
            SqlParameter param2 = new SqlParameter("@questionID", QuestionID);
            var qotdr = _context.OpenEndedResponses
                .FromSqlRaw("EXECUTE AddOpenEndedResponse @userAnwer, @questionID ", param1, param2)
                .ToArray();
        }
        //same as above but for question of the day
        public void SendOpenededResponseQotD(int Qid, string userString)
        {       
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionOpenResponse", userAnswer);

           var qotdr = _context.QuestionOfTheDayOpenResponses
                .FromSqlRaw("EXECUTE AddQuestionOfTheDayOpenResponse @questionID, @questionOpenResponse", param1, param2)
                .ToArray();

        }
        //will update a the text of a question
        public void UpdateQuestionText(int Qid, string userString)
        {            
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);

           var qotdr = _context.Questions
                .FromSqlRaw("EXECUTE UpdateQuestionText @questionID, @questionText", param1, param2)
                .ToArray();
        }

        public void UpdateQuestionTextQotD(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);

            var qotdr = _context.QuestionOfTheDays
                 .FromSqlRaw("EXECUTE UpdateQuestionTextQotD @questionID, @questionText", param1, param2)
                 .ToArray();
        }
        //will update the text for mutiple choice questions 
        public void UpdateMutipleChoiceText(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);
            var qotdr = _context.MutipleChoiceTexts
                 .FromSqlRaw("EXECUTE UpdateMutipleAnswerText @questionID, @questionText", param1, param2)
                 .ToArray();
        }
        //will update the text for mutiple choice questions 
        public void UpdateMutipleChoiceTextQotD(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);
            var qotdr = _context.MutipleAnswerQoftheDays
                 .FromSqlRaw("EXECUTE UpdateMutipleAnswerTextQotD @questionID, @questionText", param1, param2)
                 .ToArray();
        }
        ////will delete answers from mutiple choice questions
        //public void DeleteMutipleChoiceText(int qid)
        //{
        //    int surveyID = qid;
        //    SqlParameter param1 = new SqlParameter("@questionID", surveyID);

        //    var qotdr = _context.MutipleChoiceTexts
        //         .FromSqlRaw("EXECUTE deleteMutipleAnswerText @questionID", param1)
        //         .ToArray();
        //}
        ////will delete answers from mutiple choice questions
        //public void DeleteMutipleChoiceTextQotD(int qid)
        //{
        //    int surveyID = qid;
        //    SqlParameter param1 = new SqlParameter("@questionID", surveyID);

        //    var qotdr = _context.MutipleAnswerQoftheDays
        //         .FromSqlRaw("EXECUTE deleteMutipleAnswerTextQotD @questionID", param1)
        //         .ToArray();
        //}
        ////deletes the question entered
        //public void DeleteQuestion(int Qid)
        //{
        //    int QuestionID = Qid;
        //    SqlParameter param1 = new SqlParameter("@questionID", QuestionID);

        //    var qotdr = _context.Questions
        //         .FromSqlRaw("EXECUTE deleteQuestionText @questionID", param1)
        //         .ToArray();
        //}
        ////deletes the question entered
        //public void DeleteQuestionQotD(int Qid)
        //{
        //    int QuestionID = Qid;
        //    SqlParameter param1 = new SqlParameter("@questionID", QuestionID);

        //    var qotdr = _context.QuestionOfTheDays
        //         .FromSqlRaw("EXECUTE deleteQuestionTextQotD @questionID", param1)
        //         .ToArray();
        //}

        ////deletes the survey entered
        //public void DeleteSurvey(int sid)
        //{
        //    int surveyID = sid;
        //    SqlParameter param1 = new SqlParameter("@surveyID", surveyID);

        //    var qotdr = _context.Surveylists
        //         .FromSqlRaw("EXECUTE DeleteSurvey @surveyID", param1)
        //         .ToArray();
        //}
        //change the role of a user to give them the ablitily to create surveys
        //public void ChangeUserRole(string uId, int roleType)
        //{
        //    string UserId = uId;
        //    int role = roleType;
        //    SqlParameter param1 = new SqlParameter("@role", role);
        //    SqlParameter param2 = new SqlParameter("@LoginId", UserId);
        //    var qotdr = _context.AspNetUserRoles
        //         .FromSqlRaw("EXECUTE ChangeUserRole @role, @LoginId", param1, param2)
        //         .ToArray();

        //}
    }
}