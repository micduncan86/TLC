using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using TLC.Data;

namespace TLC.global
{
    public partial class fetch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static List<User> GetTeamLeaders()
        {
            UserRepository userRepo = new UserRepository();
            return userRepo.GetAll().Where(x => x.MyTeamId <= 0).ToList();
        }

        [WebMethod]
        public static string SaveTeamLeader(List<string> leader)
        {
            TeamRepository tmRepo = new TeamRepository();
            System.Web.Script.Serialization.JavaScriptSerializer jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = jSerializer.Deserialize<Dictionary<string,object>>(leader.FirstOrDefault());
            Team myTeam = tmRepo.FindBy(Convert.ToInt32(data["TeamId"].ToString()));
            myTeam.TeamLeaderId = Convert.ToInt32(data["Id"].ToString());
            tmRepo.Update(myTeam);
            tmRepo.Save();
            return "";
        }
        
        [WebMethod]
        public static string ImportData(List<string> data)
        {
            var _data = data;
            System.Web.Script.Serialization.JavaScriptSerializer jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var rows = jSerializer.Deserialize<List<Dictionary<string, object>>>(data.FirstOrDefault());
            MemberRepository memberRepo = new MemberRepository();

            foreach(Dictionary<string, object> row in rows)
            {
                Member newMember = new Member();
                var memType = typeof(Member);
                foreach(KeyValuePair<string,object> pair in row)
                {
                    memType.GetProperty(pair.Key).SetValue(newMember, pair.Value);
                }
                memberRepo.Add(newMember);
            }

            memberRepo.Save();

            return "";
        }
    }
}