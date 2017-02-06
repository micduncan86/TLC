using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TLC.Data
{
    [Table("Teams")]
    public class Team : BaseEntity
    {
        #region Properties
        public int TeamId { get; set; }
        public string Name { get; set; }
        public int TeamLeaderId { get; set; }

        [NotMapped]
        public List<Event> Events {
            get
            {
                return new EventRepository().GetEventsByTeamId(TeamId);
            }

            private set { } }


        [NotMapped]
        public TeamMember TeamLeader
        {
            get {
                return new TeamMemberRepository().FindBy(TeamLeaderId);
            }
            private set { }
        }
        public string GroupNumber { get; set; }


        [NotMapped]
        public List<TeamMember> Members
        {

            get
            {
                return new TeamMemberRepository().GetMembersByTeamId(TeamId);
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
            Name = name;
            GroupNumber = groupnumber;
            TeamLeader = new TeamMember();
            Members = new List<TeamMember>();
            Events = new List<Event>();
        }
        #endregion
    }
}
