using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLC.Data
{
    public partial class Reports
    {
        public enum rptNames
        {
            None,
            TeamRoster,
            TeamList,
            TeamEvents,
            TeamCheckUps,
            MemberFollowUp
        }

        public static List<Report> GetReports()
        {
            List<Report> ReporList = new List<Report>();
            ReporList.Add(new Report() { Name = "Team Roster", ReportFile = "RptTeamRoster" });
            ReporList.Add(new Report() { Name = "Team Events", ReportFile = "RptTeamEvents" });
            ReporList.Add(new Report() { Name = "Team List", ReportFile = "RptTeamList" });
            ReporList.Add(new Report() { Name = "Team Check Ups", ReportFile = "RptTeamCheckUps" });
            ReporList.Add(new Report() { Name = "Member Follow Up", ReportFile = "RptMemberFollowUp" });

            return ReporList;
        }

        public static object GetData(rptNames Report)
        {
            DataContext db = new DataContext();
            switch (Report)
            {
                case rptNames.MemberFollowUp:  
                case rptNames.TeamCheckUps:
                    var teamCheckups = (from team in db.Teams
                                        join usr in db.Users on team.TeamLeaderId equals usr.UserId
                                        join mem in db.TeamMembers on team.TeamId equals mem.TeamId
                                        join chk in db.CheckUps on mem.MemberId equals chk.TeamMemberId
                                        select new
                                        {
                                            TeamName = team.TeamName,
                                            TeamLeader = usr.UserName,
                                            FirstName = mem.FirstName,
                                            LastName = mem.LastName,
                                            CheckUpDate = chk.CheckUpDate,
                                            Method = chk.Method,
                                            Outcome = chk.Outcome,
                                            RequiresAction = chk.RequiresAction ? "Y" : "N"
                                        });
                    //TODO: c.CheckUpDate >= EOMonth(getdate() - 60)
                    if (Report == rptNames.MemberFollowUp)
                    {
                        var RequiredCheckups = teamCheckups.Where(x => x.RequiresAction == "Y").ToList();
                        return RequiredCheckups;
                    }
                    return teamCheckups.ToList();
                    break;
                case rptNames.TeamRoster:                         
                    var roster = (from team in db.Teams
                                   join usr in db.Users on team.TeamLeaderId equals usr.UserId
                                   join mem in db.TeamMembers on team.TeamId equals mem.TeamId
                                   orderby team.TeamName, mem.LastName
                                   select new
                                   {
                                       TeamName = team.TeamName,
                                       TeamLeader = usr.UserName,
                                       FirstName = mem.FirstName,
                                       LastName = mem.LastName,
                                       Address = mem.Address,
                                       City = mem.City,
                                       State = mem.State,
                                       ZipCode = mem.ZipCode,
                                       Phone = mem.Phone,
                                       Email = mem.Email
                                   }).ToList();
                                //TODO: pass in TeamId
                    return roster;
                    break;
                case rptNames.TeamList:
                    var teamList = (from team in db.Teams
                               join usr in db.Users on team.TeamLeaderId equals usr.UserId
                               orderby team.TeamName
                               select new
                               {
                                   TeamName = team.TeamName,
                                   Name = usr.UserName,
                                   Email = usr.Email
                               }).ToList();

                    return teamList;
                    break;
                case rptNames.TeamEvents:
                    var teamEvents = (from team in db.Teams
                                      join usr in db.Users on team.TeamLeaderId equals usr.UserId
                                      join evnt in db.Events on team.TeamId equals evnt.TeamId
                                      select new
                                      {
                                          TeamName = team.TeamName,
                                          TeamLeader = usr.UserName,
                                          EventDate = evnt.EventDate,
                                          EventDescription = evnt.Description,
                                          EventCompleted = evnt.Completed ? "Y" : "N",
                                      }).ToList();
                    //TODO: pass TeamId, allow data parameter
                    return teamEvents;
                    break;
            }
            return null;
        }
    }


    public partial class Report
    {
        public string Name { get; set; }
        public string ReportFile { get; set; }

        public Reports.rptNames ReportType
        {
            get
            {
                switch (this.Name)
                {
                    case "Team Roster":
                        return Reports.rptNames.TeamRoster;
                        break;
                    case "Team Events":
                        return Reports.rptNames.TeamEvents;
                        break;
                    case "Team List":
                        return Reports.rptNames.TeamList;
                    case "Team Check Ups":
                        return Reports.rptNames.TeamCheckUps;
                    case "Member Follow Up":
                        return Reports.rptNames.MemberFollowUp;
                        break;
                    default:
                        return Reports.rptNames.None;
                }
            }
        }

    }
}
