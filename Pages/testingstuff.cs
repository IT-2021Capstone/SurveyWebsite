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


        #region survey questions
        //gets all questions of a survey and question types of a survey 
        //This is stored in a tuple which holds 3 values the first value will be the question ID
        //the second value will be the question text and third will be question Type, and last will be is requried.
        //To use get data you need out use variableName.item1 for questionID
        //variableName.item2 for question text variableName.item3 for question type
        //variableName.item4 to check in the question is required
        //this will be easier then calling a a seperate method in html stuff
        public Tuple<int, string, int, bool>[] ViewSurveyQuestions(int surveyId)
        {

            Tuple<int, string, int, bool>[] surveyQuestionList = new Tuple<int, string, int, bool>[_context.Questions.Where(q => q.SurveyId == surveyId).Count()];
            int[] questionid = _context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.QuestionId).ToArray();
            string[] questionText = _context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.QuestionText).ToArray();
            int[] questiontype = _context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.QuestionType).ToArray();
            bool[] isRequired = _context.Questions.Where(s => s.SurveyId == surveyId).Select(q => q.IsRequired).ToArray();
            for (int i = 0; i < surveyQuestionList.Length; i++)
            {

                surveyQuestionList[i] = new Tuple<int, string, int, bool>(questionid[i], questionText[i], questiontype[i], isRequired[i]);

            }
            return surveyQuestionList;
        }
        //gets the question of the day info the question of the day id, the text for the question and the question type

        public Tuple<int, string, int> ViewQuestionsofTheDay()
        {
            //gets today's question ID
            int current = _context.QuestionOfTheDays.Where(qotd => DateTime.Now >= qotd.DateStarted && DateTime.Now <= qotd.DateEnded).Select(q => q.QuestionOfTheDayId).First();

            //gets the question of the day
            Tuple<int, string, int> surveyQuestionList;
            string questionText = _context.QuestionOfTheDays.Where(s => s.QuestionOfTheDayId == current).Select(q => q.QuestionOfDayText).First();
            int questiontype = _context.QuestionOfTheDays.Where(s => s.QuestionOfTheDayId == current).Select(q => q.QuestionOfDayType).First();
            surveyQuestionList = new Tuple<int, string, int>(current, questionText, questiontype);

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
        //gets start time for question of the day
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
        public Tuple<int, string, DateTime, DateTime>[] ViewOrder()
        {
            Tuple<int, string, DateTime, DateTime>[] orderList = new Tuple<int, string, DateTime, DateTime>[_context.SurveyOrders.Select(s => s.CurrentOrder).Count()];
            int[] listOrder = new int[_context.SurveyOrders.Select(d => d.CurrentOrder).Count()];
            listOrder = _context.SurveyOrders.Select(e => e.CurrentOrder).ToArray();
            int counter = 0;
            string[] name = new string[listOrder.Length];
            int[] ids = new int[listOrder.Length];
            DateTime[] startTimes = new DateTime[listOrder.Length];
            DateTime[] endTimes = new DateTime[listOrder.Length];
            foreach (int i in listOrder)
            {
                name[counter] = _context.SurveyOrders.Where(f => f.CurrentOrder == i).Select(f => f.SurveyName).First().ToString();
                startTimes[counter] = _context.SurveyOrders.Where(f => f.CurrentOrder == i).Select(f => f.StartTime).First();
                endTimes[counter] = _context.SurveyOrders.Where(f => f.CurrentOrder == i).Select(f => f.EndTime).First();
                if (_context.SurveyOrders.Where(s => s.CurrentOrder == i).Select(t => t.SurveyId).First() != null)
                {
                    ids[counter] = (Int32)_context.SurveyOrders.Where(s => s.CurrentOrder == i).Select(t => t.SurveyId).First();
                }
                else
                {
                    ids[counter] = (Int32)_context.SurveyOrders.Where(s => s.CurrentOrder == i).Select(t => t.QuestionOfTheDayId).First();
                }
                counter++;
            }
            for (int i = 0; i < orderList.Length; i++)
            {
                orderList[i] = new Tuple<int, string, DateTime, DateTime>(ids[i], name[i], startTimes[i], endTimes[i]);
            }
            return orderList;
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
        public int[] GetUserResponseMutipleAnswers(int qid, int TotalAnswers)
        {
            int tAnswers = TotalAnswers;
            var questionID = qid;
            int total = 0;
            int[] answerNum = new int[11];
            for (int i = 0; i < TotalAnswers; i++)
            {
                answerNum[i] = _context.MutipleChoiceResponses.Where(s => s.QuestionId == questionID && s.MutipleChoiceUserResponse == (i + 1)).Select(s => s.MutipleChoiceUserResponse).Count();
                total = total + answerNum[i];
            }
            answerNum[TotalAnswers] = total;
            return answerNum;
        }

        //same as above but for question of the day
        public int[] GetUserResponseMutipleAnswersQotD(int qid, int TotalAnswers)
        {
            int tAnswers = TotalAnswers;
            int total = 0;
            int[] answerNum = new int[11];
            for (int i = 0; i < TotalAnswers; i++)
            {
                answerNum[i] = _context.QuestionOfTheDayResponses.Where(s => s.QuestionOfTheDayId == qid && s.QuestionOfTheDayMutipleResponse == i + 1).Select(s => s.QuestionOfTheDayMutipleResponse).Count();
                total = total + answerNum[i];
            }
            answerNum[TotalAnswers] = total;
            return answerNum;
        }

        //gets the responses for true or false questions and the total number of responses
        public int[] GetUserTrueFalseResponses(int qid)
        {
            var questionID = qid;
            int[] answerNum = new int[3];
            answerNum[0] = _context.TrueFalseResponses.Where(s => s.QuestionId == questionID && s.TrueFalseUserResponse == 1).Select(s => s.TrueFalseUserResponse).Count();
            answerNum[1] = _context.TrueFalseResponses.Where(s => s.QuestionId == questionID && s.TrueFalseUserResponse == 0).Select(s => s.TrueFalseUserResponse).Count();
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
            if (_context.SurveyOrders.Count() > 0)
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
                    _context.Database.ExecuteSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                         param1, param2, param3);
                    break;
                case 2:
                    //true false question
                    _context.Database.ExecuteSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                         param1, param2, param3);
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
                    _context.Database.ExecuteSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType, @reqired ", param1, param2, param3, param4);
                    break;
                case 2:
                    //true false question
                    _context.Database.ExecuteSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType, @reqired ", param1, param2, param3, param4);
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
            var questionText = questiondaytext;
            var questionType = questiondaytype;
            DateTime starttime = start;
            DateTime endtime = end;
            SqlParameter param1 = new SqlParameter("@questionText", questionText);
            SqlParameter param2 = new SqlParameter("@questionOfDayType", questionType);
            SqlParameter param3 = new SqlParameter("@quetionStart", starttime);
            SqlParameter param4 = new SqlParameter("@questionEnd", endtime);
            _context.Database.ExecuteSqlRaw("EXECUTE AddQuestionOfTheDay @questionText, @questionOfDayType, @quetionStart, @questionEnd ", param1, param2, param3, param4);
            //use Sendmultiplequestionoftheday for multiple choice options

            int surveyID = LastQuestionAddedIdQotD();
            var name = surveyName;
            int currentO = GetLastSurvey() + 1;
            SqlParameter param5 = new SqlParameter("@id", surveyID);
            SqlParameter param6 = new SqlParameter("@currentO", currentO);
            SqlParameter param7 = new SqlParameter("@nameOfSurvey", name);
            //@id, @startTime, @endTime,@currentO,@nameOfSurvey
            _context.Database.ExecuteSqlRaw("EXECUTE AddSurveyOrderInfoQotD @id, @quetionStart, @questionEnd, @currentO, @nameOfSurvey", param5, param3, param4, param6, param7);
            return LastQuestionAddedIdQotD();
        }

        public int SendQuestionMultiple(int id, string text, int qtype, string[] options)
        {
            var surveyID = id;
            var questionType = qtype;
            var questionText = text;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);
            SqlParameter param2 = new SqlParameter("@questionText", questionText);
            SqlParameter param3 = new SqlParameter("@questionType", questionType);

            _context.Database.ExecuteSqlRaw("EXECUTE AddNonMutipleQuestion @surveyID, @questionText, @questionType",
                param1, param2, param3);

            int _questionID = LastQuestionAddedId();
            SqlParameter qid = new SqlParameter("@questionID", _questionID);

            foreach (string o in options)
            {
                //loop for each multiple choice option given
                SqlParameter option = new SqlParameter("@answerText", o);
                _context.Database.ExecuteSqlRaw("EXECUTE AddMutipleQuestion @questionID, @answerText",
                    qid, option);
            }

            return _questionID;
        }

        public int SendQuestionMultipleQotD(string questiondaytext, int questiondaytype, DateTime start, DateTime end, string[] options, string surveyName)
        {
            var questionText = questiondaytext;
            var questionType = questiondaytype;
            DateTime starttime = start;
            DateTime endtime = end;
            SqlParameter param1 = new SqlParameter("@questionText", questionText);
            SqlParameter param2 = new SqlParameter("@questionOfDayType", questionType);
            SqlParameter param3 = new SqlParameter("@quetionStart", starttime);
            SqlParameter param4 = new SqlParameter("@questionEnd", endtime);
            _context.Database.ExecuteSqlRaw("EXECUTE AddQuestionOfTheDay @questionText, @questionOfDayType, @quetionStart, @questionEnd ", param1, param2, param3, param4);
            int _questionID = LastQuestionAddedIdQotD();
            SqlParameter qid = new SqlParameter("@questionID", _questionID);
            foreach (string o in options)
            {
                //loop for each multiple choice option given
                SqlParameter option = new SqlParameter("@answerText", o);
                _context.Database.ExecuteSqlRaw("EXECUTE AddMutipleQuestionQotD @questionID, @answerText",
                    qid, option);
            }

            int surveyID = LastQuestionAddedIdQotD();
            var name = surveyName;
            int currentO = GetLastSurvey() + 1;
            SqlParameter param5 = new SqlParameter("@id", surveyID);
            SqlParameter param6 = new SqlParameter("@currentO", currentO);
            SqlParameter param7 = new SqlParameter("@nameOfSurvey", name);
            //@id, @startTime, @endTime,@currentO,@nameOfSurvey
            _context.Database.ExecuteSqlRaw("EXECUTE AddSurveyOrderInfoQotD @id, @quetionStart, @questionEnd, @currentO, @nameOfSurvey", param5, param3, param4, param6, param7);
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
            _context.Database.ExecuteSqlRaw("EXECUTE AddSurvey @userID, @dateCreated, @currentOrder", param1, param2, param3);


            int surveyID = GetCurrentSurvey();
            var startTime = stime;
            var endTime = etime;
            var name = surveyName;
            SqlParameter param4 = new SqlParameter("@id", surveyID);
            SqlParameter param5 = new SqlParameter("@startTime", startTime);
            SqlParameter param6 = new SqlParameter("@endTime", endTime);
            SqlParameter param7 = new SqlParameter("@nameOfSurvey", name);
            //@id, @startTime, @endTime,@currentO,@nameOfSurvey
            _context.Database.ExecuteSqlRaw("EXECUTE AddSurveyOrderInfo @id, @startTime, @endTime, @currentOrder, @nameOfSurvey", param4, param5, param6, param3, param7);


            return GetCurrentSurvey();
        }

        #endregion

        #region send user response to database

        //need to create proc for this
        public void SendUserTakenSurvey(string userId, int surveyId)
        {
            SqlParameter param1 = new SqlParameter("@userID", userId);
            SqlParameter param2 = new SqlParameter("@surveyID", surveyId);

             _context.Database.ExecuteSqlRaw("EXECUTE AddSurveyTaken @userID, @surveyID", param1, param2);
        }
        //sends user answer to a true of false question
        public void SendTrueFalseResponse(int Qid, int userInt)
        {
            int userAnswer = userInt;
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@userAnwer", userAnswer);
            SqlParameter param2 = new SqlParameter("@questionID", QuestionID);

            _context.Database.ExecuteSqlRaw("EXECUTE AddTrueFalseResponse @userAnwer, @questionID", param1, param2);
        }
        // sends a user response to a question with more then one answer
        public void SendMutipleResponse(int Qid, int userInt)
        {
            int userAnswer = userInt;
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@userAnwer", userAnswer);
            SqlParameter param2 = new SqlParameter("@questionID", QuestionID);

            _context.Database.ExecuteSqlRaw("EXECUTE AddMutiplequestionResponse @userAnwer, @questionID", param1, param2);
        }

        //same as above but for question of the day
        public void SendMutipleResponseQotD(int Qid, int userInt)
        {
            int QuestionID = Qid;
            int userAnswer = userInt;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionMutiResponse", userAnswer);
            _context.Database.ExecuteSqlRaw("EXECUTE AddQuestionOfTheDayMutiResponse @questionID, @questionMutiResponse", param1, param2);
        }
        //sends a user response to a open ended question
        public void SendOpenededResponse(int Qid, string userString)
        {

            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@userAnwer", userAnswer);
            SqlParameter param2 = new SqlParameter("@questionID", QuestionID);
            _context.Database.ExecuteSqlRaw("EXECUTE AddOpenEndedResponse @userAnwer, @questionID ", param1, param2);
        }
        //same as above but for question of the day
        public void SendOpenededResponseQotD(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionOpenResponse", userAnswer);

            _context.Database.ExecuteSqlRaw("EXECUTE AddQuestionOfTheDayOpenResponse @questionID, @questionOpenResponse", param1, param2);

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

            _context.Database.ExecuteSqlRaw("EXECUTE UpdateQuestionText @questionID, @questionText", param1, param2);
        }

        //same as above but for question of the day
        public void UpdateQuestionTextQotD(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);

            _context.Database.ExecuteSqlRaw("EXECUTE UpdateQuestionTextQotD @questionID, @questionText", param1, param2);
        }
        //will update the text for mutiple choice questions 
        public void UpdateMutipleChoiceText(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);
            _context.Database.ExecuteSqlRaw("EXECUTE UpdateMutipleAnswerText @questionID, @questionText", param1, param2);
        }

        //will update the text for mutiple choice questions 
        public void UpdateMutipleChoiceTextQotD(int Qid, string userString)
        {
            int QuestionID = Qid;
            string userAnswer = userString;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);
            SqlParameter param2 = new SqlParameter("@questionText", userAnswer);
            _context.Database.ExecuteSqlRaw("EXECUTE UpdateMutipleAnswerTextQotD @questionID, @questionText", param1, param2);
        }

        #endregion

        #region delete questions and more from the database

        //will delete answers from mutiple choice questions
        public void DeleteMutipleChoiceText(int qid)
        {
            int surveyID = qid;
            SqlParameter param1 = new SqlParameter("@questionID", surveyID);

            _context.Database.ExecuteSqlRaw("EXECUTE deleteMutipleAnswerText @questionID", param1);
        }
        //will delete answers from mutiple choice questions
        public void DeleteMutipleChoiceTextQotD(int qid)
        {
            int surveyID = qid;
            SqlParameter param1 = new SqlParameter("@questionID", surveyID);

            _context.Database.ExecuteSqlRaw("EXECUTE deleteMutipleAnswerTextQotD @questionID", param1);
        }
        //deletes the question entered
        public void DeleteQuestion(int Qid)
        {
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);

            _context.Database.ExecuteSqlRaw("EXECUTE deleteQuestionText @questionID", param1);
        }
        //deletes the question entered
        public void DeleteQuestionQotD(int Qid)
        {
            int QuestionID = Qid;
            SqlParameter param1 = new SqlParameter("@questionID", QuestionID);

            _context.Database.ExecuteSqlRaw("EXECUTE deleteQuestionTextQofD @questionID", param1);
        }

        //deletes the survey entered
        public void DeleteSurvey(int sid)
        {
            int surveyID = sid;
            SqlParameter param1 = new SqlParameter("@surveyID", surveyID);

            _context.Database.ExecuteSqlRaw("EXECUTE DeleteSurvey @surveyID", param1);
        }

        #endregion


        //change the role of a user to give them the ablitily to create surveys
        public void UpdateUserRole(string uId, string roleName)
        {
            string UserId = uId;
            string role = roleName;
            SqlParameter param1 = new SqlParameter("@role", role);
            SqlParameter param2 = new SqlParameter("@LoginId", UserId);
            _context.Database.ExecuteSqlRaw("EXECUTE ChangeUserRole @role, @LoginId", param1, param2);
        }

        public void AddUserRole(string uId, string roleName)
        {
            string UserId = uId;
            string role = roleName;
            SqlParameter param1 = new SqlParameter("@role", role);
            SqlParameter param2 = new SqlParameter("@LoginId", UserId);
            _context.Database.ExecuteSqlRaw("EXECUTE AddUserRole @role, @LoginId", param1, param2);
        }

        public void DeleteUserRole(string uId, string roleName)
        {
            string UserId = uId;
            SqlParameter param1 = new SqlParameter("@LoginId", UserId);
            _context.Database.ExecuteSqlRaw("EXECUTE DeleteUserRole @LoginId", param1);
        }
    }
}