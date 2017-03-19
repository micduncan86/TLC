using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TLC.Data
{
    public class Team : BaseEntity
    {
        #region Properties
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int TeamLeaderId { get; set; }
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
                return TeamLeaderId <= 0 ? new User() { UserId = -1, Email = "", UserName = "Not Assigned" } : new UserRepository().FindBy(TeamLeaderId);
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
        #endregion
    }
}
