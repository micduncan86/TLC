using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TLC.Data;

namespace TLC
{
    public class TeamController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Team> Get()
        {
            return new TeamRepository().GetAll();
        }

        // GET api/<controller>/5
        public Team Get(int id)
        {
            return new TeamRepository().FindBy(id);
        }

        // POST api/<controller>
        public Team Post([FromBody]string formdata)
        {
            var TeamRepo = new TeamRepository();
            if (formdata != string.Empty)
            {
                var formValues = formdata.Split('&').Select(s => s.Split('=')).ToDictionary(key => key[0].Trim(), value => value[1].Trim());
                var newTeam = new Team(formValues["TeamName"].Replace("+"," "),formValues["GroupNumber"]);
                newTeam.TeamLeaderId = Convert.ToInt32(formValues["TeamLeaderId"]);                
                TeamRepo.Add(newTeam);
                TeamRepo.Save();
                return Get(newTeam.TeamId);
            }
            return new Team();
        }

        // PUT api/<controller>/5
        public Team Put(int id, [FromBody]string formdata)
        {
            var TeamRepo = new TeamRepository();
            if (formdata != string.Empty)
            {
                var formValues = formdata.Split('&').Select(s => s.Split('=')).ToDictionary(key => key[0].Trim(), value => value[1].Trim());
                var team = TeamRepo.FindBy(id);
                team.Name = formValues["TeamName"].Replace("+", " ");
                team.GroupNumber = formValues["GroupNumber"];
                team.TeamLeaderId = Convert.ToInt32(formValues["TeamLeaderId"]);
                TeamRepo.Update(team);
                TeamRepo.Save();
                return team;
            }
            return new Team();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var repos = new TeamRepository();
            repos.Delete(id);
            repos.Save();
        }
    }
}