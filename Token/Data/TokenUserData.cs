using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Token.Core;
using Token.Data.Interfaces;
using Token.Models;

namespace Token.Data
{
  public class TokenUserData : ITokenUserData
  {
    public static readonly DateTime EPOCH_DATE = new DateTime(1970, 1, 1);
    private static readonly int TTL_MINUTES = Int32.Parse(Configuration.File.GetSection("Application")["DataTimeToLiveMinutes"]);
    private ILogger<TokenUserData> _logger;
    private ITokenUserRepository _tokenUserRepository;

    public TokenUserData(ILogger<TokenUserData> logger, ITokenUserRepository tokenUserRepository)
    {
      _logger = logger ?? throw new ArgumentNullException("logger");
      _tokenUserRepository = tokenUserRepository ?? throw new ArgumentNullException("tokenUserRepository"); ;
    }

    public async Task<bool> Exists(string id)
    {
      if (String.IsNullOrWhiteSpace(id))
      {
        throw new ArgumentNullException("id");
      }

      TokenUser tokenUser = await _tokenUserRepository.Load(id);

      return tokenUser.Id == id;
    }

    public async Task<TokenUser> Get(string id)
    {
      if (String.IsNullOrWhiteSpace(id))
      {
        throw new ArgumentNullException("id");
      }

      TokenUser tokenUser = await _tokenUserRepository.Load(id);

      return tokenUser;
    }

    public async Task Save(TokenUser tokenUser)
    {
      if (tokenUser == null)
      {
        throw new ArgumentNullException("tokenUser");
      }

      DateTime utcNow = DateTime.UtcNow;

      tokenUser.CreateDate = tokenUser.CreateDate ?? utcNow;
      tokenUser.UpdateDate = utcNow;

      // Remove or add expiration date as needed
      if (tokenUser.HasPointsPersistence)
      {
        tokenUser.ExpirationDate = (DateTime?)null;
      }
      else if (!tokenUser.HasPointsPersistence && !tokenUser.ExpirationDate.HasValue)
      {
        tokenUser.ExpirationDate = tokenUser.CreateDate.Value.AddMinutes(TTL_MINUTES);
      }

      // Delete the TokenUser if it is expired otherwise save it
      if (DateTime.UtcNow > tokenUser.ExpirationDate)
      {
        _logger.LogInformation("Token User is expired. Deleting Token User. User Id: {0}, Created Date: {1}, Expiration Date: {2}.", tokenUser.Id, tokenUser.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"), tokenUser.ExpirationDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        await _tokenUserRepository.Delete(tokenUser.Id);
      }
      else
      {
        await _tokenUserRepository.Save(tokenUser);
      }
      
    }
  }
}