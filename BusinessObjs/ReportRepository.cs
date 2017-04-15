using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLC.Data
{
    public partial class ReportRepository : RepositoryBase<Report>, IReportRepository
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

        public static rptNames ReportType(string Name)
        {
            switch (Name)
            {
                case "Team Roster":
                    return ReportRepository.rptNames.TeamRoster;
                case "Team Events":
                    return ReportRepository.rptNames.TeamEvents;
                case "Team List":
                    return ReportRepository.rptNames.TeamList;
                case "Team Check Ups":
                    return ReportRepository.rptNames.TeamCheckUps;
                case "Member Follow Ups":
                case "Member Follow Up":
                    return ReportRepository.rptNames.MemberFollowUp;
                default:
                    return ReportRepository.rptNames.None;
            }
        }
        public Report GetReportByName(string ReportName)
        {
            return (from reports in _dbSet
                    where reports.Name == ReportName
                    select reports).FirstOrDefault();
        }
        public static object GetData(rptNames Report, ReportParameters rptParams = null)
        {
            DataContext db = new DataContext();
            switch (Report)
            {
                case rptNames.MemberFollowUp:
                case rptNames.TeamCheckUps:
                    var teamCheckupsQuery = db.Teams.AsQueryable();
                    if (rptParams != null && rptParams.TeamId > 0)
                    {
                        teamCheckupsQuery = teamCheckupsQuery.Where(t => t.TeamId == rptParams.TeamId);
                    }
                    var teamCheckups = (from team in teamCheckupsQuery
                                        join usr in db.Users on team.TeamLeaderId equals usr.UserId
                                        join mem in db.TeamMembers on team.TeamId equals mem.TeamId
                                        join chk in db.CheckUps on mem.MemberId equals chk.TeamMemberId
                                        select new
                                        {
                                            TeamId = team.TeamId,
                                            TeamName = team.TeamName,
                                            TeamLeader = usr.UserName,
                                            MemberId = mem.MemberId,
                                            FirstName = mem.FirstName,
                                            LastName = mem.LastName,
                                            CheckUpDate = chk.CheckUpDate,
                                            Method = chk.Method,
                                            Outcome = chk.Outcome,
                                            RequiresAction = chk.RequiresAction ? "Y" : "N"
                                        }).AsQueryable(); 
                    if (rptParams != null && rptParams.FromDate != DateTime.MinValue)
                    {
                        teamCheckups = teamCheckups.Where(c => c.CheckUpDate >= rptParams.FromDate);
                    }
                    if (rptParams != null && rptParams.EndDate != DateTime.MaxValue)
                    {
                        teamCheckups = teamCheckups.Where(c => c.CheckUpDate <= rptParams.EndDate);
                    }


                    if (Report == rptNames.MemberFollowUp)
                    {
                        var RequiredCheckups = teamCheckups.Where(x => x.RequiresAction == "Y");
                        if (rptParams != null && rptParams.MemberId > 0)
                        {
                            RequiredCheckups = RequiredCheckups.Where(t => t.MemberId == rptParams.MemberId);
                        }
                        return RequiredCheckups.ToList();
                    }
                    return teamCheckups.ToList();
                case rptNames.TeamRoster:
                    var rosterQuery = db.Teams.AsQueryable();

                    if (rptParams != null && rptParams.TeamId > 0)
                    {
                        rosterQuery = rosterQuery.Where(t => t.TeamId == rptParams.TeamId);
                    }
                    var roster = (from team in rosterQuery
                                  from usr in db.Users.Where(u => u.UserId == team.TeamLeaderId).DefaultIfEmpty()
                                  from mem in db.TeamMembers.Where(m => m.TeamId == team.TeamId).DefaultIfEmpty()
                                  orderby team.TeamName, mem.LastName
                                  select new
                                  {
                                      TeamId = team.TeamId,
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
                                  });

                    return roster.ToList();

                case rptNames.TeamList:
                    var teamList = (from team in db.Teams
                                    from usr in db.Users.Where(u => u.UserId == team.TeamLeaderId).DefaultIfEmpty()
                                    orderby team.TeamName
                                    select new
                                    {
                                        TeamId = team.TeamId,
                                        TeamName = team.TeamName,
                                        Name = usr.UserName,
                                        Email = usr.Email
                                    });

                    return teamList.ToList();

                case rptNames.TeamEvents:
                    var teamEventsQuery = db.Teams.AsQueryable();
                    if (rptParams != null && rptParams.TeamId > 0)
                    {
                        teamEventsQuery = teamEventsQuery.Where(t => t.TeamId == rptParams.TeamId);
                    }
                    var teamEvents = (from team in teamEventsQuery
                                      join usr in db.Users on team.TeamLeaderId equals usr.UserId
                                      join evnt in db.Events on team.TeamId equals evnt.TeamId
                                      select new
                                      {
                                          TeamId = team.TeamId,
                                          TeamName = team.TeamName,
                                          TeamLeader = usr.UserName,
                                          EventDate = evnt.EventDate,
                                          EventDescription = evnt.Description,
                                          EventCompleted = evnt.Completed ? "Y" : "N",
                                      }).AsQueryable();

                    if (rptParams != null && rptParams.FromDate != DateTime.MinValue)
                    {
                        teamEvents = teamEvents.Where(e => e.EventDate >= rptParams.FromDate);
                    }
                    if (rptParams != null && rptParams.EndDate != DateTime.MaxValue)
                    {
                        teamEvents = teamEvents.Where(e => e.EventDate <= rptParams.EndDate);
                    }
                    return teamEvents.ToList();
            }
            return null;
        }
    }
}
