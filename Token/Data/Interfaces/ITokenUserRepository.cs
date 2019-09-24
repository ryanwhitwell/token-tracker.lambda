using System.Threading.Tasks;
using Token.Models;

namespace Token.Data.Interfaces
{
  public interface ITokenUserRepository
  {
    Task Save(TokenUser tokenUser);
    Task<TokenUser> Load(string id);
    Task Delete(string id);
  }
}
