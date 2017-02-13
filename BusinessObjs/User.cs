using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLC.Data
{
    public class User : BaseEntity
    {
        protected UserRepository Users = new UserRepository();

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //public bool isEmailConfirmed { get; set; }

        public string Name { get; set; }
        //public string PasswordHashCode { get; set; }
        //public string Role { get; set; }


        public User() : this(string.Empty) { }

        public User(string email)
        {
            if (email == string.Empty)
            {
                //Email = string.Empty;
                //PasswordHashCode = string.Empty;
                //Role = string.Empty;
                //isEmailConfirmed = false;
                Name = string.Empty;
                Username = string.Empty;
                Password = string.Empty;
                return;
            }
            //LoadUserData(Users.FindByUserName(email));

        }
        public User(int userid)
        {
            LoadUserData(Users.FindBy(userid));
        }
        private void LoadUserData(User userobj)
        {
            UserId = userobj.UserId;
            Username = userobj.Name;
            Name = userobj.Name;
            Password = userobj.Password;
            AddedById = userobj.AddedById;
            AddedDate = userobj.AddedDate;
        }
    }
}
