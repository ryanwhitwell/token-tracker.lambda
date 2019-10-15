using System.Threading.Tasks;

namespace Token.BusinessLogic.Interfaces
{
  public interface IUserProfileClient
  {
    Task<string> GetUserId(string accessToken);
  }
}