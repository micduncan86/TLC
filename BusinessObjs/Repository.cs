using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace TLC.Data
{
    public class TeamMemberRepository : RepositoryBase<TeamMember>, ITeamMemberRepository {

        public List<TeamMember> GetMembersByTeamId(int teamid)
        {
            return (from members in _dbSet
                    where members.TeamId == teamid
                    select members).ToList();
        }

    }
    public interface ITeamMemberRepository : IRepository<TeamMember> { }

    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
    }
    public interface ITeamRepository : IRepository<Team> { }

    public class UserRepository : RepositoryBase<User>, IUserRepository {
        
        public User FindByUserName(string email)
        {
            return (from users in _dbSet
                    where users.Email == email
                    select users).FirstOrDefault();
        }

    }
    public interface IUserRepository : IRepository<User> { }

    public class EventRepository : RepositoryBase<Event>, IEventRepository {

        public List<Event> GetEventsByTeamId(int teamId)
        {
            return (from events in _dbSet
                    where events.TeamId == teamId
                    select events).ToList();
        }
    }
    public interface IEventRepository : IRepository<Event> { }

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
    public interface ICheckUpRepository : IRepository<CheckUp> { }

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T FindBy(object id);
        void Add(T Entity);
        void Update(T Entity);
        void Delete(object id);
        void Save();

    }
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
        public DataContext() : base("DataDb") { }
       // public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        //public DbSet<CheckUp> CheckUps { get; set; }
    }
}
