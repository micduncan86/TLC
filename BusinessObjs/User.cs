using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TLC.Data
{

    public class User : BaseEntity
    {
        public enum enumRole
        {
            Administrater,
            User
        }      
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; private set; }
        public string PasswordHashCode { get; set; }
        public int MyTeamId { get; set; }

        [NotMapped]
        public enumRole UserRole
        {
            get
            {
                return Role == "User" ? enumRole.User : enumRole.Administrater;
            }
            set
            {
                this.Role = value == enumRole.User ? "User" : "Administrater";
            }
        }

        [NotMapped]
        public Team MyTeam
        {
            get
            {
                return this.MyTeamId == -1 ? new Team() : new TeamRepository().FindBy(this.MyTeamId);
            }
        }

        public User() : this(string.Empty) { }

        public User(string email)
        {
            if (email == string.Empty)
            {
                Email = string.Empty;
                PasswordHashCode = string.Empty;
                Role = string.Empty;
                UserName = string.Empty;
                MyTeamId = -1;
                return;
            }

        }
        public string GetRole()
        {
            return Role;
        }
    }
}
