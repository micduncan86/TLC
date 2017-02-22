using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace TLC.Data
{
    
    public class User : BaseEntity
    {

        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PasswordHashCode { get; set; }
        public string Role { get; set; }
        public int MyTeamId { get; set; }

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
    }
}
