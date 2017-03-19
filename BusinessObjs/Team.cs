using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace TLC.Data
{
    public class Team : BaseEntity
    {
        private int _teamdleaderId;
        #region Properties
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int TeamLeaderId {
            get { return _teamdleaderId; }
            set {
                ChangeLeader(value);
            }
        }
        public int CoTeamLeaderId { get; set; }
        public string TeamNumber { get; set; }

        [NotMapped]
        public List<Event> Events {
            get
            {
                return new EventRepository().GetEventsByTeamId(TeamId);
            }

            private set { } }


        [NotMapped]
        public User TeamLeader
        {
            get {
                var lst = new UserRepository().GetAll().Where(x => x.MyTeamId == TeamId).FirstOrDefault();           
                return Equals(lst, null) ? new User() { UserId = -1, Email = "", UserName = "Not Assigned" } : lst;
            }
            private set { }
        }

        [NotMapped]
        public User CoTeamLeader
        {
            get
            {
                return CoTeamLeaderId <= 0 ? new User() { UserId = -1, Email = "", UserName = "Not Assigned" } : new UserRepository().FindBy(CoTeamLeaderId);
            }
            private set { }
        }

        [NotMapped]
        public List<Member> Members
        {

            get
            {
                return new MemberRepository().GetMembersByTeamId(TeamId);
            }
            private set { }
        }

        #endregion

        #region Constructors
        public Team() : this(string.Empty, string.Empty)
        {

        }

        public Team(string name, string groupnumber)
        {
            TeamName = name;
            TeamNumber = groupnumber;
            TeamLeader = new User();
            Members = new List<Member>();
            Events = new List<Event>();
        }

        private void ChangeLeader(int newLeaderId)
        {
            var provider = new UserRepository();
            var tmRepo = new TeamRepository();
            if (this._teamdleaderId != newLeaderId)
            {
                var oldLeader = provider.FindBy(this._teamdleaderId);
                if (oldLeader != null)
                {
                    if (oldLeader.MyTeamId != this.TeamId)
                    {
                        var oldTeam = tmRepo.FindBy(oldLeader.MyTeamId);
                        if (oldTeam != null)
                        {
                            oldTeam.TeamLeaderId = -1;
                            tmRepo.Update(oldTeam);
                            tmRepo.Save();
                        }

                    }
                    oldLeader.MyTeamId = -1;
                    provider.Update(oldLeader);
                    provider.Save();
                }
            }
            this._teamdleaderId = newLeaderId;
            var newLeader = provider.FindBy(newLeaderId);
            if (newLeader != null)
            {
                if (newLeader.MyTeamId != this.TeamId)
                {
                    var oldTeam = tmRepo.FindBy(newLeader.MyTeamId);
                    if (oldTeam != null)
                    {
                        oldTeam.TeamLeaderId = -1;
                        tmRepo.Update(oldTeam);
                        tmRepo.Save();
                    }
                }
                newLeader.MyTeamId = this.TeamId;
                provider.Update(newLeader);
                provider.Save();
            }
        }
        #endregion
    }
}
