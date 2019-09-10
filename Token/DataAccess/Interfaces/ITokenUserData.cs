using System.Threading.Tasks;
using Token.Models;

namespace Token.DataAccess.Interfaces
{
    public interface ITokenUserData
    {
        Task Save(TokenUser tokenUser);
        Task<TokenUser> Get(string id);
        Task Delete(string id);
        Task<bool> Exists(string id);
    }
}
