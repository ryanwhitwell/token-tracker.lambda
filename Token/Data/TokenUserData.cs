using System;
using System.Collections.Generic;
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
    private static readonly int TTL_MINUTES = Int32.Parse(Configuration.File.GetSection("Application")["UserDataExpirationMinutes"]);
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

      DateTime now = DateTime.UtcNow;

      // Don't let the user retrieve player data if the account is expired and they don't have subscription
      if (tokenUser != null && !tokenUser.HasPointsPersistence && (now >= tokenUser.ExpirationDate.Value))
      {
        _logger.LogInformation("Discovered expired user data while loading. Removing players from token user. User Id: {0}, UTC Now: {1}, Expiration Date: {2}.", tokenUser.Id, now, tokenUser.ExpirationDate.Value);
        tokenUser.Players = new List<Player>();
      }

      return tokenUser;
    }

    public async Task Delete(string id)
    {
      if (String.IsNullOrWhiteSpace(id))
      {
        throw new ArgumentNullException("id");
      }

      await _tokenUserRepository.Delete(id);
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

      // Remove or set expiration date
      if (tokenUser.HasPointsPersistence)
      {
        tokenUser.ExpirationDate = (DateTime?)null;
      }
      else if (!tokenUser.HasPointsPersistence && !tokenUser.ExpirationDate.HasValue)
      {
        tokenUser.ExpirationDate = tokenUser.CreateDate.Value.AddMinutes(TTL_MINUTES);
      }

      // Don't let the user save player data if they don't have subscription and the account is expired
      if (!tokenUser.HasPointsPersistence || utcNow >= tokenUser.ExpirationDate.Value)
      {
        _logger.LogInformation("Discovered expired user data while saving. Removing players from token user. User Id: {0}, UTC Now: {1}, Expiration Date: {2}.", tokenUser.Id, utcNow, tokenUser.ExpirationDate.Value);
        tokenUser.Players = new List<Player>();
      }

      await _tokenUserRepository.Save(tokenUser);
    }
  }
}