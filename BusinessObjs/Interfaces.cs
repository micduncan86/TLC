using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLC.Data;

namespace TLC.Data
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T FindBy(object id);
        void Add(T Entity);
        void Update(T Entity);
        void Delete(object id);
        void Save();

    }

    public interface ICheckUpRepository : IRepository<CheckUp> { }
    public interface IEventRepository : IRepository<Event> { }
    public interface IMemberRepository : IRepository<Member> { }
    public interface IUserRepository : IRepository<User>
    {
        User Authenticate(string userName, string password);
    }
    public interface ITeamRepository : IRepository<Team> { }

    public interface ITokenRepository : IRepository<Token>
    {

        Token GenerateToken(int userId);
        bool ValidateToken(string tokenId);
        bool Kill(string tokenId);
        bool DeleteByUserId(int userId);

    }


}
