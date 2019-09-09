using System.Threading.Tasks;
using Token.Models;

namespace Token.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task Save(User user);
        Task<User> Load(string id);
        Task Delete(string id);
    }
}
