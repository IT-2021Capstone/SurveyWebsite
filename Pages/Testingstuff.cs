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
            for (int i = 0; i < TAnswers; i++)
            {
                answerNum[i] = _context.MutipleChoiceResponses.Where(s => s.QuestionId == questionID && s.MutipleChoiceUserResponse == i + 1).Select(s => s.MutipleChoiceUserResponse).Count();
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

        //gets the choices a user can select for a question that has more then one answer
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
            int qid = _context.Questions.OrderByDescending(q => q.QuestionId).FirstOrDefault().QuestionId;
            return qid;
        }

        // adding a question to the database

        //adds a question to a survey in the database 

        public int SendQuestion(int id, string text, int qtype)
        {
            //qtype 1 = open ended, qtype 2 = true/false, qtype 3 = multiple choice
            Question[] q;
            var surveyID = id;
            var questionType = qtype;
            var questionText = text;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionType", questionType);
            switch (qtype)
            {
                case 1:
                    //open ended question
                    q = _context.Questions
                         .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                         param1, param2, param3)
                         .ToArray();
                    break;
                case 2:
                    //true false question
                    q = _context.Questions
                         .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                         param1, param2, param3)
                         .ToArray();
                    //unnecessary distinction of cases for now, but if tables or procedures are modified this could be a helpful distinction
                    break;
                case 3:
                    //multiple choice question, use the multiople choice method not this one

                    break;
            }
            return LastQuestionAddedId();
        }

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
            switch (qtype)
            {
                case 1:
                    //open ended question
                    q = _context.Questions
                         .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType, @reqired ", param1, param2, param3, param4)
                         .ToArray();
                    break;
                case 2:
                    //true false question
                    q = _context.Questions
                         .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType, @reqired ", param1, param2, param3, param4)
                         .ToArray();
                    //unnecessary distinction of cases for now, but if tables or procedures are modified this could be a helpful distinction
                    break;
                case 3:
                    //multiple choice question, use the multiople choice method not this one
                    break;
            }
            return LastQuestionAddedId();
        }

        public int SendMultipleQuestion(int id, string text, int qtype, string[] options)
        {
            Question[] q;
            var surveyID = id;
            var questionType = qtype;
            var questionText = text;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionType", questionType);

            q = _context.Questions
                .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                param1, param2, param3)
                .ToArray();

            int _questionID = LastQuestionAddedId();
            SqlParameter qid = new SqlParameter("@questionID", _questionID);
            MutipleChoiceText[] mc;

            foreach (string o in options)
            {
                //loop for each multiple choice option given
                SqlParameter option = new SqlParameter("@answerText", o);
                mc = _context.MutipleChoiceTexts.FromSqlRaw("EXECUTE AddMutipleQuestion @questionID, @answerText",
                    qid, option)
                    .ToArray();
            }

            return _questionID;
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
        public int SendQuestionOfTheDay(int questiondayid, string questiondaytext, int questiondaytype)
        {
            QuestionOfTheDay[] q;
            var qid = questiondayid;
            var questionText = questiondaytext;
            var questionType = questiondaytype;
            SqlParameter param1 = new SqlParameter("@questiondayId", qid);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionOfDayType", questionType);
            q = _context.QuestionOfTheDays
                .FromSqlRaw("EXECUTE AddQuestionOfTheDayID @questiondayId, @questionText,@questionOfDayType", param1, param2, param3)
                .ToArray();
            //use Sendmultiplequestionoftheday for multiple choice options

            return LastQuestionAddedId();
        }

        public int SendMultipleQuestionOfTheDay(int questiondayid, string questiondaytext, int questiondaytype, string[] options)
        {
            QuestionOfTheDay[] q;
            var qid = questiondayid;
            var questionText = questiondaytext;
            var questionType = questiondaytype;
            SqlParameter param1 = new SqlParameter("@questiondayId", qid);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionOfDayType", questionType);

            q = _context.QuestionOfTheDays
                .FromSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                param1, param2, param3)
                .ToArray();

            MutipleChoiceText[] mc;

            foreach (string o in options)
            {
                //loop for each multiple choice option given
                SqlParameter option = new SqlParameter("@answerText", o);
                mc = _context.MutipleChoiceTexts.FromSqlRaw("EXECUTE AddMutipleQuestion @questionID, @answerText",
                    qid, option)
                    .ToArray();
            }

            return qid;
        }
        public void SendSurvey(string userid, int currentorder)
        {
            Surveylist[] s;
            var uid = userid;
            //use getuser?
            var date = DateTime.Now;
            var ordernum = currentorder;
            SqlParameter param1 = new SqlParameter("@userID", uid);
            SqlParameter param2 = new SqlParameter("@dateCreated", date);
            SqlParameter param3 = new SqlParameter("@currentOrder", ordernum);

            //get survey ID and return it?
            s = _context.Surveylists
                .FromSqlRaw("EXECUTE AddSurvey @userID,@dateCreated,@currentOrder", param1, param2, param3)
                .ToArray();
        }

        // sends a user response to a question with more then one answer
        public void SendMutipleResponse(int Qid, int userInt)
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
            var send = _context.QuestionOfTheDayResponses
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
        //deletes the question entered
        public void DeleteQuestion(int Qid)
        {
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);

            var qotdr = _context.Questions
                 .FromSqlRaw("EXECUTE UpdateQuestionText @questionID", param1)
                 .ToArray();
        }
        //deletes the survey entered
        public void DeleteSurvey(int sid)
        {
            int surveyID = sid;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);

            var qotdr = _context.Surveylists
                 .FromSqlRaw("EXECUTE UpdateQuestionText @surveyID", param1)
                 .ToArray();
        }
        //will update the text for mutiple choice questions 
        public void UpdateMutipleChoiceText(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);
            var qotdr = _context.Questions
                 .FromSqlRaw("EXECUTE UpdateMutipleAnswerText @questionID, @questionText", param1, param2)
                 .ToArray();
        }
        //will delete answers from mutiple choice questions
        public void DeleteMutipleChoiceText(int qid)
        {
            int surveyID = qid;
            SqlParameter param1 = new SqlParameter("@questionID", surveyID);

            var qotdr = _context.MutipleChoiceTexts
                 .FromSqlRaw("EXECUTE deleteMutipleAnswerText @questionID", param1)
                 .ToArray();
        }
        //change the role of a user to give them the ablitily to create surveys
        public void ChangeUserRole(string uId, int roleType)
        {
            string UserId = uId;
            int role = roleType;
            SqlParameter param1 = new SqlParameter("@role", role);
            SqlParameter param2 = new SqlParameter("@LoginId", UserId);
            var qotdr = _context.AspNetUserRoles
                 .FromSqlRaw("EXECUTE ChangeUserRole @role, @LoginId", param1, param2)
                 .ToArray();

        }

    }
}