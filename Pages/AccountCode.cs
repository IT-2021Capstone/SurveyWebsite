using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SurveyWebsite.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebsite.Pages
{
    public class AccountCode
    {
        private readonly ApplicationDbContext _context;
        public AccountCode(ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentSurvey(string userID, DateTime et, DateTime st)
        {
            var endtime = et;
            var starttime = st;
            string startSurveydate = _context.SurveyOrders.Where(a => a.StartTime == starttime).Select(a => a.StartTime).ToString();
            string endSurveydate = _context.SurveyOrders.Where(b => b.EndTime == endtime).Select(b => b.EndTime).ToString();
            string thisSurvey = _context.Users.Where(c => c.UserName == userID).Select(c => c.UserName).ToString();
            return startSurveydate + endSurveydate + thisSurvey;
        }

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

        public int[] ViewCreated(string userID)
        {
            int[] sid = new int[_context.Surveylists.Where(g => g.UserId == userID).Select(g => g.SurveyId).Count()];
            //var name = _context.SurveyOrders.Where(s => s.SurveyId == sid).Select(s => s.SurveyName).First().ToString();
            return sid;
        }

        public string ViewTaken(string userID)
        {
            return _context.SurveyTakens.Where(h => h.LoginId == userID).Select(h => h.LoginId).ToString();
        }
    }
}
//myuserid = "422c9c8e-03a5-4d4a-a2c2-835a3be77a98"