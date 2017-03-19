using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TLC.Data
{
    public class Member : BaseEntity
    {
        #region Properties
        public int MemberId { get; set; }
        public int TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Notes { get; set; }

        [NotMapped]
        public string LatestCheckUpDate
        {
            get
            {
                return this.CheckUps.Count > 0 ? this.CheckUps[0].CheckUpDate.ToShortDateString() : "";
            }
            private set { }
        }
        [NotMapped]
        public string LatestCheckUpOutCome
        {
            get
            {
                return this.CheckUps.Count > 0 ? this.CheckUps[0].Outcome : "";
            }
            private set { }
        }
        [NotMapped]
        public List<CheckUp> CheckUps
        {
            get
            {
                return new CheckUpRepository().GetCheckUpsByMemberId(this.MemberId);
            }
            private set { }
        }

        [NotMapped]
        public Team Team
        {
            get
            {
                if (this.TeamId > 0)
                {
                    return new TeamRepository().FindBy(this.TeamId);
                }
                else
                {
                    return new Team("Not Assigned", "");
                }

            }
            private set { }
        }

        #endregion

        #region Constructors
        public Member() : this(string.Empty, string.Empty, string.Empty)
        {

        }
        public Member(string firstname, string lastname, string phone)
        {
            FirstName = firstname;
            LastName = lastname;
            Phone = phone;
            Notes = string.Empty;
            SetAddress(string.Empty, string.Empty, string.Empty, string.Empty);
        }
        #endregion

        #region Methods
        public void SetAddress(string address, string city, string state, string zipcode)
        {
            Address = address;
            City = city;
            State = state;
            ZipCode = zipcode;
        }

        public bool Copy()
        {
            if (MemberId > 0)
            {
                Member copyMember = new Member
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Phone = Phone,
                    Email = Email,
                    Address = Address,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    Notes = Notes,
                    TeamId = -1
                };
                var tmRepo = new MemberRepository();
                tmRepo.Add(copyMember);
                tmRepo.Save();
                return true;
            }
            return false;
        }
        #endregion

    }
}
