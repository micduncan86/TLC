using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Security;

namespace TLC.Data
{

    public abstract class RepositoryBase<T> where T : class
    {
        protected DataContext _context;
        protected DbSet<T> _dbSet;

        public RepositoryBase()
        {
            this._context = new DataContext();
            _dbSet = _context.Set<T>();
        }
        public RepositoryBase(DataContext _dataContext)
        {
            this._context = _dataContext;
            _dbSet = _dataContext.Set<T>();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public T FindBy(object id)
        {
            return _dbSet.Find(id);
        }
        public virtual void Add(T obj)
        {
            _dbSet.Add(obj);
        }
        public virtual void Update(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public virtual void Delete(object id)
        {
            T existing = _dbSet.Find(id);
            _dbSet.Remove(existing);
        }
        public virtual void Save()
        {
            _context.SaveChanges();
        }


    }

    public class DataContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public override int SaveChanges()
        {
            AddTimeStampsUser();
            return base.SaveChanges();
        }

        public DataContext() : base("DataDb") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Member> TeamMembers { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<CheckUp> CheckUps { get; set; }

        protected void AddTimeStampsUser()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State ==EntityState.Added || x.State == EntityState.Modified));
            var currentUser = !Equals(System.Web.HttpContext.Current?.User?.Identity,null) ? System.Web.HttpContext.Current.User.Identity : new System.Web.Security.FormsIdentity(new FormsAuthenticationTicket("Anonymous",false,0));
            var loginId = -1;
            if (currentUser is FormsIdentity)
            {
                FormsIdentity id = (FormsIdentity)currentUser;
                FormsAuthenticationTicket tkt = id.Ticket;
                if (tkt.UserData.Contains(":"))
                {
                    loginId = Convert.ToInt32(tkt.UserData.Split(':')[0]);
                }
            }

            foreach (var entity in entities)
            {
                
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).AddedDate = DateTime.Now;
                    ((BaseEntity)entity.Entity).AddedById = loginId;                    
                }
                if (((BaseEntity)entity.Entity).AddedById == -1)
                {
                    ((BaseEntity)entity.Entity).AddedById = loginId;
                }
                ((BaseEntity)entity.Entity).ModifiedDate = DateTime.Now;
                ((BaseEntity)entity.Entity).ModifiedBy = currentUser.Name;

                //FUTURE DEV: Add AuditLogging Async
            }
        }
    }
    public class MemberRepository : RepositoryBase<Member>, IMemberRepository {

        public List<Member> GetMembersByTeamId(int teamid)
        {
            return (from members in _dbSet
                    where members.TeamId == teamid
                    select members).ToList();
        }

    }


    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
    }

    public class TokenRepository : RepositoryBase<Token>, ITokenRepository
    {
        public Token GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;

            //ConfigurationManager.AppSettings["AuthTokenExpiry"]
            DateTime expiredOn = DateTime.Now.AddSeconds(
                                              Convert.ToDouble(60 * 3));
            var tokendomain = new Token
            {
                UserId = userId,
                AuthToken = token,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn
            };

            var tokenRepo = new TokenRepository();

            tokenRepo.Add(tokendomain);
            tokenRepo.Save();
            var tokenModel = new Token()
            {
                UserId = userId,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn,
                AuthToken = token
            };

            return tokenModel;
        }

        public bool ValidateToken(string tokenId)
        {
            var tokenRepo = new TokenRepository();
            var token = (from tokens in tokenRepo.GetAll()
                         where tokens.AuthToken == tokenId && tokens.ExpiresOn > DateTime.Now
                         select tokens).FirstOrDefault();
            if (token != null && !(DateTime.Now > token.ExpiresOn))
            {
                //ConfigurationManager.AppSettings["AuthTokenExpiry"]
                token.ExpiresOn = token.ExpiresOn.AddSeconds(
                                              Convert.ToDouble(60 * 3));
                tokenRepo.Update(token);
                tokenRepo.Save();
                return true;
            }
            return false;
        }

        public bool Kill(string tokenId)
        {
            var tokenRepo = new TokenRepository();
            var t = tokenRepo.GetAll().Where(x => x.AuthToken == tokenId).Select(s => s.TokenId);
            tokenRepo.Delete(t);
            tokenRepo.Save();
            var isNotDeleted = tokenRepo.GetAll().Select(x => x.AuthToken == tokenId).Any();
            if (isNotDeleted) { return false; }
            return true;
        }
        public bool DeleteByUserId(int userId)
        {
            var tokenRepo = new TokenRepository();
            tokenRepo.Delete(tokenRepo.GetAll().Where(x => x.UserId == userId).Select(x => x.TokenId));
            tokenRepo.Save();

            var isNotDeleted = tokenRepo.GetAll().Select(x => x.UserId == userId).Any();
            return !isNotDeleted;
        }
    }


    public class UserRepository : RepositoryBase<User>, IUserRepository {      
        //protected string algo = System.Configuration.ConfigurationManager.AppSettings["userAlgorithm"].ToString();
        public User Authenticate(string email, string password)
        {
            return _dbSet.SqlQuery("ValidateLogin @pLogin, @pPassword", new SqlParameter("@pLogin", email), new SqlParameter("@pPassword",password)).FirstOrDefault();
        }
        public int ChangePassword(int userId,string email,string oldPassword,string newPassword)
        {
            var status = new SqlParameter("@Status", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
           _context.Database.ExecuteSqlCommand("exec @Status = PwdChange @pUserId, @pEmail, @pOldPassword, @pNewPassword",
               status
                , new SqlParameter("@pUserId", userId)
                , new SqlParameter("@pEmail", email)
                , new SqlParameter("@pOldPassword", oldPassword)
                , new SqlParameter("@pNewPassword", newPassword)
           );
            return (int)status.Value;
        }
        public int ChangePassword(int userId, string email, string newPassword)
        {
            string cmd = "DECLARE @Changed int = 0";
            //cmd += " IF EXISTS(select UserId from [User] WHERE UserId = @UserId AND Email = @Email)";
            //cmd += " BEGIN";
            cmd = " UPDATE [User] SET PasswordHashCode = HASHBYTES('SHA2_512', @NewPassword+CAST(Salt as nvarchar(36)))";
            cmd += " WHERE UserId = @UserId AND Email = @Email";
           // cmd += " SET @Changed = 1";
           // cmd += "END";
           // cmd += "SELECT @Changed";

            var rtn = _context.Database.ExecuteSqlCommand(cmd
                , new SqlParameter("@UserId", userId)
                , new SqlParameter("@Email", email)
                , new SqlParameter("@NewPassword", newPassword)
                );
            return rtn;
        }
        private string returnHash(string password)
        {
            var encodeHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            return Convert.ToBase64String(Encoding.Default.GetBytes(encodeHash));
        }
               
        public User AddUser(string email, string password)
        {
            var user = this._dbSet.SqlQuery("AddUser @pEmail, @pPassword, @pUserName", 
                new SqlParameter("@pEmail", email), 
                new SqlParameter("@pPassword", password),
                new SqlParameter("@pUserName", email)
                ).FirstOrDefault();

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return user;
        }

        public static string ReturnUserRole(User.enumRole enumRole)
        {
            return enumRole == User.enumRole.User ? "User" : "Administrater";
        }

    }


    public class EventRepository : RepositoryBase<Event>, IEventRepository {

        public List<Event> GetEventsByTeamId(int teamId)
        {
            return (from events in _dbSet
                    where events.TeamId == teamId
                    select events).ToList();
        }
    }


    public class CheckUpRepository : RepositoryBase<CheckUp>, ICheckUpRepository {

        public List<CheckUp> GetCheckUpsByMemberId(int teamMemberId)
        {
            return _dbSet.Where(x => x.TeamMemberId == teamMemberId).OrderByDescending(o => o.CheckUpDate).ToList();
           
        }
        public List<CheckUp> GetCheckUpsByTeamId(int teamId)
        {
            return (from checkups in _dbSet
                    where checkups.TeamId == teamId
                    select checkups).ToList();
        }
    }

   
}
