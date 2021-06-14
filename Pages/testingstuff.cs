﻿using DataAccess.Models;
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


        #region survey questions
        //gets all questions of a survey and question types of a survey 
        //This is stored in a tuple which holds 3 values the first value will be the question ID
        //the second value will be the question text and third will be question Type, and last will be is requried.
        //To use get data you need out use varableName.item1 for questionID
        //varableName.item2 for question text varableName.item3 for question type
        //varableName.item4 to check in the question is required
        //this will be easier then calling a a sepreate method in html stuff
        public Tuple<int, string, int, bool>[] ViewSurveyQuestions(int surveyId) 
        {

            Tuple<int, string, int, bool>[] surveyQuestionList = new Tuple<int, string, int, bool>[_context.Questions.Where(q => q.SurveyId == surveyId).Count()];
            int[] questionid = _context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.QuestionId).ToArray();
            string[] questionText = _context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.QuestionText).ToArray();
            int[] questiontype = _context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.QuestionType).ToArray();
            bool[] isRequired =_context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.IsRequired).ToArray();
            for (int i = 0; i < surveyQuestionList.Length; i++)
            {

                surveyQuestionList[i] =new Tuple<int, string, int, bool>(questionid[i],questionText[i],questiontype[i], isRequired[i]);
            }
            return surveyQuestionList;
        }

        //get the text for mutiple choice questions
        public string[] ViewMutipleChoice(int qid)
        {
            return _context.MutipleChoiceTexts.Where(a => a.QuestionId == qid).Select(b => b.AnswerText).ToArray();        
        }
        //same but for question of the day 
        public string[] ViewMutipleChoiceQotD(int qid)
        {
            return _context.MutipleAnswerQoftheDays.Where(a => a.QuestionOfTheDayId == qid).Select(b => b.DayAnswerText).ToArray();
        }

        #endregion

        #region start and end times
        //gets start time of a question of the day
        public DateTime ViewStartTimeQotD(int qid)
        {          
            return (DateTime)_context.QuestionOfTheDays.Where(a => a.QuestionOfTheDayId == qid).Select(b => b.DateStarted).First();
        }
        //gets the end time for question of the day
        public DateTime ViewEndTimeQotD(int qid)
        {
            return (DateTime)_context.QuestionOfTheDays.Where(a => a.QuestionOfTheDayId == qid).Select(b => b.DateEnded).First();
        }
        //gets the start time for a survey used to find if you can take a survey
        public DateTime ViewStartTime(int surveyId)
        {
            return (DateTime)_context.SurveyOrders.Where(a => a.SurveyId == surveyId).Select(b => b.StartTime).First();
        }
        //gets the end time for a survey
        public DateTime ViewEndTime(int surveyId)
        {
            return (DateTime)_context.SurveyOrders.Where(a => a.SurveyId == surveyId).Select(b => b.EndTime).First();
        }
        #endregion

        #region Account info
        //see current list of all surveys on the website that exits, current past and furture
        public string[] ViewOrder()
        {
            int[] listOrder = new int[_context.SurveyOrders.Select(d => d.CurrentOrder).Count()];
            listOrder = _context.SurveyOrders.Select(e => e.CurrentOrder).ToArray();
            int counter = 0;
            string[] name = new string[listOrder.Length];
            foreach (int i in listOrder)
            {
                name[counter] = _context.SurveyOrders.Where(f => f.CurrentOrder == i).Select(f => f.SurveyName).First().ToString();
                counter++;
            }
            return name;
        }

        //gets list of all surveys created only works if person has created surveys and they are logged in
        public Tuple<string, int>[] ViewCreated(string userID)
        {
            Tuple<string, int>[] list = new Tuple<string, int>[_context.Surveylists.Where(g => g.UserId == userID).Count()];
            int[] sids = _context.Surveylists.Where(g => g.UserId == userID).Select(g => g.SurveyId).ToArray();
            int counter = 0;
            string[] names = new string[sids.Length];

            foreach (int s in sids)
            {
                names[counter] = _context.SurveyOrders.Where(a => a.SurveyId == s).Select(a => a.SurveyName).First().ToString();
                counter++;
            }
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = new Tuple<string, int>(names[i], sids[i]);
            }
            return list;
        }
        //gets all surveys taken by current user, only works if logged in
        public Tuple<string, int>[] ViewTaken(string userID)
        {
            int[] sids = _context.SurveyTakens.Where(g => g.LoginId == userID).Select(g => g.SurveyId).ToArray();
            Tuple<string, int>[] list = new Tuple<string, int>[sids.Length];
            string[] names = new string[sids.Length];
            int counter = 0;
            foreach (int s in sids)
            {
                names[counter] = _context.SurveyOrders.Where(a => a.SurveyId == s).Select(a => a.SurveyName).First().ToString();
                counter++;
            }
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = new Tuple<string, int>(names[i], sids[i]);
            }
            return list;
        }
        #endregion

        #region get user resposnse
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

        #endregion

        #region retrun IDs for creating questions and surveys

        //gets the most recent question added
        private int LastQuestionAddedId()
        {
            int qid = _context.Questions.OrderByDescending(q => q.QuestionId).FirstOrDefault().QuestionId;
            return qid;
        }
        //same as above but for question of the day
        private int LastQuestionAddedIdQotD()
        {
            int qid = _context.QuestionOfTheDays.OrderByDescending(q => q.QuestionOfTheDayId).FirstOrDefault().QuestionOfTheDayId;
            return qid;
        }
        //gets the int of the last item in current order to add to create survey or to find the last survey in the list
        private int GetLastSurvey()
        {
            int sid;
            if (_context.SurveyOrders.Count() > 0 )
                sid = _context.SurveyOrders.OrderByDescending(q => q.CurrentOrder).FirstOrDefault().CurrentOrder;
            else 
            {
                sid = 0;
            }
            return sid;
        }
        //used to get the most recently added survey
        private int GetCurrentSurvey()
        {
            int sid = _context.Surveylists.OrderByDescending(q => q.SurveyId).FirstOrDefault().SurveyId;
            return sid;
        }

        #endregion

        #region add questions and surveys to the database

        //first test will update when done
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

        public int SendQuestionOfTheDay(string questiondaytext, int questiondaytype, DateTime start, DateTime end, string surveyName)
        {
            QuestionOfTheDay[] q;
            var questionText = questiondaytext;
            var questionType = questiondaytype;
            DateTime starttime = start;
            DateTime endtime = end;
            SqlParameter param1 = new SqlParameter("@questionText", questionText);
            SqlParameter param2 = new SqlParameter("@questionOfDayType", questionType);
            SqlParameter param3 = new SqlParameter("@quetionStart", starttime);
            SqlParameter param4 = new SqlParameter("@questionEnd", endtime);
            q = _context.QuestionOfTheDays
                .FromSqlRaw("EXECUTE AddQuestionOfTheDay @questionText, @questionOfDayType, @quetionStart, @questionEnd ", param1, param2, param3, param4)
                .ToArray();
            //use Sendmultiplequestionoftheday for multiple choice options

            int surveyID = LastQuestionAddedIdQotD();
            var name = surveyName;
            int currentO = GetLastSurvey() + 1;
            SqlParameter param5 = new SqlParameter("@id", surveyID);
            SqlParameter param6 = new SqlParameter("@currentO", currentO);
            SqlParameter param7 = new SqlParameter("@nameOfSurvey", name);
            //@id, @startTime, @endTime,@currentO,@nameOfSurvey
            var surveyO = _context.SurveyOrders
            .FromSqlRaw("EXECUTE AddSurveyOrderInfoQotD @id, @quetionStart, @questionEnd, @currentO, @nameOfSurvey", param5, param3, param4, param6, param7)
            .ToList();
            return LastQuestionAddedIdQotD();
        }

        public int SendQuestionMultiple(int id, string text, int qtype, string[] options)
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

            int _questionID = LastQuestionAddedId() ;
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

        public int SendQuestionMultipleQotD(string questiondaytext, int questiondaytype, DateTime start, DateTime end, string[] options, string surveyName)
        {
            QuestionOfTheDay[] q;
            var questionText = questiondaytext;
            var questionType = questiondaytype;
            DateTime starttime = start;
            DateTime endtime = end;
            SqlParameter param1 = new SqlParameter("@questionText", questionText);
            SqlParameter param2 = new SqlParameter("@questionOfDayType", questionType);
            SqlParameter param3 = new SqlParameter("@quetionStart", starttime);
            SqlParameter param4 = new SqlParameter("@questionEnd", endtime);
            q = _context.QuestionOfTheDays
                .FromSqlRaw("EXECUTE AddQuestionOfTheDay @questionText, @questionOfDayType, @quetionStart, @questionEnd ", param1, param2, param3, param4)
                .ToArray();

            MutipleAnswerQoftheDay[] mc;
            int _questionID = LastQuestionAddedIdQotD();
            SqlParameter qid = new SqlParameter("@questionID", _questionID);
            foreach (string o in options)
            {
                //loop for each multiple choice option given
                SqlParameter option = new SqlParameter("@answerText", o);
                mc = _context.MutipleAnswerQoftheDays.FromSqlRaw("EXECUTE AddMutipleQuestionQotD @questionID, @answerText",
                    qid, option)
                    .ToArray();
            }

            int surveyID = LastQuestionAddedIdQotD();
            var name = surveyName;
            int currentO = GetLastSurvey() + 1;
            SqlParameter param5 = new SqlParameter("@id", surveyID);
            SqlParameter param6 = new SqlParameter("@currentO", currentO);
            SqlParameter param7 = new SqlParameter("@nameOfSurvey", name);
            //@id, @startTime, @endTime,@currentO,@nameOfSurvey
            var surveyO = _context.SurveyOrders
            .FromSqlRaw("EXECUTE AddSurveyOrderInfoQotD @id, @quetionStart, @questionEnd, @currentO, @nameOfSurvey", param5, param3, param4, param6, param7)
            .ToList();
            return _questionID;
        }

        public int AddSurvey(string uId, DateTime stime, DateTime etime, string surveyName)
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


            int surveyID = GetCurrentSurvey();
            var startTime = stime;
            var endTime = etime;
            var name = surveyName;
            SqlParameter param4 = new SqlParameter("@id", surveyID);
            SqlParameter param5 = new SqlParameter("@startTime", startTime);
            SqlParameter param6 = new SqlParameter("@endTime", endTime);
            SqlParameter param7 = new SqlParameter("@nameOfSurvey", name);
            //@id, @startTime, @endTime,@currentO,@nameOfSurvey
            var surveyO = _context.SurveyOrders
            .FromSqlRaw("EXECUTE AddSurveyOrderInfo @id, @startTime, @endTime, @currentOrder, @nameOfSurvey", param4, param5, param6, param3, param7)
            .ToList();


            return GetCurrentSurvey();
        }

        #endregion

        #region send user response to database

        //need to create proc for this
        public void SendUserTakenSurvey(string userId, int surveyId)
        {
            SqlParameter param1 = new SqlParameter("@userID", userId);
            SqlParameter param2 = new SqlParameter("@surveyID", surveyId);
            var taken = _context.SurveyTakens.FromSqlRaw("EXECUTE AddSurveyTaken @userID, @surveyID", param1, param2).ToList();
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

        #endregion

        #region updates to questions

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

        //same as above but for question of the day
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

        #endregion

        #region delete questions and more from the database

        //will delete answers from mutiple choice questions
        public void DeleteMutipleChoiceText(int qid)
        {
            int surveyID = qid;
            SqlParameter param1 = new SqlParameter("@questionID", surveyID);

            var qotdr = _context.MutipleChoiceTexts
                 .FromSqlRaw("EXECUTE deleteMutipleAnswerText @questionID", param1)
                 .ToArray();
        }
        //will delete answers from mutiple choice questions
        public void DeleteMutipleChoiceTextQotD(int qid)
        {
            int surveyID = qid;
            SqlParameter param1 = new SqlParameter("@questionID", surveyID);

            var qotdr = _context.MutipleAnswerQoftheDays
                 .FromSqlRaw("EXECUTE deleteMutipleAnswerTextQotD @questionID", param1)
                 .ToArray();
        }
        //deletes the question entered
        public void DeleteQuestion(int Qid)
        {
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);

            var qotdr = _context.Questions
                 .FromSqlRaw("EXECUTE deleteQuestionText @questionID", param1)
                 .ToArray();
        }
        //deletes the question entered
        public void DeleteQuestionQotD(int Qid)
        {
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);

            var qotdr = _context.QuestionOfTheDays
                 .FromSqlRaw("EXECUTE deleteQuestionTextQofD @questionID", param1)
                 .ToArray();
        }

        //deletes the survey entered
        public void DeleteSurvey(int sid)
        {
            int surveyID = sid;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);

            var qotdr = _context.Surveylists
                 .FromSqlRaw("EXECUTE DeleteSurvey @surveyID", param1)
                 .ToArray();
        }

        #endregion




        // will get user role once I have it set up used to see who can create surveys
        //public int GetUserRole(string role)
        //{
        //    int roletype = (Int32)_context.AspNetUserRoles.Where(u => u.RoleId == role).Select(r => r.RoleType).FirstOrDefault();
        //    return roletype;
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