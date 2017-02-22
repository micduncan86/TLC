using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
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

        public DataContext() : base("DataDb") { }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Member> TeamMembers { get; set; }
        public DbSet<Token> Tokens { get; set; }
        //public DbSet<CheckUp> CheckUps { get; set; }
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
            var hashLookUP = returnHash(password);
            User myuser = (from user in _dbSet
                       where user.Email.Equals(email) && user.PasswordHashCode.Equals(hashLookUP)
                       select user).FirstOrDefault();
            if (myuser != null && myuser.UserId > 0)
            {
               
                return myuser; 
            }
            return null;
        }
        private string returnHash(string password)
        {
            var encodeHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            return Convert.ToBase64String(Encoding.Default.GetBytes(encodeHash));
        }
               
        public bool AddUser(string email, string password)
        {
            var nUser = new User();
            nUser.Email = email;
            nUser.Role = "User";
            nUser.AddedById = -1;
            nUser.AddedDate = DateTime.Now;
            nUser.UserName = email;
            nUser.PasswordHashCode = returnHash(password);
            nUser.MyTeamId = -1;
            _dbSet.Add(nUser);
            _context.SaveChanges();
            return false;
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
            return (from checkups in _dbSet
                    where checkups.TeamMemberId == teamMemberId
                    select checkups).ToList();
        }
        public List<CheckUp> GetCheckUpsByTeamId(int teamId)
        {
            return (from checkups in _dbSet
                    where checkups.TeamId == teamId
                    select checkups).ToList();
        }
    }

   
}
