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
    private static readonly int TTL_MINUTES = Int32.Parse(Configuration.File.GetSection("Application")["DataTimeToLiveMinutes"]);
    private ILogger<TokenUserData> _logger;
    private ITokenUserRepository _tokenUserRepository;

    public TokenUserData(ILogger<TokenUserData> logger, ITokenUserRepository tokenUserRepository)
    {
      _logger = logger ?? throw new ArgumentNullException("logger");
      _tokenUserRepository = tokenUserRepository ?? throw new ArgumentNullException("tokenUserRepository"); ;
    }

    public async Task Delete(string id)
    {
      if (String.IsNullOrWhiteSpace(id))
      {
        throw new ArgumentNullException("id");
      }

      await _tokenUserRepository.Delete(id);
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

      // If TTL has passed then force delete the TokenUser and return null
      if (tokenUser != null && tokenUser.TTL != null)
      {
        long calculatedTtl = GetTTL(tokenUser.CreateDate.Value);
        bool isExpired = calculatedTtl >= tokenUser.TTL.Value;

        if (isExpired)
        {
          _logger.LogInformation("Deleting expired User. User Id: {0}, Calculated TTL: {1}, Stored TTL: {2}.", tokenUser.Id, calculatedTtl, tokenUser.TTL.Value);
          await _tokenUserRepository.Delete(id);
          tokenUser = null;
        }
      }

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

      tokenUser.TTL = tokenUser.HasPointsPersistence ? (long?)null : TokenUserData.GetTTL(tokenUser.CreateDate);

      await _tokenUserRepository.Save(tokenUser);
    }

    public static long GetTTL(DateTime? dateTime)
    {
      if (dateTime == null || !dateTime.HasValue)
      {
        throw new ArgumentNullException("dateTime");
      }

      return (long)(dateTime.Value.AddMinutes(TTL_MINUTES) - Token.Core.Extensions.EPOCH_DATE).TotalSeconds;
    }
  }
}